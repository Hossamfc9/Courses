using Application.Common;
using Application.Common.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Learners.Queries.GetLearners;

public class GetLearnersQueryHandler(IContext _context) : IRequestHandler<GetLearnersQuery, RequestResult<CursorPagedResult<GetLearnersDto>>>
{
    public async Task<RequestResult<CursorPagedResult<GetLearnersDto>>> Handle(GetLearnersQuery request, CancellationToken cancellationToken)
    {
        var query= _context.Learners.AsNoTracking();
        
        if (!string.IsNullOrWhiteSpace(request.Cursor) && 
            CursorHelper.Decode<DateTime, Guid>(request.Cursor) is var (cursorDate, cursorId))
        {
            query = query.Where(c => c.CreatedAt < cursorDate || 
                                    (c.CreatedAt == cursorDate && c.Id.CompareTo(cursorId) < 0));
        }

        query = query.OrderByDescending(c => c.CreatedAt)
            .ThenByDescending(c => c.Id);

        var projectQuery = query.Select(c => new GetLearnersDto()
        {
            FullName = c.FullName,
            Email = c.Email,
            Department = c.Department,
            NationalId = c.NationalId
        });

        var pagedResult = await projectQuery.ToCursorPageAsync(
            limit: request.Limit,
            getCursorProps: dto => (dto.FullName, dto.NationalId)
        );
        
        return RequestResult<CursorPagedResult<GetLearnersDto>>.Success(pagedResult);
    }
}