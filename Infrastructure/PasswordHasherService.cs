using Application.Commands.UserRegistration;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        public string HashPassword(string password)
        {
            var user = new User();
            return _passwordHasher.HashPassword(user, password);
        }
    }
}
