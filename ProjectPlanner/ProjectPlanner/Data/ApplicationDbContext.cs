using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Data.Projects;

namespace ProjectPlanner.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{

    public virtual DbSet<GanttModel> Gantts{ get; set; } = default!;
    public virtual DbSet<NoteAttachmentModel> NoteAttachments { get; set; } = default!;
    public virtual DbSet<NotesModel> Notes { get; set; } = default!;
    public virtual DbSet<ProjectModel> Projects { get; set; } = default!;
    public virtual DbSet<UserProjectsModel> UserProjects { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<ProjectModel>(b =>
        {
            b.HasKey(r => r.Id);
            b.Property(f => f.Id).ValueGeneratedOnAdd();
            b.ToTable("Projects");
        });
        builder.Entity<GanttModel>(b =>
        {
            b.HasKey(r => r.Id);
            b.Property(f => f.Id).ValueGeneratedOnAdd();
            b.ToTable("Gantts");
        });
        
        builder.Entity<NoteAttachmentModel>(b =>
        {
            b.HasKey(r => r.Id);
            b.Property(f => f.Id).ValueGeneratedOnAdd();
            b.ToTable("NoteAttachments");
        });

        builder.Entity<NotesModel>(b =>
        {
            b.HasKey(r => r.Id);
            b.Property(f => f.Id).ValueGeneratedOnAdd();
            b.ToTable("Notes");
        });

        builder.Entity<UserProjectsModel>(b =>
        {
            b.HasKey(r => r.Id);
            b.Property(f => f.Id).ValueGeneratedOnAdd();
            b.ToTable("UserProjects");
        });
    }
}