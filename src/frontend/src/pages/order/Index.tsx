import React, { useCallback, useEffect, useMemo, useState } from 'react';
import AddOutlinedIcon from '@mui/icons-material/AddOutlined';

import {
  MaterialReactTable,
  type MaterialReactTableProps,
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
import { OrderEntity } from '../../types/Stock.type';
import { getOrders, useCreateOrder, useDeleteOrder, useOrder, useUpdateOrder } from '../../hooks/useOrder';
import { useQueryClient } from '@tanstack/react-query';
import { useProduct } from '../../hooks/useProduct';
const Order = () => {
  const {data: orders=[]} = useOrder();
  const [changed, setChanged] = useState(false);
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [tableData, setTableData] = useState<OrderEntity[]>(() => orders);
  const [validationErrors, setValidationErrors] = useState<{
    [cellId: string]: string;
  }>({});
  const createOrder = useCreateOrder({
    onSuccess: () => {
      setChanged(!changed);
    },
  });
  const updateOrder = useUpdateOrder({
    onSuccess: () => {
      setChanged(!changed);
    },
  });
  const deleteOrder = useDeleteOrder({
    onSuccess: () => {
      setChanged(!changed);
    },
  });
  const queryClient = useQueryClient();

  useEffect(() => {
    setTableData(orders);
  }, [orders]);
  
  useEffect(() => {
    queryClient.invalidateQueries({ queryKey: ['order'] })
    getOrders()
    .then((response) => {
      setTableData(response ?? []);
    });
  }, [changed]);
  
  const handleCreateNewRow = (values: OrderEntity) => {
    createOrder.mutate({productId: values.productId, quantity: values.quantity});
    setChanged (!changed);
  };

  const handleSaveRowEdits: MaterialReactTableProps<OrderEntity>['onEditingRowSave'] =
    async ({ exitEditingMode, row, values }) => {
      if (!Object.keys(validationErrors).length) {
        const currentOrder = orders.filter((order) => order.id === +values.id)[0];
        updateOrder.mutate({id: currentOrder.id, body: {productId: currentOrder.product.id, quantity: values.quantity}});
        exitEditingMode(); //required to exit editing mode and close modal
      }
    };

  const handleCancelRowEdits = () => {
    setValidationErrors({});
  };

  const handleDeleteRow = useCallback(
    (row: MRT_Row<OrderEntity>) => {
      if (
        !confirm(`Você deseja deletar a ordem ${row.getValue('firstName')}?`)
      ) {
        return;
      }
      deleteOrder.mutate({id: row.getValue('id')});
    },
    [tableData],
  );

  const columns = useMemo<MRT_ColumnDef<OrderEntity>[]>(
    () => [
      {
        accessorKey: 'id',
        header: 'Id',
        enableColumnOrdering: false,
        enableEditing: false, //disable editing on this column
        enableSorting: false,
        size: 80,
      },
      {
        accessorKey: 'product.name',
        header: 'Produto',
        enableColumnOrdering: false,
        enableEditing: false, //disable editing on this column
        enableSorting: false,
        size: 80
      },
      {
        accessorKey: 'quantity',
        header: 'Quantidade',
        enableColumnOrdering: false,
        enableSorting: false,
        size: 80
      },
      {
        accessorKey: 'status',
        header: 'Status',
        enableEditing: false, //disable editing on this column
        enableColumnOrdering: false,
        enableSorting: false,
        size: 80
      },
      {
        accessorKey: 'boughtBy',
        header: 'Comprador',
        enableEditing: false, //disable editing on this column
        enableColumnOrdering: false,
        enableSorting: false,
        size: 80
      }
    ],
    [],
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
          Ordem
        </Button>
        )}
      />
      <CreateOrderModal
        columns={columns}
        open={createModalOpen}
        onClose={() => setCreateModalOpen(false)}
        onSubmit={handleCreateNewRow}
      />
    </>
  );
};

interface CreateModalProps {
  columns: MRT_ColumnDef<OrderEntity>[];
  onClose: () => void;
  onSubmit: (values: OrderEntity) => void;
  open: boolean;
}

//example of creating a mui dialog modal for creating new rows
export const CreateOrderModal = ({
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
    //put your validation logic here
    onSubmit(values);
    onClose();
  };

  const {data: products=[]} = useProduct();

  return (
    <Dialog open={open}>
      <DialogTitle textAlign="center">Criar Ordem</DialogTitle>
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
          Criar Ordem
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default Order;
