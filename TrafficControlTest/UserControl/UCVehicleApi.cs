﻿using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.UserControl
{
	public partial class UcVehicleApi : System.Windows.Forms.UserControl
	{
		public string CurrentVehicleName
		{
			get
			{
				string result = null;
				cbVehicleNameList.InvokeIfNecessary(() =>
				{
					result = cbVehicleNameList.SelectedItem == null ? string.Empty : cbVehicleNameList.SelectedItem.ToString();
				});
				return result;
			}
		}

		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IMapFileManager rMapFileManager = null;
		private IMapManager rMapManager = null;

		public UcVehicleApi()
		{
			InitializeComponent();

			foreach (Control ctrl in tableLayoutPanel1.Controls)
			{
				if (ctrl is Button && ctrl.Name.StartsWith("btnVehicle"))
				{
					(ctrl as Button).Click += btn_Click;
				}
			}

			txtCoordinate1.SetHintText("X,Y or X,Y,Head");
			txtCoordinate2.SetHintText("X,Y");
			txtCoordinate1.KeyPress += ((sender, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '-')) e.Handled = true; });
			txtCoordinate2.KeyPress += ((sender, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '-')) e.Handled = true; });

			btnVehicleGoto.Click += btnVehicleGoto_Click;
			btnVehicleGotoPoint.Click += btnVehicleGotoPoint_Click;
			btnVehicleDock.Click += btnVehicleDock_Click;
			btnVehicleStop.Click += btnVehicleStop_Click;
			btnVehicleInsertMovingBuffer.Click += btnVehicleInsertMovingBuffer_Click;
			btnVehicleRemoveMovingBuffer.Click += btnVehicleRemoveMovingBuffer_Click;
			btnVehiclePause.Click += btnVehiclePause_Click;
			btnVehicleResume.Click += btnVehicleResume_Click;
			btnVehicleRequestMapList.Click += btnVehicleRequestMapList_Click;
			btnVehicleGetMap.Click += btnVehicleGetMap_Click;
			btnVehicleUploadMap.Click += btnVehicleUploadMap_Click;
			btnVehicleChangeMap.Click += btnVehicleChangeMap_Click;
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehcileInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IMapFileManager MapFileManager)
		{
			UnsubscribeEvent_IMapFileManager(rMapFileManager);
			rMapFileManager = MapFileManager;
			SubscribeEvent_IMapFileManager(rMapFileManager);
		}
		public void Set(IMapManager MapManager)
		{
			UnsubscribeEvent_IMapManager(rMapManager);
			rMapManager = MapManager;
			SubscribeEvent_IMapManager(rMapManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator, IMapFileManager MapFileManager, IMapManager MapManager)
		{
			Set(VehicleInfoManager);
			Set(VehicleCommunicator);
			Set(MapFileManager);
			Set(MapManager);
		}
		public void UpdateVehicleNameList(string[] VehicleNameList)
		{
			this.InvokeIfNecessary(() =>
			{
				if (VehicleNameList == null || VehicleNameList.Count() == 0)
				{
					cbVehicleNameList.Items.Clear();
				}
				else
				{
					string lastSelectedItemText = cbVehicleNameList.SelectedItem != null ? cbVehicleNameList.SelectedItem.ToString() : string.Empty;
					cbVehicleNameList.SelectedIndex = -1;
					cbVehicleNameList.Items.Clear();
					cbVehicleNameList.Items.AddRange(VehicleNameList.OrderBy((o) => o).ToArray());
					if (!string.IsNullOrEmpty(lastSelectedItemText))
					{
						for (int i = 0; i < cbVehicleNameList.Items.Count; ++i)
						{
							if (lastSelectedItemText == cbVehicleNameList.Items[i].ToString())
							{
								cbVehicleNameList.SelectedIndex = i;
								break;
							}
						}
					}
				}
			});
		}
		public void UpdateGoalNameList(string[] GoalNameList)
		{
			this.InvokeIfNecessary(() =>
			{
				if (GoalNameList == null || GoalNameList.Count() == 0)
				{
					cbGoalNameList.Items.Clear();
				}
				else
				{
					cbGoalNameList.Items.Clear();
					cbGoalNameList.Items.AddRange(GoalNameList.OrderBy((o) => o).ToArray());
				}
			});
		}
		public void UpdateRemoteMapNameList(string[] MapNameList)
		{
			this.InvokeIfNecessary(() =>
			{
				if (MapNameList == null || MapNameList.Count() == 0)
				{
					cbRemoteMapNameList1.Items.Clear();
					cbRemoteMapNameList1.AdjustDropDownWidth();
					cbRemoteMapNameList2.Items.Clear();
					cbRemoteMapNameList2.AdjustDropDownWidth();
				}
				else
				{
					cbRemoteMapNameList1.Items.Clear();
					cbRemoteMapNameList1.Items.AddRange(MapNameList.OrderBy((o) => o).ToArray());
					cbRemoteMapNameList1.AdjustDropDownWidth();
					cbRemoteMapNameList2.Items.Clear();
					cbRemoteMapNameList2.Items.AddRange(MapNameList.OrderBy((o) => o).ToArray());
					cbRemoteMapNameList2.AdjustDropDownWidth();
				}
			});
		}
		public void UpdateLocalMapNameList(string[] MapNameList)
		{
			this.InvokeIfNecessary(() =>
			{
				if (MapNameList == null || MapNameList.Count() == 0)
				{
					cbLocalMapNameList.Items.Clear();
					cbLocalMapNameList.AdjustDropDownWidth();
				}
				else
				{
					cbLocalMapNameList.Items.Clear();
					cbLocalMapNameList.Items.AddRange(MapNameList.OrderBy((o) => o).ToArray());
					cbLocalMapNameList.AdjustDropDownWidth();
				}
			});
		}

		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehcileInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{

			}
		}
		private void SubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.MapFileAdded += HandleEvent_MapFileManagerMapFileAdded;
			}
		}
		private void UnsubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.MapFileAdded -= HandleEvent_MapFileManagerMapFileAdded;
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapLoaded += HandleEvent_MapManagerMapLoaded;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapLoaded -= HandleEvent_MapManagerMapLoaded;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateVehicleNameList(rVehicleInfoManager.GetItemNames().ToArray());
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateVehicleNameList(rVehicleInfoManager.GetItemNames().ToArray());
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			if (!string.IsNullOrEmpty(CurrentVehicleName) && Args.StatusName.Contains("CurrentMapNameList"))
			{
				UpdateRemoteMapNameList(rVehicleInfoManager.GetItem(CurrentVehicleName).mCurrentMapNameList.ToArray());
				UpdateLocalMapNameList(rMapFileManager.GetLocalMapFileNameList());
			}
		}
		private void HandleEvent_MapFileManagerMapFileAdded(object Sender, MapFileCountChangedEventArgs Args)
		{
			UpdateLocalMapNameList(rMapFileManager.GetLocalMapFileNameList());
		}
		private void HandleEvent_MapManagerMapLoaded(object Sender, LoadMapSuccessedEventArgs Args)
		{
			UpdateGoalNameList(rMapManager.GetGoalNameList());
		}
		private void btn_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem == null)
			{
				CustomMessageBox.OutputBox("You Have to Choose \"Vehicle\" First!");
			}
		}
		private void btnVehicleGoto_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && cbGoalNameList.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_Goto(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, cbGoalNameList.SelectedItem.ToString());
			}
		}
		private void btnVehicleGotoPoint_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && !string.IsNullOrEmpty(txtCoordinate1.Text))
			{
				string[] datas = txtCoordinate1.Text.Split(',');
				if (datas.Length == 2)
				{
					rVehicleCommunicator.SendSerializableData_GotoPoint(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, int.Parse(datas[0]), int.Parse(datas[1]));
				}
				else if (datas.Length == 3)
				{
					rVehicleCommunicator.SendSerializableData_GotoTowardPoint(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, int.Parse(datas[0]), int.Parse(datas[1]), int.Parse(datas[2]));
				}
			}
		}
		private void btnVehicleDock_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{

			}
		}
		private void btnVehicleStop_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_Stop(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort);
			}
		}
		private void btnVehicleInsertMovingBuffer_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && !string.IsNullOrEmpty(txtCoordinate2.Text))
			{
				string[] datas = txtCoordinate2.Text.Split(',');
				if (datas.Length == 2)
				{
					rVehicleCommunicator.SendSerializableData_InsertMovingBuffer(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, int.Parse(datas[0]), int.Parse(datas[1]));
				}
			}
		}
		private void btnVehicleRemoveMovingBuffer_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_RemoveMovingBuffer(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort);
			}
		}
		private void btnVehiclePause_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_PauseMoving(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort);
			}
		}
		private void btnVehicleResume_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_ResumeMoving(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort);
			}
		}
		private void btnVehicleRequestMapList_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_RequestMapList(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort);
			}
		}
		private void btnVehicleGetMap_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && cbRemoteMapNameList1.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_GetMap(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, cbRemoteMapNameList1.SelectedItem.ToString().Replace("*", string.Empty));
			}
		}
		private void btnVehicleUploadMap_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && cbLocalMapNameList.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_UploadMapToAGV(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, rMapFileManager.GetMapFileFullPath(cbLocalMapNameList.SelectedItem.ToString()));
			}
		}
		private void btnVehicleChangeMap_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && cbRemoteMapNameList2.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_ChangeMap(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, cbRemoteMapNameList2.SelectedItem.ToString().Replace("*", string.Empty));
			}
		}
		private void cbVehicleNameList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(CurrentVehicleName))
			{
				UpdateRemoteMapNameList(rVehicleInfoManager.GetItem(CurrentVehicleName).mCurrentMapNameList.ToArray());
				UpdateLocalMapNameList(rMapFileManager.GetLocalMapFileNameList());
			}
		}
	}
}
