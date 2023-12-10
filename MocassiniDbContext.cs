using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Mocassini.Models;

namespace Mocassini;

public class MocassiniDbContext : DbContext
{
    public DbSet<BrandEntity> Brands { get; set; } = null!;
    public DbSet<ShoeEntity> Shoes { get; set; } = null!;
    public DbSet<ShoeVarietyEntity> ShoeVarieties { get; set; } = null!;
    public DbSet<ShoeVarietyPhotoEntity> ShoeVarietyPhotos { get; set; } = null!;
    public DbSet<OrderEntity> Orders { get; set; } = null!;
    public DbSet<OrderItemEntity> OrderItems { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;

    public MocassiniDbContext(DbContextOptions<MocassiniDbContext> options) : base(options)
    {
    }

    // make addressVO a value object
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoeEntity>()
            .HasOne(s => s.Brand)
            .WithMany(b => b.Shoes)
            .HasForeignKey(s => s.BrandId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShoeVarietyEntity>()
            .HasOne(sv => sv.Shoe)
            .WithMany(s => s.ShoeVarieties)
            .HasForeignKey(sv => sv.ShoeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ShoeVarietyPhotoEntity>()
            .HasOne(svp => svp.ShoeVariety)
            .WithMany(sv => sv.ShoeVarietyPhotos)
            .HasForeignKey(svp => svp.ShoeVarietyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<OrderItemEntity>()
            .HasOne(oi => oi.ShoeVariety)
            .WithMany(si => si.OrderItems)
            .HasForeignKey(oi => oi.ShoeVarietyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<OrderItemEntity>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderEntity>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<OrderEntity>()
            .Property(o => o.Address)
            .HasConversion(
                a => JsonSerializer.Serialize(a,
                    new JsonSerializerOptions { IgnoreNullValues = true }
                ),
                a => JsonSerializer.Deserialize<AddressVO>(a,
                    new JsonSerializerOptions { IgnoreNullValues = true }
                )
            );

        modelBuilder.Entity<OrderEntity>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);
        
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}