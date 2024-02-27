import React, { useCallback, useEffect, useMemo, useState } from 'react';
import AddOutlinedIcon from '@mui/icons-material/AddOutlined';
import {
  MaterialReactTable,
  type MaterialReactTableProps,
  type MRT_Cell,
  type MRT_ColumnDef,
  type MRT_Row,
} from 'material-react-table';
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  InputLabel,
  MenuItem,
  Select,
  Stack,
  TextField,
  Tooltip,
} from '@mui/material';
import { Delete, Edit } from '@mui/icons-material';
import { StockEntity } from '../../types/Stock.type';
import { getStocks, useCreateStock, useDeleteStock, useStock, useUpdateStock } from '../../hooks/useStock';
import { useProduct } from '../../hooks/useProduct';
import { useQueryClient } from '@tanstack/react-query';


const Stock = () => {
  const {data: stocks=[]} = useStock();
  const [changed, setChanged] = useState(false);
  const [tableData, setTableData] = useState<StockEntity[]>(()=>stocks);
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [validationErrors, setValidationErrors] = useState<{
    [cellId: string]: string;
  }>({});
  const createStock = useCreateStock({
    onSuccess: () => {
      setChanged(!changed);
    },
  });
  const updateStock = useUpdateStock({
    onSuccess: () => {
      setChanged(!changed);
    },
  });
  const deleteStock = useDeleteStock({
    onSuccess: () => {
      setChanged(!changed);
    },
  });
  const queryClient = useQueryClient();

  useEffect(() => {
    setTableData(stocks);
  }, [stocks]);
  
  useEffect(() => {
    queryClient.invalidateQueries({ queryKey: ['stock'] })
    getStocks()
    .then((response) => {
      setTableData(response ?? []);
    });
  }, [changed]);
  

  const handleCreateNewRow = (values: StockEntity) => {
    createStock.mutate({id: values.productId, body: {quantity: values.quantity}});
  };

  const handleSaveRowEdits: MaterialReactTableProps<StockEntity>['onEditingRowSave'] =
    async ({ exitEditingMode, row, values }) => {
      if (!Object.keys(validationErrors).length) {
        const currentStock = stocks.filter((stock) => stock.id === +values.id)[0];
        updateStock.mutate({id: currentStock.product.id, body: {quantity: values.quantity}});
        exitEditingMode(); //required to exit editing mode and close modal
      }
    };

  const handleCancelRowEdits = () => {
    setValidationErrors({});
  };

  const handleDeleteRow = useCallback(
    (row: MRT_Row<StockEntity>) => {
      if (
        !confirm(`Você deseja deletar o estoque do produto ${row.getValue('product.name')}?`)
      ) {
        return;
      }
      deleteStock.mutate({id: row.getValue('id')});
    },
    [tableData],
  );

  const columns = useMemo<MRT_ColumnDef<StockEntity>[]>(
    () => [
      {
        accessorKey: 'id',
        header: 'Id',
        meta: {type: 'number'} as any,
        enableColumnOrdering: false,
        enableEditing: false, //disable editing on this column
        enableSorting: false,
        size: 80,
      },
      {
        accessorKey: 'product.name',
        header: 'Produto',
        meta: {type: 'text'}as any,
        enableColumnOrdering: false,
        enableEditing: false, //disable editing on this column
        enableSorting: false,
        size: 80
      },
      {
        accessorKey: 'quantity',
        header: 'Quantidade',
        meta: {type: 'number'}as any,
        enableColumnOrdering: false,
        enableSorting: false,
        size: 80,
      },
    ],
    []
  );


  return (
    <>
      <MaterialReactTable
        displayColumnDefOptions={{
          'mrt-row-actions': {
            muiTableHeadCellProps: {
              align: 'left',
            },
            size: 120,
          },
        }}
        columns={columns}
        data={tableData}
        editingMode="modal" //default
        enableColumnOrdering
        enableEditing
        onEditingRowSave={handleSaveRowEdits}
        onEditingRowCancel={handleCancelRowEdits}
        renderRowActions={({ row, table }) => (
          <Box sx={{ display: 'flex', gap: '1rem' }}>
            <Tooltip arrow placement="left" title="Edit">
              <IconButton onClick={() => table.setEditingRow(row)}>
                <Edit />
              </IconButton>
            </Tooltip>
            <Tooltip arrow placement="right" title="Delete">
              <IconButton onClick={() => handleDeleteRow(row)}>
                <Delete />
              </IconButton>
            </Tooltip>
          </Box>
        )}
        renderTopToolbarCustomActions={() => (
          <Button
          onClick={() => setCreateModalOpen(true)}
          startIcon={<AddOutlinedIcon />}
          >
          Estoque
        </Button>
        )}
      />
      <CreateStockModal
        columns={columns}
        open={createModalOpen}
        onClose={() => setCreateModalOpen(false)}
        onSubmit={handleCreateNewRow}
      />
    </>
  );
};

interface CreateModalProps {
  columns: MRT_ColumnDef<StockEntity>[];
  onClose: () => void;
  onSubmit: (values: StockEntity) => void;
  open: boolean;
}

//example of creating a mui dialog modal for creating new rows
export const CreateStockModal = ({
  open,
  columns,
  onClose,
  onSubmit,
}: CreateModalProps) => {
  const [values, setValues] = useState<any>(() =>
    columns.reduce((acc, column) => {
      acc[column.accessorKey ?? ''] = '';
      return acc;
    }, {} as any),
  );

  const handleSubmit = () => {
    onSubmit(values);
    onClose();
  };
  const {data: products=[]} = useProduct();

  return (
    <Dialog open={open}>
      <DialogTitle textAlign="center">Criar Estoque</DialogTitle>
      <DialogContent>
        <form onSubmit={(e) => e.preventDefault()}>
          <Stack
            sx={{
              width: '100%',
              minWidth: { xs: '300px', sm: '360px', md: '400px' },
              gap: '1.5rem',
            }}
          >
              <InputLabel id="demo-simple-select-helper-label">Produto</InputLabel>
              <Select
                margin="dense"
                variant='standard'
                labelId="demo-simple-select-helper-label"
                id="demo-simple-select-helper"
                value={values.productId || ''}
                key="productId"
                name="productId"
                onChange={(e) =>
                  setValues({ ...values, [e.target.name]: e.target.value })
                }
              >
                  {products?products.map((product) => (
                    <MenuItem
                      key={product.id}
                      value={product.id}
                    >
                      {product.name}
                    </MenuItem>
                  )):[]}
              </Select>
              <TextField
                margin="dense"
                variant='standard'
                key="quantity"
                label="Quantity"
                name="quantity"
                type="number"
                onChange={(e) =>
                  setValues({ ...values, [e.target.name]: e.target.value })
                }
              />
          </Stack>
        </form>
      </DialogContent>
      <DialogActions sx={{ p: '1.25rem' }}>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleSubmit} variant="contained">
          Criar Estoque
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default Stock;