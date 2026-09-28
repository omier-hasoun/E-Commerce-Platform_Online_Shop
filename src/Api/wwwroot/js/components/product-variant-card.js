class ProductVariantCard extends HTMLElement
{
    connectedCallback()
    {
        this.innerHTML = `
        `;
    }
}

customElements.define('product-variant-card', ProductVariantCard);
