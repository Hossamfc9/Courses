using Application.Common;
using MediatR;

namespace Application.Features.Enrollments.Commands.ApproveEnrollment;

public sealed record ApproveEnrollmentCommand(Guid EnrollmentId, string Decision, string Reason) : IRequest<RequestResult<ApproveEnrollmentDto>>;