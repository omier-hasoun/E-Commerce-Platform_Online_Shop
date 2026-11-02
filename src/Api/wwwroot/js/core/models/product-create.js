
export class ProductCreate{

    constructor(images, imagesDetails, title, description, categoryId, brandId, isSerialized, price, inventory, width, height, length, weight, slug, barcode, sku, specifications) {
        this.images = images;
        this.imagesDetails = imagesDetails;
        this.title = title;
        this.description = description;
        this.categoryId = categoryId;
        this.brandId = brandId;
        this.isSerialized = isSerialized;
        this.price = price;
        this.inventory = inventory;
        this.width = width;
        this.height = height;
        this.length = length;
        this.weight = weight;
        this.slug = slug;
        this.barcode = barcode;
        this.sku = sku;
        this.specifications = specifications !== undefined ? Object.fromEntries(specifications): undefined;
    }
}
