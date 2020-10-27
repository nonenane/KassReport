using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nwuram.Framework.Settings.User;
using Nwuram.Framework.UI.Forms;
using Nwuram.Framework.UI.Service;
using Nwuram.Framework.Logging;


namespace CashReportNew
{
    public partial class frmCompareRealiz : Form
    {
        bool load = true;
        DataTable dtGoodsRealiz = null;
        char[] special_symbols = new char[] { '%', '*', '"', '\\', '\'', '/', '(', ')', '[', ']' };
        frmLoad frmWaiting = null;

        public frmCompareRealiz()
        {
            InitializeComponent();
        }

        private void frmCompareRealiz_Load(object sender, EventArgs e)
        {
            ClearFilters();

            cmbDepartments_Load();
            cmbInvGroups_Load();
            cmbTUGroups_Load();

            dtpDate1.MinDate = new DateTime(2017, 04, 01);
            dtpDate1.MaxDate = DateTime.Today;

            dtpDate2.MinDate = new DateTime(2017, 04, 01);
            dtpDate2.MaxDate = DateTime.Today;

            SetButtonsEnabled();
            load = false;
        }

        private void cmbDepartments_Load()
        {
            DataTable dt = Config.hCntMainKasReal.GetDepartments();
            DataTable dt2 = Config.hCntVVOKasReal.GetDepartments();
            if (dt != null && dt2 != null)
                dt.Merge(dt2, true, MissingSchemaAction.Ignore);
            dt.DefaultView.Sort = "id ASC";
            cmbDepartments.DataSource = dt;
            cmbDepartments.ValueMember = "id";
            cmbDepartments.DisplayMember = "name";
            if (UserSettings.User.StatusCode == "МН")
            {
                cmbDepartments.SelectedValue = UserSettings.User.IdDepartment;
                cmbDepartments.Enabled = false;
            }
        }

        private void cmbTUGroups_Load()
        {
            if (cmbDepartments.SelectedValue != null)
            {
                if (Convert.ToInt32(cmbDepartments.SelectedValue) != 6)
                cmbTUGroups.DataSource = Config.hCntMainKasReal.GetGroups(Convert.ToInt32(cmbDepartments.SelectedValue));
                else cmbTUGroups.DataSource = Config.hCntVVOKasReal.GetGroups(Convert.ToInt32(cmbDepartments.SelectedValue));
                cmbTUGroups.ValueMember = "id";
                cmbTUGroups.DisplayMember = "name";
            }
        }

        private void cmbInvGroups_Load()
        {
            if (cmbDepartments.SelectedValue != null)
            {
                if (Convert.ToInt32(cmbDepartments.SelectedValue) != 6)
                cmbInvGroups.DataSource = Config.hCntMainKasReal.GetInvGroups(Convert.ToInt32(cmbDepartments.SelectedValue));
                else cmbInvGroups.DataSource = Config.hCntVVOKasReal.GetInvGroups(Convert.ToInt32(cmbDepartments.SelectedValue));
                cmbInvGroups.ValueMember = "id";
                cmbInvGroups.DisplayMember = "name";
            }
        }

        private void SetButtonsEnabled()
        {
            cmbTUGroups.Enabled = cmbInvGroups.Enabled = cmbDepartments.SelectedValue != null && Convert.ToInt32(cmbDepartments.SelectedValue) != 0;
            btnPrint.Enabled = dgvRealiz.RowCount > 0;
        }

        private void cmbDepartments_SelectedValueChanged(object sender, EventArgs e)
        {
            cmbTUGroups_Load();
            cmbInvGroups_Load();
            Filter();
            SetButtonsEnabled();
        }

