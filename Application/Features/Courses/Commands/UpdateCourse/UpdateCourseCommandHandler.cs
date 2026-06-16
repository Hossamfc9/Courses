using Application.Common;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(IContext _context) : IRequestHandler<UpdateCourseCommand, RequestResult<UpdateCourseDto>>
{
    public async Task<RequestResult<UpdateCourseDto>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
        {
            throw new NotFoundException(request.Id, "Course");
        }

        course.Title = request.Title;
        course.DurationHours = request.DurationHours;
        course.IsActive = request.IsActive;
        course.Description = request.Description;
        course.RequiresApproval = request.RequiresApproval;

        _context.Courses.Update(course);
        await _context.SaveChangesAsync(cancellationToken);
        return RequestResult<UpdateCourseDto>.Success(new UpdateCourseDto()
        {
            Id = course.Id
        });
    }
}