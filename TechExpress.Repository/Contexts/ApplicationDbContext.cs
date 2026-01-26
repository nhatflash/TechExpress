using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
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
            

            // db-schema for Category model
            modelBuilder.Entity<Category>(ct =>
            {
                ct.Property(c => c.Id)
                    .HasColumnName("id");

                ct.Property(c => c.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired();
                
                ct.Property(c => c.ParentCategoryId)
                    .HasColumnName("parent_category_id");

                ct.Property(c => c.Description)
                    .HasColumnName("description")
                    .HasMaxLength(4096)
                    .IsRequired();

                ct.Property(c => c.ImageUrl)
                    .HasColumnName("image_url")
                    .HasMaxLength(2048);

                ct.Property(c => c.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValue(false)
                    .IsRequired();

                ct.Property(c => c.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();

                ct.Property(c => c.UpdatedAt)
                    .HasColumnName("updated_at")
                    .IsRequired();

                ct.HasKey(c => c.Id);

                ct.HasIndex(c => c.Name)
                    .HasDatabaseName("idx_category_name")
                    .IsUnique();

                ct.HasOne(c => c.ParentCategory)
                    .WithMany()
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // db-schema for SpecDefinition model
            modelBuilder.Entity<SpecDefinition>(sd =>
            {
                sd.Property(s => s.Id)
                    .HasColumnName("id");

                sd.Property(s => s.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired();

                sd.Property(s => s.CategoryId)
                    .HasColumnName("category_id")
                    .IsRequired();

                sd.Property(s => s.Unit)
                    .HasColumnName("unit")
                    .HasMaxLength(20)
                    .IsRequired();

                sd.Property(s => s.AcceptValueType)
                    .HasColumnName("accept_value_type")
                    .HasConversion<string>()
                    .IsRequired();

                sd.Property(s => s.Description)
                    .HasColumnName("description")
                    .HasMaxLength(4096)
                    .IsRequired();

                sd.Property(s => s.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValue(false)
                    .IsRequired();

                sd.Property(s => s.IsRequired)
                    .HasColumnName("is_required")
                    .HasDefaultValue(true)
                    .IsRequired();

                sd.Property(s => s.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();

                sd.Property(s => s.UpdatedAt)
                    .HasColumnName("updated_at")
                    .IsRequired();

                sd.HasKey(s => s.Id);

                sd.HasIndex(s => s.Name)
                    .HasDatabaseName("idx_spec_name")
                    .IsUnique();
                
                sd.HasIndex(s => s.CategoryId)
                    .HasDatabaseName("idx_spec_category");

                sd.HasOne(s => s.Category)
                    .WithMany()
                    .HasForeignKey(s => s.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });



            // db-schema for Brand model
            modelBuilder.Entity<Brand>(br =>
            {
                br.Property(b => b.Id)
                    .HasColumnName("id");

                br.Property(b => b.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired();
                
                br.Property(b => b.ImageUrl)
                    .HasColumnName("image_url")
                    .HasMaxLength(2048);

                br.Property(b => b.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();

                br.Property(b => b.UpdatedAt)
                    .HasColumnName("updated_at")
                    .IsRequired();

                br.HasKey(b => b.Id);

                br.HasIndex(b => b.Name)
                    .HasDatabaseName("idx_brand_name")
                    .IsUnique();
            });



            // db-schema for BrandCategory model
            modelBuilder.Entity<BrandCategory>(bc =>
            {
                bc.Property(b => b.Id)
                    .HasColumnName("id");

                bc.Property(b => b.CategoryId)
                    .HasColumnName("category_id")
                    .IsRequired();

                bc.Property(b => b.BrandId)
                    .HasColumnName("brand_id")
                    .IsRequired();

                bc.Property(b => b.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();

                bc.HasKey(b => b.Id);

                bc.HasIndex(b => new { b.CategoryId, b.BrandId })
                    .HasDatabaseName("idx_category_brand")
                    .IsUnique();
                
                bc.HasOne(b => b.Category)
                    .WithMany()
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                bc.HasOne(b => b.Brand)
                    .WithMany()
                    .HasForeignKey(b => b.BrandId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // db-schema for Product model
            modelBuilder.Entity<Product>(pd =>
            {
                pd.Property(p => p.Id)
                    .HasColumnName("id");

                pd.Property(p => p.Name)
                    .HasColumnName("name")
                    .HasMaxLength(256)
                    .IsRequired();

                pd.Property(p => p.Sku)
                    .HasColumnName("sku")
                    .HasMaxLength(256)
                    .IsRequired();

                pd.Property(p => p.CategoryId)
                    .HasColumnName("category_id")
                    .IsRequired();

                
                pd.Property(p => p.BrandId)
                    .HasColumnName("brand_id");

                pd.Property(p => p.Price)
                    .HasColumnName("price")
                    .HasPrecision(18, 2)
                    .IsRequired();

                pd.Property(p => p.Stock)
                    .HasColumnName("stock")
                    .IsRequired();

                pd.Property(p => p.Description)
                    .HasColumnName("description")
                    .HasMaxLength(4096)
                    .IsRequired();

                pd.Property(p => p.Status)
                    .HasColumnName("status")
                    .HasConversion<string>()
                    .IsRequired();

                pd.Property(p => p.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();
                    
                pd.Property(p => p.UpdatedAt)
                    .HasColumnName("updated_at")
                    .IsRequired();

                pd.HasKey(p => p.Id);

                pd.HasIndex(p => p.Name)
                    .HasDatabaseName("idx_product_name")
                    .IsUnique();

                pd.HasIndex(p => p.Sku)
                    .HasDatabaseName("idx_product_sku")
                    .IsUnique();

                pd.HasIndex(p => p.CategoryId)
                    .HasDatabaseName("idx_product_category");

                pd.HasIndex(p => p.BrandId)
                    .HasDatabaseName("idx_product_brand");

                pd.HasOne(p => p.Category)
                    .WithMany()
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                pd.HasOne(p => p.Brand)
                    .WithMany()
                    .HasForeignKey(p => p.BrandId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // db-schema for ProductSpecValue model
            modelBuilder.Entity<ProductSpecValue>(psv =>
            {
                psv.Property(p => p.Id)
                    .HasColumnName("id");

                psv.Property(p => p.ProductId)
                    .HasColumnName("product_id")
                    .IsRequired();

                psv.Property(p => p.SpecDefinitionId)
                    .HasColumnName("spec_definition_id")
                    .IsRequired();

                psv.Property(p => p.TextValue)
                    .HasColumnName("text_value")
                    .HasMaxLength(512);

                psv.Property(p => p.NumberValue)
                    .HasColumnName("number_value");

                psv.Property(p => p.DecimalValue)
                    .HasColumnName("decimal_value")
                    .HasPrecision(18, 2);

                psv.Property(p => p.BoolValue)
                    .HasColumnName("bool_value");
                
                psv.Property(p => p.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();

                psv.Property(p => p.UpdatedAt)
                    .HasColumnName("updated_at")
                    .IsRequired();

                psv.HasKey(p => p.Id);

                psv.HasIndex(p => p.ProductId)
                    .HasDatabaseName("idx_spec_value_product");

                psv.HasIndex(p => p.SpecDefinitionId)
                    .HasDatabaseName("idx_spec_value_definition");
                
                psv.HasOne(p => p.Product)
                    .WithMany(p => p.SpecValues)
                    .HasForeignKey(p => p.SpecDefinitionId)
                    .OnDelete(DeleteBehavior.Cascade);

                psv.HasOne(p => p.SpecDefinition)
                    .WithMany()
                    .HasForeignKey(p => p.SpecDefinitionId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // db-schema for ProductImage model
            modelBuilder.Entity<ProductImage>(pi =>
            {
                pi.Property(p => p.Id)
                    .HasColumnName("id");

                pi.Property(p => p.ProductId)
                    .HasColumnName("product_id")
                    .IsRequired();

                pi.Property(p => p.ImageUrl)
                    .HasColumnName("image_url")
                    .HasMaxLength(2048)
                    .IsRequired();

                pi.Property(p => p.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();

                pi.HasKey(p => p.Id);

                pi.HasIndex(p => p.ProductId)
                    .HasDatabaseName("idx_image_product");
                
                pi.HasOne(p => p.Product)
                    .WithMany(p => p.Images)
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // db-schema for Cart model
            modelBuilder.Entity<Cart>(ct =>
            {
                ct.Property(c => c.Id)
                    .HasColumnName("id");

                ct.Property(c => c.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();
                
                ct.Property(c => c.TotalPrice)
                    .HasColumnName("total_price")
                    .HasPrecision(18, 2)
                    .IsRequired();

                ct.Property(c => c.UpdatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();

                ct.Property(c => c.UpdatedAt)
                    .HasColumnName("updated_at")
                    .IsRequired();

                ct.HasKey(c => c.Id);

                ct.HasOne(c => c.User)
                    .WithOne()
                    .HasForeignKey<Cart>(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // db-schema for CartItem model
            modelBuilder.Entity<CartItem>(ci =>
            {
                ci.Property(c => c.Id)
                    .HasColumnName("id");

                ci.Property(c => c.CartId)
                    .HasColumnName("cart_id")
                    .IsRequired();

                ci.Property(c => c.ProductId)
                    .HasColumnName("product_id")
                    .IsRequired();

                ci.Property(c => c.Quantity)
                    .HasColumnName("quantity")
                    .IsRequired();

                ci.Property(c => c.UnitPrice)
                    .HasColumnName("unit_price")
                    .HasPrecision(18, 2)
                    .IsRequired();

                ci.Property(c => c.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();
                
                ci.Property(c => c.UpdatedAt)
                    .HasColumnName("updated_at")
                    .IsRequired();

                ci.HasKey(c => c.Id);

                ci.HasIndex(c => c.CartId)
                    .HasDatabaseName("idx_cart_item_cart");
                
                ci.HasIndex(c => c.ProductId)
                    .HasDatabaseName("idx_cart_item_product");

                ci.HasOne(c => c.Cart)
                    .WithMany(c => c.Items)
                    .HasForeignKey(c => c.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandCategory> BrandCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSpecValue> ProductSpecValues { get; set; }
        public DbSet<SpecDefinition> SpecDefinitions { get; set; }
    }
}
