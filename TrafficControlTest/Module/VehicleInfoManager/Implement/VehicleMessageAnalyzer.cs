﻿using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;

namespace TrafficControlTest.Implement
{
	public class VehicleMessageAnalyzer : IVehicleMessageAnalyzer
	{
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;

		public VehicleMessageAnalyzer(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(VehicleCommunicator, VehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			Unsubscribe_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			Subscribe_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			Unsubscribe_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			Subscribe_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
		}

		private void Subscribe_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.RemoteConnectStateChanged += HandleEvent_VehicleCommunicatorRemoteConnectStateChanged;
				VehicleCommunicator.ReceivedSerializableData += HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		private void Unsubscribe_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.RemoteConnectStateChanged -= HandleEvent_VehicleCommunicatorRemoteConnectStateChanged;
				VehicleCommunicator.ReceivedSerializableData -= HandleEvent_VehicleCommunicatorReceivedSerializableData;
			}
		}
		private void Subscribe_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
			}
		}
		private void Unsubscribe_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
			}
		}
		private void HandleEvent_VehicleCommunicatorRemoteConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			if (NewState == ConnectState.Disconnected)
			{
				if (rVehicleInfoManager.IsExistByIpPort(IpPort))
				{
					rVehicleInfoManager.Remove(rVehicleInfoManager.GetItemByIpPort(IpPort).mName);
				}
			}
		}
		private void HandleEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			if (Data is Serializable)
			{
				if (Data is AGVStatus)
				{
					UpdateIVehicleInfo(IpPort, Data as AGVStatus);
				}
				else if (Data is AGVPath)
				{
					UpdateIVehicleInfo(IpPort, Data as AGVPath);
				}
				else if (Data is RequestMapList && (Data as RequestMapList).Response != null)
				{
					UpdateIVehicleInfo(IpPort, (Data as RequestMapList).Response);
				}
				else if (Data is GetMap && (Data as GetMap).Response != null)
				{
					SaveMap((Data as GetMap).Response);
				}
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			rVehicleCommunicator.SendSerializableData_RequestMapList(Item.mIpPort);
		}
		private void UpdateIVehicleInfo(string IpPort, AGVStatus AgvStatus)
		{
			if (!rVehicleInfoManager.IsExist(AgvStatus.Name))
			{
				IVehicleInfo tmpData = Library.Library.GenerateIVehicleInfo(AgvStatus.Name);
				tmpData.UpdateIpPort(IpPort);
				rVehicleInfoManager.Add(AgvStatus.Name, tmpData);
			}

			rVehicleInfoManager.UpdateItem(AgvStatus.Name, IpPort);
			rVehicleInfoManager.UpdateItem(AgvStatus.Name, AgvStatus.Description.ToString(), Library.Library.GenerateIPoint2D((int)AgvStatus.X, (int)AgvStatus.Y), AgvStatus.Toward, AgvStatus.GoalName, AgvStatus.Velocity, AgvStatus.MapMatch * 100, AgvStatus.Battery, AgvStatus.AlarmMessage);
		}
		private void UpdateIVehicleInfo(string IpPort, AGVPath AgvPath)
		{
			if (!rVehicleInfoManager.IsExist(AgvPath.Name))
			{
				IVehicleInfo tmpData = Library.Library.GenerateIVehicleInfo(AgvPath.Name);
				tmpData.UpdateIpPort(IpPort);
				rVehicleInfoManager.Add(AgvPath.Name, tmpData);
			}

			rVehicleInfoManager.UpdateItem(AgvPath.Name, IpPort);
			rVehicleInfoManager.UpdateItem(AgvPath.Name, ConvertToPoints(AgvPath.PathX, AgvPath.PathY));
		}
		private IEnumerable<IPoint2D> ConvertToPoints(List<double> X, List<double> Y)
		{
			List<IPoint2D> result = new List<IPoint2D>();
			for (int i = 0; i < X.Count; ++i)
			{
				result.Add(Library.Library.GenerateIPoint2D((int)X[i], (int)Y[i]));
			}
			return result;
		}
		private void UpdateIVehicleInfo(string IpPort, List<string> MapList)
		{
			if (rVehicleInfoManager.IsExistByIpPort(IpPort))
			{
				IVehicleInfo tmpData = rVehicleInfoManager.GetItemByIpPort(IpPort);
				tmpData.BeginUpdate();
				if (MapList.Any(o => o.EndsWith("*")))
				{
					tmpData.UpdateCurrentMapName(MapList.First(o => o.EndsWith("*")).Replace("*", ".map"));
				}
				else
				{
					tmpData.UpdateCurrentMapName(string.Empty);
				}
				tmpData.UpdateCurrentMapNameList(MapList);
				tmpData.EndUpdate();
			}
		}
		private void SaveMap(FileInfo MapFile)
		{
			if (!System.IO.Directory.Exists(Library.Library.DefaultLocalMapDirectory)) System.IO.Directory.CreateDirectory(Library.Library.DefaultLocalMapDirectory);
			MapFile.SaveAs(Library.Library.DefaultLocalMapDirectory);
		}
	}
}
