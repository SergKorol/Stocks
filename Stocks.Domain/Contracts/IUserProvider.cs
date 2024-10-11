namespace Stocks.Domain.Contracts;

public interface IUserProvider
{
    Task SetUserToken(string? username, string? password);
}