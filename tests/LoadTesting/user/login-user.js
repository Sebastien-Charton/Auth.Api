import http, { get } from "k6/http";
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import { randomString } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { basicOptions, getAuthApiBaseUrl, localEnvName } from '../global-configs.js';

export const options = {
    iterations: 1
};

const baseUrl = getAuthApiBaseUrl(localEnvName);

function loginUser() {
    describe("Login user", () => {
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

        const loginUserPayload = {
            email: email,
            password: password
        };

        let loginUserResponse = http.post(`${baseUrl}user/login`, JSON.stringify(loginUserPayload), basicOptions);

        expect(loginUserResponse.status, 'user login').to.equal(200);
        expect(loginUserResponse, 'user login valid body').to.have.validJsonBody();
    });
}

export default function testSuite() {
    loginUser();
}
