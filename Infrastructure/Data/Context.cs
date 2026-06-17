using Application.Common;
using Domain.Models;
using Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class Context(DbContextOptions<Context> options) : DbContext(options), IContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Learner> Learners { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<EnrollmentDecision> EnrollmentDecisions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseConfiguration).Assembly);
    }
}