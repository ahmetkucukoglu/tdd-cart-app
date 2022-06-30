namespace CartTdd.Domain.CartAggregate;

public class CartProductIsNotFoundException : Exception
{
    public CartProductIsNotFoundException() : base("Product isn't found.") { }
}