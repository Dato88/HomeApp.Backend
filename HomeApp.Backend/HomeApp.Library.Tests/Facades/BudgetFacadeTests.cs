namespace HomeApp.Library.Tests.Facades
{
    public class BudgetFacadeTests : BaseBudgetFacadeTest
    {
        [Fact]
        public async Task GetBudgetAsync_CallsAllValidations_Once()
        {
            // Arrange
            int selectedUserId = 1;

            // Act
            await _budgetFacade.GetBudgetAsync(selectedUserId);

            // Assert
            _budgetCellCrudMock.Verify(x => x.GetAllAsync(It.IsAny<int>()), Times.Once);
            _budgetColumnCrudMock.Verify(x => x.GetAllAsync(), Times.Once);
            _budgetGroupCrudMock.Verify(x => x.GetAllAsync(It.IsAny<int>()), Times.Once);
            _budgetRowCrudMock.Verify(x => x.GetAllAsync(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(33)]
        [InlineData(1234)]
        public async Task GetBudgetAsync_CallsAllValidations_Once_WithSelectedUserId(int userId)
        {
            // Arrange
            int selectedUserId = userId;

            // Act
            await _budgetFacade.GetBudgetAsync(selectedUserId);

            // Assert
            _budgetCellCrudMock.Verify(x => x.GetAllAsync(selectedUserId), Times.Once);
            _budgetColumnCrudMock.Verify(x => x.GetAllAsync(), Times.Once);
            _budgetGroupCrudMock.Verify(x => x.GetAllAsync(selectedUserId), Times.Once);
            _budgetRowCrudMock.Verify(x => x.GetAllAsync(selectedUserId), Times.Once);
        }
    }
}
