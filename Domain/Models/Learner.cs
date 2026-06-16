namespace Domain.Models;

public class Learner : BaseEntity
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string NationalId { get; set; }
    
}