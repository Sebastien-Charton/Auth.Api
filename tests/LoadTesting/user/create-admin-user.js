import http, { get, options } from "k6/http";
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import { randomString } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { addAuthorizationToHeaders, basicOptions } from '../global-configs.js';

export function createNewAdminUser(baseUrl) {
    describe("Create a new admin user", () => {
        // Login as admin

        const loginUserPayload = {
            email: 'administrator@example.com',
            password: 'Administrator1!'
        };

        let loginUserResponse = http.post(`${baseUrl}user/login`, JSON.stringify(loginUserPayload), basicOptions);

        expect(loginUserResponse.status, 'user login').to.equal(200);
        expect(loginUserResponse, 'user login valid body').to.have.validJsonBody();

        const loginUserResult = JSON.parse(loginUserResponse.body);

        const optionsWithAuth = addAuthorizationToHeaders(loginUserResult.token);

        // create admin user
        let userName = `${randomString(10)}`
        let email = `${randomString(10)}@example.com`
        let password = "Password1!,";
        
        const createUserPayload = {
            email: email,
            password: password,
            userName: userName,
            roles: ["Administrator", "User"]
        };
        
        let createUserResponse = http.post(`${baseUrl}user/register-admin`, JSON.stringify(createUserPayload), optionsWithAuth);
        
        expect(createUserResponse.status, 'user registered').to.equal(201);
        expect(createUserResponse, 'user registered valid body').to.have.validJsonBody();
    });
}

