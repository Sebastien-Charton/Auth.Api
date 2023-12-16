import { getAuthApiBaseUrl, localEnvName} from "../global-configs.js";
import { confirmEmail } from "./confirm-email.js";
import { createNewUser } from "./create-user.js";
import { loginUser } from "./login-user.js";

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

export default function testSuite() {
    confirmEmail(baseUrl);
    createNewUser(baseUrl);
    loginUser(baseUrl);
}