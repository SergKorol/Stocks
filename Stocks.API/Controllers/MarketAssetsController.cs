using Mapster;
using Microsoft.AspNetCore.Mvc;
using Stocks.API.Requests;
using Stocks.Domain.Contracts;
using Stocks.Domain.Models;

namespace Stocks.API.Controllers;

[Route("api/v1/market-assets")]
public class MarketAssetsController(IMarketAssetRepository marketAssetRepository) : ControllerBase
{
    [HttpGet]
    [Route("assets")]
    public async Task<IActionResult> GetMarketAssets(FiltersRequest request)
    {

        var assets =  await marketAssetRepository.GetFilteredMarketAssetsAsync(request.Adapt<Filters>());
        return Ok(assets);
    }
}