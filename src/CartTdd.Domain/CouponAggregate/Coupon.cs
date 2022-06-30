using CartTdd.Domain.CartAggregate;

namespace CartTdd.Domain.CouponAggregate;

public class Coupon
{
    public Guid Id { get; private set; }
    public string Code { get; private set; }
    public DateTime ExpireDate { get; private set; }
    public decimal Amount { get; private set; }

    public Coupon(string code, DateTime expireDate, decimal amount)
    {
        Id = Guid.NewGuid();
        Code = code;
        ExpireDate = expireDate;
        Amount = amount;
    }

    internal void ApplyTo(Cart cart)
    {
        if (ExpireDate < DateTime.Now) throw new CouponHasExpiredException();

        cart.ApplyCoupon(new CartCoupon(Code, Amount));
    }
}