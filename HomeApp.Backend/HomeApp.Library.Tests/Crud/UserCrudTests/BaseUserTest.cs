namespace HomeApp.Library.Tests.Crud.UserCrudTests
{
    public class BaseUserTest : BaseTest
    {
        protected readonly PersonCrud PersonCrud;

        protected readonly Mock<IUserValidation> _userValidationMock;

        public BaseUserTest()
        {
            _userValidationMock = new Mock<IUserValidation>();
            PersonCrud = new(_context, _userValidationMock.Object);
        }
    }
}