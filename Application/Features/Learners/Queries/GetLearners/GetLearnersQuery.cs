using Application.Common;
using Application.Common.Pagination;
using MediatR;

namespace Application.Features.Learners.Queries.GetLearners;

public sealed record GetLearnersQuery(int Limit = 10, string Cursor = "") : IRequest<RequestResult<CursorPagedResult<GetLearnersDto>>>;