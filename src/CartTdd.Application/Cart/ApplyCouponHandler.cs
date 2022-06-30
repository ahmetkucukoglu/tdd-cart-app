using CartTdd.Domain.CartAggregate;
using CartTdd.Domain.CouponAggregate;
using MediatR;

namespace CartTdd.Application.Cart;

public class ApplyCouponHandler : IRequestHandler<ApplyCoupon, ApplyCouponResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly CouponApplier _couponApplier;

    public ApplyCouponHandler(
        ICartRepository cartRepository,
        CouponApplier couponApplier)
    {
        _cartRepository = cartRepository;
        _couponApplier = couponApplier;
    }

    public async Task<ApplyCouponResponse> Handle(ApplyCoupon request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(request.CartId);

        if (cart == null) throw new CartIsNotFoundException();

        await _couponApplier.ApplyAsync(request.CouponCode, cart);
        await _cartRepository.UpdateAsync(cart);

        return new ApplyCouponResponse();
    }
}