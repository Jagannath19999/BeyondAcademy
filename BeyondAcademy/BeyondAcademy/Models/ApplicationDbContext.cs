using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BeyondAcademy.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AccountRole> AccountRoles { get; set; } = null!;
        public virtual DbSet<Registration> Registrations { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AcId)
                    .HasName("PK_Account_7EFCCBA73CA59D8E");

                entity.ToTable("Account");

                entity.Property(e => e.AcId).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.UserId).HasMaxLength(255);
            });

            modelBuilder.Entity<AccountRole>(entity =>
            {
                entity.HasKey(e => e.Arid)
                    .HasName("PK__AccountR__4CB5C49B8BC75438");

                entity.ToTable("AccountRole");

                entity.Property(e => e.Arid)
                    .ValueGeneratedNever()
                    .HasColumnName("ARId");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => e.RegdId)
                    .HasName("PK_Registra_D2B0D7AF4397252C");

                entity.ToTable("Registration");

                entity.Property(e => e.RegdId).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(255);

                entity.Property(e => e.LastName).HasMaxLength(12);

                entity.Property(e => e.MobileNo).HasMaxLength(15);

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(255);

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
