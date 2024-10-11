namespace Stocks.Domain.Models;

public record UpdateWebSocketMessage
{
    public Guid? Id { get; set; }
    public double AskPrice { get; set; }
    public double BidPrice { get; set; }
    public double LastPrice { get; set; }
    public DateTime? AskUpdated { get; set; }
    public DateTime? BidUpdated { get; set; }
    public DateTime? LastUpdated { get; set; }
}