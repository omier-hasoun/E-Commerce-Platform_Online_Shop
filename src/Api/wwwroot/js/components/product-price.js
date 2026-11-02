class ProductPrice extends HTMLElement
{

    #hasDiscount = false;
    #originalPrice;
    #discountPrice;
    #discountPercent;
    #renderDiscountPercent;

    /**
     * @type {boolean}
     */
    set renderDiscountPercent(value){
        this.#renderDiscountPercent = value;
        this.setAttribute("render-discount-percent", this.#renderDiscountPercent);//will automatically rerender
    }

    static get observedAttributes() {
        return [

            'hasdiscount',
            'original-price',
            'discount-price',
            'discount-percent',
            'render-discount-percent'
        ];
    }

    attributeChangedCallback(name, oldValue, newValue) {

        if(oldValue === newValue)
            return;

        this.#readAttributes();

        this.#render();
    }


    connectedCallback()
    {
        this.#render();

    }

    #createElm() {

        if (this.#hasDiscount === false)
        {
            return `
                <p class="price">$${this.#originalPrice}</p>
            `;
        }

        return `
                <span id="discountPercent" class="discount-percentage" hidden="${this.#renderDiscountPercent}">${this.#discountPercent}% OFF</span>
                <del class="original-price">$${this.#originalPrice}</del>
                <ins class="discount-price">$${this.#discountPrice}</ins>
            `;
    }

    #readAttributes()
    {
        this.#discountPercent = this.getAttribute("discount-percent") || 0;
        this.#discountPrice = this.getAttribute("discount-price") || 0;
        this.#originalPrice = this.getAttribute("original-price") || 0;
        this.#hasDiscount = this.getAttribute("hasdiscount") === "true";
        this.#renderDiscountPercent = this.getAttribute("render-discount-percent") === "true";

    }


   #render(){
        this.#readAttributes();

        this.innerHTML = `${this.#createElm()}`;
   }

}

customElements.define('product-price', ProductPrice);
