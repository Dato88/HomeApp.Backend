namespace HomeApp.Library.Tests.Crud.BudgetRowCrudTests;

public class BudgetRowCreateTests : BaseBudgetRowTest
{
    public BudgetRowCreateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task CreateAsync_AddsBudgetRowToContext()
    {
        // Arrange
        BudgetRow budgetRow = new() { PersonId = 1, Index = 1, Name = "Test Budget Row", Year = 2021 };

        // Act
        await _budgetRowCrud.CreateAsync(budgetRow, default);

        // Assert
        Assert.Contains(budgetRow, DbContext.BudgetRows);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenBudgetRowIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _budgetRowCrud.CreateAsync(null, default));

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenBudgetYearIsNullOrEmpty()
    {
        // Arrange
        BudgetRow budgetRow = new() { PersonId = 1, Index = 1, Name = "Test Budget Row" };

        // Act & Assert
        Func<Task> action = async () => await _budgetRowCrud.CreateAsync(budgetRow, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"Year ('{budgetRow.Year}') must be a non-negative and non-zero value. (Parameter 'Year')Actual value was {budgetRow.Year}.");
    }

    [Fact]
    public async Task CreateAsync_CallsAllValidations_Once()
    {
        // Arrange
        BudgetRow budgetRow = new() { PersonId = 1, Index = 1, Name = "Test Budget Row", Year = 2021 };

        // Act
        await _budgetRowCrud.CreateAsync(budgetRow, default);

        // Assert
        _budgetValidationMock.Verify(x => x.ValidateForUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetRowIdExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _budgetValidationMock.Verify(x => x.ValidateForEmptyStringAsync(It.IsAny<string>()), Times.Once);
        _budgetValidationMock.Verify(x => x.ValidateForPositiveIndexAsync(It.IsAny<int>()), Times.Once);
    }
}
