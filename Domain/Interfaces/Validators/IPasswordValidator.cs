namespace Domain.Interfaces.Validators
{
    public interface IPasswordValidator
    {
        bool PasswordIsValid(string password, string confirmPassword);
    }
}
