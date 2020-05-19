﻿using GLCore;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.Map
{
	public class MapManager : SystemWithConfig, IMapManager
	{
		public event EventHandlerMapFileName MapLoaded;
		public event EventHandlerVehicleNamesMapFileName VehicleCurrentMapSynchronized;

		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMapFileManager rMapFileManager = null;
		private string mCurrentMapName = string.Empty;
		private string mCurrentMapHash = string.Empty;
		private bool mAutoLoadMap = false;
		private bool mIsDownloadingMapFile { get { return (mMapFileNamesOfDownloading.Count > 0); } }
		private IList<string> mMapFileNamesOfDownloading { get; } = new List<string>();

		public MapManager(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMapFileManager MapFileManager)
		{
			Set(VehicleCommunicator, VehicleInfoManager, MapFileManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IMapFileManager MapFileManager)
		{
			UnsubscribeEvent_IMapFileManager(rMapFileManager);
			rMapFileManager = MapFileManager;
			SubscribeEvent_IMapFileManager(rMapFileManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMapFileManager MapFileManager)
		{
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
			Set(MapFileManager);
		}
		public void LoadMap(string MapFileName)
		{
			GLCMD.CMD.LoadMap(rMapFileManager.GetMapFileFullPath(MapFileName), 3);
			mCurrentMapName = MapFileName;
			mCurrentMapHash = MD5HashCalculator.CalculateFileHash(rMapFileManager.GetMapFileFullPath(MapFileName));
			RaiseEvent_MapLoaded(MapFileName);
		}
		public void LoadMap2(string MapFileNameWithoutExtension)
		{
			LoadMap(MapFileNameWithoutExtension + ".map");
		}
		public string GetCurrentMapFileName()
		{
			return mCurrentMapName;
		}
		public string GetCurrentMapFileNameWithoutExtension()
		{
			return GetCurrentMapFileName().Replace(".map", string.Empty);
		}
		public string GetCurrentMapFileHash()
		{
			return mCurrentMapHash;
		}
		public string[] GetGoalNameList()
		{
			if (string.IsNullOrEmpty(mCurrentMapName))
			{
				return new string[0];
			}
			else
			{
				return GLCMD.CMD.SingleTowerPairInfo.Select(o => o.Name).ToArray();
			}
		}
		public int[] GetGoalCoordinate(string GoalName)
		{
			if (GLCMD.CMD.SingleTowerPairInfo.Any(o => o.Name == GoalName))
			{
				var goalData = GLCMD.CMD.SingleTowerPairInfo.First(o => o.Name == GoalName);
				return new int[] { goalData.X, goalData.Y, (int)goalData.Toward };
			}
			else
			{
				return new int[] { 0, 0, 0 };
			}
		}
		public void SynchronizeVehicleCurrentMap(string MapFileName)
		{
			IEnumerable<string> vehicleNames = rVehicleInfoManager.GetItemNames();
			if (vehicleNames != null && vehicleNames.Count() > 0)
			{
				foreach (string vehicleName in vehicleNames)
				{
					rVehicleCommunicator.SendSerializableData_ChangeMap(rVehicleInfoManager.GetItem(vehicleName).mIpPort, MapFileName);
				}
				RaiseEvent_VehicleCurrentMapSynchronized(vehicleNames, MapFileName);
			}
		}
		public void SynchronizeVehicleCurrentMap2(string MapFileNameWithoutExtension)
		{
			SynchronizeVehicleCurrentMap(MapFileNameWithoutExtension + ".map");
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "AutoLoadMap":
					return mAutoLoadMap.ToString();
				default:
					return null;
			}
		}
		public override void SetConfig(string ConfigName, string NewValue)
		{
			switch (ConfigName)
			{
				case "AutoLoadMap":
					mAutoLoadMap = bool.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}

		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.ReceivedSerializableData += HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.ReceivedSerializableData -= HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
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
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{

			}
		}
		private void UnsubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{

			}
		}
		protected virtual void RaiseEvent_MapLoaded(string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				MapLoaded?.Invoke(DateTime.Now, MapFileName);
			}
			else
			{
				Task.Run(() => { MapLoaded?.Invoke(DateTime.Now, MapFileName); });
			}
		}
		protected virtual void RaiseEvent_VehicleCurrentMapSynchronized(IEnumerable<string> VehicleNames, string MapFileName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleCurrentMapSynchronized?.Invoke(DateTime.Now, VehicleNames, MapFileName);
			}
			else
			{
				Task.Run(() => { VehicleCurrentMapSynchronized?.Invoke(DateTime.Now, VehicleNames, MapFileName); });
			}
		}
		private void HandleEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			if (Data is Serializable)
			{
				// 當收到「下載地圖」的回覆，使用 IMapFileManager 將其儲存，並更新「下載中地圖清單」
				if (Data is GetMap)
				{
					GetMap tmpData = Data as GetMap;
					rMapFileManager.AddMapFile(tmpData.Response.Name, tmpData.Response.Data);
					if (mMapFileNamesOfDownloading.Contains(tmpData.Response.Name)) mMapFileNamesOfDownloading.Remove(tmpData.Response.Name);
				}
				// 當收到「上傳地圖檔」的回覆，向其發送「取得當前地圖清單」的請求，以取得最新的該車地圖資訊
				else if (Data is UploadMapToAGV)
				{
					rVehicleCommunicator.SendSerializableData_RequestMapList(IpPort);
				}
				// 當收到「改變當前地圖」的回覆，向其發送「取得當前地圖清單」的請求，以取得最新的該車地圖資訊
				else if (Data is ChangeMap)
				{
					rVehicleCommunicator.SendSerializableData_RequestMapList(IpPort);
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			// 當有車連線時，向其發送「取得當前地圖清單」的請求
			rVehicleCommunicator.SendSerializableData_RequestMapList(Args.Item.mIpPort);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			// 當有車離線時，重新整理「下載中地圖清單」
			if (!string.IsNullOrEmpty(Args.Item.mCurrentMapName) && mMapFileNamesOfDownloading.Count > 0)
			{
				if (mMapFileNamesOfDownloading.Contains(Args.Item.mCurrentMapName))
				{
					mMapFileNamesOfDownloading.Remove(Args.Item.mCurrentMapName);
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			// 當車的「當前使用地圖」屬性改變時 (僅使用 Contains 判斷有可能抓到 "CurrentMapName" 與 "CurrentMapNameList" 兩種結果，所以需要做更細節的字串判斷)
			if (Args.StatusName.Contains("CurrentMapName") && Args.StatusName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Any(o => o == "CurrentMapName"))
			{
				// 當 CurrentMapName 不為空或 Null 時發送「下載地圖」的請求，並更新「下載中地圖清單」
				if (!string.IsNullOrEmpty(Args.Item.mCurrentMapName))
				{
					rVehicleCommunicator.SendSerializableData_GetMap(Args.Item.mIpPort, Args.Item.mCurrentMapName);
					mMapFileNamesOfDownloading.Add(Args.Item.mCurrentMapName);
				}

				// 當啟用「自動讀取地圖」功能時將嘗試讀取地圖
				if (mAutoLoadMap)
				{
					DateTime tmpTimestamp = DateTime.Now;
					Task.Run(() => TryLoadMap(Args.Item.mCurrentMapName));
				}
			}
		}
		private void TryLoadMap(string MapFileName)
		{
			DateTime tmpTimestamp = DateTime.Now;
			while (DateTime.Now.Subtract(tmpTimestamp).TotalMilliseconds < 5000)
			{
				// 當下載地圖完成時
				if (!mIsDownloadingMapFile)
				{
					// 若有該地圖存在，且該地圖 Hash 與當前地圖 Hash 不同時
					if (rMapFileManager.GetLocalMapFileNameList().Any(o => o == MapFileName) && MD5HashCalculator.CalculateFileHash(rMapFileManager.GetMapFileFullPath(MapFileName)) != GetCurrentMapFileHash())
					{
						LoadMap(MapFileName);
						break;
					}
					else
					{
						break;
					}
				}
				Thread.Sleep(600);
			}
		}
		private void TryLoadMap2(string MapFileNameWithoutExtension)
		{
			TryLoadMap(MapFileNameWithoutExtension + ".map");
		}
	}
}
