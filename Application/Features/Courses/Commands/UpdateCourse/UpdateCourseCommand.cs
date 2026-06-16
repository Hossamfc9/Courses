using Application.Common;
using MediatR;

namespace Application.Features.Courses.Commands.UpdateCourse;

public sealed record UpdateCourseCommand(Guid Id, string Title, string? Description, short DurationHours
,bool IsActive, bool RequiresApproval) : IRequest<RequestResult<UpdateCourseDto>>;