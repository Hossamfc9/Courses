using Application.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Enrollments.Commands.CreateEnrollment;

public class CreateEnrollmentCommandValidator : AbstractValidator<CreateEnrollmentCommand>
{
    private readonly IContext _context;
    public CreateEnrollmentCommandValidator(IContext context)
    {
        _context = context;

        RuleFor(e => e.CourseId).NotEmpty().WithMessage("CourseId cannot be null");
        RuleFor(e => e.LearnerId).NotEmpty().WithMessage("LearnerId cannot be empty");
        RuleFor(e => e.CourseId).MustAsync(IsActiveCourse).WithMessage("You can't enroll inactive course");
        RuleFor(e => e).MustAsync((AlreadyEnrolled)).WithName("Enrollment")
            .WithMessage("The learner is already enrolled in this course.");

    }

    private async Task<bool> IsActiveCourse(Guid courseId, CancellationToken cancellationToken)
    {
        var isActive = await _context.Courses
            .Where(c => c.Id == courseId)
            .Select(c => c.IsActive)
            .FirstOrDefaultAsync(cancellationToken);

        return isActive;
    }
    
    private async Task<bool> AlreadyEnrolled(CreateEnrollmentCommand command, CancellationToken cancellationToken)
    {
        bool exists = await _context.Enrollments
            .AnyAsync(e => e.CourseId == command.CourseId && e.LearnerId == command.LearnerId, cancellationToken);

        return !exists;
    }
}