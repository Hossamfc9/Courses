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
    ILogger<ApproveEnrollmentCommandHandler> _logger) // 1. Injected ILogger
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

        // 2. Capture the old status before mutating the entity
        var oldStatus = enrollment.Status.ToString();
        
        enrollment.Status = request.Decision == "Approved" 
            ? EnrollmentStatus.Approved 
            : EnrollmentStatus.Rejected;
            
        enrollment.CreatedAt = DateTime.UtcNow; // (Consider renaming this to DecisionDate in your model!)
        enrollment.DecisionReason = request.Decision; 

        // 3. Create the Audit Record using LearnerId
        var audit = new EnrollmentDecision()
        {
            Id = Guid.NewGuid(),
            EntityName = nameof(Enrollment),
            EntityId = enrollment.Id.ToString(),
            Action = "Decision Made",
            OldValue = oldStatus,
            NewValue = enrollment.Status.ToString(),
            PerformedBy = enrollment.LearnerId.ToString(), // <-- Using LearnerId as requested
            CreatedAt = DateTime.UtcNow
        };

        _context.EnrollmentDecisions.Add(audit);

        await _context.SaveChangesAsync(cancellationToken);

        // 4. Structured Logging for Serilog/Console
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
