using Microsoft.EntityFrameworkCore;
using SREES.DAL.Models;

namespace SREES.DAL.Context
{
    public class SreesContext : DbContext
    {
        public SreesContext(DbContextOptions<SreesContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracija User entiteta
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(500);

                entity.Property(e => e.Role)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.Guid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasName("IX_User_Email_Unique");

                entity.HasIndex(e => e.IsDeleted)
                    .HasName("IX_User_IsDeleted");

                entity.ToTable("Users", schema: "dbo");
            });

            // Konfiguracija Region entiteta
            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Latitude)
                    .HasPrecision(10, 6);

                entity.Property(e => e.Longitude)
                    .HasPrecision(10, 6);

                entity.Property(e => e.Guid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasIndex(e => e.Name)
                    .HasName("IX_Region_Name");

                entity.HasIndex(e => e.IsDeleted)
                    .HasName("IX_Region_IsDeleted");

                entity.ToTable("Regions", schema: "dbo");
            });

            // Konfiguracija Substation entiteta
            modelBuilder.Entity<Substation>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.SubstationType)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.Latitude)
                    .HasPrecision(10, 6);

                entity.Property(e => e.Longitude)
                    .HasPrecision(10, 6);

                entity.Property(e => e.RegionId)
                    .IsRequired();

                entity.Property(e => e.Guid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasIndex(e => e.RegionId)
                    .HasName("IX_Substation_RegionId");

                entity.HasIndex(e => e.SubstationType)
                    .HasName("IX_Substation_Type");

                entity.HasIndex(e => e.IsDeleted)
                    .HasName("IX_Substation_IsDeleted");

                // Foreign key relationship
                entity.HasOne(s => s.Region)
                    .WithMany()
                    .HasForeignKey(s => s.RegionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable("Substations", schema: "dbo");
            });

            // Konfiguracija Outage entiteta
            modelBuilder.Entity<Outage>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.RegionId)
                    .IsRequired();

                entity.Property(e => e.OutageStatus)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.Guid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_Outage_UserId");

                entity.HasIndex(e => e.RegionId)
                    .HasName("IX_Outage_RegionId");

                entity.HasIndex(e => e.OutageStatus)
                    .HasName("IX_Outage_Status");

                entity.HasIndex(e => e.IsDeleted)
                    .HasName("IX_Outage_IsDeleted");

                // Foreign key relationship
                entity.HasOne(o => o.User)
                    .WithMany()
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable("Outages", schema: "dbo");
            });
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Outage> Outages { get; set; }
        public virtual DbSet<Substation> Substations { get; set; }
        public virtual DbSet<Region> Regions { get; set; }

    }
}
