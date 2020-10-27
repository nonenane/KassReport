using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using Nwuram.Framework.ToExcelNew;
using Nwuram.Framework.Logging;

namespace CashReportNew.Reports
{
    class DocReport
    {
        static int cols = 0;
        static int count = 0;
        public static bool Show(string terminal, string kassir_name, string doc_id, DataTable dt, string report_path, string result_path)
        {
            ExcelUnLoad report = new ExcelUnLoad();

            string doc_time = ""; 

            DataRow begin_row = dt.AsEnumerable().FirstOrDefault(dr => Convert.ToInt32(dr["op_code"]) == 504);
            if (begin_row != null)
            {
                doc_time = Convert.ToDateTime(begin_row["time"]).ToString();
            }
            
            addHeader(report, terminal, kassir_name, doc_id, doc_time);
            addTableHeader(report);

            DataTable dt_print = dt.DefaultView.ToTable();
            dt_print.Columns.Remove("op_code");
            dt_print.Columns.Remove("time");

            addDataSet(report, dt_print);

            decimal itogo = 0;
            decimal money = 0;
            decimal diff = 0;

            DataRow end_row = dt.AsEnumerable().FirstOrDefault(dr => Convert.ToInt32(dr["op_code"]) == 509);
            if (end_row != null)
            {
                itogo = Convert.ToDecimal(end_row["price"]);
                money = Convert.ToDecimal(end_row["cash_val"]);
                diff = money - itogo;
            }

            addFooter(report, itogo.ToString("N2"), money.ToString("N2"), diff.ToString("N2"));

            report.Show();
            
            return true;
        }

        private static void addHeader(ExcelUnLoad report, string terminal, string kassir_name, string doc_id, string doc_time)
        {
            report.Merge(1, 1, 1, 6);
            report.AddSingleValue("Отчёт по чеку", 1, 1);
            report.SetFontSize(1, 1, 1, 1, 14);
            report.SetFontBold(1, 1, 1, 1);
            report.SetCellAlignmentToCenter(1, 1, 1, 1);

            report.AddSingleValue("Касса:", 3, 1);
            report.SetFontBold(3, 1, 3, 1);

            report.AddSingleValue(terminal, 3, 2);

            report.AddSingleValue("Кассир:", 4, 1);
            report.SetFontBold(4, 1, 4, 1);

            report.AddSingleValue(kassir_name, 4, 2);

            report.AddSingleValue("Чек:", 5, 1);
            report.SetFontBold(5, 1, 5, 1);

            report.AddSingleValue(doc_id, 5, 2);

            report.AddSingleValue("Время:", 6, 1);
            report.SetFontBold(6, 1, 6, 1);

            report.AddSingleValue(doc_time, 6, 2);
        }

        private static void addTableHeader(ExcelUnLoad report)
        {
            int rowNum = 8;
            int colNum = 1;

            report.AddSingleValue("Операция", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 15);
            
            report.AddSingleValue("EAN", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 16);

            report.AddSingleValue("Наименование", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 25);

            report.AddSingleValue("Цена за кг", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 10);

            report.AddSingleValue("Кол-во", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 10);

            report.AddSingleValue("Сумма", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 10);

            report.AddSingleValue("ЮЛ", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 10);

            report.SetFontBold(rowNum, 1, rowNum, colNum);
            report.SetCellAlignmentToCenter(rowNum, 1, rowNum, colNum);

            report.SetBorders(rowNum, 1, rowNum, colNum - 1);

            cols = colNum - 1;
        }

        private static void addDataSet(ExcelUnLoad report, DataTable dtData)
        {
            report.AddMultiValue(dtData, 9, 1);

            count = dtData.Rows.Count;

            report.SetBorders(9, 1, count + 8, cols);
            report.SetWrapText(9, 1, count + 8, 1);
            report.SetWrapText(9, 3, count + 8, 3);
        }

        private static void addFooter(ExcelUnLoad report, string сashBuy, string сashGet, string discount)
        {
            int pos = 9 + count + 1;

            report.Merge(pos, 4, pos, 5);
            report.AddSingleValue("Сумма покупки:", pos, 4);
            report.AddSingleValue(сashBuy, pos++, 6);

            report.Merge(pos, 4, pos, 5);
            report.AddSingleValue("Сумма получена:", pos, 4);
            report.AddSingleValue(сashGet, pos++, 6);

            report.Merge(pos, 4, pos, 5);
            report.AddSingleValue("Сдача:", pos, 4);
            report.AddSingleValue(discount, pos++, 6);
        }

    }
}
