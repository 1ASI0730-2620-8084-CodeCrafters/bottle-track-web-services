using BottleTrack.API.Iam.Domain.Model.Aggregates;

namespace BottleTrack.API.Iam.Application.Services;

public interface ITokenService
{
    string Generate(User user);
}
