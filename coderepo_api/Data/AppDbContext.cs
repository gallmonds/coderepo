using Microsoft.EntityFrameworkCore;
using coderepo_api.Models;
using System.Security.Cryptography.X509Certificates;

namespace coderepo_api.Data;
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Algorithm> Algorithms { get; set; }
        public DbSet<AlgorithmLang> AlgorithmLangs { get; set; }
        public DbSet<AlgorithmMeta> AlgorithmMetas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("dbuser");
        modelBuilder.Entity<Algorithm>().ToTable("algorithm");
        modelBuilder.Entity<AlgorithmLang>().ToTable("algorithm_lang");
        modelBuilder.Entity<AlgorithmMeta>().ToTable("algorithm_meta");

        modelBuilder.Entity<Algorithm>()
        .HasOne(a => a.Meta)
        .WithOne(m => m.Algorithm)
        .HasForeignKey<AlgorithmMeta>(m => m.algorithm_id);

        modelBuilder.Entity<Algorithm>()
        .HasMany(a => a.Langs)
        .WithOne(l => l.Algorithm)
        .HasForeignKey(l => l.algorithm_id);

        modelBuilder.Entity<AlgorithmLang>()
        .HasKey(al => new { al.algorithm_id, al.lang_id });
    }
    }