
import { BrandListItem } from "../models/brand-list-item.js";

export class BrandClient
{
    constructor() {

    }

    async getBrands() {
        const response = await fetch('/api/brands');
        const json = await response.json();

        return json.map(brand =>
             new BrandListItem(brand)
        );

    }
}
