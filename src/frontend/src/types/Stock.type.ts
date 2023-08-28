export interface StockEntity {
    id: number;
    productId: number;
    product: ProductEntity;
    quantity: number;
}

export interface ReportEntity {
    id: number;
    orderQuantity: number;
    productQuantity: number;
    averagePrice: number;
    total: number;
    productId: number;
    product: ProductEntity;
}

export interface ProductEntity {
    id: number;
    name: string;
    price: number;
}


export interface OrderEntity {
    id: number;
    productId: number;
    product: ProductEntity;
    quantity: number;
    statusId: number;
    status: string;
    boughtBy: string;
}




