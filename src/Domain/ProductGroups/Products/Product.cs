
using Domain.Inventories;

namespace Domain.ProductGroups.Products;

public sealed class Product : BaseEntity<ProductId>
{
    private Product()
    {
    }

    private Product(ProductId id, ProductGroupId productGroupId, Money? discountPrice, Money originalPrice, Money currentPrice, byte? discountPercentage, DateOnly? discountExpiresOn, ProductState status,
        int width, int height, int length, int weight, string sku, string slug, string barCode, Dictionary<string, string> variantSpecifications, List<ProductImage> images)
        : base(id)
    {
        ProductGroupId = productGroupId;

        DiscountPrice = discountPrice;
        DiscountPercentage = discountPercentage;
        DiscountExpiresOn = discountExpiresOn;
        OriginalPrice = originalPrice;
        CurrentPrice = currentPrice;
        Status = status;
        Width = width;
        Height = height;
        Length = length;
        Weight = weight;

        Sku = sku;
        Slug = slug;
        BarCode = barCode;
        _images = images;
        _variantSpecifications = variantSpecifications;
    }

    public static Result<Product> Create(ProductId id, ProductGroupId productGroupId, Money originalPrice, int width, int height, int length,
        int weight, string sku, string slug, string barCode, Dictionary<string, string>? variantSpecifications, List<ProductImage>? images = null)
    {
        var validationResult = Result.ValidateAll(
                                () => id.IsValid(),
                                () => ValidatePrice(originalPrice),
                                () => Validate_Width_Height_Length(width, height, length),
                                () => ValidateVariantSpecifications(variantSpecifications),
                                () => ValidateSku(sku),
                                () => ValidateSlug(slug),
                                () => ValidateBarcode(barCode));

        if (validationResult.Failed)
        {
            return validationResult.Errors;
        }

        //defaults
        byte? discountPercentage = null;
        Money? priceBeforeDisount = null;
        Money currentPrice = originalPrice;

        DateOnly? discountExpiresOn = null;

        ProductState status = ProductState.Draft;
        images ??= [];

        return new Product(id, productGroupId, priceBeforeDisount, originalPrice, currentPrice, discountPercentage, discountExpiresOn, status,
            width, height, length, weight, sku, slug, barCode, variantSpecifications, images);
    }



    public Result<Updated> Update(Money originalPrice, int width, int height, int length,
        int weight, string sku, string slug, string barCode)
    {
        var validationResult = Result.ValidateAll(
                        () => ValidatePrice(originalPrice),
                        () => Validate_Width_Height_Length(width, height, length),
                        () => ValidateSku(sku),
                        () => ValidateSlug(slug),
                        () => ValidateBarcode(barCode));

        if (validationResult.Failed)
        {
            return validationResult.Errors;
        }

        Width = width;
        Height = height;
        Length = length;
        Weight = weight;

        Sku = sku;
        Slug = slug;
        BarCode = barCode;

        OriginalPrice = originalPrice;

        // recalculate if product is updated while it is in discount
        if (HasDiscount)
            ApplyDiscount(DiscountExpiresOn!.Value, DiscountPercentage!.Value);

        return Result.Updated;
    }

    public ProductGroupId ProductGroupId { get; private init; }


    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Length { get; private set; }
    public int Weight { get; private set; }

    public Money OriginalPrice { get; private set; } = null!;


    public Money CurrentPrice { get; private set; }

    public Money? DiscountPrice { get; private set; }

    public bool HasDiscount { get; private set; } = false;
    public DateOnly? DiscountExpiresOn { get; private set; }
    public byte? DiscountPercentage { get; private set; }

    public ProductState Status { get; private set; }

