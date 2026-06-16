using Application.Common;
using MediatR;

namespace Application.Features.Courses.Commands.CreateCourse;

public sealed record CreateCourseCommand(string Title, string? Description, short DurationHours, 
    bool RequiresApproval, bool IsActive) : IRequest<RequestResult<CreateCourseCommandDto>>;