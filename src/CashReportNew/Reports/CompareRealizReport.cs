using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Nwuram.Framework.ToExcelNew;
using Nwuram.Framework.Settings.User;

namespace CashReportNew.Reports
{
    class CompareRealizReport
    {
        public static void Show(string dep_name, string tu_grp_name, string inv_grp_name, string ean, string name, DateTime date_start1, DateTime date_end1, DateTime date_start2, DateTime date_end2, DataTable data, string count1, string count2, string diff_count, string sum1, string sum2, string diff_sum)
        {
            ExcelUnLoad report = new ExcelUnLoad();

            report.AddSingleValue("Сравнение реализации", 1, 2);
            report.SetFontSize(1, 2, 1, 2, 14);
            report.SetFontBold(1, 2, 1, 2);

            report.Merge(2, 3, 2, 4);
            report.AddSingleValue("Выгрузил:", 2, 3);
            report.AddSingleValue(UserSettings.User.FullUsername, 2, 5);

            report.Merge(3, 3, 3, 4);
            report.AddSingleValue("Дата и время выгрузки:", 3, 3);
            report.AddSingleValue(DateTime.Now.ToString(), 3, 5);

            report.SetFontBold(2, 3, 3, 3);

            report.AddSingleValue("Отдел:", 4, 1);
            report.AddSingleValue(dep_name, 4, 2);

            report.AddSingleValue("ТУ группа:", 5, 1);
            report.AddSingleValue(tu_grp_name, 5, 2);

            report.AddSingleValue("Инв. группа:", 6, 1);
            report.AddSingleValue(inv_grp_name, 6, 2);

            report.SetFontBold(4, 1, 6, 1);

            report.AddSingleValue("Дата 1:", 4, 3);
            report.AddSingleValue(date_start1.ToShortDateString(), 4, 4);

            report.AddSingleValue("Дата 2:", 5, 3);
            report.AddSingleValue(date_start2.ToShortDateString(), 5, 4);

            report.AddSingleValue("Время с:", 4, 5);
            report.AddSingleValue(GetTimeString(date_start1.Hour) + ":" + GetTimeString(date_start1.Minute), 4, 6);

            report.AddSingleValue("Время с:", 5, 5);
            report.AddSingleValue(GetTimeString(date_start2.Hour) + ":" + GetTimeString(date_start2.Minute), 5, 6);

            report.AddSingleValue("по", 4, 7);
            report.AddSingleValue(GetTimeString(date_end1.Hour) + ":" + GetTimeString(date_end1.Minute), 4, 8);

            report.AddSingleValue("по", 5, 7);
            report.AddSingleValue(GetTimeString(date_end2.Hour) + ":" + GetTimeString(date_end2.Minute), 5, 8);

            report.SetFontBold(4, 3, 5, 3);
            report.SetFontBold(4, 5, 5, 5);
            report.SetFontBold(4, 7, 5, 7);

            report.AddSingleValue("EAN:", 6, 3);
            report.AddSingleValue(ean, 6, 4);
            report.SetFontBold(6, 4, 6, 4);

            report.Merge(6, 5, 6, 6);
            report.AddSingleValue("Наименование:", 6, 5);
            report.AddSingleValue(name, 6, 7);
            report.SetFontBold(6, 3, 6, 7);

            AddColumnHeaders(report, 8, 1, new string[] { "EAN", "Наименование", "Кол-во 1", "Кол-во 2", "Разница", "Сумма 1", "Сумма 2", "Разница" });
            report.SetColumnWidth(8, 1, 8, 1, 15);
            report.SetColumnWidth(8, 2, 8, 2, 45);

            report.AddMultiValue(data, 9, 1);
            report.SetBorders(8, 1, 8 + data.Rows.Count, 8);

            int lastRowIndex = 8 + data.Rows.Count + 2;
            report.AddSingleValue("Итого:", lastRowIndex, 1);
            report.SetFontBold(lastRowIndex, 1, lastRowIndex, 1);

            int colIndex = 3;
            report.AddSingleValue(count1, lastRowIndex, colIndex++);
            report.AddSingleValue(count2, lastRowIndex, colIndex++);
            report.AddSingleValue(diff_count, lastRowIndex, colIndex++);

            report.AddSingleValue(sum1, lastRowIndex, colIndex++);
            report.AddSingleValue(sum2, lastRowIndex, colIndex++);
            report.AddSingleValue(diff_sum, lastRowIndex, colIndex++);

            report.SetBorders(lastRowIndex, 3, lastRowIndex, colIndex - 1);
            report.SetFontBold(lastRowIndex, 3, lastRowIndex, colIndex - 1);

            report.SetCellAlignmentToRight(9, 3, lastRowIndex, 9);

            report.SetColumnWidth(1, 4, 1, 4, 12);
            report.SetColumnWidth(1, 8, 1, 8, 15);

            report.Show();
        }

        private static string GetTimeString(int time)
        {
            return time < 10 ? "0" + time.ToString() : time.ToString();
        }

        private static void AddColumnHeaders(ExcelUnLoad report, int rowIndex, int colIndex, string[] columnNames)
        {
            int startcolIndex = colIndex;
            foreach (string columnName in columnNames)
            {
                report.AddSingleValue(columnName, rowIndex, colIndex);
                colIndex++;
            }
            report.SetCellAlignmentToCenter(rowIndex, startcolIndex, rowIndex, colIndex);
            report.SetFontBold(rowIndex, startcolIndex, rowIndex, colIndex);
        }
    }
}
