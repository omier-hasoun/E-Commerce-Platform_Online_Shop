
import '/js/components/key-value-editor.js';
import '/js/components/images-editor.js';


import { BrandClient } from '../core/client/brand-client.js';
import { CategoryClient } from '../core/client/category-client.js';
import { ProductClient } from '../core/client/product-client.js';
import { Router } from '../core/router.js';

const brandsClient = new BrandClient();
const categoryClient = new CategoryClient();
const productClient = new ProductClient();
const router = new Router();

let mode;

export async function init(){

    mode = isCreateMode() ? "create" : isUpdateMode() ? "update": "invalidMode";

    await loadBrands();
    await loadCategories();

    if(mode === "create")
    {
        DisplayBtnToFillDataForTesting();
        createNewProductMode();
        return;
    }

    if(mode === "update")
    {
        await updateProductMode();
        return;
    }

    throw new Error("Unexpected error.");
}

document.addEventListener("submit", async (e) => {
    e.preventDefault();
    getSubmitBtn().disabled = true;
    const form = e.target;

    if (!(form instanceof HTMLFormElement)) return;

    const formData = new FormData(form);

    const kvEditor = e.target.querySelector("key-value-editor");
    const imgEditor = e.target.querySelector("images-editor");

    const specifications = kvEditor?.keyValuePairs ?? [];
    const images = imgEditor?.getData() ?? {
        imagesDetails: [],
        files: []
    };

    const specsObj = {};

    if (Array.isArray(specifications)) {
        specifications.forEach(({ key, value }) => {
            if (key) specsObj[key] = value;
        });
    } else if (specifications && typeof specifications === "object") {
        Object.assign(specsObj, specifications);
    }

    formData.set(
        "ImagesDetailsJson",
        JSON.stringify(images.imagesDetails)
    );

    formData.set("Specifications", JSON.stringify(specsObj));

    for (const file of images.files) {
        formData.append("ImagesFiles", file);
    }

    const checkbox = e.target.querySelector("#isSerialized");

    formData.set("isSerialized", checkbox.checked);

    try {
        if(mode === "create")
        {
            const productId = await productClient.createProduct(formData);
            router.navigateToProductEditor("update", productId, null, false);
            console.log(productId);
            return;
        }

        // update mode
        await productClient.updateProduct(getProductId(), formData);

    } catch (error) {
        console.error("Product creation failed:", error);
    }
});

function getForm(){

    return document.getElementById("productEditorForm");
}

function getSubmitBtn(){

    return document.getElementById("submitFormBtn");
}

function getProductId(){
    return router.pathname.match(/^\/products\/edit\/(\d+)$/)[1];
}

function createNewProductMode(){

    const submitBtn = getSubmitBtn();

    submitBtn.textContent = "Create";
    submitBtn.disabled = false;




}



async function updateProductMode(){

    const form = getForm();
    const submitBtn = getSubmitBtn();

    submitBtn.textContent = "Update";
    submitBtn.disabled = true;

    const productId = getProductId();

    const product = await productClient.getProductByIdForAdmin(productId);

    DisplayProduct(product);

    form.addEventListener("change", (e) => {
            console.log("Changed:", e.target.name, e.target.value);
            submitBtn.disabled = false;
    });

}

function isCreateMode(){

    return router.pathname === "/products/create";
}

function isUpdateMode(){

    const match = router.pathname.match(/^\/products\/edit\/(\d+)$/);

    return match !== null;
}

async function loadBrands(){
    const brandSelector = document.getElementById('brandSelector');
    brandSelector.items = await brandsClient.getBrands();

}

async function loadCategories(){
    const categorySelector = document.getElementById('categorySelector');

    categorySelector.items = await categoryClient.getCategories();
}


function DisplayBtnToFillDataForTesting(){

    const container = document.createElement("div");
    const autoFillBtn = document.createElement("button");

    autoFillBtn.type = "button";
    autoFillBtn.textContent ="auto fill for testing purposes.";
    autoFillBtn.className = "rounded-btn";

    autoFillBtn.addEventListener("click", () => {

        const randomNumber = Math.floor(1000 + Math.random() * 9000);

        const specs = new Map();
        specs.set("Color", "blue");
        specs.set("size", "large");
        specs.set("easy to use", "yes");

        const title = `Random Product ${randomNumber}`;
        const product = {
            title: title,
            description:`Test product ${randomNumber}. High-quality product for everyday use.`,
            brandId:"019DD6B8-96D0-734B-97CA-02B8094E7F00",
            categoryId: "1",
            isSerialized: true,
            originalPrice:(Math.random() * 900 + 10).toFixed(2),
            sku: `SKU-${randomNumber}`,
            barCode:`${Date.now()}`.slice(-13),
            slug: title.toLowerCase().replace(/[^a-z0-9]+/g, "-").replace(/^-|-$/g, ""),
            width: Math.floor(Math.random() * 50) + 10,
            height: Math.floor(Math.random() * 50) + 10,
            length:Math.floor(Math.random() * 50) + 5,
            weight:Math.floor(Math.random() * 5000) + 100,
            specifications: specs,
            stockQuantity:0

        }
        DisplayProduct(product);

    });
    container.className = "product-editor-buttons"
    container.append(autoFillBtn);
    getForm().append(container);
}

function DisplayProduct(product)
{
    const setValue = (id, value) => {
        document.getElementById(id).value = value;
    };

    setValue("titleInput", product.title);
    setValue(
        "descriptionInput",
        product.description
    );

    setValue("priceInput", product.originalPrice);

    setValue("skuInput", product.sku);
    setValue("barCodeInput", product.barCode);
    setValue(
        "slugInput",
        product.slug
    );

    setValue("stockQuantity", product.stockQuantity);

    document.getElementById(
        "isSerialized"
    ).checked = product.isSerialized;

    setValue("widthInput", product.width);
    setValue("heightInput", product.height);
    setValue("lengthInput", product.length);
    setValue("weightInput", product.weight);

    const dropdown = document.getElementById("categorySelector");
    dropdown.select(product.categoryId);

    const dropdown2 = document.getElementById("brandSelector");
    dropdown2.select(product.brandId);

    document.getElementById("keyValueEditor").keyValuePairs = product.specifications;
    document.getElementById("imagesEditor").images = product.images;
}
