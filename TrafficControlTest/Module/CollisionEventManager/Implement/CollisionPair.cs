﻿using System;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibraryOfICollisionEventManager;

namespace TrafficControlTest.Implement
{
	class CollisionPair : ICollisionPair
	{
		public event EventHandlerICollisionPair StateUpdated;

		public string mName { get; private set; }
		public IVehicleInfo mVehicle1 { get; private set; }
		public IVehicleInfo mVehicle2 { get; private set; }
		public IRectangle2D mCollisionRegion { get; private set; }
		public ITimePeriod mPeriod { get; private set; }
		public TimeSpan mDuration { get { return DateTime.Now.Subtract(mStartTimestamp); } }
		public DateTime mLastUpdated { get; private set; }

		private DateTime mStartTimestamp { get; set; }

		public CollisionPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D CollisionRegion, ITimePeriod Period)
		{
			Set(Vehicle1, Vehicle2, CollisionRegion, Period);
		}

		public void Set(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D CollisionRegion, ITimePeriod Period)
		{
			mName = Vehicle1.mName + "&" + Vehicle2.mName;
			mVehicle1 = Vehicle1;
			mVehicle2 = Vehicle2;
			mCollisionRegion = CollisionRegion;
			mPeriod = Period;
			mStartTimestamp = DateTime.Now;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StateUpdated();
		}
		public void Update(IRectangle2D CollisionRegion, ITimePeriod Period)
		{
			mCollisionRegion = CollisionRegion;
			mPeriod = Period;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StateUpdated();
		}
		public override string ToString()
		{
			string result = string.Empty;
			result += $"============================================================";
			result += $"\n[ CollisionPair ]";
			result += $"\nVehicles: {mVehicle1.mName} & {mVehicle2.mName}";
			result += $"\nName: {mName}";
			result += $"\nRegion: {mCollisionRegion.ToString()}";
			result += $"\nEstimate Time(s): {mPeriod.ToString(Library.Library.TIME_FORMAT)}";
			result += $"\nEvent Duration(s): {mDuration.TotalSeconds.ToString("F2")}";
			result += $"\nLastUpdated: {mLastUpdated.ToString(Library.Library.TIME_FORMAT)}";
			result += $"\n============================================================";
			return result;
		}

		private void RaiseEvent_StateUpdated(bool Sync = true)
		{
			if (Sync)
			{
				StateUpdated?.Invoke(DateTime.Now, mName, this);
			}
			else
			{
				Task.Run(() => StateUpdated?.Invoke(DateTime.Now, mName, this));
			}
		}
	}
}