using Application.Common;
using Courses.Models.Enums;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Enrollments.Commands.CreateEnrollment;

public class CreateEnrollmentCommandHandler(IContext _context) : IRequestHandler<CreateEnrollmentCommand, RequestResult<CreateEnrollmentDto>>
{
    public async Task<RequestResult<CreateEnrollmentDto>> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .Select(c => new { c.Id, c.RequiresApproval })
            .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);

        var enrollment = new Enrollment
        {
            CourseId = request.CourseId,
            LearnerId = request.LearnerId,
    
            CreatedAt = DateTime.UtcNow, 
    
            Status = course.RequiresApproval 
                ? EnrollmentStatus.PendingApproval 
                : EnrollmentStatus.Approved
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync(cancellationToken);

        return RequestResult<CreateEnrollmentDto>.Success(new CreateEnrollmentDto());
    }
}