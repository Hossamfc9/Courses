using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Common;

public interface IContext
{
    DbSet<Course> Courses { get; }
    DbSet<Learner> Learners { get; }
    DbSet<Enrollment> Enrollments { get; }
    DbSet<EnrollmentDecision> EnrollmentDecisions { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}