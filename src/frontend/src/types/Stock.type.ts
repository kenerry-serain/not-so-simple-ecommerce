export interface StockEntity {
    id: number;
    product: ProductEntity;
    quantity: number;
}

export interface ProductEntity {
    id: number;
    name: string;
    price: number;
}


export interface OrderEntity {
    id: number;
    product: ProductEntity;
    quantity: number;
    statusId: number;
    status: string;
}




