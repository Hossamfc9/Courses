namespace Domain.Models;

public class Course : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public short DurationHours { get; set; }
    public bool RequiresApproval { get; set; }
    public bool IsActive { get; set; }
}