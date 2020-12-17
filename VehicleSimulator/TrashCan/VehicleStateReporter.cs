﻿using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Communication;
using VehicleSimulator.Interface;

namespace VehicleSimulator.Implement
{
	public class VehicleStateReporter : IVehicleStateReporter
	{
		public event EventHandlerLibrary.EventHandlerDateTime SystemStarted;
		public event EventHandlerLibrary.EventHandlerDateTime SystemStopped;

		public bool mIsExcuting { get { return (mThdReportVehicleState != null && mThdReportVehicleState.IsAlive == true) ? true : false; } }
		public bool mAutoStart { get; set; } = true;

		private IVehicleSimulatorInfo rVehicleSimulatorInfo = null;
		private ICommunicatorClient rCommunicatorClient = null;
		private Thread mThdReportVehicleState = null;
		private bool[] mThdReportVehicleStateExitFlag = null;

		public VehicleStateReporter(IVehicleSimulatorInfo VehicleSimulatorInfo, ICommunicatorClient CommunicatorClient)
		{
			Set(VehicleSimulatorInfo, CommunicatorClient);
		}
		public void Set(IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			UnsubscribeEvent_IVehicleSimulatorInfo(rVehicleSimulatorInfo);
			rVehicleSimulatorInfo = VehicleSimulatorInfo;
			SubscribeEvent_IVehicleSimulatorInfo(rVehicleSimulatorInfo);
		}
		public void Set(ICommunicatorClient CommunicatorClient)
		{
			UnsubscribeEvent_ICommunicatorClient(rCommunicatorClient);
			rCommunicatorClient = CommunicatorClient;
			SubscribeEvent_ICommunicatorClient(rCommunicatorClient);
		}
		public void Set(IVehicleSimulatorInfo VehicleSimulatorInfo, ICommunicatorClient CommunicatorClient)
		{
			Set(VehicleSimulatorInfo);
			Set(CommunicatorClient);
		}
		public void Start()
		{
			InitializeThread();
		}
		public void Stop()
		{
			DestroyThread();
		}

