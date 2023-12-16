import http, { get } from "k6/http";
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import { addAuthorizationToHeaders,basicOptions } from '../global-configs.js';

export function getUserById(baseUrl) {
    describe("Get user by id", () => {
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

        // get user by id

        let getUserByIdResponse = http.get(`${baseUrl}user/${loginUserResult.userId}`, optionsWithAuth);

        expect(getUserByIdResponse.status, 'get user by id').to.equal(200);
        expect(getUserByIdResponse, 'get user by id valid body').to.have.validJsonBody();
    });
}