using Microsoft.EntityFrameworkCore;
using Stocks.Database.Entities;
using Stocks.Domain.Models;

namespace Stocks.Database.Queries;

public static class QueryMarketAssetsExtention
{
    public static IQueryable<MarketAssetEntity> GetQueryableAssets(
        this IQueryable<MarketAssetEntity> query, List<string>? includes, Filters? filters)
    {
        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (filters == null) return query;

        if (!string.IsNullOrEmpty(filters.AssetId) && Guid.TryParse(filters.AssetId, out Guid assetGuid))
        {
            query = query.Where(x => x.Id == assetGuid);
        }

        if (!string.IsNullOrEmpty(filters.Kind))
        {
            query = query.Where(x => x.Kind == filters.Kind.Trim());
        }

        if (!string.IsNullOrEmpty(filters.Exchange))
        {
            query = query.Where(x => x.Exchange == filters.Exchange.Trim());
        }

        if (!string.IsNullOrEmpty(filters.Symbol))
        {
            query = query.Where(x => x.Symbol == filters.Symbol.Trim());
        }

        if (!string.IsNullOrEmpty(filters.TickChange) && decimal.TryParse(filters.TickChange, out decimal tickChange))
        {
            query = query.Where(x => x.TickSize == tickChange);
        }

        if (!string.IsNullOrEmpty(filters.Asc))
        {
            query = query.OrderBy(x => EF.Property<object>(x, filters.Asc.Trim()));
        }
        else if (!string.IsNullOrEmpty(filters.Desc))
        {
            query = query.OrderByDescending(x => EF.Property<object>(x, filters.Desc.Trim()));
        }

        return query;
    }

}