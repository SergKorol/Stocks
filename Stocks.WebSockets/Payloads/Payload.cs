namespace Stocks.WebSockets.Payloads;

public record Payload
{
    public required string Type { get; init; }
    public required string Id { get; init; }
    public required string InstrumentId { get; init; }
    public required string Provider  { get; init; }
    public required bool Subscribe { get; init; }
    public required string[] Kinds { get; init; }
}