﻿namespace TrafficControlTest.UserControl
{
	partial class UcSetting
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

		#region 元件設計工具產生的程式碼

		/// <summary> 
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.dgvSettings = new System.Windows.Forms.DataGridView();
            this.tlpSettings = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tlpMapManagementSetting = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvMapManagementSetting = new System.Windows.Forms.DataGridView();
            this.cmenuDgvMapManagementSetting = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmenuItemAddRegion = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuItemInsertRegion = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuItemClearCurrentMapName = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuItemDeleteRegion = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSettings)).BeginInit();
            this.tlpSettings.SuspendLayout();
            this.tlpMapManagementSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapManagementSetting)).BeginInit();
            this.cmenuDgvMapManagementSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSettings
            // 
            this.dgvSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSettings.Location = new System.Drawing.Point(3, 63);
            this.dgvSettings.Name = "dgvSettings";
            this.dgvSettings.RowHeadersWidth = 51;
            this.dgvSettings.RowTemplate.Height = 27;
            this.dgvSettings.Size = new System.Drawing.Size(844, 54);
            this.dgvSettings.TabIndex = 0;
            this.dgvSettings.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSettings_CellContentClick);
            this.dgvSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSettings_CellValueChanged);
            // 
            // tlpSettings
            // 
            this.tlpSettings.ColumnCount = 1;
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings.Controls.Add(this.label1, 0, 0);
            this.tlpSettings.Controls.Add(this.dgvSettings, 0, 1);
            this.tlpSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpSettings.Name = "tlpSettings";
            this.tlpSettings.RowCount = 2;
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings.Size = new System.Drawing.Size(850, 120);
            this.tlpSettings.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "    General Setting";
            // 
            // tlpMapManagementSetting
            // 
            this.tlpMapManagementSetting.ColumnCount = 1;
            this.tlpMapManagementSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapManagementSetting.Controls.Add(this.label2, 0, 0);
            this.tlpMapManagementSetting.Controls.Add(this.dgvMapManagementSetting, 0, 1);
            this.tlpMapManagementSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpMapManagementSetting.Location = new System.Drawing.Point(0, 120);
            this.tlpMapManagementSetting.Name = "tlpMapManagementSetting";
            this.tlpMapManagementSetting.RowCount = 2;
            this.tlpMapManagementSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpMapManagementSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapManagementSetting.Size = new System.Drawing.Size(850, 120);
            this.tlpMapManagementSetting.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(3, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(369, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "    Map Management Setting";
            // 
            // dgvMapManagementSetting
            // 
            this.dgvMapManagementSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMapManagementSetting.ContextMenuStrip = this.cmenuDgvMapManagementSetting;
            this.dgvMapManagementSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMapManagementSetting.Location = new System.Drawing.Point(3, 63);
            this.dgvMapManagementSetting.Name = "dgvMapManagementSetting";
            this.dgvMapManagementSetting.RowHeadersWidth = 51;
            this.dgvMapManagementSetting.RowTemplate.Height = 27;
            this.dgvMapManagementSetting.Size = new System.Drawing.Size(844, 54);
            this.dgvMapManagementSetting.TabIndex = 1;
            this.dgvMapManagementSetting.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMapManagementSetting_CellContentClick);
            this.dgvMapManagementSetting.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMapManagementSetting_ValueChanged);
            this.dgvMapManagementSetting.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvMapManagementSetting_ValueChanged);
            this.dgvMapManagementSetting.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvMapManagementSetting_MouseDown);
            // 
            // cmenuDgvMapManagementSetting
            // 
            this.cmenuDgvMapManagementSetting.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmenuDgvMapManagementSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuItemAddRegion,
            this.cmenuItemInsertRegion,
            this.cmenuItemClearCurrentMapName,
            this.cmenuItemDeleteRegion});
            this.cmenuDgvMapManagementSetting.Name = "cmenuDgvMapManagementSetting";
            this.cmenuDgvMapManagementSetting.Size = new System.Drawing.Size(253, 100);
            // 
            // cmenuItemAddRegion
            // 
            this.cmenuItemAddRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.cmenuItemAddRegion.ForeColor = System.Drawing.Color.White;
            this.cmenuItemAddRegion.Name = "cmenuItemAddRegion";
            this.cmenuItemAddRegion.Size = new System.Drawing.Size(252, 24);
            this.cmenuItemAddRegion.Text = "Add Region";
            this.cmenuItemAddRegion.Click += new System.EventHandler(this.cmenuItemAddRegion_Click);
            // 
            // cmenuItemInsertRegion
            // 
            this.cmenuItemInsertRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.cmenuItemInsertRegion.ForeColor = System.Drawing.SystemColors.Control;
            this.cmenuItemInsertRegion.Name = "cmenuItemInsertRegion";
            this.cmenuItemInsertRegion.Size = new System.Drawing.Size(252, 24);
            this.cmenuItemInsertRegion.Text = "Insert Region";
            this.cmenuItemInsertRegion.Click += new System.EventHandler(this.cmenuItemInsertRegion_Click);
            // 
            // cmenuItemClearCurrentMapName
            // 
            this.cmenuItemClearCurrentMapName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.cmenuItemClearCurrentMapName.ForeColor = System.Drawing.Color.White;
            this.cmenuItemClearCurrentMapName.Name = "cmenuItemClearCurrentMapName";
            this.cmenuItemClearCurrentMapName.Size = new System.Drawing.Size(252, 24);
            this.cmenuItemClearCurrentMapName.Text = "Clear Current Map Name";
            this.cmenuItemClearCurrentMapName.Click += new System.EventHandler(this.cmenuItemClearCurrentMapName_Click);
            // 
            // cmenuItemDeleteRegion
            // 
            this.cmenuItemDeleteRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.cmenuItemDeleteRegion.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmenuItemDeleteRegion.Name = "cmenuItemDeleteRegion";
            this.cmenuItemDeleteRegion.Size = new System.Drawing.Size(252, 24);
            this.cmenuItemDeleteRegion.Text = "Delete Region";
            this.cmenuItemDeleteRegion.Click += new System.EventHandler(this.cmenuItemDeleteRegion_Click);
            // 
            // UcSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
            this.Controls.Add(this.tlpMapManagementSetting);
            this.Controls.Add(this.tlpSettings);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "UcSetting";
            this.Size = new System.Drawing.Size(850, 600);
            this.Load += new System.EventHandler(this.UcSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSettings)).EndInit();
            this.tlpSettings.ResumeLayout(false);
            this.tlpSettings.PerformLayout();
            this.tlpMapManagementSetting.ResumeLayout(false);
            this.tlpMapManagementSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapManagementSetting)).EndInit();
            this.cmenuDgvMapManagementSetting.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.DataGridView dgvSettings;
		private System.Windows.Forms.TableLayoutPanel tlpSettings;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tlpMapManagementSetting;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DataGridView dgvMapManagementSetting;
		private System.Windows.Forms.ContextMenuStrip cmenuDgvMapManagementSetting;
		private System.Windows.Forms.ToolStripMenuItem cmenuItemAddRegion;
		private System.Windows.Forms.ToolStripMenuItem cmenuItemClearCurrentMapName;
        private System.Windows.Forms.ToolStripMenuItem cmenuItemDeleteRegion;
        private System.Windows.Forms.ToolStripMenuItem cmenuItemInsertRegion;
    }
}
