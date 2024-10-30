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

    //public virtual DbSet<UserData> UserDatas { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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

       // modelBuilder.Entity<UserData>(entity =>
       // {
        //    entity.HasKey(e => e.IdUser).HasName("PK__UserData__B7C926385C59E4A3");

        //    entity.Property(e => e.Email)
         //       .HasMaxLength(50)
        //        .IsUnicode(false);
       //     entity.Property(e => e.UserName)
       //         .HasMaxLength(50)
      //          .IsUnicode(false);
      //      entity.Property(e => e.Password)
       //         .HasMaxLength(100)
       //         .IsUnicode(false);
     //   });

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId);
      OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
