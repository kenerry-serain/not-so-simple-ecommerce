import React, { useCallback, useMemo, useState } from 'react';
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
import { data } from './makeData';
import { StockEntity } from '../../types/Stock.type';

const Report = () => {
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [tableData, setTableData] = useState<StockEntity[]>(() => data);
  const [validationErrors, setValidationErrors] = useState<{
    [cellId: string]: string;
  }>({});

  const handleCreateNewRow = (values: StockEntity) => {
    tableData.push(values);
    setTableData([...tableData]);
  };

  const handleSaveRowEdits: MaterialReactTableProps<StockEntity>['onEditingRowSave'] =
    async ({ exitEditingMode, row, values }) => {
      if (!Object.keys(validationErrors).length) {
        tableData[row.index] = values;
        //send/receive api updates here, then refetch or update local table data for re-render
        setTableData([...tableData]);
        exitEditingMode(); //required to exit editing mode and close modal
      }
    };

  const handleCancelRowEdits = () => {
    setValidationErrors({});
  };

  const handleDeleteRow = useCallback(
    (row: MRT_Row<StockEntity>) => {
      if (
        !confirm(`Are you sure you want to delete ${row.getValue('firstName')}`)
      ) {
        return;
      }
      //send api delete request here, then refetch or update local table data for re-render
      tableData.splice(row.index, 1);
      setTableData([...tableData]);
    },
    [tableData],
  );

  const getCommonEditTextFieldProps = useCallback(
    (
      cell: MRT_Cell<StockEntity>,
    ): MRT_ColumnDef<StockEntity>['muiTableBodyCellEditTextFieldProps'] => {
      return {
        error: !!validationErrors[cell.id],
        helperText: validationErrors[cell.id],
        onBlur: (event) => {
          const isValid =
            cell.column.id === 'email'
              ? validateEmail(event.target.value)
              : cell.column.id === 'price'
              ? validatePrice(+event.target.value)
              : validateRequired(event.target.value);
          if (!isValid) {
            //set validation error for cell if invalid
            setValidationErrors({
              ...validationErrors,
              [cell.id]: `${cell.column.columnDef.header} is required`,
            });
          } else {
            //remove validation error for cell if valid
            delete validationErrors[cell.id];
            setValidationErrors({
              ...validationErrors,
            });
          }
        },
      };
    },
    [validationErrors],
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
        enableSorting: false,
        size: 80,
        muiTableBodyCellEditTextFieldProps: ({ cell }) => ({
            ...getCommonEditTextFieldProps(cell),
          }),
      },
      {
        accessorKey: 'quantity',
        header: 'Quantidade',
        meta: {type: 'number'}as any,
        enableColumnOrdering: false,
        enableSorting: false,
        size: 80,
        muiTableBodyCellEditTextFieldProps: ({ cell }) => ({
            ...getCommonEditTextFieldProps(cell),
            type: 'number'
          }),
      },
    //   {
    //     accessorKey: '',
    //     header: 'First Name',
    //     size: 140,
        // muiTableBodyCellEditTextFieldProps: ({ cell }) => ({
        //   ...getCommonEditTextFieldProps(cell),
        // }),
    //   },
    //   {
    //     accessorKey: 'lastName',
    //     header: 'Last Name',
    //     size: 140,
    //     muiTableBodyCellEditTextFieldProps: ({ cell }) => ({
    //       ...getCommonEditTextFieldProps(cell),
    //     }),
    //   },
    //   {
    //     accessorKey: 'email',
    //     header: 'Email',
    //     muiTableBodyCellEditTextFieldProps: ({ cell }) => ({
    //       ...getCommonEditTextFieldProps(cell),
    //       type: 'email',
    //     }),
    //   },
    //   {
    //     accessorKey: 'age',
    //     header: 'Age',
    //     size: 80,
    //     muiTableBodyCellEditTextFieldProps: ({ cell }) => ({
    //       ...getCommonEditTextFieldProps(cell),
    //       type: 'number',
    //     }),
    //   },
    //   {
    //     accessorKey: 'state',
    //     header: 'State',
    //     muiTableBodyCellEditTextFieldProps: {
    //       select: true, //change to select for a dropdown
    //       children: states.map((state) => (
    //         <MenuItem key={state} value={state}>
    //           {state}
    //         </MenuItem>
    //       )),
    //     },
    //   },
    ],
    [getCommonEditTextFieldProps],
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
            variant="contained"
          >
            Criar Estoque
          </Button>
        )}
      />
      <CreateNewAccountModal
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
export const CreateNewAccountModal = ({
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
          Criar Estoque
        </Button>
      </DialogActions>
    </Dialog>
  );
};

const validateRequired = (value: string) => !!value.length;
const validateEmail = (email: string) =>
  !!email.length &&
  email
    .toLowerCase()
    .match(
      /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
    );
const validatePrice = (price: number) => price >= 18 && price <= 50;

export default Report;