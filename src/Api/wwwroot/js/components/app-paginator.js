class AppPaginator extends HTMLElement {
    prevBtn;
    nextBtn;
    currentPage;
    #hasNextPage = false;
    #currentPageNumber = 1;

    connectedCallback() {
        this.innerHTML = `
            <nav class="app-paginator">
                <button class="borderless-btn app-paginator-btn" data-action="prev">Prev</button>
                <span class="app-paginator-current-page" data-element="current-page"></span>
                <button class="borderless-btn app-paginator-btn" data-action="next">Next</button>
            </nav>
        `;

        this.prevBtn = this.querySelector('[data-action="prev"]');
        this.nextBtn = this.querySelector('[data-action="next"]');
        this.currentPage = this.querySelector('[data-element="current-page"]');

        this.nextBtn.disabled = true;
        this.prevBtn.disabled = true;
        this.currentPage.textContent = this.#currentPageNumber;
        this.#attachEvents();
    }

    setState(currentPage, hasNextPage) {

        this.#hasNextPage = hasNextPage
        this.#currentPageNumber = currentPage;
        this.#render();
    }

    #attachEvents() {
        this.prevBtn.addEventListener('click', () => {
            this.#previousPage();
        });

        this.nextBtn.addEventListener('click', () => {
            this.#nextPage();
        });
    }

    #previousPage() {

        this.#currentPageNumber--;
        this.#render();
        this.#dispatchPageChange();
    }

    #nextPage()
    {
        this.#currentPageNumber++;
        this.#render();
        this.#dispatchPageChange();
    }

    #render() {
        this.currentPage.textContent = this.#currentPageNumber;
        this.prevBtn.disabled = this.#currentPageNumber === 1;
        this.nextBtn.disabled = !this.#hasNextPage;
    }

    #dispatchPageChange() {
        this.dispatchEvent(
            new CustomEvent("pagechange", {
                detail: {
                    page: this.#currentPageNumber

                }
        }));
    }
}

customElements.define('app-paginator', AppPaginator);
