using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Nwuram.Framework.ToExcelNew;

namespace CashReportNew.Reports
{
    class ScanReport
    {
        public static int Show(DataTable dt)
        {
            ExcelUnLoad report = new ExcelUnLoad();

            int i = 0;
            int rowNum = 1;
            int page_count = 0;

            List<DataRow> rows_unload = dt.AsEnumerable().Skip(i).Take(44).ToList<DataRow>();
            while (rows_unload.Count > 0)
            {
                report.AddSingleValue("ean", rowNum, 1);
                report.SetColumnWidth(rowNum, 1, rowNum, 1, 15);
                report.AddSingleValue("count", rowNum, 2);
                report.SetColumnWidth(rowNum, 2, rowNum, 2, 12);
                report.AddSingleValue("summa", rowNum, 3);
                report.SetColumnWidth(rowNum, 3, rowNum, 3, 12);

                report.AddMultiValue(rows_unload.CopyToDataTable(), rowNum + 1, 1);

                report.AddSingleValue("Итого", rowNum + rows_unload.Count, 1);
                report.AddSingleValue(rows_unload.Sum(r => Convert.ToDecimal(r["sum_count"])).ToString("N3"), rowNum + rows_unload.Count, 2);
                report.AddSingleValue(rows_unload.Sum(r => Convert.ToDecimal(r["sum_cash_val"])).ToString("N2"), rowNum + rows_unload.Count, 3);

                i += 44;
                rows_unload = dt.AsEnumerable().Skip(i).Take(44).ToList<DataRow>();
                rowNum += 50;
                page_count++;
            }

            report.Show();

            return page_count;
        }
    }
}
