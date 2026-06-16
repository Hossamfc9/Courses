using Application.Common;
using Domain.Models;
using MediatR;

namespace Application.Features.Courses.Commands.CreateCourse;

public sealed class CreateCourseCommandHandler(IContext _context) : 
    IRequestHandler<CreateCourseCommand, RequestResult<CreateCourseCommandDto>>
{
    public async Task<RequestResult<CreateCourseCommandDto>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            DurationHours = request.DurationHours,
            RequiresApproval = request.RequiresApproval,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Courses.Add(course);

        await _context.SaveChangesAsync(cancellationToken);
        return RequestResult<CreateCourseCommandDto>.Success(new CreateCourseCommandDto()
        {
            CourseId = course.Id,
        });
    }
}