		private void SubscribeEvent_IVehicleSimulatorInfo(IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			if (VehicleSimulatorInfo != null)
			{
				VehicleSimulatorInfo.StateUpdated += HandleEvent_VehicleSimulatorInfoStateUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleSimulatorInfo(IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			if (VehicleSimulatorInfo != null)
			{
				VehicleSimulatorInfo.StateUpdated -= HandleEvent_VehicleSimulatorInfoStateUpdated;
			}
		}
		private void SubscribeEvent_ICommunicatorClient(ICommunicatorClient CommunicatorClient)
		{
			if (CommunicatorClient != null)
			{
				CommunicatorClient.ConnectStateChanged += HandleEvent_CommunicatorClientConnectStateChanged;
			}
		}
		private void UnsubscribeEvent_ICommunicatorClient(ICommunicatorClient CommunicatorClient)
		{
			if (CommunicatorClient != null)
			{
				CommunicatorClient.ConnectStateChanged += HandleEvent_CommunicatorClientConnectStateChanged;
			}
		}
		private void HandleEvent_VehicleSimulatorInfoStateUpdated(DateTime OccurTime, string Name, IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			//Subtask_ReportVehicleState();
		}
		private void HandleEvent_CommunicatorClientConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			if (mAutoStart)
			{
				if (NewState == ConnectState.Connected)
				{
					Start();
				}
				else
				{
					Stop();
				}
			}
		}
		protected virtual void RaiseEvent_SystemStarted(bool Sync = true)
		{
			if (Sync)
			{
				SystemStarted?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { SystemStarted?.Invoke(DateTime.Now); });
			}
		}
		protected virtual void RaiseEvent_SystemStopped(bool Sync = true)
		{
			if (Sync)
			{
				SystemStopped?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { SystemStopped?.Invoke(DateTime.Now); });
			}
		}
		private void InitializeThread()
		{
			mThdReportVehicleStateExitFlag = new bool[] { false };
			mThdReportVehicleState = new Thread(() => Task_ReportVehicleState(mThdReportVehicleStateExitFlag));
			mThdReportVehicleState.IsBackground = true;
			mThdReportVehicleState.Start();
		}
		private void DestroyThread()
		{
			if (mThdReportVehicleState != null)
			{
				if (mThdReportVehicleState.IsAlive)
				{
					mThdReportVehicleStateExitFlag[0] = true;
				}
				mThdReportVehicleState = null;
			}
		}
		private void Task_ReportVehicleState(bool[] ExitFlag)
		{
			try
			{
				RaiseEvent_SystemStarted();
				while (!ExitFlag[0])
				{
					Subtask_ReportVehicleState();
					Thread.Sleep(750);
				}
			}
			finally
			{
				Subtask_ReportVehicleState();
				RaiseEvent_SystemStopped();
			}
		}
		private void Subtask_ReportVehicleState()
		{
			if (rVehicleSimulatorInfo != null && rCommunicatorClient != null && rCommunicatorClient.mConnectState == ConnectState.Connected)
			{
				AGVStatus tmpStatus = ConvertToAGVStatus(rVehicleSimulatorInfo);
				if (tmpStatus != null)
				{
					rCommunicatorClient.SendSerializableData(tmpStatus);
				}

				AGVPath tmpPath = ConvertToAGVPath(rVehicleSimulatorInfo);
				if (tmpPath != null)
				{
					rCommunicatorClient.SendSerializableData(tmpPath);
				}
			}
		}
		private AGVStatus ConvertToAGVStatus(IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			AGVStatus result = null;
			if (VehicleSimulatorInfo != null)
			{
				result = new AGVStatus();
				result.AlarmMessage = VehicleSimulatorInfo.mAlarmMessage;
				result.Battery = VehicleSimulatorInfo.mBattery;
				if (Enum.TryParse(VehicleSimulatorInfo.mState, out EDescription description))
				{
					result.Description = description;
				}
				else
				{
					result.Description = EDescription.Idle;
				}
				result.GoalName = VehicleSimulatorInfo.mTarget;
				result.MapMatch = VehicleSimulatorInfo.mMapMatch;
				result.Name = VehicleSimulatorInfo.mName;
				result.Toward = VehicleSimulatorInfo.mToward;
				result.Velocity = VehicleSimulatorInfo.mTranslationVelocity;
				result.X = VehicleSimulatorInfo.mPosition.mX;
				result.Y = VehicleSimulatorInfo.mPosition.mY;
				result.IsInterveneAvailable = VehicleSimulatorInfo.mIsInterveneAvailable;
				result.IsBeingIntervened = VehicleSimulatorInfo.mIsBeingIntervened;
				result.InterveneCommand = VehicleSimulatorInfo.mInterveneCommand;
			}
			return result;
		}
		private AGVPath ConvertToAGVPath(IVehicleSimulatorInfo VehicleSimulatorInfo)
		{
			AGVPath result = null;
			if (VehicleSimulatorInfo == null) return result;
			if ((VehicleSimulatorInfo.mPath == null || VehicleSimulatorInfo.mPath.Count() == 0) && VehicleSimulatorInfo.mBufferTarget == null)
			{
				result = new AGVPath();
				result.Name = VehicleSimulatorInfo.mName;
				result.PathX = new List<double>();
				result.PathY = new List<double>();
			}
			else
			{
				result = new AGVPath();
				result.Name = VehicleSimulatorInfo.mName;
				result.PathX = VehicleSimulatorInfo.mPath.Select((o) => (double)o.mX).ToList();
				result.PathY = VehicleSimulatorInfo.mPath.Select((o) => (double)o.mY).ToList();
				if (VehicleSimulatorInfo.mBufferTarget != null)
				{
					result.PathX.Insert(0, VehicleSimulatorInfo.mBufferTarget.mX);
					result.PathY.Insert(0, VehicleSimulatorInfo.mBufferTarget.mY);
				}
			}
			return result;
		}
	}
}