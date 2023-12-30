using Microsoft.EntityFrameworkCore;
using Project_PHE.Entities;

namespace Project_PHE.Data
{
    public class PheDbContext : DbContext
    {
        public PheDbContext(DbContextOptions<PheDbContext> options) : base(options) { }

        public virtual DbSet<Approvel> Approvals { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");


            modelBuilder.Entity<Approvel>(entity =>
            {
                entity.HasKey(e => e.Guid)
                .HasName("PRIMARY");

                entity.Property(e => e.Guid)
               .HasMaxLength(36)
               .HasColumnName("guid");

                entity.ToTable("approvel");

                entity.UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.ApprovalVendor)
                .HasMaxLength(36)
                .HasColumnName("vendor");

                entity.HasOne(d => d.VendorNavigation)
                  .WithOne(p => p.ApprovelNavigation)
                  .HasForeignKey<Approvel>(v => v.Guid)
                  .OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Guid)
                .HasName("PRIMARY");

                entity.Property(e => e.Guid)
               .HasMaxLength(36)
               .HasColumnName("guid");

                entity.ToTable("employee");

                entity.UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");

                entity.Property(e => e.Phone)
                .HasMaxLength(16)
                .HasColumnName("phone");

                entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");

                entity.HasMany(d => d.RoleEmployes)
                   .WithMany(p => p.EmployeeRoles);

            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.Guid)
                .HasName("PRIMARY");

                entity.Property(e => e.Guid)
               .HasMaxLength(36)
               .HasColumnName("guid");

                entity.ToTable("vendor");

                entity.UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.NameCompany)
                .HasMaxLength(255)
                .HasColumnName("name_company");

                entity.Property(e => e.EmailCompany)
                .HasMaxLength(255)
                .HasColumnName("email_company");

                entity.Property(e => e.PhoneCompany)
                .HasMaxLength(16)
                .HasColumnName("phone_company");

                entity.Property(e => e.UploadImage)
                .HasMaxLength(255)
                .HasColumnName("upload_image");

                entity.Property(e => e.IsApproved)
                .HasMaxLength(255)
                .HasColumnName("is_approved");

                // Menambahkan hubungan satu-satu ke entitas Employee
                entity.HasOne(d => d.EmployeeNavigation)
                    .WithOne(p => p.VendorNavigation)
                    .HasForeignKey<Vendor>(v => v.EmployeeVendor)
                    .OnDelete(DeleteBehavior.Restrict);  // Sesuaikan DeleteBehavior sesuai kebutuhan
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PRIMARY");

                entity.Property(e => e.Guid)
                    .HasMaxLength(36)
                    .HasColumnName("guid");

                entity.ToTable("role");

                entity.UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                // Adding predefined roles
                entity.HasData(
                    new Role { Guid = "3fa85f64-5717-4562-b3fc-2c963f66afa6", Name = "user" },
                    new Role { Guid = "6f0cab9c-77ee-4720-fbe7-08db584bbbc0", Name = "admin" },
                    new Role { Guid = "158f7caf-D2AD-45ad-4c30-08db58db1641", Name = "manager" },
                    new Role { Guid = "158f7caf-D2AD-45ad-4c30-48db58db1641", Name = "vendor" }

                );
            });
        }
    }
}
