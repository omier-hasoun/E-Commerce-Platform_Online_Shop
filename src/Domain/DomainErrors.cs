
namespace Domain;

// for copy paste

/*
 public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

 public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

 public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

 public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

 public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

 public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

 public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

 public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

 public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");
*/

public static class DomainErrors
{
    public const string BaseErrorCode = "DomainErrors";

    public static readonly Error ProductGroupIdInvalid = Products.ProductGroupIdInvalid;

    public static readonly Error ReturnItemRequestIdInvalid = ReturnItemRequests.ReturnItemRequestIdInvalid;

    public static readonly Error ProductIdInvalid = Products.ProductIdInvalid;

    public static readonly Error OrderIdInvalid = Orders.OrderIdInvalid;

    public static readonly Error ProductReviewIdInvalid = ProductReviews.ProductReviewIdInvalid;

    public static readonly Error UserIdInvalid = Users.UserIdInvalid;

    public static readonly Error InvalidStateTransition = Error.Validation($"{BaseErrorCode}.{nameof(InvalidStateTransition)}");

    public static readonly Error MoneyAmountInvalid = Error.Validation($"{BaseErrorCode}.{nameof(MoneyAmountInvalid)}");

    public static readonly Error GuestIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(GuestIdInvalid)}");

    public static readonly Error CartItemIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(CartItemIdInvalid)}");

    public static readonly Error MissingInput = Error.Validation($"{BaseErrorCode}.{nameof(MissingInput)}");

    public static readonly Error Locked = Error.Validation($"{BaseErrorCode}.{nameof(Locked)}");

    public static readonly Error EmailInvalid = Error.Validation($"{BaseErrorCode}.{nameof(EmailInvalid)}");

    public static readonly Error OrderPaymentIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(OrderPaymentIdInvalid)}");

    public static readonly Error ProviderReferenceIdMissing = Error.Validation($"{BaseErrorCode}.{nameof(ProviderReferenceIdMissing)}");

    public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

    public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

    public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

    public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    public static class ReturnItemRequests
    {
        public const string BaseErrorCode = "DomainErrors.ReturnItemRequest";

        public static readonly Error SerialNumbersCountNotEqualToQuantity = Error.Validation($"{BaseErrorCode}.{SerialNumbersCountNotEqualToQuantity}");

        public static readonly Error SerialNumbersRequired = Error.Validation($"{BaseErrorCode}.{SerialNumbersRequired}");

        public static readonly Error ProductDoesNotRequireSerialNumbers = Error.Validation($"{BaseErrorCode}.{ProductDoesNotRequireSerialNumbers}");

        public static readonly Error InvalidSerialNumbers = Error.Validation($"{BaseErrorCode}.{InvalidSerialNumbers}");

        public static readonly Error ReturnItemRequestIdInvalid = Error.Validation($"{BaseErrorCode}.{ReturnItemRequestIdInvalid}");


        public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }
    public static class Checkouts
    {

        public static readonly Error NoCustomerIdentityProvided = Error.Validation($"{BaseErrorCode}.{nameof(NoCustomerIdentityProvided)}");

        public static readonly Error NoItemsProvided = Error.Validation($"{BaseErrorCode}.{nameof(NoItemsProvided)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error CheckoutIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(CheckoutIdInvalid)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }



    public static class Products
    {
        public const string BaseErrorCode = "DomainErrors.Products";

        public static readonly Error ProductIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(ProductIdInvalid)}");

        public static readonly Error TitleInvalid = Error.Validation($"{BaseErrorCode}.{nameof(TitleInvalid)}");

        public static readonly Error DescriptionInvalid = Error.Validation($"{BaseErrorCode}.{nameof(DescriptionInvalid)}");

        public static readonly Error PriceInvalid = Error.Validation($"{BaseErrorCode}.{nameof(PriceInvalid)}");

        public static readonly Error QuantityInvalid = Error.Validation($"{BaseErrorCode}.{nameof(QuantityInvalid)}");

        public static readonly Error AverageRatingInvalid = Error.Validation($"{BaseErrorCode}.{nameof(AverageRatingInvalid)}");

        public static readonly Error InvalidImageFileName = Error.Validation($"{BaseErrorCode}.{nameof(InvalidImageFileName)}");

        public static readonly Error TitleOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(TitleOutOfRange)}");

        public static readonly Error DescriptionOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(DescriptionOutOfRange)}");

        public static readonly Error MaxNumberOfVariantsReached = Error.Validation($"{BaseErrorCode}.{nameof(MaxNumberOfVariantsReached)}");

        public static readonly Error ConnotPublishArchivedProduct = Error.Validation($"{BaseErrorCode}.{nameof(ConnotPublishArchivedProduct)}");

        public static readonly Error rename100 = Error.Validation($"{BaseErrorCode}.{nameof(rename100)}");

        public static readonly Error UpdateNotAllowedOnArchivedProducts = Error.Validation($"{BaseErrorCode}.{nameof(UpdateNotAllowedOnArchivedProducts)}");

        public static readonly Error CannotPublishWithoutAnyProduct = Error.Validation($"{BaseErrorCode}.{nameof(CannotPublishWithoutAnyProduct)}");

        public static readonly Error CannotChangeBrandAfterPublish = Error.Validation($"{BaseErrorCode}.{nameof(CannotChangeBrandAfterPublish)}");

        public static readonly Error CannotUpdateIsSerializedAfterPublish = Error.Validation($"{BaseErrorCode}.{nameof(CannotUpdateIsSerializedAfterPublish)}");

        public static readonly Error SpecificationsInvalid = Error.Validation($"{BaseErrorCode}.{nameof(SpecificationsInvalid)}");

        public static readonly Error CannotChangeCategoryAfterPublish = Error.Validation($"{BaseErrorCode}.{nameof(CannotChangeCategoryAfterPublish)}");


        public static readonly Error ProductGroupIdInvalid = Error.Validation($"{BaseErrorCode}.{ProductGroupIdInvalid}");

        public static readonly Error PriceOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(PriceOutOfRange)}");

        public static readonly Error ImagesNamesIsEmpty = Error.Validation($"{BaseErrorCode}.{nameof(ImagesNamesIsEmpty)}");

        public static readonly Error BarCodeRequired = Error.Validation($"{BaseErrorCode}.{nameof(BarCodeRequired)}");

        public static readonly Error BarCodeLengthOutOfRange = Error.Validation($"{BaseErrorCode}.{BarCodeLengthOutOfRange}");

        public static readonly Error SlugLengthOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(SlugLengthOutOfRange)}");

        public static readonly Error ImagesLimitExceeded = Error.Validation($"{BaseErrorCode}.{nameof(ImagesLimitExceeded)}");

        public static readonly Error SkuOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(SkuOutOfRange)}");

        public static readonly Error AtleastOneSpecificationRequired = Error.Validation($"{BaseErrorCode}.{nameof(AtleastOneSpecificationRequired)}");

        public static readonly Error InvalidSpecification = Error.Validation($"{BaseErrorCode}.{nameof(InvalidSpecification)}");

        public static readonly Error BarCodeOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(BarCodeOutOfRange)}");

        public static readonly Error MaxAllowedSpecificationsNumberExceeded = Error.Validation($"{BaseErrorCode}.{nameof(MaxAllowedSpecificationsNumberExceeded)}");

        public static readonly Error ImagesCountMustMatchProductImagesCount = Error.Validation($"{BaseErrorCode}.{nameof(ImagesCountMustMatchProductImagesCount)}");

        public static readonly Error ImagesNamesMustMatchProductImagesNames = Error.Validation($"{BaseErrorCode}.{nameof(ImagesNamesMustMatchProductImagesNames)}");

        public static readonly Error DiscountValueOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(DiscountValueOutOfRange)}");

        public static readonly Error DateMustBeInFuture = Error.Validation($"{BaseErrorCode}.{nameof(DateMustBeInFuture)}");

        public static readonly Error ProductPriceNotApplicableForDiscount = Error.Validation($"{BaseErrorCode}.{nameof(ProductPriceNotApplicableForDiscount)}");

        public static readonly Error rename10 = Error.Validation($"{BaseErrorCode}.{nameof(rename10)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");


    }

    public static class ProductReviews
    {
        public const string BaseErrorCode = "DomainErrors.ProductReviews";


        public static readonly Error ProductReviewIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(ProductReviewIdInvalid)}");

        public static readonly Error CommentLengthOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(CommentLengthOutOfRange)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");
    }




    public static class Orders
    {
        public const string BaseErrorCode = "DomainErrors.Orders";

        public static readonly Error OrderIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(OrderIdInvalid)}");

        public static readonly Error ItemsNumberLimitExceeded = Error.Validation($"{BaseErrorCode}.{nameof(ItemsNumberLimitExceeded)}");

        public static readonly Error ShippingFeesOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(ShippingFeesOutOfRange)}");

        public static readonly Error TotalItemsPriceOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(TotalItemsPriceOutOfRange)}");

        public static readonly Error SerialNumbersOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(SerialNumbersOutOfRange)}");

        public static readonly Error InvalidReturnQuantity = Error.Validation($"{BaseErrorCode}.{nameof(InvalidReturnQuantity)}");

        public static readonly Error OrderItemIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(OrderItemIdInvalid)}");

        public static readonly Error TaxAmountInvalid = Error.Validation($"{BaseErrorCode}.{nameof(TaxAmountInvalid)}");

        public static readonly Error ItemQuantityOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(ItemQuantityOutOfRange)}");

        public static readonly Error BillingAddressInvalid = Error.Validation($"{BaseErrorCode}.{nameof(BillingAddressInvalid)}");

        public static readonly Error ShippingAddressInvalid = Error.Validation($"{BaseErrorCode}.{nameof(ShippingAddressInvalid)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

        public static readonly Error EmailInvalid = Error.Validation($"{BaseErrorCode}.{nameof(EmailInvalid)}");
    }

    public static class Brands
    {
        public const string BaseErrorCode = "DomainErrors.Brands";

        public static readonly Error BrandIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(BrandIdInvalid)}");

        public static readonly Error NameInvalid = Error.Validation($"{BaseErrorCode}.{nameof(NameInvalid)}");
        public static readonly Error DescriptionInvalid = Error.Validation($"{BaseErrorCode}.{nameof(DescriptionInvalid)}");
        public static readonly Error CompanyNameInvalid = Error.Validation($"{BaseErrorCode}.{nameof(CompanyNameInvalid)}");
        public static readonly Error SlugInvalid = Error.Validation($"{BaseErrorCode}.{nameof(SlugInvalid)}");
        public static readonly Error LogoUrlInvalid = Error.Validation($"{BaseErrorCode}.{nameof(LogoUrlInvalid)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

        public static readonly Error rename10 = Error.Validation($"{BaseErrorCode}.{nameof(rename10)}");
    }



    public static class Categories
    {
        public const string BaseErrorCode = "DomainErrors.Categories";

        public static readonly Error CategoryIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(CategoryIdInvalid)}");

        public static readonly Error InvalidName = Error.Validation($"{BaseErrorCode}.{nameof(InvalidName)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");
    }

    public static class Carts
    {
        public const string BaseErrorCode = "DomainErrors.Carts";

        public static readonly Error CartIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(CartIdInvalid)}");

        public static readonly Error MaxNumberOfItemsReached = Error.Validation($"{BaseErrorCode}.{nameof(MaxNumberOfItemsReached)}");

        public static readonly Error CartItemIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(CartItemIdInvalid)}");

        public static readonly Error QuantityOutOfRange = Error.Validation($"{BaseErrorCode}.{nameof(QuantityOutOfRange)}");

        public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");
    }


    public static class Warehouses
    {
        public const string BaseErrorCode = "DomainErrors.Warehouses";


        public static readonly Error WarehouseIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(WarehouseIdInvalid)}");

        public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }



    public static class Users
    {
        public const string BaseErrorCode = "DomainErrors.Users";

        public static readonly Error UserIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(UserIdInvalid)}");

        public static readonly Error MaxNumberOfAddressesReached = Error.Validation($"{BaseErrorCode}.{nameof(MaxNumberOfAddressesReached)}");

        public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }

    public static class ShippingAddresses
    {
        public const string BaseErrorCode = "DomainErrors.CustomerShippingAddresses";

        public static readonly Error CustomerShippingAddressesIdInvalid = Error.Validation($"{BaseErrorCode}.CustomerShippingAddressesIdInvalid");

        public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }



    public static class Addresses
    {
        public const string BaseErrorCode = "DomainErrors.Addresses";

        public static readonly Error AddressIdInvalid = Error.Validation($"{BaseErrorCode}.{nameof(AddressIdInvalid)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }



    public static class Shipments
    {
        public const string BaseErrorCode = "DomainErrors.Shipments";

        public static readonly Error ShipmentIdInvalid = Error.Validation($"{BaseErrorCode}.ShipmentIdInvalid");

        public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }



    public static class PaymentProviders
    {
        public const string BaseErrorCode = "DomainErrors.PaymentProviders";

        public static readonly Error PaymentProviderIdInvalid = Error.Validation($"{BaseErrorCode}.PaymentProviderIdInvalid");

        public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }

    public static class Transactions
    {
        public const string BaseErrorCode = "DomainErrors.Transactions";

        public static readonly Error TransactionIdInvalid = Error.Validation($"{BaseErrorCode}.TransactionIdInvalid");

        public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }

    public static class UserPaymentMethodLogs
    {
        public const string BaseErrorCode = "DomainErrors.UserPaymentMethodLogs";

        public static readonly Error UserPaymentMethodLogIdInvalid = Error.Validation($"{BaseErrorCode}.UserPaymentMethodLogIdInvalid");

        public static readonly Error rename1 = Error.Validation($"{BaseErrorCode}.{nameof(rename1)}");

        public static readonly Error rename2 = Error.Validation($"{BaseErrorCode}.{nameof(rename2)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error rename5 = Error.Validation($"{BaseErrorCode}.{nameof(rename5)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error rename8 = Error.Validation($"{BaseErrorCode}.{nameof(rename8)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }

    public static class Inventories
    {
        public const string BaseErrorCode = "DomainErrors.ProductsStock";

        public static readonly Error ProductStockIdInvalid = Error.Validation($"{BaseErrorCode}.ProductStockIdInvalid");

        public static readonly Error InSufficientStock = Error.Validation($"{BaseErrorCode}.{nameof(InSufficientStock)}");

        public static readonly Error UnableToResetStockNow = Error.Validation($"{BaseErrorCode}.{nameof(UnableToResetStockNow)}");

        public static readonly Error rename3 = Error.Validation($"{BaseErrorCode}.{nameof(rename3)}");

        public static readonly Error rename4 = Error.Validation($"{BaseErrorCode}.{nameof(rename4)}");

        public static readonly Error QuantityWasGreaterThanReserved = Error.Validation($"{BaseErrorCode}.{nameof(QuantityWasGreaterThanReserved)}");

        public static readonly Error rename6 = Error.Validation($"{BaseErrorCode}.{nameof(rename6)}");

        public static readonly Error rename7 = Error.Validation($"{BaseErrorCode}.{nameof(rename7)}");

        public static readonly Error QuantityInvalid = Error.Validation($"{BaseErrorCode}.{nameof(QuantityInvalid)}");

        public static readonly Error rename9 = Error.Validation($"{BaseErrorCode}.{nameof(rename9)}");

    }
}
