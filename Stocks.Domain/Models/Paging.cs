namespace Stocks.Domain.Models;

public class Paging
{
    public required int Page { get; set; }
    public required int Pages { get; set; }
    public required int Items { get; set; }
}