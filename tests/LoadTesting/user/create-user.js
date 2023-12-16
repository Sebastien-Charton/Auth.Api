import http, { get } from "k6/http";
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import { randomString } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { basicOptions } from '../global-configs.js';

export function createNewUser(baseUrl) {
    describe("Create a new user", () => {
        let userName = `${randomString(10)}`
        let email = `${randomString(10)}@example.com`
        let password = "Password1!,";
        
        const createUserPayload = {
            email: email,
            password: password,
            userName: userName
        };
        
        let createUserResponse = http.post(`${baseUrl}user/register`, JSON.stringify(createUserPayload), basicOptions);
        
        expect(createUserResponse.status, 'user registered').to.equal(201);
        expect(createUserResponse, 'user registered valid body').to.have.validJsonBody();
    });
}

