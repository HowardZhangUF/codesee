﻿using System;
using System.Collections.Generic;
using System.Linq;
using Geometry;
using GLCore;
using GLStyle;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.CollisionEvent;
using LibraryForVM;

namespace TrafficControlTest.UserControl
{
	public partial class UcMap : System.Windows.Forms.UserControl
	{
		private IVehicleInfoManager rVehicleInfoManager = null;
		private ICollisionEventManager rCollisionEventManager = null;
		private Dictionary<string, int> mIconIdsOfVehicle = new Dictionary<string, int>();
		private Dictionary<string, int> mIconIdsOfVehiclePath = new Dictionary<string, int>();
		private Dictionary<string, int> mIconIdsOfCollisionRegion = new Dictionary<string, int>();

		public UcMap()
		{
			InitializeComponent();
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(ICollisionEventManager CollisionEventManager)
		{
			UnsubscribeEvent_ICollisionEventManager(rCollisionEventManager);
			rCollisionEventManager = CollisionEventManager;
			SubscribeEvent_ICollisionEventManager(rCollisionEventManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, ICollisionEventManager CollisionEventManager)
		{
			Set(VehicleInfoManager);
			Set(CollisionEventManager);
		}
		public void SetStyleFileName(string StyleFileName)
		{
			StyleManager.LoadStyle(StyleFileName);
		}
		/// <summary>註冊圖像 ID</summary>
		public void RegisterIconId(IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null && !string.IsNullOrEmpty(VehicleInfo.mName))
			{
				int VehicleIconId = GLCMD.CMD.SerialNumber.Next();
				int VehiclePathIconId = GLCMD.CMD.AddMultiStripLine("Path", null);

				mIconIdsOfVehicle.Add(VehicleInfo.mName, VehicleIconId);
				mIconIdsOfVehiclePath.Add(VehicleInfo.mName, VehiclePathIconId);
			}
		}
		/// <summary>把圖像加入至地圖</summary>
		public void PrintIcon(IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null && !string.IsNullOrEmpty(VehicleInfo.mName))
			{
				if (VehicleInfo.mLocationCoordinate != null)
				{
					GLCMD.CMD.AddAGV(mIconIdsOfVehicle[VehicleInfo.mName], VehicleInfo.mName, VehicleInfo.mLocationCoordinate.mX, VehicleInfo.mLocationCoordinate.mY, VehicleInfo.mLocationToward);
				}
				if (VehicleInfo.mLocationCoordinate != null && VehicleInfo.mPath != null)
				{
					GLCMD.CMD.SaftyEditMultiGeometry<IPair>(mIconIdsOfVehiclePath[VehicleInfo.mName], true, (line) =>
					{
						line.Clear();
						line.AddRangeIfNotNull(GetPath(VehicleInfo));
					});
				}
			}
		}
		/// <summary>把圖像從地圖中移除</summary>
		public void EraseIcon(IVehicleInfo VehicleInfo)
		{
            Console.WriteLine($"車子{VehicleInfo.mName}被移除");

			if (VehicleInfo != null && !string.IsNullOrEmpty(VehicleInfo.mName))
			{
				GLCMD.CMD.DeleteAGV(mIconIdsOfVehicle[VehicleInfo.mName]);
				GLCMD.CMD.DeleteMulti(mIconIdsOfVehiclePath[VehicleInfo.mName]);

				mIconIdsOfVehicle.Remove(VehicleInfo.mName);
				mIconIdsOfVehiclePath.Remove(VehicleInfo.mName);
			}
		}
		public void RegisterIconId(ICollisionPair CollisionPair)
		{
			if (CollisionPair != null && CollisionPair.mCollisionRegion != null && !string.IsNullOrEmpty(CollisionPair.mName))
			{
				int id = GLCMD.CMD.AddMultiArea("CollisionArea", GetRegion(CollisionPair));
				mIconIdsOfCollisionRegion.Add(CollisionPair.mName, id);
			}
		}
		public void PrintIcon(ICollisionPair CollisionPair)
		{
			if (CollisionPair != null && CollisionPair.mCollisionRegion != null && !string.IsNullOrEmpty(CollisionPair.mName))
			{
				GLCMD.CMD.SaftyEditMultiGeometry<IArea>(mIconIdsOfCollisionRegion[CollisionPair.mName], true, (area) =>
				{
					area.Clear();
					area.AddRangeIfNotNull(GetRegion(CollisionPair));
				});
			}
		}
		public void EraseIcon(ICollisionPair CollisionPair)
		{
			if (CollisionPair != null && CollisionPair.mCollisionRegion != null && !string.IsNullOrEmpty(CollisionPair.mName))
			{
				GLCMD.CMD.DeleteMulti(mIconIdsOfCollisionRegion[CollisionPair.mName]);
				mIconIdsOfCollisionRegion.Remove(CollisionPair.mName);
			}
		}
		public void FocusPoint(int X, int Y)
		{
			gluiCtrl1.Focus(X, Y);
		}
		public bool FocusVehicle(string VehicleName)
		{
			if (GLCMD.CMD.ContainsAGV(VehicleName, out int x, out int y, out double toward))
			{
				gluiCtrl1.Focus(x, y);
				return true;
			}
			else
			{
				return false;
			}
		}

		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{
				CollisionEventManager.ItemAdded += HandleEvent_CollisionEventManagerItemAdded;
				CollisionEventManager.ItemRemoved += HandleEvent_CollisionEventManagerItemRemoved;
				CollisionEventManager.ItemUpdated += HandleEvent_CollisionEventManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_ICollisionEventManager(ICollisionEventManager CollisionEventManager)
		{
			if (CollisionEventManager != null)
			{
				CollisionEventManager.ItemAdded -= HandleEvent_CollisionEventManagerItemAdded;
				CollisionEventManager.ItemRemoved -= HandleEvent_CollisionEventManagerItemRemoved;
				CollisionEventManager.ItemUpdated -= HandleEvent_CollisionEventManagerItemUpdated;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			RegisterIconId(Args.Item);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			EraseIcon(Args.Item);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			PrintIcon(Args.Item);
		}
		private void HandleEvent_CollisionEventManagerItemAdded(object Sender, ItemCountChangedEventArgs<ICollisionPair> Args)
		{
			RegisterIconId(Args.Item);
		}
		private void HandleEvent_CollisionEventManagerItemRemoved(object Sender, ItemCountChangedEventArgs<ICollisionPair> Args)
		{
			EraseIcon(Args.Item);
		}
		private void HandleEvent_CollisionEventManagerItemUpdated(object Sender, ItemUpdatedEventArgs<ICollisionPair> Args)
		{
			PrintIcon(Args.Item);
		}
		private static IEnumerable<IPair> GetPath(IVehicleInfo VehicleInfo)
		{
			List<IPair> result = null;
			if (VehicleInfo.mPath != null && VehicleInfo.mPath.Count() > 0)
			{
				result = new List<IPair>();
				result.Add(new Pair(VehicleInfo.mLocationCoordinate.mX, VehicleInfo.mLocationCoordinate.mY));
				for (int i = 0; i < VehicleInfo.mPath.Count(); ++i)
				{
					result.Add(new Pair(VehicleInfo.mPath.ElementAt(i).mX, VehicleInfo.mPath.ElementAt(i).mY));
				}
			}
			return result;
		}
		private static IEnumerable<IPair> GetPathDetail(IVehicleInfo VehicleInfo)
		{
			List<IPair> result = null;
			if (VehicleInfo.mPathDetail != null && VehicleInfo.mPathDetail.Count() > 0)
			{
				result = new List<IPair>();
				result.Add(new Pair(VehicleInfo.mLocationCoordinate.mX, VehicleInfo.mLocationCoordinate.mY));
				for (int i = 0; i < VehicleInfo.mPathDetail.Count(); ++i)
				{
					result.Add(new Pair(VehicleInfo.mPathDetail.ElementAt(i).mX, VehicleInfo.mPathDetail.ElementAt(i).mY));
				}
			}
			return result;
		}
		private static IEnumerable<IArea> GetRegion(ICollisionPair CollisionPair)
		{
			List<IArea> result = null;
			if (CollisionPair != null && CollisionPair.mCollisionRegion != null)
			{
				result = new List<IArea>();
				result.Add(new Area(CollisionPair.mCollisionRegion.mMinX, CollisionPair.mCollisionRegion.mMinY, CollisionPair.mCollisionRegion.mMaxX, CollisionPair.mCollisionRegion.mMaxY));
			}
			return result;
		}

        private void gluiCtrl1_Load(object sender, EventArgs e)
        {

        }
    }
}
