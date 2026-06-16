namespace Courses.Models.Enums;

public enum EnrollmentStatus : byte
{
    PendingApproval = 1,
    Approved,
    Rejected,
    Cancelled
}