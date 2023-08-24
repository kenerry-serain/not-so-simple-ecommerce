import axios, { AxiosResponse, AxiosRequestConfig } from "axios";

function createInstance(baseUrl: string) {
  const baseConfig = { baseURL: baseUrl };
  const instance = axios.create(baseConfig);

  instance.interceptors.request.use(async (request) => {

    request.headers = request.headers || {};
    request.headers["Content-type"] = "application/json";

    return request;
  });

  instance.interceptors.response.use((response: AxiosResponse) => {
    return response;
  });

  return instance;
}


export const http = createInstance("base-url");
