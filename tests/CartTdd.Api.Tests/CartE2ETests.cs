using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using CartTdd.Application.Cart;
using Xunit;
using Xunit.Priority;

namespace CartTdd.Api.Tests;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class CartE2ETests : IClassFixture<CartE2ETestsFixture>
{
    private readonly CartE2ETestsFixture _fixture;

    public CartE2ETests(CartE2ETestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact, Priority(1)]
    public async void Should_ReturnSuccess_When_CreateCart()
    {
        var request = new CreateCart
        {
            Products = new List<CreateCartProductResponse>
            {
                new() {Sku = "SKU_1", Quantity = 1, Price = 100M},
                new() {Sku = "SKU_1", Quantity = 2, Price = 200M}
            }
        };

        var responseMessage = await _fixture.HttpClient.PostAsJsonAsync($"carts/{_fixture.CartId}", request);

        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }

    [Fact, Priority(2)]
    public async void Should_ReturnSuccess_When_ApplyCoupon()
    {
        var request = new
        {
            couponCode = "COUPON100"
        };

        var responseMessage =
            await _fixture.HttpClient.PutAsJsonAsync($"carts/{_fixture.CartId}/apply-coupon", request);

        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }

    [Fact, Priority(3)]
    public async void Should_ReturnCart_When_GetCart()
    {
        var responseMessage = await _fixture.HttpClient.GetFromJsonAsync<GetCartResponse>($"carts/{_fixture.CartId}");

        Assert.Equal(400M, responseMessage?.TotalPrice);
        Assert.Equal(2, responseMessage?.Products.Count());
        Assert.NotNull(responseMessage?.Coupon);
        Assert.Equal("COUPON100", responseMessage?.Coupon?.Code);
        Assert.Equal(100M, responseMessage?.Coupon?.Amount);
    }

    [Fact]
    public async void Should_ReturnError_When_ApplyCoupon_If_CartIsNotFound()
    {
        var request = new
        {
            couponCode = "COUPON100"
        };

        var responseMessage =
            await _fixture.HttpClient.PutAsJsonAsync($"carts/{Guid.NewGuid()}/apply-coupon", request);
        var response = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.InternalServerError, responseMessage.StatusCode);
        Assert.Equal("Cart isn't found.", response);
    }

    [Fact]
    public async void Should_ReturnError_When_ApplyCoupon_If_CouponIsNotFound()
    {
        var request = new
        {
            couponCode = "COUPON200"
        };

        var responseMessage =
            await _fixture.HttpClient.PutAsJsonAsync($"carts/{_fixture.CartId}/apply-coupon", request);
        var response = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.InternalServerError, responseMessage.StatusCode);
        Assert.Equal("Coupon isn't found.", response);
    }

    [Fact]
    public async void Should_ReturnError_When_ApplyCoupon_If_CouponHasExpired()
    {
        var request = new
        {
            couponCode = "EXPIRED_COUPON100"
        };

        var responseMessage =
            await _fixture.HttpClient.PutAsJsonAsync($"carts/{_fixture.CartId}/apply-coupon", request);
        var response = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.InternalServerError, responseMessage.StatusCode);
        Assert.Equal("Coupon has expired.", response);
    }

    [Fact]
    public async void Should_ReturnError_When_GetCart_If_CartIsNotFound()
    {
        var responseMessage = await _fixture.HttpClient.GetAsync($"carts/{Guid.NewGuid()}");
        var response = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.InternalServerError, responseMessage.StatusCode);
        Assert.Equal("Cart isn't found.", response);
    }
}