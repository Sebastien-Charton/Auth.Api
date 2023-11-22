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


let session = new Httpx({baseURL: 'https://localhost:5001/api/'});

function createNewUser() {
    describe("Create a new User", () => {

        let userName = `${randomString(10)}`
        let email = `${randomString(10)}@example.com`
        let password = "Password1!,";

        const options = {
            headers: {
                'Content-type': 'application/json',
                'Accept-Language': 'en-US'
            },
        };

        const payload = {
            'email': email,
            'password': password,
            'userName': userName
        };

        let response = http.post(`https://localhost:5001/api/user/register`, JSON.stringify(payload), options);
        // let response = session.post('user/register', payload, options);

        expect(response.status).to.equal(201);
        expect(response).to.have.validJsonBody();
    });
}

export default function testSuite() {
    createNewUser();
}
