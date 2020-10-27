using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using Nwuram.Framework.ToExcelNew;
using Nwuram.Framework.Settings.User;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CashReportNew.Reports
{
    class CashReport
    {
        static bool isPrintGood = false;
        static int numberReport = 0;

        public static void printReport(DateTime date, string dep_name, string doc_start, string doc_end, string time_start, string time_end, string sum_start, string sum_end, DataTable dt, List<string> sums, List<Color> colors)
        {
            FileInfo newFile = new FileInfo(Application.StartupPath + "\\Templates\\Terminal_Report.xlsx");

            if (!File.Exists(Application.StartupPath + "\\Templates\\Terminal_Report.xlsx"))
            {
                MessageBox.Show("Отсутствует шаблон отчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ExcelPackage pck = new ExcelPackage(newFile);
            ExcelWorksheet ws = pck.Workbook.Worksheets[1];
            Cursor.Current = Cursors.WaitCursor;
            ws.Cells[3, 2].Value = date.ToShortDateString();
            ws.Cells[3, 5].Value = dep_name;
            ws.Cells[3, 9].Value = DateTime.Now.ToString();
            ws.Cells[4, 9].Value = UserSettings.User.FullUsername;
            ws.Cells[5, 2].Value = doc_start;
            ws.Cells[5, 4].Value = doc_end;
            ws.Cells[6, 2].Value = time_start;
            ws.Cells[6, 4].Value = time_end;
            ws.Cells[7, 2].Value = sum_start;
            ws.Cells[7, 4].Value = sum_end;

            int rowNum = 12;
            foreach (DataRow row in dt.Rows)
            {
                int colNum = 1;
                ws.Cells[rowNum, colNum++].Value = row["terminal"];
                ws.Cells[rowNum, colNum++].Value = row["kassir_name"];
                ws.Cells[rowNum, colNum++].Value = row["doc_id"];
                ws.Cells[rowNum, colNum++].Value = row["time"];
                ws.Cells[rowNum, 4].Style.Numberformat.Format = "HH:mm";
                ws.Cells[rowNum, colNum++].Value = row["operation_name"];
                ws.Cells[rowNum, colNum++].Value = row["group_name"];
                ws.Cells[rowNum, colNum++].Value = row["ean"];
                ws.Cells[rowNum, colNum++].Value = row["cname"];
                ws.Cells[rowNum, colNum++].Value = row["count"];
                ws.Cells[rowNum, 9, rowNum, 9].Style.Numberformat.Format = "#,##0.000";
                ws.Cells[rowNum, 10, rowNum, 10].Style.Numberformat.Format = "#,##0.00";
                ws.Cells[rowNum, colNum++].Value = row["cash_val"];
                ws.Cells[rowNum, colNum++].Value = row["legalEntity"];
                ws.Cells[rowNum, colNum++].Value = row["legalEntityTK"];

                if (Convert.ToBoolean(row["is_annul"]))
                {
                    setColor(ws, colors[1], rowNum, 1, colNum-1);
                }
                else if (Convert.ToInt32(row["op_code"]) == 507)
                {
                    setColor(ws, colors[0], rowNum, 1, colNum-1);
                }
                else if (Convert.ToInt32(row["op_code"]) == 524)
                {
                    setColor(ws, colors[2], rowNum, 1, colNum-1);
                }
                if (!row["legalEntity"].ToString().Equals(row["legalEntityTK"].ToString()))
                    setColor(ws, Color.LightGreen, rowNum, 11, 12);
                ws.Cells[rowNum, 2].Style.WrapText = ws.Cells[rowNum, 5].Style.WrapText 
                    = ws.Cells[rowNum, 6].Style.WrapText
                    = ws.Cells[rowNum, 8].Style.WrapText = true;

                rowNum++;
            }

            setBorder(ws, 11, 1, 11 + dt.Rows.Count, 12);
            ws.Cells[12, 1, 12 + dt.Rows.Count, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[12, 1, 12 + dt.Rows.Count, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells[rowNum, 8].Value = "Реализовано товаров:";
            ws.Cells[rowNum, 9].Value = sums[0];

            ws.Cells[rowNum + 1, 8].Value = "Общая реализация составляет:";
            ws.Cells[rowNum + 1, 9].Value = sums[1];

            ws.Cells[rowNum + 2, 8].Value = "Обслужено клиентов:";
            ws.Cells[rowNum + 2, 9].Value = sums[2];

            ws.Cells[rowNum, 9, rowNum + 2, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[rowNum, 9, rowNum + 2, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            setBorder(ws, rowNum, 8, rowNum + 2, 8);

            while (!isPrintGood)
                showReport(pck, numberReport++);
            isPrintGood = false;
            Cursor.Current = Cursors.Default;
        }

        private static void showReport(ExcelPackage pck, int number)
        {
            try
            {
                pck.SaveAs(new FileInfo(Application.StartupPath + $"\\Terminal_Report({numberReport}).xlsx"));
                System.Diagnostics.Process.Start(Application.StartupPath + $"\\Terminal_Report({numberReport}).xlsx");
                isPrintGood = true;
            }
            catch { isPrintGood = false; }
        }

        private static void setColor(ExcelWorksheet ws, Color color,int rowExcel, int colExcelStart, int colExcelEnd)
        {
            ws.Cells[rowExcel, colExcelStart, rowExcel, colExcelEnd].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells[rowExcel, colExcelStart, rowExcel, colExcelEnd].Style.Fill.BackgroundColor.SetColor(color);
        }

        private static void setBorder(ExcelWorksheet ws, int rowExcelStart, int rowExcelEnd, int colExcelStart, int colExcelEnd)
        {
            ws.Cells[rowExcelStart, rowExcelEnd, colExcelStart, colExcelEnd].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowExcelStart, rowExcelEnd, colExcelStart, colExcelEnd].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowExcelStart, rowExcelEnd, colExcelStart, colExcelEnd].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowExcelStart, rowExcelEnd, colExcelStart, colExcelEnd].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        public static void Show(DateTime date, string dep_name, string doc_start, string doc_end, string time_start, string time_end, string sum_start, string sum_end, DataTable dt, List<string> sums, List<Color> colors)
        {
            ExcelUnLoad report = new ExcelUnLoad();

            AddHeader(report, date, dep_name, doc_start, doc_end, time_start, time_end, sum_start, sum_end);
            AddLegend(report, colors);
            AddTableHeaders(report);

            int rowNum = 12;
            foreach (DataRow row in dt.Rows)
            {
                int colNum = 1;
                report.AddSingleValue(row["terminal"].ToString(), rowNum, colNum++);
                report.AddSingleValue(row["kassir_name"].ToString(), rowNum, colNum++);
                report.AddSingleValue(row["doc_id"].ToString(), rowNum, colNum++);
                report.AddSingleValue(row["time"].ToString(), rowNum, colNum++);
                report.AddSingleValue(row["operation_name"].ToString(), rowNum, colNum++);
                report.AddSingleValue(row["group_name"].ToString(), rowNum, colNum++);
                report.AddSingleValue(row["ean"].ToString(), rowNum, colNum++);
                report.AddSingleValue(row["cname"].ToString(), rowNum, colNum++);
                report.AddSingleValue(row["count"].ToString(), rowNum, colNum++);
                report.AddSingleValue(row["cash_val"].ToString(), rowNum, colNum++);
                report.AddSingleValue(row["legalEntity"].ToString(), rowNum, colNum++);

                if (Convert.ToBoolean(row["is_annul"]))
                {
                    report.SetCellColor(rowNum, 1, rowNum, colNum - 1, colors[1]);
                }
                else if (Convert.ToInt32(row["op_code"]) == 507)
                {
                    report.SetCellColor(rowNum, 1, rowNum, colNum - 1, colors[0]);
                }
                else if (Convert.ToInt32(row["op_code"]) == 524)
                {
                    report.SetCellColor(rowNum, 1, rowNum, colNum - 1, colors[2]);
                }
                rowNum++;
            }
            report.SetBorders(11, 1, 11 + dt.Rows.Count, 10);
            report.SetCellAlignmentToCenter(12, 1, 11 + dt.Rows.Count, 1);
            report.SetCellAlignmentToCenter(12, 3, 11 + dt.Rows.Count, 4);
            report.SetCellAlignmentToRight(12, 9, 11 + dt.Rows.Count, 10);
            AddFooter(report, sums, 12 + dt.Rows.Count);

            report.Show();
        }

        private static void AddHeader(ExcelUnLoad report, DateTime date, string dep_name, string doc_start, string doc_end, string time_start, string time_end, string sum_start, string sum_end)
        {
            report.Merge(1, 1, 1, 10);
            report.AddSingleValue("Отчёт по кассам", 1, 1);
            report.SetFontSize(1, 1, 1, 1, 14);
            report.SetFontBold(1, 1, 1, 1);
            report.SetCellAlignmentToCenter(1, 1, 1, 1);

            report.AddSingleValue("Дата:", 3, 1);
            report.AddSingleValue(date.ToShortDateString(), 3, 2);

            report.AddSingleValue("Отдел:", 3, 4);
            report.AddSingleValue(dep_name, 3, 5);

            report.AddSingleValue("Дата и время выгрузки:", 3, 8);
            report.AddSingleValue(DateTime.Now.ToString(), 3, 9);

            report.AddSingleValue("Выгрузил:", 4, 8);
            report.AddSingleValue(UserSettings.User.FullUsername, 4, 9);

            report.AddSingleValue("Чеки с:", 5, 1);
            report.AddSingleValue(doc_start, 5, 2);
            report.AddSingleValue("по:", 5, 3);
            report.AddSingleValue(doc_end, 5, 4);

            report.AddSingleValue("Время с:", 6, 1);
            report.AddSingleValue(time_start, 6, 2);
            report.AddSingleValue("по:", 6, 3);
            report.AddSingleValue(time_end, 6, 4);

            report.AddSingleValue("Сумма с:", 7, 1);
            report.AddSingleValue(sum_start, 7, 2);
            report.AddSingleValue("по:", 7, 3);
            report.AddSingleValue(sum_end, 7, 4);

            report.SetFontBold(3, 1, 7, 1);
            report.SetFontBold(5, 3, 7, 3);
            report.SetFontBold(3, 4, 3, 4);
            report.SetFontBold(3, 8, 4, 8);
        }

        private static void AddLegend(ExcelUnLoad report, List<Color> colors)
        {
            report.SetCellColor(6, 9, 6, 9, colors[0]);
            report.AddSingleValue(" - Возвраты", 6, 10);

            report.SetCellColor(7, 9, 7, 9, colors[1]);
            report.AddSingleValue(" - Аннулированные", 7, 10);

            report.SetCellColor(8, 9, 8, 9, colors[2]);
            report.AddSingleValue(" - Акции", 8, 10);

            report.SetCellColor(9, 9, 9, 9, Color.LawnGreen);
            report.AddSingleValue(" - Не совпадают ЮЛ", 9, 10);

            report.SetBorders(6, 9, 9, 10);
        }

        private static void AddTableHeaders(ExcelUnLoad report)
        {
            int rowNum = 11;
            int colNum = 1;

            report.AddSingleValue("Касса", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 15);
            report.AddSingleValue("Кассир", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 15);
            report.AddSingleValue("Чек", rowNum, colNum++);
            report.AddSingleValue("Время", rowNum, colNum++);
            report.AddSingleValue("Операция", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 20);
            report.AddSingleValue("Группа", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 15);
            report.AddSingleValue("EAN", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 15);
            report.AddSingleValue("Наименование", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 45);
            report.AddSingleValue("Кол-во", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 15);
            report.AddSingleValue("Сумма", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 20);
            report.AddSingleValue("ЮЛ касса", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 15);

            report.SetFontBold(rowNum, 1, rowNum, colNum);
            report.SetCellAlignmentToCenter(rowNum, 1, rowNum, colNum);
        }

        private static void AddFooter(ExcelUnLoad report, List<string> sums, int rowNum)
        {
            report.AddSingleValue("Реализовано товаров:", rowNum, 8);
            report.AddSingleValue(sums[0], rowNum, 9);

            report.AddSingleValue("Общая реализация составляет:", rowNum + 1, 8);
            report.AddSingleValue(sums[1], rowNum + 1, 9);

            report.AddSingleValue("Обслужено клиентов:", rowNum + 2, 8);
            report.AddSingleValue(sums[2], rowNum + 2, 9);

            report.SetFontBold(rowNum, 8, rowNum + 2, 8);
            report.SetBorders(rowNum, 9, rowNum + 2, 9);
            report.SetCellAlignmentToRight(rowNum, 9, rowNum + 2, 9);
        }
    }
}
