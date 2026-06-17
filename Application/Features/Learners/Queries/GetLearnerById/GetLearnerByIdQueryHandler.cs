using Application.Common;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Learners.Queries.GetLearnerById;

public class GetLearnerByIdQueryHandler(IContext _context) : IRequestHandler<GetLearnerByIdQuery, RequestResult<GetLearnerByIdDto>>
{
    public async Task<RequestResult<GetLearnerByIdDto>> Handle(GetLearnerByIdQuery request, CancellationToken cancellationToken)
    {
        var learner = await _context.Learners.FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (learner == null)
        {
            throw new NotFoundException(request.Id, "Learner");
        }

        var dto = new GetLearnerByIdDto()
        {
            FullName = learner.FullName,
            Department = learner.Department,
            Email = learner.Email,
            NationalId = learner.NationalId,
        };
        return RequestResult<GetLearnerByIdDto>.Success(dto);
    }
}