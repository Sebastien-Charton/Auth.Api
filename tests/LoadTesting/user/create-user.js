import http, { get } from "k6/http";
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import { randomString } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { getAuthApiBaseUrl, localEnvName } from './global-configs.js';

export const options = {
    iterations: 1
};

const baseUrl = getAuthApiBaseUrl(localEnvName);

function createNewUser() {
    describe("Create a new user", () => {

        let options = {
            headers: {
                'Content-type': 'application/json',
                'Accept-Language': 'en-US'
            },
        };

        // Create User
        let userName = `${randomString(10)}`
        let email = `${randomString(10)}@example.com`
        let password = "Password1!,";
        
        const createUserPayload = {
            email: email,
            password: password,
            userName: userName
        };
        
        let createUserResponse = http.post(`${baseUrl}user/register`, JSON.stringify(createUserPayload), options);
        
        expect(createUserResponse.status, 'user registered').to.equal(201);
        expect(createUserResponse, 'user registered valid body').to.have.validJsonBody();
        console.log(createUserResponse.body);
    });
}

export default function testSuite() {
    createNewUser();
}
