namespace Courses.Requests.Enrollment;

public class ApproveEnrollmentRequest
{
    public Guid EnrollmentId { get; set; }
    public string Decision { get; set; }
}