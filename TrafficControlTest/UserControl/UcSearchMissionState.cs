﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest.UserControl
{
	public class UcSearchMissionState : UcSearch
	{
		public override string mKeyword { get; } = "Mission";

		public UcSearchMissionState() : base() { }

		protected override string ConvertSearchOptionsToSqlCommand(string Keyword, int Limit)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(Keyword) || Keyword == "Recent")
			{
				result = $"SELECT * FROM MissionState ORDER BY ReceiveTimestamp DESC LIMIT {Limit.ToString()}";
			}
			else
			{
				result = $"SELECT * FROM MissionState WHERE (ID LIKE '%{Keyword}%') ORDER BY ReceiveTimestamp DESC LIMIT {Limit.ToString()}";
			}
			return result;
		}
		protected override void UpdateGui_DgvSearchResult_Initialize()
		{
			this.InvokeIfNecessary(() =>
			{
				DataGridView dgv = GetDgv();

				dgv.SelectionChanged += ((sender, e) => dgv.ClearSelection());

				dgv.RowHeadersVisible = false;
				dgv.AllowUserToAddRows = false;
				dgv.AllowUserToResizeRows = false;
				dgv.AllowUserToResizeColumns = false;
				dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				dgv.MultiSelect = false;
				dgv.BackgroundColor = TableBackColor;
				dgv.GridColor = TableGridLineColor;
				dgv.BorderStyle = BorderStyle.None;

				dgv.EnableHeadersVisualStyles = false;
				dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold);
				dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
				dgv.ColumnHeadersDefaultCellStyle.BackColor = TableHeaderBackColor;
				dgv.ColumnHeadersDefaultCellStyle.ForeColor = TableHeaderForeColor;
				dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
				dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
				dgv.ColumnHeadersHeight = 60;

				dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
				dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
				dgv.DefaultCellStyle.BackColor = TableEvenRowBackColor;
				dgv.DefaultCellStyle.ForeColor = TableRowForeColor;
				dgv.RowTemplate.Height = 25;

				dgv.Columns.Add("No", "No");
				dgv.Columns[0].Width = 50;
				dgv.Columns.Add("ID", "ID");
				dgv.Columns[1].Width = 200;
				dgv.Columns.Add("Type", "Type");
				dgv.Columns[2].Width = 80;
				dgv.Columns.Add("Priority", "Priority");
				dgv.Columns[3].Width = 70;
				dgv.Columns.Add("VehicleID", "VehicleID");
				dgv.Columns[4].Width = 130;
				dgv.Columns.Add("Parameters", "Parameters");
				dgv.Columns[5].Width = 160;
				dgv.Columns.Add("SourceIPPort", "SourceIPPort");
				dgv.Columns[6].Width = 180;
				dgv.Columns.Add("ExecutorID", "ExecutorID");
				dgv.Columns[7].Width = 130;
				dgv.Columns.Add("SendState", "SendState");
				dgv.Columns[8].Width = 110;
				dgv.Columns.Add("ExecuteState", "ExecuteState");
				dgv.Columns[9].Width = 130;
				dgv.Columns.Add("ReceiveTimestamp", "ReceiveTimestamp");
				dgv.Columns[10].Width = 180;
				dgv.Columns.Add("StartTimestamp", "StartTimestamp");
				dgv.Columns[11].Width = 180;
				dgv.Columns.Add("StopTimestamp", "StopTimestamp");
				dgv.Columns[12].Width = 180;
				dgv.Columns.Add("LastUpdateTimestamp", "LastUpdateTimestamp");
				dgv.Columns[13].Width = 180;

				foreach (DataGridViewColumn column in dgv.Columns)
				{
					column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
					column.SortMode = DataGridViewColumnSortMode.NotSortable;
					column.ReadOnly = true;
				}
			});
		}
	}
}
