using CartTdd.Domain.CartAggregate;
using MediatR;

namespace CartTdd.Application.Cart;

public class GetCartHandler : IRequestHandler<GetCart, GetCartResponse>
{
    private readonly ICartRepository _cartRepository;

    public GetCartHandler(
        ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<GetCartResponse> Handle(GetCart request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(request.Id);

        if (cart == null) throw new CartIsNotFoundException();

        return new GetCartResponse
        {
            Id = cart.Id,
            TotalPrice = cart.TotalPrice,
            Coupon = cart.Coupon != null
                ? new GetCartCouponResponse {Code = cart.Coupon.Code, Amount = cart.Coupon.Amount}
                : null,
            Products = cart.Products.Select(p => new GetCartProductResponse
            {
                Sku = p.Sku,
                Price = p.Price,
                Quantity = p.Quantity,
                TotalPrice = p.TotalPrice
            })
        };
    }
}