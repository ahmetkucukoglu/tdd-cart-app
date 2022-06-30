using System;
using CartTdd.Domain.CouponAggregate;
using CartTdd.Infrastructure.CouponAggregate;
using CartTdd.Infrastructure.Database;
using Microsoft.Extensions.Options;

namespace CartTdd.Domain.Tests.CouponApplierTests;

public class CouponApplierIntegrationTestsFixture : IDisposable
{
    private readonly DbContext _dbContext;
    public readonly CouponApplier CouponApplier;

    public CouponApplierIntegrationTestsFixture()
    {
        _dbContext = new DbContext(Options.Create(new DbSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = $"cart-tdd-{Guid.NewGuid()}"
        }));

        _dbContext.Coupons.InsertOne(new Coupon("COUPON100", DateTime.Now.AddDays(1), 100M));
        _dbContext.Coupons.InsertOne(new Coupon("EXPIRED_COUPON100", DateTime.Now.AddDays(-1), 100M));

        CouponApplier = new CouponApplier(new CouponRepository(_dbContext));
    }

    public void Dispose() => _dbContext.Client.DropDatabase(_dbContext.DatabaseName);
}