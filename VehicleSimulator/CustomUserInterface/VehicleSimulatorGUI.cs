using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleSimulator
{
	public enum Language
	{
		EnUs,
		ZhTw
	}

	public partial class VehicleSimulatorGUI : Form
	{
		private Color mColorOfMenuSelected = Color.FromArgb(41, 41, 41);
		private Color mColorOfMenuUnselected = Color.FromArgb(28, 28, 28);

		private SimulatorProcessContainer mCore = new SimulatorProcessContainer();

		public VehicleSimulatorGUI()
		{
			InitializeComponent();
		}

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr a, int msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		protected void Constructor()
		{
			UnsubscribeEvent_SimulatorProcessContainer(mCore);
			mCore = new SimulatorProcessContainer();
			SubscribeEvent_SimulatorProcessContainer(mCore);

			ucContentOfSimulator1.Set(mCore);
			ucContentOfSetting1.Set(mCore);
			btnMenuOfSimulator_Click(null, null);
		}
		protected void Destructor()
		{
			mCore.Clear();
			UnsubscribeEvent_SimulatorProcessContainer(mCore);
			mCore = null;
		}

        private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
        {
			Constructor();
		}
		private void VehicleSimulatorGUI_FormClosing(object sender, FormClosingEventArgs e)
		{

		}
		private void ctrlTitle_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = e.Button == MouseButtons.Left;
			if (flag)
			{
				ReleaseCapture();
				SendMessage(Handle, 0x112, 0xf012, 0);
			}
		}
		private void btnMinimizeProgram_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}
		private void btnCloseProgram_Click(object sender, EventArgs e)
		{
			Destructor();
			Task.Run(() => 
			{
				formProgress frm = new formProgress();
				frm.StartPosition = FormStartPosition.CenterParent;
				frm.SetTitleText("Program Closing ...");
				frm.Show();
				Application.DoEvents();
				int i = 0;
				while (i <= 20)
				{
					frm.SetProgressValue(i * 5);
					Application.DoEvents();
					System.Threading.Thread.Sleep(100);
					i++;
				}
				frm.Close();
				pnlTitle.InvokeIfNecessary(() => { Close(); });
			});
		}
        private void btnMenuOfSimulator_Click(object sender, EventArgs e)
        {
            btnMenuOfSimulator.BackColor = mColorOfMenuSelected;
            btnMenuOfConsole.BackColor = mColorOfMenuUnselected;
            btnMenuOfSetting.BackColor = mColorOfMenuUnselected;
            btnMenuOfAbout.BackColor = mColorOfMenuUnselected;
            ucContentOfSimulator1.BringToFront();
        }
        private void btnMenuOfConsole_Click(object sender, EventArgs e)
        {
            btnMenuOfSimulator.BackColor = mColorOfMenuUnselected;
            btnMenuOfConsole.BackColor = mColorOfMenuSelected;
            btnMenuOfSetting.BackColor = mColorOfMenuUnselected;
            btnMenuOfAbout.BackColor = mColorOfMenuUnselected;
            ucContentOfConsole1.BringToFront();
        }
        private void btnMenuOfSetting_Click(object sender, EventArgs e)
        {
            btnMenuOfSimulator.BackColor = mColorOfMenuUnselected;
            btnMenuOfConsole.BackColor = mColorOfMenuUnselected;
            btnMenuOfSetting.BackColor = mColorOfMenuSelected;
            btnMenuOfAbout.BackColor = mColorOfMenuUnselected;
            ucContentOfSetting1.BringToFront();
        }
        private void btnMenuOfAbout_Click(object sender, EventArgs e)
        {
            btnMenuOfSimulator.BackColor = mColorOfMenuUnselected;
            btnMenuOfConsole.BackColor = mColorOfMenuUnselected;
            btnMenuOfSetting.BackColor = mColorOfMenuUnselected;
            btnMenuOfAbout.BackColor = mColorOfMenuSelected;
            ucContentOfAbout1.BringToFront();
		}


		//.txt檔   文件內文規格說明
		//
		//(Simulator:)=創建模擬車數量
		//(Map:)=載入地圖檔路徑 (功能施工中)
		//(Next:)=對各別模擬車輸入座標 (可輸出至btnSetAndMove 但無作用)
		//(End) or null =結束讀取.txt檔

		private void btnMeanOfReadTxtData_Click(object sender, EventArgs e)  //TODO:方法待封裝
		{
			TxtDataReadingInfo rTxtDataReadingInfo = new TxtDataReadingInfo();
			List<string> ListOfReadLine = new List<string>();
			string TxtFilePath = rTxtDataReadingInfo.TxtDataChoose();

			if (TxtFilePath != null)
			{
				ListOfReadLine = rTxtDataReadingInfo.TxtDataRead(TxtFilePath);

				int List_index = 0;             //用於List讀取
				bool ContinueLoop = true;       //用於是否結束讀取.txt
				int SimulatorName_index = 1;    //用於寫入SetAndMoveTextBox判斷
				string simulatorName;
				string prefix = "Simulator";
				

				while (ContinueLoop)
				{
					var OneLine = ListOfReadLine[List_index];
					switch (OneLine)
					{
						case "Simulator:":

							List_index++;
							OneLine = ListOfReadLine[List_index];
							int TotalOfSimulator = Convert.ToInt32(OneLine);
							ucContentOfSimulator1.AddSimulators(TotalOfSimulator);
							List_index++;

							break;

						case "MapPath:":

							List_index++;
							OneLine = ListOfReadLine[List_index];
							ucContentOfSetting1.SetMapFolder(OneLine);
							List_index++;

							break;

						case "Next:":

							List_index++;
							OneLine = ListOfReadLine[List_index];
							simulatorName = null;
							simulatorName = prefix + SimulatorName_index.ToString().PadLeft(3, '0');
							ucContentOfSimulator1.PushToSetAndMoveTextBox(simulatorName, OneLine);
							SimulatorName_index++;
							List_index++;

							break;

						case "End":

							ContinueLoop = false;

							break;

						case null:

							ContinueLoop = false;

							break;


					}



				}
			}
		}

		private void SubscribeEvent_SimulatorProcessContainer(SimulatorProcessContainer SimulatorProcessContainer)
		{
			if (SimulatorProcessContainer != null)
			{
				SimulatorProcessContainer.SimulatorAdded += HandleEvent_SimulatorProcessContainerSimulatorAdded;
				SimulatorProcessContainer.SimulatorRemoved += HandleEvent_SimulatorProcessContainerSimulatorRemoved;
			}
		}
		private void UnsubscribeEvent_SimulatorProcessContainer(SimulatorProcessContainer SimulatorProcessContainer)
		{
			if (SimulatorProcessContainer != null)
			{
				SimulatorProcessContainer.SimulatorAdded -= HandleEvent_SimulatorProcessContainerSimulatorAdded;
				SimulatorProcessContainer.SimulatorRemoved -= HandleEvent_SimulatorProcessContainerSimulatorRemoved;
			}
		}
		private void SubscribeEvent_SimulatorProcess(SimulatorProcess SimulatorProcess)
		{
			if (SimulatorProcess != null)
			{
				SimulatorProcess.DebugMessage += HandleEvent_SimulatorProcessDebugMessage;
			}
		}
		private void UnsubscribeEvent_SimulatorProcess(SimulatorProcess SimulatorProcess)
		{
			if (SimulatorProcess != null)
			{
				SimulatorProcess.DebugMessage -= HandleEvent_SimulatorProcessDebugMessage;
			}
		}
		private void HandleEvent_SimulatorProcessContainerSimulatorAdded(object sender, SimulatorAddedEventArgs e)
		{
			SubscribeEvent_SimulatorProcess(e.SimulatorProcess);
		}
		private void HandleEvent_SimulatorProcessContainerSimulatorRemoved(object sender, SimulatorRemovedEventArgs e)
		{
			UnsubscribeEvent_SimulatorProcess(e.SimulatorProcess);
		}
		private void HandleEvent_SimulatorProcessDebugMessage(object sender, DebugMessageEventArgs e)
		{
			ucContentOfConsole1.AddLog(e.OccurTime, $"[{e.Category}] [{e.SubCategory}] {e.Message}");
		}

        
    }
}
