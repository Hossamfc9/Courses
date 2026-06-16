using FluentValidation;

namespace Application.Features.Courses.Commands.CreateCourse;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(command => command.Title).NotEmpty().WithMessage("Title cannot be empty");
        RuleFor(command => command.DurationHours)
            .NotEmpty().GreaterThan((short)0);
    }
}