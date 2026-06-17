using Application.Features.Learners.Commands.AddLearner;
using Application.Features.Learners.Queries.GetLearnerById;
using Application.Features.Learners.Queries.GetLearners;
using Courses.Requests.Learner;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Courses.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LearnersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddLearner([FromBody] AddLearnerRequest request)
    {
        return Ok(await mediator.Send(new AddLearnerCommand(request.FullName, request.Email, request.NationalId,
            request.Department)));
    }

    [HttpGet("{LearnerId}")]
    public async Task<IActionResult> GetLearnerById([FromRoute] Guid LearnerId)
    {
        return Ok(await mediator.Send(new GetLearnerByIdQuery(LearnerId)));
    }

    [HttpGet]
    public async Task<IActionResult> GetLearners([FromQuery] GetLearnersRequest request)
    {
        return Ok(await mediator.Send(new GetLearnersQuery(request.Limit, request.Cursor)));
    }
}