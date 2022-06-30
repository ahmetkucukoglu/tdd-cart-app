using CartTdd.Domain.CartAggregate;
using CartTdd.Infrastructure.Database;
using MongoDB.Driver;

namespace CartTdd.Infrastructure.CartAggregate;

public class CartRepository : ICartRepository
{
    private readonly DbContext _dbContext;

    public CartRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Cart?> GetAsync(Guid id)
    {
        var filter = Builders<Cart>.Filter.Eq(c => c.Id, id);

        var documents = await _dbContext.Carts.FindAsync(filter);

        return await documents.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Cart cart)
    {
        await _dbContext.Carts.InsertOneAsync(cart);
    }

    public async Task UpdateAsync(Cart cart)
    {
        var filter = Builders<Cart>.Filter.Eq(c => c.Id, cart.Id);

        await _dbContext.Carts.ReplaceOneAsync(filter, cart);
    }
}