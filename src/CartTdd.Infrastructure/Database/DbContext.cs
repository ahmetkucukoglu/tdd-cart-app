using System.Runtime.CompilerServices;
using CartTdd.Domain.CartAggregate;
using CartTdd.Domain.CouponAggregate;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

[assembly: InternalsVisibleTo("CartTdd.Api.Tests"),
           InternalsVisibleTo("CartTdd.Domain.Tests")]

namespace CartTdd.Infrastructure.Database;

public class DbContext
{
    internal IMongoClient Client { get; set; }
    internal string DatabaseName { get; set; }
    public IMongoCollection<Cart> Carts { get; set; }
    public IMongoCollection<Coupon> Coupons { get; set; }

    public DbContext(IOptions<DbSettings> dbSettings)
    {
        DatabaseName = dbSettings.Value.DatabaseName;
        Client = new MongoClient(dbSettings.Value.ConnectionString);

        var database = Client.GetDatabase(dbSettings.Value.DatabaseName);

        Carts = database.GetCollection<Cart>("carts");
        Coupons = database.GetCollection<Coupon>("coupons");
    }
}