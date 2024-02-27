import { UseQueryResult, useMutation, useQuery } from "@tanstack/react-query";
import { ProductEntity } from "../types/Stock.type";
import { http } from "../utils/HttpClient";

export const getProducts = async () => {
  const { data } = await http.get<ProductEntity[]>("main/api/product");
  return data;
};

export const useProduct = (): UseQueryResult<ProductEntity[]>  =>
  useQuery({
    queryKey: ["product"],
    queryFn: getProducts,
  });

export type ProductRequest = {
  name: string;
  price: number;
};

export const useCreateProduct = ({onSuccess}) =>
  useMutation({
    mutationKey: ["createProduct"],
    mutationFn: (body: ProductRequest) => http.post(`/main/api/product`, body),
    onSuccess: () => onSuccess(),
    onError: () => {},
  });

export const useUpdateProduct = ({onSuccess}) =>
  useMutation({
    mutationKey: ["updateProduct"],
    mutationFn: (variables: { id: number; body: ProductRequest }) =>
      http.put(`/main/api/product/${variables.id}`, variables.body),
      onSuccess: () => onSuccess(),
    onError: () => {},
  });

export const useDeleteProduct = ({onSuccess}) =>
  useMutation({
    mutationKey: ["deleteProduct"],
    mutationFn: (variables: { id: number })  => http.delete(`/main/api/product/${variables.id}`),
    onSuccess: () => onSuccess(),
    onError: () => {},
  });
