using MediatR;

namespace CartTdd.Application.Cart;

public record CreateCart : IRequest<CreateCartResponse>
{
    public Guid Id { get; init; }
    public IEnumerable<CreateCartProductResponse> Products { get; init; }
}

public record CreateCartProductResponse
{
    public string Sku { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
}

public record CreateCartResponse;