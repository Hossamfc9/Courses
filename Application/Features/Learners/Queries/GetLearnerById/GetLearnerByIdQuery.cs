using Application.Common;
using MediatR;

namespace Application.Features.Learners.Queries.GetLearnerById;

public sealed record GetLearnerByIdQuery(Guid Id) : IRequest<RequestResult<GetLearnerByIdDto>>;