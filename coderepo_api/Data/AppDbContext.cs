using coderepo_api.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Algorithm> Algorithms { get; set; }
    public DbSet<AlgorithmMeta> AlgorithmMeta { get; set; }
    public DbSet<AlgorithmChangelog> AlgorithmChangelogs { get; set; }
    public DbSet<AlgorithmLang> AlgorithmLangs { get; set; }

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

        modelBuilder.Entity<Algorithm>()
            .ToTable("algorithm")
            .HasKey(a => a.algorithm_id);

        modelBuilder.Entity<AlgorithmMeta>()
            .ToTable("algorithm_meta")
            .HasKey(am => am.algorithm_id);
        modelBuilder.Entity<AlgorithmMeta>()
            .Property(a => a.created_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<AlgorithmChangelog>()
            .ToTable("algorithm_changelog")
            .HasKey(ac => ac.algorithm_id);
        modelBuilder.Entity<AlgorithmChangelog>()
            .Property(ac => ac.created_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<AlgorithmLang>()
            .ToTable("algorithm_lang")
            .HasKey(al => al.algorithm_id);
        modelBuilder.Entity<AlgorithmLang>()
            .Property(al => al.created_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }
}
