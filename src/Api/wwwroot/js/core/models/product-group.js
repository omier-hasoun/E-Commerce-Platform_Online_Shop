import { Product } from "./product.js";

/**
 *
 * @property {product[]} products
 */
export class ProductGroup{
    /**
     *
     * @param {*} data
     */
    constructor(data) {
        this.requestedProductId = data.requestedProductId;
        this.mainProductId = data.mainProductId;
        this.title = data.title ?? 'Untitled Product';
        this.description = data.description;
        this.brand = data.brand ?? '';
        this.category = data.category ?? '';

        this.rating = Number(data.averageRating ?? 0);
        this.specifications =new Map(Object.entries(data.specifications));
        this.products = data.products.map(p => new Product(p));
        this.requestedProduct = this.products.find(p => p.id === this.requestedProductId);
    }
}
