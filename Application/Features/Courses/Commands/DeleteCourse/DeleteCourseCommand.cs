using Application.Common;
using MediatR;

namespace Application.Features.Courses.Commands.DeleteCourse;

public sealed record DeleteCourseCommand(Guid Id) : IRequest<RequestResult<DeleteCourseDto>>;