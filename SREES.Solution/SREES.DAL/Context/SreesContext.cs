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
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETDATE()")
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
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETDATE()")
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
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETDATE()")
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

            // Konfiguracija Pole entiteta
            modelBuilder.Entity<Pole>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Latitude)
                    .HasPrecision(10, 6);

                entity.Property(e => e.Longitude)
                    .HasPrecision(10, 6);

                entity.Property(e => e.Address)
                    .HasMaxLength(500);

                entity.Property(e => e.PoleType)
                    .HasConversion<int>();

                entity.Property(e => e.Guid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasIndex(e => e.RegionId)
                    .HasName("IX_Pole_RegionId");

                entity.HasIndex(e => e.PoleType)
                    .HasName("IX_Pole_Type");

                entity.HasIndex(e => e.IsDeleted)
                    .HasName("IX_Pole_IsDeleted");

                // Foreign key relationship
                entity.HasOne(p => p.Region)
                    .WithMany()
                    .HasForeignKey(p => p.RegionId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.ToTable("Poles", schema: "dbo");
            });

            // Konfiguracija Building entiteta
            modelBuilder.Entity<Building>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Latitude)
                    .HasPrecision(10, 6);

                entity.Property(e => e.Longitude)
                    .HasPrecision(10, 6);

                entity.Property(e => e.OwnerName)
                    .HasMaxLength(200);

                entity.Property(e => e.Address)
                    .HasMaxLength(500);

                entity.Property(e => e.Guid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasIndex(e => e.RegionId)
                    .HasName("IX_Building_RegionId");

                entity.HasIndex(e => e.PoleId)
                    .HasName("IX_Building_PoleId");

                entity.HasIndex(e => e.IsDeleted)
                    .HasName("IX_Building_IsDeleted");

                // Foreign key relationships
                entity.HasOne(b => b.Region)
                    .WithMany()
                    .HasForeignKey(b => b.RegionId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(b => b.Pole)
                    .WithMany()
                    .HasForeignKey(b => b.PoleId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.ToTable("Buildings", schema: "dbo");
            });

            // Konfiguracija Feeder entiteta
            modelBuilder.Entity<Feeder>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(200);

                entity.Property(e => e.FeederType)
                    .HasConversion<int>();

                entity.Property(e => e.Guid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasIndex(e => e.SubstationId)
                    .HasName("IX_Feeder_SubstationId");

                entity.HasIndex(e => e.FeederType)
                    .HasName("IX_Feeder_Type");

                entity.HasIndex(e => e.IsDeleted)
                    .HasName("IX_Feeder_IsDeleted");

                // Foreign key relationship
                entity.HasOne(f => f.Substation)
                    .WithMany()
                    .HasForeignKey(f => f.SubstationId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.ToTable("Feeders", schema: "dbo");
            });

            // Konfiguracija Customer entiteta
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CustomerType)
                    .HasConversion<int>();

                entity.Property(e => e.Guid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LastUpdateTime)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasIndex(e => e.BuildingId)
                    .HasName("IX_Customer_BuildingId");

                entity.HasIndex(e => e.CustomerType)
                    .HasName("IX_Customer_Type");

                entity.HasIndex(e => e.IsActive)
                    .HasName("IX_Customer_IsActive");

                entity.HasIndex(e => e.IsDeleted)
                    .HasName("IX_Customer_IsDeleted");

                // Foreign key relationship
                entity.HasOne(c => c.Building)
                    .WithMany()
                    .HasForeignKey(c => c.BuildingId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.ToTable("Customers", schema: "dbo");
            });
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Outage> Outages { get; set; }
        public virtual DbSet<Substation> Substations { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Pole> Poles { get; set; }
        public virtual DbSet<Feeder> Feeders { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

    }
}
