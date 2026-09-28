class CategorySelector extends HTMLElement
{
    #categories;

    set categories(value){
        this.#categories = value;
        this.#categories.forEach(c=> {

            this.#categoryOptions.append(this.#toCategoryOption(c));
        });
    }
    /** @type {HTMLSelectElement} */
    #categoryOptions;
    connectedCallback()
    {
        this.innerHTML = `
            <select name="categories-selector" data-element="categories" class="dropdown">
                <option data-id="all">All</option>
            </select>
        `;

        this.#categoryOptions = document.querySelector('[data-element="categories"]');
    }

    getSelectedCategory(){

        const value = this.#categoryOptions.options[this.#categoryOptions.selectedIndex];
        const id = value.getAttribute('data-id');
        return id === 'all' ? null : {
            id: value.getAttribute('data-id'),
            name: value.textContent
        };
    }


    select(id){
        if(id === null)
        {
            this.#categoryOptions.selectedIndex = 0;
            return;
        }
        /** @type {HTMLOptionElement} */
        const op = this.querySelector(`[data-id="${id}"]`);
        this.#categoryOptions.selectedIndex = op !== null ? op.index : 0;
    }


    #toCategoryOption(category){
        const option = document.createElement('option');
        option.setAttribute('data-id', category.id);
        option.textContent = category.name;
        return option;
    }
}
customElements.define('category-selector', CategorySelector);
