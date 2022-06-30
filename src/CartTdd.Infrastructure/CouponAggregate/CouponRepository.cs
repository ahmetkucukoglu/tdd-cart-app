using CartTdd.Domain.CouponAggregate;
using CartTdd.Infrastructure.Database;
using MongoDB.Driver;

namespace CartTdd.Infrastructure.CouponAggregate;

public class CouponRepository : ICouponRepository
{
    private readonly DbContext _dbContext;

    public CouponRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Coupon?> GetAsync(string code)
    {
        var filter = Builders<Coupon>.Filter.Eq(c => c.Code, code);

        var documents = await _dbContext.Coupons.FindAsync(filter);
        
        return await documents.FirstOrDefaultAsync();
    }
}