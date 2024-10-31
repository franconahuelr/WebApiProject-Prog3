using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiProject.Models.Entities;

namespace WebApiProject.Models.Context;

public partial class DbApiProjectContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbApiProjectContext()
    {
    }

    public DbApiProjectContext(DbContextOptions<DbApiProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración para la entidad Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Product__2E8946D48FB3A361");

            entity.ToTable("Product");

            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        // Configuración para la entidad ShoppingCart
        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.IdCart).HasName("PK__ShoppingCart__123456");

            entity.ToTable("ShoppingCart");

            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(450); // Longitud típica para UserId en Identity

            // Configuración de la relación con CartItems
            entity.HasMany(sc => sc.CartItems)
                .WithOne(ci => ci.ShoppingCart)
                .OnDelete(DeleteBehavior.Cascade); 
        });

        // Configuración para la entidad CartItem
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.IdCartItem); 

            entity.HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade); 
        });

    }
}
