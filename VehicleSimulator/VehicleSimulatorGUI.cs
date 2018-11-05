﻿using Geometry;
using GLCore;
using GLStyle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleSimulator
{
	public partial class VehicleSimulatorGUI : Form
	{
		private VehicleSimulatorProcess process = new VehicleSimulatorProcess();

		public VehicleSimulatorGUI()
		{
			InitializeComponent();

			StyleManager.LoadStyle("Style.ini");
		}

		private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
		{
			SubscribeVehicleSimulatorProcessEvent();

			List<Pair> path1 = new List<Pair>();
			path1.Add(new Pair(9000, -6000));
			path1.Add(new Pair(-8000, -7000));
			path1.Add(new Pair(5000, 5000));
			path1.Add(new Pair(-3000, 4000));
			path1.Add(new Pair(-3000, -9000));

			List<Pair> path2 = new List<Pair>();
			path2.Add(new Pair(-4000, -8000));
			path2.Add(new Pair(8000, -7000));
			path2.Add(new Pair(-9000, 6000));
			path2.Add(new Pair(3000, 4000));
			path2.Add(new Pair(-5000, -5000));

			process.AddVehicleSimualtor("AGV01", 1000, 40);
			process.AddVehicleSimualtor("AGV02", 1000, 40);
			process.VehicleSimulatorMove("AGV01", path1);
			process.VehicleSimulatorMove("AGV02", path2);
			//process.StartCommunication("127.0.0.1", 8000);
		}

		private void VehicleSimulatorGUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			UnsubscribeVehicleSimulatorProcessEvent();
			//process.StopCommunication();
		}

		#region Map Process

		Dictionary<string, int> VehicleIconIDs = new Dictionary<string, int>();

		Dictionary<string, int> VehiclePathIconIDs = new Dictionary<string, int>();

		private void AddAGVIcon(string name)
		{
			if (!VehicleIconIDs.Keys.Contains(name) && !VehiclePathIconIDs.Keys.Contains(name))
			{
				VehicleIconIDs.Add(name, GLCMD.CMD.SerialNumber.Next());
				GLCMD.CMD.AddAGV(VehicleIconIDs[name], name);

				int pathIconID = GLCMD.CMD.AddMultiStripLine("PathLine", null);
				VehiclePathIconIDs.Add(name, pathIconID);
			}
		}

		private void RemoveAGVIcon(string name)
		{
			if (VehicleIconIDs.Keys.Contains(name) && VehiclePathIconIDs.Keys.Contains(name))
			{
				GLCMD.CMD.DeleteAGV(VehicleIconIDs[name]);
				GLCMD.CMD.DeleteMulti(VehiclePathIconIDs[name]);
				VehicleIconIDs.Remove(name);
				VehiclePathIconIDs.Remove(name);
			}
		}

		private void UpdateAGVIcon(string name, int x, int y, double toward)
		{
			if (VehicleIconIDs.Keys.Contains(name))
			{
				GLCMD.CMD.AddAGV(VehicleIconIDs[name], name, x, y, toward);
			}
		}

		private void UpdateAGVPathIcon(string name, List<Pair> path)
		{
			if (VehiclePathIconIDs.Keys.Contains(name))
			{
				GLCMD.CMD.SaftyEditMultiGeometry<IPair>(VehiclePathIconIDs[name], true, (line) =>
				{
					line.Clear();
					line.AddRangeIfNotNull(path);
				});
			}
		}

		#endregion

		#region Process

		private void SubscribeVehicleSimulatorProcessEvent()
		{
			process.VehicleSimulatorAdded += Process_VehicleSimulatorAdded;
			process.VehicleSimulatorRemoved += Process_VehicleSimulatorRemoved;
			process.VehicleSimulatorPositionChanged += Process_VehicleSimulatorPositionChanged;
			process.VehicleSimulatorPathChanged += Process_VehicleSimulatorPathChanged;
			process.VehicleSimulatorStatusChanged += Process_VehicleSimulatorStatusChanged;
			process.DebugMessage += Process_DebugMessage;
		}

		private void UnsubscribeVehicleSimulatorProcessEvent()
		{
			process.VehicleSimulatorAdded -= Process_VehicleSimulatorAdded;
			process.VehicleSimulatorRemoved -= Process_VehicleSimulatorRemoved;
			process.VehicleSimulatorPositionChanged -= Process_VehicleSimulatorPositionChanged;
			process.VehicleSimulatorPathChanged -= Process_VehicleSimulatorPathChanged;
			process.VehicleSimulatorStatusChanged -= Process_VehicleSimulatorStatusChanged;
			process.DebugMessage -= Process_DebugMessage;
		}

		private void Process_VehicleSimulatorAdded(string name)
		{
			AddAGVIcon(name);
		}

		private void Process_VehicleSimulatorRemoved(string name)
		{
			RemoveAGVIcon(name);
		}

		private void Process_VehicleSimulatorPositionChanged(string name, TowardPair position, List<Pair> path)
		{
			UpdateAGVIcon(name, position.Position.X, position.Position.Y, position.Toward.Theta);
			List<Pair> tmp = path.DeepClone();
			tmp.Insert(0, new Pair(position.Position.X, position.Position.Y));
			UpdateAGVPathIcon(name, tmp);
		}

		private void Process_VehicleSimulatorPathChanged(string name, List<Pair> path)
		{
		}

		private void Process_VehicleSimulatorStatusChanged(string name, string status)
		{
		}

		private void Process_DebugMessage(DateTime timeStamp, string category, string message)
		{
		}

		#endregion
	}
}
