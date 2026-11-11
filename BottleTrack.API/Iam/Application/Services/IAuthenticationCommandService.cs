using BottleTrack.API.Iam.Application.Errors;
using BottleTrack.API.Iam.Domain.Model.Commands;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Iam.Application.Services;

public interface IAuthenticationCommandService
{
    Task<Result<AuthenticatedUser, UserError>> Handle(SignInCommand command,
        CancellationToken cancellationToken = default);
}
