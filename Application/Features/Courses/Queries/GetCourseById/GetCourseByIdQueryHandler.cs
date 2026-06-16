using Application.Common;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Courses.Queries.GetCourseById;

public class GetCourseByIdQueryHandler(IContext _context) : IRequestHandler<GetCourseByIdQuery, RequestResult<GetCourseByIdDto>>
{
    public async Task<RequestResult<GetCourseByIdDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.Where(c => c.Id == request.Id).FirstOrDefaultAsync();
        if (course == null)
        {
            throw new NotFoundException(request.Id, "Course");
        }

        return RequestResult<GetCourseByIdDto>.Success(new GetCourseByIdDto()
        {
            Title = course.Title,
            Description = course.Description,
            DurationHours = course.DurationHours,
            IsActive = course.IsActive,
            RequiresApproval = course.RequiresApproval,
        });
    }
}