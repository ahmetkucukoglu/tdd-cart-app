namespace CartTdd.Domain.CartAggregate;

public class CartProduct
{
    public string Sku { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal TotalPrice => Quantity * Price;

    public CartProduct(string sku, int quantity, decimal price)
    {
        Sku = sku;
        Quantity = quantity;
        Price = price;
    }

    internal void Increase() => Quantity++;
    internal void Decrease() => Quantity--;
}