import { UseQueryResult, useMutation, useQuery } from "@tanstack/react-query";
import { OrderEntity } from "../types/Stock.type";
import { http } from "../utils/HttpClient";

export const getOrders = async () => {
  const { data } = await http.get<OrderEntity[]>("/order/api/request");
  return data;
};

export const useOrder = (): UseQueryResult<OrderEntity[]> =>
  useQuery({
    queryKey: ["order"],
    queryFn: getOrders,
  });

export type OrderRequest = {
  productId: number;
  quantity: number;
};

export const useCreateOrder = () =>
  useMutation({
    mutationKey: ["createOrder"],
    mutationFn: (body: OrderRequest) =>
      http.post(`/order/api/request`, body),
    onSuccess: () => {},
    onError: () => {},
  });

export const useUpdateOrder = () =>
  useMutation({
    mutationFn: (variables: { id: number; body: OrderRequest }) =>
      http.put(`/order/api/request/${variables.id}`, variables.body),
    onSuccess: () => {},
    onError: () => {},
  });

export const useDeleteOrder = () =>
  useMutation({
    mutationKey: ["deleteOrder"],
    mutationFn:  (variables: { id: number })  =>
      http.delete(`/order/api/request/${variables.id}`),
    onSuccess: () => {},
    onError: () => {},
  });
