using FluentValidation;

namespace Application.Features.Courses.Queries.GetCourseById;

public class GetCourseByIdQueryValidator : AbstractValidator<GetCourseByIdQuery>
{
    public GetCourseByIdQueryValidator()
    {
        RuleFor(q => q.Id).NotNull().WithMessage("Id cannot be null");
    }
}