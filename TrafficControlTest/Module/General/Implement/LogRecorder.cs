﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General.Interface;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Module.General.Implement
{
	public class LogRecorder : ILogRecorder
	{
		private DatabaseAdapter mDber;
		private string mTableNameOfVehicleState = "CurrentVehicleState";
		private string mTableNameOfMissionState = "MissionState";

		public LogRecorder()
		{
			mDber = new SqliteDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\Log.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			InitializeDatabaseTable();
		}
		public void Start()
		{
			mDber.Start();
		}
		public void Stop()
		{
			mDber.Stop();
		}
		public void RecordGeneralLog(string Timestamp, string Category, string SubCategory, string Message)
		{
			mDber.EnqueueNonQueryCommand($"INSERT INTO {Category} VALUES ('{Timestamp}', '{SubCategory}', '{Message}')");
		}
		public void RecordVehicleInfo(DatabaseDataOperation Action, IVehicleInfo VehicleInfo)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					VehicleInfoDataAdd(VehicleInfo);
					break;
				case DatabaseDataOperation.Remove:
					VehicleInfoDataRemove(VehicleInfo);
					break;
				case DatabaseDataOperation.Update:
					VehicleInfoDataUpdate(VehicleInfo);
					break;
			}
		}
		public void RecordMissionState(DatabaseDataOperation Action, IMissionState MissionState)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					MissionStateDataAdd(MissionState);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					MissionStateDataUpdate(MissionState);
					break;
			}
		}
		//public void RecordCollisionEvent(object Action /*Add, Update, Remove*/, ICollisionPair CollisionPair)
		//{
		//	throw new NotImplementedException();
		//}

		private void InitializeDatabaseTable()
		{
			CreateTableOfGeneralLog("VehicleCommunicator");
			CreateTableOfGeneralLog("VehicleInfoManager");
			CreateTableOfGeneralLog("CollisionEventManager");
			CreateTableOfGeneralLog("CollisionEventDetector");
			CreateTableOfGeneralLog("VehicleControlManager");
			CreateTableOfGeneralLog("CollisionEventHandler");
			CreateTableOfGeneralLog("VehicleControlHandler");
			CreateTableOfGeneralLog("MissionStateManager");
			CreateTableOfGeneralLog("VehicleInfoUpdater");
			CreateTableOfGeneralLog("HostCommunicator");
			CreateTableOfGeneralLog("HostMessageAnalyzer");
			CreateTableOfGeneralLog("MissionDispatcher");
			CreateTableOfGeneralLog("MapFileManager");
			CreateTableOfGeneralLog("MapManager");
			CreateTableOfGeneralLog("MissionStateReporter");
			CreateTableOfGeneralLog("MissionUpdater");
			CreateTableOfCurrentVehicleState();
			CreateTableOfAllMissionState();
		}
		private void CreateTableOfGeneralLog(string TableName)
		{
			mDber.ExecuteNonQueryCommand($"CREATE TABLE IF NOT EXISTS {TableName} (Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, SubCategory TEXT, Message TEXT);");
		}
		private void CreateTableOfCurrentVehicleState()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNameOfVehicleState} (";
			tmp += "ID TEXT UNIQUE, ";
			tmp += "State TEXT, ";
			tmp += "X INTEGER, ";
			tmp += "Y INTEGER, ";
			tmp += "Toward INTEGER, ";
			tmp += "Target TEXT, ";
			tmp += "Velocity REAL, ";
			tmp += "LocationScore REAL, ";
			tmp += "BatteryValue REAL, ";
			tmp += "AlarmMessage TEXT, ";
			tmp += "Path TEXT, ";
			tmp += "IPPort TEXT, ";
			tmp += "MissionID TEXT, ";
			tmp += "InterveneCommand TEXT, ";
			tmp += "MapName TEXT, ";
			tmp += "LastUpdateTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
			mDber.ExecuteNonQueryCommand(tmp);
		}
		private void CreateTableOfAllMissionState()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNameOfMissionState} (";
			tmp += "ID TEXT, ";
			tmp += "Type TEXT, ";
			tmp += "Priority INTEGER, ";
			tmp += "VehicleID TEXT, ";
			tmp += "Parameters TEXT, ";
			tmp += "SourceIPPort TEXT, ";
			tmp += "ExecutorID TEXT, ";
			tmp += "SendState TEXT, ";
			tmp += "ExecuteState TEXT, ";
			tmp += "ReceiveTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "ExecutionStartTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "ExecutionStopTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "LastUpdateTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
			mDber.ExecuteNonQueryCommand(tmp);
		}
		private void VehicleInfoDataAdd(IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO {mTableNameOfVehicleState} VALUES (";
			tmp += $"'{VehicleInfo.mName}', ";
			tmp += $"'{VehicleInfo.mCurrentState}', ";
			tmp += $"{VehicleInfo.mLocationCoordinate.mX.ToString()}, ";
			tmp += $"{VehicleInfo.mLocationCoordinate.mY.ToString()}, ";
			tmp += $"{((int)VehicleInfo.mLocationToward).ToString()}, ";
			tmp += $"'{VehicleInfo.mCurrentTarget}', ";
			tmp += $"{VehicleInfo.mVelocity.ToString("F2")}, ";
			tmp += $"{VehicleInfo.mLocationScore.ToString("F2")}, ";
			tmp += $"{VehicleInfo.mBatteryValue.ToString("F2")}, ";
			tmp += $"'{VehicleInfo.mAlarmMessage}', ";
			tmp += $"'{VehicleInfo.mPathString}', ";
			tmp += $"'{VehicleInfo.mIpPort}', ";
			tmp += $"'{VehicleInfo.mCurrentMissionId}', ";
			tmp += $"'{VehicleInfo.mCurrentInterveneCommand}', ";
			tmp += $"'{VehicleInfo.mCurrentMapName}', ";
			tmp += $"'{VehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}')";
			mDber.EnqueueNonQueryCommand(tmp);
		}
		private void VehicleInfoDataRemove(IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"DELETE FROM {mTableNameOfVehicleState} WHERE ID = '{VehicleInfo.mName}'";
			mDber.EnqueueNonQueryCommand(tmp);
		}
		private void VehicleInfoDataUpdate(IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE {mTableNameOfVehicleState} SET ";
			tmp += $"State = '{VehicleInfo.mCurrentState}', ";
			tmp += $"X = {VehicleInfo.mLocationCoordinate.mX.ToString()}, ";
			tmp += $"Y = {VehicleInfo.mLocationCoordinate.mY.ToString()}, ";
			tmp += $"Toward = {((int)VehicleInfo.mLocationToward).ToString()}, ";
			tmp += $"Target = '{VehicleInfo.mCurrentTarget}', ";
			tmp += $"Velocity = {VehicleInfo.mVelocity.ToString("F2")}, ";
			tmp += $"LocationScore = {VehicleInfo.mLocationScore.ToString("F2")}, ";
			tmp += $"BatteryValue = {VehicleInfo.mBatteryValue.ToString("F2")}, ";
			tmp += $"AlarmMessage = '{VehicleInfo.mAlarmMessage}', ";
			tmp += $"Path = '{VehicleInfo.mPathString}', ";
			tmp += $"IPPort = '{VehicleInfo.mIpPort}', ";
			tmp += $"MissionID = '{VehicleInfo.mCurrentMissionId}', ";
			tmp += $"InterveneCommand = '{VehicleInfo.mCurrentInterveneCommand}', ";
			tmp += $"MapName = '{VehicleInfo.mCurrentMapName}', ";
			tmp += $"LastUpdateTimestamp = '{VehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}' ";
			tmp += $"WHERE ID = '{VehicleInfo.mName}'";
			mDber.EnqueueNonQueryCommand(tmp);
		}
		private void MissionStateDataAdd(IMissionState MissionState)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO {mTableNameOfMissionState} VALUES (";
			tmp += $"'{MissionState.mName}', ";
			tmp += $"'{MissionState.mMission.mMissionType}', ";
			tmp += $"{MissionState.mMission.mPriority.ToString()}, ";
			tmp += $"'{MissionState.mMission.mVehicleId}', ";
			tmp += $"'{MissionState.mMission.mParametersString}', ";
			tmp += $"'{MissionState.mSourceIpPort}', ";
			tmp += $"'{MissionState.mExecutorId}', ";
			tmp += $"'{MissionState.mSendState.ToString()}', ";
			tmp += $"'{MissionState.mExecuteState.ToString()}', ";
			tmp += $"'{MissionState.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{MissionState.mExecutionStartTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{MissionState.mExecutionStopTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{MissionState.mLastUpdate.ToString(Library.Library.TIME_FORMAT)}')";
			mDber.EnqueueNonQueryCommand(tmp);
		}
		private void MissionStateDataUpdate(IMissionState MissionState)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE {mTableNameOfMissionState} SET ";
			tmp += $"Type = '{MissionState.mMission.mMissionType}', ";
			tmp += $"Priority = {MissionState.mMission.mPriority.ToString()}, ";
			tmp += $"VehicleID = '{MissionState.mMission.mVehicleId}', ";
			tmp += $"Parameters = '{MissionState.mMission.mParametersString}', ";
			tmp += $"SourceIPPort = '{MissionState.mSourceIpPort}', ";
			tmp += $"ExecutorID = '{MissionState.mExecutorId}', ";
			tmp += $"SendState = '{MissionState.mSendState.ToString()}', ";
			tmp += $"ExecuteState = '{MissionState.mExecuteState.ToString()}', ";
			tmp += $"ReceiveTimestamp = '{MissionState.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"ExecutionStartTimestamp = '{MissionState.mExecutionStartTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"ExecutionStopTimestamp = '{MissionState.mExecutionStopTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"LastUpdateTimestamp = '{MissionState.mLastUpdate.ToString(Library.Library.TIME_FORMAT)}' ";
			tmp += $"WHERE ID = '{MissionState.mName}'";
			mDber.EnqueueNonQueryCommand(tmp);
		}
	}
}