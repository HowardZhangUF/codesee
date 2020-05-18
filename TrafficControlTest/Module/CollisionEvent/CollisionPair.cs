﻿using System;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CollisionEvent
{
	class CollisionPair : ICollisionPair
	{
		public event EventHandlerICollisionPair StateUpdated;

		public string mName { get; private set; }
		public IVehicleInfo mVehicle1 { get; private set; }
		public IVehicleInfo mVehicle2 { get; private set; }
		public IRectangle2D mCollisionRegion { get; private set; }
		public ITimePeriod mPeriod { get; private set; }
        public ITimePeriod mPassPeriodOfVehicle1WithCurrentVelocity { get; private set; }
        public ITimePeriod mPassPeriodOfVehicle2WithCurrentVelocity { get; private set; }
        public ITimePeriod mPassPeriodOfVehicle1WithMaximumVeloctiy { get; private set; }
        public ITimePeriod mPassPeriodOfVehicle2WithMaximumVeloctiy { get; private set; }
        public TimeSpan mDuration { get { return DateTime.Now.Subtract(mStartTimestamp); } }
		public DateTime mLastUpdated { get; private set; }

		private DateTime mStartTimestamp { get; set; }

		public CollisionPair(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy)
		{
			Set(Vehicle1, Vehicle2, CollisionRegion, Period, PassPeriodOfVehicle1WithCurrentVelocity, PassPeriodOfVehicle2WithCurrentVelocity, PassPeriodOfVehicle1WithMaximumVeloctiy, PassPeriodOfVehicle2WithMaximumVeloctiy);
		}

		public void Set(IVehicleInfo Vehicle1, IVehicleInfo Vehicle2, IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy)
		{
			mName = "CollisionOf" + Vehicle1.mName + "&" + Vehicle2.mName;
			mVehicle1 = Vehicle1;
			mVehicle2 = Vehicle2;
			mCollisionRegion = CollisionRegion;
			mPeriod = Period;
            mPassPeriodOfVehicle1WithCurrentVelocity = PassPeriodOfVehicle1WithCurrentVelocity;
            mPassPeriodOfVehicle2WithCurrentVelocity = PassPeriodOfVehicle2WithCurrentVelocity;
            mPassPeriodOfVehicle1WithMaximumVeloctiy = PassPeriodOfVehicle1WithMaximumVeloctiy;
            mPassPeriodOfVehicle2WithMaximumVeloctiy = PassPeriodOfVehicle2WithMaximumVeloctiy;
            mStartTimestamp = DateTime.Now;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StateUpdated();
		}
		public void Update(IRectangle2D CollisionRegion, ITimePeriod Period, ITimePeriod PassPeriodOfVehicle1WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle2WithCurrentVelocity, ITimePeriod PassPeriodOfVehicle1WithMaximumVeloctiy, ITimePeriod PassPeriodOfVehicle2WithMaximumVeloctiy)
		{
			mCollisionRegion = CollisionRegion;
			mPeriod = Period;
			mPassPeriodOfVehicle1WithCurrentVelocity = PassPeriodOfVehicle1WithCurrentVelocity;
			mPassPeriodOfVehicle2WithCurrentVelocity = PassPeriodOfVehicle2WithCurrentVelocity;
			mPassPeriodOfVehicle1WithMaximumVeloctiy = PassPeriodOfVehicle1WithMaximumVeloctiy;
			mPassPeriodOfVehicle2WithMaximumVeloctiy = PassPeriodOfVehicle2WithMaximumVeloctiy;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StateUpdated();
		}
		public override string ToString()
		{
			string result = string.Empty;
			result += $"{mName}/{mCollisionRegion.ToString()}/{mPeriod.ToString()}";
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