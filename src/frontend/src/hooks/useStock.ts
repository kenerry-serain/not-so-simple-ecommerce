import { useQuery } from "@tanstack/react-query";
import { StockEntity } from "../types/Stock.type";

const getStock = (): StockEntity[] => {
    return [
        { id: 1, product: {id: 1, name: 'Product 01'}, quantity: 35 },
        { id: 2, product: {id: 2, name: 'Product 02'}, quantity: 36 },
        { id: 3, product: {id: 3, name: 'Product 03'}, quantity: 37 },
        { id: 4, product: {id: 4, name: 'Product 04'}, quantity: 38 },
    ] as StockEntity[];
};

export const useStock = () =>
    useQuery({
        queryKey: ["stock"],
        queryFn: () => getStock(),
    });
