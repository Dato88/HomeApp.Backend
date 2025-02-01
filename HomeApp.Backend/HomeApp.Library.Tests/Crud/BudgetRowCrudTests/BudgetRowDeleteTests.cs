namespace HomeApp.Library.Tests.Crud.BudgetRowCrudTests;

public class BudgetRowDeleteTests : BaseBudgetRowTest
{
    // public BudgetRowDeleteTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }
    //
    // [Fact]
    // public async Task DeleteAsync_RemovesBudgetRowFromContext()
    // {
    //     // Arrange
    //     BudgetRow budgetRow = new() { Index = 1, Name = "Test Budget Row" };
    //
    //     DbContext.BudgetRows.Add(budgetRow);
    //     await DbContext.SaveChangesAsync();
    //
    //     // Act
    //     await _budgetRowCrud.DeleteAsync(budgetRow.Id, default);
    //
    //     // Assert
    //     DbContext.BudgetRows.Should().NotContain(budgetRow);
    // }
    //
    // [Theory]
    // [InlineData(0)]
    // [InlineData(-3)]
    // public async Task DeleteAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    // {
    //     // Act & Assert
    //     var action = async () => await _budgetRowCrud.DeleteAsync(id, default);
    //
    //     await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
    //         .WithMessage(
    //             $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    // }
    //
    // [Fact]
    // public async Task DeleteAsync_ThrowsException_WhenBudgetRowNotFound()
    // {
    //     // Act
    //     var action = async () => await _budgetRowCrud.DeleteAsync(999, default);
    //
    //     // Assert
    //     await action.Should().ThrowAsync<InvalidOperationException>()
    //         .WithMessage(BudgetMessage.RowNotFound);
    // }
}
