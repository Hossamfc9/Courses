using Application.Common;
using Application.Common.Exceptions;
using Courses.Models.Enums;
using Domain.Models; // Ensure this points to where AuditRecord lives
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InvalidDataException = Application.Common.Exceptions.InvalidDataException;

namespace Application.Features.Enrollments.Commands.ApproveEnrollment;

public class ApproveEnrollmentCommandHandler(
    IContext _context,
    ILogger<ApproveEnrollmentCommandHandler> _logger)
    : IRequestHandler<ApproveEnrollmentCommand, RequestResult<ApproveEnrollmentDto>>
{
    public async Task<RequestResult<ApproveEnrollmentDto>> Handle(ApproveEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.Id == request.EnrollmentId, cancellationToken);

        if (enrollment == null)
        {
            throw new NotFoundException(request.EnrollmentId, "Enrollment");
        }

        if (enrollment.Status != EnrollmentStatus.PendingApproval)
        {
            _logger.LogWarning("Attempted to make a decision on Enrollment {EnrollmentId} which is currently {Status}", 
                enrollment.Id, enrollment.Status);
                
            throw new InvalidDataException(request.EnrollmentId, "Enrollment");
        }

        var oldStatus = enrollment.Status.ToString();
        
        enrollment.Status = request.Decision == "Approved" 
            ? EnrollmentStatus.Approved 
            : EnrollmentStatus.Rejected;
            
        enrollment.CreatedAt = DateTime.UtcNow;
        enrollment.DecisionReason = request.Decision; 

        var audit = new EnrollmentDecision()
        {
            Id = Guid.NewGuid(),
            EntityName = nameof(Enrollment),
            EntityId = enrollment.Id.ToString(),
            Action = "Decision Made",
            OldValue = oldStatus,
            NewValue = enrollment.Status.ToString(),
            PerformedBy = enrollment.LearnerId.ToString(), 
            CreatedAt = DateTime.UtcNow
        };

        _context.EnrollmentDecisions.Add(audit);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Audit: {EntityName} {EntityId} was updated. Action: {Action}. Status changed from {OldValue} to {NewValue} by Learner {PerformedBy}. Reason: {Reason}",
            nameof(Enrollment), 
            enrollment.Id, 
            "Decision Made", 
            oldStatus, 
            enrollment.Status.ToString(), 
            enrollment.LearnerId,
            enrollment.DecisionReason);

        return RequestResult<ApproveEnrollmentDto>.Success(new ApproveEnrollmentDto());
    }
}
