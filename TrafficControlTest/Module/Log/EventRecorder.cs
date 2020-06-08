﻿using System;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Log
{
	public class EventRecorder : IEventRecorder
	{
		public bool mIsExecuting { get { return rDatabaseAdapter != null ? rDatabaseAdapter.mIsExecuting : false; } }

		private DatabaseAdapter rDatabaseAdapter = null;
		private string mTableNameOfVehicleState = "CurrentVehicleState";
		private string mTableNamePrefixOfHistoryVehicleInfo = "HistoryVehicleInfoOf";
		private string mTableNameOfMissionState = "MissionState";
		private string mTableNameOfHistoryHostCommunication = "HistoryHostCommunication";

		public EventRecorder(DatabaseAdapter DatabaseAdapter)
		{
			Set(DatabaseAdapter);
		}
		public void Set(DatabaseAdapter DatabaseAdapter)
		{
			if (DatabaseAdapter != null)
			{
				rDatabaseAdapter = DatabaseAdapter;
				InitializeDatabaseTable();
			}
		}
		public void Start()
		{
			rDatabaseAdapter.Start();
		}
		public void Stop()
		{
			rDatabaseAdapter.Stop();
		}
		public void CreateTableOfHistoryVehicleInfo(string VehicleName)
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNamePrefixOfHistoryVehicleInfo}{VehicleName.Replace("-", "Dash")} (";
			tmp += "RecordTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "ID TEXT, ";
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
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
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
		public void RecordHistoryVehicleInfo(DatabaseDataOperation Action, DateTime Timestamp, IVehicleInfo VehicleInfo)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					HistoryVehicleInfoDataAdd(Timestamp, VehicleInfo);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					// do nothing
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
		public void RecordHistoryHostCommunication(DatabaseDataOperation Action, DateTime Timestamp, string Event, string IpPort, string Data)
		{
			switch (Action)
			{
				case DatabaseDataOperation.Add:
					HistoryHostCommunicationAdd(Timestamp, Event, IpPort, Data);
					break;
				case DatabaseDataOperation.Remove:
					// do nothing
					break;
				case DatabaseDataOperation.Update:
					// do nothing
					break;
			}
		}

		private void InitializeDatabaseTable()
		{
			CreateTableOfCurrentVehicleState();
			CreateTableOfAllMissionState();
			CreateTableOfHistoryHostCommunication();
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
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
		}
		private void CreateTableOfAllMissionState()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNameOfMissionState} (";
			tmp += "ID TEXT PRIMARY KEY, ";
			tmp += "HostMissionID Text,";
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
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
		}
		private void CreateTableOfHistoryHostCommunication()
		{
			string tmp = string.Empty;
			tmp += $"CREATE TABLE IF NOT EXISTS {mTableNameOfHistoryHostCommunication} (";
			tmp += "No INTEGER PRIMARY KEY AUTOINCREMENT, ";
			tmp += "Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, ";
			tmp += "Event TEXT, ";
			tmp += "IPPort TEXT, ";
			tmp += "Data TEXT)";
			rDatabaseAdapter.ExecuteNonQueryCommand(tmp);
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
			tmp += $"{VehicleInfo.mTranslationVelocity.ToString("F2")}, ";
			tmp += $"{VehicleInfo.mLocationScore.ToString("F2")}, ";
			tmp += $"{VehicleInfo.mBatteryValue.ToString("F2")}, ";
			tmp += $"'{VehicleInfo.mErrorMessage}', ";
			tmp += $"'{VehicleInfo.mPathString}', ";
			tmp += $"'{VehicleInfo.mIpPort}', ";
			tmp += $"'{VehicleInfo.mCurrentMissionId}', ";
			tmp += $"'{VehicleInfo.mCurrentInterveneCommand}', ";
			tmp += $"'{VehicleInfo.mCurrentMapName}', ";
			tmp += $"'{VehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void VehicleInfoDataRemove(IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"DELETE FROM {mTableNameOfVehicleState} WHERE ID = '{VehicleInfo.mName}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
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
			tmp += $"Velocity = {VehicleInfo.mTranslationVelocity.ToString("F2")}, ";
			tmp += $"LocationScore = {VehicleInfo.mLocationScore.ToString("F2")}, ";
			tmp += $"BatteryValue = {VehicleInfo.mBatteryValue.ToString("F2")}, ";
			tmp += $"AlarmMessage = '{VehicleInfo.mErrorMessage}', ";
			tmp += $"Path = '{VehicleInfo.mPathString}', ";
			tmp += $"IPPort = '{VehicleInfo.mIpPort}', ";
			tmp += $"MissionID = '{VehicleInfo.mCurrentMissionId}', ";
			tmp += $"InterveneCommand = '{VehicleInfo.mCurrentInterveneCommand}', ";
			tmp += $"MapName = '{VehicleInfo.mCurrentMapName}', ";
			tmp += $"LastUpdateTimestamp = '{VehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}' ";
			tmp += $"WHERE ID = '{VehicleInfo.mName}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryVehicleInfoDataAdd(DateTime Timestamp, IVehicleInfo VehicleInfo)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO {mTableNamePrefixOfHistoryVehicleInfo}{VehicleInfo.mName.Replace("-", "Dash")} VALUES (";
			tmp += $"'{Timestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{VehicleInfo.mName}', ";
			tmp += $"'{VehicleInfo.mCurrentState}', ";
			tmp += $"{VehicleInfo.mLocationCoordinate.mX.ToString()}, ";
			tmp += $"{VehicleInfo.mLocationCoordinate.mY.ToString()}, ";
			tmp += $"{((int)VehicleInfo.mLocationToward).ToString()}, ";
			tmp += $"'{VehicleInfo.mCurrentTarget}', ";
			tmp += $"{VehicleInfo.mTranslationVelocity.ToString("F2")}, ";
			tmp += $"{VehicleInfo.mLocationScore.ToString("F2")}, ";
			tmp += $"{VehicleInfo.mBatteryValue.ToString("F2")}, ";
			tmp += $"'{VehicleInfo.mErrorMessage}', ";
			tmp += $"'{VehicleInfo.mPathString}', ";
			tmp += $"'{VehicleInfo.mIpPort}', ";
			tmp += $"'{VehicleInfo.mCurrentMissionId}', ";
			tmp += $"'{VehicleInfo.mCurrentInterveneCommand}', ";
			tmp += $"'{VehicleInfo.mCurrentMapName}', ";
			tmp += $"'{VehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT)}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void MissionStateDataAdd(IMissionState MissionState)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO {mTableNameOfMissionState} VALUES (";
			tmp += $"'{MissionState.mName}', ";
			tmp += $"'{MissionState.mMission.mMissionId}', ";
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
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void MissionStateDataUpdate(IMissionState MissionState)
		{
			string tmp = string.Empty;
			tmp += $"UPDATE {mTableNameOfMissionState} SET ";
			tmp += $"Priority = {MissionState.mMission.mPriority.ToString()}, ";
			tmp += $"ExecutorID = '{MissionState.mExecutorId}', ";
			tmp += $"SendState = '{MissionState.mSendState.ToString()}', ";
			tmp += $"ExecuteState = '{MissionState.mExecuteState.ToString()}', ";
			tmp += $"ReceiveTimestamp = '{MissionState.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"ExecutionStartTimestamp = '{MissionState.mExecutionStartTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"ExecutionStopTimestamp = '{MissionState.mExecutionStopTimestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"LastUpdateTimestamp = '{MissionState.mLastUpdate.ToString(Library.Library.TIME_FORMAT)}' ";
			tmp += $"WHERE ID = '{MissionState.mName}' AND ReceiveTimestamp = '{MissionState.mReceivedTimestamp.ToString(Library.Library.TIME_FORMAT)}'";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
		private void HistoryHostCommunicationAdd(DateTime Timestamp, string Event, string IpPort, string Data)
		{
			string tmp = string.Empty;
			tmp += $"INSERT INTO {mTableNameOfHistoryHostCommunication} (Timestamp, Event, IPPort, Data) VALUES (";
			tmp += $"'{Timestamp.ToString(Library.Library.TIME_FORMAT)}', ";
			tmp += $"'{Event}', ";
			tmp += $"'{IpPort}', ";
			tmp += $"'{Data}')";
			rDatabaseAdapter.EnqueueNonQueryCommand(tmp);
		}
	}
}
