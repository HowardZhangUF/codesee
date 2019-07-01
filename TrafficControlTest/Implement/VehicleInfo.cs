﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleInfo;

namespace TrafficControlTest.Implement
{
	class VehicleInfo : IVehicleInfo
	{
		public event EventHandlerIVehicleInfo StateUpdated;

		public string mName
		{
			get { return _Name; }
			private set
			{
				if (!string.IsNullOrEmpty(value))
				{
					_Name = value;
					mLastUpdated = DateTime.Now;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mState
		{
			get { return _State; }
			private set
			{
				if (!string.IsNullOrEmpty(value) && _State != value)
				{
					mLastState = _State;
					_State = value;
					mStateStartTimestamp = DateTime.Now;
					mLastUpdated = DateTime.Now;
				}
			}
		}
		public string mLastState { get; private set; }
		public TimeSpan mStateDuration
		{
			get { return DateTime.Now.Subtract(mStateStartTimestamp); }
		}
		public IPoint2D mPosition
		{
			get { return _Position; }
			private set
			{
				if (value != null && (_Position == null || (_Position.mX != value.mX || _Position.mY != value.mY)))
				{
					_Position = value;
					mLastUpdated = DateTime.Now;
				}
			}
		}
		public double mToward
		{
			get { return _Toward; }
			private set
			{
				if (_Toward != value)
				{
					_Toward = value;
					mLastUpdated = DateTime.Now;
				}
			}
		}
		public string mTarget
		{
			get { return _Target; }
			private set
			{
				if (!string.IsNullOrEmpty(value) && _Target != value)
				{
					mLastTarget = _Target;
					_Target = value;
					mLastUpdated = DateTime.Now;
				}
			}
		}
		public string mLastTarget { get; private set; }
		public double mVelocity
		{
			get { return _Velocity; }
			private set
			{
				_Velocity = value;
				if (mLastVelocity.Count > mVelocifyDataCount) mLastVelocity.RemoveAt(0);
				mLastVelocity.Add(value);
				mLastUpdated = DateTime.Now;
			}
		}
		public double mAverageVelocity
		{
			get { return mLastVelocity.Sum() / mLastVelocity.Count; }
		}
		public double mMapMatch
		{
			get
			{
				return _MapMatch;
			}
			set
			{
				if (_MapMatch != value)
				{
					_MapMatch = value;
					mLastUpdated = DateTime.Now;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mBattery
		{
			get
			{
				return _Battery;
			}
			private set
			{
				if (_Battery != value)
				{
					_Battery = value;
					mLastUpdated = DateTime.Now;
				}
			}
		}
		public bool mPathBlocked
		{
			get
			{
				return _PathBlocked;
			}
			set
			{
				if (_PathBlocked != value)
				{
					_PathBlocked = value;
					if (_PathBlocked == true) mPathBlockedStartTimestamp = DateTime.Now;
					mLastUpdated = DateTime.Now;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public TimeSpan mPathBlockedDuration
		{
			get { return DateTime.Now.Subtract(mPathBlockedStartTimestamp); }
		}
		public string mAlarmMessage
		{
			get
			{
				return _AlarmMessage;
			}
			private set
			{
				if (!string.IsNullOrEmpty(value) && _AlarmMessage != value)
				{
					_AlarmMessage = value;
					mLastUpdated = DateTime.Now;
				}
			}
		}
		public int mSafetyFrameRadius
		{
			get
			{
				return _SafetyFrameRadius;
			}
			set
			{
				if (_SafetyFrameRadius != value)
				{
					_SafetyFrameRadius = value;
					mLastUpdated = DateTime.Now;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public int mBufferFrameRadius { get; set; } = 500;
		public int mTotalFrameRadius
		{
			get
			{
				return mSafetyFrameRadius + mBufferFrameRadius;
			}
		}
		public IEnumerable<IPoint2D> mPath
		{
			get
			{
				return _Path;
			}
			set
			{
				if (value != null)
				{
					_Path = value;
					_PathDetail = null;
					_PathRegion = null;
					mLastUpdated = DateTime.Now;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public IEnumerable<IPoint2D> mPathDetail
		{
			get
			{
				if (_PathDetail == null) _PathDetail = CalculatePathDetail(_Path, (mSafetyFrameRadius + mBufferFrameRadius) / 10);
				return _PathDetail;
			}
			private set
			{
				if (value != null)
				{
					_PathDetail = value;
					mLastUpdated = DateTime.Now;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public IRectangle2D mPathRegion
		{
			get
			{
				if (_PathRegion == null) _PathRegion = CalculatePathRegion(_Path, mSafetyFrameRadius + mBufferFrameRadius);
				return _PathRegion;
			}
			private set
			{
				if (value != null)
				{
					_PathRegion = value;
					mLastUpdated = DateTime.Now;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mIpPort
		{
			get
			{
				return _IpPort;
			}
			private set
			{
				if (!string.IsNullOrEmpty(value) && _IpPort != value)
				{
					_IpPort = value;
					mLastUpdated = DateTime.Now;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public DateTime mLastUpdated { get; private set; }
		public int mVehicleIconId { get; set; }
		public int mPathIconId { get; set; }

		public string _Name = string.Empty;
		public string _State = string.Empty;
		public IPoint2D _Position = null;
		public double _Toward = 0.0f;
		public string _Target = string.Empty;
		public double _Velocity = 0.0f;
		public double _MapMatch = 0.0f;
		public double _Battery = 0.0f;
		public bool _PathBlocked = false;
		public string _AlarmMessage = string.Empty;
		public int _SafetyFrameRadius = 500;
		public IEnumerable<IPoint2D> _Path = null;
		public IEnumerable<IPoint2D> _PathDetail = null;
		public IRectangle2D _PathRegion = null;
		public string _IpPort = string.Empty;

		private DateTime mStateStartTimestamp = DateTime.Now;
		private List<double> mLastVelocity = new List<double>();
		private int mVelocifyDataCount = 10;
		private DateTime mPathBlockedStartTimestamp = DateTime.Now;

		public VehicleInfo(string Name)
		{
			Set(Name);
		}
		public void Set(string Name)
		{
			mName = Name;
		}
		public void Update(string State, IPoint2D Position, double Toward, double Battery, double Velocity, string Target, string AlarmMessage)
		{
			mState = State;
			mPosition = Position;
			mToward = Toward;
			mBattery = Battery;
			mVelocity = Velocity;
			mTarget = Target;
			mAlarmMessage = AlarmMessage;
			RaiseEvent_StateUpdated();
		}
		public void Update(IEnumerable<IPoint2D> Path)
		{
			mPath = Path;
		}
		public void Update(string IpPort)
		{
			mIpPort = IpPort;
		}
		public override string ToString()
		{
			string result = string.Empty;
			result += $"Name: {mName}, ";
			result += $"IpPort: {mIpPort}, ";
			result += $"State: {mState}, ";
			result += $"Position: {(mPosition != null ? mPosition.ToString() : string.Empty)}, ";
			result += $"Toward: {mToward.ToString("F2")}, ";
			result += $"Target: {mTarget}, ";
			result += $"Velocity: {mVelocity.ToString("F2")}, ";
			result += $"AverageVelocity: {(!double.IsNaN(mAverageVelocity) ? mAverageVelocity.ToString("F2") : string.Empty)}, ";
			result += $"Battery: {mBattery.ToString("F2")}, ";
			result += $"Path: {((mPath != null && mPath.Count() > 0) ? Library.Library.ConvertToString(mPath) : string.Empty)}";
			return result;
		}

		protected virtual void RaiseEvent_StateUpdated(bool Sync = true)
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
		private IEnumerable<IPoint2D> CalculatePathDetail(IEnumerable<IPoint2D> Path, int Interval)
		{
			List<IPoint2D> result = null;
			if (Path != null && Path.Count() > 0)
			{
				result = new List<IPoint2D>();
				List<IPoint2D> tmpPath = Path.ToList();
				tmpPath.Insert(0, mPosition);
				result.Add(tmpPath[0]);
				for (int i = 1; i < tmpPath.Count; ++i)
				{
					result.AddRange(Library.Library.ConvertLineToPoints(tmpPath[i - 1], tmpPath[i], Interval));
					result.Add(tmpPath[i]);
				}
			}
			return result;
		}
		private IRectangle2D CalculatePathRegion(IEnumerable<IPoint2D> Path, int Amplify)
		{
			IRectangle2D result = null;
			if (Path != null && Path.Count() > 0)
			{
				List<IPoint2D> tmpPath = Path.ToList();
				tmpPath.Insert(0, mPosition);
				result = Library.Library.GetCoverRectangle(tmpPath);
				result = Library.Library.GetAmplifyRectangle(result, Amplify, Amplify);
			}
			return result;
		}
	}
}
