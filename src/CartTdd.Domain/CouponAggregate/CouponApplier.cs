using CartTdd.Domain.CartAggregate;

namespace CartTdd.Domain.CouponAggregate;

public class CouponApplier
{
    private readonly ICouponRepository _couponRepository;

    public CouponApplier(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }
    
    public async Task ApplyAsync(string code, Cart cart)
    {
        var coupon = await _couponRepository.GetAsync(code);

        if (coupon == null) throw new CouponIsNotFoundException();
        
        coupon.ApplyTo(cart);
    }
}