using System;
using CartTdd.Domain.CartAggregate;
using CartTdd.Domain.CouponAggregate;
using Xunit;

namespace CartTdd.Domain.Tests.CouponApplierTests;

public class CouponApplierIntegrationTests : IClassFixture<CouponApplierIntegrationTestsFixture>
{
    private readonly Cart _cart;
    private readonly CouponApplierIntegrationTestsFixture _fixture;

    public CouponApplierIntegrationTests(CouponApplierIntegrationTestsFixture fixture)
    {
        _fixture = fixture;

        _cart = new Cart(Guid.NewGuid());
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));
    }

    [Fact]
    public async void Should_Succeed_When_ApplyCoupon_If_CouponIsValid()
    {
        await _fixture.CouponApplier.ApplyAsync("COUPON100", _cart);

        Assert.Equal(400M, _cart.TotalPrice);
        Assert.NotNull(_cart.Coupon);
        Assert.Equal("COUPON100", _cart.Coupon?.Code);
        Assert.Equal(100M, _cart.Coupon?.Amount);
    }

    [Fact]
    public async void Should_ThrowException_When_ApplyCoupon_If_CouponIsNotFound()
    {
        var exception = await Assert.ThrowsAsync<CouponIsNotFoundException>(async () =>
            await _fixture.CouponApplier.ApplyAsync("COUPON200", _cart));

        Assert.Equal(500M, _cart.TotalPrice);
        Assert.Null(_cart.Coupon);
        Assert.Equal("Coupon isn't found.", exception.Message);
    }

    [Fact]
    public async void Should_ThrowException_When_ApplyCoupon_If_CouponHasExpired()
    {
        var exception = await Assert.ThrowsAsync<CouponHasExpiredException>(async () =>
            await _fixture.CouponApplier.ApplyAsync("EXPIRED_COUPON100", _cart));

        Assert.Equal(500M, _cart.TotalPrice);
        Assert.Null(_cart.Coupon);
        Assert.Equal("Coupon has expired.", exception.Message);
    }
}