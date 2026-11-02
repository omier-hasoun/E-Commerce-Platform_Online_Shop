
using Application.Common.Behaviors;
using Application.Features.Management.Products.Commands.CreateProduct;
using Application.Features.Public.Products.Queries.SearchProductSuggestions;
using Domain.Services.Checkout;
using Domain.Services.Shipping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCustomServices(config)
                .AddDomainServices()
                .AddMediatRServices();

        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration config)
    {

        services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();

        return services;
    }
    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<ShippingCostCalculator>();
        services.AddTransient<CheckoutService>();


        return services;
    }

    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR((m) =>
        {
            m.RegisterServicesFromAssemblyContaining<AssemblyMarker>();
            m.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}
