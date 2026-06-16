using Application.Features.Courses.Commands.CreateCourse;
using Application.Features.Courses.Commands.DeleteCourse;
using Application.Features.Courses.Commands.UpdateCourse;
using Application.Features.Courses.Queries.GetCourseById;
using Application.Features.Courses.Queries.GetCourses;
using Courses.Requests;
using Courses.Requests.Course;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
    {
        return Ok(await _mediator.Send(new CreateCourseCommand(request.Title, request.Description, 
            request.DurationHours, request.RequiresApproval, request.IsActive)));
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses([FromQuery] GetCoursesRequest request)
    {
        return Ok(await _mediator.Send(new GetCoursesQuery(request.Limit, request.Cursor ?? "")));
    }

    [HttpGet("{CourseId}")]
    public async Task<IActionResult> GetCourseById([FromRoute] GetCourseByIdRequest request)
    {
        return Ok(await _mediator.Send(new GetCourseByIdQuery(request.CourseId)));
    }
    
    [HttpDelete("{CourseId}")]
    public async Task<IActionResult> DeleteCourse([FromRoute] DeleteCourseRequest request)
    {
        return Ok(await _mediator.Send(new DeleteCourseCommand(request.CourseId)));
    }

    [HttpPut("{CourseId}")]
    public async Task<IActionResult> UpdateCourse([FromRoute] Guid CourseId, [FromBody] UpdateCourseRequest request)
    {
        return Ok(await _mediator.Send(new UpdateCourseCommand(CourseId, request.Title, request.Description,
            request.DurationHours, request.IsActive, request.RequiresValidation)));
    }

}