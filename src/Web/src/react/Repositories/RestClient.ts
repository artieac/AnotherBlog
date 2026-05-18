import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse, AxiosError } from 'axios';

class RestClient {
    private client: AxiosInstance;

    constructor() {
        this.client = axios.create({
            headers: {
                'Content-Type': 'application/json',
            },
        });

        this.setupInterceptors();
    }

    private setupInterceptors() {
        this.client.interceptors.response.use(
            (response) => response,
            (error: AxiosError) => {
                if (error.response) {
                    const status = error.response.status;
                    if (status === 401) {
                        // Handle unauthorized (e.g., redirect to login or clear auth state)
                        console.error('Unauthorized access - 401');
                    } else if (status === 403) {
                        console.error('Forbidden access - 403');
                    } else if (status === 500) {
                        console.error('Server error - 500');
                    }
                }
                return Promise.reject(this.normalizeError(error));
            }
        );
    }

    private normalizeError(error: AxiosError): any {
        return {
            message: (error.response?.data as any)?.message || error.message || 'An unexpected error occurred',
            status: error.response?.status,
            data: error.response?.data,
        };
    }

    public async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
        const response: AxiosResponse<T> = await this.client.get(url, config);
        return response.data;
    }

    public async post<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
        const response: AxiosResponse<T> = await this.client.post(url, data, config);
        return response.data;
    }

    public async put<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
        const response: AxiosResponse<T> = await this.client.put(url, data, config);
        return response.data;
    }

    public async delete<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
        const response: AxiosResponse<T> = await this.client.delete(url, config);
        return response.data;
    }
}

export default new RestClient();
