namespace CartTdd.Domain.CartAggregate;

public class CartCoupon
{
    public string Code { get; private set; }
    public decimal Amount { get; private set; }

    internal CartCoupon(string code, decimal amount)
    {
        Code = code;
        Amount = amount;
    }
}