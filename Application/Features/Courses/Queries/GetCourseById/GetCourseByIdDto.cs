namespace Application.Features.Courses.Queries.GetCourseById;

public class GetCourseByIdDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int DurationHours { get; set; }
    public bool IsActive { get; set; }
    public bool RequiresApproval { get; set; }
}