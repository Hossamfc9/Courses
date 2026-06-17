using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Learner : BaseEntity
{
    [Required]
    public string FullName { get; set; }
    [Required]
    public string NationalId { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    
    public virtual ICollection<Enrollment> Enrollments { get; set; }
}