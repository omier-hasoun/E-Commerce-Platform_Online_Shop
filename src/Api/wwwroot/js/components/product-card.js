class ProductCard extends HTMLElement
{
    /** @type {HTMLElement} */
    #productCardElm;
    #productId = "";
    #productPriceElm;

    #startX;
    #startY;
    #moved;
    CLICK_THRESHOLD = 25;
  connectedCallback()
  {
    const id = this.getAttribute('id');
    if (!id)
        throw new Error("product id missing");

    this.#productId = id;

    const title = this.getAttribute('title') || 'Default Item';
    const loading = this.getAttribute('loading') || 'lazy';
    const image = this.getAttribute('thumbnail');
    const brand = this.getAttribute('brand') || '';
    const inStock = this.getAttribute('inStock') === 'true';
    const textColor = inStock ? "green" : "red";
    const isAvailableText = inStock ? "In Stock" : "Unavailable";

        this.innerHTML = `
 <article class="product-card" data-element="product"->
        <figure>
            <img src="${image}" alt="${title}" loading="${loading}">
        </figure>
        <div class="card-info">
            <p class="card-brand">${brand}</p>
            <h3 class="card-title">${title}</h3>
            ${this.createRating()}
            <p class="card-stock-info" style="color:${textColor}">${isAvailableText}</p>
            <product-price data-element="product-price" class="card-price"></product-price>
        </div>
    </article>
    `;

    this.#productCardElm = this.querySelector('[data-element="product"]');
    this.#productPriceElm = this.querySelector('[data-element="product-price"]');
    const hasDiscount = this.getAttribute('hasDiscount') === 'true';
    const originalPrice = this.getAttribute('originalPrice');

    const discountPrice = this.getAttribute('discountPrice');
    const discountPercentage = this.getAttribute('discountPercentage');
    this.#productPriceElm.setAttribute('originalprice', originalPrice);
    this.#productPriceElm.setAttribute('hasdiscount', hasDiscount);
    this.#productPriceElm.setAttribute('discountprice', discountPrice);
    this.#productPriceElm.setAttribute('discountpercentage', discountPercentage);
    this.#productPriceElm.render();

    this.#productCardElm.addEventListener('pointerdown',(e) => {
        this.#startX = e.clientX;
        this.#startY = e.clientY;
        this.#moved = false;

    });

    this.#productCardElm.addEventListener('pointermove',(e) => {
        if(e.buttons !== 1){
            return;
        }
        const dx = e.clientX - this.#startX;
        const dy= e.clientY - this.#startY;

        if(Math.hypot(dx, dy) > this.CLICK_THRESHOLD)
        {
            this.#moved = true;
            this.#productCardElm.style.cursor = "text";
        }
    });
    this.#productCardElm.addEventListener('pointerup',(e) => {
        this.#productCardElm.style.cursor = "pointer";

        if(this.#moved)
        {

            return;
        }
        this.#dispatchPageChange();
    });

    this.#productCardElm.addEventListener('pointerleave',(e) => {
            this.#productCardElm.style.cursor = "pointer";
    });

  }

#dispatchPageChange() {
    this.dispatchEvent(
        new CustomEvent("productclick", {
            bubbles: true,
            detail: {
                productId: this.#productId
            }
        })
    );
}

#dispatch

createRating() {
    const rating = this.getAttribute('rating') || '0';

    return  `<span class="card-rating">${rating} ⭐</span>`;
}

render(){
    this.connectedCallback();
}

}
customElements.define('product-card', ProductCard);
