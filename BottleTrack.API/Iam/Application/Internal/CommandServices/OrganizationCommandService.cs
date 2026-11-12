using BottleTrack.API.Iam.Application.Errors;
using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Commands;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using BottleTrack.API.Iam.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Iam.Application.Internal.CommandServices;

public class OrganizationCommandService : IOrganizationCommandService
{
    private readonly IOrganizationRepository organizationRepository;
    private readonly IUnitOfWork unitOfWork;

    public OrganizationCommandService(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        this.organizationRepository = organizationRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Organization, OrganizationError>> Handle(RegisterOrganizationCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.BusinessName))
            return Result<Organization, OrganizationError>.Failure(OrganizationError.BusinessNameRequired);

        TaxId taxId;
        try
        {
            taxId = new TaxId(command.TaxId);
        }
        catch (ArgumentException)
        {
            return Result<Organization, OrganizationError>.Failure(OrganizationError.InvalidTaxId);
        }

        EmailAddress email;
        try
        {
            email = new EmailAddress(command.Email);
        }
        catch (ArgumentException)
        {
            return Result<Organization, OrganizationError>.Failure(OrganizationError.InvalidEmail);
        }

        Organization? existingOrganization = await organizationRepository.FindByTaxIdAsync(taxId, cancellationToken);
        if (existingOrganization != null)
            return Result<Organization, OrganizationError>.Failure(OrganizationError.TaxIdAlreadyRegistered);

        Organization organization = Organization.Register(command.BusinessName, taxId, email);
        organization.UpdateProfile(command.BusinessName, command.Phone, command.Address);

        await organizationRepository.AddAsync(organization, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Organization, OrganizationError>.Success(organization);
    }

    public async Task<Result<Organization, OrganizationError>> Handle(UpdateOrganizationCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.BusinessName))
            return Result<Organization, OrganizationError>.Failure(OrganizationError.BusinessNameRequired);

        Organization? organization = await organizationRepository.FindByIdAsync(command.OrganizationId,
            cancellationToken);
        if (organization == null)
            return Result<Organization, OrganizationError>.Failure(OrganizationError.OrganizationNotFound);

        organization.UpdateProfile(command.BusinessName, command.Phone, command.Address);

        organizationRepository.Update(organization);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Organization, OrganizationError>.Success(organization);
    }
}
