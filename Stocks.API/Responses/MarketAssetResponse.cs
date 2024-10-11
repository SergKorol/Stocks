namespace Stocks.API.Responses;

public record MarketAssetResponse
{
    public Guid Id { get; set; }
    public string Symbol { get; set; }
    public string Kind { get; set; }
    public string Exchange { get; set; }
    public string Description { get; set; }
    public decimal TickSize { get; set; }
    public string Currency { get; set; }
    
    public double AskPrice { get; set; }
    public double BidPrice { get; set; }
    public double LastPrice { get; set; }
    public DateTime? AskUpdated { get; set; }
    public DateTime? BidUpdated { get; set; }
    public DateTime? LastUpdated { get; set; }
}