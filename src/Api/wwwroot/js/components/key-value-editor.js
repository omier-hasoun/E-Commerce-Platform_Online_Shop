class KeyValueEditor extends HTMLElement
{
    #maxPairs = 50;
    #keyValuePairs = new Map();
    #form = null;
    /**
     * @type {HTMLDivElement}
     */
    #keyValueInputsContainer
    #addBtn;

    set keyValuePairs(value)
    {
        if(value instanceof Map)
        {
            this.#keyValuePairs = value;
            this.#render();
            return;
        }
        throw new Error("keyValuePairs must be an instance of Map");
    }

    get keyValuePairs()
    {
        this.#update();

        return this.#keyValuePairs;
    }

    connectedCallback()
    {
        this.#render();
    }

  #onFormData = (e) => {
    // Append or manipulate entries inside e.formData directly

    for (const [key, val] of this.keyValuePairs) {
      e.formData.append(key, val);
    }
  };

    #getCurrentRowsCount()
    {
        const rows = this.#keyValueInputsContainer.querySelectorAll('[name="key-value-row"]');

        return rows === null ? 0 : rows.length;
    }

    #update(){
        this.#clear();
        const keyValuePairs = this.#keyValueInputsContainer.querySelectorAll('[name="key-value-row"]');

        keyValuePairs?.forEach(kv => {

            const key = kv.querySelector('[name="key"]').value;
            const value = kv.querySelector('[name="value"]').value;

            this.#addValue(key, value);
        })
    }

    #clear()
    {
        this.#keyValuePairs.clear();
    }

    #addValue(key, value)
    {
        key = key.trim();
        value = value.trim();

        if(key === "" || value === "")
            return;//ignore without errors

        this.#keyValuePairs.set(key, value);
    }

    /**
     * @returns {HTMLDivElement}
     */
    #createKeyValueInput(key = "", value = "")
    {
        if(typeof(key) !== "string" || typeof(value) !== "string")
            throw new Error("key and value must be a string");

        const div = document.createElement("div");
        div.setAttribute("class", "key-value-row");
        div.setAttribute("name", "key-value-row");


        const k = document.createElement("input");

        k.setAttribute("type", "text");
        k.setAttribute("name", "key");
        k.value = key;
        k.placeholder = "attribute name";

        const v = document.createElement("input");


        v.setAttribute("type", "text");
        v.setAttribute("name", "value");
        v.value = value;
        v.placeholder = "attribute value";


        const btn = document.createElement("button");

        btn.setAttribute("class", "remove-key-value")
        btn.textContent = "❌";
        btn.type = "button";

        btn.addEventListener("click", () => {
            div.remove();

            this.#addBtn.disabled = false;
        });

        div.append(k, v, btn);

        return div;
    }

    #addKeyValueInput(key ="", value ="")
    {
        const rowsCount = this.#getCurrentRowsCount();
        if(rowsCount >= this.#maxPairs)
        {
            return;
        }

        const keyValueInput = this.#createKeyValueInput(key, value);
        this.#keyValueInputsContainer.insertAdjacentElement("beforeend", keyValueInput);

        if(rowsCount + 1 === this.#maxPairs)
            this.#addBtn.disabled = true;
    }

    #render(){

        this.replaceChildren();

        const div = document.createElement("div");
        div.className = "key-value-editor";

        this.#addBtn = document.createElement("button");

        this.#addBtn.textContent = "╋";

        this.#addBtn.className = "add-key-value";
        this.#addBtn.type = "button";

        this.#addBtn.addEventListener("click", () => {

            this.#addKeyValueInput();

        });

        this.#keyValueInputsContainer = document.createElement("div");

        this.#keyValueInputsContainer.className = "key-value-inputs";

        div.append(this.#keyValueInputsContainer, this.#addBtn);

        this.append(div);


        if(this.#keyValuePairs.size > 0)
        {
            this.#keyValuePairs.forEach((value, key) => {
                this.#addKeyValueInput(key, value);
            });
        }
    }
}

customElements.define('key-value-editor', KeyValueEditor);