    public string Sku { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public string BarCode { get; private set; } = null!;

    public Inventory Inventory { get; private set; }

    private bool CanTransitionTo(ProductState newStatus)
    {
        return (Status, newStatus) switch
        {
            (ProductState.Draft, ProductState.Published) => true,
            (ProductState.Draft, ProductState.Archived) => true,
            (ProductState.Published, ProductState.Unpublished) => true,
            (ProductState.Published, ProductState.Archived) => true,
            (ProductState.Unpublished, ProductState.Published) => true,
            (ProductState.Unpublished, ProductState.Archived) => true,
            _ => false
        };

    }
    private bool CanModify() => Status != ProductState.Archived;
    public bool IsPublished() => Status == ProductState.Published;

    private List<ProductImage> _images = [];
    public IReadOnlyCollection<ProductImage> Images { get { return _images.AsReadOnly(); } private set { _images = value is null ? [] : value.ToList(); } }


    private Dictionary<string, string> _variantSpecifications = [];
    public IReadOnlyDictionary<string, string> VariantSpecifications { get { return _variantSpecifications.AsReadOnly(); } private set { _variantSpecifications = value is null ? [] : value.ToDictionary(); } }

    public Result<Updated> UpdateImages(List<ProductImage> images)
    {
        if (!CanModify())
            return DomainErrors.Locked;

        if (images is null || images.Count == 0)
            return DomainErrors.Products.ImagesRequired;

        if (images.Count > ProductRules.MaxNumberOfImages)
            return DomainErrors.Products.ImagesLimitExceeded.WithParameters(ProductRules.MaxNumberOfImages);

        this.Images = images;

        SortImages();

        return Result.Updated;
    }

    public Result<Deleted> RemoveImages(List<string> fileNames)
    {
        if (!CanModify())
            return DomainErrors.Locked;

        if (fileNames is null || fileNames.Count == 0)
            return DomainErrors.MissingInput;

        if (fileNames.Count > _images.Count)
            return DomainErrors.Products.ImagesCountMustMatchProductImagesCount;

        foreach (var name in fileNames)
        {
            if (!_images.Exists(x => x.FileName == name))
            {
                return DomainErrors.Products.InvalidImageFileName.WithParameters(name);
            }

        }
        foreach (var name in fileNames)
        {
            var image = _images.FirstOrDefault(x => x.FileName == name);

            // happens if the list contains duplicate filenames which should not happen but if it happend we just skip it
            if (image is null)
                continue;

            _images.Remove(image);
        }

        SortImages();
        return Result.Deleted;
    }

    public Result<Success> Publish()
    {
        if (!CanTransitionTo(ProductState.Published))
        {
            return DomainErrors.InvalidStateTransition;
        }
        if (Inventory == null)
        {
            return DomainErrors.Products.InventoryRequired;
        }

        Status = ProductState.Published;
        return Result.Success;
    }
    public Result<Success> Unpublish()
    {
        if (!CanTransitionTo(ProductState.Unpublished))
        {
            return DomainErrors.InvalidStateTransition;
        }
        Status = ProductState.Unpublished;
        return Result.Success;
    }
    public Result<Success> Archive()
    {
        if (!CanTransitionTo(ProductState.Archived))
        {
            return DomainErrors.InvalidStateTransition;
        }
        Status = ProductState.Archived;
        return Result.Success;
    }

    private void  SortImages()
    {
        byte sortOrder = 1;
        List<ProductImage> sortedImages = new(_images.Count);

        foreach (var image in _images.OrderBy(i => i.SortOrder))
        {
            sortedImages.Add(image.ChangeSortOrder(sortOrder++));
        }
        _images = sortedImages;

    }

    public Result<Success> UpdateImagesSortOrder(IReadOnlyCollection<ProductImage> images)
    {
        if (!CanModify())
            return DomainErrors.Locked;

        if (images is null || images.Count != _images.Count)
        {
            return DomainErrors.Products.ImagesCountMustMatchProductImagesCount;
        }

        foreach (var image in images)
        {
            // no need to check strictly here this is enough
            if (!_images.Any(x => x.FileName == image.FileName))
            {
                return DomainErrors.Products.ImagesNamesMustMatchProductImagesNames;
            }

        }

        Images = images;
        SortImages();
        return Result.Success;
    }

    public Result<Success> ApplyDiscount(DateOnly discountExpiresOn, byte discountPercentage)
    {
        if (!CanModify())
            return DomainErrors.Locked;

        if (OriginalPrice.Value < ProductRules.MinPriceToApplyADiscount)
        {
            return DomainErrors.Products.ProductPriceNotApplicableForDiscount;
        }

        var valResult = ValidateDiscountPercentage(discountPercentage);

        if (valResult.Failed)
            return valResult;

        var val2Result = ValidateDiscountExpiresOn(discountExpiresOn);

        if (val2Result.Failed)
            return val2Result;

        DiscountPrice = Money.Create(CalculateDiscount(OriginalPrice.Value, discountPercentage));
        this.DiscountExpiresOn = discountExpiresOn;
        this.DiscountPercentage = discountPercentage;
        this.CurrentPrice = DiscountPrice;
        this.HasDiscount = true;
        return Result.Success;
    }

    private static decimal CalculateDiscount(decimal price, byte discount)
    {
        return price * (100 - discount) / 100;
    }


    #region validators
    private static Result<Success> ValidatePrice(Money price)
    {
        if (ValHelper.IsOutOfRange(price.Value, ProductRules.MinPrice, ProductRules.MaxPrice))
        {
            return DomainErrors.Products.PriceOutOfRange;
        }
        return Result.Success;
    }

    private static Result<Success> ValidateDiscountPercentage(byte discountPercentage)
    {
        if (ValHelper.IsOutOfRange(discountPercentage, ProductRules.MinDiscountPercentageValue, ProductRules.MaxDiscountPercentageValue))
        {
            return DomainErrors.Products.DiscountValueOutOfRange;
        }
        return Result.Success;
    }

    private static Result<Success> ValidateDiscountExpiresOn(DateOnly discountExpiresOn)
    {
        if (!ValHelper.IsDateInFuture(discountExpiresOn))
        {
            return DomainErrors.Products.DateMustBeInFuture;
        }

        return Result.Success;
    }
    private static Result<Success> Validate_Width_Height_Length(int width, int height, int length)
    {
        if
        (
            ValHelper.IsOutOfRange(width, ProductRules.Min_Height_Width_Length_cm, ProductRules.Max_Height_Width_Length_cm) ||
            ValHelper.IsOutOfRange(height, ProductRules.Min_Height_Width_Length_cm, ProductRules.Max_Height_Width_Length_cm) ||
            ValHelper.IsOutOfRange(length, ProductRules.Min_Height_Width_Length_cm, ProductRules.Max_Height_Width_Length_cm)
        )
        {
            return DomainErrors.Products.ImagesNamesIsEmpty;
        }
        return Result.Success;
    }

    private static Result<Success> ValidateBarcode(string barcode)
    {
        if (string.IsNullOrEmpty(barcode))
        {
            return DomainErrors.Products.BarCodeRequired;
        }

        if (ValHelper.IsOutOfRange(barcode.Length, ProductRules.MinBarcodeLength, ProductRules.MaxBarcodeLength))
        {
            return DomainErrors.Products.BarCodeOutOfRange;
        }
        return Result.Success;
    }
    private static Result<Success> ValidateSlug(string slug)
    {

        if (string.IsNullOrEmpty(slug) || ValHelper.IsOutOfRange(slug.Length, ProductRules.MinSlugLength, ProductRules.MaxSlugLength))
        {
            return DomainErrors.Products.SlugLengthOutOfRange;
        }

        return Result.Success;
    }

    private static Result<Success> ValidateSku(string sku)
    {

        if (string.IsNullOrEmpty(sku) || ValHelper.IsOutOfRange(sku.Length, ProductRules.MinSkuLength, ProductRules.MaxSkuLength))
        {
            return DomainErrors.Products.SkuOutOfRange;
        }

        return Result.Success;
    }

    private static Result<Success> ValidateVariantSpecifications(Dictionary<string, string> specifications)
    {
        if (specifications is null || specifications.Count == 0)
        {
            return Result.Success;
        }

        if (specifications.Count > ProductRules.MaxNumberOfSpecifications)
        {
            return DomainErrors.Products.MaxAllowedSpecificationsNumberExceeded;
        }

        foreach (var spec in specifications)
        {
            bool invalidKey =
            string.IsNullOrEmpty(spec.Key) || ValHelper.IsOutOfRange(spec.Key.Length, ProductRules.MinSpecificationKeyLength, ProductRules.MaxSpecificationKeyLength);

            bool invalidValue = string.IsNullOrEmpty(spec.Value) ||
            ValHelper.IsOutOfRange(spec.Value.Length, ProductRules.MinSpecificationValueLength, ProductRules.MaxSpecificationValueLength);


            if (invalidKey & invalidValue)
            {
                return DomainErrors.Products.InvalidSpecification;
            }
        }

        return Result.Success;
    }
    #endregion
}
