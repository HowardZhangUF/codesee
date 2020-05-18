﻿using System.Collections.Generic;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.Map
{
	/// <summary>
	/// - Reference: IVehicleCommunicator, IVehicleInfoManager, IMapFileManager
	/// - 提供載入地圖的功能
	/// - 提供取得當前地圖檔名稱的功能
	/// - 提供取得當前地圖檔 Hash 的功能
	/// - 提供取得地圖物件清單的功能
	/// - 提供取得指定地圖物件座標的功能
	/// - 提供同步當前地圖至所有車的功能
	/// - 提供設定「自動載入地圖」的功能
	/// - 載入地圖後會發出事件
	/// - 同步地圖至所有車後會發出事件
	/// - 當收到「下載地圖」的回覆，使用 IMapFileManager 將其儲存，並更新「下載中地圖清單」
	/// - 當收到「上傳地圖檔」的回覆，向其發送「取得當前地圖清單」的請求，以取得最新的該車地圖資訊
	/// - 當收到「改變當前地圖」的回覆，向其發送「取得當前地圖清單」的請求，以取得最新的該車地圖資訊
	/// - 當有車連線時，向其發送「取得當前地圖清單」的請求
	/// - 當有車離線時，重新整理「下載中地圖清單」
	/// - 當車的「當前使用地圖」屬性改變時，當 CurrentMapName 不為空或 Null 時發送「下載地圖」的請求，並更新「下載中地圖清單」
	/// - 當車的「當前使用地圖」屬性改變時，當啟用「自動讀取地圖」功能時將嘗試讀取地圖
	/// </summary>
	public interface IMapManager : ISystemWithConfig
	{
		event EventHandlerMapFileName MapLoaded;
		event EventHandlerVehicleNamesMapFileName VehicleCurrentMapSynchronized;

		void Set(IVehicleCommunicator VehicleCommunicator);
		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMapFileManager MapFileManager);
		void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMapFileManager MapFileManager);
		void LoadMap(string MapFileName);
		void LoadMap2(string MapFileNameWithoutExtension);
		string GetCurrentMapFileName();
		string GetCurrentMapFileNameWithoutExtension();
		string GetCurrentMapFileHash();
		string[] GetGoalNameList();
		int[] GetGoalCoordinate(string GoalName);
		void SynchronizeVehicleCurrentMap(string MapFileName);
		void SynchronizeVehicleCurrentMap2(string MapFileNameWithoutExtension);
	}
}
