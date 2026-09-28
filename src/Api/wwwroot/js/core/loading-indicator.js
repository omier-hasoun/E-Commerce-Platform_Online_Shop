export class LoadingIndicator {

    /** @type {HTMLElement} */
    static show(element) {
        element.insertAdjacentHTML('afterbegin',`
            <div id="loading" class="loading">
                <span class="spinner">Loading...</span>
            </div>
        `);
    }

    static hide() {
    /** @type {HTMLElement} */
    document.getElementById('loading')?.remove();
    }
}
