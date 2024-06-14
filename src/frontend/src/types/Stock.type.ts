export interface StockEntity {
    id: number;
    productId: number;
    product: ProductEntity;
    quantity: number;
}

export interface ReportEntity {
    id: number;
    totalOrders: number;
    totalOrdered: number;
    averagePrice: number;
    totalSold: number;
    productId: number;
    productName: string;
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




