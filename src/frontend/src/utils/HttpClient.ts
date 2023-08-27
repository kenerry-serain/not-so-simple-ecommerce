import axios, { AxiosResponse } from "axios";

function createInstance(baseUrl: string) {
  const baseConfig = { baseURL: baseUrl };
  const instance = axios.create(baseConfig);
  instance.interceptors.response.use((response) => {
    console.log('response', response)
    // if (response.status === 401) 
    //   window.location.assign('login');
    
    return response;
  });
  instance.interceptors.request.use(async (request) => {
    console.log('request')
    request.headers = request.headers || {};
    request.headers["Content-type"] = "application/json";
    request.headers["Authorization"] = `Bearer ${localStorage.getItem("jwtToken")}`;
    return request;
  }, error => {
    return Promise.reject(error);
  });

 

  return instance;
}


export const http = createInstance(import.meta.env.VITE_APP_BASE_URL!);
