namespace Courses.Requests.Enrollment;

public class CreateEnrollmentRequest
{
    public Guid LearnerId { get; set; }
    public Guid CourseId { get; set; }
}