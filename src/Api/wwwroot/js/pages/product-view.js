
import '/js/components/product-gallery.js';
import '/js/components/product-price.js';


import { ProductClient } from "../core/client/product-client.js";
import { ProductGroup } from '../core/models/product-group.js';
import { LoadingIndicator } from '../core/loading-indicator.js';



const client = new ProductClient();
export async function init()
{

    loadProduct(await getProduct());
}

async function getProduct(){
    LoadingIndicator.show(document.getElementById('content'))
    const path = window.location.pathname;
    const match = path.match(/^\/products\/(\d+)$/);
    const product = await client.getProduct(match[1]);
    return product;
}
/**
 *
 * @param {ProductGroup} productGroup
 */
function loadProduct(productGroup){
    console.log('product with id ' + productGroup.requestedProductId + ' was loaded successfully.');
    LoadingIndicator.hide();
    const titleElm = document.getElementById('productTitle');
    const descriptionElm = document.getElementById('productDescription');
    // const categoryElm = document.getElementById('productCategory');

    const specificationsElm = document.getElementById('productSpecifications');
    // const variantSpecificaitonsElm = document.getElementById('productVariantSpecificaitons');
    const addtocartBtn = document.getElementById('addtocartBtn');


    const galleryElm = document.getElementById('productGallery');
    galleryElm.images = productGroup.requestedProduct.images;


    const ratingElm = document.getElementById('productRating');
    const priceElm = document.getElementById('productPrice');
    const stockElm = document.getElementById('productStock');

    priceElm.setAttribute('discountPercentage', productGroup.requestedProduct.discountPercentage);
    priceElm.setAttribute('discountPrice', productGroup.requestedProduct.discountPrice);
    priceElm.setAttribute('originalPrice', productGroup.requestedProduct.originalPrice);
    priceElm.setAttribute('hasDiscount', productGroup.requestedProduct.hasDiscount);

    priceElm.render();

    titleElm.textContent = productGroup.brand + " " + productGroup.title;

    descriptionElm.innerHTML = productGroup.description;

    ratingElm.innerText = productGroup.rating + " ⭐";
    stockElm.textContent = productGroup.requestedProduct.inStock ? "In stock" : "currently unavailable";
    stockElm.style.color = productGroup.requestedProduct.inStock ? "green" : "red";
    addtocartBtn.hidden = !productGroup.requestedProduct.inStock;

    if(productGroup.specifications.size === 0)
        specificationsElm.remove();
    else{

        productGroup.specifications.forEach((value, key) => {
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
