namespace Client
{
    partial class MainForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.lbUsername = new System.Windows.Forms.Label();
            this.timerFetchData = new System.Windows.Forms.Timer(this.components);
            this.guna2TabControl1 = new Guna.UI2.WinForms.Guna2TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtTargetPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtShareCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnConnectCode = new Guna.UI2.WinForms.Guna2Button();
            this.guna2TabControl2 = new Guna.UI2.WinForms.Guna2TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ContextMenuAppList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEndTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.label24 = new System.Windows.Forms.Label();
            this.lblNetDown = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblPercentDisk = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lblPercentRam = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblPercentCPU = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblNetUp = new System.Windows.Forms.Label();
            this.progressBar4 = new System.Windows.Forms.ProgressBar();
            this.label9 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.lvEventLogs = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblip = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblMachineName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnRefreshList = new Guna.UI2.WinForms.Guna2Button();
            this.dgvClients = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colClientId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMachineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOwner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label23 = new System.Windows.Forms.Label();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.guna2TabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.guna2TabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.ContextMenuAppList.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(56, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(388, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Remote Monitor — Dashboard:";
            // 
            // lbUsername
            // 
            this.lbUsername.AutoSize = true;
            this.lbUsername.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lbUsername.ForeColor = System.Drawing.Color.White;
            this.lbUsername.Location = new System.Drawing.Point(468, 2);
            this.lbUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(113, 37);
            this.lbUsername.TabIndex = 1;
            this.lbUsername.Text = "Tên user";
            // 
            // timerFetchData
            // 
            this.timerFetchData.Enabled = true;
            this.timerFetchData.Interval = 1000;
            this.timerFetchData.Tick += new System.EventHandler(this.timerFetchData_Tick);
            // 
            // guna2TabControl1
            // 
            this.guna2TabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.guna2TabControl1.Controls.Add(this.tabPage2);
            this.guna2TabControl1.Controls.Add(this.tabPage1);
            this.guna2TabControl1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.guna2TabControl1.ItemSize = new System.Drawing.Size(180, 40);
            this.guna2TabControl1.Location = new System.Drawing.Point(8, 43);
            this.guna2TabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TabControl1.Name = "guna2TabControl1";
            this.guna2TabControl1.SelectedIndex = 0;
            this.guna2TabControl1.Size = new System.Drawing.Size(1644, 754);
            this.guna2TabControl1.TabButtonHoverState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TabControl1.TabButtonHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.guna2TabControl1.TabButtonHoverState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TabControl1.TabButtonHoverState.ForeColor = System.Drawing.Color.White;
            this.guna2TabControl1.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.guna2TabControl1.TabButtonIdleState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TabControl1.TabButtonIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2TabControl1.TabButtonIdleState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TabControl1.TabButtonIdleState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.guna2TabControl1.TabButtonIdleState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2TabControl1.TabButtonSelectedState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TabControl1.TabButtonSelectedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(37)))), ((int)(((byte)(49)))));
            this.guna2TabControl1.TabButtonSelectedState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TabControl1.TabButtonSelectedState.ForeColor = System.Drawing.Color.White;
            this.guna2TabControl1.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.guna2TabControl1.TabButtonSize = new System.Drawing.Size(180, 40);
            this.guna2TabControl1.TabIndex = 2;
            this.guna2TabControl1.TabMenuBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(20)))), ((int)(((byte)(33)))));
            this.tabPage2.Controls.Add(this.txtTargetPassword);
            this.tabPage2.Controls.Add(this.txtShareCode);
            this.tabPage2.Controls.Add(this.btnConnectCode);
            this.tabPage2.Controls.Add(this.guna2TabControl2);
            this.tabPage2.Controls.Add(this.lblip);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.lblMachineName);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.ForeColor = System.Drawing.Color.Black;
            this.tabPage2.Location = new System.Drawing.Point(184, 4);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1456, 746);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Theo dõi";
            // 
            // txtTargetPassword
            // 
            this.txtTargetPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTargetPassword.DefaultText = "";
            this.txtTargetPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTargetPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTargetPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTargetPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTargetPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTargetPassword.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtTargetPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTargetPassword.Location = new System.Drawing.Point(530, 7);
            this.txtTargetPassword.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.txtTargetPassword.Name = "txtTargetPassword";
            this.txtTargetPassword.PlaceholderText = "Mật khẩu";
            this.txtTargetPassword.SelectedText = "";
            this.txtTargetPassword.Size = new System.Drawing.Size(201, 41);
            this.txtTargetPassword.TabIndex = 31;
            // 
            // txtShareCode
            // 
            this.txtShareCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtShareCode.DefaultText = "";
            this.txtShareCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtShareCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtShareCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtShareCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtShareCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtShareCode.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtShareCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtShareCode.Location = new System.Drawing.Point(283, 7);
            this.txtShareCode.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.txtShareCode.Name = "txtShareCode";
            this.txtShareCode.PlaceholderText = "ID Đối tác";
            this.txtShareCode.SelectedText = "";
            this.txtShareCode.Size = new System.Drawing.Size(201, 41);
            this.txtShareCode.TabIndex = 30;
            // 
            // btnConnectCode
            // 
            this.btnConnectCode.BorderRadius = 15;
            this.btnConnectCode.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnConnectCode.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnConnectCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnConnectCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnConnectCode.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnConnectCode.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnConnectCode.ForeColor = System.Drawing.Color.White;
            this.btnConnectCode.Location = new System.Drawing.Point(15, 7);
            this.btnConnectCode.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnectCode.Name = "btnConnectCode";
            this.btnConnectCode.Size = new System.Drawing.Size(212, 41);
            this.btnConnectCode.TabIndex = 29;
            this.btnConnectCode.Text = "Xem Lịch Sử";
            this.btnConnectCode.Click += new System.EventHandler(this.btnConnectCode_Click);
            // 
            // guna2TabControl2
            // 
            this.guna2TabControl2.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.guna2TabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2TabControl2.Controls.Add(this.tabPage4);
            this.guna2TabControl2.Controls.Add(this.tabPage3);
            this.guna2TabControl2.Controls.Add(this.tabPage5);
            this.guna2TabControl2.ItemSize = new System.Drawing.Size(180, 40);
            this.guna2TabControl2.Location = new System.Drawing.Point(15, 87);
            this.guna2TabControl2.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TabControl2.Name = "guna2TabControl2";
            this.guna2TabControl2.SelectedIndex = 0;
            this.guna2TabControl2.Size = new System.Drawing.Size(1351, 651);
            this.guna2TabControl2.TabButtonHoverState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TabControl2.TabButtonHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.guna2TabControl2.TabButtonHoverState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TabControl2.TabButtonHoverState.ForeColor = System.Drawing.Color.White;
            this.guna2TabControl2.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.guna2TabControl2.TabButtonIdleState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TabControl2.TabButtonIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2TabControl2.TabButtonIdleState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TabControl2.TabButtonIdleState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.guna2TabControl2.TabButtonIdleState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2TabControl2.TabButtonSelectedState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TabControl2.TabButtonSelectedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(37)))), ((int)(((byte)(49)))));
            this.guna2TabControl2.TabButtonSelectedState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TabControl2.TabButtonSelectedState.ForeColor = System.Drawing.Color.White;
            this.guna2TabControl2.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.guna2TabControl2.TabButtonSize = new System.Drawing.Size(180, 40);
            this.guna2TabControl2.TabIndex = 10;
            this.guna2TabControl2.TabMenuBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.listView1);
            this.tabPage4.Location = new System.Drawing.Point(184, 4);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage4.Size = new System.Drawing.Size(1163, 643);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Danh sách ứng dụng";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BackColor = System.Drawing.SystemColors.Window;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.ContextMenuStrip = this.ContextMenuAppList;
            this.listView1.ForeColor = System.Drawing.Color.Black;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(4, 4);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1159, 635);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Process Name";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Window Title";
            this.columnHeader2.Width = 419;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Memory";
            this.columnHeader3.Width = 300;
            // 
            // ContextMenuAppList
            // 
            this.ContextMenuAppList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ContextMenuAppList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEndTaskToolStripMenuItem});
            this.ContextMenuAppList.Name = "contextMenuStrip1";
            this.ContextMenuAppList.Size = new System.Drawing.Size(131, 28);
            // 
            // menuEndTaskToolStripMenuItem
            // 
            this.menuEndTaskToolStripMenuItem.Name = "menuEndTaskToolStripMenuItem";
            this.menuEndTaskToolStripMenuItem.Size = new System.Drawing.Size(130, 24);
            this.menuEndTaskToolStripMenuItem.Text = "EndTask";
            this.menuEndTaskToolStripMenuItem.Click += new System.EventHandler(this.EndTaskToolStripMenuItem_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage3.Controls.Add(this.progressBar3);
            this.tabPage3.Controls.Add(this.label24);
            this.tabPage3.Controls.Add(this.lblNetDown);
            this.tabPage3.Controls.Add(this.chart1);
            this.tabPage3.Controls.Add(this.lblPercentDisk);
            this.tabPage3.Controls.Add(this.label26);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label25);
            this.tabPage3.Controls.Add(this.lblPercentRam);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.lblPercentCPU);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.lblNetUp);
            this.tabPage3.Controls.Add(this.progressBar4);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.progressBar1);
            this.tabPage3.Location = new System.Drawing.Point(184, 4);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage3.Size = new System.Drawing.Size(1163, 643);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Tài nguyên hệ thống";
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(91, 59);
            this.progressBar3.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(393, 28);
            this.progressBar3.TabIndex = 5;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label24.Location = new System.Drawing.Point(12, 183);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(259, 32);
            this.label24.TabIndex = 30;
            this.label24.Text = "Biểu đồ theo thời gian:";
            // 
            // lblNetDown
            // 
            this.lblNetDown.AutoSize = true;
            this.lblNetDown.ForeColor = System.Drawing.Color.White;
            this.lblNetDown.Location = new System.Drawing.Point(427, 135);
            this.lblNetDown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNetDown.Name = "lblNetDown";
            this.lblNetDown.Size = new System.Drawing.Size(0, 32);
            this.lblNetDown.TabIndex = 33;
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            chartArea3.AxisX.IsMarginVisible = false;
            chartArea3.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea3.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea3.AxisX.LineColor = System.Drawing.Color.White;
            chartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea3.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea3.AxisX.Title = "Thời gian";
            chartArea3.AxisX.TitleFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            chartArea3.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea3.AxisX2.TitleForeColor = System.Drawing.Color.White;
            chartArea3.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea3.AxisY.LineColor = System.Drawing.Color.WhiteSmoke;
            chartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea3.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea3.AxisY.Maximum = 100D;
            chartArea3.AxisY.Minimum = 0D;
            chartArea3.AxisY.TitleFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            chartArea3.AxisY.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            chartArea3.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            chartArea3.BorderColor = System.Drawing.Color.Transparent;
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            legend3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            legend3.ForeColor = System.Drawing.Color.White;
            legend3.IsTextAutoFit = false;
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(19, 233);
            this.chart1.Margin = new System.Windows.Forms.Padding(4);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series5.BackImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            series5.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            series5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            series5.BorderWidth = 3;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series5.Color = System.Drawing.Color.PaleTurquoise;
            series5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            series5.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            series5.Legend = "Legend1";
            series5.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            series5.Name = "CPU";
            series5.ShadowColor = System.Drawing.Color.LightGray;
            series6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            series6.BorderWidth = 3;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series6.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            series6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            series6.LabelBackColor = System.Drawing.Color.Transparent;
            series6.Legend = "Legend1";
            series6.Name = "RAM";
            this.chart1.Series.Add(series5);
            this.chart1.Series.Add(series6);
            this.chart1.Size = new System.Drawing.Size(1052, 390);
            this.chart1.TabIndex = 29;
            this.chart1.Text = "chart1";
            // 
            // lblPercentDisk
            // 
            this.lblPercentDisk.AutoSize = true;
            this.lblPercentDisk.ForeColor = System.Drawing.Color.White;
            this.lblPercentDisk.Location = new System.Drawing.Point(505, 95);
            this.lblPercentDisk.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPercentDisk.Name = "lblPercentDisk";
            this.lblPercentDisk.Size = new System.Drawing.Size(0, 32);
            this.lblPercentDisk.TabIndex = 9;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label26.Location = new System.Drawing.Point(305, 135);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(109, 32);
            this.label26.TabIndex = 32;
            this.label26.Text = "Recieved";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label12.Location = new System.Drawing.Point(12, 21);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 32);
            this.label12.TabIndex = 1;
            this.label12.Text = "CPU:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label25.Location = new System.Drawing.Point(97, 135);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(68, 32);
            this.label25.TabIndex = 31;
            this.label25.Text = "Send";
            // 
            // lblPercentRam
            // 
            this.lblPercentRam.AutoSize = true;
            this.lblPercentRam.ForeColor = System.Drawing.Color.White;
            this.lblPercentRam.Location = new System.Drawing.Point(505, 59);
            this.lblPercentRam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPercentRam.Name = "lblPercentRam";
            this.lblPercentRam.Size = new System.Drawing.Size(0, 32);
            this.lblPercentRam.TabIndex = 8;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label13.Location = new System.Drawing.Point(12, 57);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 32);
            this.label13.TabIndex = 2;
            this.label13.Text = "RAM:";
            // 
            // lblPercentCPU
            // 
            this.lblPercentCPU.AutoSize = true;
            this.lblPercentCPU.ForeColor = System.Drawing.Color.White;
            this.lblPercentCPU.Location = new System.Drawing.Point(505, 21);
            this.lblPercentCPU.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPercentCPU.Name = "lblPercentCPU";
            this.lblPercentCPU.Size = new System.Drawing.Size(0, 32);
            this.lblPercentCPU.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label14.Location = new System.Drawing.Point(12, 92);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 32);
            this.label14.TabIndex = 3;
            this.label14.Text = "DISK:";
            // 
            // lblNetUp
            // 
            this.lblNetUp.AutoSize = true;
            this.lblNetUp.ForeColor = System.Drawing.Color.White;
            this.lblNetUp.Location = new System.Drawing.Point(177, 135);
            this.lblNetUp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNetUp.Name = "lblNetUp";
            this.lblNetUp.Size = new System.Drawing.Size(0, 32);
            this.lblNetUp.TabIndex = 27;
            // 
            // progressBar4
            // 
            this.progressBar4.Location = new System.Drawing.Point(91, 95);
            this.progressBar4.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar4.Name = "progressBar4";
            this.progressBar4.Size = new System.Drawing.Size(393, 28);
            this.progressBar4.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label9.Location = new System.Drawing.Point(12, 135);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 32);
            this.label9.TabIndex = 26;
            this.label9.Text = "Mạng:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(91, 23);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(393, 28);
            this.progressBar1.TabIndex = 4;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.tabPage5.Controls.Add(this.lvEventLogs);
            this.tabPage5.Location = new System.Drawing.Point(184, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1163, 643);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Nhật ký hệ thống";
            // 
            // lvEventLogs
            // 
            this.lvEventLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvEventLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lvEventLogs.FullRowSelect = true;
            this.lvEventLogs.GridLines = true;
            this.lvEventLogs.HideSelection = false;
            this.lvEventLogs.Location = new System.Drawing.Point(3, 3);
            this.lvEventLogs.Name = "lvEventLogs";
            this.lvEventLogs.Size = new System.Drawing.Size(1157, 644);
            this.lvEventLogs.TabIndex = 0;
            this.lvEventLogs.UseCompatibleStateImageBehavior = false;
            this.lvEventLogs.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Phân loại";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Nguồn";
            this.columnHeader5.Width = 150;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Thời gian hoạt động";
            this.columnHeader6.Width = 120;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Nội dung hoạt động";
            this.columnHeader7.Width = 430;
            // 
            // lblip
            // 
            this.lblip.AutoSize = true;
            this.lblip.ForeColor = System.Drawing.Color.White;
            this.lblip.Location = new System.Drawing.Point(660, 52);
            this.lblip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblip.Name = "lblip";
            this.lblip.Size = new System.Drawing.Size(0, 35);
            this.lblip.TabIndex = 24;
            this.lblip.UseCompatibleTextRendering = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label6.Location = new System.Drawing.Point(613, 52);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 32);
            this.label6.TabIndex = 23;
            this.label6.Text = "IP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label5.Location = new System.Drawing.Point(593, 52);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 32);
            this.label5.TabIndex = 22;
            this.label5.Text = "|";
            // 
            // lblMachineName
            // 
            this.lblMachineName.AutoSize = true;
            this.lblMachineName.ForeColor = System.Drawing.Color.White;
            this.lblMachineName.Location = new System.Drawing.Point(257, 52);
            this.lblMachineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMachineName.Name = "lblMachineName";
            this.lblMachineName.Size = new System.Drawing.Size(0, 32);
            this.lblMachineName.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label3.Location = new System.Drawing.Point(136, 52);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 32);
            this.label3.TabIndex = 20;
            this.label3.Text = "Tên máy:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(197)))), ((int)(((byte)(229)))));
            this.label2.Location = new System.Drawing.Point(8, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 32);
            this.label2.TabIndex = 19;
            this.label2.Text = "Thông tin:";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(20)))), ((int)(((byte)(33)))));
            this.tabPage1.Controls.Add(this.btnRefreshList);
            this.tabPage1.Controls.Add(this.dgvClients);
            this.tabPage1.Controls.Add(this.label23);
            this.tabPage1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabPage1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.tabPage1.Location = new System.Drawing.Point(184, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1456, 746);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Admin";
            // 
            // btnRefreshList
            // 
            this.btnRefreshList.BorderRadius = 15;
            this.btnRefreshList.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRefreshList.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRefreshList.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRefreshList.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRefreshList.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnRefreshList.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnRefreshList.ForeColor = System.Drawing.Color.White;
            this.btnRefreshList.Location = new System.Drawing.Point(8, 7);
            this.btnRefreshList.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefreshList.Name = "btnRefreshList";
            this.btnRefreshList.Size = new System.Drawing.Size(273, 68);
            this.btnRefreshList.TabIndex = 21;
            this.btnRefreshList.Text = "Làm mới danh sách";
            this.btnRefreshList.Click += new System.EventHandler(this.btnRefreshList_Click);
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            this.dgvClients.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvClients.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvClients.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvClients.ColumnHeadersHeight = 30;
            this.dgvClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colClientId,
            this.colMachineName,
            this.colOwner,
            this.colStatus});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvClients.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvClients.GridColor = System.Drawing.Color.Black;
            this.dgvClients.Location = new System.Drawing.Point(19, 143);
            this.dgvClients.Margin = new System.Windows.Forms.Padding(4);
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.ReadOnly = true;
            this.dgvClients.RowHeadersVisible = false;
            this.dgvClients.RowHeadersWidth = 51;
            this.dgvClients.Size = new System.Drawing.Size(1367, 594);
            this.dgvClients.TabIndex = 20;
            this.dgvClients.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvClients.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvClients.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvClients.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvClients.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvClients.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvClients.ThemeStyle.GridColor = System.Drawing.Color.Black;
            this.dgvClients.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvClients.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
            this.dgvClients.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvClients.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvClients.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvClients.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvClients.ThemeStyle.ReadOnly = true;
            this.dgvClients.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvClients.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvClients.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvClients.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.dgvClients.ThemeStyle.RowsStyle.Height = 22;
            this.dgvClients.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvClients.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvClients.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClients_CellClick);
            // 
            // colClientId
            // 
            this.colClientId.HeaderText = "ShareCode";
            this.colClientId.MinimumWidth = 6;
            this.colClientId.Name = "colClientId";
            this.colClientId.ReadOnly = true;
            // 
            // colMachineName
            // 
            this.colMachineName.HeaderText = "MachineName";
            this.colMachineName.MinimumWidth = 6;
            this.colMachineName.Name = "colMachineName";
            this.colMachineName.ReadOnly = true;
            // 
            // colOwner
            // 
            this.colOwner.HeaderText = "Username";
            this.colOwner.MinimumWidth = 6;
            this.colOwner.Name = "colOwner";
            this.colOwner.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.label23.Location = new System.Drawing.Point(4, 90);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(271, 32);
            this.label23.TabIndex = 19;
            this.label23.Text = "--- Danh Sách Client ---";
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox1.Image")));
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(8, 6);
            this.guna2PictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(40, 33);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox1.TabIndex = 3;
            this.guna2PictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(1643, 800);
            this.Controls.Add(this.guna2PictureBox1);
            this.Controls.Add(this.guna2TabControl1);
            this.Controls.Add(this.lbUsername);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.guna2TabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.guna2TabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ContextMenuAppList.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbUsername;
        private System.Windows.Forms.Timer timerFetchData;
        private Guna.UI2.WinForms.Guna2TabControl guna2TabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TabPage tabPage2;
        private Guna.UI2.WinForms.Guna2DataGridView dgvClients;
        private Guna.UI2.WinForms.Guna2Button btnRefreshList;
        private System.Windows.Forms.Label lblNetDown;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label lblNetUp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblip;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblMachineName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TabControl guna2TabControl2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.Label lblPercentDisk;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblPercentRam;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblPercentCPU;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ProgressBar progressBar4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2Button btnConnectCode;
        private System.Windows.Forms.ContextMenuStrip ContextMenuAppList;
        private System.Windows.Forms.ToolStripMenuItem menuEndTaskToolStripMenuItem;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private Guna.UI2.WinForms.Guna2TextBox txtShareCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClientId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMachineName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOwner;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ListView lvEventLogs;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private Guna.UI2.WinForms.Guna2TextBox txtTargetPassword;
    }
}