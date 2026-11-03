using BottleTrack.API.Iam.Application.Errors;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Commands;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Iam.Application.Services;

public interface IUserCommandService
{
    Task<Result<User, UserError>> Handle(RegisterUserCommand command, CancellationToken cancellationToken = default);
}
