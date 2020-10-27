using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Nwuram.Framework.ToExcelNew;
using Nwuram.Framework.Settings.User;

namespace CashReportNew.Reports
{
    class RealizReport
    {
        public static void ShowRealizReport(string dep_name, string group_name, DateTime date_start, DateTime date_end, DataTable dt, List<string> numbers)
        {
            ExcelUnLoad report = new ExcelUnLoad();

            AddHeader(report, dep_name, group_name, date_start, date_end);
            AddTableHeaders(report);

            dt.Columns.Remove("ws");
            report.AddMultiValue(dt, 9, 1);
            report.SetWrapText(9, 3, dt.Rows.Count, 3);
            report.SetCellAlignmentToCenter(9, 7, dt.Rows.Count, 7);
            report.SetCellAlignmentToRight(9, 4, 8 + dt.Rows.Count, 6);
            report.SetBorders(8, 1, 8 + dt.Rows.Count, 7);
            report.SetPageOrientationToLandscape();

            AddFooter(report, numbers, 10 + dt.Rows.Count);

            report.Show();
        }

        private static void AddHeader(ExcelUnLoad report, string dep_name, string group_name, DateTime date_start, DateTime date_end)
        {
            report.Merge(1, 1, 1, 6);
            report.AddSingleValue("Отчёт по реализации по кассам", 1, 1);
            report.SetFontBold(1, 1, 1, 1);
            report.SetFontSize(1, 1, 1, 1, 16);
            report.SetCellAlignmentToCenter(1, 1, 1, 1);

            report.AddSingleValue("Отдел:", 3, 1);
            report.AddSingleValue(dep_name, 3, 2);

            report.AddSingleValue("ТУ группа:", 4, 1);
            report.AddSingleValue(group_name, 4, 2);

            report.AddSingleValue("Период с:", 5, 1);
            report.AddSingleValue(date_start.ToString(), 5, 2);
            report.AddSingleValue("по:", 6, 1);
            report.AddSingleValue(date_end.ToString(), 6, 2);

            report.AddSingleValue("Дата и время выгрузки:", 3, 5);
            report.SetWrapText(3, 5, 3, 5);
            report.AddSingleValue(DateTime.Now.ToString(), 3, 6);

            report.AddSingleValue("Выгрузил:", 4, 5);
            report.AddSingleValue(UserSettings.User.FullUsername, 4, 6);

            report.SetFontBold(3, 1, 6, 1);
            report.SetFontBold(3, 5, 4, 5);
        }

        private static void AddTableHeaders(ExcelUnLoad report)
        {
            int rowNum = 8;
            int colNum = 1;

            report.AddSingleValue("ТУ группа", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 20);
            report.AddSingleValue("EAN", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 15);
            report.AddSingleValue("Наименование", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 35);
            report.AddSingleValue("Кол-во", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 12);
            report.AddSingleValue("Сумма", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 12);
            report.AddSingleValue("Покупателей", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 15);
            report.AddSingleValue("ЮЛ", rowNum, colNum++);
            report.SetColumnWidth(rowNum, colNum - 1, rowNum, colNum - 1, 5);

            report.SetFontBold(rowNum, 1, rowNum, colNum);
            report.SetCellAlignmentToCenter(rowNum, 1, rowNum, colNum);
        }

        private static void AddFooter(ExcelUnLoad report, List<string> numbers, int rowNum)
        {
            report.AddSingleValue("Итого:", rowNum, 4);
            report.AddSingleValue("Итого в руб.:", rowNum + 3, 4);

            report.AddSingleValue(numbers[0], rowNum, 5);
            report.AddSingleValue(numbers[1], rowNum, 6);

            report.AddSingleValue(numbers[2], rowNum + 1, 5);
            report.AddSingleValue(numbers[3], rowNum + 1, 6);

            report.AddSingleValue(numbers[4], rowNum + 3, 6);

            report.SetFontBold(rowNum, 4, rowNum + 3, 6);
            report.SetBorders(rowNum, 5, rowNum + 1, 6);
            report.SetBorders(rowNum + 3, 6, rowNum + 3, 6);

            report.SetCellAlignmentToRight(rowNum, 5, rowNum + 3, 6);
        }
    }
}
