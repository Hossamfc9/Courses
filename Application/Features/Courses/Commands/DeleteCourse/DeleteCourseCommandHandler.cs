using Application.Common;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Courses.Commands.DeleteCourse;

public class DeleteCourseCommandHandler(IContext _context) : IRequestHandler<DeleteCourseCommand, RequestResult<DeleteCourseDto>>
{
    public async Task<RequestResult<DeleteCourseDto>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);
        if (course == null)
        {
            throw new NotFoundException(request.Id, "Course");
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync(cancellationToken);

        return RequestResult<DeleteCourseDto>.Success(new DeleteCourseDto());
    }
}