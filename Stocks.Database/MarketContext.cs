using Microsoft.EntityFrameworkCore;
using Stocks.Database.Entities;

namespace Stocks.Database;

public sealed class MarketContext(DbContextOptions<MarketContext> options) : DbContext(options)
{
    public DbSet<MarketAssetEntity> MarketAssets { get; set; }
    public DbSet<MarketPriceChangeEntity> MarketPrices { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MarketPriceChangeEntity>()
            .Property(p => p.AskUpdated)
            .HasConversion(
                src => src.Value.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src.Value, DateTimeKind.Utc),
                dst => dst.Value.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst.Value, DateTimeKind.Utc)
            );
        
        builder.Entity<MarketPriceChangeEntity>()
            .Property(p => p.BidUpdated)
            .HasConversion(
                src => src.Value.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src.Value, DateTimeKind.Utc),
                dst => dst.Value.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst.Value, DateTimeKind.Utc)
            );
        
        builder.Entity<MarketPriceChangeEntity>()
            .Property(p => p.LastUpdated)
            .HasConversion(
                src => src.Value.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src.Value, DateTimeKind.Utc),
                dst => dst.Value.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst.Value, DateTimeKind.Utc)
            );
        
        builder.Entity<MarketAssetEntity>()
            .HasOne(asset => asset.MarketPrice)
            .WithOne(price => price.MarketAsset)
            .HasForeignKey<MarketPriceChangeEntity>(price => price.Id)
            .IsRequired(false);
    }
}