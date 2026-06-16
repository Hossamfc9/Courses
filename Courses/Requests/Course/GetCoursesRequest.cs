namespace Courses.Requests.Course;

public class GetCoursesRequest
{
    public int Limit { get; set; }
    public string? Cursor { get; set; }
}