using FluentValidation;

namespace Application.Features.Courses.Commands.DeleteCourse;

public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
{
    public DeleteCourseCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Course id is required.");
    }
}