class DropDownList extends HTMLElement {

    static formAssociated = true;

    #required = false;
    #items = [];
    #valueKey = 'id';
    #labelKey = 'name';

    #internals;
    /** @type {HTMLSelectElement|null} */
    #selectElement = null;

    static get observedAttributes() {
        return ['name', 'required', 'placeholder', 'none-label', 'value-key', 'label-key'];
    }

    constructor() {
        super();
        this.#internals = this.attachInternals();
    }

    connectedCallback() {
        this.#readAttributes();
        this.#render();
    }

    attributeChangedCallback() {
        this.#readAttributes();
        if (this.#selectElement) {
            this.#render();
        }
        else{
            throw new Error("SelectElement missing in drop-down-list");
        }
    }

    get form() { return this.#internals.form; }
    get name() { return this.getAttribute('name'); }
    get type() { return 'drop-down-list'; }
    get validity() { return this.#internals.validity; }
    get validationMessage() { return this.#internals.validationMessage; }
    get willValidate() { return this.#internals.willValidate; }
    checkValidity() { return this.#internals.checkValidity(); }
    reportValidity() { return this.#internals.reportValidity(); }

    set items(value) {
        this.#items = Array.isArray(value) ? value : [];
        this.#render();
    }

    get items() {
        return this.#items;
    }

    get value() {
        if (!this.#selectElement) return null;
        const selectedOption = this.#selectElement.options[this.#selectElement.selectedIndex];
        return selectedOption && selectedOption.value !== "" ? selectedOption.value : null;
    }

    set value(val) {
        this.select(val);
    }

    getSelectedOption() {
        if (!this.#selectElement) return null;
        const index = this.#selectElement.selectedIndex;
        if (index === -1) return null;

        const option = this.#selectElement.options[index];
        const val = option.value;

        if (val === "") return null;

        const rawItem = this.#items.find(item => String(this.#getItemValue(item)) === val);

        return {
            id: val,
            name: option.textContent,
            item: rawItem ?? null
        };
    }

    select(id) {
        if (!this.#selectElement) return;

        if (id === null || id === undefined || id === "") {
            this.#selectElement.selectedIndex = 0;
        } else {
            const targetValue = String(id).toLowerCase();
            const option = Array.from(this.#selectElement.options).find(op => op.value === targetValue);
            this.#selectElement.selectedIndex = option ? option.index : 0;
        }

        this.#syncFormState();
    }

    #readAttributes() {
        this.#required = this.hasAttribute('required');
        this.#valueKey = this.getAttribute('value-key') || 'id';
        this.#labelKey = this.getAttribute('label-key') || 'name';
    }

    #getItemValue(item) {
        if (typeof item === 'object' && item !== null) {
            return item[this.#valueKey] ?? item.value ?? item.id ?? '';
        }
        return item;
    }

    #getItemLabel(item) {
        if (typeof item === 'object' && item !== null) {
            return item[this.#labelKey] ?? item.label ?? item.name ?? '';
        }
        return item;
    }

    #toOptionElement(item) {
        const option = document.createElement('option');
        option.value = this.#getItemValue(item);
        option.textContent = this.#getItemLabel(item);
        return option;
    }

    #syncFormState() {
        const val = this.value;
        this.#internals.setFormValue(val ?? '');

        if (this.#required && !val) {
            this.#internals.setValidity(
                { valueMissing: true },
                'Please select an item in the list.',
                this.#selectElement
            );
        } else {
            this.#internals.setValidity({});
        }
    }

    #render() {
        const placeholderText = this.getAttribute('placeholder') || 'Select an Option';
        const noneLabelText = this.getAttribute('none-label') || 'No Option';

        const currentValue = this.value;

        this.innerHTML = `
            <select data-element="select" class="drop-down-list-default">
                <option value="" selected disabled hidden>${placeholderText}</option>
            </select>
        `;

        this.#selectElement = this.querySelector('[data-element="select"]');

        if (!this.#required) {
            const noneOption = document.createElement('option');
            noneOption.value = '';
            noneOption.textContent = noneLabelText;
            this.#selectElement.append(noneOption);
        }

        if (this.#items.length > 0) {
            this.#items.forEach(item => {
                this.#selectElement.append(this.#toOptionElement(item));
            });
        }

        if (currentValue !== null) {
            this.select(currentValue);
        } else {
            this.#syncFormState();
        }

        this.#selectElement.addEventListener('change', () => {
            this.#syncFormState();
            this.dispatchEvent(new Event('change', { bubbles: true }));
        });
    }
}

customElements.define('drop-down-list', DropDownList);
