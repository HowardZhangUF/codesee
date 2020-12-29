﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator.New
{
	public class SimulatorRemovedEventArgs : EventArgs
	{
		public DateTime OccurTime { get; private set; }
		public string SimulatorName { get; private set; }
		public SimulatorProcess SimulatorProcess { get; private set; }

		public SimulatorRemovedEventArgs(DateTime OccurTime, string SimulatorName, SimulatorProcess SimulatorProcess)
		{
			this.OccurTime = OccurTime;
			this.SimulatorName = SimulatorName;
			this.SimulatorProcess = SimulatorProcess;
		}
	}
}
