import { UseQueryResult, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { StockEntity } from "../types/Stock.type";
import { http } from "../utils/HttpClient";

export const getStocks = async () => {
  const { data } = await http.get<StockEntity[]>("/main/api/stock");
  return data;
};

export const useStock = (): UseQueryResult<StockEntity[]> =>
  useQuery({
    queryKey: ["stock"],
    queryFn: getStocks,
  });

export type StockRequest = {
  quantity: number;
};

export const useCreateStock = () =>
  useMutation({
    mutationKey: ["createStock"],
    mutationFn: (variables: { id: number; body: StockRequest }) =>
      http.post(`/main/api/product/${variables.id}/stock`, variables.body),
    onSuccess: () => {
    },
    onError: () => {},
  });

export const useUpdateStock = () =>
  useMutation({
    mutationKey: ["updateStock"],
    mutationFn: (variables: { id: number; body: StockRequest }) =>
      http.put(`/main/api/product/${variables.id}/stock`, variables.body),
    onSuccess: () => {
    },
    onError: () => {},
  });

export const useDeleteStock = () =>
  useMutation({
    mutationKey: ["deleteStock"],
    mutationFn: (variables: { id: number }) =>
      http.delete(`/main/api/product/${variables.id}/stock`),
    onSuccess: () => {
    },
    onError: () => {},
  });
