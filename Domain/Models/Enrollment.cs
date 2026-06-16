using System.ComponentModel.DataAnnotations.Schema;
using Courses.Models;
using Courses.Models.Enums;

namespace Domain.Models;

public class Enrollment : BaseEntity
{
    public EnrollmentStatus Status { get; set; }
    [ForeignKey("Learner")]
    public Guid LearnerId { get; set; }
    [ForeignKey("Course")]
    public Guid CourseId { get; set; } 
    public virtual Course Course { get; set; }
    public virtual Learner Learner { get; set; }
}