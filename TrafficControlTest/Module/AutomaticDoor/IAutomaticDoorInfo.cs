﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public interface IAutomaticDoorInfo : IItem
	{
		IRectangle2D mRange { get; }
		string mIpPort { get; }
		bool mIsConnected { get; }
		bool mIsOpened { get; }
		DateTime mLastUpdated { get; }

		void Set(string Name, IRectangle2D Range, string IpPort);
		void UpdateIsConnected(bool IsConnected);
		void UpdateIsOpened(bool IsOpened);
	}
}
