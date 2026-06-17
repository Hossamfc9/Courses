using FluentValidation;

namespace Application.Features.Enrollments.Queries.GetEnrollments;

public class GetEnrollmentsQueryValidator : AbstractValidator<GetEnrollmentsQuery>
{
    public GetEnrollmentsQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThan(0).WithMessage("Page limit must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Page limit cannot exceed 100 items per request.");
        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate != null && x.ToDate != null)
            .WithMessage("'ToDate' must be greater than or equal to 'FromDate'.");
        RuleFor(x => x.ToDate)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .When(x => x.ToDate != null)
            .WithMessage("Search date cannot be set in the future.");
    }
}
