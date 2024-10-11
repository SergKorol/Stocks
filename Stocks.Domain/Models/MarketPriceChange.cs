namespace Stocks.Domain.Models;

public record MarketPriceChange
{
    public double Price { get; set; }
    public DateTime TimeStamp { get; set; }
}