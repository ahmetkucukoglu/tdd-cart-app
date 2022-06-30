namespace CartTdd.Domain.CouponAggregate;

public interface ICouponRepository
{
    Task<Coupon?> GetAsync(string code);
}