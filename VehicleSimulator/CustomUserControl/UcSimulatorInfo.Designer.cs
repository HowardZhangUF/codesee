﻿namespace VehicleSimulator
{
    partial class UcSimulatorInfo
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent() {
            this.SetAndMoveTextBox = new System.Windows.Forms.TextBox();
            this.cbMoveTarget = new System.Windows.Forms.ComboBox();
            this.cbGoalList = new System.Windows.Forms.ComboBox();
            this.txtHostIpPort = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvSimulatorInfo = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSimulatorConnect = new System.Windows.Forms.Button();
            this.btnSimulatorSetLocation = new System.Windows.Forms.Button();
            this.btnSimulatorMove = new System.Windows.Forms.Button();
            this.btnSimulatorSetAndMove = new System.Windows.Forms.Button();
            this.btnSimulatorStop = new System.Windows.Forms.Button();
            this.lblSimulatorName = new System.Windows.Forms.Label();
            this.lbPoints = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimulatorInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // SetAndMoveTextBox
            // 
            this.SetAndMoveTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SetAndMoveTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.SetAndMoveTextBox.Font = new System.Drawing.Font("新細明體", 10.2F);
            this.SetAndMoveTextBox.ForeColor = System.Drawing.Color.Gray;
            this.SetAndMoveTextBox.Location = new System.Drawing.Point(152, 201);
            this.SetAndMoveTextBox.Name = "SetAndMoveTextBox";
            this.SetAndMoveTextBox.Size = new System.Drawing.Size(292, 28);
            this.SetAndMoveTextBox.TabIndex = 13;
            this.SetAndMoveTextBox.Text = "(Target)(Target)...(Loop)";
            this.SetAndMoveTextBox.GotFocus += new System.EventHandler(this.SetAndMoveTextBox_GotFocus);
            this.SetAndMoveTextBox.LostFocus += new System.EventHandler(this.SetAndMoveTextBox_LostFocus);
            // 
            // cbMoveTarget
            // 
            this.cbMoveTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMoveTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.cbMoveTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMoveTarget.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbMoveTarget.ForeColor = System.Drawing.Color.White;
            this.cbMoveTarget.FormattingEnabled = true;
            this.cbMoveTarget.Location = new System.Drawing.Point(153, 161);
            this.cbMoveTarget.Margin = new System.Windows.Forms.Padding(4);
            this.cbMoveTarget.Name = "cbMoveTarget";
            this.cbMoveTarget.Size = new System.Drawing.Size(290, 28);
            this.cbMoveTarget.TabIndex = 12;
            // 
            // cbGoalList
            // 
            this.cbGoalList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbGoalList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.cbGoalList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGoalList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbGoalList.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbGoalList.ForeColor = System.Drawing.Color.White;
            this.cbGoalList.FormattingEnabled = true;
            this.cbGoalList.Location = new System.Drawing.Point(153, 121);
            this.cbGoalList.Margin = new System.Windows.Forms.Padding(4);
            this.cbGoalList.Name = "cbGoalList";
            this.cbGoalList.Size = new System.Drawing.Size(290, 28);
            this.cbGoalList.TabIndex = 11;
            // 
            // txtHostIpPort
            // 
            this.txtHostIpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHostIpPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.txtHostIpPort.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtHostIpPort.ForeColor = System.Drawing.Color.White;
            this.txtHostIpPort.Location = new System.Drawing.Point(152, 81);
            this.txtHostIpPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtHostIpPort.Name = "txtHostIpPort";
            this.txtHostIpPort.Size = new System.Drawing.Size(292, 28);
            this.txtHostIpPort.TabIndex = 9;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvSimulatorInfo, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.btnSimulatorConnect, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtHostIpPort, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnSimulatorSetLocation, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnSimulatorMove, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.cbGoalList, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cbMoveTarget, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnSimulatorSetAndMove, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.SetAndMoveTextBox, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnSimulatorStop, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblSimulatorName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbPoints, 0, 9);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(11, 10);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(447, 398);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Operation";
            // 
            // dgvSimulatorInfo
            // 
            this.dgvSimulatorInfo.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.dgvSimulatorInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSimulatorInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.dgvSimulatorInfo, 2);
            this.dgvSimulatorInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSimulatorInfo.Location = new System.Drawing.Point(0, 354);
            this.dgvSimulatorInfo.Margin = new System.Windows.Forms.Padding(0);
            this.dgvSimulatorInfo.Name = "dgvSimulatorInfo";
            this.dgvSimulatorInfo.RowHeadersWidth = 51;
            this.dgvSimulatorInfo.RowTemplate.Height = 27;
            this.dgvSimulatorInfo.Size = new System.Drawing.Size(447, 44);
            this.dgvSimulatorInfo.TabIndex = 0;
            this.dgvSimulatorInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSimulatorInfo_CellValueChanged);
            this.dgvSimulatorInfo.SelectionChanged += new System.EventHandler(this.dgvSimulatorInfo_SelectionChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 67);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(441, 1);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gray;
            this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 346);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(441, 1);
            this.panel2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 315);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Information";
            // 
            // btnSimulatorConnect
            // 
            this.btnSimulatorConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSimulatorConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.btnSimulatorConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimulatorConnect.ForeColor = System.Drawing.Color.White;
            this.btnSimulatorConnect.Location = new System.Drawing.Point(3, 80);
            this.btnSimulatorConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSimulatorConnect.Name = "btnSimulatorConnect";
            this.btnSimulatorConnect.Size = new System.Drawing.Size(143, 30);
            this.btnSimulatorConnect.TabIndex = 7;
            this.btnSimulatorConnect.Text = "Connect";
            this.btnSimulatorConnect.UseVisualStyleBackColor = false;
            this.btnSimulatorConnect.Click += new System.EventHandler(this.btnSimulatorConnect_Click);
            // 
            // btnSimulatorSetLocation
            // 
            this.btnSimulatorSetLocation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSimulatorSetLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimulatorSetLocation.ForeColor = System.Drawing.Color.White;
            this.btnSimulatorSetLocation.Location = new System.Drawing.Point(3, 120);
            this.btnSimulatorSetLocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSimulatorSetLocation.Name = "btnSimulatorSetLocation";
            this.btnSimulatorSetLocation.Size = new System.Drawing.Size(143, 30);
            this.btnSimulatorSetLocation.TabIndex = 10;
            this.btnSimulatorSetLocation.Text = "Set Location";
            this.btnSimulatorSetLocation.UseVisualStyleBackColor = true;
            this.btnSimulatorSetLocation.Click += new System.EventHandler(this.btnSimulatorSetLocation_Click);
            // 
            // btnSimulatorMove
            // 
            this.btnSimulatorMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSimulatorMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimulatorMove.ForeColor = System.Drawing.Color.White;
            this.btnSimulatorMove.Location = new System.Drawing.Point(3, 160);
            this.btnSimulatorMove.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSimulatorMove.Name = "btnSimulatorMove";
            this.btnSimulatorMove.Size = new System.Drawing.Size(143, 30);
            this.btnSimulatorMove.TabIndex = 5;
            this.btnSimulatorMove.Text = "Move";
            this.btnSimulatorMove.UseVisualStyleBackColor = true;
            this.btnSimulatorMove.Click += new System.EventHandler(this.btnSimulatorMove_Click);
            // 
            // btnSimulatorSetAndMove
            // 
            this.btnSimulatorSetAndMove.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSimulatorSetAndMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimulatorSetAndMove.ForeColor = System.Drawing.Color.White;
            this.btnSimulatorSetAndMove.Location = new System.Drawing.Point(3, 201);
            this.btnSimulatorSetAndMove.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSimulatorSetAndMove.Name = "btnSimulatorSetAndMove";
            this.btnSimulatorSetAndMove.Size = new System.Drawing.Size(143, 28);
            this.btnSimulatorSetAndMove.TabIndex = 10;
            this.btnSimulatorSetAndMove.Text = "SetAndMove";
            this.btnSimulatorSetAndMove.UseVisualStyleBackColor = true;
            this.btnSimulatorSetAndMove.Click += new System.EventHandler(this.btnSimulatorSetAndMove_Click);
            // 
            // btnSimulatorStop
            // 
            this.btnSimulatorStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSimulatorStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimulatorStop.ForeColor = System.Drawing.Color.White;
            this.btnSimulatorStop.Location = new System.Drawing.Point(3, 238);
            this.btnSimulatorStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSimulatorStop.Name = "btnSimulatorStop";
            this.btnSimulatorStop.Size = new System.Drawing.Size(143, 29);
            this.btnSimulatorStop.TabIndex = 4;
            this.btnSimulatorStop.Text = "Stop";
            this.btnSimulatorStop.UseVisualStyleBackColor = true;
            this.btnSimulatorStop.Click += new System.EventHandler(this.btnSimulatorStop_Click);
            // 
            // lblSimulatorName
            // 
            this.lblSimulatorName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSimulatorName.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSimulatorName, 2);
            this.lblSimulatorName.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblSimulatorName.ForeColor = System.Drawing.Color.White;
            this.lblSimulatorName.Location = new System.Drawing.Point(3, 6);
            this.lblSimulatorName.Name = "lblSimulatorName";
            this.lblSimulatorName.Size = new System.Drawing.Size(441, 28);
            this.lblSimulatorName.TabIndex = 8;
            this.lblSimulatorName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblSimulatorName.UseWaitCursor = true;
            // 
            // lbPoints
            // 
            this.lbPoints.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbPoints.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbPoints, 10);
            this.lbPoints.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbPoints.ForeColor = System.Drawing.Color.White;
            this.lbPoints.Location = new System.Drawing.Point(127, 274);
            this.lbPoints.Name = "lbPoints";
            this.lbPoints.Size = new System.Drawing.Size(192, 28);
            this.lbPoints.TabIndex = 14;
            this.lbPoints.Text = "(Start) =>(End)";
            this.lbPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UcSimulatorInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UcSimulatorInfo";
            this.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.Size = new System.Drawing.Size(469, 420);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimulatorInfo)).EndInit();
            this.ResumeLayout(false);

        }

        
        private void SetAndMoveTextBox_LostFocus(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(SetAndMoveTextBox.Text))
            {
                SetAndMoveTextBox.ForeColor = System.Drawing.Color.Gray;
                SetAndMoveTextBox.Text = "(Target)(Target)...(Loop)";
            }
        }

        private void SetAndMoveTextBox_GotFocus(object sender, System.EventArgs e)
        {
            if (SetAndMoveTextBox.Text == "(Target)(Target)...(Loop)")
            {
                SetAndMoveTextBox.Clear();
                SetAndMoveTextBox.ForeColor = System.Drawing.Color.White;
            }

        }


        #endregion

        private System.Windows.Forms.TextBox SetAndMoveTextBox;
        private System.Windows.Forms.ComboBox cbMoveTarget;
        private System.Windows.Forms.ComboBox cbGoalList;
        private System.Windows.Forms.TextBox txtHostIpPort;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvSimulatorInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSimulatorConnect;
        private System.Windows.Forms.Button btnSimulatorSetLocation;
        private System.Windows.Forms.Button btnSimulatorMove;
        private System.Windows.Forms.Button btnSimulatorSetAndMove;
        private System.Windows.Forms.Button btnSimulatorStop;
        private System.Windows.Forms.Label lblSimulatorName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbPoints;
    }
}
