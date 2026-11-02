import { PageLoader } from "./page-loader.js";

export class Router {

    #loader = new PageLoader();

    get url()
    {
        return window.location.href;
    }

    get pathname()
    {
        return window.location.pathname;
    }

    get searchParamsString()
    {
        return window.location.search;
    }

    get searchParamsObj()
    {
        return new URLSearchParams(window.location.search);
    }

    constructor()
    {

    }

/**
 * @param {URL | string} url
 */
    replaceState(url){
        const search = this.searchParamsString === "?null" ? "" : this.searchParamsString;

        const href = url instanceof URL ? url : new URL(url, window.location.origin);
        href.search = search;

        history.replaceState(null, "", href);
    }

   async load(page, forceFullReload = false) {

    await this.#loader.load(`/${page}.html`, forceFullReload);
    const pageModule = await import(`/js/pages/${page}.js`);

    await pageModule.init();
  }

/**
 * @param {URL | string} url
 */
  pushState(url){

    const href = url instanceof URL ? url : new URL(url, window.location.origin);

    href.search = href.search === "?null" ? "" : href.search;
    history.pushState(null, '', href);
  }

  async loadCurrentPage(){

    const path = window.location.pathname;

    const searchParams =  new URLSearchParams(window.location.search);

    if (path === '/'){
        await this.navigateToHome(searchParams, false);
    }

    else if (path === "/products")
    {
        await this.navigateToProducts(searchParams, false);
    }

    else if (path.match(/^\/products\/(\d+)/))
    {
        // productId
        const match = path.match(/^\/products\/(\d+)/);
        await this.navigateToProductView(match[1], searchParams, false);
    }

    else if (path === "/products/create")
    {
        await this.navigateToProductEditor("create", null, searchParams, false);
    }

    else if (path.match(/^\/products\/edit\/(\d+)$/))
    {
        const match = path.match(/^\/products\/edit\/(\d+)$/);

        await this.navigateToProductEditor("update", match[1], searchParams, false);
    }

    else if (path === "/login")
    {
        await this.navigateToLogin(searchParams, false)
    }

    window.scrollTo(0,0);

  }

    #handleUrlState(path, searchParams = null, pushstate = true)
    {
        const url = new URL(path, window.location.origin);

        if(searchParams)
            url.search = searchParams;

        if(pushstate)
        {
            this.pushState(url);
        }
        else
        {
            this.replaceState(url);
        }
    }

    async navigateToProducts(searchParams = null, pushstate = true){

        this.#handleUrlState('/products', searchParams, pushstate)

        await this.load('products', false);
    }

    async navigateToHome(searchParams = null, pushstate = true){

        this.#handleUrlState('/', searchParams, pushstate)

        await this.load('home', true);
    }

    async navigateToProductView(productId, searchParams = null, pushstate = true){

        this.#handleUrlState(`/products/${productId}`, searchParams, pushstate)

        await this.load('product-view', false);
    }

    async navigateToProductEditor(mode, id=null, searchParams = null, pushstate = true){

        let path;
        if(mode === "update" && id !== null)
        {
            path = `/products/edit/${id}`;
        }
        else if (mode === "create")
        {
            path = `/products/create`;
        }
        else
        {
            throw new Error("no mode was provided to view product editor page");
        }
        this.#handleUrlState(path, searchParams, pushstate);

        await this.load('product-editor', true);
    }

    async navigateToLogin(searchParams = null, pushstate = true){

        this.#handleUrlState('/login', searchParams, pushstate);
        await this.load('login', true);
    }

    replaceUrl(url){

        window.location.href = url;
    }
}
