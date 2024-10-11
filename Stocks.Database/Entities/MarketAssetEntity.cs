using System.ComponentModel.DataAnnotations;

namespace Stocks.Database.Entities;

public sealed class MarketAssetEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Symbol { get; set; }
    public string Description { get; set; }
    
    public string Kind { get; set; }
    
    public string? Exchange { get; set; }
    
    public decimal? TickSize { get; set; }
    
    public string? Currency { get; set; }

    public MarketPriceChangeEntity? MarketPrice { get; set; }
}