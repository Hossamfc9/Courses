using Courses.Models.Enums;

namespace Application.Features.Enrollments.Queries.GetEnrollments;

public class GetEnrollmentsDto
{
    public Guid Id { get; set; }
    public Guid LearnerId { get; set; }
    public Guid CourseId { get; set; }
    public EnrollmentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}