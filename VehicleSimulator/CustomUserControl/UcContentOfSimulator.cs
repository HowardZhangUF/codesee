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

		/// <summary>
		/// 用於新增多台模擬車車輛
		/// </summary>
		/// <param name="TotalOfSimulator">模擬車車輛數量</param>
		public void AddSimulators(int TotalOfSimulator)  //For .txt data reading (in VehicleSimulatorGUI.cs)
		{
			string simulatorName;
			string prefix = "Simulator";

			for (int index = 1; index < (TotalOfSimulator+1); index++)
			{
				while (true)
				{
					simulatorName = prefix + index.ToString().PadLeft(3, '0');
					if (!rCore.IsSimulatorProcessExist(simulatorName))
					{
						rCore.AddSimulatorProcess(simulatorName);
						break;
					}
					else
					{
						index++;
					}

				}


				if (string.IsNullOrEmpty(mCurrentDisplayedSimulatorName))
				{
					UpdateGui_ChangeDisplaySimulator(mSimulatorShortcutCollection.First().Key);
				}


			}
		}

		/// <summary>
		/// 用於更改該模擬車SetAndMoveTextBox之Value
		/// </summary>
		/// <param name="SimulatorName">模擬車名稱</param>
		/// <param name="ValueOfSetAndMoveTextBox"></param>
		public void PushToSetAndMoveTextBox(string SimulatorName,string ValueOfSetAndMoveTextBox)   //For .txt data reading (in VehicleSimulatorGUI.cs)
		{
			UpdateGui_ChangeDisplaySimulator(SimulatorName);
			UpdateGui_ChangeSimulatorInfo(SimulatorName, ValueOfSetAndMoveTextBox);
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
		private void UpdateGui_ChangeDisplaySimulator(string SimulatorName) //換車視窗顯示  Ex:Simulator002 視窗 => Simulator001 視窗
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
		private void UpdateGui_ChangeSimulatorInfo(string SimulatorName,string ValueOfTextBoxChange) //SimulatorName之SetAndMoveTextBox   
		{
			if (mCurrentDisplayedSimulatorName != SimulatorName)
			{
				mCurrentDisplayedSimulatorName = SimulatorName;

				mSimulatorInfoCollection[mCurrentDisplayedSimulatorName].SetTextBoxValue = ValueOfTextBoxChange;
			}
            else
            {
				mSimulatorInfoCollection[mCurrentDisplayedSimulatorName].SetTextBoxValue = ValueOfTextBoxChange;

			}
		}

        
    }
}
