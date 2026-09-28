
class BrandSelector extends HTMLElement
{
    #brands;

    set brands(value){
        this.#brands = value;
        this.#brands.forEach(b => {

            this.#brandsOptions.append(this.#toBrandOption(b));
        });
    }
    /** @type {HTMLSelectElement} */
    #brandsOptions;
    connectedCallback()
    {
        this.innerHTML = `
            <select name="brand-selector" data-element="brands" class="dropdown">
                <option data-id="all">All</option>
            </select>
        `;


        this.#brandsOptions = document.querySelector('[data-element="brands"]');
    }

    getSelectedBrand(){

        const value = this.#brandsOptions.options[this.#brandsOptions.selectedIndex];
        const id = value.getAttribute('data-id');
        return id === 'all' ? null : {
            id: value.getAttribute('data-id'),
            name: value.textContent
        };
    }

    select(id){
        if(id === null)
        {
            this.#brandsOptions.selectedIndex = 0;
            return;
        }
    /** @type {HTMLOptionElement} */
        const op = this.querySelector(`[data-id="${id}"]`);
        this.#brandsOptions.selectedIndex = op !== null ? op.index : 0;
    }

    #toBrandOption(brand){
        const option = document.createElement('option');
        option.setAttribute('data-id', brand.id);
        option.textContent = brand.name;
        return option;
    }
}

customElements.define('brand-selector', BrandSelector);
