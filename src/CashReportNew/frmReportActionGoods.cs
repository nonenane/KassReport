using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CashReportNew
{
    public partial class frmReportActionGoods : Form
    {
        public frmReportActionGoods()
        {
            InitializeComponent();
            dtpDateStart.Value = DateTime.Now.AddDays(-7);
        }

        private void setWidthColumn(int indexRow, int indexCol, int width, Nwuram.Framework.ToExcelNew.ExcelUnLoad report)
        {
            report.SetColumnWidth(indexRow, indexCol, indexRow, indexCol, width);
        }

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            var result = await Task<bool>.Factory.StartNew(() =>
            {
                Config.DoOnUIThread(() =>
                {
                    this.Enabled = false;
                }, this);
                DataTable dtRepot = Config.hCntMainKasReal.GetDataActionGoodsRealiz(dtpDateStart.Value.Date, dtpDateEnd.Value.Date);
                if (dtRepot == null || dtRepot.Rows.Count == 0)
                {
                    MessageBox.Show("Нет данных для формирования отчёта.", "Печать", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Config.DoOnUIThread(() =>
                    {
                        this.Enabled = true;
                    }, this);
                    return false;
                }

                Nwuram.Framework.ToExcelNew.ExcelUnLoad report = new Nwuram.Framework.ToExcelNew.ExcelUnLoad();

                int indexRow = 1;
                int maxColumns = 5;

                setWidthColumn(indexRow, 1, 11, report);
                setWidthColumn(indexRow, 2, 26, report);
                setWidthColumn(indexRow, 3, 18, report);
                setWidthColumn(indexRow, 4, 18, report);
                setWidthColumn(indexRow, 5, 18, report);
                

                #region "Head"
                report.Merge(indexRow, 1, indexRow, maxColumns);
                report.AddSingleValue($"Суммарный отчёт по акционным товарам", indexRow, 1);
                report.SetFontBold(indexRow, 1, indexRow, 1);
                report.SetFontSize(indexRow, 1, indexRow, 1, 16);
                report.SetCellAlignmentToCenter(indexRow, 1, indexRow, 1);
                indexRow++;
                indexRow++;

                report.Merge(indexRow, 1, indexRow, maxColumns);
                report.AddSingleValue($"Период с {dtpDateStart.Value.ToShortDateString()} по {dtpDateEnd.Value.ToShortDateString()}", indexRow, 1);
                indexRow++;

                report.Merge(indexRow, 1, indexRow, maxColumns);
                report.AddSingleValue("Выгрузил: " + Nwuram.Framework.Settings.User.UserSettings.User.FullUsername, indexRow, 1);
                indexRow++;

                report.Merge(indexRow, 1, indexRow, maxColumns);
                report.AddSingleValue("Дата выгрузки: " + DateTime.Now.ToString(), indexRow, 1);
                indexRow++;
                indexRow++;
                #endregion

                report.AddSingleValue("EAN", indexRow, 1);
                report.AddSingleValue("Наименование товара", indexRow, 2);
                report.AddSingleValue("Цена продажи, руб.", indexRow, 3);
                report.AddSingleValue("Сумма количества шт./кг.", indexRow, 4);
                report.AddSingleValue("Сумма реализации, руб.", indexRow, 5);

                report.SetFontBold(indexRow, 1, indexRow, maxColumns);
                report.SetBorders(indexRow, 1, indexRow, maxColumns);
                report.SetWrapText(indexRow, 1, indexRow, maxColumns);
                report.SetCellAlignmentToCenter(indexRow, 1, indexRow, maxColumns);
                report.SetCellAlignmentToJustify(indexRow, 1, indexRow, maxColumns);
                indexRow++;

                var groupEan = dtRepot.AsEnumerable().GroupBy(r =>new { ean = r.Field<string>("ean"), cName = r.Field<string>("cName") });

                foreach (var gEan in groupEan)
                {
                    EnumerableRowCollection<DataRow> rowCollect = dtRepot.AsEnumerable().Where(r => r.Field<string>("ean") == gEan.Key.ean);


                    int startRow = indexRow;
                    report.SetWrapText(indexRow, 1, indexRow + rowCollect.Count() - 1, maxColumns);
                    report.Merge(indexRow, 1, indexRow + rowCollect.Count() - 1, 1);
                    report.Merge(indexRow, 2, indexRow + rowCollect.Count() - 1, 2);

                    report.AddSingleValue(gEan.Key.ean, indexRow, 1);
                    report.AddSingleValue(gEan.Key.cName, indexRow, 2);
                    report.SetBorders(indexRow, 1, indexRow + rowCollect.Count() - 1, maxColumns);
                    report.SetCellAlignmentToCenter(indexRow, 1, indexRow + rowCollect.Count() - 1, 2);
                    report.SetCellAlignmentToJustify(indexRow, 1, indexRow + rowCollect.Count() - 1, 2);


                    foreach (DataRow row in rowCollect)
                    {
                        report.SetFormat(indexRow, 3, indexRow, 5, "0.00");
                        report.AddSingleValueObject(row["price"], indexRow, 3);
                        report.AddSingleValueObject(row["count"], indexRow, 4);
                        report.AddSingleValueObject(row["SumResult"], indexRow, 5);
                        report.SetCellAlignmentToRight(indexRow, 3, indexRow, 5);
                        report.SetCellAlignmentToJustify(indexRow, 3, indexRow, 5);
                        indexRow++;
                    }
                }


                report.SetPageSetup(1, 9999, true);
                report.Show();

                Config.DoOnUIThread(() =>
                {
                    this.Enabled = true;
                }, this);
                return true;
            });
        }

        private void dtpDateStart_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDateStart.Value.Date > dtpDateEnd.Value.Date || dtpDateStart.Value.Month != dtpDateEnd.Value.Month || dtpDateStart.Value.Year != dtpDateEnd.Value.Year)
                dtpDateEnd.Value = dtpDateStart.Value.Date;
        }

        private void dtpDateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDateStart.Value.Date > dtpDateEnd.Value.Date || dtpDateStart.Value.Month != dtpDateEnd.Value.Month || dtpDateStart.Value.Year != dtpDateEnd.Value.Year)
                dtpDateStart.Value = dtpDateEnd.Value.Date;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
