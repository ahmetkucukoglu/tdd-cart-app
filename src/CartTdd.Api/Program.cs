using CartTdd.Application.Cart;
using CartTdd.Domain.CartAggregate;
using CartTdd.Domain.CouponAggregate;
using CartTdd.Infrastructure.CartAggregate;
using CartTdd.Infrastructure.CouponAggregate;
using CartTdd.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

DbConfiguration.Configure();

builder.Services.Configure<DbSettings>(builder.Configuration.GetRequiredSection(nameof(DbSettings)));
builder.Services.AddScoped<DbContext>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<CouponApplier>();
builder.Services.AddMediatR(typeof(CreateCart));

var app = builder.Build();

app.UseExceptionHandler(applicationBuilder => applicationBuilder.Run(async httpContext =>
{
    var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();

    if (exceptionHandlerPathFeature?.Error != null)
    {
        await httpContext.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message);
    }
}));

app.MapPost("/carts/{id}", async (Guid id, [FromBody] CreateCart request, IMediator mediator)
    => await mediator.Send(request with {Id = id}));

app.MapPut("/carts/{id}/apply-coupon", async (Guid id, [FromBody] ApplyCoupon request, IMediator mediator)
    => await mediator.Send(request with {CartId = id}));

app.MapGet("/carts/{id}", async (Guid id, IMediator mediator)
    => await mediator.Send(new GetCart(id)));

app.Run();