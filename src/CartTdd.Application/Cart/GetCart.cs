using MediatR;

namespace CartTdd.Application.Cart;

public record GetCart(Guid Id) : IRequest<GetCartResponse>;

public record GetCartResponse
{
    public Guid Id { get; init; }
    public decimal TotalPrice { get; init; }
    public GetCartCouponResponse? Coupon { get; init; }
    public IEnumerable<GetCartProductResponse> Products { get; init; }
}

public record GetCartCouponResponse
{
    public string Code { get; init; }
    public decimal Amount { get; init; }
}

public record GetCartProductResponse
{
    public string Sku { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public decimal TotalPrice { get; init; }
}