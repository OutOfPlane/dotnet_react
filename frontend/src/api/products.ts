import { Product } from "../backendmodels";

const API_BASE_URL = "/api";

export async function getProduct(id: number) {
    const response = await fetch(`${API_BASE_URL}/products/${id}`);
    if(!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
    const data: Product = await response.json();
    return data;
}