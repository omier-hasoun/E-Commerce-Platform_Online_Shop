import { ProductListItem } from "/js/core/models/product-list-item.js";
import { ProductGroup } from "/js/core/models/product-group.js";

export class ProductClient
{
    constructor() {

    }
/**
 * @typedef {Object} ProductsResult
 * @property {Product[]} products
 * @property {number} page
 * @property {number} size
 * @property {boolean} hasMore
 */

/**
 * @param {string} query
 * @param {number} page
 * @param {number} size
 * @returns {Promise<ProductsResult>}
 */
    async getProducts(query, page=1, size=25, maxPrice = 0, discount = false, goodRatingOnly = false, category= null, brand = null, )
    {
        const url = new URL( '/api/products', window.location.origin);

        if(query && query.length < 50)
            url.searchParams.append('searchQuery', query);
        
        if(page > 0){
            url.searchParams.append('page', page);
        }

        if(size > 0 && size < 50){
            url.searchParams.append('size', size);
        }


        if(brand)
            url.searchParams.append('brandId', brand);

        if(category)
            url.searchParams.append('categoryId', category);

        if(maxPrice > 0)
            url.searchParams.append('maxPrice', maxPrice);

        if(discount === true)
            url.searchParams.append('discountedProductsOnly', discount);

        if(goodRatingOnly ===true)
            url.searchParams.append('goodRatedProductsOnly', goodRatingOnly);

        const response = await fetch(url);
        const json = await response.json();

        return {
            products: json.items.map((item) => new ProductListItem(item)),
            page: json.page,
            size: json.size,
            hasMore: json.hasMore,
        };
    }

/**
 * @param {string} id
 * @returns {Promise<ProductGroup>}
 */
    async getProduct(id)
    {
        const url = new URL( `/api/products/${id}`, window.location.origin);

        const response = await fetch(url);
        const json = await response.json();
        return new ProductGroup(json);
    }

}
