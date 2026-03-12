using MediatR;

namespace Application.AuthUser
{
    public sealed record RegisterUserCommand(
    string UserName,
    string Email,
    string Password
    ) : IRequest<int>;
}
