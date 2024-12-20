namespace HomeApp.Library;

public static class BudgetMessage
{
    public const string BudgetCellId = "BudgetCell Id";
    public const string CellNotFound = "BudgetCell not found";

    public const string BudgetRowId = "BudgetRow Id";
    public const string RowNotFound = "BudgetRow not found";
    public const string InvalidRowId = "Invalid BudgetRowId";
    public const string RowIdExists = "BudgetRowId does exist already";
    public const string RowIdNotExist = "BudgetRowId does not exist";

    public const string BudgetColumnId = "BudgetColumn Id";
    public const string ColumnNotFound = "BudgetColumn not found";
    public const string InvalidColumnId = "Invalid BudgetColumnId";
    public const string ColumnIdExist = "BudgetColumnId does exist already";
    public const string ColumnIdNotExist = "BudgetColumnId does not exist";
    public const string ColumnIndexAlreadyExists = "A BudgetColumn with the same Index and Name already exists";

    public const string BudgetGroupId = "BudgetGroup Id";
    public const string GroupNotFound = "BudgetGroup not found";
    public const string InvalidGroupId = "Invalid BudgetGroupId";
    public const string GroupIdExist = "BudgetGroupId does exist already";
    public const string GroupIdNotExist = "BudgetGroupId does not exist";
    public const string GroupIndexAlreadyExists = "A BudgetGroup with the same Index and Name already exists";

    public const string UserChangeNotAllowed = "User Change is not allowed";
    public const string NameIsNullOrEmpty = "Name cannot be empty";
    public const string IndexMustBePositive = "Index must be a positive number";
}

public static class PersonMessage
{
    public const string PersonId = "Person Id";
    public const string PersonIdZero = "PersonId is zero";
    public const string PersonNotFound = "Person not found";
    public const string PersonAlreadyExists = "Personname already exists";
    public const string InvalidEmail = "Invalid email format";
    public const string PasswordShort = "Password must be at least 8 characters long";
    public const string PasswordUppercaseMissing = "Password must contain at least one uppercase letter";
    public const string PasswordLowercaseMissing = "Password must contain at least one lowercase letter";
    public const string PasswordDigitMissing = "Password must contain at least one digit";
    public const string PasswordSpecialCharMissing = "Password must contain at least one special character";
    public const string PropertiesMissing = "Person Required properties are missing";
    public const string WeakPassword = "Password is too weak";
    public const string MaxLengthExeed = "Max length exceeded";
}
