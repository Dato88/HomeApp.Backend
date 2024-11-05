namespace HomeApp.Library.Cruds
{
    public class PersonCrud(HomeAppContext context, IUserValidation userValidation) : BaseCrud<Person>(context, null)
    {
        private readonly IUserValidation _userValidation = userValidation;

        public override async Task CreateAsync(Person person, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(person);

            _userValidation.ValidateRequiredProperties(person);
            _userValidation.ValidateMaxLength(person);
            await _userValidation.ValidateUsernameDoesNotExistAsync(person.Username, cancellationToken);
            _userValidation.ValidateEmailFormat(person.Email);
            _userValidation.ValidatePasswordStrength(person.Password);
            _userValidation.ValidateLastLoginDate(person.LastLogin);

            _context.Users.Add(person);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public override async Task DeleteAsync(int userId, CancellationToken cancellationToken)
        {
            Person? user = await _context.Users.FindAsync(userId, cancellationToken);

            if (user == null)
                throw new InvalidOperationException(UserMessage.UserNotFound);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public override async Task<Person> FindByIdAsync(int id, CancellationToken cancellationToken)
        {
            Person? user = await _context.Users.FindAsync(id, cancellationToken);

            if (user == null)
                throw new InvalidOperationException(UserMessage.UserNotFound);

            return user;
        }

        public override async Task<IEnumerable<Person>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.ToListAsync(cancellationToken);
        }

        public override async Task UpdateAsync(Person person, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(person);

            _userValidation.ValidateRequiredProperties(person);
            _userValidation.ValidateMaxLength(person);
            _userValidation.ValidateEmailFormat(person.Email);
            _userValidation.ValidatePasswordStrength(person.Password);
            _userValidation.ValidateLastLoginDate(person.LastLogin);

            Person? existingUser = await _context.Users.FindAsync(person.Id, cancellationToken);

            if (existingUser == null)
                throw new InvalidOperationException(UserMessage.UserNotFound);

            if (person.Username != existingUser.Username)
                await _userValidation.ValidateUsernameDoesNotExistAsync(person.Username, cancellationToken);

            existingUser.Username = person.Username;
            existingUser.FirstName = person.FirstName;
            existingUser.LastName = person.LastName;
            existingUser.Password = person.Password;
            existingUser.Email = person.Email;
            existingUser.LastLogin = person.LastLogin;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}