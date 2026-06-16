using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Common;

public interface IContext
{
    DbSet<Course> Courses { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}