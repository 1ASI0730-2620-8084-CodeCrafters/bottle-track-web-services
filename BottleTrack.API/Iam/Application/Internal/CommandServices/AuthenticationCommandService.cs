using BottleTrack.API.Iam.Application.Errors;
using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Commands;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using BottleTrack.API.Iam.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Iam.Application.Internal.CommandServices;

public class AuthenticationCommandService : IAuthenticationCommandService
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordHasher passwordHasher;
    private readonly ITokenService tokenService;

    public AuthenticationCommandService(IUserRepository userRepository, IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        this.userRepository = userRepository;
        this.passwordHasher = passwordHasher;
        this.tokenService = tokenService;
    }

    public async Task<Result<AuthenticatedUser, UserError>> Handle(SignInCommand command,
        CancellationToken cancellationToken = default)
    {
        EmailAddress email;
        try
        {
            email = new EmailAddress(command.Email);
        }
        catch (ArgumentException)
        {
            return Result<AuthenticatedUser, UserError>.Failure(UserError.InvalidCredentials);
        }

        User? user = await userRepository.FindByEmailAsync(email, cancellationToken);
        if (user == null)
            return Result<AuthenticatedUser, UserError>.Failure(UserError.InvalidCredentials);

        if (!passwordHasher.Verify(command.Password, user.PasswordHash))
            return Result<AuthenticatedUser, UserError>.Failure(UserError.InvalidCredentials);

        string token = tokenService.Generate(user);
        return Result<AuthenticatedUser, UserError>.Success(new AuthenticatedUser(user, token));
    }
}
