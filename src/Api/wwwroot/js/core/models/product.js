export class Product
{
  constructor(data = {}) {
    this.id = data.id ?? 0;
    this.originalPrice = data.originalPrice;
    this.discountPrice = data.discountPrice;
    this.discountPercentage = data.discountPercentage;
    this.hasDiscount = data.hasDiscount;
    this.inStock = Boolean(data.inStock ?? false);
    this.images = data.images;
    this.specifications = data.variantSpecifications;
  }

}
