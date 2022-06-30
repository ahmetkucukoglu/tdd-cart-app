using MediatR;

namespace CartTdd.Application.Cart;

public record ApplyCoupon(Guid CartId, string CouponCode) : IRequest<ApplyCouponResponse>;

public record ApplyCouponResponse;