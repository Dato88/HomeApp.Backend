﻿using HomeApp.DataAccess.Tests.Helper;
using HomeApp.DataAccess.Tests.Helper.CreateDummyData;
using HomeApp.DataAccess.Validations;

namespace HomeApp.DataAccess.Tests.ValidationTests;

public class BudgetValidationTests : BaseTest
{
    private readonly BudgetValidation _budgetValidation;
    private readonly CreateDummyPeople _createDummyPeople;

    public BudgetValidationTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        _budgetValidation = new BudgetValidation(DbContext);
        _createDummyPeople = new CreateDummyPeople(unitTestingApiFactory);
    }

    // [Fact]
    // public async Task ValidateBudgetCellForUserIdChangeAsync_ValidUserId_And_ValidCellId_NoExceptionThrown()
    // {
    //     // Arrange
    //     var userId = 1;
    //     var budgetCellId = 1;
    //
    //     // Act & Assert
    //     var action = async () =>
    //         await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(userId, budgetCellId, default);
    //
    //     await action.Should().NotThrowAsync<ArgumentOutOfRangeException>(PersonMessage.PersonId);
    //     await action.Should().NotThrowAsync<ArgumentOutOfRangeException>(BudgetMessage.BudgetCellId);
    //     await action.Should().NotThrowAsync<InvalidOperationException>(BudgetMessage.UserChangeNotAllowed);
    // }

    [Theory]
    [InlineData(0)]
    [InlineData(-99)]
    public async Task ValidateBudgetCellForUserIdChangeAsync_ZeroOrNegativeUserId_And_ValidCellId_NoExceptionThrown(
        int selectedUserId)
    {
        // Arrange
        var userId = selectedUserId;
        var budgetCellId = 1;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(userId, budgetCellId, default);

        var output =
            $"{PersonMessage.PersonId} ('{userId}') must be a non-negative and non-zero value. (Parameter '{PersonMessage.PersonId}')Actual value was {userId}.";
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("UserId is null or negative")
            .WithMessage(output);

        await action.Should().NotThrowAsync<InvalidOperationException>("BudgetCellId user has not changed");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-99)]
    public async Task ValidateBudgetCellForUserIdChangeAsync_ValidUserId_And_ZeroOrNegativeCellId_NoExceptionThrown(
        int selectedCellId)
    {
        // Arrange
        var userId = 1;
        var budgetCellId = selectedCellId;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(userId, budgetCellId, default);

        var output =
            $"{BudgetMessage.BudgetCellId} ('{budgetCellId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetCellId}')Actual value was {budgetCellId}.";
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("BudgetCellId is null or negative")
            .WithMessage(output);

        await action.Should().NotThrowAsync<InvalidOperationException>("BudgetCellId user has not changed");
    }

    [Fact]
    public async Task ValidateBudgetCellForUserIdChangeAsync_InValidNewUserId_And_ValidCellId_NoExceptionThrown()
    {
        // Arrange
        var person = await _createDummyPeople.CreateOneDummyPerson();

        BudgetColumn budgetColumn = new() { Index = 1, Name = "Test" };
        DbContext.BudgetColumns.Add(budgetColumn);
        await DbContext.SaveChangesAsync();

        BudgetGroup budgetGroup = new() { PersonId = person.Id, Index = 1, Name = "Test" };
        DbContext.BudgetGroups.Add(budgetGroup);
        await DbContext.SaveChangesAsync();

        BudgetRow budgetRow = new() { PersonId = person.Id, Index = 1, Name = "Test", Year = DateTime.Today.Year };
        DbContext.BudgetRows.Add(budgetRow);
        await DbContext.SaveChangesAsync();

        BudgetCell budgetCell = new()
        {
            BudgetColumnId = budgetColumn.Id,
            BudgetGroupId = budgetGroup.Id,
            BudgetRowId = budgetRow.Id,
            PersonId = person.Id,
            Name = "Cell Name"
        };

        DbContext.BudgetCells.Add(budgetCell);
        await DbContext.SaveChangesAsync();

        var newUserId = 99;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(newUserId, budgetCell.Id, default);

        await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("BudgetColumnId does exist already");

        await action.Should().ThrowAsync<InvalidOperationException>("Invalid BudgetColumnId")
            .WithMessage(BudgetMessage.UserChangeNotAllowed);
    }

    // [Fact]
    // public async Task ValidateBudgetColumnIdExistsAsync_ValidColumnId_NoExceptionThrown()
    // {
    //     // Arrange
    //     var budgetColumnId = 1;
    //
    //     // Act & Assert
    //     var action = async () =>
    //         await _budgetValidation.ValidateBudgetColumnIdExistsAsync(budgetColumnId, default);
    //
    //     await action.Should().NotThrowAsync<ArgumentException>("BudgetColumnId is Valid");
    //     await action.Should().NotThrowAsync<InvalidOperationException>(BudgetMessage.ColumnIdExist);
    // }

    [Fact]
    public async Task ValidateBudgetColumnIdExistsAsync_ValidColumnId_InvalidOperationExceptionThrown()
    {
        // Arrange
        var budgetColumnId = 1;

        DbContext.BudgetColumns.Add(new BudgetColumn { Index = 1, Name = "Test" });
        DbContext.SaveChanges();

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetColumnIdExistsAsync(budgetColumnId, default);

        await action.Should().ThrowAsync<InvalidOperationException>("BudgetColumnId already Exists")
            .WithMessage(BudgetMessage.ColumnIdExist);
    }

    [Fact]
    public async Task ValidateBudgetColumnIdExistsNotAsync_ValidColumnId_NoExceptionThrown()
    {
        // Arrange
        var budgetColumnId = 1;

        DbContext.BudgetColumns.Add(new BudgetColumn { Index = 1, Name = "Test" });
        DbContext.SaveChanges();

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetColumnIdExistsNotAsync(budgetColumnId, default);

        await action.Should().NotThrowAsync<ArgumentException>("BudgetColumnId is Valid");
        await action.Should().NotThrowAsync<InvalidOperationException>("BudgetColumnId does not exists");
    }

    [Fact]
    public async Task ValidateBudgetColumnIdExistsNotAsync_InvalidGroupId_ArgumentExceptionThrown()
    {
        // Arrange
        var budgetColumnId = 0;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetColumnIdExistsNotAsync(budgetColumnId, default);

        var output =
            $"{BudgetMessage.BudgetColumnId} ('{budgetColumnId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetColumnId}')Actual value was {budgetColumnId}.";

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Invalid BudgetColumnId").WithMessage(output);
    }

    [Fact]
    public async Task ValidateBudgetColumnIdExistsNotAsync_ValidColumnId_InvalidOperationExceptionThrown()
    {
        // Arrange
        var budgetColumnId = 1;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetColumnIdExistsNotAsync(budgetColumnId, default);

        await action.Should().ThrowAsync<InvalidOperationException>("BudgetColumnId does not Exist")
            .WithMessage(BudgetMessage.ColumnIdNotExist);
    }

    [Fact]
    public async Task
        ValidateBudgetColumnIndexAndNameAlreadyExistsAsync_DuplicateIndexAndName_ThrowsInvalidOperationException()
    {
        // Arrange
        var budgetIndex = 1;
        var budgetName = "January";

        DbContext.BudgetColumns.Add(new BudgetColumn { Index = budgetIndex, Name = budgetName });
        DbContext.SaveChanges();

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetColumnIndexAndNameExistsAsync(budgetIndex, budgetName, default);

        await action.Should().ThrowAsync<InvalidOperationException>("BudgetColumn Index and Name already Exist")
            .WithMessage(BudgetMessage.ColumnIndexAlreadyExists);
    }

    [Fact]
    public async Task ValidateBudgetColumnIndexAndNameAlreadyExistsAsync_UniqueIndexAndName_NoExceptionThrown()
    {
        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetColumnIndexAndNameExistsAsync(1, "Unique Column", default);
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ValidateBudgetGroupForUserIdChangeAsync_ValidUserId_And_ValidGroupId_NoExceptionThrown()
    {
        // Arrange
        var userId = 1;
        var budgetGroupId = 1;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetGroupForUserIdChangeAsync(userId, budgetGroupId, default);

        await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("BudgetGroupId is not zero or negative");
        await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-99)]
    public async Task
        ValidateBudgetGroupForUserIdChangeAsync_ZeroOrNegativeUserId_And_ValidGroupId_NoExceptionThrown(
            int selectedUserId)
    {
        // Arrange
        var userId = selectedUserId;
        var budgetGroupId = 1;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetGroupForUserIdChangeAsync(userId, budgetGroupId, default);

        var output =
            $"{PersonMessage.PersonId} ('{userId}') must be a non-negative and non-zero value. (Parameter '{PersonMessage.PersonId}')Actual value was {userId}.";
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("UserId is null or negative")
            .WithMessage(output);

        await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-99)]
    public async Task
        ValidateBudgetGroupForUserIdChangeAsync_ValidUserId_And_ZeroOrNegativeGroupId_NoExceptionThrown(
            int selectedGroupId)
    {
        // Arrange
        var userId = 1;
        var budgetGroupId = selectedGroupId;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetGroupForUserIdChangeAsync(userId, budgetGroupId, default);

        var output =
            $"{BudgetMessage.BudgetGroupId} ('{budgetGroupId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetGroupId}')Actual value was {budgetGroupId}.";
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("BudgetGroupId is null or negative")
            .WithMessage(output);

        await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
    }

    // [Fact]
    // public async Task ValidateBudgetGroupIdExistsAsync_ValidGroupId_NoExceptionThrown()
    // {
    //     // Arrange
    //     var budgetGroupId = 1;
    //
    //     // Act & Assert
    //     var action = async () =>
    //         await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetGroupId, default);
    //
    //     await action.Should().NotThrowAsync<ArgumentException>("BudgetGroupId is Valid");
    //     await action.Should().NotThrowAsync<InvalidOperationException>("BudgetGroupId exists");
    // }

    [Fact]
    public async Task ValidateBudgetGroupIdExistsAsync_InvalidGroupId_ArgumentExceptionThrown()
    {
        // Arrange
        var budgetGroupId = 0;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetGroupId, default);

        var output =
            $"{BudgetMessage.BudgetGroupId} ('{budgetGroupId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetGroupId}')Actual value was {budgetGroupId}.";

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Invalid BudgetGroupId").WithMessage(output);
    }

    [Fact]
    public async Task ValidateBudgetGroupIdExistsAsync_ValidGroupId_InvalidOperationExceptionThrown()
    {
        // Arrange
        var person = await _createDummyPeople.CreateOneDummyPerson();
        var budgetGroupId = 1;

        DbContext.BudgetGroups.Add(new BudgetGroup { Index = 1, PersonId = person.Id, Name = "Test" });
        DbContext.SaveChanges();

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetGroupId, default);

        await action.Should().ThrowAsync<InvalidOperationException>("BudgetGroupId does Exist already")
            .WithMessage(BudgetMessage.GroupIdExist);
    }

    [Fact]
    public async Task ValidateBudgetGroupIdExistsNotAsync_ValidGroupId_NoExceptionThrown()
    {
        // Arrange
        var budgetGroupId = 1;
        var person = await _createDummyPeople.CreateOneDummyPerson();

        DbContext.BudgetGroups.Add(new BudgetGroup { Index = 1, PersonId = person.Id, Name = "Test" });
        DbContext.SaveChanges();

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetGroupIdExistsNotAsync(budgetGroupId, default);

        await action.Should().NotThrowAsync<ArgumentException>("BudgetGroupId is Valid");
        await action.Should().NotThrowAsync<InvalidOperationException>("BudgetGroupId exists");
    }

    [Fact]
    public async Task ValidateBudgetGroupIdExistsNotAsync_InvalidGroupId_ArgumentExceptionThrown()
    {
        // Arrange
        var budgetGroupId = 0;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetGroupIdExistsNotAsync(budgetGroupId, default);

        var output =
            $"{BudgetMessage.BudgetGroupId} ('{budgetGroupId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetGroupId}')Actual value was {budgetGroupId}.";

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Invalid BudgetGroupId").WithMessage(output);
    }

    [Fact]
    public async Task ValidateBudgetGroupIdExistsNotAsync_GroupDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var budgetGroupId = 999;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetGroupIdExistsNotAsync(budgetGroupId, default);

        await action.Should().ThrowAsync<InvalidOperationException>("BudgetGroupId does not exist")
            .WithMessage(BudgetMessage.GroupIdNotExist);
    }

    [Fact]
    public async Task
        ValidateBudgetGroupIndexAndNameAlreadyExistsAsync_DuplicateIndexAndName_ThrowsInvalidOperationException()
    {
        // Arrange
        var person = await _createDummyPeople.CreateOneDummyPerson();

        var budgetGroup = new BudgetGroup { Index = 1, PersonId = person.Id, Name = "Duplicate Group" };
        DbContext.BudgetGroups.Add(budgetGroup);
        await DbContext.SaveChangesAsync();

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(budgetGroup.Index,
                budgetGroup.Name, default);
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(BudgetMessage.GroupIndexAlreadyExists);
    }

    [Fact]
    public async Task ValidateBudgetGroupIndexAndNameAlreadyExistsAsync_UniqueIndexAndName_NoExceptionThrown()
    {
        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(1, "Unique Group", default);
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ValidateBudgetRowForUserIdChangeAsync_ValidUserId_And_ValidRowId_NoExceptionThrown()
    {
        // Arrange
        var userId = 1;
        var budgetRowId = 1;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetRowForUserIdChangeAsync(userId, budgetRowId, default);

        await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("BudgetRowId is not zero or negative");
        await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-99)]
    public async Task ValidateBudgetRowForUserIdChangeAsync_ZeroOrNegativeUserId_And_ValidRowId_NoExceptionThrown(
        int selectedUserId)
    {
        // Arrange
        var userId = selectedUserId;
        var budgetRowId = 1;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetRowForUserIdChangeAsync(userId, budgetRowId, default);

        var output =
            $"{PersonMessage.PersonId} ('{userId}') must be a non-negative and non-zero value. (Parameter '{PersonMessage.PersonId}')Actual value was {userId}.";
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("UserId is null or negative")
            .WithMessage(output);

        await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-99)]
    public async Task ValidateBudgetRowForUserIdChangeAsync_ValidUserId_And_ZeroOrNegativeRowId_NoExceptionThrown(
        int selectedRowId)
    {
        // Arrange
        var userId = 1;
        var budgetRowId = selectedRowId;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetRowForUserIdChangeAsync(userId, budgetRowId, default);

        var output =
            $"{BudgetMessage.BudgetRowId} ('{budgetRowId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetRowId}')Actual value was {budgetRowId}.";
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("BudgetRowId is null or negative")
            .WithMessage(output);

        await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
    }

    [Fact]
    public async Task ValidateBudgetRowIdExistsAsync_ValidBudgetRowId_NoExceptionThrown()
    {
        // Arrange
        var budgetRowId = 1;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetRowId, default);

        await action.Should().NotThrowAsync<ArgumentException>("BudgetRowId is Valid");
        await action.Should().NotThrowAsync<InvalidOperationException>("BudgetRowId exists");
    }

    [Fact]
    public async Task ValidateBudgetRowIdExistsAsync_InvalidBudgetRowId_ArgumentExceptionThrown()
    {
        // Arrange
        var budgetRowId = 0;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetRowId, default);

        var output =
            $"{BudgetMessage.BudgetRowId} ('{budgetRowId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetRowId}')Actual value was {budgetRowId}.";

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Invalid BudgetRowId").WithMessage(output);
    }

    // [Fact]
    // public async Task ValidateBudgetRowIdExistsAsync_BudgetRowDoesExist_ThrowsInvalidOperationException()
    // {
    //     // Arrange
    //     var budgetRowId = 1;
    //
    //     DbContext.BudgetRows.Add(new BudgetRow { Index = 1, Name = "Test" });
    //     DbContext.SaveChanges();
    //
    //     // Act & Assert
    //     var action = async () =>
    //         await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetRowId, default);
    //
    //     await action.Should().ThrowAsync<InvalidOperationException>("BudgetRowId does Exist already")
    //         .WithMessage(BudgetMessage.RowIdExists);
    // }

    // [Fact]
    // public async Task ValidateBudgetRowIdExistsNotAsync_ValidBudgetRowId_NoExceptionThrown()
    // {
    //     // Arrange
    //     var budgetRowId = 1;
    //     var person = await _createDummyPeople.CreateOneDummyPerson();
    //
    //     DbContext.BudgetRows.Add(new BudgetRow { Index = 1, PersonId = person.Id, Name = "Test" });
    //     DbContext.SaveChanges();
    //
    //     // Act & Assert
    //     var action = async () =>
    //         await _budgetValidation.ValidateBudgetRowIdExistsNotAsync(budgetRowId, default);
    //
    //     await action.Should().NotThrowAsync<ArgumentException>("BudgetRowId is Valid");
    //     await action.Should().NotThrowAsync<InvalidOperationException>("BudgetRowId exists");
    // }

    [Theory]
    [InlineData(0)]
    [InlineData(-99)]
    public async Task ValidateBudgetRowIdExistsNotAsync_InvalidBudgetRowId_ArgumentExceptionThrown(
        int selectedRowId)
    {
        // Arrange
        var budgetRowId = selectedRowId;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetRowIdExistsNotAsync(budgetRowId, default);

        var output =
            $"{BudgetMessage.BudgetRowId} ('{budgetRowId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetRowId}')Actual value was {budgetRowId}.";

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Invalid BudgetRowId").WithMessage(output);
    }

    [Fact]
    public async Task ValidateBudgetRowIdExistsNotAsync_BudgetRowDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var budgetRowId = 999;

        // Act & Assert
        var action = async () =>
            await _budgetValidation.ValidateBudgetRowIdExistsNotAsync(budgetRowId, default);

        await action.Should().ThrowAsync<InvalidOperationException>("BudgetRowId does not exist")
            .WithMessage(BudgetMessage.RowIdNotExist);
    }

    [Fact]
    public async Task ValidateForEmptyString_NoExceptionThrown()
    {
        // Arrange
        var name = "TestName";

        // Act & Assert
        var action = async () => await _budgetValidation.ValidateForEmptyStringAsync(name);

        await action.Should().NotThrowAsync<ArgumentException>("Name is not null or empty");
    }

    [Fact]
    public async Task ValidateForEmptyString_EmptyString_ArgumentExceptionThrown()
    {
        // Arrange
        var name = string.Empty;

        // Act & Assert
        var action = async () => await _budgetValidation.ValidateForEmptyStringAsync(name);

        var output = "The value cannot be an empty string. (Parameter 'Budget Name')";

        await action.Should().ThrowAsync<ArgumentException>("Name is empty").WithMessage(output);
    }

    [Fact]
    public async Task ValidateForPositiveIndex_NoExceptionThrown()
    {
        // Arrange
        var index = 1;

        // Act & Assert
        var action = async () => await _budgetValidation.ValidateForPositiveIndexAsync(index);

        await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("Index is not negative");
    }

    [Fact]
    public async Task ValidateForPositiveIndex_ArgumentOutOfRangeExceptionThrown()
    {
        // Arrange
        var index = -99;

        // Act & Assert
        var action = async () => await _budgetValidation.ValidateForPositiveIndexAsync(index);

        var output =
            $"{BudgetMessage.IndexMustBePositive} ('{index}') must be a non-negative value. (Parameter 'Index must be a positive number')Actual value was {index}.";
        await action.Should().ThrowAsync<ArgumentException>("Index is negative").WithMessage(output);
    }

    [Fact]
    public async Task ValidateForUserIdAsync_ValidUserId_NoExceptionThrown()
    {
        // Arrange
        var userId = 1;

        Person person = new()
        {
            Username = "testuser",
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        DbContext.People.Add(person);

        DbContext.SaveChanges();

        // Act & Assert
        var action = async () => await _budgetValidation.ValidateForUserIdAsync(userId, default);

        await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("UserId is Valid");
        await action.Should().NotThrowAsync<InvalidOperationException>("UserId exists");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-99)]
    public async Task ValidateForUserIdAsync_NegativeUserIdOrZeroUserId_ArgumentOutOfRangeException(
        int selectedUserId)
    {
        // Arrange
        var userId = selectedUserId;

        // Act & Assert
        var action = async () => await _budgetValidation.ValidateForUserIdAsync(userId, default);

        var output =
            $"{PersonMessage.PersonId} ('{userId}') must be a non-negative and non-zero value. (Parameter '{PersonMessage.PersonId}')Actual value was {userId}.";

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Name is null").WithMessage(output);
    }
}
