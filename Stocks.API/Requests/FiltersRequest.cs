namespace Stocks.API.Requests;

public record FiltersRequest
{
    public string? AssetId { get; set; }
    public string? Asc { get; set; }
    public string? Desc { get; set; }
    public string? Kind { get; set; }
    public string? Symbol { get; set; }
    public string? Exchange { get; set; }
    public string? TickChange { get; set; }
}