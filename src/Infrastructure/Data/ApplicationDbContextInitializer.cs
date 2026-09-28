
using Application.Entities;
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
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, AppDbContext context, UserManager<AppUser> userManager, RoleManager<Role> roleManager)
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
        var random = new Random();
        var brandId1 = new BrandId(Guid.Parse("019dd6b8-96d0-734b-97ca-02b8094e7f00"));
        var brandName1 = "Apple";
        var brandId2 = new BrandId(Guid.Parse("019dd6b8-96d0-7904-8a7a-d1e750789a04"));
        var brandName2 = "Nvidia";

        // context.Brands.Add(Brand.Create(brandId1, brandName1, "shity company", "Apple GmbH", "APP", null).Value);
        // context.Brands.Add(Brand.Create(brandId2, brandName2, "shity company", "NVidia GmbH", "NVIDIA", null).Value);

        var categoryId1 = new CategoryId(1);
        var categoryName1 = "Smart phones";
        var categoryId2 = new CategoryId(2);
        var categoryName2 = "Graphic cards";

        context.Categories.Add(Category.Create(categoryId1, categoryName1, null).Value);
        context.Categories.Add(Category.Create(categoryId2, categoryName2, null).Value);

        var productGroupId1 = new ProductGroupId(3);
        var productGroupId2 = new ProductGroupId(4);
        var productGroup1 = ProductGroup.Create(productGroupId1, brandId1, brandName1, categoryId1, categoryName1, "Iphone 17 pro max", "A shitty phone", true, new Dictionary<string, string>() { { "Ram", "8Gb" }, { "Display", "17 Zoll" } }).Value;
        var productGroup2 = ProductGroup.Create(productGroupId2, brandId2, brandName2, categoryId2, categoryName2, "Iphone 17 pro", "A shitty phone", true, new Dictionary<string, string>() { { "Ram", "6Gb" }, { "Display", "14 Zoll" } }).Value;

        context.ProductGroups.Add(productGroup1);
        context.ProductGroups.Add(productGroup2);
        await context.SaveAsync();


        var productId1 = new ProductId(5);
        var productId2 = new ProductId(6);
        var productId3 = new ProductId(7);
        var productId4 = new ProductId(8);

        productGroup1.AddProduct(productId1, Money.Create(200), 20, 20, 20, 2, "OMIERHASOUNKAMHEA", "omier-hasoun.com", "1234567890", new Dictionary<string, string>() { { "Color", "blue" } });
        productGroup1.AddProduct(productId2, Money.Create(220), 10, 10, 10, 2, "OMIERHASOUNKAMHEA", "omier-hasoun.com", "1234567890", new Dictionary<string, string>() { { "Color", "pink" } });
        productGroup2.AddProduct(productId3, Money.Create(160), 10, 10, 10, 2, "ommei30273", "omier-hasoun.com", "1234567890", new Dictionary<string, string>() { { "Color", "pink" } });
        productGroup2.AddProduct(productId4, Money.Create(150), 10, 10, 10, 2, "ommei30272", "omier-hasoun.com", "1234567890", new Dictionary<string, string>() { { "Color", "gold" } });
        await context.SaveAsync();


        for (int i = 1; i <= 1000; i++)
        {
            // 1. Create the ProductGroup
            var pgId = new ProductGroupId(i + 10); // Offset to avoid previous IDs
            var brandId = (i % 2 == 0) ? brandId1 : brandId2;
            var categoryId = (i % 2 == 0) ? categoryId1 : categoryId2;
            var brandName = (i % 2 == 0) ? brandName1 : brandName2;
            var categoryName = (i % 2 == 0) ? categoryName1 : categoryName2;


            var pgResult = ProductGroup.Create(
                pgId,
                brandId,
                brandName,
                categoryId,
                categoryName,
                $"Product Group {i}",
                "Generated test group",
                true,
                new Dictionary<string, string>() { { "Spec", "Standard" } }
            );

            if (pgResult.Succeeded)
            {
                var productGroup = pgResult.Value;
                context.ProductGroups.Add(productGroup);
                await context.SaveAsync();

                // 2. Add 1 to 3 products
                int productCount = random.Next(1, 10);
                for (int j = 1; j <= productCount; j++)
                {
                    var pId = new ProductId((i * 10) + j);
                    productGroup.AddProduct(
                        pId,
                        Money.Create(100 + (i * 10)),
                        10, 5, 5, 1,
                        $"SKU-{i}-{j}",
                        "example.com",
                        "1234567890",
                        new Dictionary<string, string>() { { "Variation", $"Opt-{j}" } }
                    );
                }
                productGroup.PublishGroup();
                // 3. Add to context
                await context.SaveAsync();
            }
        }
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




        var addressId = new AddressId(201);
        var address = Address.Create(addressId, "Omier Hason", "01789 386 4983", "DE", "41", "Essen", "45355", "Marktstr. 41", null, null, null, "best appartment").Value;
        context.Addresses.Add(address);

        var warehouseId = new WarehouseId(20);
        var warehouse = Warehouse.Create(warehouseId, addressId, "Omier's Warehouse GmbH").Value;
        context.Warehouses.Add(warehouse);

        var inventory1 = Inventory.Create(warehouseId, productId1, 43).Value;
        var inventory2 = Inventory.Create(warehouseId, productId2, 2).Value;
        var inventory3 = Inventory.Create(warehouseId, productId3, 540).Value;
        var inventory4 = Inventory.Create(warehouseId, productId4, 10).Value;


        context.Inventories.Add(inventory1);
        context.Inventories.Add(inventory2);
        context.Inventories.Add(inventory3);
        context.Inventories.Add(inventory4);

        var cart1 = Cart.CreateForGuest(new CartId(20), new GuestAccountId(Guid.Parse("019e0f1a-7b38-79e5-8912-9a9f83a4a549"))).Value;
        var cart2 = Cart.CreateForUser(new CartId(21), Guid.Parse("10000000-0000-0000-0000-000000000001")).Value;


        cart1.AddItem(new CartItemId(10), productId2, 3);
        cart1.AddItem(new CartItemId(11), productId4, 1);

        cart2.AddItem(new CartItemId(12), productId3, 2);
        cart2.AddItem(new CartItemId(112), productId4, 10);


        context.Carts.Add(cart1);
        context.Carts.Add(cart2);



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
