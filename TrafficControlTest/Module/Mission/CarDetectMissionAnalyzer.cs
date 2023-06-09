﻿using System.Collections.Generic;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.Mission
{
	public class CarDetectMissionAnalyzer : MissionAnalyzer
	{
		public static CarDetectMissionAnalyzer mInstance = new CarDetectMissionAnalyzer();
		public override MissionType mMissionType { get; } = MissionType.CarDetect;
		protected override string[] mNecessaryItem { get; } = new string[] { "X", "Y" };
		protected override string[] mOptionalItem { get; } = new string[] { "Head", "MissionID", "VehicleID", "Priority" };

		protected CarDetectMissionAnalyzer() : base() { }
		protected override MissionAnalyzeResult FillIMissionUsingDictionary(ref IMission Mission, ref string AnalyzeFailedDetail)
		{
			List<string> errorItem = new List<string>();

			// Check Paramter X
			int xValue = 0;
			if (!string.IsNullOrEmpty(mItemCollection["X"]) && int.TryParse(mItemCollection["X"], out int x))
			{
				xValue = x;
			}
			else
			{
				errorItem.Add("X");
			}

			// Check Paramter Y
			int yValue = 0;
			if (!string.IsNullOrEmpty(mItemCollection["Y"]) && int.TryParse(mItemCollection["Y"], out int y))
			{
				yValue = y;
			}
			else
			{
				errorItem.Add("Y");
			}

			// Check Parameter Head
			int headValue = int.MaxValue;
			if (!string.IsNullOrEmpty(mItemCollection["Head"]))
			{
				if (int.TryParse(mItemCollection["Head"], out int head))
				{
					headValue = head;
				}
				else
				{
					errorItem.Add("Head");
				}
			}
			else
			{
				headValue = int.MaxValue;
			}

			// Check Parameter Priority
			int priorityValue = 0;
			if (!string.IsNullOrEmpty(mItemCollection["Priority"]))
			{
				if (int.TryParse(mItemCollection["Priority"], out int priority))
				{
					if (priority >= mPriorityMin && priority <= mPriorityMax)
					{
						priorityValue = priority;
					}
					else
					{
						errorItem.Add("Priority");
					}
				}
				else
				{
					errorItem.Add("Priority");
				}
			}
			else
			{
				priorityValue = mPriorityDefault;
			}

			// Export Result
			if (errorItem.Count == 0)
			{
				if (headValue == int.MaxValue)
				{
					Mission = GenerateIMission(mMissionType, mItemCollection["MissionID"], priorityValue, mItemCollection["VehicleID"], new string[] { xValue.ToString(), yValue.ToString() });
				}
				else
				{
					Mission = GenerateIMission(mMissionType, mItemCollection["MissionID"], priorityValue, mItemCollection["VehicleID"], new string[] { xValue.ToString(), yValue.ToString(), headValue.ToString() });
				}
				return MissionAnalyzeResult.Successed;
			}
			else
			{
				AnalyzeFailedDetail = $"Parameter\"{string.Join(",", errorItem)}\"ValueError";
				return MissionAnalyzeResult.Failed;
			}
		}
	}
}
