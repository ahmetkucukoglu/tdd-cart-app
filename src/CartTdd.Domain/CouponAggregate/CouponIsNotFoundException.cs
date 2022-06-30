namespace CartTdd.Domain.CouponAggregate;

public class CouponIsNotFoundException : Exception
{
    public CouponIsNotFoundException() : base("Coupon isn't found.")
    {
    }
}