        private void cmbTUGroups_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!load)
            {
                if (cmbTUGroups.SelectedValue != null && Convert.ToInt32(cmbTUGroups.SelectedValue) != 0)
                {
                    cmbInvGroups.SelectedValue = 0;
                }
                Filter();
            }
        }

        private void cmbInvGroups_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!load)
            {
                if (cmbInvGroups.SelectedValue != null && Convert.ToInt32(cmbInvGroups.SelectedValue) != 0)
                {
                    cmbTUGroups.SelectedValue = 0;
                }
                Filter();
            }
        }

        private void Filter()
        {
            if (dgvRealiz.DataSource != null)
            {
                string filter = "ean like '%" + txtEAN.Text + "%' and cname like '%" + txtName.Text + "%'";

                if (cmbDepartments.SelectedValue != null && Convert.ToInt32(cmbDepartments.SelectedValue) != 0)
                {
                    filter += " and id_otdel = " + cmbDepartments.SelectedValue.ToString();
                }

                if (cmbTUGroups.SelectedValue != null && Convert.ToInt32(cmbTUGroups.SelectedValue) != 0)
                {
                    filter += " and id_grp1 = " + cmbTUGroups.SelectedValue.ToString();
                }

                if (cmbInvGroups.SelectedValue != null && Convert.ToInt32(cmbInvGroups.SelectedValue) != 0)
                {
                    filter += " and id_grp2 = " + cmbInvGroups.SelectedValue.ToString();
                }

                (dgvRealiz.DataSource as DataTable).DefaultView.RowFilter = filter;
                SetSums();
                SetButtonsEnabled();
            }
        }

        private void bgw_Load_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];
            int id_otdel = Convert.ToInt32(args[4]);

            DataTable dtGoods = Config.hCntMainKasReal.GetGoods(id_otdel);

            if (id_otdel != 6)
            {
                DataTable dtRealiz1 = Config.hCntMainKasReal.GetRealizForCompare(Convert.ToDateTime(args[0]), Convert.ToDateTime(args[1]), id_otdel);
                DataTable dtRealiz2 = Config.hCntMainKasReal.GetRealizForCompare(Convert.ToDateTime(args[2]), Convert.ToDateTime(args[3]), id_otdel);

                dtGoodsRealiz = GetResultTable(dtGoods, dtRealiz1, dtRealiz2);
            }
            else if (id_otdel == 0)
            {
                DataTable dtRealiz1 = Config.hCntMainKasReal.GetRealizForCompare(Convert.ToDateTime(args[0]), Convert.ToDateTime(args[1]), id_otdel);
                DataTable dtRealiz2 = Config.hCntMainKasReal.GetRealizForCompare(Convert.ToDateTime(args[2]), Convert.ToDateTime(args[3]), id_otdel);

                dtGoodsRealiz = GetResultTable(dtGoods, dtRealiz1, dtRealiz2);

                DataTable dtRealiz3 = Config.hCntVVOKasReal.GetRealizForCompare(Convert.ToDateTime(args[0]), Convert.ToDateTime(args[1]), id_otdel);
                DataTable dtRealiz4 = Config.hCntVVOKasReal.GetRealizForCompare(Convert.ToDateTime(args[2]), Convert.ToDateTime(args[3]), id_otdel);

                DataTable dtGoods2 = Config.hCntVVOKasReal.GetGoods(id_otdel);
                DataTable dt = GetResultTable(dtGoods2, dtRealiz3, dtRealiz4);

                dtGoodsRealiz.Merge(dt, true, MissingSchemaAction.Ignore);
                //добавить ВВО
            }
            else
            {
                DataTable dtRealiz3 = Config.hCntVVOKasReal.GetRealizForCompare(Convert.ToDateTime(args[0]), Convert.ToDateTime(args[1]), id_otdel);
                DataTable dtRealiz4 = Config.hCntVVOKasReal.GetRealizForCompare(Convert.ToDateTime(args[2]), Convert.ToDateTime(args[3]), id_otdel);
                DataTable dtGoods2 = Config.hCntVVOKasReal.GetGoods(id_otdel);

                dtGoodsRealiz = GetResultTable(dtGoods2, dtRealiz3, dtRealiz4);

                //добавить ВВО
            }
        }

        private DataTable GetResultTable(DataTable dtGoods, DataTable dtRealiz1, DataTable dtRealiz2)
        {
            DataTable result = dtGoods.Copy();
            AddColumnsToTable(result, new string[] { "count1", "count2", "diff_count", "sum1", "sum2", "diff_sum" });

            for (int i = 0; i < result.Rows.Count; i++)
            {
                DataRow row = result.Rows[i];
                string ean = row["ean"].ToString();
                DataRow[] realiz1 = dtRealiz1.Select("ean  = '" + ean.Trim() + "'");
                DataRow[] realiz2 = dtRealiz2.Select("ean  = '" + ean.Trim() + "'");

                if (realiz1.Length == 0 && realiz2.Length == 0)
                {
                    result.Rows.RemoveAt(i);
                    i--;
                    continue;
                }

                decimal count1 = 0;
                decimal count2 = 0;
                decimal sum1 = 0;
                decimal sum2 = 0;

                if (realiz1.Length > 0)
                {
                    count1 = Convert.ToDecimal(realiz1[0]["count"]);
                    sum1 = Convert.ToDecimal(realiz1[0]["summa"]);
                }

                if (realiz2.Length > 0)
                {
                    count2 = Convert.ToDecimal(realiz2[0]["count"]);
                    sum2 = Convert.ToDecimal(realiz2[0]["summa"]);
                }

                row["count1"] = count1.ToString("N2");
                row["count2"] = count2.ToString("N2");
                row["diff_count"] = (count1 - count2).ToString("N2");
                row["sum1"] = sum1.ToString("N2");
                row["sum2"] = sum2.ToString("N2");
                row["diff_sum"] = (sum1 - sum2).ToString("N2");
            }

            return result;
        }

        private void AddColumnsToTable(DataTable table, string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                table.Columns.Add(columnName, typeof(decimal));
            }
        }

        private void bgw_Load_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgvRealiz.AutoGenerateColumns = false;
            dgvRealiz.DataSource = dtGoodsRealiz;
            EnableControlsService.RestoreControlEnabledState(this);
            frmWaiting.Close();

            Filter();
            SetSums();
            printLogShowForm();

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            EnableControlsService.SaveControlsEnabledState(this);
            EnableControlsService.SetControlsEnabled(this, false);

            frmWaiting = new frmLoad("Ждите, идёт загрузка данных...");
            frmWaiting.Show();

            bgw_Load.RunWorkerAsync(new object[] { DateStart1, DateEnd1, DateStart2, DateEnd2, Convert.ToInt32(cmbDepartments.SelectedValue) });
        }

        private void printLogShowForm()
        {
            Logging.StartFirstLevel(1488);
            Logging.Comment("Произведен просмотр данных на форме «Сравнение реализации» с параметрами");
            Logging.Comment($"Дата1:{dtpDate1.Value.ToShortDateString()}");
            Logging.Comment($"Время с:{dtpTimeStart1.Value.ToShortTimeString()} и по:{dtpTimeEnd1.Value.ToShortTimeString()}");
            Logging.Comment($"Значение чек-бокса «одно время»:{cbEqualTime.Checked.ToString()}");
            Logging.Comment($"Дата2:{dtpDate2.Value.ToShortDateString()}");
            Logging.Comment($"Время с:{dtpTimeStart2.Value.ToShortTimeString()} и по:{dtpTimeEnd2.Value.ToShortTimeString()}");
            Logging.Comment($"Id отдела:{cmbDepartments.SelectedValue.ToString()} Наименование:{cmbDepartments.Text}");
            Logging.Comment($"Id Т/У группы:{cmbTUGroups.SelectedValue.ToString()} Наименование:{cmbTUGroups.Text}");
            Logging.Comment($"Id инв. группы:{cmbInvGroups.SelectedValue.ToString()} Наименование:{cmbInvGroups.Text}");
            Logging.StopFirstLevel();
        }

        private DateTime DateStart1
        {
            get { return dtpDate1.Value.Date.AddHours(dtpTimeStart1.Value.Hour).AddMinutes(dtpTimeStart1.Value.Minute); }
        }

        private DateTime DateEnd1
        {
            get { return dtpDate1.Value.Date.AddHours(dtpTimeEnd1.Value.Hour).AddMinutes(dtpTimeEnd1.Value.Minute); }
        }

        private DateTime DateStart2
        {
            get { return dtpDate2.Value.Date.AddHours(dtpTimeStart2.Value.Hour).AddMinutes(dtpTimeStart2.Value.Minute); }
        }

        private DateTime DateEnd2
        {
            get { return dtpDate2.Value.Date.AddHours(dtpTimeEnd2.Value.Hour).AddMinutes(dtpTimeEnd2.Value.Minute); }
        }

        private void txtEAN_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void txtEAN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == '\b'))
            {
                e.Handled = true;
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (special_symbols.Contains(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvRealiz.DataSource != null)
            {
                DataTable dtReport = (dgvRealiz.DataSource as DataTable).DefaultView.ToTable();
                RemoveColumnsFromDataTable(dtReport, new string[] { "id_tovar", "id_otdel", "id_grp1", "id_grp2" });
                LogPrintReport();
                Reports.CompareRealizReport.Show(cmbDepartments.Text, cmbTUGroups.Text, cmbInvGroups.Text, txtEAN.Text, txtName.Text, DateStart1, DateEnd1, DateStart2, DateEnd2, dtReport, txtCount1.Text, txtCount2.Text, txtDiffCount.Text, txtSum1.Text, txtSum2.Text, txtDiffSum.Text);
            }
        }

        private void LogPrintReport()
        {
            Logging.StartFirstLevel(79);
            Logging.Comment("Выгрузка отчёта сравнения реализации товаров");
            Logging.Comment("Период 1: " + dtpDate1.Value.ToShortDateString() + " " + dtpTimeStart1.Value.ToShortTimeString() + " - " + dtpTimeEnd1.Value.ToShortTimeString());
            Logging.Comment("Период 2: " + dtpDate2.Value.ToShortDateString() + " " + dtpTimeStart2.Value.ToShortTimeString() + " - " + dtpTimeEnd2.Value.ToShortTimeString());
            Logging.Comment("Признак 'одно время':" + cbEqualTime.Checked.ToString());
            Logging.Comment("Отдел: " + cmbDepartments.Text);
            Logging.Comment("ТУ группа: " + cmbTUGroups.Text);
            Logging.Comment("Инв. группа: " + cmbInvGroups.Text);
            Logging.Comment("EAN: " + txtEAN.Text);
            Logging.Comment("Наименование товара: " + txtName.Text);
            Logging.StopFirstLevel();
        }

        private void RemoveColumnsFromDataTable(DataTable dt, string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                dt.Columns.Remove(columnName);
            }
        }

        private void SetSums()
        {
            decimal count1 = 0;
            decimal count2 = 0;
            decimal diff_count = 0;
            decimal sum1 = 0;
            decimal sum2 = 0;
            decimal diff_sum = 0;

            if (dgvRealiz.DataSource != null && dgvRealiz.RowCount > 0)
            {
                DataTable dtView = (dgvRealiz.DataSource as DataTable).DefaultView.ToTable();

                count1 = dtView.AsEnumerable().Sum(x => Convert.ToDecimal(x["count1"]));
                count2 = dtView.AsEnumerable().Sum(x => Convert.ToDecimal(x["count2"]));
                diff_count = dtView.AsEnumerable().Sum(x => Convert.ToDecimal(x["diff_count"]));

                sum1 = dtView.AsEnumerable().Sum(x => Convert.ToDecimal(x["sum1"]));
                sum2 = dtView.AsEnumerable().Sum(x => Convert.ToDecimal(x["sum2"]));
                diff_sum = dtView.AsEnumerable().Sum(x => Convert.ToDecimal(x["diff_sum"]));
            }

            txtCount1.Text = count1.ToString("N2");
            txtCount2.Text = count2.ToString("N2");
            txtDiffCount.Text = diff_count.ToString("N2");

            txtSum1.Text = sum1.ToString("N2");
            txtSum2.Text = sum2.ToString("N2");
            txtDiffSum.Text = diff_sum.ToString("N2");
        }

        private void cbEqualTime_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEqualTime.Checked)
            {
                SetEqualTime();
                ClearGrid();
            }
            dtpTimeStart2.Enabled = dtpTimeEnd2.Enabled = !cbEqualTime.Checked;
        }

        private void SetEqualTime()
        {
            dtpTimeStart2.Value = dtpTimeStart1.Value;
            dtpTimeEnd2.Value = dtpTimeEnd1.Value;
        }

        private void dtpTimeStart1_ValueChanged(object sender, EventArgs e)
        {
            if (cbEqualTime.Checked)
            {
                dtpTimeStart2.Value = dtpTimeStart1.Value;
            }

            if (dtpTimeStart1.Value > dtpTimeEnd1.Value)
            {
                dtpTimeStart1.Value = dtpTimeEnd1.Value;
            }

            ClearGrid();
        }

        private void dtpTimeEnd1_ValueChanged(object sender, EventArgs e)
        {
            if (cbEqualTime.Checked)
            {
                dtpTimeEnd2.Value = dtpTimeEnd1.Value;
            }

            if (dtpTimeEnd1.Value < dtpTimeStart1.Value)
            {
                dtpTimeEnd1.Value = dtpTimeStart1.Value;
            }

            ClearGrid();
        }

        private void dtpTimeStart2_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTimeStart2.Value > dtpTimeEnd2.Value)
            {
                dtpTimeStart2.Value = dtpTimeEnd2.Value;
            }

            ClearGrid();
        }

        private void dtpTimeEnd2_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTimeEnd2.Value < dtpTimeStart2.Value)
            {
                dtpTimeEnd2.Value = dtpTimeStart2.Value;
            }

            ClearGrid();
        }

        private void dtpDate1_ValueChanged(object sender, EventArgs e)
        {
            ClearGrid();
        }

        private void dtpDate2_ValueChanged(object sender, EventArgs e)
        {
            ClearGrid();
        }

        private void ClearGrid()
        {
            dgvRealiz.DataSource = null;
            SetSums();
        }

        private void ClearFilters()
        {
            dtpDate1.Value = DateTime.Now.Date.AddDays(-1);
            dtpDate2.Value = DateTime.Now.Date;

            dtpTimeStart1.Value = dtpTimeStart2.Value = DateTime.Now.Date.AddHours(6);
            dtpTimeEnd1.Value = dtpTimeEnd2.Value = DateTime.Now.Date.AddHours(23).AddMinutes(59);

            cmbDepartments.SelectedValue = cmbTUGroups.SelectedValue = cmbInvGroups.SelectedValue = 0;

            cbEqualTime.Checked = false;

            SetSums();
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }
    }
}
