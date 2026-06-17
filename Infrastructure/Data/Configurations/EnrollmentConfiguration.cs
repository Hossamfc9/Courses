using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollment");
        builder.HasKey(c => c.Id);
        builder.HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Learner)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.LearnerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(e => new { e.CourseId, e.LearnerId }).IsUnique();
    }
}