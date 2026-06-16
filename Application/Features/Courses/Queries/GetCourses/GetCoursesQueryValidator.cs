using FluentValidation;

namespace Application.Features.Courses.Queries.GetCourses;

public class GetCoursesQueryValidator : AbstractValidator<GetCoursesQuery>
{
    public GetCoursesQueryValidator()
    {
        RuleFor(c => c.Limit).GreaterThan(0);
    }
}