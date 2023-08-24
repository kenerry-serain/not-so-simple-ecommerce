import { GridColDef, GridValueGetterParams } from "@mui/x-data-grid";

const stockColumns: GridColDef[] = [
    {
        field: 'product.name',
        headerName: 'Product',
        type: 'string',
        width: 250,
        valueGetter: (gridValue: GridValueGetterParams) => `${gridValue.row.product.name || ''}`,
    },
    {
        field: 'quantity',
        headerName: 'Quantity',
        type: 'number',
        width: 250
    }
];

export default stockColumns;