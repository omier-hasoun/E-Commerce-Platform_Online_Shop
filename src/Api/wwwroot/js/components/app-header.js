class AppHeader extends HTMLElement
{
    #searchForm;
    #searchInput;
    #searchBtn;
    #loginLink;


    connectedCallback()
    {
        this.innerHTML =  `
    <header>
        <div class="website-icon-container">
            <a href="/">
                <img class="website-icon" src="/favicon.ico" alt="Omier's E-Commerce Website">
            </a>
        </div>

    <nav id="mainMenuNav" class="h-nav" aria-label="main-menu" >
        <ul >
            <li><a href="/">Home</a></li>
            <li><a href="#orders">Orders</a></li>
            <li><a href="#profile">Profile</a></li>
            <li><a id="loginLink" href="/login">Login</a></li>
        </ul>
    </nav>

    <form id="hsearchForm" class="h-search-form">
            <input id="hsearchInput" class="h-search-input" type="search" name="q" placeholder="Search products..." maxlength="49" required>
            <button id="hsearchBtn" class="h-search-btn"  type="submit" disabled>&#128269</button>
    </form>
</header>
        `;

        this.#searchForm = this.querySelector("#hsearchForm");
        this.#searchBtn = this.querySelector("#hsearchBtn");
        this.#searchInput = this.querySelector("#hsearchInput");
        this.#loginLink = this.querySelector("#loginLink");


        this.#searchInput.addEventListener("input", () => {
            const hasText = this.#searchInput.value.trim().length >= 1;
            this.#searchBtn.disabled  = !hasText;
        });

        this.#searchBtn.addEventListener("click", (e) => {
            e.preventDefault();

            this.#searchBtn.disabled = true;

            this.dispatchEvent(new CustomEvent("querysubmit", {
               bubbles: true,
                detail: {
                    text : this.#searchInput.value
                }
            }));

        })

        this.#loginLink.addEventListener("click", (e) => {
            e.preventDefault();

            location.href = `/login?returnurl=${location.pathname + location.search}`;

        })

    }

    setSearchInput(value){
        this.#searchInput.value = value;
    }

}

customElements.define('app-header', AppHeader);
