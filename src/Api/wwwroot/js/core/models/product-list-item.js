export class ProductListItem
{
  constructor(data = {}) {
    this.id = data.id ?? 0;
    this.title = data.title ?? data.productName ?? 'Untitled Product';
    this.brand = data.brand ?? '';
    this.rating = Number(data.averageRating ?? 0);
    this.originalPrice = Number(data.originalPrice ?? 0);
    this.discountPrice = Number(data.discountPrice ?? 0 );
    this.discountPercentage = Number(data.discountPercentage ?? 0 );
    this.hasDiscount = Boolean(data.hasDiscount ?? false);
    this.inStock = Boolean(data.inStock ?? false);
    this.thumbnail = data.thumbnailUrl ?? '/media/images/products/default-image-small.png';
  }

}
