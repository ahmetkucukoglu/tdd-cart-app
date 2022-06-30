using System;
using System.Net.Http;
using CartTdd.Domain.CouponAggregate;
using CartTdd.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace CartTdd.Api.Tests;

public class CartE2ETestsFixture : IDisposable
{
    private readonly CartApplication _application;
    public readonly HttpClient HttpClient;

    public Guid CartId { get; } = Guid.NewGuid();

    public CartE2ETestsFixture()
    {
        _application = new CartApplication();

        HttpClient = _application.CreateClient();

        SeedDatabase();
    }

    public void Dispose()
    {
        HttpClient.Dispose();
        
        DropDatabase();

        _application.Dispose();
    }

    private void SeedDatabase()
    {
        using var scope = _application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

        dbContext.Coupons.InsertOne(new Coupon("COUPON100", DateTime.Now.AddDays(1), 100M));
        dbContext.Coupons.InsertOne(new Coupon("EXPIRED_COUPON100", DateTime.Now.AddDays(-1), 100M));
    }
    
    private void DropDatabase()
    {
        using var scope = _application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        dbContext.Client.DropDatabase(dbContext.DatabaseName);
    }
}