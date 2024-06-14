import React, { useMemo, useState } from 'react';
import {
  MaterialReactTable,
  type MRT_ColumnDef,
} from 'material-react-table';
import { ReportEntity } from '../../types/Stock.type';
import { useQueryClient } from '@tanstack/react-query';
import React, { useEffect, useMemo, useState } from 'react';
import { getReports, useReport } from '../../hooks/useReport';

const Report = () => {
  const {data: reports=[]} = useReport();
  const [tableData, setTableData] = useState<ReportEntity[]>(() => reports);
  const [changed] = useState(false);

  const queryClient = useQueryClient();

  useEffect(() => {
    queryClient.invalidateQueries({ queryKey: ['product'] })
    getReports()
    .then((response) => {
      setTableData(response ?? []);
    });
  }, [changed]);
  const columns = useMemo<MRT_ColumnDef<ReportEntity>[]>(
    () => [
      {
        accessorKey: 'productName',
        header: 'Produto',
        enableColumnOrdering: false,
        enableSorting: false,
        size: 80,
      },
      {
        accessorKey: 'totalOrders',
        header: 'Qtde Pedidos',
        enableColumnOrdering: false,
        enableSorting: true,
        size: 80,
      },
      {
        accessorKey: 'totalOrdered',
        header: 'Qtde Produtos Pedidos',
        enableColumnOrdering: false,
        enableSorting: true,
        size: 80,
      },
      {
        accessorKey: 'totalSold',
        header: 'Total',
        enableColumnOrdering: false,
        enableSorting: true,
        size: 80,
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
        enableColumnOrdering
      />
    </>
  );
};

export default Report;