using Application.Common;
using MediatR;

namespace Application.Features.Enrollments.Commands.CreateEnrollment;

public sealed record CreateEnrollmentCommand(Guid LearnerId, Guid CourseId) : IRequest<RequestResult<CreateEnrollmentDto>>;