namespace HomeApp.Library.Tests.Crud.UserCrudTests
{
    public class BaseUserTest : BaseTest
    {
        protected readonly UserCrud _userCrud;

        protected readonly Mock<IUserValidation> _userValidationMock;

        public BaseUserTest()
        {
            _userValidationMock = new Mock<IUserValidation>();
            _userCrud = new(_context, _userValidationMock.Object);
        }
    }
}
