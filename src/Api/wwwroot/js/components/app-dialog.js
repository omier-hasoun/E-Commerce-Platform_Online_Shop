class AppDialog extends HTMLElement
{
    /** @type {HTMLButtonElement} */
    #closeDialogBtn;
    /** @type {HTMLDialogElement} */
    #dialog;
    connectedCallback()
    {

        this.#dialog = document.createElement('dialog');

        this.#closeDialogBtn = document.createElement('button');
        this.#closeDialogBtn.setAttribute('id', 'closeDialogBtn');
        this.#closeDialogBtn.setAttribute('type', 'button');

        this.#closeDialogBtn.innerHTML = '&#10060;';
        this.#closeDialogBtn.addEventListener("click", () => {
            this.close();
        });

        this.#dialog.append(this.#closeDialogBtn);

        this.#dialog.append(...this.children);

        this.innerHTML = ''
        this.append(this.#dialog);

    }

    showModal(){
        this.#dialog.showModal();
        this.style.display = "block";
    }

    close(){
        this.#dialog.close();
        this.style.display = "none";
    }

}

customElements.define('app-dialog', AppDialog);
