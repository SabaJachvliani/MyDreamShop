using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.UserRegistration
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasherService _passwordHasher;

        public RegisterUserCommandHandler(
            IApplicationDbContext context,
            IPasswordHasherService passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userNameExists = await _context.Users
                .AnyAsync(x => x.UserName == request.UserName, cancellationToken);

            if (userNameExists)
                throw new Exception("Username already exists");

            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailExists)
                throw new Exception("Email already exists");

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
