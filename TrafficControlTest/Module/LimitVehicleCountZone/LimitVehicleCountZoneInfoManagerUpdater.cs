﻿using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public class LimitVehicleCountZoneInfoManagerUpdater : SystemWithLoopTask, ILimitVehicleCountZoneInfoManagerUpdater
	{
		private ILimitVehicleCountZoneInfoManager rLimitVehicleCountZoneInfoManager = null;
		private IMapManager rMapManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;

		public LimitVehicleCountZoneInfoManagerUpdater(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(LimitVehicleCountZoneInfoManager, MapManager, VehicleInfoManager);
		}
		public void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			UnsubscribeEvent_ILimitVehicleCountZoneInfoManager(rLimitVehicleCountZoneInfoManager);
			rLimitVehicleCountZoneInfoManager = LimitVehicleCountZoneInfoManager;
			SubscribeEvent_ILimitVehicleCountZoneInfoManager(rLimitVehicleCountZoneInfoManager);
		}
		public void Set(IMapManager MapManager)
		{
			UnsubscribeEvent_IMapManager(rMapManager);
			rMapManager = MapManager;
			SubscribeEvent_IMapManager(rMapManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager, IMapManager MapManager, IVehicleInfoManager VehicleInfoManager)
		{
			Set(LimitVehicleCountZoneInfoManager);
			Set(MapManager);
			Set(VehicleInfoManager);
		}
		public override string GetSystemInfo()
		{
			string result = string.Empty;
			result += $"CountOfLimitVehicleCountZoneInfo: {rLimitVehicleCountZoneInfoManager.mCount}, CountOfVehicleInfo: {rVehicleInfoManager.mCount}";
			return result;
		}
		public override void Task()
		{
			Subtask_CalculateVehicleNameListInLimitVehicleCountZoneInfo();
		}

		private void SubscribeEvent_ILimitVehicleCountZoneInfoManager(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			if (LimitVehicleCountZoneInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_ILimitVehicleCountZoneInfoManager(ILimitVehicleCountZoneInfoManager LimitVehicleCountZoneInfoManager)
		{
			if (LimitVehicleCountZoneInfoManager != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged += HandleEvent_IMapManagerMapChanged;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged -= HandleEvent_IMapManagerMapChanged;
			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				// do nothing
			}
		}
		private void HandleEvent_IMapManagerMapChanged(object sender, MapChangedEventArgs e)
		{
			rLimitVehicleCountZoneInfoManager.RemoveAll();
			List<IMapObjectOfRectangle> singleVehicleInfos = rMapManager.GetRectangleMapObjects(TypeOfMapObjectOfRectangle.SingleVehicle);
			if (singleVehicleInfos != null && singleVehicleInfos.Count > 0)
			{
				Dictionary<int, int> unionCollection = CalculateUnionCollection(singleVehicleInfos);
				for (int i = 0; i < singleVehicleInfos.Count; ++i)
				{
					// SingleVehicle 區塊沒有額外命名，所以名字採用流水號，允需車數量固定為 1
					if (unionCollection.ContainsKey(i))
					{
						ILimitVehicleCountZoneInfo tmp = Library.Library.GenerateILimitVehicleCountZoneInfo(i.ToString().PadLeft(3, '0'), singleVehicleInfos[i].mRange, 1, true, unionCollection[i]);
						rLimitVehicleCountZoneInfoManager.Add(tmp.mName, tmp);
					}
					else
					{
						ILimitVehicleCountZoneInfo tmp = Library.Library.GenerateILimitVehicleCountZoneInfo(i.ToString().PadLeft(3, '0'), singleVehicleInfos[i].mRange, 1, false, 0);
						rLimitVehicleCountZoneInfoManager.Add(tmp.mName, tmp);
					}
				}
			}
		}
		private void Subtask_CalculateVehicleNameListInLimitVehicleCountZoneInfo()
		{
			List<ILimitVehicleCountZoneInfo> tmpLimitVehicleCountZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().ToList();
			if (tmpLimitVehicleCountZoneInfos.Count > 0)
			{
				List<List<Tuple<string, DateTime>>> newDatas = new List<List<Tuple<string, DateTime>>>();
				// 計算每一個 ILimitVehicleCountZoneInfo 的 CurrentVehicleNameList 資訊
				for (int i = 0; i < tmpLimitVehicleCountZoneInfos.Count; ++i)
				{
					List<Tuple<string, DateTime>> tmpCurrentVehicleNameList = new List<Tuple<string, DateTime>>();
					List<string> tmpVehicleNames = rVehicleInfoManager.GetItems().Where(o => tmpLimitVehicleCountZoneInfos[i].mRange.IsIncludePoint(o.mLocationCoordinate)).Select(o => o.mName).ToList();
					for (int j = 0; j < tmpVehicleNames.Count; ++j)
					{
						string vehicleName = tmpVehicleNames[j];
						DateTime enterTimestamp = DateTime.Now;
						if (tmpLimitVehicleCountZoneInfos[i].mIsUnioned) // 若該車早已進入聯集區域，則沿用該時間戳
						{
							int unionId = tmpLimitVehicleCountZoneInfos[i].mUnionId;
							List<ILimitVehicleCountZoneInfo> unionedZoneInfos = rLimitVehicleCountZoneInfoManager.GetItems().Where(o => o.mUnionId == unionId).ToList();
							for (int k = 0; k < unionedZoneInfos.Count; ++k)
							{
								if (unionedZoneInfos[k].ContainsVehicle(vehicleName))
								{
									if (unionedZoneInfos[k].GetVehicleEnterTimestamp(vehicleName) < enterTimestamp)
									{
										enterTimestamp = unionedZoneInfos[k].GetVehicleEnterTimestamp(vehicleName);
									}
								}
							}
						}
						tmpCurrentVehicleNameList.Add(new Tuple<string, DateTime>(vehicleName, enterTimestamp));
					}
					newDatas.Add(tmpCurrentVehicleNameList);
				}

				// 將所有 ILimitVehicleCountZoneInfo 的資訊都計算完後，再一次更新
				for (int i = 0; i < tmpLimitVehicleCountZoneInfos.Count; ++i)
				{
					rLimitVehicleCountZoneInfoManager.UpdateCurrentVehicleNameList(tmpLimitVehicleCountZoneInfos[i].mName, newDatas[i]);
				}
			}
		}

		private static Dictionary<int, int> CalculateUnionCollection(List<IMapObjectOfRectangle> Rectangles)
		{
			Dictionary<int, int> result = new Dictionary<int, int>(); // key = 區塊編號, value = union id
			int currentUnionId = 65536; // union id 從 65536 開始

			for (int i = 0; i < Rectangles.Count; ++i)
			{
				for (int j = i + 1; j < Rectangles.Count; ++j)
				{
					if (GeometryAlgorithm.IsRectangleOverlap(Rectangles[i].mRange, Rectangles[j].mRange))
					{
						if (result.ContainsKey(i) && result.ContainsKey(j))
						{
							// do nothing
						}
						else if (result.ContainsKey(i) && !result.ContainsKey(j)) // 如果 i 已經跟別人 Union 了，則填入同樣的 Union Id
						{
							result.Add(j, result[i]);
						}
						else if (!result.ContainsKey(i) && result.ContainsKey(j)) // 如果 j 已經跟別人 Union 了，則填入同樣的 Union Id
						{
							result.Add(i, result[j]);
						}
						else if (!result.ContainsKey(i) && !result.ContainsKey(j))
						{
							result.Add(i, currentUnionId);
							result.Add(j, currentUnionId);
							currentUnionId += 1;
						}
					}
				}
			}

			// 範例輸出：
			// key value
			// 1   65536
			// 3   65536
			// 4   65537
			// 5   65537
			// 7   65536
			// 其中 1, 3, 7 是 Union ， 4, 5 是 Union

			return result;
		}
	}
}
