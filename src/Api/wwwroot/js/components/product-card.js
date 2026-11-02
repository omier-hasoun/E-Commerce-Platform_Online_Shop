class ProductCard extends HTMLElement {
    #productId;
    #title;
    #brand;
    #inStock = false;
    #rating;

    #imageLoading;
    #imageSrc;
    #imageAlt; // Added semicolon

    #hasDiscount = false;
    #originalPrice;
    #discountPrice;
    #discountPercent;

    #isInitialized = false; // Flag to prevent duplicate event listener attachments

    static get observedAttributes() {
        return [
            'product-id',
            'title',
            'loading',
            'thumbnail',
            'brand',
            'instock',
            'rating',
            'hasdiscount',
            'original-price',
            'discount-price',
            'discount-percent'
        ];
    }

    /** @type {HTMLElement} */
    #articleElm = null;
    /** @type {ProductPrice} */
    #productPriceElm;

    CLICK_THRESHOLD = 25;
    #startX;
    #startY;
    #moved;

    attributeChangedCallback(name, oldValue, newValue) {
        if (oldValue === newValue) return;

        // Only re-render if the element is already connected to the DOM
        if (this.isConnected) {
            this.#readAttributes();
            this.#render();
        }
    }

    connectedCallback() {
        this.#readAttributes();
        this.#render();

        // Ensure event listeners are attached ONLY ONCE
        if (!this.#isInitialized) {
            this.#addEventListeners();
            this.#isInitialized = true;
        }
    }

    #dispatchEvents() {
        this.dispatchEvent(
            new CustomEvent("productclick", {
                bubbles: true,
                detail: {
                    productId: this.#productId
                }
            })
        );
    }

    #render() {
        const isAvailableText = this.#inStock ? "In stock" : "Unavailable";
        const isAvailableTextColor = this.#inStock ? "green" : "red";

        // Fixed string typo in data-element attribute
        this.innerHTML = `
        <article class="product-card" data-element="product-card">
            <figure>
                <img src="${this.#imageSrc}" alt="${this.#imageAlt}" loading="${this.#imageLoading}">
            </figure>
            <div class="card-info">
                <p class="card-brand">${this.#brand}</p>
                <h3 class="card-title">${this.#title}</h3>
                <span class="card-rating">${this.#rating} ⭐</span>
                <p class="card-stock-info" style="color:${isAvailableTextColor}">${isAvailableText}</p>
                <product-price data-element="product-price" class="card-price"></product-price>
            </div>
        </article>
        `;

        this.#articleElm = this.querySelector('[data-element="product-card"]');
        this.#productPriceElm = this.querySelector('[data-element="product-price"]');

        if (this.#productPriceElm) {
            this.#productPriceElm.setAttribute('original-price', this.#originalPrice ?? '');
            this.#productPriceElm.setAttribute('hasdiscount', this.#hasDiscount);
            this.#productPriceElm.setAttribute('discount-price', this.#discountPrice ?? '');
            this.#productPriceElm.setAttribute('discount-percent', this.#discountPercent ?? '');
        }

        this.style.cursor = "pointer";
    }

    #addEventListeners() {
        this.addEventListener('pointerdown', (e) => {
            this.#startX = e.clientX;
            this.#startY = e.clientY;
            this.#moved = false;
        });

        this.addEventListener('pointermove', (e) => {
            if (e.buttons !== 1) return;

            // Fixed subtraction syntax error
            const dx = e.clientX - this.#startX;
            const dy = e.clientY - this.#startY;

            if (Math.hypot(dx, dy) > this.CLICK_THRESHOLD) {
                this.#moved = true;
                this.style.cursor = "text";
            }
        });

        this.addEventListener('pointerup', () => {
            this.style.cursor = "pointer";

            if (this.#moved) return;

            this.#dispatchEvents();
        });

        this.addEventListener('pointerleave', () => {
            this.style.cursor = "pointer";
        });
    }

    #readAttributes() {
        this.#productId = this.getAttribute('product-id');

        this.#title = this.getAttribute('title') || 'Default Item';

        this.#imageLoading = this.getAttribute('loading') || 'lazy';
        this.#imageSrc = this.getAttribute('thumbnail');
        this.#imageAlt = this.getAttribute('title');

        this.#brand = this.getAttribute('brand') || '';

        this.#inStock = this.getAttribute('instock') === 'true';
        this.#rating = this.getAttribute('rating') || '0';

        this.#hasDiscount = this.getAttribute('hasdiscount') === 'true';
        this.#originalPrice = this.getAttribute('original-price');
        this.#discountPrice = this.getAttribute('discount-price');
        this.#discountPercent = this.getAttribute('discount-percent');
    }
}

customElements.define('product-card', ProductCard);
