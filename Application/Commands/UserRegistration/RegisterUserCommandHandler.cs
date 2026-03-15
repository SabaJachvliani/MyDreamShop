using Application.Common.Exeptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserRegistration
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IApplicationDbContext context,
            IPasswordHasherService passwordHasher,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Starting user registration. UserName: {UserName}, Email: {Email}",
                request.UserName,
                request.Email);

            var userNameExists = await _context.Users
                .AnyAsync(x => x.UserName == request.UserName, cancellationToken);

            if (userNameExists)
            {
                _logger.LogWarning(
                    "Registration failed. Username already exists: {UserName}",
                    request.UserName);

                throw new ConflictException("Username already exists.");
            }

            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                _logger.LogWarning(
                    "Registration failed. Email already exists: {Email}",
                    request.Email);

                throw new ConflictException("Email already exists.");
            }

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

            _logger.LogInformation(
                "User registered successfully. UserId: {UserId}, Email: {Email}",
                user.Id,
                user.Email);

            return user.Id;
        }
    }
}