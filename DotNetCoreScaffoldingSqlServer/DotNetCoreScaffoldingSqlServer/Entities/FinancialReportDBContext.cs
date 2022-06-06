using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class FinancialReportDBContext : DbContext
    {
        public FinancialReportDBContext()
        {
        }

        public FinancialReportDBContext(DbContextOptions<FinancialReportDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CommonCode> CommonCodes { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<DocumentCategory> DocumentCategories { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Header> Headers { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuPermission> MenuPermissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleAssign> RoleAssigns { get; set; }
        public virtual DbSet<RoleWiseMenuAssign> RoleWiseMenuAssigns { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSession> UserSessions { get; set; }
        public virtual DbSet<UserWiseUnitPermission> UserWiseUnitPermissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ANWAR-PMS,1433;Database=FinancialReportDB;User Id=sa; Password=has@321;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CommonCode>(entity =>
            {
                entity.ToTable("CommonCode", "Master");

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.NameBangla).HasMaxLength(200);

                entity.Property(e => e.NameEnglish).HasMaxLength(200);

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Type).HasMaxLength(200);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.CommonCodes)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_DepartmentCommonCode");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department", "Master");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.NameBangla).HasMaxLength(200);

                entity.Property(e => e.NameEnglish).HasMaxLength(200);

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupDepartment");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_UnitDepartment");
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.ToTable("Designation", "Master");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.NameBangla).HasMaxLength(200);

                entity.Property(e => e.NameEnglish).HasMaxLength(200);

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Designations)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_DepartmentDesignation");
            });

            modelBuilder.Entity<DocumentCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("DocumentCategory", "Master");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group", "Master");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.NameBangla).HasMaxLength(200);

                entity.Property(e => e.NameEnglish).HasMaxLength(200);

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Header>(entity =>
            {
                entity.ToTable("Header", "Master");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HeaderCode).HasMaxLength(200);

                entity.Property(e => e.HeaderName).HasMaxLength(200);

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Remarks).HasMaxLength(2000);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.HeaderType)
                    .WithMany(p => p.Headers)
                    .HasForeignKey(d => d.HeaderTypeId)
                    .HasConstraintName("FK_HeaderTypeId");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu", "Auth");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MenuIcon).HasMaxLength(1000);

                entity.Property(e => e.MenuLocation).HasMaxLength(1000);

                entity.Property(e => e.MenuName).HasMaxLength(200);

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ReportPath).HasMaxLength(1000);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.MenuType)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.MenuTypeId)
                    .HasConstraintName("FK_CommonCodeMenuType");
            });

            modelBuilder.Entity<MenuPermission>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MenuPermission");

                entity.Property(e => e.AuthYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DeleteYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.EditYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.MenuIcon).HasMaxLength(1000);

                entity.Property(e => e.MenuLocation).HasMaxLength(1000);

                entity.Property(e => e.MenuName).HasMaxLength(200);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ViewDetailYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "Auth");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Purpose).HasMaxLength(1000);

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RoleName).HasMaxLength(200);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RoleAssign>(entity =>
            {
                entity.ToTable("RoleAssign", "Auth");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleAssigns)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RoleToRoleAssign");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RoleAssigns)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRoleAssign");
            });

            modelBuilder.Entity<RoleWiseMenuAssign>(entity =>
            {
                entity.ToTable("RoleWiseMenuAssign", "Auth");

                entity.Property(e => e.AuthYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeleteYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.EditYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.ViewDetailYn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.RoleWiseMenuAssigns)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_MenuRoleWiseMenuAssign");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleWiseMenuAssigns)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RoleToRoleWiseMenuAssign");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction", "Master");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Remarks).HasMaxLength(2000);

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Value).HasColumnType("decimal(38, 3)");

                entity.HasOne(d => d.FirstHeader)
                    .WithMany(p => p.TransactionFirstHeaders)
                    .HasForeignKey(d => d.FirstHeaderId)
                    .HasConstraintName("FK_HeaderTransaction");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupTransaction");

                entity.HasOne(d => d.HeaderType)
                    .WithMany(p => p.TransactionHeaderTypes)
                    .HasForeignKey(d => d.HeaderTypeId)
                    .HasConstraintName("FK_CommonCodeHeaderType");

                entity.HasOne(d => d.SecondHeader)
                    .WithMany(p => p.TransactionSecondHeaders)
                    .HasForeignKey(d => d.SecondHeaderId)
                    .HasConstraintName("FK_SecondHeaderTransaction");

                entity.HasOne(d => d.ThirdHeader)
                    .WithMany(p => p.TransactionThirdHeaders)
                    .HasForeignKey(d => d.ThirdHeaderId)
                    .HasConstraintName("FK_ThirdHeaderTransaction");

                entity.HasOne(d => d.TransactionType)
                    .WithMany(p => p.TransactionTransactionTypes)
                    .HasForeignKey(d => d.TransactionTypeId)
                    .HasConstraintName("FK_CommonCodeTransType");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_UnitTransaction");

                entity.HasOne(d => d.Version)
                    .WithMany(p => p.TransactionVersions)
                    .HasForeignKey(d => d.VersionId)
                    .HasConstraintName("FK_CommonCodeVersion");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Unit", "Master");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.NameBangla).HasMaxLength(200);

                entity.Property(e => e.NameEnglish).HasMaxLength(200);

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupUnit");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "Auth");

                entity.HasIndex(e => e.UserId, "UKey_UserId")
                    .IsUnique();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EmployeeId).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(1000);

                entity.Property(e => e.MobileNo).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(1000);

                entity.Property(e => e.PasswordSalt).HasMaxLength(1000);

                entity.Property(e => e.RefreshToken).HasMaxLength(1000);

                entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_DepartmentUser");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK_DesignationUser");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupUser");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_UnitUser");
            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("PK_UserSession_SessionId");

                entity.ToTable("UserSession", "Auth");

                entity.Property(e => e.IpAddress).HasMaxLength(200);

                entity.Property(e => e.LoginDateTime).HasColumnType("datetime");

                entity.Property(e => e.LogoutDateTime).HasColumnType("datetime");

                entity.Property(e => e.MacAddress).HasMaxLength(200);

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSessions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserToUserSession");
            });

            modelBuilder.Entity<UserWiseUnitPermission>(entity =>
            {
                entity.HasKey(e => e.UnitPermissionId)
                    .HasName("PK_UserWiseUnitPermission_UnitPermissionId");

                entity.ToTable("UserWiseUnitPermission", "Auth");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Recstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.UserWiseUnitPermissions)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_UserWiseUnitPermissionUnit");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserWiseUnitPermissions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserToWiseUnitPermission");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
