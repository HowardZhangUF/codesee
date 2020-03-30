﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Module.General.Interface;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CycleMission
{
	public interface ICycleMissionGenerator : ISystemWithLoopTask
	{
		event EventHandlerCycleMissionAssigned CycleMissionAssigned;
		event EventHandlerCycleMissionRemoved CycleMissionRemoved;
		event EventHandlerCycleMissionIndexUpdated CycleMissionIndexUpdated;

		void Set(IVehicleInfoManager VehicleInfoManager);
		void Set(IMissionStateManager MissionStateManager);
		void Set(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager);
		void AssignCycleMission(string VehicleId, string[] Targets, int StartIndex = 0);
		void RemoveCycleMission(string VehicleId);
		bool GetAssigned(string VehicleId);
		string[] GetMissionList(string VehicleId);
		int GetCurrentMissionIndex(string VehicleId);
	}
}