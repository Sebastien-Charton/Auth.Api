export function getAuthApiBaseUrl(environment){
    switch (environment) {
        case 'local':
            return 'https://localhost:5001/api/';

    }
}

export const localEnvName = 'local';
