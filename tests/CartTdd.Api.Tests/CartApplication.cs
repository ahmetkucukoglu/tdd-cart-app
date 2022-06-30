using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CartTdd.Api.Tests;

class CartApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddConfiguration(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"DbSettings:ConnectionString", "mongodb://localhost:27017"},
                    {"DbSettings:DatabaseName", $"cart-tdd-{Guid.NewGuid()}"}
                })
                .Build());
        });

        return base.CreateHost(builder);
    }
}