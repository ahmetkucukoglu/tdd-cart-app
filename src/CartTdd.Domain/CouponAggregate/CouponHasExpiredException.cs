namespace CartTdd.Domain.CouponAggregate;

public class CouponHasExpiredException : Exception
{
    public CouponHasExpiredException() : base("Coupon has expired.")
    {
    }
}