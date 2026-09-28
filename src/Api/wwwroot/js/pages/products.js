import '/js/components/app-dialog.js';
import '/js/components/app-paginator.js';
import '/js/components/product-card.js';
import '/js/components/product-price.js';

import { ProductClient } from "../core/client/product-client.js";

// eslint-disable-next-line no-unused-vars
import { BrandClient } from '../core/client/brand-client.js';
import { CategoryClient } from '../core/client/category-client.js';
import { LoadingIndicator } from "../core/loading-indicator.js";
import { ProductListItem } from "../core/models/product-list-item.js";
import { Router } from '../core/router.js';


const productClient = new ProductClient();
const brandsClient = new BrandClient();
const categoryClient = new CategoryClient();


let productsContainer = document.getElementById("productsContainer");
let paginator;
let filtersBtn;
let submitFiltersBtn;
let filtersDialog;


const router = new Router();

document.addEventListener("productclick", productClickedHandler);

function productClickedHandler(event){
    const productId = event.detail.productId;

    router.navigateToProductView(productId);
}

function initializeDom(){

    /** @type {AppDialog} */
    filtersDialog = document.getElementById('filtersDialog');

    filtersBtn = document.getElementById('showFiltersBtn');

    const maxPriceElm = document.getElementById('maxPriceInput');
    const categoryElm = document.getElementById('categorySelector');
    const brandElm = document.getElementById('brandSelector');
    const discountElm = document.getElementById('discount');
    const goodRatingOnlyElm = document.getElementById('goodRatingOnly');

    filtersBtn.addEventListener("click", (e) =>{
        const searchParams = new URLSearchParams(window.location.search);

        maxPriceElm.value = searchParams.get('maxPrice') || "";
        categoryElm.select(searchParams.get('category') || null);
        brandElm.select(searchParams.get('brand')|| null);
        discountElm.checked = searchParams.get('discount') || false ==="true";
        goodRatingOnlyElm.checked = searchParams.get('goodRatingOnly') || false ==="true";


        filtersDialog.showModal();
    });

    submitFiltersBtn =  document.getElementById('submitFiltersBtn');

    submitFiltersBtn.addEventListener('click', async () =>{
        //sync url with filter values
        const maxPrice = maxPriceElm.value || 0;
        const category = categoryElm.getSelectedCategory();
        const brand = brandElm.getSelectedBrand();
        const goodRatingOnly = goodRatingOnlyElm.checked;
        const discount = discountElm.checked;

        const url = new URL(window.location.href);

        if (brand)
            url.searchParams.set('brand', brand.id);
        else
            url.searchParams.delete('brand');

        if (category)
            url.searchParams.set('category', category.id);
        else
            url.searchParams.delete('category');

        if (discount)
            url.searchParams.set('discount', "true");
        else
            url.searchParams.delete('discount');

        if (goodRatingOnly)
            url.searchParams.set('goodRatingOnly', "true");
        else
            url.searchParams.delete('goodRatingOnly');

        if (maxPrice > 0)
            url.searchParams.set('maxPrice', maxPrice);
        else
            url.searchParams.delete('maxPrice');

        if(url.href !== window.location.href)
            router.pushState(url);

        await loadProducts();
        filtersDialog.close();

    } )

    paginator = document.getElementById('paginator');

    paginator.addEventListener("pagechange", (event) => {
        const url = new URL(window.location.href);

        url.searchParams.set("page", event.detail.page);
        router.pushState(url);
        router.loadCurrentPage();
    });
}



export async function init()
{
    initializeDom();
    await loadBrands();
    await loadCategories();
    await loadProducts();
}

async function loadBrands(){
    const brandSelector = document.getElementById('brandSelector');
    brandSelector.brands = await brandsClient.getBrands();

    console.log(brandSelector.getSelectedBrand());
}

async function loadCategories(){
    const categorySelector = document.getElementById('categorySelector');

    categorySelector.categories = await categoryClient.getCategories();
    console.log(categorySelector.getSelectedCategory());
}

async function loadProducts() {
    productsContainer?.replaceChildren('');
    LoadingIndicator.show(productsContainer);

    const urlParams = new URLSearchParams(location.search);
    try
    {
        const productsPage = await productClient.getProducts(
                                            urlParams.get('q'),
                                            urlParams.get('page'),
                                            urlParams.get('size'),
                                            urlParams.get('maxPrice'),
                                            urlParams.get('discount') ==="true",
                                            urlParams.get('goodRatingOnly') ==="true",
                                            urlParams.get('category'),
                                            urlParams.get('brand'));

        productsContainer = document.getElementById("productsContainer");

        if (productsPage.size > 0)
            renderProducts(productsPage.products)
        else
        {
            productsContainer.textContent = "No matching products was found";
        }

        paginator.setState(productsPage.page, productsPage.hasMore)

        console.log(productsPage.size + " products loaded successfully");
    } catch (error) {
        console.error("Failed to load products:", error);
    }
    LoadingIndicator.hide();
}

/**
 *
 * @param {ProductListItem[]} products
 */
function renderProducts(products)
{
    productsContainer= document.getElementById("productsContainer");
    productsContainer.replaceChildren([]);
    products.forEach((product, index) => {

        const loading = index <= 3 ? 'eager' : 'lazy';
        const card = document.createElement('product-card');
        card.classList.add('product-card-aspect');
        if (product.hasDiscount) {
            card.setAttribute('hasDiscount', 'true');
        }
        card.setAttribute('id', product.id);
        card.setAttribute('title', product.title);
        card.setAttribute('brand', product.brand);
        card.setAttribute('thumbnail', product.thumbnail);

        card.setAttribute('rating', product.rating);
        card.setAttribute('inStock', product.inStock);
        card.setAttribute('originalPrice', product.originalPrice);
        card.setAttribute('discountPrice', product.discountPrice);

        card.setAttribute('discountPercentage', product.discountPercentage);
        card.setAttribute('loading', loading);
        productsContainer.appendChild(card);
        card.render();

    });


}
