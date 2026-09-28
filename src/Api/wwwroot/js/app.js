
import '/js/components/app-footer.js';
import '/js/components/app-header.js';
import '/js/components/brand-selector.js';
import '/js/components/category-selector.js';

import { Router } from './core/router.js';

const router = new Router();
const size = 25;
const page = 1;

document.addEventListener("querysubmit", async (event) => {

    const filters = {
        q: event.detail.text?.trim(),
        size: size,
        page: page,
    }

    await router.navigateToProducts(new URLSearchParams(filters));
});

async function initializePage() {

    const text = new URLSearchParams(location.search).get('q');
    document.querySelector("app-header").setSearchInput(text);
    await router.loadCurrentPage();
}

window.addEventListener("DOMContentLoaded", async () => await initializePage());

window.addEventListener("popstate", async () => await initializePage());
