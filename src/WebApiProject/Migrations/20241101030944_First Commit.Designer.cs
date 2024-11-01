﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiProject.Models.Context;

#nullable disable

namespace WebApiProject.Migrations
{
    [DbContext(typeof(DbApiProjectContext))]
    [Migration("20241101030944_First Commit")]
    partial class FirstCommit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.CartItem", b =>
                {
                    b.Property<int>("IdCartItem")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCartItem"));

                    b.Property<string>("ClientIdUser")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ShoppingCartIdCart")
                        .HasColumnType("int");

                    b.HasKey("IdCartItem");

                    b.HasIndex("ClientIdUser");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShoppingCartIdCart");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.Product", b =>
                {
                    b.Property<int>("IdProduct")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProduct"));

                    b.Property<string>("Brand")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("IdProduct")
                        .HasName("PK__Product__2E8946D48FB3A361");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.ShoppingCart", b =>
                {
                    b.Property<int>("IdCart")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCart"));

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdCart")
                        .HasName("PK__ShoppingCart__123456");

                    b.ToTable("ShoppingCart", (string)null);
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.User", b =>
                {
                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdUser");

                    b.HasIndex("RoleId");

                    b.ToTable("Users", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.Admin", b =>
                {
                    b.HasBaseType("WebApiProject.Models.Entities.User");

                    b.Property<string>("Permissions")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.ToTable("Admins", (string)null);
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.Client", b =>
                {
                    b.HasBaseType("WebApiProject.Models.Entities.User");

                    b.ToTable("Clients", (string)null);
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.CartItem", b =>
                {
                    b.HasOne("WebApiProject.Models.Entities.Client", null)
                        .WithMany("CartItems")
                        .HasForeignKey("ClientIdUser")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApiProject.Models.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApiProject.Models.Entities.ShoppingCart", "ShoppingCart")
                        .WithMany("CartItems")
                        .HasForeignKey("ShoppingCartIdCart")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("ShoppingCart");
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.User", b =>
                {
                    b.HasOne("Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.Admin", b =>
                {
                    b.HasOne("WebApiProject.Models.Entities.User", null)
                        .WithOne()
                        .HasForeignKey("WebApiProject.Models.Entities.Admin", "IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.Client", b =>
                {
                    b.HasOne("WebApiProject.Models.Entities.User", null)
                        .WithOne()
                        .HasForeignKey("WebApiProject.Models.Entities.Client", "IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.ShoppingCart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("WebApiProject.Models.Entities.Client", b =>
                {
                    b.Navigation("CartItems");
                });
#pragma warning restore 612, 618
        }
    }
}
