using MediatR;

namespace Application.Commands.UserRegistration
{
    public sealed record RegisterUserCommand(
    string UserName,
    string Email,
    string Password
    ) : IRequest<int>;
}
