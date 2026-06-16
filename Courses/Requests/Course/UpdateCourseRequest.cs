namespace Courses.Requests.Course;

public class UpdateCourseRequest
{
    public string Title { get; set; }
    public short DurationHours { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
    public bool RequiresValidation { get; set; }
}