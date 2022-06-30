using CartTdd.Domain.CartAggregate;
using MediatR;

namespace CartTdd.Application.Cart;

public class CreateCartHandler : IRequestHandler<CreateCart, CreateCartResponse>
{
    private readonly ICartRepository _cartRepository;

    public CreateCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    
    public async Task<CreateCartResponse> Handle(CreateCart request, CancellationToken cancellationToken)
    {
        var cart = new Domain.CartAggregate.Cart(request.Id);

        foreach (var product in request.Products)
        {
            cart.AddProduct(new CartProduct(product.Sku, product.Quantity, product.Price));
        }

        await _cartRepository.CreateAsync(cart);

        return new CreateCartResponse();
    }
}