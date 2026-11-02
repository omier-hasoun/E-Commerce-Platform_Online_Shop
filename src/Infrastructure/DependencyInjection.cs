#region usings
using System.Threading.Channels;
using Application.Entities;
using Domain.Brands;
using Domain.Carts;
using Domain.Carts.CartItems;
using Domain.Categories;
using Domain.Common.Entities.Addresses;
using Domain.Orders;
using Domain.Orders.OrderLines;
using Domain.ProductGroups;
using Domain.ProductGroups.Products;
using Domain.Warehouses;
using FileSignatures;
using Infrastructure.BackgroundJobs;
using Infrastructure.Channels;
using Infrastructure.Common.Exceptions;
using Infrastructure.Configurations;
using Infrastructure.Data.IdGenerators;
using Infrastructure.Data.IdGenerators.Primitives;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DiscountReset;
using Infrastructure.Services.Email.Console;
using Infrastructure.Services.Email.Maileroo;
using Infrastructure.Services.Hashing;
using Infrastructure.Services.Messaging;
using Infrastructure.Services.Payment.StripeService;
using Infrastructure.Services.Storage;
using Infrastructure.Services.Storage.Images;
using Infrastructure.Services.UrlProviders;
using Infrastructure.Services.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;
using Shippo;
using App = Application.Common.Abstractions;
#endregion

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment enviroment)
    {
        services.AddCustomServices(config)
                .AddEfCoreServices(config, enviroment)
                .AddIdGenServices(config)
                .AddIdGeneratorServices()
                .AddIdentityServices()
                .AddFileSignaturesServices()
                .AddOptions(config);
        return services;
    }


    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ApplicationUrlsOptions>(config.GetSection(ApplicationUrlsOptions.SectionName));

        services.Configure<MediaOptions>(config.GetSection(MediaOptions.SectionName));

        services.Configure<StripeOptions>(options =>
        {
            options.TestKey = config["STRIPE_TEST_KEY"] ?? throw new StripeApiKeyWasNotProvidedException();
        });

        services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 20 * 1024 * 1024;//20 mb
            options.MemoryBufferThreshold = 32 * 1024; // 32 KB
        });

        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IShippoSDK, ShippoSDK>(sp =>
        {
            var key = config["SHIPPO_TEST_KEY"] ?? throw new InvalidOperationException("shippo api key was not configured.");
            return new ShippoSDK(key, null, "2018-02-08");
        });

        services.AddTransient<IImageValidator, ImageValidator>();
        services.AddSingleton<IFileStorageService, LocalFileStorageService>();

        services.AddHostedService<ImageProcessingWorker>();
        services.AddHostedService<ZeroOclockWorker>();
        services.AddHostedService<StripeEventProcessingWorker>();
        services.AddHostedService<OrderRefundProcessingWorker>();
        services.AddHostedService<OutboxMessagesProcessingWorker>();

        services.AddSingleton(sp => {

            return Channel.CreateBounded<ImageProcessingJob>(new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleWriter = false,
                SingleReader = true,
                AllowSynchronousContinuations = false,
            });

         });

        services.AddSingleton<IProductThumbnailUrlProvider, ProductThumbnailUrlProvider>();

        services.AddSingleton<IApplicationUrlProvider, ApplicationUrlProvider>();

        services.AddSingleton<IImageStorageService, ImagesStorageService>();
        services.AddSingleton<IImageJobReader, ImageProcessingJobsChannel>();
        services.AddSingleton<IImageJobWriter, ImageProcessingJobsChannel>();
        services.AddSingleton<IPaymentGateway, StripePaymentGateway>();
        services.AddSingleton<IEmailService, MailerooMailService>(sp =>
        {
            var noReplyInfoEmail = config["NoReplyInfoEmail"] ?? throw new InvalidOperationException("NoReplyInfoEmail was not configured");
            var serviceEmail = config["ServiceEmail"] ?? throw new InvalidOperationException("ServiceEmail was not configured");

            return new MailerooMailService(sp.GetRequiredService<IHttpClientFactory>(), noReplyInfoEmail, serviceEmail);
        });

        services.AddSingleton<IUniqueImageNameProvider, UniqueImageNameProvider>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddSingleton<IZeroOclockService, DiscountResetService>();

        services.AddHttpClient("Maileroo", client =>
        {
            var apiKey = config["OMIER_MAILEROO_KEY"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("Maileroo API key was not configured.");
            }

            client.BaseAddress = new Uri("https://smtp.maileroo.com/api/v2/");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        }).AddStandardResilienceHandler(options =>
        {
            options.Retry.MaxRetryAttempts = 1;

            options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(15);
            options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(20);

        });

  return services;
    }

    private static IServiceCollection AddIdGeneratorServices(this IServiceCollection services)
    {
        services.AddKeyedSingleton<IPrimitiveTypeIdGenerator<Guid>, GuidV7Generator>("GuidV7");
        services.AddKeyedSingleton<IPrimitiveTypeIdGenerator<long>, SnowflakeGenerator>("Snowflake");

        services.AddSingleton<App.IIdGenerator<long>, SnowflakeGenerator>();


        services.AddSingleton<App.IIdGenerator<ProductGroupId>, ProductGroupIdGenerator>();

        services.AddSingleton<App.IIdGenerator<ProductId>, ProductIdGenerator>();

        services.AddSingleton<App.IIdGenerator<OrderLineId>, OrderLineIdGenerator>();
        services.AddSingleton<App.IIdGenerator<CategoryId>, CategoryIdGenerator>();

        services.AddSingleton<App.IIdGenerator<BrandId>, BrandIdGenerator>();

        services.AddSingleton<App.IIdGenerator<OrderId>, OrderIdGenerator>();
        services.AddSingleton<App.IIdGenerator<CartId>, CartIdGenerator>();
        services.AddSingleton<App.IIdGenerator<CartItemId>, CartItemIdGenerator>();
        services.AddSingleton<App.IIdGenerator<WarehouseId>, WarehouseIdGenerator>();
        services.AddSingleton<App.IIdGenerator<AddressId>, AddressIdGenerator>();

        return services;
    }

    private static IServiceCollection AddIdGenServices(this IServiceCollection services, IConfiguration config)
    {

        string strMachineId = config["MACHINE_ID"] ?? throw new MachineIdWasNotProvidedException();

        if(!byte.TryParse(strMachineId, out var machineId))
        {
            throw new MachineIdWasNotProvidedException();
        }

        services.AddSingleton<IdGen.IIdGenerator<long>, IdGen.IdGenerator>((p) =>
        {
            var options = new IdGen.IdGeneratorOptions()
            {
                TimeSource = new IdGen.DefaultTimeSource(new DateTime(2026, 5, 25, 0, 0, 0, DateTimeKind.Utc)),
                IdStructure = new IdGen.IdStructure(41, 11, 11),
                SequenceOverflowStrategy = IdGen.SequenceOverflowStrategy.SpinWait,
            };
            return new IdGen.IdGenerator(machineId, options);
        });



        return services;
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {

        services.AddIdentityCore<AppUser>(op =>
        {
            op.Tokens.PasswordResetTokenProvider = "UserResetPassToken";
        })
                    .AddRoles<Role>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddApiEndpoints()
                    .AddTokenProvider<UserResetPasswordTokenProvider>("UserResetPassToken");


        services.AddTransient<IEmailSender<AppUser>, SecurityEmailSender>();

        services.AddScoped<IPasswordHasher<AppUser>, UserPasswordHashService>();

        return services;
    }
    private static IServiceCollection AddFileSignaturesServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileFormatInspector, FileFormatInspector>( sp =>
        {
            return new FileFormatInspector(FileFormatLocator.GetFormats());
        });
        return services;
    }
    private static string GetDbConnectionString(IConfiguration config, IWebHostEnvironment environment)
    {
        string? connString;

        bool isProductionOrStaging = environment.IsDevelopment() == false;

        if (isProductionOrStaging)
            connString = config["CONNECTION_STRING"];
        else
            connString = config.GetConnectionString("DefaultConnection")!;

        if (connString == null)
        {
            throw new ConnectionStringWasNotProvidedException();
        }
        return connString;
    }


    private static IServiceCollection AddEfCoreServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
    {
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(GetDbConnectionString(config, environment)).AddInterceptors(
                sp.GetRequiredService<SoftDeleteEntitySaveChangesInterceptor>(),
                sp.GetRequiredService<AuditedEntitySaveChangesInterceptor>(),
                sp.GetRequiredService<EventsPublisherSaveChangesInterceptor>()
            );
        });

        services.AddScoped<IAppDbContext, AppDbContext>();

        //interceptors
        services.AddScoped<AuditedEntitySaveChangesInterceptor>();
        services.AddScoped<EventsPublisherSaveChangesInterceptor>();

        services.AddScoped<SoftDeleteEntitySaveChangesInterceptor>();

        return services;
    }


}
