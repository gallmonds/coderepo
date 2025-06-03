using coderepo_api.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>().ToTable("dbuser");

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.username)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.email)
            .IsUnique();
    }
}
