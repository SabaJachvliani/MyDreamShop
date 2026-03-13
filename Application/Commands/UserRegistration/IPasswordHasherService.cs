namespace Application.Commands.UserRegistration
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
    }
}
