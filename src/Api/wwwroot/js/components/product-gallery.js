class ProductGallery extends HTMLElement
{
    #imageViewerElm;
    #currentImageIndexElm;
    #prevBtnElm;
    #nextBtnElm;

    #images =[];
    #currentImageIndex = 0;

    set images(value){
        this.#images = value.sort((a, b) => a.sortOrder - b.sortOrder);
        this.#nextBtnElm.disabled = this.#images.length <= 1;
        this.#prevBtnElm.disabled = this.#images.length <= 1;

        this.#setCurrentImage(0);
    }
    connectedCallback()
    {

        this.innerHTML = `
<div id="productGallery" class="product-gallery">
  <div class="imageview"><img data-element="current-image" alt="Product image"></div>
  <button data-action="prev" type="button" class="rounded-btn left-btn" aria-label="Previous image">&#129120;</button>
  <span class="image-index" data-element="current-image-index"></span>
  <button data-action="next" type="button" class="rounded-btn right-btn" aria-label="Next image">&#129122;</button>
</div>
        `;
        this.#prevBtnElm = this.querySelector('[data-action="prev"]');
        this.#nextBtnElm = this.querySelector('[data-action="next"]');
        this.#imageViewerElm = this.querySelector('[data-element="current-image"]');
        this.#currentImageIndexElm = this.querySelector('[data-element="current-image-index"]');

        this.#imageViewerElm.setAttribute('loading', 'eager');
        this.#imageViewerElm.setAttribute('fetchpriority', 'medium');
        this.#attachEvents();

   }

    #attachEvents() {
        this.#prevBtnElm.addEventListener('click', () => {
            this.#setCurrentImage(this.#currentImageIndex - 1);
        });

        this.#nextBtnElm.addEventListener('click', () => {
            this.#setCurrentImage(this.#currentImageIndex + 1);
        });
    }

    #setCurrentImage(index) {

        const lastImageIndex = this.#images.length-1 | 0;

        this.#currentImageIndex = index > lastImageIndex ? 0 :
                                 index < 0 ? lastImageIndex : index;

        this.#render();
    }

    #render(){
        this.#imageViewerElm.setAttribute('src', `/${this.#images.at(this.#currentImageIndex).image}`);
        this.#imageViewerElm.setAttribute('alt', this.getAttribute('alt') || 'product image');
        this.#currentImageIndexElm.textContent = `${this.#images[this.#currentImageIndex].sortOrder}/${this.#images.length}`;
    }

}

customElements.define('product-gallery', ProductGallery);
