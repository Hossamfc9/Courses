using Application.Features.Enrollments.Commands.ApproveEnrollment;
using Application.Features.Enrollments.Commands.CreateEnrollment;
using Application.Features.Enrollments.Queries.GetEnrollments;
using Courses.Requests.Enrollment;
using Infrastructure.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Role("Learner")]
    public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentRequest request)
    {
        return Ok(await mediator.Send(new CreateEnrollmentCommand(request.LearnerId, request.CourseId)));
    }

    [HttpPost("{EnrollmentId}/{Decision}")]
    [Role("Admin")]
    public async Task<IActionResult> ApproveEnrollment([FromRoute] ApproveEnrollmentRequest request, [FromBody] string Reason)
    {
        return Ok(await mediator.Send(new ApproveEnrollmentCommand(request.EnrollmentId, request.Decision, Reason)));
    }

    [HttpGet]
    public async Task<IActionResult> GetEnrollments([FromQuery] GetEnrollmentsRequest request)
    {
        return Ok(await mediator.Send(new GetEnrollmentsQuery(request.Limit, request.Cursor, request.LearnerId,
            request.CourseId, request.Status.ToString(), request.FromDate, request.ToDate)));
    }
}