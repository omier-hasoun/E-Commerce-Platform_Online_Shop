class ProductPrice extends HTMLElement
{
    /** @type {HTMLSpanElement} */
    discountPercentageElm;
    connectedCallback()
    {
        this.innerHTML = `${this.createPrice()}`;
        this.discountPercentageElm = this.querySelector('#discountPercentage');

        if(this.discountPercentageElm)
            this.discountPercentageElm.hidden = true;

    }

    createPrice() {

        const hasDiscount = this.getAttribute('hasdiscount') === 'true';
        const originalPrice = this.getAttribute('originalprice') ?? 0;

        if (hasDiscount === false)
        {
            return `
                <p class="price">$${originalPrice}</p>
            `;
        }

        const discountPrice = this.getAttribute('discountprice') ?? 0;
        const discountPercentage = this.getAttribute('discountpercentage') ?? 0;

        return `
                <span id="discountPercentage" class="discount-percentage">${discountPercentage}% OFF</span>
                <del class="original-price">$${originalPrice}</del>
                <ins class="discount-price">$${discountPrice}</ins>
            `;


    }

    showDiscountPercentage(){
        this.discountPercentageElm.hidden = false;

    }

   render(){
    this.connectedCallback();
   }

}

customElements.define('product-price', ProductPrice);
