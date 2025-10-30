import { User } from "../backendmodels";
import { Login } from "../backendmodels";

const API_BASE_URL = "http://localhost:5232";

export async function loginUser(username: string, password: string) {
    const loginData = new Login();
    loginData.username = username;
    loginData.password = password;


    const response = await fetch(`${API_BASE_URL}/users/login`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(loginData)
    });
    if(!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
    const data: User = await response.json();
    return data;
}