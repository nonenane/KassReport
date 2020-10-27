using Nwuram.Framework.Logging;
using Nwuram.Framework.Settings.Connection;
using Nwuram.Framework.Settings.User;
using Nwuram.Framework.UI.Forms;
using Nwuram.Framework.UI.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CashReportNew
{
    public partial class MainForm : Form
    {
        private char[] special_symbols = new char[] { '\\', '/', '%', '*', '(', ')', '[', ']' };
        private DataTable dtCash = null;
        private DataTable dtRealiz = null;
        private DataTable dtTerminals = null;
        private DataTable dtKassirNames = null;
        private frmLoad frmWaiting = null;
        private bool dtp_cash = false;
        string cmbDepsRealizSelectedValue;

        //private decimal ret;
        //private decimal net;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (ConnectionSettings.GetServer().Contains("K21"))
            {
                this.Text += UserSettings.User.Status + ". " + UserSettings.User.FullUsername + " K21";
            }
            else
            {
                this.Text += UserSettings.User.Status + ". " + UserSettings.User.FullUsername + " Х14";
            }
            tslConnection.Text = ConnectionSettings.GetServer() + " - " + ConnectionSettings.GetDatabase();
            toolStripStatusLabel2.Text = ConnectionSettings.GetServer("2") + "-" + ConnectionSettings.GetDatabase();

            dtpDateCash.MinDate = new DateTime(2017, 04, 01);
            dtpDateCash.MaxDate = DateTime.Today;

            dtpDateStart.MinDate = new DateTime(2017, 04, 01);
            dtpDateStart.MaxDate = DateTime.Today;

            dtpDateEnd.MinDate = new DateTime(2017, 04, 01);
            dtpDateEnd.MaxDate = DateTime.Today;

            dtpDateCash.Value = DateTime.Now.Date;

            tabCash_Load();
            tabRealiz_Load();
            panel2.BackColor = Color.FromArgb(254, 75, 75);

            Console.WriteLine(cmbDepsCash.SelectedValue.ToString());

            DataTable dTypeNote = new DataTable();
            dTypeNote.Columns.Add("id", typeof(int));
            dTypeNote.Columns.Add("cName", typeof(string));

            dTypeNote.Rows.Add(1, "Все типы");
            dTypeNote.Rows.Add(2, "Продажи");
            dTypeNote.Rows.Add(3, "Возврат");

            cmbTypeNotes.DataSource = dTypeNote;
            cmbTypeNotes.DisplayMember = "cName";
            cmbTypeNotes.ValueMember = "id";

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            deleteOldReport();
            if (MessageBox.Show("Вы хотите выйти из программы?", "Выход из программы", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        public void deleteOldReport()
        {
            string[] dirs = Directory.GetFiles(Application.StartupPath, "*.xlsx");

            foreach (string n in dirs)
            {
                try
                {
                    System.GC.Collect(); // попытка остановить процесс и удалить предыдущий файл excel
                    System.GC.WaitForPendingFinalizers();
                    File.Delete(n);
                }
                catch { continue; }
            }
        }

        #region Формирование запросов с касс

        private void txtEAN_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckDigits(e);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (special_symbols.Contains(e.KeyChar) || e.KeyChar == '\'')
            {
                e.Handled = true;
            }
        }

        private void txtDocsStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckDigits(e);
        }

        private void txtDocsEnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckDigits(e);
        }

        private void txtSumStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckDigits(e);
        }

        private void txtSumEnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckDigits(e);
        }

        private void CheckDigits(KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == '\b'))
            {
                e.Handled = true;
            }
        }

        private void tabCash_Load()
        {
            cmbDepsCash_Load();
            cmbLegalEntities_Load();
            ClearFilters();
        }

        private void ClearFilters()
        {
            dtpDateCash.Value = DateTime.Now.Date;
            dtpTimeStart.Value = DateTime.Now.Date.AddHours(6);
            dtpTimeEnd.Value = DateTime.Now.Date.AddHours(23).AddMinutes(59);
            cmbTerminals.SelectedValue = 0;
            cmbKassir.SelectedValue = 0;

            if (UserSettings.User.StatusCode != "МН")
                cmbDepsCash.SelectedValue = 0;
            cbCash.Checked = cbKassir.Checked = false;
            txtDocsStart.Text = txtDocsEnd.Text = txtSumStart.Text = txtSumEnd.Text = "";
            txtCountStart.Text = txtCountEnd.Text = "";
            txtEAN.Text = txtName.Text = "";
            cbVozvr.Checked = cbAnnul.Checked = cbActions.Checked = false;
            CountSums();
        }

        private void cmbDepsCash_Load()
        {
            DataTable dt = Config.hCntMainKasReal.GetDepartments();
            DataTable dt2 = Config.hCntVVOKasReal.GetDepartments();
            if (dt2 != null && dt != null)
                dt.Merge(dt2, true, MissingSchemaAction.Ignore);
            dt.DefaultView.Sort = "id ASC";
            cmbDepsCash.DataSource = dt;
            cmbDepsCash.ValueMember = "id";
            cmbDepsCash.DisplayMember = "name";

            //май 2020 доработка - выбор отдела из авторизации и выключаем выбор
            if (UserSettings.User.StatusCode == "МН")
            {
                cmbDepsCash.SelectedValue = UserSettings.User.IdDepartment;
                cmbDepsCash.Enabled = false;
            }
        }

        private void cmbLegalEntities_Load()
        {
            if (cmbDepsCash.SelectedValue == null) return;
            DataTable dt = new DataTable();
            if (cmbDepsCash.SelectedValue.ToString() == "6")
            {
                dt = Config.hCntVVOKasReal.getLegalEntities(dtpDateCash.Value);
                DataRow rowToAdd = dt.NewRow();
                rowToAdd["id"] = 0;
                rowToAdd["Abbriviation"] = "ВСЕ ЮЛ";
                dt.Rows.InsertAt(rowToAdd, 0);
            }
            // втсавить ВСЕ ОТДЕЛЫ
            else if (cmbDepsCash.SelectedValue.ToString() == "0")
            {
                DataTable d = Config.hCntVVOKasReal.getLegalEntities(dtpDateCash.Value);
                dt = Config.hCntMainKasReal.getLegalEntities(dtpDateCash.Value);
                if (d != null && dt != null)
                    dt.Merge(d, true, MissingSchemaAction.Ignore);
            }
            else dt = Config.hCntMainKasReal.getLegalEntities(dtpDateCash.Value);
            if (dt == null) return;
            dt.DefaultView.Sort = "id ASC";
            cmbLegalEntities.ValueMember = "id";
            cmbLegalEntities.DisplayMember = "Abbriviation";
            cmbLegalEntities.DataSource = dt;
        }
        private void cmbTerminals_Load()
        {
            int pos = 0;
            if (cmbTerminals.SelectedValue != null)
                pos = Convert.ToInt32(cmbTerminals.SelectedValue);

            dtTerminals = Config.hCntMainKasReal.GetTerminals(dtpDateCash.Value);
            cmbTerminals.DataSource = dtTerminals;
            cmbTerminals.ValueMember = "terminal";
            cmbTerminals.DisplayMember = "terminal";
            cmbTerminals.Enabled = cbCash.Checked;

            if (cmbTerminals.DataSource == null) return;
            if ((cmbTerminals.DataSource as DataTable).Rows.Count == 0) return;

            if (pos > 0)
            {
                cmbTerminals.SelectedValue = pos;
                if (cmbTerminals.SelectedIndex < 0)
                {
                    cbCash.Checked = false;
                    cmbTerminals.SelectedIndex = 0;
                }
            }
            else
            {
                cbCash.Checked = false;
                cmbTerminals.SelectedIndex = 0;
            }
        }

        private void cmbKassir_Load()
        {
            string pos = "";
            if (cmbKassir.SelectedValue != null)
                pos = cmbKassir.SelectedValue.ToString();

            dtKassirNames = Config.hCntMainKasReal.GetKassirNames(dtpDateCash.Value);
            cmbKassir.DataSource = dtKassirNames;
            cmbKassir.ValueMember = "kassir_id";
            cmbKassir.DisplayMember = "kassir_name";
            cmbKassir.Enabled = cbKassir.Checked;

            if (cmbKassir.DataSource == null) return;
            if ((cmbKassir.DataSource as DataTable).Rows.Count == 0) return;

            if (pos != "")
            {
                cmbKassir.SelectedValue = pos;
                if (cmbKassir.SelectedIndex < 0)
                {
                    cbKassir.Checked = false;
                    cmbKassir.SelectedIndex = 0;
                }

            }
            else
            {
                cbKassir.Checked = false;
                cmbKassir.SelectedIndex = 0;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            EnableControlsService.SaveControlsEnabledState(this);
            EnableControlsService.SetControlsEnabled(this, false);

            frmWaiting = new frmLoad("Ждите, идёт загрузка данных...");
            frmWaiting.TopMost = false;
            frmWaiting.Show();

            DateTime startTime;
            DateTime endTime;
            if (cbDayData.Checked)
            {
                startTime = dtpDateCash.Value.Date.AddHours(6).AddMinutes(0);
                endTime = dtpDateCash.Value.Date.AddHours(4).AddMinutes(0).AddDays(1);
            }
            else
            {
                startTime = dtpDateCash.Value.Date.AddHours(dtpTimeStart.Value.Hour).AddMinutes(dtpTimeStart.Value.Minute);
                endTime = dtpDateCash.Value.Date.AddHours(dtpTimeEnd.Value.Hour).AddMinutes(dtpTimeEnd.Value.Minute);
            }

            string code = UserSettings.User.StatusCode;

            bgw_LoadCash.RunWorkerAsync(new object[] { startTime,
                                                       endTime,
                                                       (code=="МН" ? Convert.ToInt32(cmbDepsCash.SelectedValue) : 0),
                                                       (cbCash.Checked ? (int?)Convert.ToInt32(cmbTerminals.SelectedValue) : null),
                                                       (cbKassir.Checked ? (long?)Convert.ToInt32(cmbKassir.SelectedValue) : null),
                                                       (txtDocsStart.Text.Length > 0 ? (int?)Convert.ToInt32(txtDocsStart.Text) : null),
                                                       (txtDocsEnd.Text.Length > 0 ? (int?)Convert.ToInt32(txtDocsEnd.Text) : null),
                                                       (txtSumStart.Text.Length > 0 ? (int?)Convert.ToInt64(txtSumStart.Text) : null),
                                                       (txtSumEnd.Text.Length > 0 ? (int?)Convert.ToInt64(txtSumEnd.Text) : null),
                                                       (txtCountStart.Text.Length > 0 ? (long?)(double.Parse(txtCountStart.Text) * Convert.ToDouble(1000)) : null),
                                                       (txtCountEnd.Text.Length > 0 ? (long?)(double.Parse(txtCountEnd.Text) * Convert.ToDouble(1000)) : null)});
        }

        private void bgw_LoadCash_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];
            //получение данных с двух коннектов
            if (Convert.ToInt32(args[2]) == 6)
                dtCash = Config.hCntVVOKasReal.GetCashData(Convert.ToDateTime(args[0]), Convert.ToDateTime(args[1]), Convert.ToInt32(args[2]), (int?)args[3], (long?)args[4], (int?)args[5], (int?)args[6], (int?)args[7], (int?)args[8], (long?)args[9], (long?)args[10]);
            else if (Convert.ToInt32(args[2]) == 0)
            {
                dtCash = Config.hCntMainKasReal.GetCashData(Convert.ToDateTime(args[0]), Convert.ToDateTime(args[1]), Convert.ToInt32(args[2]), (int?)args[3], (long?)args[4], (int?)args[5], (int?)args[6], (int?)args[7], (int?)args[8], (long?)args[9], (long?)args[10]);
                DataTable dt2 = Config.hCntVVOKasReal.GetCashData(Convert.ToDateTime(args[0]), Convert.ToDateTime(args[1]), 6, (int?)args[3], (long?)args[4], (int?)args[5], (int?)args[6], (int?)args[7], (int?)args[8], (long?)args[9], (long?)args[10]);
                if (dt2 != null)
                    dtCash.Merge(dt2, true, MissingSchemaAction.Ignore);
            }
            else
                dtCash = Config.hCntMainKasReal.GetCashData(Convert.ToDateTime(args[0]), Convert.ToDateTime(args[1]), Convert.ToInt32(args[2]), (int?)args[3], (long?)args[4], (int?)args[5], (int?)args[6], (int?)args[7], (int?)args[8], (long?)args[9], (long?)args[10]);
        }

        private void bgw_LoadCash_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgvCash.AutoGenerateColumns = false;
            dgvCash.DataSource = dtCash;
            dgvCash_Filter();

            //if (cbVozvr.Checked)
            //{
            //    cbVozvr.Checked = false;
            //    dgvCash_Filter();

            //    DataTable dt = (dgvCash.DataSource as DataTable).DefaultView.ToTable();
            //    ret = GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 507");
            //    net = GetDecimalSumValue(dt, "sum(count)", "op_code = 507");
            //    cbVozvr.Checked = true;

            //    dgvCash_Filter();
            //}
            //else
            //{
            //    dgvCash_Filter();
            //    DataTable dt = (dgvCash.DataSource as DataTable).DefaultView.ToTable();
            //    ret = GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 507");
            //    net = GetDecimalSumValue(dt, "sum(count)", "op_code = 507");
            //}


            EnableControlsService.RestoreControlEnabledState(this);
            frmWaiting.Close();
            printLogViewForm();
        }

        private void printLogViewForm()
        {
            Logging.StartFirstLevel(1488);
            Logging.Comment("Произведен просмотр данных на форме «Формирование запросов с касс» с параметрами:");
            Logging.Comment($"Дата: {dtpDateCash.Value.ToShortDateString()}");
            Logging.Comment($"Id ЮЛ фильтра:{cmbLegalEntities.SelectedValue.ToString()} Аббривиатура:{cmbLegalEntities.Text} ");
            if (!cbDayData.Checked)
                Logging.Comment("Время с " + dtpTimeStart.Value.ToShortTimeString() + " по " + dtpTimeEnd.Value.ToShortTimeString());
            Logging.Comment($"Id отдела:{cmbDepsCash.SelectedValue.ToString()} Наименование:{cmbDepsCash.Text}");
            if (cbCash.Checked)
                Logging.Comment($"Id кассы:{cmbTerminals.SelectedValue.ToString()} Наименование{cmbTerminals.Text}");
            if (cbKassir.Checked)
                Logging.Comment($"Id кассира:{cmbKassir.SelectedValue.ToString()} ФИО:{cmbKassir.Text}");
            if (txtDocsStart.Text.Length > 0 || txtDocsEnd.Text.Length > 0)
            {
                Logging.Comment("Чеки с " + txtDocsStart.Text + " по " + txtDocsEnd.Text);
            }

            if (txtSumStart.Text.Length > 0 || txtSumEnd.Text.Length > 0)
            {
                Logging.Comment("Сумма с " + txtSumStart.Text + " по " + txtSumEnd.Text);
            }
            if (txtCountStart.Text.Length > 0 && txtCountEnd.Text.Length > 0)
                Logging.Comment($"Вес от {txtCountStart.Text} до {txtCountEnd.Text}");
            Logging.Comment($"Значение чек-бокса «Данные за рабочий день»:{cbDayData.Checked.ToString()}");
            Logging.Comment($"Значение чек-бокса «Возвраты»:{cbVozvr.Checked.ToString()}");
            Logging.Comment($"Значение чек-бокса «Аннулированные записи»:{cbAnnul.Checked.ToString()}");
            Logging.Comment($"Значение чек-бокса «Акции»:{cbActions.Checked.ToString()}");
            Logging.StopFirstLevel();
        }

        private void printLogViewFormRealiz()
        {
            Logging.StartFirstLevel(1488);
            Logging.Comment("Произведен просмотр данных на форме «Реализация товаров» с параметрами:");
            Logging.Comment($"Период с: {dtpDateStart.Value.ToShortDateString()} и по:{dtpDateEnd.Value.ToShortDateString()}");
            Logging.Comment($"Время с: {dtpTimeStartRealiz.Value.ToShortTimeString()} и по:{dtpTimeEndRealiz.Value.ToShortTimeString()}");
            Logging.Comment($"Id ЮЛ фильтра:{cmbLegalEntitiesRealiz.SelectedValue.ToString()} Аббривиатура:{cmbLegalEntitiesRealiz.Text} ");
            Logging.Comment($"Id отдела:{cmbDepsRealiz.SelectedValue.ToString()} Наименование:{cmbDepsRealiz.Text}");
            Logging.Comment($"Id Т/У группы:{cmbGroups.SelectedValue.ToString()} Наименование:{cmbGroups.Text}");
            Logging.Comment($"Значение чек-бокса «Только оптовые кассы»:{Wholesale_checkBox.Checked.ToString()}");
            Logging.Comment($"Значение чек-бокса «Оптовые безналичные отгрузки»:{Cashless_checkBox.Checked.ToString()}");

            Logging.StopFirstLevel();
        }

        private void dgvCash_Filter()
        {
            if (dtCash != null)
            {
                try
                {
                    string filter = "ean like '%" + txtEAN.Text + "%' and cname like '%" + txtName.Text + "%'";

                    if (!cbVozvr.Checked)
                    {
                        filter += " and op_code <> 507";

                    }

                    //if (!cbAnnul.Checked)
                    //{
                    //filter += " and is_annul = false";
                    //}

                    if (!cbActions.Checked)
                    {
                        filter += " and op_code <> 524";
                    }

                    filter += cmbLegalEntities.SelectedValue.ToString() == "0" ? "" : $" AND legalEntity LIKE \'%{cmbLegalEntities.Text}%\'";

                    if (cbCash.Checked)
                        filter += " and terminal = " + cmbTerminals.SelectedValue.ToString();
                    if (cbKassir.Checked)
                        filter += $" and kassir_name LIKE '%{cmbKassir.Text.ToString()}%'";
                    if (cmbDepsCash.SelectedValue.ToString() != "0")
                        filter += " and idDep = " + cmbDepsCash.SelectedValue.ToString();
                    dtCash.DefaultView.RowFilter = filter;

                    //if (cbVozvr.Checked)
                    //{
                    //    string dop_f = filter.Replace("and op_code <> 507", "");

                    //    (dgvCash.DataSource as DataTable).DefaultView.RowFilter = dop_f;

                    //    ret = GetDecimalSumValue((dgvCash.DataSource as DataTable).DefaultView.ToTable(), "sum(cash_val)", "op_code = 507");
                    //    net = GetDecimalSumValue((dgvCash.DataSource as DataTable).DefaultView.ToTable(), "sum(count)", "op_code = 507");

                    //    (dgvCash.DataSource as DataTable).DefaultView.RowFilter = filter;
                    //}
                    //else
                    //{
                    //    (dgvCash.DataSource as DataTable).DefaultView.RowFilter = filter;
                    //}


                    CountSums();
                    dgvCash.DataSource = dtCash;
                    dgvCash.Refresh();
                }
                catch { }
            }
        }

        private void txtEAN_TextChanged(object sender, EventArgs e)
        {
            dgvCash_Filter();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            dgvCash_Filter();
        }

        private void cbVozvr_CheckedChanged(object sender, EventArgs e)
        {
            dgvCash_Filter();
        }

        private void cbAnnul_CheckedChanged(object sender, EventArgs e)
        {
            dgvCash_Filter();
        }

        private void cbActions_CheckedChanged(object sender, EventArgs e)
        {
            dgvCash_Filter();
        }

        private void dgvCash_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Rows[e.RowIndex].Selected)
            {
                int width = dgv.Width;
                Rectangle r = dgv.GetRowDisplayRectangle(e.RowIndex, false);
                Rectangle rect = new Rectangle(r.X, r.Y, width - 1, r.Height - 1);

                ControlPaint.DrawBorder(e.Graphics, rect,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid,
                    SystemColors.Highlight, 2, ButtonBorderStyle.Solid);
            }
        }

        private void dgvCash_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow curRow = dgvCash.Rows[e.RowIndex];
                Color color = Color.White;



                if (Convert.ToBoolean(curRow.Cells["is_annul"].Value) && cbAnnul.Checked)
                {
                    color = pnlAnnul.BackColor;
                }
                else if (Convert.ToInt32(curRow.Cells["op_code"].Value) == 507)
                {
                    color = pnlVozvr.BackColor;
                }
                else if (Convert.ToInt32(curRow.Cells["op_code"].Value) == 524)
                {
                    color = pnlActions.BackColor;
                }

                curRow.DefaultCellStyle.BackColor = curRow.DefaultCellStyle.SelectionBackColor = color;
            }
        }

        private void cbCash_CheckedChanged(object sender, EventArgs e)
        {
            cmbTerminals.Enabled = cbCash.Checked;
            dgvCash_Filter();
            //ClearGrid();
        }

        private void cbKassir_CheckedChanged(object sender, EventArgs e)
        {
            cmbKassir.Enabled = cbKassir.Checked;
            dgvCash_Filter();
            //ClearGrid();
        }

        private void dtpDateCash_ValueChanged(object sender, EventArgs e)
        {
            if (!dtp_cash)
            {
                ClearGrid();
                TerminalsAndKassirNamesLoad();

            }
            cmbLegalEntities_Load();
        }

        private void cmbDepsCash_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearGrid();
            cmbLegalEntities_Load();
        }

        private void cmbTerminals_SelectedValueChanged(object sender, EventArgs e)
        {
            dgvCash_Filter();
            //ClearGrid();
        }

        private void cmbKassir_SelectedValueChanged(object sender, EventArgs e)
        {
            dgvCash_Filter();
            //ClearGrid();
        }

        private void txtDocsStart_TextChanged(object sender, EventArgs e)
        {
            ClearGrid();
        }

        private void txtDocsEnd_TextChanged(object sender, EventArgs e)
        {
            ClearGrid();
        }

        private void dtpTimeStart_ValueChanged(object sender, EventArgs e)
        {
            if ((dtpTimeStart.Value - dtpTimeEnd.Value).TotalSeconds > 0)
            {
                dtpTimeStart.Value = dtpTimeEnd.Value;
            }
            ClearGrid();
        }

        private void dtpTimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if ((dtpTimeStart.Value - dtpTimeEnd.Value).TotalSeconds > 0)
            {
                dtpTimeEnd.Value = dtpTimeStart.Value;
            }
            ClearGrid();
        }

        private void txtSumStart_TextChanged(object sender, EventArgs e)
        {
            ClearGrid();
        }

        private void txtSumEnd_TextChanged(object sender, EventArgs e)
        {
            ClearGrid();
        }

        private void ClearGrid()
        {
            //dgvCash.DataSource = null;
            CountSums();
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CountSums()
        {
            if (dgvCash.DataSource != null && dgvCash.RowCount > 0)
            {
                DataTable dt = (dgvCash.DataSource as DataTable).DefaultView.ToTable();

                //if (cbVozvr.Checked)
                //{
                //    txtRealizNetto.Text = (GetDecimalSumValue(dt, "sum(count)", "") - net).ToString("N3");
                //    txtRealizSum.Text = (GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 505") - ret).ToString("N2");
                //}
                //else
                //{
                //    txtRealizNetto.Text = GetDecimalSumValue(dt, "sum(count)", "op_code = 505").ToString("N3");
                //    txtRealizSum.Text = (GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 505")).ToString("N2");
                //}

                if (cbAnnul.Checked)
                {
                    txtRealizNetto.Text = (GetDecimalSumValue(dt, "sum(count)", "op_code = 505 and is_annul = false") - GetDecimalSumValue(dt, "sum(count)", "op_code = 507 and is_annul = false")).ToString("N3");
                    txtRealizSum.Text = (GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 505 and is_annul = false") - GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 507 and is_annul = false")).ToString("N2");
                }
                else
                {
                    txtRealizNetto.Text = (GetDecimalSumValue(dt, "sum(count)", "op_code = 505") - GetDecimalSumValue(dt, "sum(count)", "op_code = 507")).ToString("N3");
                    txtRealizSum.Text = (GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 505") - GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 507")).ToString("N2");
                }

                //txtRealizNetto.Text = (GetDecimalSumValue(dt, "sum(count)", "op_code = 505") - GetDecimalSumValue(dt, "sum(count)", "op_code = 507")).ToString("N3");
                //txtRealizSum.Text = (GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 505") - GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 507")).ToString("N2");
                txtClients.Text = dt.AsEnumerable().Select(r => r["terminal"].ToString() + "-" + r["doc_id"].ToString()).Distinct().Count().ToString();
            }
            else
            {
                txtRealizNetto.Text = txtRealizSum.Text = txtClients.Text = "";
            }
        }

        private void TerminalsAndKassirNamesLoad()
        {
            EnableControlsService.SaveControlsEnabledState(this);
            EnableControlsService.SetControlsEnabled(this, false);

            frmWaiting = new frmLoad("Ждите, идёт загрузка номеров касс и фамилий кассиров...");
            frmWaiting.Show();

            bgw_LoadTerminals.RunWorkerAsync(dtpDateCash.Value.Date);
        }

        private void bgw_LoadTerminals_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime date = Convert.ToDateTime(e.Argument);
            dtTerminals = Config.hCntMainKasReal.GetTerminals(date);
            dtKassirNames = Config.hCntMainKasReal.GetKassirNames(date);
        }

        private void bgw_LoadTerminals_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbTerminals_Load();
            cmbKassir_Load();

            frmWaiting.Close();
            EnableControlsService.RestoreControlEnabledState(this);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvCash.DataSource != null && (dgvCash.DataSource as DataTable).DefaultView.ToTable().Rows.Count > 0)
            {
                DataTable dt;
                if (MessageBox.Show("Напечатать товары только с разными ЮЛ?", "Запрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    dt = (dgvCash.DataSource as DataTable).DefaultView.ToTable();
                    Reports.CashReport.printReport(dtpDateCash.Value, cmbDepsCash.Text, txtDocsStart.Text, txtDocsEnd.Text, dtpTimeStart.Value.ToShortTimeString(), dtpTimeEnd.Value.ToShortTimeString(), txtSumStart.Text, txtSumEnd.Text, dt, new List<string>() { txtRealizNetto.Text, txtRealizSum.Text, txtClients.Text }, new List<Color>() { pnlVozvr.BackColor, pnlAnnul.BackColor, pnlActions.BackColor });
                }
                else
                {
                    dt = (dgvCash.DataSource as DataTable).DefaultView.ToTable().AsEnumerable().Where(r => r.Field<string>("legalEntity") != r.Field<string>("legalEntityTK")).CopyToDataTable();
                    List<string> computedTxt = new List<string>();
                    if (cbAnnul.Checked)
                    {
                        computedTxt.Add( (GetDecimalSumValue(dt, "sum(count)", "op_code = 505 and is_annul = false") - GetDecimalSumValue(dt, "sum(count)", "op_code = 507 and is_annul = false")).ToString("N3"));
                        computedTxt.Add( (GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 505 and is_annul = false") - GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 507 and is_annul = false")).ToString("N2"));
                    }
                    else
                    {
                        computedTxt.Add( (GetDecimalSumValue(dt, "sum(count)", "op_code = 505") - GetDecimalSumValue(dt, "sum(count)", "op_code = 507")).ToString("N3"));
                        computedTxt.Add( (GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 505") - GetDecimalSumValue(dt, "sum(cash_val)", "op_code = 507")).ToString("N2"));
                    }
                    computedTxt.Add(dt.AsEnumerable().Select(r => r["terminal"].ToString() + "-" + r["doc_id"].ToString()).Distinct().Count().ToString());
                    Reports.CashReport.printReport(dtpDateCash.Value, cmbDepsCash.Text, txtDocsStart.Text, txtDocsEnd.Text, dtpTimeStart.Value.ToShortTimeString(), dtpTimeEnd.Value.ToShortTimeString(), txtSumStart.Text, txtSumEnd.Text, dt, computedTxt, 
                        new List<Color>() { pnlVozvr.BackColor, pnlAnnul.BackColor, pnlActions.BackColor });
                }
                //Reports.CashReport.Show(dtpDateCash.Value, cmbDepsCash.Text, txtDocsStart.Text, txtDocsEnd.Text, dtpTimeStart.Value.ToShortTimeString(), dtpTimeEnd.Value.ToShortTimeString(), txtSumStart.Text, txtSumEnd.Text, dt, new List<string>() { txtRealizNetto.Text, txtRealizSum.Text, txtClients.Text }, new List<Color>() { pnlVozvr.BackColor, pnlAnnul.BackColor, pnlActions.BackColor });
                LogPrint();
            }
            else
            {
                MessageBox.Show("Нет данных для выгрузки!");
            }
        }

        private void LogPrint()
        {
            Logging.StartFirstLevel(1066);
            Logging.Comment("Начало выгрузки данных с вкладки \"Формирование запросов с касс\"");
            Logging.Comment("Дата выгрузки = " + dtpDateCash.Value.ToShortDateString() + ", отдел = " + cmbDepsCash.Text);
            if (txtDocsStart.Text.Length > 0 || txtDocsEnd.Text.Length > 0)
            {
                Logging.Comment("Чеки с " + txtDocsStart.Text + " по " + txtDocsEnd.Text);
            }

            if (txtSumStart.Text.Length > 0 || txtSumEnd.Text.Length > 0)
            {
                Logging.Comment("Сумма с " + txtSumStart.Text + " по " + txtSumEnd.Text);
            }
            Logging.Comment($"Id ЮЛ фильтра:{cmbLegalEntities.SelectedValue.ToString()} Аббривиатура:{cmbLegalEntities.Text} ");
            if (cbCash.Checked)
                Logging.Comment($"Id кассы:{cmbTerminals.SelectedValue.ToString()} Наименование{cmbTerminals.Text}");
            if (cbKassir.Checked)
                Logging.Comment($"Id кассира:{cmbKassir.SelectedValue.ToString()} ФИО:{cmbKassir.Text}");
            if (txtCountStart.Text.Length > 0 && txtCountEnd.Text.Length > 0)
                Logging.Comment($"Вес от {txtCountStart.Text} до {txtCountEnd.Text}");
            Logging.Comment($"Значение чек-бокса «Данные за рабочий день»:{cbDayData.Checked.ToString()}");
            if (!cbDayData.Checked)
                Logging.Comment("Время с " + dtpTimeStart.Value.ToShortTimeString() + " по " + dtpTimeEnd.Value.ToShortTimeString());
            Logging.Comment($"Значение чек-бокса «Возвраты»:{cbVozvr.Checked.ToString()}");
            Logging.Comment($"Значение чек-бокса «Аннулированные записи»:{cbAnnul.Checked.ToString()}");
            Logging.Comment($"Значение чек-бокса «Акции»:{cbActions.Checked.ToString()}");
            if (txtEAN.Text.Length > 0)
                Logging.Comment($"Значение фильтра по EAN:{txtEAN.Text}");
            if (txtName.Text.Length > 0)
                Logging.Comment($"Значение фильтра по наименованию товара:{txtName.Text}");
            Logging.Comment("Конец выгрузки данных");
            Logging.StopFirstLevel();
        }

        #endregion

        #region Реализация товаров

        private void tabRealiz_Load()
        {
            cmbDepsRealiz_Load();
            cmbGroups_Load();
            cmbLegalEntitiesRealiz_Load();
            cmbTerminalsRealiz_Load();
            ClearRealizFilters();
        }
        DataTable dtTerminalsRealiz = new DataTable();
        private void cmbTerminalsRealiz_Load()
        {
            int pos = 0;
            if (cmbTerminalsRealiz.SelectedValue != null)
                pos = Convert.ToInt32(cmbTerminalsRealiz.SelectedValue);

            dtTerminals = Config.hCntMainKasReal.GetTerminals(dtpDateCash.Value);
            cmbTerminalsRealiz.DataSource = dtTerminals;
            cmbTerminalsRealiz.ValueMember = "terminal";
            cmbTerminalsRealiz.DisplayMember = "terminal";
            cmbTerminalsRealiz.Enabled = cbCash.Checked;

            if (cmbTerminalsRealiz.DataSource == null) return;
            if ((cmbTerminalsRealiz.DataSource as DataTable).Rows.Count == 0) return;

            if (pos > 0)
            {
                cmbTerminalsRealiz.SelectedValue = pos;
                if (cmbTerminalsRealiz.SelectedIndex < 0)
                {
                    cbCash.Checked = false;
                    if (cmbTerminals.Items.Count > 0)
                        cmbTerminals.SelectedIndex = 0;
                }
            }
            else
            {
                cbCash.Checked = false;
                if (cmbTerminals.Items.Count > 0)
                    cmbTerminals.SelectedIndex = 0;
            }
        }
        private void cmbDepsRealiz_Load()
        {
            DataTable dt = Config.hCntMainKasReal.GetDepartments();
            DataTable dt2 = Config.hCntVVOKasReal.GetDepartments();
            if (dt != null && dt2 != null)
                dt.Merge(dt2, true, MissingSchemaAction.Ignore);
            dt.DefaultView.Sort = "id ASC";

            cmbDepsRealiz.ValueMember = "id";
            cmbDepsRealiz.DisplayMember = "name";
            cmbDepsRealiz.DataSource = dt;

            //май 2020 доработка - выбор отдела из авторизации и выключаем выбор
            if (UserSettings.User.StatusCode == "МН")
            {
                cmbDepsRealiz.SelectedValue = UserSettings.User.IdDepartment;
                cmbDepsRealiz.Enabled = false;
            }
        }

        private void cmbGroups_Load()
        {
            if (cmbDepsRealiz.SelectedValue != null)
            {
                if (Convert.ToInt32(cmbDepsRealiz.SelectedValue.ToString()) != 6)
                    cmbGroups.DataSource = Config.hCntMainKasReal.GetGroups(Convert.ToInt32(cmbDepsRealiz.SelectedValue.ToString()));
                else cmbGroups.DataSource = Config.hCntVVOKasReal.GetGroups(Convert.ToInt32(cmbDepsRealiz.SelectedValue.ToString()));
                cmbGroups.ValueMember = "id";
                cmbGroups.DisplayMember = "name";
            }
            cmbGroups.Enabled = cmbDepsRealiz.SelectedValue != null ? Convert.ToInt32(cmbDepsRealiz.SelectedValue.ToString()) != 0 ? true : false : false;
        }

        private void cmbLegalEntitiesRealiz_Load()
        {
            if (cmbDepsRealiz.DataSource == null) return;
            DataTable dt = new DataTable();
            if (cmbDepsRealiz.SelectedValue.ToString() == "6")
            {
                dt = Config.hCntVVOKasReal.getLegalEntities(dtpDateStart.Value);
                DataRow rowToAdd = dt.NewRow();
                rowToAdd["id"] = 0;
                rowToAdd["Abbriviation"] = "ВСЕ ЮЛ";
                dt.Rows.InsertAt(rowToAdd, 0);
            }
            else if (cmbDepsRealiz.SelectedValue.ToString() == "0")
            {
                DataTable d = Config.hCntVVOKasReal.getLegalEntities(dtpDateStart.Value);
                dt = Config.hCntMainKasReal.getLegalEntities(dtpDateStart.Value);
                if (dt != null & d != null)
                    dt.Merge(d, true, MissingSchemaAction.Ignore);
            }
            else dt = Config.hCntMainKasReal.getLegalEntities(dtpDateStart.Value);
            if (dt == null) return;
            dt.DefaultView.Sort = "id ASC";
            cmbLegalEntitiesRealiz.ValueMember = "id";
            cmbLegalEntitiesRealiz.DisplayMember = "Abbriviation";
            cmbLegalEntitiesRealiz.DataSource = dt;
        }

        private void cmbDepsRealiz_SelectedValueChanged(object sender, EventArgs e)
        {
            cmbGroups_Load();
            cmbGroups.Enabled = cmbDepsRealiz.SelectedValue != null ? Convert.ToInt32(cmbDepsRealiz.SelectedValue.ToString()) != 0 ? true : false : false;
            ClearRealizGrid();
            cmbLegalEntitiesRealiz_Load();
        }

        private void txtEANRealiz_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckDigits(e);
        }

        private void txtNameRealiz_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (special_symbols.Contains(e.KeyChar) || e.KeyChar == '\'')
            {
                e.Handled = true;
            }
        }

        private void ClearRealizGrid()
        {
            dgvRealiz.DataSource = null;
            CountRealizSums();
        }

        private void CountRealizSums()
        {
            if (dgvRealiz.DataSource != null && dgvRealiz.RowCount > 0)
            {
                DataTable dt = (dgvRealiz.DataSource as DataTable).DefaultView.ToTable();
                txtNettoKgAll.Text = GetDecimalSumValue(dt, "sum(sum_count)", "len(ean) = 4").ToString("N3");
                txtSumKgAll.Text = GetDecimalSumValue(dt, "sum(sum_cash_val)", "len(ean) = 4").ToString("N2");
                txtNettoShtAll.Text = GetDecimalSumValue(dt, "sum(sum_count)", "len(ean) <> 4").ToString("N3");
                txtSumShtAll.Text = GetDecimalSumValue(dt, "sum(sum_cash_val)", "len(ean) <> 4").ToString("N2");
                txtSumAll.Text = GetDecimalSumValue(dt, "sum(sum_cash_val)", "").ToString("N2");

                txtRealiz.Text = (GetDecimalSumValue(dt, "sum(sum_cash_val)", "") / ((dtpDateEnd.Value.Date - dtpDateStart.Value.Date).Days + 1)).ToString("N2");
            }
            else
            {
                txtNettoKgAll.Text = txtSumKgAll.Text = txtNettoShtAll.Text = txtSumShtAll.Text = txtSumAll.Text = txtRealiz.Text = "";
            }
        }

        private void dtpDateStart_ValueChanged(object sender, EventArgs e)
        {
            ClearRealizGrid();
            cmbLegalEntitiesRealiz_Load();
        }

        private void dtpDateEnd_ValueChanged(object sender, EventArgs e)
        {
            ClearRealizGrid();
            cmbLegalEntitiesRealiz_Load();
        }

        private void cmbGroups_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearRealizGrid();
        }

        private void txtEANRealiz_TextChanged(object sender, EventArgs e)
        {
            dgvRealiz_Filter();
        }

        private void txtNameRealiz_TextChanged(object sender, EventArgs e)
        {
            dgvRealiz_Filter();
        }

        private void dgvRealiz_Filter()
        {
            if (dgvRealiz.DataSource != null)
            {
                string filter = "ean like '%" + txtEANRealiz.Text + "%' and cname like '%" + txtNameRealiz.Text + "%'";
                filter += cmbLegalEntitiesRealiz.SelectedValue.ToString() == "0" ? "" : $" AND legalEntity LIKE \'%{cmbLegalEntitiesRealiz.Text}%\'";
                if (cmbTerminalsRealiz.Enabled)
                {
                    filter += $" AND terminal = {cmbTerminalsRealiz.SelectedValue}";///Фильтрация по кассе 
                }
                (dgvRealiz.DataSource as DataTable).DefaultView.RowFilter = filter;
                CountRealizSums();
            }
        }

        private void bgw_LoadRealiz_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];
            DateTime date_start = Convert.ToDateTime(args[0]);
            DateTime date_end = Convert.ToDateTime(args[1]);
            int id_otdel = Convert.ToInt32(args[2]);
            int id_grp1 = Convert.ToInt32(args[3]);
            string listTerminal = args[4].ToString();
            bool isCashless = (bool)args[5];

            List<DataTable> data_for_months = new List<DataTable>();

            while (!(date_start.Month == date_end.Month || (date_start.Month == date_end.Month - 1 && date_end.Hour <= 3)))
            {
                DateTime date_end_current = new DateTime(date_start.Month == 12 ? date_start.Year + 1 : date_start.Year, date_start.Month == 12 ? 1 : date_start.Month + 1, 1).AddHours(3);
                data_for_months.Add(Config.hCntMainKasReal.GetRealizData(date_start, date_end_current, id_otdel, id_grp1, listTerminal));
                date_start = date_end_current.Date.AddHours(6);
            }

            //получение данных с двух коннектов
            if (cmbDepsRealizSelectedValue != "6" && cmbDepsRealizSelectedValue != "0")
                data_for_months.Add(Config.hCntMainKasReal.GetRealizData(date_start, date_end, id_otdel, id_grp1, listTerminal));
            else if (cmbDepsRealizSelectedValue == "6")
                data_for_months.Add(Config.hCntVVOKasReal.GetRealizData(date_start, date_end, id_otdel, id_grp1, listTerminal));
            else
            {
                DataTable dt = Config.hCntMainKasReal.GetRealizData(date_start, date_end, 0, id_grp1, listTerminal);
                DataTable dt2 = Config.hCntVVOKasReal.GetRealizData(date_start, date_end, 6, id_grp1, listTerminal);
                if (dt != null & dt2 != null)
                    dt.Merge(dt2, true, MissingSchemaAction.Ignore);
                data_for_months.Add(dt);
            }

            if (data_for_months.Count > 0)
            {
                if (data_for_months[0] == null) return;

                dtRealiz = data_for_months[0].Copy();
                if (!dtRealiz.Columns.Contains("ws"))
                {
                    DataColumn col = new DataColumn("ws", typeof(bool));
                    col.DefaultValue = false;
                    dtRealiz.Columns.Add(col);
                }
                for (int i = 1; i < data_for_months.Count; i++)
                {
                    DataTable dt = data_for_months[i];

                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow good_row = dtRealiz.AsEnumerable().FirstOrDefault(r => r["ean"].ToString() == row["ean"].ToString());
                        if (good_row == null)
                        {
                            dtRealiz.Rows.Add(row.ItemArray);
                        }
                        else
                        {
                            good_row["sum_count"] = Convert.ToDecimal(good_row["sum_count"]) + Convert.ToDecimal(row["sum_count"]);
                            good_row["sum_cash_val"] = Convert.ToDecimal(good_row["sum_cash_val"]) + Convert.ToDecimal(row["sum_cash_val"]);
                            good_row["clients_count"] = Convert.ToDecimal(good_row["clients_count"]) + Convert.ToDecimal(row["clients_count"]);
                            good_row["rcena"] = Convert.ToDecimal(row["rcena"]);
                        }
                    }
                }


                if (isCashless)
                {
                    DataTable dtTmp = Config.hCntMainKasReal.SelectWholeSales(date_start, date_end, id_otdel);
                    //MessageBox.Show(dtTmp.Rows.Count.ToString());
                    if (dtTmp != null && dtTmp.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtTmp.Rows)
                        {
                            dtRealiz.Rows.Add(row.ItemArray);
                        }
                    }
                }
            }
        }

        private void bgw_LoadRealiz_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgvRealiz.AutoGenerateColumns = false;
            dgvRealiz.DataSource = dtRealiz;
            dgvRealiz_Filter();
            printLogViewFormRealiz();


            frmWaiting.Close();
            EnableControlsService.RestoreControlEnabledState(this);
        }

        private void btnRefreshRealiz_Click(object sender, EventArgs e)
        {
            if (dtpDateStart.Value > dtpDateEnd.Value || dtpDateStart.Value == dtpDateEnd.Value && dtpTimeStartRealiz.Value > dtpTimeEndRealiz.Value)

            {
                MessageBox.Show("Не правильно указан период.\nДата начала больше даты конца.");
            }
            else
            {
                EnableControlsService.SaveControlsEnabledState(this);
                EnableControlsService.SetControlsEnabled(this, false);
                cmbDepsRealizSelectedValue = cmbDepsRealiz.SelectedValue.ToString();
                string listTerminal = "";
                if (Wholesale_checkBox.Checked)
                {
                    string number_list = Config.hCntMainKasReal.GetTillList();
                    listTerminal = number_list;
                }
                bool isCashless = Cashless_checkBox.Checked;

                frmWaiting = new frmLoad("Ждите, идёт загрузка данных...");
                frmWaiting.TopMost = true;
                frmWaiting.Show();
                bgw_LoadRealiz.RunWorkerAsync(new object[] { dtpDateStart.Value.Date.AddHours(dtpTimeStartRealiz.Value.Hour).AddMinutes(dtpTimeStartRealiz.Value.Minute),
                                                         dtpDateEnd.Value.Date.AddHours(dtpTimeEndRealiz.Value.Hour).AddMinutes(dtpTimeEndRealiz.Value.Minute),
                                                         Convert.ToInt32(cmbDepsRealiz.SelectedValue),
                                                         Convert.ToInt32(cmbGroups.SelectedValue),
                                                         listTerminal,
                                                         isCashless
                });
            }
        }

        private void dtpTimeStartRealiz_ValueChanged(object sender, EventArgs e)
        {
            ClearRealizGrid();
        }

        private void dtpTimeEndRealiz_ValueChanged(object sender, EventArgs e)
        {
            ClearRealizGrid();
        }

        private void btnClearFiltersRealiz_Click(object sender, EventArgs e)
        {
            ClearRealizFilters();
        }

        private void ClearRealizFilters()
        {
            dtpDateStart.Value = DateTime.Now.Date.AddDays(-1);
            dtpDateEnd.Value = DateTime.Now.Date;

            dtpTimeStartRealiz.Value = DateTime.Now.Date.AddHours(6);
            dtpTimeEndRealiz.Value = DateTime.Now.Date.AddHours(23).AddMinutes(59);
            if (UserSettings.User.StatusCode != "МН")
                cmbDepsRealiz.SelectedValue = 0;
            cmbGroups.SelectedValue = 0;

            txtEANRealiz.Text = txtNameRealiz.Text = "";

            CountRealizSums();
        }

        private void dgvRealiz_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvRealiz.CurrentRow != null)
            {
                txtCashPrice.Text = Convert.ToDecimal(dgvRealiz.CurrentRow.Cells["rcena"].Value).ToString("N2");
                if (Convert.ToDecimal(dgvRealiz.CurrentRow.Cells["count_realiz"].Value) != 0)
                    txtPrice.Text = (Convert.ToDecimal(dgvRealiz.CurrentRow.Cells["cash_val_realiz"].Value) / Convert.ToDecimal(dgvRealiz.CurrentRow.Cells["count_realiz"].Value)).ToString("N2");
                else
                    txtPrice.Text = "0";
            }
            else
            {
                txtCashPrice.Text = txtPrice.Text = "";
            }
        }

        private void btnPrintRealiz_Click(object sender, EventArgs e)
        {
            if (dgvRealiz.DataSource != null && (dgvRealiz.DataSource as DataTable).DefaultView.ToTable().Rows.Count > 0)
            {
                if (dtpDateStart.Value < dtpDateEnd.Value || dtpDateStart.Value == dtpDateEnd.Value && dtpTimeStartRealiz.Value < dtpTimeEndRealiz.Value)
                {
                    DataTable dt = (dgvRealiz.DataSource as DataTable).DefaultView.ToTable();
                    dt.Columns.Remove("rcena");

                    Reports.RealizReport.ShowRealizReport(cmbDepsRealiz.Text, cmbGroups.Text, dtpDateStart.Value.Date.AddHours(dtpTimeStartRealiz.Value.Hour).AddMinutes(dtpTimeStartRealiz.Value.Minute), dtpDateEnd.Value.Date.AddHours(dtpTimeEndRealiz.Value.Hour).AddMinutes(dtpTimeEndRealiz.Value.Minute), dt, new List<string>() { txtNettoKgAll.Text, txtSumKgAll.Text, txtNettoShtAll.Text, txtSumShtAll.Text, txtSumAll.Text });
                    LogPrintRealiz();
                }
                else
                {
                    MessageBox.Show("Не правильно указан период.\nДата начала больше даты конца.");
                }
            }
            else
            {
                MessageBox.Show("Нет данных для выгрузки!");
            }
        }

        private void LogPrintRealiz()
        {
            Logging.StartFirstLevel(1066);
            Logging.Comment("Выгружены данные с вкладки \"Реализация товаров\"");
            Logging.Comment("Период с " + dtpDateStart.Value.ToShortDateString() + " " + dtpTimeStartRealiz.Value.ToShortTimeString() + " по " + dtpDateEnd.Value.ToShortDateString() + " " + dtpTimeEndRealiz.Value.ToShortTimeString());
            Logging.Comment("Отдел = " + cmbDepsRealiz.Text + ", ТУ группа = " + cmbGroups.Text);
            Logging.Comment($"Id ЮЛ фильтра:{cmbLegalEntities.SelectedValue.ToString()} Аббривиатура:{cmbLegalEntities.Text} ");
            if (txtEANRealiz.Text.Length > 0)
                Logging.Comment($"Значение фильтра по EAN:{txtEANRealiz.Text}");
            if (txtNameRealiz.Text.Length > 0)
                Logging.Comment($"Значение фильтра по наименованию товара:{txtNameRealiz.Text}");
            Logging.Comment($"Значение чек-бокса «Только оптовые кассы»:{Wholesale_checkBox.Checked.ToString()}");
            Logging.Comment($"Значение чек-бокса «Оптовые безналичные отгрузки»:{Cashless_checkBox.Checked.ToString()}");
            Logging.StopFirstLevel();
        }

        private void btnExitRealiz_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCompareRealiz_Click(object sender, EventArgs e)
        {
            frmCompareRealiz frmCompare = new frmCompareRealiz();
            frmCompare.ShowDialog();
        }

        #endregion

        private decimal GetDecimalSumValue(DataTable dt, string sum, string filter)
        {
            object sum_value = dt.Compute(sum, filter);
            return sum_value == DBNull.Value ? 0 : Convert.ToDecimal(sum_value);
        }

        private void btnPrintDoc_Click(object sender, EventArgs e)
        {
            if (dgvCash.CurrentRow != null)
            {
                DataTable dt = Config.hCntMainKasReal.GetDocReport(dtpDateCash.Value.Date, Convert.ToInt32(dgvCash.CurrentRow.Cells["terminal"].Value), Convert.ToInt32(dgvCash.CurrentRow.Cells["doc_id"].Value));
                DataTable dt2 = Config.hCntVVOKasReal.GetDocReport(dtpDateCash.Value.Date, Convert.ToInt32(dgvCash.CurrentRow.Cells["terminal"].Value), Convert.ToInt32(dgvCash.CurrentRow.Cells["doc_id"].Value));
                if ((dt2 != null ? dt2.Rows.Count > 0 ? true : false : false) && dt == null)
                    dt = dt2;
                if (dt != null && dt2 != null)
                    dt.Merge(dt2, true, MissingSchemaAction.Ignore);

                if (!Reports.DocReport.Show(dgvCash.CurrentRow.Cells["terminal"].Value.ToString(), dgvCash.CurrentRow.Cells["kassir_name"].Value.ToString(),
                                       dgvCash.CurrentRow.Cells["doc_id"].Value.ToString(), dt, Application.StartupPath + "\\Templates\\doc_report", Application.StartupPath + "\\doc_report"))
                {
                    MessageBox.Show("Ошибка при выгрузке чека в Excel! Обратитесь в ОЭЭС!");
                }
                LogPrintDoc();
            }
        }

        private void LogPrintDoc()
        {
            Logging.StartFirstLevel(1067);
            Logging.Comment("Начало печати чека");
            Logging.Comment("Дата = " + dtpDateCash.Value.ToShortDateString() + ", отдел = " + cmbDepsCash.Text);
            Logging.Comment("Чек = " + dgvCash.CurrentRow.Cells["doc_id"].Value.ToString() + ", касса = " + dgvCash.CurrentRow.Cells["terminal"].Value.ToString() + ", кассир = " + dgvCash.CurrentRow.Cells["kassir_name"].Value.ToString());
            Logging.Comment($"Время открытия чека: {dgvCash.CurrentRow.Cells["time"].Value.ToString()}");
            Logging.Comment($"Время закрытия чека: {dgvCash.CurrentRow.Cells["time"].Value.ToString()}");
            Logging.Comment("Конец печати чека");
            Logging.StopFirstLevel();
        }

        private void btnPrintScan_Click(object sender, EventArgs e)
        {
            if (dgvRealiz.DataSource != null && (dgvRealiz.DataSource as DataTable).DefaultView.ToTable().Rows.Count > 0)
            {
                int pages = Reports.ScanReport.Show(Config.hCntMainKasReal.GetScanReport(dtpDateStart.Value.Date));
                LogPrintScan(pages);
            }
            else
            {
                MessageBox.Show("Нет данных для выгрузки!");
            }
        }

        private void LogPrintScan(int pages)
        {
            Logging.StartFirstLevel(1068);
            Logging.Comment("Выгружены данные для сканирования");
            Logging.Comment("Страниц в отчёте " + pages.ToString());
            Logging.StopFirstLevel();
        }

        private void CheckDigitsAndSeparator(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]))
            {
                e.Handled = true;
            }

            int count = ((TextBox)sender).Text.Length - (((TextBox)sender).Text.Replace(",", "").Length);
            if (e.KeyChar == ',' && count > 0)
            {
                e.Handled = true;
            }
        }

        private void txtCountStart_TextChanged(object sender, EventArgs e)
        {
            if (txtCountStart.Text != "" && txtCountStart.Text != "," && double.Parse(txtCountStart.Text) > Convert.ToDouble("999,999"))
                txtCountStart.Text = "999";

            int pos = txtCountStart.SelectionStart - 1;
            if (txtCountStart.Text.Length - txtCountStart.Text.IndexOf(',') > 4)
            {
                txtCountStart.Text = txtCountStart.Text.Remove(pos, 1);
                txtCountStart.SelectionStart = pos;
            }
            ClearGrid();
        }

        private void txtCountEnd_TextChanged(object sender, EventArgs e)
        {
            if (txtCountEnd.Text != "" && txtCountEnd.Text != "," && double.Parse(txtCountEnd.Text) > Convert.ToDouble("999,999"))
                txtCountEnd.Text = "999";

            int pos = txtCountEnd.SelectionStart - 1;
            if (txtCountEnd.Text.Length - txtCountEnd.Text.IndexOf(',') > 4)
            {
                txtCountEnd.Text = txtCountEnd.Text.Remove(pos, 1);
                txtCountEnd.SelectionStart = pos;
            }

            ClearGrid();
        }

        private void txtCountStart_Validating(object sender, CancelEventArgs e)
        {
            decimal d = 0;
            if (txtCountStart.Text.Length > 0 && !decimal.TryParse(txtCountStart.Text, out d))
            {
                e.Cancel = true;
            }
        }

        private void txtCountEnd_Validating(object sender, CancelEventArgs e)
        {
            decimal d = 0;
            if (txtCountEnd.Text.Length > 0 && !decimal.TryParse(txtCountEnd.Text, out d))
            {
                e.Cancel = true;
            }
        }

        private void dtpDateCash_CloseUp(object sender, EventArgs e)
        {
            ClearGrid();
            TerminalsAndKassirNamesLoad();

            dtp_cash = false;
        }

        private void dtpDateCash_DropDown(object sender, EventArgs e)
        {
            dtp_cash = true;
        }

        private void cbDayData_CheckedChanged(object sender, EventArgs e)
        {

            if (cbDayData.Checked)
            {
                dtpTimeStart.Enabled = false;
                dtpTimeEnd.Enabled = false;
            }
            else
            {
                dtpTimeStart.Enabled = true;
                dtpTimeEnd.Enabled = true;
            }
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String code = Nwuram.Framework.Settings.User.UserSettings.User.StatusCode;
            String fullCode = Nwuram.Framework.Settings.User.UserSettings.User.Status;
            if (Directory.Exists(Application.StartupPath + @"\HelpDoc"))
            {
                DirectoryInfo dir = new DirectoryInfo(Application.StartupPath + @"\HelpDoc");
                String[] fileList = Directory.GetFiles(Application.StartupPath + @"\HelpDoc");
                FileInfo[] files = dir.GetFiles("*" + fullCode + "*");
                if (files.Count() > 0)
                {
                    Process.Start(files[0].FullName);
                }
                else
                {
                    MessageBox.Show("Нет руководство пользователя", "Нет руководства", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void Wholesale_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            ClearRealizGrid();
        }

        private void Cashless_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            ClearRealizGrid();
        }

        private void dgvRealiz_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtRealiz != null && dtRealiz.Rows.Count > 0)
            {
                //DataGridViewRow dg = dgvRealiz.Rows[e.RowIndex];
                DataRowView drv = dtRealiz.DefaultView[e.RowIndex];
                if ((bool)drv["ws"])
                    e.CellStyle.BackColor = Color.FromArgb(254, 75, 75);
            }
        }

        private void cmbLegalEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvCash_Filter();
        }

        private void cmbKassir_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                string text = this.cmbKassir.GetItemText(cmbKassir.Items[e.Index]);
                e.DrawBackground();
                using (SolidBrush br = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(text, e.Font, br, e.Bounds);
                }

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    this.toolTip1.Show(text, cmbKassir, e.Bounds.Right, e.Bounds.Bottom);

                }
                else
                {
                    this.toolTip1.Hide(cmbKassir);
                }
                e.DrawFocusRectangle();
            }
            catch { }
        }

        private void cmbKassir_DropDownClosed(object sender, EventArgs e)
        {
            this.toolTip1.Hide(cmbKassir);
        }

        private void cmbKassir_Leave(object sender, EventArgs e)
        {
            this.toolTip1.Hide(cmbKassir);
        }

        private void cmbLegalEntitiesRealiz_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvRealiz_Filter();
        }

        private void cbTerminalRealiz_CheckedChanged(object sender, EventArgs e)
        {
            cmbTerminalsRealiz.Enabled = cbTerminalRealiz.Checked;
            dgvRealiz_Filter();
        }

        private void cmbTerminalsRealiz_SelectedValueChanged(object sender, EventArgs e)
        {
            dgvRealiz_Filter();
        }
    }
}
