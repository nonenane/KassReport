namespace CashReportNew
{
    partial class frmCompareRealiz
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbEqualTime = new System.Windows.Forms.CheckBox();
            this.dtpTimeEnd1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpTimeStart1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtpTimeEnd2 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpTimeStart2 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpDate2 = new System.Windows.Forms.DateTimePicker();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbInvGroups = new System.Windows.Forms.ComboBox();
            this.cmbTUGroups = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbDepartments = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvRealiz = new System.Windows.Forms.DataGridView();
            this.id_tovar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ean = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.count1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.count2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.diff_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sum1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sum2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.diff_sum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_otdel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_grp1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_grp2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtEAN = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCount1 = new System.Windows.Forms.TextBox();
            this.txtCount2 = new System.Windows.Forms.TextBox();
            this.txtDiffCount = new System.Windows.Forms.TextBox();
            this.txtDiffSum = new System.Windows.Forms.TextBox();
            this.txtSum2 = new System.Windows.Forms.TextBox();
            this.txtSum1 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.bgw_Load = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRealiz)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbEqualTime);
            this.groupBox1.Controls.Add(this.dtpTimeEnd1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpTimeStart1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpDate1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 56);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cbEqualTime
            // 
            this.cbEqualTime.AutoSize = true;
            this.cbEqualTime.Location = new System.Drawing.Point(391, 26);
            this.cbEqualTime.Name = "cbEqualTime";
            this.cbEqualTime.Size = new System.Drawing.Size(85, 17);
            this.cbEqualTime.TabIndex = 1;
            this.cbEqualTime.Text = "одно время";
            this.cbEqualTime.UseVisualStyleBackColor = true;
            this.cbEqualTime.CheckedChanged += new System.EventHandler(this.cbEqualTime_CheckedChanged);
            // 
            // dtpTimeEnd1
            // 
            this.dtpTimeEnd1.CustomFormat = "HH:mm";
            this.dtpTimeEnd1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeEnd1.Location = new System.Drawing.Point(324, 23);
            this.dtpTimeEnd1.Name = "dtpTimeEnd1";
            this.dtpTimeEnd1.ShowUpDown = true;
            this.dtpTimeEnd1.Size = new System.Drawing.Size(51, 20);
            this.dtpTimeEnd1.TabIndex = 3;
            this.dtpTimeEnd1.ValueChanged += new System.EventHandler(this.dtpTimeEnd1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(296, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "по:";
            // 
            // dtpTimeStart1
            // 
            this.dtpTimeStart1.CustomFormat = "HH:mm";
            this.dtpTimeStart1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeStart1.Location = new System.Drawing.Point(228, 23);
            this.dtpTimeStart1.Name = "dtpTimeStart1";
            this.dtpTimeStart1.ShowUpDown = true;
            this.dtpTimeStart1.Size = new System.Drawing.Size(51, 20);
            this.dtpTimeStart1.TabIndex = 2;
            this.dtpTimeStart1.ValueChanged += new System.EventHandler(this.dtpTimeStart1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Время с:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Дата 1:";
            // 
            // dtpDate1
            // 
            this.dtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate1.Location = new System.Drawing.Point(57, 23);
            this.dtpDate1.Name = "dtpDate1";
            this.dtpDate1.Size = new System.Drawing.Size(91, 20);
            this.dtpDate1.TabIndex = 1;
            this.dtpDate1.ValueChanged += new System.EventHandler(this.dtpDate1_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpTimeEnd2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dtpTimeStart2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.dtpDate2);
            this.groupBox2.Location = new System.Drawing.Point(495, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(388, 56);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // dtpTimeEnd2
            // 
            this.dtpTimeEnd2.CustomFormat = "HH:mm";
            this.dtpTimeEnd2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeEnd2.Location = new System.Drawing.Point(324, 23);
            this.dtpTimeEnd2.Name = "dtpTimeEnd2";
            this.dtpTimeEnd2.ShowUpDown = true;
            this.dtpTimeEnd2.Size = new System.Drawing.Size(51, 20);
            this.dtpTimeEnd2.TabIndex = 3;
            this.dtpTimeEnd2.ValueChanged += new System.EventHandler(this.dtpTimeEnd2_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(296, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "по:";
            // 
            // dtpTimeStart2
            // 
            this.dtpTimeStart2.CustomFormat = "HH:mm";
            this.dtpTimeStart2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeStart2.Location = new System.Drawing.Point(228, 23);
            this.dtpTimeStart2.Name = "dtpTimeStart2";
            this.dtpTimeStart2.ShowUpDown = true;
            this.dtpTimeStart2.Size = new System.Drawing.Size(51, 20);
            this.dtpTimeStart2.TabIndex = 2;
            this.dtpTimeStart2.ValueChanged += new System.EventHandler(this.dtpTimeStart2_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(170, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Время с:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Дата 2:";
            // 
            // dtpDate2
            // 
            this.dtpDate2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate2.Location = new System.Drawing.Point(57, 23);
            this.dtpDate2.Name = "dtpDate2";
            this.dtpDate2.Size = new System.Drawing.Size(91, 20);
            this.dtpDate2.TabIndex = 1;
            this.dtpDate2.ValueChanged += new System.EventHandler(this.dtpDate2_ValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.cmbInvGroups);
            this.groupBox3.Controls.Add(this.cmbTUGroups);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.cmbDepartments);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(12, 74);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(589, 56);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(373, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Инв. группа:";
            // 
            // cmbInvGroups
            // 
            this.cmbInvGroups.DisplayMember = "grp2_name";
            this.cmbInvGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInvGroups.FormattingEnabled = true;
            this.cmbInvGroups.Location = new System.Drawing.Point(449, 24);
            this.cmbInvGroups.Name = "cmbInvGroups";
            this.cmbInvGroups.Size = new System.Drawing.Size(121, 21);
            this.cmbInvGroups.TabIndex = 6;
            this.cmbInvGroups.ValueMember = "id";
            this.cmbInvGroups.SelectedValueChanged += new System.EventHandler(this.cmbInvGroups_SelectedValueChanged);
            // 
            // cmbTUGroups
            // 
            this.cmbTUGroups.DisplayMember = "grp1_name";
            this.cmbTUGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTUGroups.FormattingEnabled = true;
            this.cmbTUGroups.Location = new System.Drawing.Point(246, 24);
            this.cmbTUGroups.Name = "cmbTUGroups";
            this.cmbTUGroups.Size = new System.Drawing.Size(121, 21);
            this.cmbTUGroups.TabIndex = 6;
            this.cmbTUGroups.ValueMember = "id";
            this.cmbTUGroups.SelectedValueChanged += new System.EventHandler(this.cmbTUGroups_SelectedValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(181, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "ТУ группа:";
            // 
            // cmbDepartments
            // 
            this.cmbDepartments.DisplayMember = "dep_name";
            this.cmbDepartments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartments.FormattingEnabled = true;
            this.cmbDepartments.Location = new System.Drawing.Point(53, 24);
            this.cmbDepartments.Name = "cmbDepartments";
            this.cmbDepartments.Size = new System.Drawing.Size(121, 21);
            this.cmbDepartments.TabIndex = 5;
            this.cmbDepartments.ValueMember = "id";
            this.cmbDepartments.SelectedValueChanged += new System.EventHandler(this.cmbDepartments_SelectedValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Отдел:";
            // 
            // dgvRealiz
            // 
            this.dgvRealiz.AllowUserToAddRows = false;
            this.dgvRealiz.AllowUserToDeleteRows = false;
            this.dgvRealiz.AllowUserToResizeRows = false;
            this.dgvRealiz.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRealiz.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRealiz.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRealiz.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRealiz.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRealiz.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_tovar,
            this.ean,
            this.cname,
            this.count1,
            this.count2,
            this.diff_count,
            this.sum1,
            this.sum2,
            this.diff_sum,
            this.id_otdel,
            this.id_grp1,
            this.id_grp2});
            this.dgvRealiz.Location = new System.Drawing.Point(12, 162);
            this.dgvRealiz.Name = "dgvRealiz";
            this.dgvRealiz.ReadOnly = true;
            this.dgvRealiz.RowHeadersVisible = false;
            this.dgvRealiz.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRealiz.Size = new System.Drawing.Size(871, 381);
            this.dgvRealiz.TabIndex = 3;
            // 
            // id_tovar
            // 
            this.id_tovar.DataPropertyName = "id_tovar";
            this.id_tovar.HeaderText = "id_tovar";
            this.id_tovar.Name = "id_tovar";
            this.id_tovar.ReadOnly = true;
            this.id_tovar.Visible = false;
            // 
            // ean
            // 
            this.ean.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ean.DataPropertyName = "ean";
            this.ean.HeaderText = "EAN";
            this.ean.Name = "ean";
            this.ean.ReadOnly = true;
            this.ean.Width = 140;
            // 
            // cname
            // 
            this.cname.DataPropertyName = "cname";
            this.cname.HeaderText = "Наименование";
            this.cname.Name = "cname";
            this.cname.ReadOnly = true;
            // 
            // count1
            // 
            this.count1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.count1.DataPropertyName = "count1";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.count1.DefaultCellStyle = dataGridViewCellStyle2;
            this.count1.HeaderText = "Кол-во 1";
            this.count1.Name = "count1";
            this.count1.ReadOnly = true;
            this.count1.Width = 75;
            // 
            // count2
            // 
            this.count2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.count2.DataPropertyName = "count2";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.count2.DefaultCellStyle = dataGridViewCellStyle3;
            this.count2.HeaderText = "Кол-во 2";
            this.count2.Name = "count2";
            this.count2.ReadOnly = true;
            this.count2.Width = 75;
            // 
            // diff_count
            // 
            this.diff_count.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.diff_count.DataPropertyName = "diff_count";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.diff_count.DefaultCellStyle = dataGridViewCellStyle4;
            this.diff_count.HeaderText = "Разница";
            this.diff_count.Name = "diff_count";
            this.diff_count.ReadOnly = true;
            this.diff_count.Width = 80;
            // 
            // sum1
            // 
            this.sum1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.sum1.DataPropertyName = "sum1";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.sum1.DefaultCellStyle = dataGridViewCellStyle5;
            this.sum1.HeaderText = "Сумма 1 ";
            this.sum1.Name = "sum1";
            this.sum1.ReadOnly = true;
            this.sum1.Width = 75;
            // 
            // sum2
            // 
            this.sum2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.sum2.DataPropertyName = "sum2";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            this.sum2.DefaultCellStyle = dataGridViewCellStyle6;
            this.sum2.HeaderText = "Сумма 2";
            this.sum2.Name = "sum2";
            this.sum2.ReadOnly = true;
            this.sum2.Width = 75;
            // 
            // diff_sum
            // 
            this.diff_sum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.diff_sum.DataPropertyName = "diff_sum";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = null;
            this.diff_sum.DefaultCellStyle = dataGridViewCellStyle7;
            this.diff_sum.HeaderText = "Разница";
            this.diff_sum.Name = "diff_sum";
            this.diff_sum.ReadOnly = true;
            this.diff_sum.Width = 80;
            // 
            // id_otdel
            // 
            this.id_otdel.DataPropertyName = "id_otdel";
            this.id_otdel.HeaderText = "id_otdel";
            this.id_otdel.Name = "id_otdel";
            this.id_otdel.ReadOnly = true;
            this.id_otdel.Visible = false;
            // 
            // id_grp1
            // 
            this.id_grp1.DataPropertyName = "id_grp1";
            this.id_grp1.HeaderText = "id_grp1";
            this.id_grp1.Name = "id_grp1";
            this.id_grp1.ReadOnly = true;
            this.id_grp1.Visible = false;
            // 
            // id_grp2
            // 
            this.id_grp2.DataPropertyName = "id_grp2";
            this.id_grp2.HeaderText = "id_grp2";
            this.id_grp2.Name = "id_grp2";
            this.id_grp2.ReadOnly = true;
            this.id_grp2.Visible = false;
            // 
            // txtEAN
            // 
            this.txtEAN.Location = new System.Drawing.Point(12, 136);
            this.txtEAN.Name = "txtEAN";
            this.txtEAN.Size = new System.Drawing.Size(116, 20);
            this.txtEAN.TabIndex = 4;
            this.toolTips.SetToolTip(this.txtEAN, "Поиск по EAN");
            this.txtEAN.TextChanged += new System.EventHandler(this.txtEAN_TextChanged);
            this.txtEAN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEAN_KeyPress);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(134, 136);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(135, 20);
            this.txtName.TabIndex = 5;
            this.toolTips.SetToolTip(this.txtName, "Поиск по наименованию");
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 552);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Итого кол-во 1:";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 583);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Итого кол-во 2:";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.Location = new System.Drawing.Point(12, 607);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 31);
            this.label12.TabIndex = 8;
            this.label12.Text = "Итого разница по кол-ву:";
            // 
            // txtCount1
            // 
            this.txtCount1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCount1.BackColor = System.Drawing.Color.White;
            this.txtCount1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtCount1.Location = new System.Drawing.Point(103, 549);
            this.txtCount1.Name = "txtCount1";
            this.txtCount1.ReadOnly = true;
            this.txtCount1.Size = new System.Drawing.Size(100, 20);
            this.txtCount1.TabIndex = 9;
            this.txtCount1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCount2
            // 
            this.txtCount2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCount2.BackColor = System.Drawing.Color.White;
            this.txtCount2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtCount2.Location = new System.Drawing.Point(103, 580);
            this.txtCount2.Name = "txtCount2";
            this.txtCount2.ReadOnly = true;
            this.txtCount2.Size = new System.Drawing.Size(100, 20);
            this.txtCount2.TabIndex = 10;
            this.txtCount2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDiffCount
            // 
            this.txtDiffCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtDiffCount.BackColor = System.Drawing.Color.White;
            this.txtDiffCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtDiffCount.Location = new System.Drawing.Point(103, 611);
            this.txtDiffCount.Name = "txtDiffCount";
            this.txtDiffCount.ReadOnly = true;
            this.txtDiffCount.Size = new System.Drawing.Size(100, 20);
            this.txtDiffCount.TabIndex = 11;
            this.txtDiffCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDiffSum
            // 
            this.txtDiffSum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtDiffSum.BackColor = System.Drawing.Color.White;
            this.txtDiffSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtDiffSum.Location = new System.Drawing.Point(300, 611);
            this.txtDiffSum.Name = "txtDiffSum";
            this.txtDiffSum.ReadOnly = true;
            this.txtDiffSum.Size = new System.Drawing.Size(100, 20);
            this.txtDiffSum.TabIndex = 17;
            this.txtDiffSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSum2
            // 
            this.txtSum2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSum2.BackColor = System.Drawing.Color.White;
            this.txtSum2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtSum2.Location = new System.Drawing.Point(300, 580);
            this.txtSum2.Name = "txtSum2";
            this.txtSum2.ReadOnly = true;
            this.txtSum2.Size = new System.Drawing.Size(100, 20);
            this.txtSum2.TabIndex = 16;
            this.txtSum2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSum1
            // 
            this.txtSum1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSum1.BackColor = System.Drawing.Color.White;
            this.txtSum1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtSum1.Location = new System.Drawing.Point(300, 549);
            this.txtSum1.Name = "txtSum1";
            this.txtSum1.ReadOnly = true;
            this.txtSum1.Size = new System.Drawing.Size(100, 20);
            this.txtSum1.TabIndex = 15;
            this.txtSum1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.Location = new System.Drawing.Point(209, 607);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 31);
            this.label13.TabIndex = 14;
            this.label13.Text = "Итого разница по сумме:";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(209, 583);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 13);
            this.label14.TabIndex = 13;
            this.label14.Text = "Итого сумма 2:";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(209, 552);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(85, 13);
            this.label15.TabIndex = 12;
            this.label15.Text = "Итого сумма 1:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Image = global::CashReportNew.Properties.Resources.pict_refresh;
            this.btnRefresh.Location = new System.Drawing.Point(813, 604);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(32, 32);
            this.btnRefresh.TabIndex = 19;
            this.toolTips.SetToolTip(this.btnRefresh, "Обновить");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnExit.Image = global::CashReportNew.Properties.Resources.pict_close;
            this.btnExit.Location = new System.Drawing.Point(851, 604);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(32, 32);
            this.btnExit.TabIndex = 18;
            this.toolTips.SetToolTip(this.btnExit, "Выход");
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Image = global::CashReportNew.Properties.Resources.WZPRINT;
            this.btnPrint.Location = new System.Drawing.Point(775, 604);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(32, 32);
            this.btnPrint.TabIndex = 20;
            this.toolTips.SetToolTip(this.btnPrint, "Печать");
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClearFilters
            // 
            this.btnClearFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFilters.Image = global::CashReportNew.Properties.Resources._8;
            this.btnClearFilters.Location = new System.Drawing.Point(851, 74);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(32, 32);
            this.btnClearFilters.TabIndex = 21;
            this.toolTips.SetToolTip(this.btnClearFilters, "Очистить фильтры");
            this.btnClearFilters.UseVisualStyleBackColor = true;
            this.btnClearFilters.Click += new System.EventHandler(this.btnClearFilters_Click);
            // 
            // bgw_Load
            // 
            this.bgw_Load.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_Load_DoWork);
            this.bgw_Load.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgw_Load_RunWorkerCompleted);
            // 
            // frmCompareRealiz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 643);
            this.Controls.Add(this.btnClearFilters);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtDiffSum);
            this.Controls.Add(this.txtSum2);
            this.Controls.Add(this.txtSum1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtDiffCount);
            this.Controls.Add(this.txtCount2);
            this.Controls.Add(this.txtCount1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtEAN);
            this.Controls.Add(this.dgvRealiz);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmCompareRealiz";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сравнение реализации";
            this.Load += new System.EventHandler(this.frmCompareRealiz_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRealiz)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbEqualTime;
        private System.Windows.Forms.DateTimePicker dtpTimeEnd1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpTimeStart1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDate1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpTimeEnd2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTimeStart2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpDate2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbInvGroups;
        private System.Windows.Forms.ComboBox cmbTUGroups;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbDepartments;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvRealiz;
        private System.Windows.Forms.TextBox txtEAN;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCount1;
        private System.Windows.Forms.TextBox txtCount2;
        private System.Windows.Forms.TextBox txtDiffCount;
        private System.Windows.Forms.TextBox txtDiffSum;
        private System.Windows.Forms.TextBox txtSum2;
        private System.Windows.Forms.TextBox txtSum1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.ToolTip toolTips;
        private System.ComponentModel.BackgroundWorker bgw_Load;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_tovar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ean;
        private System.Windows.Forms.DataGridViewTextBoxColumn cname;
        private System.Windows.Forms.DataGridViewTextBoxColumn count1;
        private System.Windows.Forms.DataGridViewTextBoxColumn count2;
        private System.Windows.Forms.DataGridViewTextBoxColumn diff_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn sum1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sum2;
        private System.Windows.Forms.DataGridViewTextBoxColumn diff_sum;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_otdel;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_grp1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_grp2;
        private System.Windows.Forms.Button btnClearFilters;
    }
}