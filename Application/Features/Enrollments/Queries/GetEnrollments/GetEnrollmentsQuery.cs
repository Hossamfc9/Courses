using Application.Common;
using Application.Common.Pagination;
using MediatR;

namespace Application.Features.Enrollments.Queries.GetEnrollments;

public sealed record GetEnrollmentsQuery(int Limit, string Cursor, Guid? LearnerId, Guid? CourseId,
  string? Status, DateTime? FromDate, DateTime? ToDate) : IRequest<RequestResult<CursorPagedResult<GetEnrollmentsDto>>>;