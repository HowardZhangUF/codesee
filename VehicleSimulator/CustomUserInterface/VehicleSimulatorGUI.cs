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



		private void btnMeanOfReadTxtData_Click(object sender, EventArgs e)  //TODO:方法待封裝
		{
			try
			{
                TxtDataReadingInfo rTxtDataReadingInfo = new TxtDataReadingInfo();
                List<string> ListOfReadLine = new List<string>();
                string TxtFilePath = rTxtDataReadingInfo.Choose_Txt_Data_Path();
				bool TxtDataCheckingMode = false;//用於確認是否繼續讀取文件內容

				if (TxtFilePath != null)
				{
					ListOfReadLine = rTxtDataReadingInfo.Read_Txt_Data(TxtFilePath);
					TxtDataCheckingMode = true;
				}

				int NameList_index = 0;
                int List_index = 0;//用於List讀取
				bool SetMapPath = false;
				String simulatorName;

				var OneLine = ListOfReadLine[List_index];
				
				


				while (TxtDataCheckingMode)
				{
					OneLine = ListOfReadLine[List_index];

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
							SetMapPath = true;
							List_index++;

							break;

						case "Next:":

							List_index++;
							OneLine = ListOfReadLine[List_index];
							var NameList = ucContentOfSimulator1.GetSimulatorShortcutCollectionAllKey;
							simulatorName =NameList[NameList_index];
							ucContentOfSimulator1.PushToSetAndMoveTextBox(simulatorName, OneLine);
							NameList_index++;
							List_index++;

							break;

						case "End":
							TxtDataCheckingMode = false;

							break;

						case null:
							TxtDataCheckingMode = false;

							break;

						default:
							MessageBox.Show("請輸入正確的關鍵字以自動建立模擬車");
							TxtDataCheckingMode = false;

							break;
					}
				}

				if (SetMapPath == false)
				{
					string MapPath = System.IO.Path.GetDirectoryName(TxtFilePath);
					ucContentOfSetting1.SetMapFolder(MapPath);
				}

				ucContentOfSimulator1.UpdateGui_ChangeDisplaySimulator(ucContentOfSimulator1.GetmSimulatorShortcutCollectionFirstKey);
            }
            catch(KeyNotFoundException) {MessageBox.Show("KeyNotFoundException");}
			catch(Exception Ex)			{ExceptionHandling.HandleException(Ex);}
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
