using FluentValidation;

namespace Application.Features.Enrollments.Commands.ApproveEnrollment;

public class ApproveEnrollmentCommandValidator : AbstractValidator<ApproveEnrollmentCommand>
{
    public ApproveEnrollmentCommandValidator()
    {
        RuleFor(x => x.Decision)
            .Must(d => d == "Approved" || d == "Rejected")
            .WithMessage("Decision must be either 'Approved' or 'Rejected'.");
        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("reason is required");
        
    }
}