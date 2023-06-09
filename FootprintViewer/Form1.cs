﻿using Geometry;
using GLCore;
using GLStyle;
using LibraryForVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace FootprintViewer
{
	public partial class Form1 : Form
	{
		

		private DateTime mStartTimestamp { get; set; } = DateTime.MinValue;
		private DateTime mEndTimestamp { get; set; } = DateTime.MinValue;
		private DateTime mCurrentTimestamp { get; set; } = DateTime.MinValue;
		private Dictionary<string, List<HistoryVehicleInfo>> mHistoryVehicleInfos { get; set; } = new Dictionary<string, List<HistoryVehicleInfo>>();
		private DatabaseAdapter mDatabaseAdapter = new SqliteDatabaseAdapter(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false);

		private Dictionary<string, int> mIconIdsOfVehicle = new Dictionary<string, int>();
		private Dictionary<string, int> mIconIdsOfVehiclePath = new Dictionary<string, int>();
		private Dictionary<string, int> mIconIdsOfVehicleLaser = new Dictionary<string, int>();
		private int mDgvVehicleInfoRightClickRowIndex { get; set; } = -1;
		private int mDgvVehicleInfoRightClickColIndex { get; set; } = -1;
		private Dictionary<string, string> mTimestampKeywordTable = new Dictionary<string, string>()
		{
			{ "GeneralLog", "Timestamp" },
			{ "HistoryAutomaticDoorControlInfo", "ReceiveTimestamp" },
			{ "HistoryHostCommunicationInfo", "ReceiveTimestamp" },
			{ "HistoryMissionInfo", "ReceiveTimestamp" },
			{ "HistoryVehicleControlInfo", "ReceiveTimestamp" },
			{ "HistoryVehicleInfo", "RecordTimestamp" }
		};

		public Form1()
		{
			InitializeComponent();

			gluiCtrl1.SetControlMode(true);
			gluiCtrl1.SetEditMode(true);
			gluiCtrl1.ShowObjectText = false;
			StyleManager.LoadStyle("Style.ini");
			GLCMD.CMD.Initial();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				InitializeDgvVehicleInfo();
				InitializeTimeComboBox(cbStart);
				InitializeTimeComboBox(cbEnd);
				InitializeCbLogTableName();
				InitializeDgvLogTable();
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void btnSelectMapFile_Click(object sender, EventArgs e)
		{
			try
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title = "Choose Map File";
					ofd.Filter = "All Files (*.*)|*.*";
					ofd.Multiselect = false;
					if (ofd.ShowDialog() == DialogResult.OK)
					{
						txtMapFile.Text = ofd.FileName;
					}
				}
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void btnSelectLogFile_Click(object sender, EventArgs e)
		{
			try
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title = "Choose Log File";
					ofd.Filter = "All Files (*.*)|*.*";
					ofd.Multiselect = false;
					if (ofd.ShowDialog() == DialogResult.OK)
					{
						txtLogFile.Text = ofd.FileName;
					}
				}
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void btnLoadSetting_Click(object sender, EventArgs e)
		{
			try
			{
				string mapFile = txtMapFile.Text;
				string logFile = txtLogFile.Text;
				DateTime startTimestamp = dtpStart.Value.Date;
				DateTime endTimestamp = dtpEnd.Value.Date;
				startTimestamp = startTimestamp.AddHours(int.Parse(cbStart.SelectedItem.ToString().Substring(0, 2)));
				endTimestamp = endTimestamp.AddHours(int.Parse(cbEnd.SelectedItem.ToString().Substring(0, 2)));

				string tmp = string.Empty;
				tmp += $"[Load Setting]";
				tmp += $" ";
				tmp += $"MapFile:{mapFile}";
				tmp += $" ";
				tmp += $"LogFile:{logFile}";
				tmp += $" ";
				tmp += $"Start:{startTimestamp.ToString("yyyy/MM/dd HH:mm:ss")}";
				tmp += $" ";
				tmp += $"End:{endTimestamp.ToString("yyyy/MM/dd HH:mm:ss")}";
				RecordLogMessage(rtxtLog, tmp);

				InitializeFootprintData(mapFile, logFile, startTimestamp, endTimestamp);

				if (mHistoryVehicleInfos.Count > 0)
				{
					tbTimestamp.Value = tbTimestamp.Maximum / 10;
					RecordLogMessage(rtxtLog, "Load Successed!");
					MessageBox.Show("Load Successed!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					RecordLogMessage(rtxtLog, "Load Failed! No Log Data!");
					MessageBox.Show("Load Failed! No Log Data!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void dgvVehicleInfo_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left)
				{
					var hit = dgvVehicleInfo.HitTest(e.X, e.Y);
					if (hit.RowIndex > -1 && hit.ColumnIndex > -1)
					{
						int x = int.Parse(dgvVehicleInfo.Rows[hit.RowIndex].Cells["X"].Value.ToString());
						int y = int.Parse(dgvVehicleInfo.Rows[hit.RowIndex].Cells["Y"].Value.ToString());
						gluiCtrl1.Focus(x, y);
					}
				}
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void dgvVehicleInfo_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					var hit = dgvVehicleInfo.HitTest(e.X, e.Y);
					mDgvVehicleInfoRightClickRowIndex = hit.RowIndex;
					mDgvVehicleInfoRightClickColIndex = hit.ColumnIndex;
				}
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void menuItemCopyText_Click(object sender, EventArgs e)
		{
			try
			{
				if (mDgvVehicleInfoRightClickRowIndex >= 0 && mDgvVehicleInfoRightClickRowIndex < dgvVehicleInfo.RowCount && mDgvVehicleInfoRightClickColIndex >= 0 && mDgvVehicleInfoRightClickColIndex < dgvVehicleInfo.ColumnCount && !string.IsNullOrEmpty(dgvVehicleInfo.Rows[mDgvVehicleInfoRightClickRowIndex].Cells[mDgvVehicleInfoRightClickColIndex].Value.ToString()))
				{
					Clipboard.SetText(dgvVehicleInfo.Rows[mDgvVehicleInfoRightClickRowIndex].Cells[mDgvVehicleInfoRightClickColIndex].Value.ToString());
				}
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void tbTimestamp_ValueChanged(object sender, EventArgs e)
		{
			
			try
			{
				if (mHistoryVehicleInfos.Count == 0) return;

				//Senconds_perValue : tbTimestamp每格代表幾秒
				//目前預設tbTimestamp全長3600(寫死 之後要改成可更改)
				double Senconds_perValue = (mEndTimestamp.Subtract(mStartTimestamp).TotalSeconds / (tbTimestamp.Maximum - tbTimestamp.Minimum))* tbTimestamp.Value;
				mCurrentTimestamp = mStartTimestamp.AddSeconds(Senconds_perValue);

				lblCurrentTimestamp.Text = mCurrentTimestamp.ToString("yyyy/MM/dd HH:mm:ss");
				RefreshGui();
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
			
		}
		private void btnLoadLogTable_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbLogTableName.SelectedIndex > -1)
				{
					string logFile = txtLogFile.Text;
					string logTableName = cbLogTableName.Text;
					DateTime startTimestamp = dtpStart.Value.Date;
					DateTime endTimestamp = dtpEnd.Value.Date;
					startTimestamp = startTimestamp.AddHours(int.Parse(cbStart.SelectedItem.ToString().Substring(0, 2)));
					endTimestamp = endTimestamp.AddHours(int.Parse(cbEnd.SelectedItem.ToString().Substring(0, 2)));

					string tmp = string.Empty;
					tmp += $"[Load Log Table]";
					tmp += $" ";
					tmp += $"LogFile:{logFile}";
					tmp += $" ";
					tmp += $"LogTableName: {logTableName}";
					tmp += $" ";
					tmp += $"Start:{startTimestamp.ToString("yyyy/MM/dd HH:mm:ss")}";
					tmp += $" ";
					tmp += $"End:{endTimestamp.ToString("yyyy/MM/dd HH:mm:ss")}";
					RecordLogMessage(rtxtLog, tmp);

					ReadLogData(logFile, logTableName, startTimestamp, endTimestamp);
				}
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}
		private void btnRecovery_Click(object sender, EventArgs e)
		{
			// Exception 裡面會記錄執行失敗的 Sql Command ，此方法可以將那些 Sql Command 撈出來並重新執行
			// 但若是因為 Table Format 錯誤而導致的，則必須先手動調整 Database 的 Table Format 後，再執行此方法
			try
			{
				string databaseFilePath = string.Empty;
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title = "Choose Database File";
					ofd.Filter = "All Files (*.*)|*.*";
					ofd.Multiselect = false;
					if (ofd.ShowDialog() == DialogResult.OK)
					{
						databaseFilePath = ofd.FileName;
					}
				}

				List<string> exceptionFilePaths = new List<string>();
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title = "Choose Exception Files";
					ofd.Filter = "All Files (*.*)|*.*";
					ofd.Multiselect = true;
					if (ofd.ShowDialog() == DialogResult.OK)
					{
						exceptionFilePaths = ofd.FileNames.ToList();
					}
				}

				foreach (string exceptionFilePath in exceptionFilePaths)
				{
					List<string> cmds = new List<string>();
					string[] lines = File.ReadAllLines(exceptionFilePath);
					for (int i = 0; i < lines.Length; ++i)
					{
						if (lines[i].StartsWith("- [Cmd] - "))
						{
							cmds.Add(lines[i].Replace("- [Cmd] - ", string.Empty));
						}
						else if (lines[i].StartsWith("INSERT INTO "))
						{
							cmds.Add(lines[i]);
						}
						else if (lines[i].StartsWith("UPDATE "))
						{
							cmds.Add(lines[i]);
						}
					}

					mDatabaseAdapter.SetDatabaseParameters(databaseFilePath, string.Empty, string.Empty, string.Empty, string.Empty);
					if (mDatabaseAdapter.Connect() == true)
					{
						mDatabaseAdapter.ExecuteNonQueryCommands(cmds);
						RecordLogMessage(rtxtLog, "Recovery Done!");
					}
				}
			}
			catch (Exception ex)
			{
				RecordLogMessage(rtxtLog, ex.ToString());
			}
		}

		private void InitializeDgvVehicleInfo()
		{
			DataGridView dgv = dgvVehicleInfo;

			dgv.SelectionChanged += ((sender, e) => dgv.ClearSelection());

			dgv.RowHeadersVisible = false;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToResizeRows = false;
			dgv.AllowUserToResizeColumns = false;
			dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgv.MultiSelect = false;
			dgv.BackgroundColor = Color.FromArgb(53, 53, 53);
			dgv.GridColor = Color.FromArgb(86, 86, 86);
			dgv.BorderStyle = BorderStyle.None;

			dgv.EnableHeadersVisualStyles = false;
			dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold);
			dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204);
			dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
			dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dgv.ColumnHeadersHeight = 30;

			dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
			dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
			dgv.DefaultCellStyle.BackColor = Color.FromArgb(31, 31, 31);
			dgv.DefaultCellStyle.ForeColor = Color.White;
			dgv.RowTemplate.Height = 30;

			dgv.Columns.Add("Timestamp", "Timestamp");
			dgv.Columns[0].Width = 200;
			dgv.Columns.Add("LastUpdate", "LastUpdate");
			dgv.Columns[1].Width = 200;
			dgv.Columns.Add("Name", "Name");
			dgv.Columns[2].Width = 180;
			dgv.Columns.Add("State", "State");
			dgv.Columns[3].Width = 140;
			dgv.Columns.Add("X", "X");
			dgv.Columns[4].Width = 100;
			dgv.Columns.Add("Y", "Y");
			dgv.Columns[5].Width = 100;
			dgv.Columns.Add("Toward", "Toward");
			dgv.Columns[6].Width = 100;
			dgv.Columns.Add("Target", "Target");
			dgv.Columns[7].Width = 200;
			dgv.Columns.Add("Battery", "Battery");
			dgv.Columns[8].Width = 80;
			dgv.Columns.Add("Score", "Score");
			dgv.Columns[9].Width = 80;
			dgv.Columns.Add("PathPointCount", "PathPointCount");
			dgv.Columns[10].Width = 140;
			dgv.Columns.Add("Path", "Path");
			dgv.Columns[11].Width = 200;
			dgv.Columns.Add("FillColumn", "");
			dgv.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
				column.ReadOnly = true;
			}
		}
		private void InitializeTimeComboBox(ComboBox ComboBox)
		{
			ComboBox.Items.Clear();
			ComboBox.Items.Add("00:00");
			ComboBox.Items.Add("01:00");
			ComboBox.Items.Add("02:00");
			ComboBox.Items.Add("03:00");
			ComboBox.Items.Add("04:00");
			ComboBox.Items.Add("05:00");
			ComboBox.Items.Add("06:00");
			ComboBox.Items.Add("07:00");
			ComboBox.Items.Add("08:00");
			ComboBox.Items.Add("09:00");
			ComboBox.Items.Add("10:00");
			ComboBox.Items.Add("11:00");
			ComboBox.Items.Add("12:00");
			ComboBox.Items.Add("13:00");
			ComboBox.Items.Add("14:00");
			ComboBox.Items.Add("15:00");
			ComboBox.Items.Add("16:00");
			ComboBox.Items.Add("17:00");
			ComboBox.Items.Add("18:00");
			ComboBox.Items.Add("19:00");
			ComboBox.Items.Add("20:00");
			ComboBox.Items.Add("21:00");
			ComboBox.Items.Add("22:00");
			ComboBox.Items.Add("23:00");
			ComboBox.SelectedIndex = 0;
		}
		private void InitializeFootprintData(string MapFilePath, string LogFilePath, DateTime StartTimestamp, DateTime EndTimestamp)
		{
			// 初始化設定
			mStartTimestamp = StartTimestamp;
			mEndTimestamp = EndTimestamp;
			mCurrentTimestamp = StartTimestamp;
			tbTimestamp.Value = 0;
			foreach (var pair in mHistoryVehicleInfos)
			{
				EraseVehicleIcon(pair.Key);
				RemoveVehicleInfoDataRow(pair.Key);
			}
			mHistoryVehicleInfos.Clear();

			ReadMapData(MapFilePath);
			ReadLogData(LogFilePath, StartTimestamp, EndTimestamp);
			
			foreach (var pair in mHistoryVehicleInfos.OrderBy(o => o.Key))
			{
				RegisterVehicleIconId(pair.Key);
				AddVehicleInfoDataRow(pair.Key);
			}

			RefreshGui();
		}
		private void InitializeCbLogTableName()
		{
			cbLogTableName.SelectedIndex = -1;
			cbLogTableName.Items.Clear();
			cbLogTableName.Items.AddRange(mTimestampKeywordTable.Keys.ToArray());
		}
		private void InitializeDgvLogTable()
		{
			DataGridView dgv = dgvLogTable;

			dgv.RowHeadersVisible = false;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToResizeRows = false;

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.DefaultCellStyle.Format = column.HeaderText.Contains("Timestamp") ? "yyyy/MM/dd HH:mm:ss.fff" : string.Empty;
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			}
		}
		private void UpdateDgvLogTable()
		{
			DataGridView dgv = dgvLogTable;

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.DefaultCellStyle.Format = column.HeaderText.Contains("Timestamp") ? "yyyy/MM/dd HH:mm:ss.fff" : string.Empty;
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			}
		}
		private void ReadMapData(string MapFilePath)
		{
			// 地圖介面讀取地圖
			gluiCtrl1.LoadMap(MapFilePath);
			gluiCtrl1.AdjustZoom();
			gluiCtrl1.Focus(GLCMD.CMD.MapCenter.X, GLCMD.CMD.MapCenter.Y);
		}
		private void ReadLogData(string LogFilePath, DateTime StartTimestamp, DateTime EndTimestamp)
		{
			if (!File.Exists(LogFilePath)) return;

			// Database 設定與連線
			mDatabaseAdapter.SetDatabaseParameters(LogFilePath, string.Empty, string.Empty, string.Empty, string.Empty);
			if (mDatabaseAdapter.Connect() == true)
			{
				string startTimestamp = $"{StartTimestamp.ToString("yyyy-MM-dd HH:mm:ss")}";
				string endTimestamp = $"{EndTimestamp.ToString("yyyy-MM-dd HH:mm:ss")}";
				string sqlCmd3 = $"SELECT RecordTimestamp,LastUpdateTimestamp,ID,State,X,Y,Toward,Target,BatteryValue,LocationScore,Path FROM HistoryVehicleInfo WHERE RecordTimestamp BETWEEN '{startTimestamp}' AND '{endTimestamp}'";
				string[] historyVehicleInfoStrings = GetHistoryVehicleInfoStrings(mDatabaseAdapter.ExecuteQueryCommand(sqlCmd3));
				for (int i = 0; i < historyVehicleInfoStrings.Length; ++i)
				{
					HistoryVehicleInfo tmpInfo = HistoryVehicleInfo.FromString(historyVehicleInfoStrings[i], new string[] { "#" });
					if (!mHistoryVehicleInfos.ContainsKey(tmpInfo.Name)) mHistoryVehicleInfos.Add(tmpInfo.Name, new List<HistoryVehicleInfo>());
					mHistoryVehicleInfos[tmpInfo.Name].Add(tmpInfo);
				}
			}
		}
		private void ReadLogData(string LogFilePath, string TableName, DateTime StartTimestamp, DateTime EndTimestamp)
		{
			if (!File.Exists(LogFilePath)) return;

			// Database 設定與連線
			mDatabaseAdapter.SetDatabaseParameters(LogFilePath, string.Empty, string.Empty, string.Empty, string.Empty);
			if (mDatabaseAdapter.Connect() == true)
			{
				string startTimestamp = $"{StartTimestamp.ToString("yyyy-MM-dd HH:mm:ss")}";
				string endTimestamp = $"{EndTimestamp.ToString("yyyy-MM-dd HH:mm:ss")}";
				string sqlCmd = $"SELECT * FROM {TableName} WHERE {mTimestampKeywordTable[TableName]} BETWEEN '{startTimestamp}' AND '{endTimestamp}'";
				DataSet dataSet = mDatabaseAdapter.ExecuteQueryCommand(sqlCmd);
				if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
				{
					DataTable dataTable = dataSet.Tables[0];
					dgvLogTable.DataSource = dataTable;
					UpdateDgvLogTable();
				}
			}
		}
		private void RefreshGui()
		{
			// 根據 CurrentTimestamp 更新地圖上的自走車位置
			lblCurrentTimestamp.InvokeIfNecessary(() =>
			{
				lblCurrentTimestamp.Text = mCurrentTimestamp.ToString("yyyy/MM/dd HH:mm:ss");
			});
			foreach (var a in mHistoryVehicleInfos)
			{
				for (int i = a.Value.Count - 1; i >= 0; i--)
				{
					if (a.Value[i].Timestamp <= mCurrentTimestamp)
					{
						PrintVehicleIcon(a.Key, a.Value[i]);
						UpdateVehicleInfoDataRow(a.Key, a.Value[i]);
						break;
					}
				}
			}
		}
		private void RegisterVehicleIconId(string VehicleName)
		{
			// 註冊圖像 ID
			if (!string.IsNullOrEmpty(VehicleName) && !mIconIdsOfVehicle.ContainsKey(VehicleName))
			{
				int VehicleIconId = GLCMD.CMD.SerialNumber.Next();
				int VehiclePathIconId = GLCMD.CMD.AddMultiStripLine("Path", null);
				int VehiclePathPointsIconId = GLCMD.CMD.AddMultiPair("Laser", null);

				mIconIdsOfVehicle.Add(VehicleName, VehicleIconId);
				mIconIdsOfVehiclePath.Add(VehicleName, VehiclePathIconId);
				mIconIdsOfVehicleLaser.Add(VehicleName, VehiclePathPointsIconId);
			}
		}
		private void PrintVehicleIcon(string VehicleName, HistoryVehicleInfo HistoryVehicleInfo)
		{
			// 把圖像加入至地圖
			if (!string.IsNullOrEmpty(VehicleName) && mIconIdsOfVehicle.ContainsKey(VehicleName))
			{
				if (HistoryVehicleInfo != null)
				{
					GLCMD.CMD.AddAGV(mIconIdsOfVehicle[VehicleName], VehicleName, HistoryVehicleInfo.X, HistoryVehicleInfo.Y, HistoryVehicleInfo.Toward);
				}
				if (HistoryVehicleInfo != null && !string.IsNullOrEmpty(HistoryVehicleInfo.Path))
				{
					// 繪製路徑線時要記得繪製自走車到第一個路徑點的線段
					List<IPair> path = new List<IPair>();
					path.Add(new Pair(HistoryVehicleInfo.X, HistoryVehicleInfo.Y));
					path.AddRange(HistoryVehicleInfo.Path2);
					GLCMD.CMD.SaftyEditMultiGeometry<IPair>(mIconIdsOfVehiclePath[VehicleName], true, (line) =>
					{
						line.Clear();
						line.AddRangeIfNotNull(path);
					});
				}
				if (HistoryVehicleInfo != null && !string.IsNullOrEmpty(HistoryVehicleInfo.Laser))
				{
					// do nothing
				}
			}
		}
		private void EraseVehicleIcon(string VehicleName)
		{
			// 把圖像從地圖中移除
			if (!string.IsNullOrEmpty(VehicleName) && mIconIdsOfVehicle.ContainsKey(VehicleName))
			{
				GLCMD.CMD.DeleteAGV(mIconIdsOfVehicle[VehicleName]);
				GLCMD.CMD.DeleteMulti(mIconIdsOfVehiclePath[VehicleName]);
				GLCMD.CMD.DeleteMulti(mIconIdsOfVehicleLaser[VehicleName]);

				mIconIdsOfVehicle.Remove(VehicleName);
				mIconIdsOfVehiclePath.Remove(VehicleName);
				mIconIdsOfVehicleLaser.Remove(VehicleName);
			}
		}
		private void AddVehicleInfoDataRow(string VehicleName)
		{
			dgvVehicleInfo.Rows.Add();
			dgvVehicleInfo.Rows[dgvVehicleInfo.RowCount - 1].Cells["Name"].Value = VehicleName;
		}
		private void UpdateVehicleInfoDataRow(string VehicleName, HistoryVehicleInfo HistoryVehicleInfo)
		{
			for (int i = 0; i < dgvVehicleInfo.Rows.Count; ++i)
			{
				if (dgvVehicleInfo.Rows[i].Cells["Name"].Value.ToString() == VehicleName)
				{
					dgvVehicleInfo.Rows[i].Cells["Timestamp"].Value = HistoryVehicleInfo.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
					dgvVehicleInfo.Rows[i].Cells["LastUpdate"].Value = HistoryVehicleInfo.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss.fff");
					dgvVehicleInfo.Rows[i].Cells["Name"].Value = HistoryVehicleInfo.Name;
					dgvVehicleInfo.Rows[i].Cells["State"].Value = HistoryVehicleInfo.State;
					dgvVehicleInfo.Rows[i].Cells["X"].Value = HistoryVehicleInfo.X;
					dgvVehicleInfo.Rows[i].Cells["Y"].Value = HistoryVehicleInfo.Y;
					dgvVehicleInfo.Rows[i].Cells["Toward"].Value = HistoryVehicleInfo.Toward;
					dgvVehicleInfo.Rows[i].Cells["Target"].Value = HistoryVehicleInfo.Target;
					dgvVehicleInfo.Rows[i].Cells["Battery"].Value = HistoryVehicleInfo.Battery;
					dgvVehicleInfo.Rows[i].Cells["Score"].Value = HistoryVehicleInfo.Score;
					dgvVehicleInfo.Rows[i].Cells["PathPointCount"].Value = HistoryVehicleInfo.Path2.Count;
					dgvVehicleInfo.Rows[i].Cells["Path"].Value = HistoryVehicleInfo.Path;
				}
			}
		}
		private void RemoveVehicleInfoDataRow(string VehicleName)
		{
			for (int i = 0; i < dgvVehicleInfo.Rows.Count; ++i)
			{
				if (dgvVehicleInfo.Rows[i].Cells["Name"].Value != null && dgvVehicleInfo.Rows[i].Cells["Name"].Value.ToString() == VehicleName)
				{
					dgvVehicleInfo.Rows.RemoveAt(i);
					break;
				}
			}
		}

		private static void RecordLogMessage(RichTextBox RichTextBox, string Text)
		{
			RichTextBox.Text += $"[{DateTime.Now.ToString("HH:mm:ss.fff")}] ";
			RichTextBox.Text += Text + "\r\n";
			RichTextBox.ScrollToCaret();
		}
		private static void ClearLogMessage(RichTextBox RichTextBox)
		{
			RichTextBox.Clear();
		}
		private static string[] GetVehicleTableNames(DataSet DataSet)
		{
			List<string> result = new List<string>();
			if (DataSet != null && DataSet.Tables != null && DataSet.Tables.Count > 0)
			{
				DataTable dataTable = DataSet.Tables[0];
				if (dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; ++i)
					{
						result.Add(dataTable.Rows[i].ItemArray.First().ToString());
					}
				}
			}
			return result.ToArray();
		}
		private static string GetVehicleName(string VehicleTableName)
		{
			return VehicleTableName.Replace("HistoryVehicleInfoOf", string.Empty).Replace("Dash", "-");
		}
		private static string[] GetHistoryVehicleInfoStrings(DataSet DataSet)
		{
			List<string> result = new List<string>();
			if (DataSet != null && DataSet.Tables != null && DataSet.Tables.Count > 0)
			{
				DataTable dataTable = DataSet.Tables[0];
				if (dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; ++i)
					{
						result.Add(string.Join("#", dataTable.Rows[i].ItemArray.Select(o => o.ToString())));
					}
				}
			}
			return result.ToArray();
		}

		private Thread AutoDisplay_HistoryInfo;
		ManualResetEvent _pauseEvent;


		private void btnPlayHistoryvideo_Click(object sender, EventArgs e)
        {
            if (AutoDisplay_HistoryInfo == null)
            {
				
				_pauseEvent = new ManualResetEvent(true);
				AutoDisplay_HistoryInfo = new Thread(() =>
				{
					while (true)
					{
						
						_pauseEvent.WaitOne(Timeout.Infinite);

						ttt();

						Thread.Sleep(200);
					}

				});
				AutoDisplay_HistoryInfo.Name = "test";
				AutoDisplay_HistoryInfo.IsBackground = true;
				AutoDisplay_HistoryInfo.Start();
			}
            else
            {
				_pauseEvent.Set();
			}
		}

		private void ttt()
        {
            if (mCurrentTimestamp != mEndTimestamp)
            {
				
				mCurrentTimestamp = mCurrentTimestamp.AddSeconds(1);
				tbTimestamp.InvokeIfNecessary(()=> { tbTimestamp.Value += 1; });
				RefreshGui();
			}
		}

        private void btnPauseHistoryvideo_Click(object sender, EventArgs e)
        {
			_pauseEvent.Reset();
		}

        
    }






    public class HistoryVehicleInfo
	{
		public DateTime Timestamp { get; private set; } = DateTime.MinValue;
		public DateTime LastUpdate { get; private set; } = DateTime.MinValue;
		public string Name { get; private set; } = string.Empty;
		public string State { get; private set; } = string.Empty;
		public int X { get; private set; } = 0;
		public int Y { get; private set; } = 0;
		public int Toward { get; private set; } = 0;
		public string Target { get; private set; } = string.Empty;
		public int Battery { get; private set; } = 0;
		public int Score { get; private set; } = 0;
		public string Path { get; private set; } = string.Empty;
		public List<IPair> Path2
		{
			get
			{
				List<IPair> result = new List<IPair>();
				if (!string.IsNullOrEmpty(Path))
				{
					string[] tmp = Path.Split(new string[] { "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < tmp.Length; i += 2)
					{
						result.Add(new Pair(int.Parse(tmp[i]), int.Parse(tmp[i + 1])));
					}
				}
				return result;
			}
		}
		public string Laser { get; private set; } = string.Empty;

		public HistoryVehicleInfo(DateTime Timestamp, DateTime LastUpdate, string Name, string State, int X, int Y, int Toward, string Target, int Battery, int Score, string Path, string Laser)
		{
			Set(Timestamp, LastUpdate, Name, State, X, Y, Toward, Target, Battery, Score, Path, Laser);
		}
		public void Set(DateTime Timestamp, DateTime LastUpdate, string Name, string State, int X, int Y, int Toward, string Target, int Battery, int Score, string Path, string Laser)
		{
			this.Timestamp = Timestamp;
			this.LastUpdate = LastUpdate;
			this.Name = Name;
			this.State = State;
			this.X = X;
			this.Y = Y;
			this.Toward = Toward;
			this.Target = Target;
			this.Battery = Battery;
			this.Score = Score;
			this.Path = Path;
			this.Laser = Laser;
		}
		public override string ToString()
		{
			return $"{Name}/{State}/{Target}";
		}
		public static HistoryVehicleInfo FromString(string String, string[] Seperator)
		{
			string[] data = String.Split(Seperator, StringSplitOptions.None);
			return new HistoryVehicleInfo(DateTime.Parse(data[0]), DateTime.Parse(data[1]), data[2], data[3], int.Parse(data[4]), int.Parse(data[5]), (int)double.Parse(data[6]), data[7], (int)double.Parse(data[8]), (int)double.Parse(data[9]), data[10], string.Empty);
		}
	}
}
