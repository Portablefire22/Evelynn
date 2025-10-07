using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Data.Projects;

namespace ProjectPlanner.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{

    public virtual DbSet<ProjectModel> Projects { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<ProjectModel>(b =>
        {
            b.HasKey(r => new { r.Id, r.Name, r.Description, r.Created, r.Modified, r.ModifiedBy});
            b.ToTable("Projects");
        });
    }
}