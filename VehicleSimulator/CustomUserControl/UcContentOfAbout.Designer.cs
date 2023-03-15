namespace VehicleSimulator
{
	partial class UcContentOfAbout
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.lbVersionNumber = new System.Windows.Forms.Label();
            this.lbExplanation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(584, 360);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Icon by icons8";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label2.Location = new System.Drawing.Point(584, 390);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "https://icons8.com/";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.lbVersion.Font = new System.Drawing.Font("新細明體", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbVersion.ForeColor = System.Drawing.Color.White;
            this.lbVersion.Location = new System.Drawing.Point(50, 31);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(389, 33);
            this.lbVersion.TabIndex = 2;
            this.lbVersion.Text = "VehicleSimulator Version :";
            // 
            // lbVersionNumber
            // 
            this.lbVersionNumber.AutoSize = true;
            this.lbVersionNumber.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbVersionNumber.ForeColor = System.Drawing.Color.White;
            this.lbVersionNumber.Location = new System.Drawing.Point(430, 36);
            this.lbVersionNumber.Name = "lbVersionNumber";
            this.lbVersionNumber.Size = new System.Drawing.Size(57, 28);
            this.lbVersionNumber.TabIndex = 3;
            this.lbVersionNumber.Text = "ver.";
            // 
            // lbExplanation
            // 
            this.lbExplanation.AutoSize = true;
            this.lbExplanation.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbExplanation.ForeColor = System.Drawing.Color.White;
            this.lbExplanation.Location = new System.Drawing.Point(56, 129);
            this.lbExplanation.Name = "lbExplanation";
            this.lbExplanation.Size = new System.Drawing.Size(383, 40);
            this.lbExplanation.TabIndex = 5;
            this.lbExplanation.Text = "Version Number:\r\n  major.AddNewFunction.Debug.YYMMDD";
            // 
            // UcContentOfAbout
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.Controls.Add(this.lbExplanation);
            this.Controls.Add(this.lbVersionNumber);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UcContentOfAbout";
            this.Size = new System.Drawing.Size(720, 420);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label lbVersionNumber;
        private System.Windows.Forms.Label lbExplanation;
    }
}
