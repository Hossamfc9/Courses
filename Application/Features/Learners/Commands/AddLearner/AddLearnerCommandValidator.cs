using Application.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Learners.Commands.AddLearner;

public class AddLearnerCommandValidator : AbstractValidator<AddLearnerCommand>
{
    private readonly IContext _context;
    public AddLearnerCommandValidator(IContext context)
    {
        _context = context;
        RuleFor(x => x.FullName).NotEmpty()
            .WithMessage("Full Name is required");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address format is required.");
        
        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required.")
            .MustAsync(BeUniqueNationalId).WithMessage("A user with this National ID already exists.");
    }

    private async Task<bool> BeUniqueNationalId(string nationalId, CancellationToken cancellationToken)
    {
        bool exist = await _context.Learners.AnyAsync(u => u.NationalId == nationalId, cancellationToken);
        return !exist;
    }
}