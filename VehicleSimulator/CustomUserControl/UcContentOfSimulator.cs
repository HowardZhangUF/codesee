using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace VehicleSimulator
{
	public partial class UcContentOfSimulator : UserControl
	{
		private SimulatorProcessContainer rCore = null;
		private Dictionary<string, UcSimulatorShortcut> mSimulatorShortcutCollection = new Dictionary<string, UcSimulatorShortcut>();
		private Dictionary<string, UcSimulatorInfo> mSimulatorInfoCollection = new Dictionary<string, UcSimulatorInfo>();
		private string mCurrentDisplayedSimulatorName = string.Empty;
		private int mStartIndexOfName = 1;

		public UcContentOfSimulator()
		{
			InitializeComponent();
		}
		public void Set(SimulatorProcessContainer SimulatorProcessContainer)
		{
			if (SimulatorProcessContainer != null)
			{
				UnsubscribeEvent_SimulatorProcessContainer(rCore);
				rCore = SimulatorProcessContainer;
				SubscribeEvent_SimulatorProcessContainer(rCore);
			}
		}

		private void btnAddSimulator_Click(object sender, EventArgs e)
		{
			int i = mStartIndexOfName;
			string prefix = "Simulator";
			while (true)
			{
				string simulatorName = prefix + i.ToString().PadLeft(3, '0');
				if (!rCore.IsSimulatorProcessExist(simulatorName))
				{
					rCore.AddSimulatorProcess(simulatorName);
					break;
				}
				else
				{
					i++;
				}
				
			}
			

			if (string.IsNullOrEmpty(mCurrentDisplayedSimulatorName))
			{
				UpdateGui_ChangeDisplaySimulator(mSimulatorShortcutCollection.First().Key);
			}

		}
		private void btnTxt_Click(object sender, EventArgs e)
		{


			TxtInfo rTxtInfo = new TxtInfo();
			List<string> ListReadLine = new List<string>(); 
			string TxtFilePath = rTxtInfo.TxtChoose();

			if (TxtFilePath != null)
			{
				ListReadLine = rTxtInfo.TxtRead(TxtFilePath);
				int index = 0;//用於List讀取
				int i = mStartIndexOfName;//用於車輛創建
				bool ContinueLoop = true;

				while (ContinueLoop)
                {
					var StringReadLine = ListReadLine[index];
					switch(StringReadLine)
                    {
						case "Simulator:": //TO DO:新增車子數量
							index++;
							StringReadLine= ListReadLine[index];
							for(int count=0;count< Convert.ToInt32(StringReadLine); count++)
                            {
								string prefix = "Simulator";
								while (true)
								{
									string simulatorName = prefix + i.ToString().PadLeft(3, '0');
									if (!rCore.IsSimulatorProcessExist(simulatorName))
									{
										rCore.AddSimulatorProcess(simulatorName);
										break;
									}
									else
									{
										i++;
									}

								}


								if (string.IsNullOrEmpty(mCurrentDisplayedSimulatorName))
								{
									UpdateGui_ChangeDisplaySimulator(mSimulatorShortcutCollection.First().Key);
								}


							}

							index++;
							break;

						case "Next:":  //TO DO:將迴圈座標放進各個車中
							index++;
							StringReadLine = ListReadLine[index];


							index++;
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

		private void btnRemoveSimulator_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(mCurrentDisplayedSimulatorName))
			{
				rCore.RemoveSimulatorProcess(mCurrentDisplayedSimulatorName);
				if (mSimulatorShortcutCollection.Count > 0)
				{
					UpdateGui_ChangeDisplaySimulator(mSimulatorShortcutCollection.First().Key);
				}
				else
				{
					UpdateGui_ChangeDisplaySimulator(string.Empty);
				}
			}
		}
		private void numStartIndexOfName_ValueChanged(object sender, EventArgs e)
		{
			mStartIndexOfName = (int)numStartIndexOfName.Value;
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
		private void HandleEvent_SimulatorProcessContainerSimulatorAdded(object sender, SimulatorAddedEventArgs e)
		{
			string simulatorName = e.SimulatorName;

			UcSimulatorShortcut newShortcut = new UcSimulatorShortcut(e.SimulatorProcess) { Dock = DockStyle.Top, Height = 80 };
			mSimulatorShortcutCollection.Add(simulatorName, newShortcut);
			mSimulatorShortcutCollection[simulatorName].Click += HandleEvent_UcSimulatorShortcutClick;
			pnlMenu.Controls.Add(mSimulatorShortcutCollection[simulatorName]);
			List<string> simulatorNames = mSimulatorShortcutCollection.Keys.OrderBy(o => o).ToList();
			for (int i = 0; i < simulatorNames.Count; ++i)
			{
				pnlMenu.Controls.SetChildIndex(mSimulatorShortcutCollection[simulatorNames[i]], simulatorNames.Count - 1 - i);
			}

			UcSimulatorInfo newInfo = new UcSimulatorInfo(e.SimulatorProcess) { Dock = DockStyle.Fill };
			mSimulatorInfoCollection.Add(simulatorName, newInfo);
			pnlContent.Controls.Add(mSimulatorInfoCollection[simulatorName]);
		}
		private void HandleEvent_SimulatorProcessContainerSimulatorRemoved(object sender, SimulatorRemovedEventArgs e)
		{
			string simulatorName = e.SimulatorName;

			pnlMenu.Controls.Remove(mSimulatorShortcutCollection[simulatorName]);
			mSimulatorShortcutCollection[simulatorName].Click -= HandleEvent_UcSimulatorShortcutClick;
			mSimulatorShortcutCollection.Remove(simulatorName);

			pnlContent.Controls.Remove(mSimulatorInfoCollection[simulatorName]);
			mSimulatorInfoCollection.Remove(simulatorName);
		}
		private void HandleEvent_UcSimulatorShortcutClick(object sender, EventArgs e)
		{
			string currentClickedSimulatorName = (sender as UcSimulatorShortcut).GetCurrentSimulatorName();
			UpdateGui_ChangeDisplaySimulator(currentClickedSimulatorName);
		}
		private void UpdateGui_ChangeDisplaySimulator(string SimulatorName)
		{
			if (mCurrentDisplayedSimulatorName != SimulatorName)
			{
				mCurrentDisplayedSimulatorName = SimulatorName;
				foreach (UcSimulatorShortcut ctrl in mSimulatorShortcutCollection.Values)
				{
					ctrl.SetBackColor(Color.FromArgb(67, 67, 67));
				}
				if (!string.IsNullOrEmpty(mCurrentDisplayedSimulatorName))
				{
					mSimulatorShortcutCollection[mCurrentDisplayedSimulatorName].SetBackColor(Color.FromArgb(18, 78, 103));
					mSimulatorInfoCollection[mCurrentDisplayedSimulatorName].BringToFront();
				}
			}
		}
    }
}
