﻿using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Implement;

namespace TrafficControlTest.Implement
{
	public class VehicleInfoManager : ItemManager<IVehicleInfo>, IVehicleInfoManager
	{
		public IVehicleInfo this[string Name] { get { return GetItem(Name); } }

		public VehicleInfoManager()
		{
		}
		public bool IsExistByIpPort(string IpPort)
		{
			return mItems.Values.Any(o => o.mIpPort == IpPort);
		}
		public IVehicleInfo GetItemByIpPort(string IpPort)
		{
			return (IsExistByIpPort(IpPort) ? mItems.Values.First((o) => o.mIpPort == IpPort) : null);
		}
		public void UpdateItem(string Name, string NewState, IPoint2D NewLocationCoordinate, double NewLocationToward, string NewTarget, double NewVelocity, double NewLocationScore, double NewBatteryValue, string NewAlarmMessage)
		{
			if (IsExist(Name))
			{
				mItems[Name].BeginUpdate();
				mItems[Name].UpdateCurrentState(NewState);
				mItems[Name].UpdateLocationCoordinate(NewLocationCoordinate);
				mItems[Name].UpdateLocationToward(NewLocationToward);
				mItems[Name].UpdateCurrentTarget(NewTarget);
				mItems[Name].UpdateVelocity(NewVelocity);
				mItems[Name].UpdateLocationScore(NewLocationScore);
				mItems[Name].UpdateBatteryValue(NewBatteryValue);
				mItems[Name].UpdateAlarmMessage(NewAlarmMessage);
				mItems[Name].EndUpdate();
			}
		}
		public void UpdateItem(string Name, IEnumerable<IPoint2D> Path)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdatePath(Path);
			}
		}
		public void UpdateItem(string Name, string IpPort)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateIpPort(IpPort);
			}
		}
		public void UpdateItemMissionId(string Name, string MissionId)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateCurrentMissionId(MissionId);
			}
		}
		public void UpdateItemInterveneCommand(string Name, string InterveneCommand)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateCurrentInterveneCommand(InterveneCommand);
			}
		}
		public void UpdateItemMapName(string Name, string MapName)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateCurrentMapName(MapName);
			}
		}
		public void UpdateItemMapNameList(string Name, IEnumerable<string> MapNameList)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateCurrentMapNameList(MapNameList);
			}
		}
	}
}
