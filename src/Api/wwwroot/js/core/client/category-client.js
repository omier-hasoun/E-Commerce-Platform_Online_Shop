
import {CategoryListItem  } from "../models/category-list-item.js";


export class CategoryClient
{
    constructor() {

    }

    async getCategories() {
        const response = await fetch('/api/categories');
        const json = await response.json();

        return json.map(c =>
             new CategoryListItem(c)
        );

    }
}
