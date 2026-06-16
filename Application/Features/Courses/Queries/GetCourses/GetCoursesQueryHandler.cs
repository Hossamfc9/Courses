using Application.Common;
using Application.Common.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Courses.Queries.GetCourses;

public class GetCoursesQueryHandler(IContext _context) : IRequestHandler<GetCoursesQuery, RequestResult<CursorPagedResult<GetCourseDto>>>
{
    public async Task<RequestResult<CursorPagedResult<GetCourseDto>>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Courses.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Cursor) && 
            CursorHelper.Decode<DateTime, Guid>(request.Cursor) is var (cursorDate, cursorId))
        {
            query = query.Where(c => c.CreatedAt < cursorDate || 
                                    (c.CreatedAt == cursorDate && c.Id.CompareTo(cursorId) < 0));
        }
        query = query.OrderByDescending(c => c.CreatedAt)
            .ThenByDescending(c => c.Id);
        var projectedQuery = query.Select(c => new GetCourseDto
        {
            Title = c.Title,
            Description = c.Description,
            DurationHours = c.DurationHours,
            IsActive = c.IsActive,
            RequiresApproval = c.RequiresApproval
        });

        var pagedResult = await projectedQuery.ToCursorPageAsync(
            limit: request.Limit,
            getCursorProps: dto => (dto.Title, dto.DurationHours) 
        );

        return RequestResult<CursorPagedResult<GetCourseDto>>.Success(pagedResult);
    }
}