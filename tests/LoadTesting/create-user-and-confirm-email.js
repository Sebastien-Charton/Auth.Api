import http from "k6/http";
import {describe, expect} from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import {Httpx} from 'https://jslib.k6.io/httpx/0.0.6/index.js';
import {randomString} from 'https://jslib.k6.io/k6-utils/1.0.0/index.js';

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

// let session = new Httpx({baseURL: 'https://localhost:5001/api/'});

const baseUrl = 'https://localhost:5001/api/user/';

function createNewUser() {
    describe("Create a new User and validate email", () => {


        // Create User
        let userName = `${randomString(10)}`
        let email = `${randomString(10)}@example.com`
        let password = "Password1!,";

        let options = {
            headers: {
                'Content-type': 'application/json',
                'Accept-Language': 'en-US'
            },
        };

        const createUserPayload = {
            email: email,
            password: password,
            userName: userName
        };

        let createUserResponse = http.post(`${baseUrl}register`, JSON.stringify(createUserPayload), options);
        // let createUserResponse = session.post('user/register', payload, options);

        expect(createUserResponse.status, 'user registered').to.equal(201);
        expect(createUserResponse, 'user registered valid body').to.have.validJsonBody();

        var userId = JSON.parse(createUserResponse.body);

        // Login User

        const loginUserPayload = {
            email: email,
            password: password
        };

        let loginUserResponse = http.post(`${baseUrl}login`, JSON.stringify(loginUserPayload), options);

        expect(loginUserResponse.status, 'user login').to.equal(200);
        expect(loginUserResponse, 'user login valid body').to.have.validJsonBody();

        let authToken = JSON.parse(loginUserResponse.body).token;

        expect(authToken, `auth token is ${authToken}`).to.be.a('string');

        // Get email confirmation token

        let headersWithBearer = {
            headers: {
                'Content-type': 'application/json',
                'Accept-Language': 'en-US',
                'Authorization': `Bearer ${authToken}`
            }
        };

        let getEmailConfirmationTokenResponse = http.post(`${baseUrl}confirmation-email-token/${userId}`,null, headersWithBearer);

        expect(getEmailConfirmationTokenResponse.status, 'email confirmation').to.equal(200);
        expect(getEmailConfirmationTokenResponse, 'email confirmation valid body').to.have.validJsonBody();

        var emailToken = JSON.parse(getEmailConfirmationTokenResponse.body).token;

        // Confirm email

        const confirmEmailPayload = {
            userId: userId,
            token: emailToken
        };

        let confirmEmailResponse = http.post(`${baseUrl}confirm-email`, JSON.stringify(confirmEmailPayload), headersWithBearer);

        expect(confirmEmailResponse.status, 'confirm email').to.equal(200);
        expect(confirmEmailResponse, 'confirm email valid body').to.have.validJsonBody();
        expect(JSON.parse(confirmEmailResponse.body), 'email confirmed').to.equal(true);
    });
}

export default function testSuite() {
    createNewUser();
}
