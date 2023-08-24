import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import App from './App';
import Stock from './pages/stock/Index';
import Product from './pages/product/Index';
import Login from './pages/login/Index';
import Order from './pages/order/Index';
import Report from './pages/report/Index';

import './index.css';
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

const queryClient = new QueryClient();
const router = createBrowserRouter([{
  path: '/',
  element: <App />,
  children: [
    {
      path: 'stock',
      element: <Stock />
    },
    {
      path: 'product',
      element: <Product />
    },
    {
      path: 'order',
      element: <Order />
    },
    {
      path: 'login',
      element: <Login />
    },
    {
      path: 'report',
      element: <Report />
    },
    {
      path: '*',
      element: <Login />
    }]
}])

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  </React.StrictMode>,
)
