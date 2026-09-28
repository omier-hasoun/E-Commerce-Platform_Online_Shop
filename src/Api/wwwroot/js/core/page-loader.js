
export class PageLoader {

    constructor() {

    this.contentContainer = document.getElementById("content");
    this.parser = new DOMParser();

    }
/**
 * @param {string} pageUrl
 */
    async load(pageUrl, forceFullReload)
    {
        const response = await fetch(pageUrl);

        if (!response.ok) {
            throw new Error(`Failed to load page: ${response.status}`);
        }


        const raw = await response.text();
        const parsedHtml = this.parser.parseFromString(raw, "text/html");

        if(forceFullReload){
            document.documentElement.replaceWith(parsedHtml.documentElement);
            return;
        }

        const content = parsedHtml.getElementById("content");

        if (!content) {
            throw new Error("Page does not contain #content");
        }
        const pageName = parsedHtml.body.getAttribute("data-page");
        document.body.setAttribute('data-page', pageName);

        document.head.querySelector(`[data-page-css]`)?.remove();
        const cssElm = parsedHtml.head.querySelector(`[data-page-css]`)

        document.head.append(cssElm)

        document.getElementById("content")?.replaceChildren(...content.children)

    }

}
