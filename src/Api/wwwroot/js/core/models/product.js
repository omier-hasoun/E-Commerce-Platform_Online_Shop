export class Product
{
  constructor(data = {}) {
    this.id = data.id;
    this.title = data.title;
    this.description = data.description;

    this.rating = data.averageRating;

    this.isSerialized = data.isSerialized;

    this.categoryId = data.categoryId;

    this.brandId = data.brandId;
    this.brand = data.brand;
    this.category = data.category;

    this.originalPrice = data.originalPrice;
    this.discountPrice = data.discountPrice;
    this.discountPercent = data.discountPercentage;
    this.hasDiscount = data.hasDiscount;

    this.images = data.images === null || !Array.isArray(data.images) ?  [{ id: 0, image: "media/images/products/default-image-medium.png", sortOrder: 1}]: data.images;
    this.specifications = new Map(
        Object.entries(data.specifications ?? {})
    );;

    this.slug = data.slug;

    this.inStock = Boolean(data.inStock ?? false);
    this.stockQuantity = data.stockQuantity;

    this.discountExpiresOn = data.discountExpiresOn;
    this.width = data.width;
    this.height = data.height;
    this.length = data.length;
    this.weight = data.weight;


    // these props exists only in Admin features
    this.barCode = data.barCode;
    this.sku = data.sku;
    this.status = data.status;
    this.isPublished = data.isPublished;
  }

}
