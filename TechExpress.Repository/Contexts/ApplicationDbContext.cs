using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository.Models;

namespace TechExpress.Repository.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // db-schema for User model
            modelBuilder.Entity<User>(user =>
            {
                user.Property(u => u.Id)
                    .HasColumnName("id");

                user.Property(u => u.Email)
                    .HasColumnName("email")
                    .HasMaxLength(256)
                    .IsRequired();

                user.Property(u => u.PasswordHash)
                    .HasColumnName("password_hash")
                    .HasMaxLength(256)
                    .IsRequired();

                user.Property(u => u.Role)
                    .HasColumnName("role")
                    .HasConversion<string>()
                    .IsRequired();

                user.Property(u => u.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(256);

                user.Property(u => u.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(256);

                user.Property(u => u.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(20);

                user.Property(u => u.Gender)
                    .HasColumnName("gender")
                    .HasConversion<string>();

                user.Property(u => u.Address)
                    .HasColumnName("address")
                    .HasMaxLength(256);

                user.Property(u => u.Ward)
                    .HasColumnName("ward")
                    .HasMaxLength(100);

                user.Property(u => u.Province)
                    .HasColumnName("province")
                    .HasMaxLength(100);

                user.Property(u => u.PostalCode)
                    .HasColumnName("postal_code")
                    .HasMaxLength(20);

                user.Property(u => u.AvatarImage)
                    .HasColumnName("avatar_image");

                user.Property(u => u.Status)
                    .HasColumnName("status")
                    .HasConversion<string>();

                user.Property(u => u.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();

                user.Property(u => u.Identity)
                    .HasColumnName("identity")
                    .HasMaxLength(20);

                user.Property(u => u.Salary)
                    .HasColumnName("salary")
                    .HasPrecision(18, 2);

                user.HasKey(u => u.Id);

                user.HasIndex(u => u.Email)
                    .HasDatabaseName("idx_user_email")
                    .IsUnique();

                user.HasIndex(u => u.Phone)
                    .HasDatabaseName("idx_user_phone")
                    .IsUnique();

                user.HasIndex(u => u.Identity)
                    .HasDatabaseName("idx_user_identity")
                    .IsUnique();
            });
        }

        public DbSet<User> Users { get; set; }
    }
}
