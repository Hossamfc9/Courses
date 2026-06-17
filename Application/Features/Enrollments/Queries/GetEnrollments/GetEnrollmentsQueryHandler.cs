using Application.Common;
using Application.Common.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Enrollments.Queries.GetEnrollments;

public class GetEnrollmentsQueryHandler(IContext _context) : IRequestHandler<GetEnrollmentsQuery, RequestResult<CursorPagedResult<GetEnrollmentsDto>>>
{
    public async Task<RequestResult<CursorPagedResult<GetEnrollmentsDto>>> Handle(GetEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Enrollments.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Cursor) &&
            CursorHelper.Decode<DateTime, Guid>(request.Cursor) is var (cursorDate, cursorId))
        {
            query = query.Where(c => c.CreatedAt < cursorDate || 
                                     (c.CreatedAt == cursorDate && c.Id.CompareTo(cursorId) < 0));
        }

        if (request.CourseId != null)
        {
            query = query.Where(c => c.CourseId == request.CourseId);
        }

        if (request.LearnerId != null)
        {
            query = query.Where(c => c.LearnerId == request.LearnerId);
        }

        if (request.FromDate != null)
        {
            query = query.Where(c => c.CreatedAt >= request.FromDate);
        }
        
        if (request.ToDate != null)
        {
            query = query.Where(c => c.CreatedAt <= request.ToDate);
        }

        query = query.OrderByDescending(c => c.CreatedAt)
                     .ThenByDescending(c => c.Id);

        var projectQuery = query.Select(e => new GetEnrollmentsDto()
        {
            Id = e.Id,
            CourseId = e.CourseId,
            LearnerId = e.LearnerId,
            CreatedAt = e.CreatedAt,
            Status = e.Status
        });

        var pagedResult = await projectQuery.ToCursorPageAsync(
            limit: request.Limit,
            getCursorProps: dto => (dto.CreatedAt, dto.Id)
        );

        return RequestResult<CursorPagedResult<GetEnrollmentsDto>>.Success(pagedResult);
    }
}
