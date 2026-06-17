using Courses.Models.Enums;

namespace Courses.Requests.Enrollment;

public class GetEnrollmentsRequest
{
    public int Limit { get; set; } = 10;
    public string Cursor { get; set; } = string.Empty;
    public Guid? LearnerId { get; set; }
    public Guid? CourseId { get; set; }
    public EnrollmentStatus? Status { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }
}