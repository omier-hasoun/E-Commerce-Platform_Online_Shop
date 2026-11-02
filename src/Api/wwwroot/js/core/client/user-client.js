
// import { User } from "../models/brand-list-item.js";

export class UserClient
{
    constructor() {

    }

    /**
     *
     * @param {string} email
     * @param {string} password
     */
    async login(email, password)
    {
        const url = new URL("/api/auth/login", window.location.origin);
        url.searchParams.append("useCookies", "true");
        try
        {
            await fetch(url, {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                credentials: "include",
                body: JSON.stringify({
                    email: email,
                    password: password,
                    twoFactorCode: null,
                    twoFactorRecoveryCode: null
                })
            });



        }
        catch (error)
        {
            console.log(error);
        }
    }

        /**
     *
     * @param {*} email
     * @param {*} password
     * @returns
     */
    async register(email, password)
    {
        const url = new URL("/api/auth/register", window.location.origin);

        try
        {
            const response = await fetch(url, {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json"
                    },
                    credentials: "include",
                    body: JSON.stringify({
                        email: email,
                        password: password
                })
            });

            if(response.ok)
            {
                return {
                    success: true
                };
            }
            const json = await response.json();
            const errors = json.errors;

            return {
                success: false,
                passwordTooShort: errors.passwordTooShort !== undefined,
                passwordRequiresLower: errors.PasswordRequiresLower !== undefined,
                passwordRequiresUpper: errors.PasswordRequiresUpper !== undefined,
                passwordRequiresNonAlphanumeric: errors.PasswordRequiresNonAlphanumeric !== undefined,
                passwordRequiresDigit: errors.PasswordRequiresDigit !== undefined,
                dublicateEmail: errors.DuplicateUserName !== undefined
            };
        }
        catch (error) {
            console.log(error);
        }

        return {
            success: false
        };
    }


    /**
     *
     * @returns {boolean}
     */
    async isLoggedIn()
    {
        try
        {
            const response = await fetch("/api/auth/manage/info");

            return response.ok;

        } catch (error)
        {
            console.log(error);
        }

        return false;
    }

    /**
     *
     * @returns {boolean}
     */
    async isEmailConfirmed()
    {
        try
        {
            const response = await fetch("/api/auth/manage/info");
            const data = await response.json();

            return data.isEmailConfirmed;

        } catch (error)
        {
            console.log(error);
        }

        return false;

    }



}
