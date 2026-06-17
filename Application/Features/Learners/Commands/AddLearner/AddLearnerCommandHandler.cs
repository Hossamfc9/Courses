using Application.Common;
using Domain.Models;
using MediatR;

namespace Application.Features.Learners.Commands.AddLearner;

public class AddLearnerCommandHandler(IContext _context) : IRequestHandler<AddLearnerCommand, RequestResult<AddLearnerCommandDto>>
{
    public async Task<RequestResult<AddLearnerCommandDto>> Handle(AddLearnerCommand request, CancellationToken cancellationToken)
    {
        var learner = new Learner()
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            NationalId = request.NationalId,
            Email = request.Email,
            Department = request.Department,
            CreatedAt = DateTime.UtcNow
        };

        _context.Learners.Add(learner);

        await _context.SaveChangesAsync(cancellationToken);

        return RequestResult<AddLearnerCommandDto>.Success(new AddLearnerCommandDto()
        {
            LearnerId = learner.Id,
        });
    }
}