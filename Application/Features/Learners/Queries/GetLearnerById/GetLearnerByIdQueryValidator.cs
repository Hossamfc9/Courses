using FluentValidation;

namespace Application.Features.Learners.Queries.GetLearnerById;

public class GetLearnerByIdQueryValidator : AbstractValidator<GetLearnerByIdQuery>
{
    public GetLearnerByIdQueryValidator()
    {
        RuleFor(l => l.Id).NotNull().NotEmpty()
            .WithMessage("Id cannot be null or empty");
    }
}