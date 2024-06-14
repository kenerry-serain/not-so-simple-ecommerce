import { UseQueryResult, useQuery } from "@tanstack/react-query";
import { ReportEntity } from "../types/Stock.type";
import { http } from "../utils/HttpClient";

export const getReports = async () => {
  const { data } = await http.get<ReportEntity[]>("/main/api/report/order");
  return data;
};

export const useReport = (): UseQueryResult<ReportEntity[]> =>
  useQuery({
    queryKey: ["report"],
    queryFn: getReports,
  });
