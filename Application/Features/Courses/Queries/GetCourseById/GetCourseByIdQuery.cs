using Application.Common;
using MediatR;

namespace Application.Features.Courses.Queries.GetCourseById;

public sealed record GetCourseByIdQuery(Guid Id) : IRequest<RequestResult<GetCourseByIdDto>>;