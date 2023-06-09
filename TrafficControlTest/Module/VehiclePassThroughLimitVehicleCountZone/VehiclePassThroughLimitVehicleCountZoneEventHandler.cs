﻿using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.LimitVehicleCountZone;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.VehiclePassThroughLimitVehicleCountZone
{
	public class VehiclePassThroughLimitVehicleCountZoneEventHandler : SystemWithLoopTask, IVehiclePassThroughLimitVehicleCountZoneEventHandler
	{
		private IVehiclePassThroughLimitVehicleCountZoneEventManager rVehiclePassThroughLimitVehicleCountZoneEventManager = null;
		private IVehicleControlManager rVehicleControlManager = null;
		private object mLock = new object();

		public VehiclePassThroughLimitVehicleCountZoneEventHandler(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager, IVehicleControlManager VehicleControlManager)
		{
			Set(VehiclePassThroughLimitVehicleCountZoneEventManager, VehicleControlManager);
		}
		public void Set(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager)
		{
			UnsubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(rVehiclePassThroughLimitVehicleCountZoneEventManager);
			rVehiclePassThroughLimitVehicleCountZoneEventManager = VehiclePassThroughLimitVehicleCountZoneEventManager;
			SubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(rVehiclePassThroughLimitVehicleCountZoneEventManager);
		}
		public void Set(IVehicleControlManager VehicleControlManager)
		{
			UnsubscribeEvent_IVehicleControlManager(rVehicleControlManager);
			rVehicleControlManager = VehicleControlManager;
			SubscribeEvent_IVehicleControlManager(rVehicleControlManager);
		}
		public void Set(IVehiclePassThroughLimitVehicleCountZoneEventManager VehiclePassThroughLimitVehicleCountZoneEventManager, IVehicleControlManager VehicleControlManager)
		{
			Set(VehiclePassThroughLimitVehicleCountZoneEventManager);
			Set(VehicleControlManager);
		}
		public override string GetSystemInfo()
		{
			return $"Even Count: {rVehiclePassThroughLimitVehicleCountZoneEventManager.mCount}";
		}
		public override void Task()
		{
			Subtask_HandleVehiclePassThroughLimitVehicleCountZoneEvents();
		}

		private void SubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(IVehiclePassThroughLimitVehicleCountZoneEventManager rVehiclePassThroughLimitVehicleCountZoneEventManager)
		{
			if (rVehiclePassThroughLimitVehicleCountZoneEventManager != null)
			{
				rVehiclePassThroughLimitVehicleCountZoneEventManager.ItemAdded += HandleEvent_VehiclePassThroughLimitVehicleCountZoneEventManagerItemAdded;
				rVehiclePassThroughLimitVehicleCountZoneEventManager.ItemRemoved += HandleEvent_VehiclePassThroughLimitVehicleCountZoneEventManagerItemRemoved;
				rVehiclePassThroughLimitVehicleCountZoneEventManager.ItemUpdated += HandleEvent_VehiclePassThroughLimitVehicleCountZoneEventManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehiclePassThroughLimitVehicleCountZoneEventManager(IVehiclePassThroughLimitVehicleCountZoneEventManager rVehiclePassThroughLimitVehicleCountZoneEventManager)
		{
			if (rVehiclePassThroughLimitVehicleCountZoneEventManager != null)
			{
				rVehiclePassThroughLimitVehicleCountZoneEventManager.ItemAdded += HandleEvent_VehiclePassThroughLimitVehicleCountZoneEventManagerItemAdded;
				rVehiclePassThroughLimitVehicleCountZoneEventManager.ItemRemoved += HandleEvent_VehiclePassThroughLimitVehicleCountZoneEventManagerItemRemoved;
				rVehiclePassThroughLimitVehicleCountZoneEventManager.ItemUpdated += HandleEvent_VehiclePassThroughLimitVehicleCountZoneEventManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVehicleControlManager(IVehicleControlManager rVehicleControlManager)
		{
			if (rVehicleControlManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehicleControlManager(IVehicleControlManager rVehicleControlManager)
		{
			if (rVehicleControlManager != null)
			{
				// do nothing
			}
		}
		private void HandleEvent_VehiclePassThroughLimitVehicleCountZoneEventManagerItemAdded(object sender, ItemCountChangedEventArgs<IVehiclePassThroughLimitVehicleCountZoneEvent> e)
		{
			lock (mLock)
			{
				HandleVehiclePassThroughLimitVehicleCountZoneEvent(e.Item);
			}
		}
		private void HandleEvent_VehiclePassThroughLimitVehicleCountZoneEventManagerItemRemoved(object sender, ItemCountChangedEventArgs<IVehiclePassThroughLimitVehicleCountZoneEvent> e)
		{
			lock (mLock)
			{
				RemoveRelatedVehicleControl(e.Item);
				UninterveneRelatedVehicle(e.Item);
			}
		}
		private void HandleEvent_VehiclePassThroughLimitVehicleCountZoneEventManagerItemUpdated(object sender, ItemUpdatedEventArgs<IVehiclePassThroughLimitVehicleCountZoneEvent> e)
		{
			lock (mLock)
			{
				HandleVehiclePassThroughLimitVehicleCountZoneEvent(e.Item);
			}
		}
		private void Subtask_HandleVehiclePassThroughLimitVehicleCountZoneEvents()
		{
			lock (mLock)
			{
				List<IVehiclePassThroughLimitVehicleCountZoneEvent> events = rVehiclePassThroughLimitVehicleCountZoneEventManager.GetItems().ToList();
				for (int i = 0; i < events.Count; ++i)
				{
					HandleVehiclePassThroughLimitVehicleCountZoneEvent(events[i]);
				}
			}
		}
		private void HandleVehiclePassThroughLimitVehicleCountZoneEvent(IVehiclePassThroughLimitVehicleCountZoneEvent VehiclePassThroughLimitVehicleCountZoneEvent)
		{
			if (VehiclePassThroughLimitVehicleCountZoneEvent != null)
			{
				IVehicleInfo Vehicle = VehiclePassThroughLimitVehicleCountZoneEvent.rVehicleInfo;
				IVehicleControl PauseMovingControl = Library.Library.GenerateIVehicleControl(Vehicle.mName, Command.PauseMoving, null, VehiclePassThroughLimitVehicleCountZoneEvent.mName, VehiclePassThroughLimitVehicleCountZoneEvent.ToString());
				IVehicleControl VehiclePastResumeMoving = (rVehicleControlManager.GetItems().ToList().Find(o => o.mVehicleId == Vehicle.mName && o.mCommand.ToString() == "ResumeMoving"));
				if(VehiclePastResumeMoving!=null)
                {
					rVehicleControlManager.Remove(VehiclePastResumeMoving.mName);
                }
				if (!IsIVehicleControlAlreadyExistedInManager(PauseMovingControl)) // 避免重複丟相同的干預
				{
					rVehicleControlManager.Add(PauseMovingControl.mName, PauseMovingControl);
				}
			}
		}
		private bool IsIVehicleControlAlreadyExistedInManager(IVehicleControl VehicleControl)
		{
			return rVehicleControlManager.GetItems().Any(o => o.mVehicleId == VehicleControl.mVehicleId && o.mCommand == VehicleControl.mCommand && o.mParametersString == VehicleControl.mParametersString && o.mCauseId == VehicleControl.mCauseId);
		}
		private void RemoveRelatedVehicleControl(IVehiclePassThroughLimitVehicleCountZoneEvent VehiclePassThroughLimitVehicleCountZoneEvent)
		{
			while (rVehicleControlManager.IsExistByCauseId(VehiclePassThroughLimitVehicleCountZoneEvent.mName))
			{
				IVehicleControl control = rVehicleControlManager.GetItemByCauseId(VehiclePassThroughLimitVehicleCountZoneEvent.mName);
				if (control != null && control.mSendState == SendState.Unsend)
				{
					rVehicleControlManager.UpdateExecuteFailedReason(control.mName, FailedReason.PassThroughLimitVehicleCountZoneEventRemoved);
					rVehicleControlManager.UpdateExecuteState(control.mName, ExecuteState.ExecuteFailed);
				}
				System.Threading.Thread.Sleep(200);
			}
		}
		private void UninterveneRelatedVehicle(IVehiclePassThroughLimitVehicleCountZoneEvent VehiclePassThroughLimitVehicleCountZoneEvent)
		{
			UninterveneVehicle(VehiclePassThroughLimitVehicleCountZoneEvent.rVehicleInfo, VehiclePassThroughLimitVehicleCountZoneEvent.mName, VehiclePassThroughLimitVehicleCountZoneEvent.ToString());
		}
		private void UninterveneVehicle(IVehicleInfo VehicleInfo, string CauseId, string CauseDetail)
		{
			if (VehicleInfo != null)
			{
				// 如果「自走車已經被干預」或「有干預正在送給該自走車」
				if (IsVehicleBeenIntervened(VehicleInfo) || rVehicleControlManager.GetItems().Any(o => o.mVehicleId == VehicleInfo.mName && o.mCommand == Command.PauseMoving && o.mCauseId == CauseId && o.mSendState == SendState.Sending))
				{
					IVehicleControl vehicleControl = Library.Library.GenerateIVehicleControl(VehicleInfo.mName, Command.ResumeMoving, null, CauseId, CauseDetail);
					rVehicleControlManager.Add(vehicleControl.mName, vehicleControl);
				}
			}
		}
		private bool IsVehicleBeenIntervened(IVehicleInfo VehicleInfo) // 判斷自走車是否已經被干預
		{
			return (VehicleInfo.mCurrentState == "Pause" || !string.IsNullOrEmpty(VehicleInfo.mCurrentInterveneCommand));
		}
	}
}
