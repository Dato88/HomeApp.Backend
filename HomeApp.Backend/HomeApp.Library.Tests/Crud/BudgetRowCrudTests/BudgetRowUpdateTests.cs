namespace HomeApp.Library.Tests.Crud.BudgetRowCrudTests;

public class BudgetRowUpdateTests : BaseBudgetRowTest
{
    public BudgetRowUpdateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task UpdateAsync_UpdatesBudgetRowInContext()
    {
        // Arrange
        BudgetRow budgetRow = new() { Index = 1, Name = "Test Budget Row", Year = 2021 };

        DbContext.BudgetRows.Add(budgetRow);
        await DbContext.SaveChangesAsync();

        BudgetRow updateBudgetRow = new() { Id = budgetRow.Id, Index = 2, Name = "Updated Budget Row", Year = 2022 };

        // Act
        await _budgetRowCrud.UpdateAsync(updateBudgetRow, default);

        // Assert
        budgetRow.Id.Should().Be(1);
        budgetRow.Index.Should().Be(updateBudgetRow.Index);
        budgetRow.Name.Should().Be(updateBudgetRow.Name);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenBudgetRowIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _budgetRowCrud.UpdateAsync(null, default));

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenBudgetYearIsNullOrEmpty()
    {
        // Arrange
        BudgetRow budgetRow = new() { Index = 1, Name = "Test Budget Row", Year = 2021 };

        DbContext.BudgetRows.Add(budgetRow);

        await DbContext.SaveChangesAsync();

        BudgetRow budgetRowUpdate = new() { Id = budgetRow.Id, Name = "Test Budget Row Update" };

        // Act & Assert
        var action = async () => await _budgetRowCrud.UpdateAsync(budgetRowUpdate, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"Year ('{budgetRowUpdate.Year}') must be a non-negative and non-zero value. (Parameter 'Year')Actual value was {budgetRowUpdate.Year}.");
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenExistingBudgetRowIsNull()
    {
        BudgetRow budgetRow = new() { Index = 1, Name = "Test Budget Row", Year = 2021 };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _budgetRowCrud.UpdateAsync(budgetRow, default));
    }

    [Fact]
    public async Task UpdateAsync_CallsAllValidations_Once()
    {
        // Arrange
        BudgetRow budgetRow = new() { Index = 1, Name = "Test Budget Row", Year = 2021 };

        DbContext.BudgetRows.Add(budgetRow);

        await DbContext.SaveChangesAsync();

        // Act
        await _budgetRowCrud.UpdateAsync(budgetRow, default);

        // Assert
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetRowForUserIdChangeAsync(It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetRowIdExistsNotAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _budgetValidationMock.Verify(x => x.ValidateForEmptyStringAsync(It.IsAny<string>()), Times.Once);
        _budgetValidationMock.Verify(x => x.ValidateForPositiveIndexAsync(It.IsAny<int>()), Times.Once);
    }
}
