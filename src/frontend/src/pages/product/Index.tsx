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
  Stack,
  TextField,
  Tooltip,
} from '@mui/material';
import { Delete, Edit } from '@mui/icons-material';
import { ProductEntity } from '../../types/Stock.type';
import { getProducts, useCreateProduct, useDeleteProduct, useProduct, useUpdateProduct } from '../../hooks/useProduct';
import { useQueryClient } from '@tanstack/react-query';

const Product = () => {
  const {data: products=[]} = useProduct();
  const [changed, setChanged] = useState(false);
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [tableData, setTableData] = useState<ProductEntity[]>(() => products);
  const [validationErrors, setValidationErrors] = useState<{
    [cellId: string]: string;
  }>({});
  const createProduct = useCreateProduct({
    onSuccess: () => {
      setChanged(!changed);
    },
  });
  
  const updateProduct = useUpdateProduct({
    onSuccess: () => {
      setChanged(!changed);
    },
  });
  const deleteProduct = useDeleteProduct({
    onSuccess: () => {
      setChanged(!changed);
    },
  });
  const queryClient = useQueryClient();

  useEffect(() => {
    queryClient.invalidateQueries({ queryKey: ['product'] })
    getProducts()
    .then((response) => {
      setTableData(response ?? []);
    });
  }, [changed]);

  const handleCreateNewRow = (values: ProductEntity) => {
    createProduct.mutate(values);
  };

  const handleSaveRowEdits: MaterialReactTableProps<ProductEntity>['onEditingRowSave'] =
    async ({ exitEditingMode, row, values }) => {
      if (!Object.keys(validationErrors).length) {
        const currentProduct = products.filter((product) => product.id === +values.id)[0];
        updateProduct
          .mutate({id: currentProduct.id, body: {name: values.name, price: values.price}});
        exitEditingMode(); //required to exit editing mode and close modal
      }
    };

  const handleCancelRowEdits = () => {
    setValidationErrors({});
  };

  const handleDeleteRow = useCallback(
    (row: MRT_Row<ProductEntity>) => {
      if (
        !confirm(`Você deseja deletar o produto ${row.getValue('name')}?`)
      ) {
        return;
      }
      //send api delete request here, then refetch or update local table data for re-render
      deleteProduct
        .mutate({id: row.getValue('id')});
    },
    [tableData],
  );

  const columns = useMemo<MRT_ColumnDef<ProductEntity>[]>(
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
        accessorKey: 'name',
        header: 'Nome',
        meta: {type: 'text'}as any,
        enableColumnOrdering: false,
        enableSorting: false,
        size: 80,
      },
      {
        accessorKey: 'price',
        header: 'Preço',
        meta: {type: 'number'}as any,
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
            Produto
          </Button>
        )}
      />
      <CreateProductModal
        columns={columns}
        open={createModalOpen}
        onClose={() => setCreateModalOpen(false)}
        onSubmit={handleCreateNewRow}
      />
    </>
  );
};

interface CreateModalProps {
  columns: MRT_ColumnDef<ProductEntity>[];
  onClose: () => void;
  onSubmit: (values: ProductEntity) => void;
  open: boolean;
}

//example of creating a mui dialog modal for creating new rows
export const CreateProductModal = ({
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

  return (
    <Dialog open={open}>
      <DialogTitle textAlign="center">Criar Produto</DialogTitle>
      <DialogContent>
        <form onSubmit={(e) => e.preventDefault()}>
          <Stack
            sx={{
              width: '100%',
              minWidth: { xs: '300px', sm: '360px', md: '400px' },
              gap: '1.5rem',
            }}
          >
            { columns.filter(column => column.enableEditing != false).map((column) => (
              <TextField
              margin="dense"
              variant='standard'
                key={column.accessorKey}
                label={column.header}
                name={column.accessorKey}
                type={(column.meta as any).type}
                onChange={(e) =>
                  setValues({ ...values, [e.target.name]: e.target.value })
                }
              />
            ))}
          </Stack>
        </form>
      </DialogContent>
      <DialogActions sx={{ p: '1.25rem' }}>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleSubmit} variant="contained">
          Criar Produto
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default Product;

function useQuery(arg0: string, getProducts: () => Promise<any>, arg2: {
  // Use the onSuccess callback to update table data
  onSuccess: (data: any) => void;
}): { data: any; isLoading: any; isError: any; } {
  throw new Error('Function not implemented.');
}
