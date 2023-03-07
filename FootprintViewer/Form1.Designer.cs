namespace FootprintViewer
{
	partial class Form1
	{
		/// <summary>
		/// 設計工具所需的變數。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清除任何使用中的資源。
		/// </summary>
		/// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 設計工具產生的程式碼

		/// <summary>
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tbTimestamp = new System.Windows.Forms.TrackBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpSetting = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rtxtLog = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSelectLogFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMapFile = new System.Windows.Forms.TextBox();
            this.txtLogFile = new System.Windows.Forms.TextBox();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.cbStart = new System.Windows.Forms.ComboBox();
            this.cbEnd = new System.Windows.Forms.ComboBox();
            this.btnSelectMapFile = new System.Windows.Forms.Button();
            this.btnLoadSetting = new System.Windows.Forms.Button();
            this.btnRecovery = new System.Windows.Forms.Button();
            this.tpFootprint = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gluiCtrl1 = new GLUI.GLUICtrl();
            this.dgvVehicleInfo = new System.Windows.Forms.DataGridView();
            this.cmenuDgvVehicleInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemCopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPlayHistoryvideo = new System.Windows.Forms.Button();
            this.lblCurrentTimestamp = new System.Windows.Forms.Label();
            this.tpLog = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLoadLogTable = new System.Windows.Forms.Button();
            this.cbLogTableName = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvLogTable = new System.Windows.Forms.DataGridView();
            this.btnPauseHistoryvideo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbTimestamp)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpSetting.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tpFootprint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleInfo)).BeginInit();
            this.cmenuDgvVehicleInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpLog.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogTable)).BeginInit();
            this.SuspendLayout();
            // 
            // tbTimestamp
            // 
            this.tbTimestamp.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbTimestamp.Location = new System.Drawing.Point(0, 0);
            this.tbTimestamp.Margin = new System.Windows.Forms.Padding(4);
            this.tbTimestamp.Maximum = 3600;
            this.tbTimestamp.Name = "tbTimestamp";
            this.tbTimestamp.Size = new System.Drawing.Size(1163, 56);
            this.tbTimestamp.TabIndex = 0;
            this.tbTimestamp.Value = 1;
            this.tbTimestamp.ValueChanged += new System.EventHandler(this.tbTimestamp_ValueChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpSetting);
            this.tabControl1.Controls.Add(this.tpFootprint);
            this.tabControl1.Controls.Add(this.tpLog);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(20, 10);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1179, 701);
            this.tabControl1.TabIndex = 1;
            // 
            // tpSetting
            // 
            this.tpSetting.Controls.Add(this.tableLayoutPanel1);
            this.tpSetting.Location = new System.Drawing.Point(4, 39);
            this.tpSetting.Margin = new System.Windows.Forms.Padding(4);
            this.tpSetting.Name = "tpSetting";
            this.tpSetting.Padding = new System.Windows.Forms.Padding(4);
            this.tpSetting.Size = new System.Drawing.Size(1171, 658);
            this.tpSetting.TabIndex = 0;
            this.tpSetting.Text = "Setting";
            this.tpSetting.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 800F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.rtxtLog, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1163, 650);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // rtxtLog
            // 
            this.rtxtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtLog.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rtxtLog.Location = new System.Drawing.Point(804, 4);
            this.rtxtLog.Margin = new System.Windows.Forms.Padding(4);
            this.rtxtLog.Name = "rtxtLog";
            this.rtxtLog.ReadOnly = true;
            this.rtxtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtxtLog.Size = new System.Drawing.Size(355, 642);
            this.rtxtLog.TabIndex = 0;
            this.rtxtLog.Text = "";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel2.Controls.Add(this.btnSelectLogFile, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtMapFile, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLogFile, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.dtpStart, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.dtpEnd, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.cbStart, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.cbEnd, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.btnSelectMapFile, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnLoadSetting, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.btnRecovery, 2, 6);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(792, 642);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnSelectLogFile
            // 
            this.btnSelectLogFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectLogFile.Location = new System.Drawing.Point(729, 66);
            this.btnSelectLogFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectLogFile.Name = "btnSelectLogFile";
            this.btnSelectLogFile.Size = new System.Drawing.Size(59, 54);
            this.btnSelectLogFile.TabIndex = 13;
            this.btnSelectLogFile.Text = "...";
            this.btnSelectLogFile.UseVisualStyleBackColor = true;
            this.btnSelectLogFile.Click += new System.EventHandler(this.btnSelectLogFile_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 79);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Log File";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 141);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "Start Date";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 27);
            this.label3.TabIndex = 2;
            this.label3.Text = "Map File";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 203);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 27);
            this.label4.TabIndex = 3;
            this.label4.Text = "Start Time";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 265);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 27);
            this.label5.TabIndex = 4;
            this.label5.Text = "End Date";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 327);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 27);
            this.label6.TabIndex = 5;
            this.label6.Text = "End Time";
            // 
            // txtMapFile
            // 
            this.txtMapFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMapFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMapFile.Location = new System.Drawing.Point(151, 11);
            this.txtMapFile.Margin = new System.Windows.Forms.Padding(4);
            this.txtMapFile.Name = "txtMapFile";
            this.txtMapFile.ReadOnly = true;
            this.txtMapFile.Size = new System.Drawing.Size(570, 39);
            this.txtMapFile.TabIndex = 6;
            // 
            // txtLogFile
            // 
            this.txtLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLogFile.Location = new System.Drawing.Point(151, 73);
            this.txtLogFile.Margin = new System.Windows.Forms.Padding(4);
            this.txtLogFile.Name = "txtLogFile";
            this.txtLogFile.ReadOnly = true;
            this.txtLogFile.Size = new System.Drawing.Size(570, 39);
            this.txtLogFile.TabIndex = 7;
            // 
            // dtpStart
            // 
            this.dtpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpStart.CustomFormat = "";
            this.dtpStart.Location = new System.Drawing.Point(151, 135);
            this.dtpStart.Margin = new System.Windows.Forms.Padding(4);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(570, 39);
            this.dtpStart.TabIndex = 8;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEnd.CustomFormat = "";
            this.dtpEnd.Location = new System.Drawing.Point(151, 259);
            this.dtpEnd.Margin = new System.Windows.Forms.Padding(4);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(570, 39);
            this.dtpEnd.TabIndex = 9;
            // 
            // cbStart
            // 
            this.cbStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStart.FormattingEnabled = true;
            this.cbStart.Location = new System.Drawing.Point(151, 205);
            this.cbStart.Margin = new System.Windows.Forms.Padding(4);
            this.cbStart.Name = "cbStart";
            this.cbStart.Size = new System.Drawing.Size(570, 34);
            this.cbStart.TabIndex = 10;
            // 
            // cbEnd
            // 
            this.cbEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEnd.FormattingEnabled = true;
            this.cbEnd.Location = new System.Drawing.Point(151, 329);
            this.cbEnd.Margin = new System.Windows.Forms.Padding(4);
            this.cbEnd.Name = "cbEnd";
            this.cbEnd.Size = new System.Drawing.Size(570, 34);
            this.cbEnd.TabIndex = 11;
            // 
            // btnSelectMapFile
            // 
            this.btnSelectMapFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectMapFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectMapFile.Location = new System.Drawing.Point(729, 4);
            this.btnSelectMapFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectMapFile.Name = "btnSelectMapFile";
            this.btnSelectMapFile.Size = new System.Drawing.Size(59, 54);
            this.btnSelectMapFile.TabIndex = 12;
            this.btnSelectMapFile.Text = "...";
            this.btnSelectMapFile.UseVisualStyleBackColor = true;
            this.btnSelectMapFile.Click += new System.EventHandler(this.btnSelectMapFile_Click);
            // 
            // btnLoadSetting
            // 
            this.btnLoadSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadSetting.Location = new System.Drawing.Point(4, 376);
            this.btnLoadSetting.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadSetting.Name = "btnLoadSetting";
            this.btnLoadSetting.Size = new System.Drawing.Size(139, 54);
            this.btnLoadSetting.TabIndex = 14;
            this.btnLoadSetting.Text = "Load";
            this.btnLoadSetting.UseVisualStyleBackColor = true;
            this.btnLoadSetting.Click += new System.EventHandler(this.btnLoadSetting_Click);
            // 
            // btnRecovery
            // 
            this.btnRecovery.Location = new System.Drawing.Point(729, 376);
            this.btnRecovery.Margin = new System.Windows.Forms.Padding(4);
            this.btnRecovery.Name = "btnRecovery";
            this.btnRecovery.Size = new System.Drawing.Size(59, 54);
            this.btnRecovery.TabIndex = 15;
            this.btnRecovery.Text = "Re";
            this.btnRecovery.UseVisualStyleBackColor = true;
            this.btnRecovery.Visible = false;
            this.btnRecovery.Click += new System.EventHandler(this.btnRecovery_Click);
            // 
            // tpFootprint
            // 
            this.tpFootprint.Controls.Add(this.splitContainer1);
            this.tpFootprint.Controls.Add(this.panel1);
            this.tpFootprint.Location = new System.Drawing.Point(4, 39);
            this.tpFootprint.Margin = new System.Windows.Forms.Padding(4);
            this.tpFootprint.Name = "tpFootprint";
            this.tpFootprint.Padding = new System.Windows.Forms.Padding(4);
            this.tpFootprint.Size = new System.Drawing.Size(1171, 658);
            this.tpFootprint.TabIndex = 1;
            this.tpFootprint.Text = "Footprint";
            this.tpFootprint.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gluiCtrl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvVehicleInfo);
            this.splitContainer1.Size = new System.Drawing.Size(1163, 562);
            this.splitContainer1.SplitterDistance = 435;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 6;
            // 
            // gluiCtrl1
            // 
            this.gluiCtrl1.AllowObjectMenu = true;
            this.gluiCtrl1.AllowUndoMenu = true;
            this.gluiCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gluiCtrl1.Location = new System.Drawing.Point(0, 0);
            this.gluiCtrl1.Margin = new System.Windows.Forms.Padding(5);
            this.gluiCtrl1.Name = "gluiCtrl1";
            this.gluiCtrl1.ShowAGVText = true;
            this.gluiCtrl1.ShowAxis = true;
            this.gluiCtrl1.ShowGrid = true;
            this.gluiCtrl1.ShowObjectText = true;
            this.gluiCtrl1.Size = new System.Drawing.Size(1163, 435);
            this.gluiCtrl1.TabIndex = 2;
            this.gluiCtrl1.Zoom = 10D;
            // 
            // dgvVehicleInfo
            // 
            this.dgvVehicleInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVehicleInfo.ContextMenuStrip = this.cmenuDgvVehicleInfo;
            this.dgvVehicleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVehicleInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvVehicleInfo.Margin = new System.Windows.Forms.Padding(4);
            this.dgvVehicleInfo.Name = "dgvVehicleInfo";
            this.dgvVehicleInfo.RowHeadersWidth = 51;
            this.dgvVehicleInfo.RowTemplate.Height = 24;
            this.dgvVehicleInfo.Size = new System.Drawing.Size(1163, 122);
            this.dgvVehicleInfo.TabIndex = 0;
            this.dgvVehicleInfo.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvVehicleInfo_MouseDoubleClick);
            this.dgvVehicleInfo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvVehicleInfo_MouseDown);
            // 
            // cmenuDgvVehicleInfo
            // 
            this.cmenuDgvVehicleInfo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmenuDgvVehicleInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCopyText});
            this.cmenuDgvVehicleInfo.Name = "cmenuDgvVehicleInfo";
            this.cmenuDgvVehicleInfo.Size = new System.Drawing.Size(147, 28);
            // 
            // menuItemCopyText
            // 
            this.menuItemCopyText.Name = "menuItemCopyText";
            this.menuItemCopyText.Size = new System.Drawing.Size(146, 24);
            this.menuItemCopyText.Text = "Copy Text";
            this.menuItemCopyText.Click += new System.EventHandler(this.menuItemCopyText_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPauseHistoryvideo);
            this.panel1.Controls.Add(this.btnPlayHistoryvideo);
            this.panel1.Controls.Add(this.tbTimestamp);
            this.panel1.Controls.Add(this.lblCurrentTimestamp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(4, 566);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1163, 88);
            this.panel1.TabIndex = 3;
            // 
            // btnPlayHistoryvideo
            // 
            this.btnPlayHistoryvideo.Location = new System.Drawing.Point(550, 62);
            this.btnPlayHistoryvideo.Name = "btnPlayHistoryvideo";
            this.btnPlayHistoryvideo.Size = new System.Drawing.Size(75, 23);
            this.btnPlayHistoryvideo.TabIndex = 2;
            this.btnPlayHistoryvideo.Text = "Play";
            this.btnPlayHistoryvideo.UseVisualStyleBackColor = true;
            this.btnPlayHistoryvideo.Click += new System.EventHandler(this.btnPlayHistoryvideo_Click);
            // 
            // lblCurrentTimestamp
            // 
            this.lblCurrentTimestamp.AutoSize = true;
            this.lblCurrentTimestamp.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblCurrentTimestamp.Location = new System.Drawing.Point(7, 60);
            this.lblCurrentTimestamp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentTimestamp.Name = "lblCurrentTimestamp";
            this.lblCurrentTimestamp.Size = new System.Drawing.Size(203, 27);
            this.lblCurrentTimestamp.TabIndex = 1;
            this.lblCurrentTimestamp.Text = "1911/1/1 00:00:00";
            // 
            // tpLog
            // 
            this.tpLog.Controls.Add(this.tableLayoutPanel3);
            this.tpLog.Location = new System.Drawing.Point(4, 39);
            this.tpLog.Margin = new System.Windows.Forms.Padding(4);
            this.tpLog.Name = "tpLog";
            this.tpLog.Padding = new System.Windows.Forms.Padding(4);
            this.tpLog.Size = new System.Drawing.Size(1171, 658);
            this.tpLog.TabIndex = 2;
            this.tpLog.Text = "Log";
            this.tpLog.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 533F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.btnLoadLogTable, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.cbLogTableName, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.dgvLogTable, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1163, 650);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // btnLoadLogTable
            // 
            this.btnLoadLogTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadLogTable.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnLoadLogTable.Location = new System.Drawing.Point(684, 4);
            this.btnLoadLogTable.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadLogTable.Name = "btnLoadLogTable";
            this.btnLoadLogTable.Size = new System.Drawing.Size(125, 54);
            this.btnLoadLogTable.TabIndex = 15;
            this.btnLoadLogTable.Text = "Load";
            this.btnLoadLogTable.UseVisualStyleBackColor = true;
            this.btnLoadLogTable.Click += new System.EventHandler(this.btnLoadLogTable_Click);
            // 
            // cbLogTableName
            // 
            this.cbLogTableName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLogTableName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLogTableName.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbLogTableName.FormattingEnabled = true;
            this.cbLogTableName.Location = new System.Drawing.Point(151, 13);
            this.cbLogTableName.Margin = new System.Windows.Forms.Padding(4);
            this.cbLogTableName.Name = "cbLogTableName";
            this.cbLogTableName.Size = new System.Drawing.Size(525, 34);
            this.cbLogTableName.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(4, 17);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 27);
            this.label7.TabIndex = 3;
            this.label7.Text = "Table";
            // 
            // dgvLogTable
            // 
            this.dgvLogTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel3.SetColumnSpan(this.dgvLogTable, 4);
            this.dgvLogTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLogTable.Location = new System.Drawing.Point(4, 66);
            this.dgvLogTable.Margin = new System.Windows.Forms.Padding(4);
            this.dgvLogTable.Name = "dgvLogTable";
            this.dgvLogTable.RowHeadersWidth = 51;
            this.dgvLogTable.RowTemplate.Height = 24;
            this.dgvLogTable.Size = new System.Drawing.Size(1155, 580);
            this.dgvLogTable.TabIndex = 16;
            // 
            // btnPauseHistoryvideo
            // 
            this.btnPauseHistoryvideo.Location = new System.Drawing.Point(681, 62);
            this.btnPauseHistoryvideo.Name = "btnPauseHistoryvideo";
            this.btnPauseHistoryvideo.Size = new System.Drawing.Size(75, 23);
            this.btnPauseHistoryvideo.TabIndex = 4;
            this.btnPauseHistoryvideo.Text = "Pause";
            this.btnPauseHistoryvideo.UseVisualStyleBackColor = true;
            this.btnPauseHistoryvideo.Click += new System.EventHandler(this.btnPauseHistoryvideo_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 701);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Footprint Viewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbTimestamp)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpSetting.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tpFootprint.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleInfo)).EndInit();
            this.cmenuDgvVehicleInfo.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tpLog.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogTable)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TrackBar tbTimestamp;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tpSetting;
		private System.Windows.Forms.TabPage tpFootprint;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.RichTextBox rtxtLog;
		private System.Windows.Forms.Label lblCurrentTimestamp;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtMapFile;
		private System.Windows.Forms.TextBox txtLogFile;
		private System.Windows.Forms.DateTimePicker dtpStart;
		private System.Windows.Forms.DateTimePicker dtpEnd;
		private System.Windows.Forms.ComboBox cbStart;
		private System.Windows.Forms.ComboBox cbEnd;
		private System.Windows.Forms.Button btnSelectLogFile;
		private System.Windows.Forms.Button btnSelectMapFile;
		private System.Windows.Forms.Button btnLoadSetting;
		private GLUI.GLUICtrl gluiCtrl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView dgvVehicleInfo;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ContextMenuStrip cmenuDgvVehicleInfo;
		private System.Windows.Forms.ToolStripMenuItem menuItemCopyText;
		private System.Windows.Forms.TabPage tpLog;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Button btnLoadLogTable;
		private System.Windows.Forms.ComboBox cbLogTableName;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.DataGridView dgvLogTable;
		private System.Windows.Forms.Button btnRecovery;
        private System.Windows.Forms.Button btnPlayHistoryvideo;
        private System.Windows.Forms.Button btnPauseHistoryvideo;
    }
}

