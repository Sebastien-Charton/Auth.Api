import http from "k6/http";
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import { randomString } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { addAuthorizationToHeaders, getAuthApiBaseUrl, localEnvName, basicOptions } from "../global-configs.js";

export const options = {
    iterations: 1
    // stages: [
    //     {duration: '1m', target: 10},
    //     {duration: '1m', target: 20},
    //     {duration: '1m', target: 0},
    // ],
    // thresholds: {
    //     http_req_failed: ['rate<0.01'], // http errors should be less than 1%
    //     http_req_duration: ['p(95)<200'], // 95% of requests should be below 200ms
    // },
};

const baseUrl = getAuthApiBaseUrl(localEnvName);

function confirmEmail() {
    describe("Create a new User and confirm email", () => {

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

        let authToken = JSON.parse(loginUserResponse.body).token;

        expect(authToken, `auth token is ${authToken}`).to.be.a('string');

        console.log(`auth token is ${authToken}`);
        // Get email confirmation token

        let optionWithAuth = addAuthorizationToHeaders(authToken);

        console.log(`optionWithAuth is ${JSON.stringify(optionWithAuth)}`);

        let getEmailConfirmationTokenResponse = http.post(`${baseUrl}user/confirmation-email-token`, null, optionWithAuth);

        expect(getEmailConfirmationTokenResponse.status, 'email confirmation').to.equal(200);
        expect(getEmailConfirmationTokenResponse, 'email confirmation valid body').to.have.validJsonBody();

        var emailToken = JSON.parse(getEmailConfirmationTokenResponse.body).token;

        // Confirm email

        const confirmEmailPayload = {
            userId: JSON.parse(loginUserResponse.body).id,
            token: emailToken
        };

        let confirmEmailResponse = http.post(`${baseUrl}user/confirm-email`, JSON.stringify(confirmEmailPayload), optionWithAuth);

        expect(confirmEmailResponse.status, 'confirm email').to.equal(204);
    });
}

export default function testSuite() {
    confirmEmail();
}
