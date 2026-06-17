using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class LearnerConfiguration : IEntityTypeConfiguration<Learner>
{
    public void Configure(EntityTypeBuilder<Learner> builder)
    {
        builder.ToTable("Learner");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.FullName).IsRequired();
        builder.Property(c => c.Email).IsRequired();
        builder.Property(c => c.NationalId).IsRequired();
        
        
    }
}