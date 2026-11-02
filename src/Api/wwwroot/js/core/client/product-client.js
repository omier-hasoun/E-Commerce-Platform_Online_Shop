import { ProductListItem } from "/js/core/models/product-list-item.js";
import { Product } from "/js/core/models/product.js";
import {ProductCreate  } from "../models/product-create.js";


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
    async getProductsForAdmin(query, page=1, size=25, maxPrice = 0, discount = false, goodRatingOnly = false, category= null, brand = null, statuses = null)
    {
        const url = new URL( '/api/management/products', window.location.origin);

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

        try
        {
            const response = await fetch(url);
            const json = await response.json();

            return {
                products: json.items.map((item) => new ProductListItem(item)),
                page: json.page,
                size: json.size,
                hasMore: json.hasMore,
            };
        }
        catch
        {
            return null;
        }


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

        try
        {
            const response = await fetch(url);
            const json = await response.json();

            return {
                products: json.items.map((item) => new ProductListItem(item)),
                page: json.page,
                size: json.size,
                hasMore: json.hasMore,
            };
        }
        catch
        {
            return null;
        }


    }

/**
 * @param {string} id
 * @returns {Promise<Product>}
 */
    async getProductById(id)
    {
        const url = new URL( `/api/products/${id}`, window.location.origin);

        try
        {
            const response = await fetch(url);
            const json = await response.json();
            return new Product(json);
        }
        catch
        {
            return null;
        }

    }

    /**
 * @param {string} id
 * @returns {Promise<Product>}
 */
    async getProductByIdForAdmin(id)
    {
        const url = new URL( `/api/management/products/${id}`, window.location.origin);

        try
        {
            const response = await fetch(url);
            const json = await response.json();
            return new Product(json);
        }
        catch
        {
            return null;
        }

    }


    /**
     *
     * @param {ProductCreate} formData
     * @returns {Promise<string>}
     */
    async createProduct(formData) {
        const url = new URL(`/api/management/products`, window.location.origin);

        try {
            const response = await fetch(url, {
                method: "POST",
                credentials: "include",
                body: formData
                // DO NOT set "Content-Type" here. The browser sets it automatically with the boundary.
            });

            if (!response.ok) {
                const errorText = await response.text();
                console.error('Server validation error:', errorText);
                return null;
            }

            const json = await response.json();
            return json.id;
        } catch (error) {
            console.error('Fetch error:', error);
            return null;
        }
    }

    async updateProduct(productId, formData)
    {
        const url = new URL(`/api/management/products/${productId}`, window.location.origin);



        try {
            const response = await fetch(url, {
                method: "PUT",
                credentials: "include",
                body: formData
            });

            if (!response.ok ) {
                const errorText = await response.text();
                console.error('Server validation error:', errorText);
            }



        } catch (error) {
            console.error('Fetch error:', error);
        }
    }

}
