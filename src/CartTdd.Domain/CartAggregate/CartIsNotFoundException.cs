namespace CartTdd.Domain.CartAggregate;

public class CartIsNotFoundException : Exception
{
    public CartIsNotFoundException() : base("Cart isn't found.")
    {
    }
}