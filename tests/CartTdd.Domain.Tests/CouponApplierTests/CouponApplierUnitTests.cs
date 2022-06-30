using System;
using System.Threading.Tasks;
using CartTdd.Domain.CartAggregate;
using CartTdd.Domain.CouponAggregate;
using Moq;
using Xunit;

namespace CartTdd.Domain.Tests.CouponApplierTests;

public class CouponApplierTests
{
    private readonly Cart _cart;

    public CouponApplierTests()
    {
        _cart = new Cart(Guid.NewGuid());
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));
    }

    [Fact]
    public async void Should_Succeed_When_ApplyCoupon()
    {
        var coupon = new Coupon("COUPON100", DateTime.Now.AddDays(1), 100M);

        var mockCouponRepository = new Mock<ICouponRepository>();
        mockCouponRepository.Setup(s => s.GetAsync(It.IsAny<string>())).Returns(Task.FromResult<Coupon?>(coupon));

        var couponApplier = new CouponApplier(mockCouponRepository.Object);
        await couponApplier.ApplyAsync(coupon.Code, _cart);

        Assert.Equal(400M, _cart.TotalPrice);
        Assert.Equal(coupon.Code, _cart.Coupon?.Code);
        Assert.Equal(coupon.Amount, _cart.Coupon?.Amount);
    }

    [Fact]
    public async void Should_ThrowException_When_ApplyCoupon_If_CouponIsNotFound()
    {
        var mockCouponRepository = new Mock<ICouponRepository>();
        mockCouponRepository.Setup(s => s.GetAsync(It.IsAny<string>())).Returns(Task.FromResult<Coupon?>(null));

        var couponApplier = new CouponApplier(mockCouponRepository.Object);

        var exception =
            await Assert.ThrowsAsync<CouponIsNotFoundException>(async () =>
                await couponApplier.ApplyAsync("COUPON200", _cart));

        Assert.Equal(500M, _cart.TotalPrice);
        Assert.Null(_cart.Coupon);
        Assert.Equal("Coupon isn't found.", exception.Message);
    }

    [Fact]
    public async void Should_ThrowException_When_ApplyCoupon_If_CouponHasExpired()
    {
        var coupon = new Coupon("EXPIRED_COUPON100", DateTime.Now.AddDays(-1), 100M);

        var mockCouponRepository = new Mock<ICouponRepository>();
        mockCouponRepository.Setup(s => s.GetAsync(It.IsAny<string>())).Returns(Task.FromResult<Coupon?>(coupon));

        var couponApplier = new CouponApplier(mockCouponRepository.Object);

        var exception =
            await Assert.ThrowsAsync<CouponHasExpiredException>(async () =>
                await couponApplier.ApplyAsync(coupon.Code, _cart));

        Assert.Equal(500M, _cart.TotalPrice);
        Assert.Null(_cart.Coupon);
        Assert.Equal("Coupon has expired.", exception.Message);
    }
}