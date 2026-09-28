using Domain.Brands;
using Domain.Categories;
using Domain.Common.Entities.Addresses;
using Domain.Orders.OrderLines;
using Domain.Orders.Shipments;
using Domain.PaymentProviders;
using Domain.ProductReviews;
using Domain.ProductGroups.Products;
using Domain.Inventories;
using Domain.ReturnItemRequestsReviews;
using Domain.ReturnItemRequests;
using Domain.Transactions;
using Domain.Warehouses;
using Domain.UserShippingAddresses;
using Application.Entities;
using Domain.Carts.CartItems;
using Domain.Carts;
using Domain.Countries;
using Domain.Currencies;
using Domain.Countries.StateProvinces;

namespace Application.Common.Abstractions;

public interface IAppDbContext

{
    DbSet<AppUser> Users { get; }

    DbSet<Order> Orders { get; }
    DbSet<OrderLine> OrderLines { get; }

    DbSet<ProductReview> ProductReviews { get; }
    DbSet<ProductGroup> ProductGroups { get; }
    DbSet<Product> Products { get; }

    DbSet<Brand> Brands { get; }

    DbSet<AppSettings> AppSettings { get; }

    DbSet<Warehouse> Warehouses { get; }
    DbSet<Inventory> Inventories { get; }

    DbSet<Country> Countries { get; }
    DbSet<Currency> Currencies { get; }
    DbSet<StateProvince> StateProvinces { get; }

    DbSet<PaymentProvider> PaymentProviders { get; }
    DbSet<Transaction> Transactions { get; }

    DbSet<Category> Categories { get; }

    DbSet<ReturnItemRequest> ReturnRequests { get; }
    DbSet<ReturnItemRequestReview> ReturnItemRequestReviews { get; }

    DbSet<Address> Addresses { get; }
    DbSet<UserShippingAddress> ShippingAddresses { get; }

    DbSet<Cart> Carts { get; }

    DbSet<CartItem> CartItems { get; }

    DbSet<Shipment> Shipments { get; }

    Task SaveAsync(CancellationToken ct = default);
}
