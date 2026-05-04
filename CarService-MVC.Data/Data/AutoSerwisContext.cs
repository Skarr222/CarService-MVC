using CarService_MVC.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CarService_MVC.Data.Data;

public partial class AutoSerwisContext : DbContext
{
    public AutoSerwisContext()
    {
    }

    public AutoSerwisContext(DbContextOptions<AutoSerwisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<CmsContent> CmsContents { get; set; }

    public virtual DbSet<ContactRequest> ContactRequests { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<RepairOrder> RepairOrders { get; set; }

    public virtual DbSet<RepairOrderService> RepairOrderServices { get; set; }

    public virtual DbSet<RepairPhoto> RepairPhotos { get; set; }

    public virtual DbSet<RepairStatusHistory> RepairStatusHistories { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VwActiveRepairOrder> VwActiveRepairOrders { get; set; }

    public virtual DbSet<VwRepairOrderSummary> VwRepairOrderSummaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3214EC07DF37C8D2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<CmsContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CmsConte__3214EC0796A0A4EB");

            entity.HasIndex(e => e.Key, "IX_CmsContents_Key");

            entity.HasIndex(e => e.Section, "IX_CmsContents_Section");

            entity.HasIndex(e => e.Key, "UQ__CmsConte__C41E0289FC55CF53").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Section).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(300);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ContactRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactR__3214EC0753C03C7E");

            entity.HasIndex(e => e.IsRead, "IX_ContactRequests_IsRead");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Message).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ResponseNote).HasMaxLength(2000);
            entity.Property(e => e.Subject).HasMaxLength(300);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0707E021A6");

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.HireDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Position).HasMaxLength(50);
        });

        modelBuilder.Entity<RepairOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RepairOr__3214EC074B863D26");

            entity.HasIndex(e => e.ClientId, "IX_RepairOrders_ClientId");

            entity.HasIndex(e => e.CreatedAt, "IX_RepairOrders_CreatedAt").IsDescending();

            entity.HasIndex(e => e.EmployeeId, "IX_RepairOrders_EmployeeId");

            entity.HasIndex(e => e.Status, "IX_RepairOrders_Status");

            entity.HasIndex(e => e.VehicleId, "IX_RepairOrders_VehicleId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Client).WithMany(p => p.RepairOrders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RepairOrders_Clients");

            entity.HasOne(d => d.Employee).WithMany(p => p.RepairOrders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_RepairOrders_Employees");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.RepairOrders)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RepairOrders_Vehicles");
        });

        modelBuilder.Entity<RepairOrderService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RepairOr__3214EC07647E8B9C");

            entity.HasIndex(e => e.RepairOrderId, "IX_ROS_RepairOrderId");

            entity.HasIndex(e => e.ServiceId, "IX_ROS_ServiceId");

            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.RepairOrder).WithMany(p => p.RepairOrderServices)
                .HasForeignKey(d => d.RepairOrderId)
                .HasConstraintName("FK_ROS_RepairOrders");

            entity.HasOne(d => d.Service).WithMany(p => p.RepairOrderServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ROS_Services");
        });

        modelBuilder.Entity<RepairPhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RepairPh__3214EC072BF9B71C");

            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.RepairOrder).WithMany(p => p.RepairPhotos)
                .HasForeignKey(d => d.RepairOrderId)
                .HasConstraintName("FK_RP_RepairOrders");
        });

        modelBuilder.Entity<RepairStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RepairSt__3214EC07F4019DC8");

            entity.ToTable("RepairStatusHistory");

            entity.HasIndex(e => e.RepairOrderId, "IX_RSH_RepairOrderId");

            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ChangedBy).HasMaxLength(200);
            entity.Property(e => e.Comment).HasMaxLength(500);

            entity.HasOne(d => d.RepairOrder).WithMany(p => p.RepairStatusHistories)
                .HasForeignKey(d => d.RepairOrderId)
                .HasConstraintName("FK_RSH_RepairOrders");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Services__3214EC078990C1F2");

            entity.HasIndex(e => e.ServiceCategoryId, "IX_Services_CategoryId");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.EstimatedPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.ServiceCategory).WithMany(p => p.Services)
                .HasForeignKey(d => d.ServiceCategoryId)
                .HasConstraintName("FK_Services_Categories");
        });

        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceC__3214EC073A45C6AC");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IconCss).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Testimon__3214EC078E3EF381");

            entity.Property(e => e.ClientName).HasMaxLength(200);
            entity.Property(e => e.Content).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vehicles__3214EC075DF8007E");

            entity.HasIndex(e => e.ClientId, "IX_Vehicles_ClientId");

            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.EngineType)
                .HasMaxLength(50)
                .HasDefaultValue(EngineType.Benzyna)
                .HasConversion<string>();
            entity.Property(e => e.LicensePlate).HasMaxLength(20);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.Vin)
                .HasMaxLength(17)
                .HasColumnName("VIN");

            entity.HasOne(d => d.Client).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Vehicles_Clients");
        });

        modelBuilder.Entity<VwActiveRepairOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ActiveRepairOrders");

            entity.Property(e => e.ClientName).HasMaxLength(201);
            entity.Property(e => e.ClientPhone).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.LicensePlate).HasMaxLength(20);
            entity.Property(e => e.MechanicName).HasMaxLength(201);
            entity.Property(e => e.StatusName)
                .HasMaxLength(21)
                .IsUnicode(false);
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VehicleName).HasMaxLength(201);
        });

        modelBuilder.Entity<VwRepairOrderSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_RepairOrderSummary");

            entity.Property(e => e.CategoryName).HasMaxLength(200);
            entity.Property(e => e.LineTotal).HasColumnType("decimal(21, 2)");
            entity.Property(e => e.ServiceName).HasMaxLength(200);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}