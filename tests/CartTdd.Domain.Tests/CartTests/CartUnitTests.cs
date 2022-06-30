using System;
using System.Linq;
using CartTdd.Domain.CartAggregate;
using Xunit;

namespace CartTdd.Domain.Tests.CartTests;

public class CartUnitTests
{
    private readonly Cart _cart;

    public CartUnitTests()
    {
        _cart = new Cart(Guid.NewGuid());
    }

    [Fact]
    public void Should_BeEmptyCart_When_CreateCart()
    {
        Assert.Equal(0, _cart.TotalPrice);
        Assert.Empty(_cart.Products);
    }

    [Fact]
    public void Should_Succeed_When_AddProduct()
    {
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));

        Assert.Equal(500, _cart.TotalPrice);
        Assert.Equal(2, _cart.Products.Count);

        var product1 = _cart.Products.ElementAt(0);

        Assert.Equal("SKU_1", product1.Sku);
        Assert.Equal(1, product1.Quantity);
        Assert.Equal(100, product1.Price);

        var product2 = _cart.Products.ElementAt(1);

        Assert.Equal("SKU_2", product2.Sku);
        Assert.Equal(2, product2.Quantity);
        Assert.Equal(200, product2.Price);
    }

    [Fact]
    public void Should_Succeed_When_RemoveProduct()
    {
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));

        _cart.RemoveProduct("SKU_2");

        Assert.Equal(100, _cart.TotalPrice);
        Assert.Single(_cart.Products);
    }

    [Fact]
    public void Should_ThrowException_When_RemoveProduct_If_ProductIsNotFound()
    {
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));

        var exception = Assert.Throws<CartProductIsNotFoundException>(() => _cart.RemoveProduct("SKU_3"));

        Assert.Equal("Product isn't found.", exception.Message);
    }

    [Fact]
    public void Should_Succeed_When_IncreaseProductQuantity()
    {
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));

        _cart.IncreaseProductQuantity("SKU_2");

        var product2 = _cart.Products.FirstOrDefault(p => p.Sku == "SKU_2");

        Assert.Equal(700, _cart.TotalPrice);
        Assert.Equal(3, product2?.Quantity);
    }

    [Fact]
    public void Should_ThrowException_When_IncreaseProductQuantity_If_ProductIsNotFound()
    {
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));

        var exception = Assert.Throws<CartProductIsNotFoundException>(() => _cart.IncreaseProductQuantity("SKU_3"));

        Assert.Equal("Product isn't found.", exception.Message);
    }

    [Fact]
    public void Should_Succeed_When_DecreaseProductQuantity()
    {
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));

        _cart.DecreaseProductQuantity("SKU_2");

        var product2 = _cart.Products.FirstOrDefault(p => p.Sku == "SKU_2");

        Assert.Equal(300, _cart.TotalPrice);
        Assert.Equal(1, product2?.Quantity);
        Assert.Equal(2, _cart.Products.Count);
    }

    [Fact]
    public void Should_ThrowException_When_DecreaseProductQuantity_If_ProductIsNotFound()
    {
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));

        var exception = Assert.Throws<CartProductIsNotFoundException>(() => _cart.DecreaseProductQuantity("SKU_3"));

        Assert.Equal("Product isn't found.", exception.Message);
    }

    [Fact]
    public void Should_RemoveItem_When_DecreaseProductQuantity_If_ProductQuantityIsOne()
    {
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));

        _cart.DecreaseProductQuantity("SKU_1");

        Assert.Equal(400, _cart.TotalPrice);
        Assert.Single(_cart.Products);
    }

    [Fact]
    public void Should_Succeed_When_ClearProducts()
    {
        _cart.AddProduct(new CartProduct("SKU_1", 1, 100M));
        _cart.AddProduct(new CartProduct("SKU_2", 2, 200M));

        _cart.ClearProducts();

        Assert.Equal(0, _cart.TotalPrice);
        Assert.Empty(_cart.Products);
    }
}