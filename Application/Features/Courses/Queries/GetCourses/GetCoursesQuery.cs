using Application.Common;
using Application.Common.Pagination;
using MediatR;

namespace Application.Features.Courses.Queries.GetCourses;

public sealed record GetCoursesQuery(int Limit, string Cursor) : IRequest<RequestResult<CursorPagedResult<GetCourseDto>>>;