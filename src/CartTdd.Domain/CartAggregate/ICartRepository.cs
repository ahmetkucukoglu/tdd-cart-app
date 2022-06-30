namespace CartTdd.Domain.CartAggregate;

public interface ICartRepository
{
    Task<Cart?> GetAsync(Guid id);
    Task CreateAsync(Cart cart);
    Task UpdateAsync(Cart cart);
}