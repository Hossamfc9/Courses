namespace Domain.Models;

public class EnrollmentDecision : BaseEntity
{
    public string EntityId { get; init; }
    public string EntityName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string PerformedBy { get; set; } = string.Empty;
}