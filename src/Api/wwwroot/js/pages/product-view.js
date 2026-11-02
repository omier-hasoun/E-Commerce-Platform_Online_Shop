
import '/js/components/product-gallery.js';
import '/js/components/product-price.js';


import { ProductClient } from "../core/client/product-client.js";
import { LoadingIndicator } from '../core/loading-indicator.js';
import { Product } from '../core/models/product.js';
import { Router } from '../core/router.js';



const client = new ProductClient();
let product = new Product();
let router = new Router();
export async function init()
{
    product = await loadProduct();
    render();
    updateUrlToDisplaySlug();
}

async function loadProduct(){
    LoadingIndicator.show(document.getElementById('content'))
    const path = window.location.pathname;
    const match = path.match(/^\/products\/(\d+)/);
    const product = await client.getProduct(match[1]);
    return product;
}

function updateUrlToDisplaySlug()
{
    let url = window.location.pathname + "/" + product.slug + window.location.search;
    router.replaceState(url);
}

function render(){
    LoadingIndicator.hide();

    console.log('product with id ' + product.id + ' was loaded successfully.');
    const titleElm = document.getElementById('productTitle');
    const descriptionElm = document.getElementById('productDescription');

    const specificationsElm = document.getElementById('productSpecifications');

    const addtocartBtn = document.getElementById('addtocartBtn');


    const galleryElm = document.getElementById('productGallery');
    galleryElm.images = product.images;


    const ratingElm = document.getElementById('productRating');
    const priceElm = document.getElementById('productPrice');
    const stockElm = document.getElementById('productStock');

    priceElm.setAttribute('discount-percent', product.discountPercent);
    priceElm.setAttribute('discount-price', product.discountPrice);
    priceElm.setAttribute('original-price', product.originalPrice);
    priceElm.setAttribute('hasdiscount', product.hasDiscount);

    titleElm.textContent = product.brand + " " + product.title;

    descriptionElm.innerHTML = product.description;

    ratingElm.innerText = product.rating + " ⭐";
    stockElm.textContent = product.inStock ? "In stock" : "currently unavailable";
    stockElm.style.color = product.inStock ? "green" : "red";
    addtocartBtn.hidden = !product.inStock;

    if(product.specifications.size === 0)
        specificationsElm.remove();
    else{

        product.specifications.forEach((value, key) => {
            specificationsElm.appendChild(renderSpecification(key, value));
        });
    }


}

function renderSpecification(key, value){
    const div = document.createElement('div');
    const dt = document.createElement('dt');
    const b = document.createElement('b');

    b.textContent = `${key}:\t\t`;
    dt.appendChild(b);
    const dd = document.createElement('dd');
    dd.textContent = value;
    div.appendChild(dt);
    div.appendChild(dd);
    div.classList.add('spec')
    return div;
}
