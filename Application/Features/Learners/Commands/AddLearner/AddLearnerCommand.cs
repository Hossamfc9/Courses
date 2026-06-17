using Application.Common;
using MediatR;

namespace Application.Features.Learners.Commands.AddLearner;

public sealed record AddLearnerCommand(string FullName, string Email, string NationalId, string Department) : IRequest<RequestResult<AddLearnerCommandDto>>;