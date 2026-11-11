using BottleTrack.API.Incidents.Application.Errors;
using BottleTrack.API.Incidents.Application.Services;
using BottleTrack.API.Incidents.Domain.Model.Aggregates;
using BottleTrack.API.Incidents.Domain.Model.Commands;
using BottleTrack.API.Incidents.Domain.Repositories;
using BottleTrack.API.Shared.Application.Patterns;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Incidents.Application.Internal.CommandServices;

public class IncidentCommandService : IIncidentCommandService
{
    private readonly IIncidentRepository incidentRepository;
    private readonly IUnitOfWork unitOfWork;

    public IncidentCommandService(IIncidentRepository incidentRepository, IUnitOfWork unitOfWork)
    {
        this.incidentRepository = incidentRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Incident, IncidentError>> Handle(ReportIncidentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Description))
            return Result<Incident, IncidentError>.Failure(IncidentError.InvalidIncidentData);

        int count = await incidentRepository.CountAsync(cancellationToken);
        string code = "INC-" + (count + 1).ToString("D5");

        Incident incident = Incident.Report(code, command.TransportOperationId, command.DeliveryStopId,
            command.ReportedBy, command.Source, command.Type, command.Severity, command.Description,
            command.AffectedQuantity);

        await incidentRepository.AddAsync(incident, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Incident, IncidentError>.Success(incident);
    }

    public async Task<Result<Incident, IncidentError>> Handle(AddEvidenceCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.FileUrl) || string.IsNullOrWhiteSpace(command.FileType))
            return Result<Incident, IncidentError>.Failure(IncidentError.InvalidIncidentData);

        Incident? incident = await incidentRepository.FindByIdWithEvidencesAsync(command.IncidentId, cancellationToken);
        if (incident == null)
            return Result<Incident, IncidentError>.Failure(IncidentError.IncidentNotFound);

        incident.AddEvidence(command.FileUrl, command.FileType, command.UploadedBy);

        incidentRepository.Update(incident);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Incident, IncidentError>.Success(incident);
    }

    public async Task<Result<Incident, IncidentError>> Handle(ResolveIncidentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Resolution))
            return Result<Incident, IncidentError>.Failure(IncidentError.InvalidIncidentData);

        Incident? incident = await incidentRepository.FindByIdWithEvidencesAsync(command.IncidentId, cancellationToken);
        if (incident == null)
            return Result<Incident, IncidentError>.Failure(IncidentError.IncidentNotFound);

        incident.Resolve(command.Resolution);

        incidentRepository.Update(incident);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Incident, IncidentError>.Success(incident);
    }
}
