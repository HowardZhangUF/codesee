﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class Mission : IMission
	{
		public string mMissionType { get; private set; }
		public string mMissionId { get; private set; }
		public int mPriority { get; private set; }
		public string mVehicleId { get; private set; }
		public string[] mParameters { get; private set; }

		public Mission(string MissionType, string MissionId, int Priority, string VehicleId, string[] Parameters)
		{
			Set(MissionType, MissionId, Priority, VehicleId, Parameters);
		}
		public void Set(string MissionType, string MissionId, int Priority, string VehicleId, string[] Parameters)
		{
			mMissionType = MissionType;
			mMissionId = MissionId;
			mPriority = Priority;
			mVehicleId = VehicleId;
			mParameters = Parameters;
		}
		public void UpdatePriority(int Priority)
		{
			mPriority = Priority;
		}
		public override string ToString()
		{
			string result = string.Empty;
			result += mMissionType;
			if (!string.IsNullOrEmpty(mVehicleId)) result += $"/{mVehicleId}";
			if (mParameters != null && mParameters.Length > 0) result += $"/{string.Join(",", mParameters)}";
			return result;
		}
	}
}
