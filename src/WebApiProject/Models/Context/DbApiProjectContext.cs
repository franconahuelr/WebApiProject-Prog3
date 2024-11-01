
using Microsoft.EntityFrameworkCore;
using WebApiProject.Models.Entities;

namespace WebApiProject.Models.Context;

public partial class DbApiProjectContext : DbContext
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
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Admin> Admins { get; set; }

    public DbSet<Role> Roles { get; set; }


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
                .HasMaxLength(450);

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
        // Configuración para la entidad User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);
            entity.ToTable("Users");
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Password)
                .IsRequired();
            entity.Property(e => e.RoleId)
                .IsRequired();
               
        });
        // Configuración para la entidad Client
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Clients");
            entity.HasMany(c => c.CartItems)
                  .WithOne()
                  .OnDelete(DeleteBehavior.Cascade); // Eliminar artículos del carrito si se elimina el cliente
        });

        // Configuración para la entidad Admin
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admins");

            entity.Property(e => e.Permissions)
                .HasMaxLength(200);

        });
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Roles");

            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        });

    }
}
