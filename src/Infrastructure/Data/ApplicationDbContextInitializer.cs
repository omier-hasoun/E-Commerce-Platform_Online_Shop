
using Application.Entities;
using Application.Features.Management.Products.Commands.CreateProduct;
using Application.Features.Management.Products.Commands.PublishProduct;
using Application.Features.Management.Products.Dtos;
using Application.Features.Management.Products.Queries.GetProductById;
using Domain.Brands;
using Domain.Carts;
using Domain.Carts.CartItems;
using Domain.Categories;
using Domain.Common.Entities.Addresses;
using Domain.Common.ValueObjects;
using Domain.Currencies;
using Domain.Inventories;
using Domain.ProductGroups;
using Domain.ProductGroups.Products;
using Domain.Warehouses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, AppDbContext context, UserManager<AppUser> userManager, RoleManager<Role> roleManager, IMediator mediator)
{
    public async Task InitialiseAndSeedData()
    {
        try
        {
            //await context.Database.MigrateAsync();


            await SeedData();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
        }
    }

    private async Task SeedData()
    {
        var user1 = new AppUser()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            Email = "om@gmail.com",
            NormalizedEmail = "om@gmail.com".ToUpper(),
            UserName = "om@gmail.com",
            NormalizedUserName = "om@gmail.com".ToUpper(),
            EmailConfirmed = true,
            AccessFailedCount = 0,
            PhoneNumber = "092737287",

        };

        var result = await userManager.CreateAsync(user1, "1234Omier#");



        var random = new Random();
        var brandId1 = new BrandId(Guid.Parse("019dd6b8-96d0-734b-97ca-02b8094e7f00"));
        var brandName1 = "Apple";
        var brandId2 = new BrandId(Guid.Parse("019dd6b8-96d0-7904-8a7a-d1e750789a04"));
        var brandName2 = "Nvidia";


        //context.Brands.Add(Brand.Create(brandId1, brandName1, "shity company", "Apple GmbH", "APP", null).Value);
        //context.Brands.Add(Brand.Create(brandId2, brandName2, "shity company", "NVidia GmbH", "NVIDIA", null).Value);


        var categoryId1 = new CategoryId(1);
        var categoryName1 = "Smart phones";
        var categoryId2 = new CategoryId(2);
        var categoryName2 = "Graphic cards";

        //context.Categories.Add(Category.Create(categoryId1, categoryName1, null).Value);
        //context.Categories.Add(Category.Create(categoryId2, categoryName2, null).Value);

        var addressId = new AddressId(201);
        var address = Address.Create(addressId, "Omier Hason", "01789 386 4983", "DE", "41", "Essen", "45355", "Marktstr. 41", null, null, null, "best appartment").Value;
        //context.Addresses.Add(address);

        var warehouseId = new WarehouseId(20);
        var warehouse = Warehouse.Create(warehouseId, addressId, "Omier's Warehouse GmbH").Value;

        //context.Warehouses.Add(warehouse);
        //await context.SaveAsync();

        var cart1 = Cart.CreateForGuest(new CartId(20), new GuestAccountId(Guid.Parse("019e0f1a-7b38-79e5-8912-9a9f83a4a549"))).Value;
        var cart2 = Cart.CreateForUser(new CartId(21), Guid.Parse("10000000-0000-0000-0000-000000000001")).Value;

        context.Carts.Add(cart1);
        context.Carts.Add(cart2);

        for (int i = 1; i <= 200; i++)
        {
            var brandId = (i % 2 == 0) ? brandId1 : brandId2;
            var categoryId = (i % 2 == 0) ? categoryId1 : categoryId2;
            var title = $"Product Title {i} .";
            var description = $"Product Description {i} this product is useful for this case and this case and is not waterproof bla bla blaaa.";
            var isSerialized = (i % 2 == 0) ? false : true;
            var price = (decimal)(i * 5) + 0.99m;
            var width =  12;
            var height = 15;
            var length =  5;
            var weight = 500;
            var sku = $"PRODUCT-{i}-0027-OH";
            var barCode = $"11208362{i}";
            var slug = string.Join("-", title.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            var stockQuantity = (i % 2 == 0) ? 0 : 15;
            var inventory = new InventoryDto(stockQuantity);
            var specs = new Dictionary<string, string>() {{ "Spec", "Standard" }, { "color", "blue" }, { "sfsdflfdsfd sdfd", "bldf sfsd uess 13" } };

            var command = new CreateProductCommand(brandId.Value, categoryId.Value, title, description, isSerialized,
                                                   price, width, height, length, weight, sku, barCode, slug, inventory, null, null, specs);

            var id = await mediator.Send(command, CancellationToken.None);

            await mediator.Send(new PublishProductCommand(id.Value), CancellationToken.None);

            if(i % 10 == 0)
            {
                cart1.AddItem(new CartItemId(10), new ProductId(id.Value), 3);

                cart2.AddItem(new CartItemId(12), new ProductId(id.Value), 1);

            }

        }
        await context.SaveAsync();

        await SeedCurrencies();

        await roleManager.CreateAsync(new Role()
        {
            Id = Guid.Parse("45995BD9-F233-4EB3-BDFA-41B012113B85"),
            Name = "Admin",
        });
        await roleManager.CreateAsync(new Role()
        {
            Id = Guid.Parse("45995BD9-F233-4EB3-BDFA-41B012113B11"),
            Name = "Staff",

        });
        await roleManager.CreateAsync(new Role()
        {
            Id = Guid.Parse("45995BD9-F233-4EB3-BDFA-41B012113B91"),
            Name = "Manager",

        });



        await context.SaveAsync();
    }


    private async Task SeedCurrencies()
    {
        context.Currencies.Add(new Currency
        {
            Code = "USD",
            Name = "Dollars",
            Symbol = "$"

        });
        context.Currencies.Add(new Currency
        {
            Code = "EUR",
            Name = "Euro",
            Symbol = "€"

        });

        context.Currencies.Add(new Currency
        {
            Code = "KR",
            Name = "Swedish krona",
            Symbol = "kr"

        });

        context.Currencies.Add(new Currency
        {
            Code = "GBP",
            Name = "Pounds",
            Symbol = "£"

        });

        context.Currencies.Add(new Currency
        {
            Code = "SYP",
            Name = "Pounds",
            Symbol = "£"

        });
    }
}
