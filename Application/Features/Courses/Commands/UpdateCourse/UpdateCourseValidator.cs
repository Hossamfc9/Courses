using FluentValidation;

namespace Application.Features.Courses.Commands.UpdateCourse;

public class UpdateCourseValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseValidator()
    {
        RuleFor(c => c.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(c => c.DurationHours).GreaterThan((short)0);
    }
}