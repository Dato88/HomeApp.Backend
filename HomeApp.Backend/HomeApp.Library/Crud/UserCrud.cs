namespace HomeApp.Library.Crud
{
    public class UserCrud(HomeAppContext context, IUserValidation userValidation) : BaseCrud<User>(context, null)
    {
        private readonly IUserValidation _userValidation = userValidation;

        public override async Task CreateAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            _userValidation.ValidateRequiredProperties(user);
            _userValidation.ValidateMaxLength(user);
            await _userValidation.ValidateUsernameDoesNotExistAsync(user.Username);
            _userValidation.ValidateEmailFormat(user.Email);
            _userValidation.ValidatePasswordStrength(user.Password);
            _userValidation.ValidateLastLoginDate(user.LastLogin);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int userId)
        {
            User? user = await _context.Users.FindAsync(userId);

            if (user == null)
                throw new InvalidOperationException(UserMessage.UserNotFound);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public override async Task<User> FindByIdAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new InvalidOperationException(UserMessage.UserNotFound);

            return user;
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public override async Task UpdateAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            _userValidation.ValidateRequiredProperties(user);
            _userValidation.ValidateMaxLength(user);
            _userValidation.ValidateEmailFormat(user.Email);
            _userValidation.ValidatePasswordStrength(user.Password);
            _userValidation.ValidateLastLoginDate(user.LastLogin);

            User? existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser == null)
                throw new InvalidOperationException(UserMessage.UserNotFound);

            if (user.Username != existingUser.Username)
                await _userValidation.ValidateUsernameDoesNotExistAsync(user.Username);

            existingUser.Username = user.Username;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Password = user.Password;
            existingUser.Email = user.Email;
            existingUser.LastLogin = user.LastLogin;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
        }
    }
}
