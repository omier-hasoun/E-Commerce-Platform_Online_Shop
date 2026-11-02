
import { UserClient } from "../core/client/user-client.js";
import { Router } from '../core/router.js';

const userClient = new UserClient();
const router = new Router();


export async function init(){

    await redirectIfLoggedIn();
}

async function redirectIfLoggedIn(){
    const loggedIn = await userClient.isLoggedIn();

    if(loggedIn)
    {
        router.navigateToHome(null, false);
    }

}

document.addEventListener("submit", async (e) => {

    e.preventDefault();

    const registerMode = document.getElementById('tab-register').checked; // true/false

    let loggedIn;
    if(registerMode)
    {
        loggedIn = await register();
    }
    else
    {
        loggedIn = await login();
    }


    if(loggedIn)
    {
        const params = new URLSearchParams(location.search);
        const url = params.get("returnurl");

        if(url){
            router.replaceState("/")
            router.replaceUrl(url);
            return;
        }

        router.navigateToHome(null, false);
        return;
    }

})

async function register(){

    const email = document.getElementById('registerEmail').value;
    const password = document.getElementById('registerPassword').value;

    const response = await userClient.register(email, password);

    if (response.success === false)
    {
        document.getElementById('emailError').hidden = !response.dublicateEmail;
        document.getElementById('requiresLowerError').hidden = !response.passwordRequiresLower;
        document.getElementById('requiresUpperError').hidden = !response.passwordRequiresUpper;
        document.getElementById('requiresNonAlphanumericError').hidden = !response.passwordRequiresNonAlphanumeric;
        document.getElementById('requiresDigitError').hidden = !response.passwordRequiresDigit;
        document.getElementById('passwordTooShortError').hidden = !response.passwordTooShort;
        return false;
    }

    await userClient.login(email, password);
    const loggedIn = await userClient.isLoggedIn();

    return loggedIn;
}

async function login(){

    const email = document.getElementById('loginEmail').value;
    const password = document.getElementById('loginPassword').value;

    await userClient.login(email, password);


    const loggedIn = await userClient.isLoggedIn();

    const errorMessageElm = document.getElementById('loginError');

    errorMessageElm.hidden = loggedIn;

    return loggedIn;
}
