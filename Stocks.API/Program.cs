using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Stocks.API.Middleware;
using Stocks.Application;
using Stocks.Database;
using Stocks.Domain;
using Stocks.HTTP;
using Stocks.WebSockets;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddWebSocketServices(builder.Configuration);
builder.Services.AddHttpServices(builder.Configuration);
builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Stocks.API",
        Version = "v1",
        Description = "Stocks test API"
    });
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MarketContext>();
    await context.Database.MigrateAsync();
}
app.UseMiddleware<MarketAssetsMiddleware>();
app
    .UseSwagger(o =>
    {
        o.RouteTemplate = "swagger/{documentName}/docs.json";
    })
    .UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint("/swagger/v1/docs.json", "Stocks API V1");
        c.DocExpansion(DocExpansion.None);
    });


app.UseHttpsRedirection();
app.MapControllers();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }

    await next();
});
app.Run();