using coderepo_api.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Algorithm> Algorithms { get; set; }
    public DbSet<AlgorithmMeta> AlgorithmMetas { get; set; }
    public DbSet<AlgorithmLang> AlgorithmLangs { get; set; }
    public DbSet<AlgorithmCollaborator> AlgorithmCollaborators { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TagAlgorithm> TagAlgorithms { get; set; }
    public DbSet<DbUser> Users { get; set; }
    public DbSet<Media> Media { get; set; }
    public DbSet<SupportedLang> SupportedLangs { get; set; }
    public DbSet<AlgorithmChangelog> AlgorithmChangelogs { get; set; }
    public DbSet<Rating> Ratings { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DbUser>(entity =>
        {
            entity.ToTable("dbuser");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("user_id");
            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");
            entity.Property(e => e.IsFlagged).HasColumnName("isflagged");
            entity.Property(e => e.IsBanned).HasColumnName("isbanned");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.auditIsDeleted).HasColumnName("audit_isdeleted");
            entity.Property(e => e.PfpId).HasColumnName("pfp_id");

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(u => u.MediaDb)
                  .WithMany(m => m.Users)
                  .HasForeignKey(u => u.PfpId);
        });

        modelBuilder.Entity<Algorithm>(entity =>
        {
            entity.ToTable("algorithm");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("algorithm_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");

            entity.HasOne(a => a.UserDb)
                  .WithMany(u => u.Algorithms)
                  .HasForeignKey(a => a.OwnerId);

            entity.ToTable("algorithm");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("algorithm_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
        });

        modelBuilder.Entity<AlgorithmMeta>(entity =>
        {
            entity.ToTable("algorithm_meta");
            entity.HasKey(e => e.AlgorithmId);
            entity.Property(e => e.AlgorithmId).HasColumnName("algorithm_id");
            entity.Property(e => e.RootPath).HasColumnName("root_path");
            entity.Property(e => e.IsPrivate).HasColumnName("isprivate");
            entity.Property(e => e.IsFlagged).HasColumnName("isflagged");
            entity.Property(e => e.IsDisabled).HasColumnName("isdisabled");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");

            entity.HasOne(am => am.Algorithm)
                  .WithOne(a => a.AlgorithmMeta)
                  .HasForeignKey<AlgorithmMeta>(am => am.AlgorithmId);

            entity.ToTable("algorithm_meta");
            entity.HasKey(e => e.AlgorithmId);
            entity.Property(e => e.AlgorithmId).HasColumnName("algorithm_id");
            entity.Property(e => e.RootPath).HasColumnName("root_path");
            entity.Property(e => e.IsPrivate).HasColumnName("isprivate");
            entity.Property(e => e.IsFlagged).HasColumnName("isflagged");
            entity.Property(e => e.IsDisabled).HasColumnName("isdisabled");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");
        });

        modelBuilder.Entity<AlgorithmLang>(entity =>
        {
            entity.ToTable("algorithm_lang");
            entity.HasKey(e => new { e.AlgorithmId, e.SupportedLangId });
            entity.Property(e => e.AlgorithmId).HasColumnName("algorithm_id");
            entity.Property(e => e.SupportedLangId).HasColumnName("lang_id");
            entity.Property(e => e.RootlangPath).HasColumnName("rootlang_path");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");

            entity.HasOne(al => al.Algorithm)
                  .WithMany(a => a.AlgorithmLangs)
                  .HasForeignKey(al => al.AlgorithmId);

            entity.HasOne(al => al.SupportedLang)
                  .WithMany(sl => sl.AlgorithmLangs)
                  .HasForeignKey(al => al.SupportedLangId);
        });

        modelBuilder.Entity<AlgorithmCollaborator>(entity =>
        {
            entity.ToTable("algorithm_collaborator");
            entity.HasKey(e => new { e.AlgorithmId, e.UserId });
            entity.Property(e => e.AlgorithmId).HasColumnName("algorithm_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");

            entity.HasOne(ac => ac.Algorithm)
                  .WithMany(a => a.AlgorithmCollaorators)
                  .HasForeignKey(ac => ac.AlgorithmId);

            entity.HasOne(ac => ac.User)
                  .WithMany(u => u.AlgorithmCollaborators)
                  .HasForeignKey(ac => ac.UserId);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("tag");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("tag_id");
            entity.Property(e => e.Name).HasColumnName("tag_name");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");
        });

        modelBuilder.Entity<TagAlgorithm>(entity =>
        {
            entity.ToTable("tag_algorithm");
            entity.HasKey(e => new { e.TagId, e.AlgorithmId });
            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.AlgorithmId).HasColumnName("algorithm_id");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");

            entity.HasOne(ta => ta.Tag)
                  .WithMany(t => t.AlgorithmTags)
                  .HasForeignKey(ta => ta.TagId);

            entity.HasOne(ta => ta.Algorithm)
                  .WithMany(a => a.AlgorithmTags)
                  .HasForeignKey(ta => ta.AlgorithmId);
        });

        modelBuilder.Entity<Media>(entity =>
        {
            entity.ToTable("media");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("media_id");
            entity.Property(e => e.FilePath).HasColumnName("file_path");
            entity.Property(e => e.MimeType).HasColumnName("mime_type");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IsDeleted).HasColumnName("audit_isdeleted");
        });

        modelBuilder.Entity<SupportedLang>(entity =>
        {
            entity.ToTable("supportedlang");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("lang_id");
            entity.Property(e => e.LangName).HasColumnName("lang_name");
            entity.Property(e => e.LangExtension).HasColumnName("lang_extension");
            entity.Property(e => e.IconId).HasColumnName("icon_id");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");

            entity.HasOne(sl => sl.MediaDb)
                  .WithMany(m => m.Supportedlangs)
                  .HasForeignKey(sl => sl.IconId);
        });

        modelBuilder.Entity<AlgorithmChangelog>(entity =>
        {
            entity.ToTable("algorithm_changelog");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("changelog_id");
            entity.Property(e => e.AlgorithmId).HasColumnName("algorithm_id");
            entity.Property(e => e.SupportedLangId).HasColumnName("lang_id");
            entity.Property(e => e.FilePath).HasColumnName("file_path");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");

            entity.HasOne(e => e.Algorithm)
                  .WithMany(a => a.AlgorithmChangelogs)
                  .HasForeignKey(e => e.AlgorithmId);
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("rating");
            entity.HasKey(e => new { e.UserId, e.TypeId, e.ContentId });

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.ContentId).HasColumnName("content_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.AuditIsDeleted).HasColumnName("audit_isdeleted");
            entity.Property(e => e.Version).HasColumnName("version");

            entity.HasOne(r => r.User)
                  .WithMany(u => u.Ratings)
                  .HasForeignKey(r => r.UserId);

            entity.HasOne(r => r.Algorithm)
                  .WithMany(a => a.Ratings)
                  .HasForeignKey(r => r.ContentId)
                  .HasPrincipalKey(a => a.Id);
        });

    }
}