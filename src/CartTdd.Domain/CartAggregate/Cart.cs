namespace CartTdd.Domain.CartAggregate;

public class Cart
{
    public Guid Id { get; private set; }
    public CartCoupon? Coupon { get; private set; }


    private List<CartProduct> _products = new();
    public IReadOnlyList<CartProduct> Products => _products.AsReadOnly();


    public decimal TotalPrice => Products.Sum(p => p.TotalPrice) - (Coupon?.Amount).GetValueOrDefault(0);

    public Cart(Guid cartId)
    {
        Id = cartId;
    }
    
    public void AddProduct(CartProduct cartProduct)
    {
        ArgumentNullException.ThrowIfNull(cartProduct);
        
        _products.Add(cartProduct);
    }

    public void RemoveProduct(string sku)
    {
        var product = _products.FirstOrDefault(p => p.Sku == sku);

        if (product == null) throw new CartProductIsNotFoundException();

        _products.Remove(product);
    }

    public void IncreaseProductQuantity(string sku)
    {
        var product = _products.FirstOrDefault(p => p.Sku == sku);

        if (product == null) throw new CartProductIsNotFoundException();

        product.Increase();
    }

    public void DecreaseProductQuantity(string sku)
    {
        var product = _products.FirstOrDefault(p => p.Sku == sku);

        if (product == null) throw new CartProductIsNotFoundException();

        if (product.Quantity == 1)
        {
            RemoveProduct(sku);

            return;
        }

        product.Decrease();
    }

    public void ClearProducts() => _products.Clear();

    internal void ApplyCoupon(CartCoupon coupon)
    {
        ArgumentNullException.ThrowIfNull(coupon);

        Coupon = coupon;
    }
}