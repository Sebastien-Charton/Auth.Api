export function getAuthApiBaseUrl(environment){
    switch (environment) {
        case 'local':
            return 'https://localhost:5001/api/';

    }
}

export let basicOptions = {
    headers: {
        'Content-type': 'application/json',
        'Accept-Language': 'en-US'
    },
};

export function addAuthorizationToHeaders(authToken){
    basicOptions.headers.Authorization = `Bearer ${authToken}`;
    return basicOptions;
}

export const localEnvName = 'local';
