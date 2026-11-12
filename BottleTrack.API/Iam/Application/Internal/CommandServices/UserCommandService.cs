using BottleTrack.API.Iam.Application.Errors;
using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Commands;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using BottleTrack.API.Iam.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Iam.Application.Internal.CommandServices;

public class UserCommandService : IUserCommandService
{
    private const int MinimumPasswordLength = 8;

    private readonly IUserRepository userRepository;
    private readonly IOrganizationRepository organizationRepository;
    private readonly IPasswordHasher passwordHasher;
    private readonly IUnitOfWork unitOfWork;

    public UserCommandService(IUserRepository userRepository, IOrganizationRepository organizationRepository,
        IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        this.userRepository = userRepository;
        this.organizationRepository = organizationRepository;
        this.passwordHasher = passwordHasher;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<User, UserError>> Handle(RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
            return Result<User, UserError>.Failure(UserError.InvalidUserData);

        if (command.Password == null || command.Password.Length < MinimumPasswordLength)
            return Result<User, UserError>.Failure(UserError.PasswordTooShort);

        EmailAddress email;
        try
        {
            email = new EmailAddress(command.Email);
        }
        catch (ArgumentException)
        {
            return Result<User, UserError>.Failure(UserError.InvalidEmail);
        }

        Organization? organization = await organizationRepository.FindByIdAsync(command.OrganizationId, cancellationToken);
        if (organization == null)
            return Result<User, UserError>.Failure(UserError.OrganizationNotFound);

        User? existingUser = await userRepository.FindByEmailAsync(email, cancellationToken);
        if (existingUser != null)
            return Result<User, UserError>.Failure(UserError.EmailAlreadyRegistered);

        string passwordHash = passwordHasher.Hash(command.Password);
        User user = User.Register(command.OrganizationId, command.FirstName, command.LastName, email, passwordHash,
            command.Role);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<User, UserError>.Success(user);
    }

    public async Task<Result<User, UserError>> Handle(UpdateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
            return Result<User, UserError>.Failure(UserError.InvalidUserData);

        User? user = await userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user == null)
            return Result<User, UserError>.Failure(UserError.UserNotFound);

        EmailAddress email;
        try
        {
            email = new EmailAddress(command.Email);
        }
        catch (ArgumentException)
        {
            return Result<User, UserError>.Failure(UserError.InvalidEmail);
        }

        User? existingUser = await userRepository.FindByEmailAsync(email, cancellationToken);
        if (existingUser != null && existingUser.Id != command.UserId)
            return Result<User, UserError>.Failure(UserError.EmailAlreadyRegistered);

        user.UpdateProfile(command.FirstName, command.LastName, email);

        userRepository.Update(user);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<User, UserError>.Success(user);
    }

    public async Task<Result<User, UserError>> Handle(AssignUserRoleCommand command,
        CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user == null)
            return Result<User, UserError>.Failure(UserError.UserNotFound);

        user.AssignRole(command.Role);

        userRepository.Update(user);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<User, UserError>.Success(user);
    }
}
