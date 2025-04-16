import axios, { AxiosInstance, AxiosResponse } from 'axios';

// Defining a type for the error response if needed
// type ApiError = {
//   message: string;
//   code?: string;
// };

class ApiService {
  private api: AxiosInstance;

  constructor(sessionId: string) {
    this.api = axios.create({
      baseURL: 'https://localhost:7100/api/',
      headers: {
        'Content-Type': 'application/json',
        'X-Session-Id': sessionId,
      },
    });
  }

  setAuthToken(token?: string): void {
    if (token) {
      this.api.defaults.headers['Authorization'] = `Bearer ${token}`;
    } else {
      delete this.api.defaults.headers['Authorization'];
    }
  }

  async get<T>(url: string, params: Record<string, any> = {}, responseType: 'json' | 'blob' = 'json'): Promise<T> {
    try {
      const response: AxiosResponse<T> = await this.api.get(url, { params, responseType });
      return responseType === 'blob' ? response.data : response.data;
    } catch (error: any) {
      this.handleError(error);
      throw error;  // Ensuring the error is rethrown after handling it.
    }
  }

  async post<T>(url: string, data: Record<string, any>): Promise<T> {
    try {
      const response: AxiosResponse<T> = await this.api.post(url, data);
      return response.data;
    } catch (error: any) {
      this.handleError(error);
      throw error;
    }
  }

  private handleError(error: any): void {
    console.log('API Error:', error?.config?.url + ', Error: ' + error?.response?.data || error?.message);
    console.error('API Error:', error?.response?.data || error?.message);
  }
}

export default (sessionId: string): ApiService => new ApiService(sessionId);
