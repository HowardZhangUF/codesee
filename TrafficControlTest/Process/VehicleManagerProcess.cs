﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Account;
using TrafficControlTest.Module.CollisionEvent;
using TrafficControlTest.Module.Communication;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.Configure;
using TrafficControlTest.Module.CycleMission;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Log;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.Vehicle;
using static TrafficControlTest.Library.EventHandlerLibrary;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Process
{
	public class VehicleManagerProcess
	{
		public event EventHandler<DebugMessageEventArgs> DebugMessage;
		public event EventHandler<SignificantEventEventArgs> SignificantEvent;
		public event EventHandler<UserLogChangedEventArgs> AccessControlUserLogChanged;

		public bool mIsAnyUserLoggedIn { get { return (mAccessControl != null && !string.IsNullOrEmpty(mAccessControl.mCurrentUser)); } }
		public string mCurrentLoggedInUserName { get { return (mAccessControl != null && !string.IsNullOrEmpty(mAccessControl.mCurrentUser) ? mAccessControl.mCurrentUser : string.Empty); } }

		private bool mIsAllSystemStopped
		{
			get
			{
				return !mImportantEventRecorder.mIsExecuting
					&& !mVehicleCommunicator.mIsExecuting
					&& !mCollisionEventDetector.mIsExecuting
					&& !mVehicleControlHandler.mIsExecuting
					&& !mHostCommunicator.mIsExecuting
					&& !mMissionDispatcher.mIsExecuting
					&& !mCycleMissionGenerator.mIsExecuting;
			}
		}

		private IConfigurator mConfigurator = null;
		private DatabaseAdapter mDatabaseAdapterOfLogRecord = null;
		private DatabaseAdapter mDatabaseAdapterOfEventRecord = null;
		private DatabaseAdapter mDatabaseAdapterOfSystemData = null;
		private ILogRecorder mLogRecorder = null;
		private IEventRecorder mEventRecorder = null;
		private IImportantEventRecorder mImportantEventRecorder = null;
		private ILogExporter mLogExporter = null;
		private IAccountManager mAccountManager = null;
		private IAccessControl mAccessControl = null;
		private IVehicleCommunicator mVehicleCommunicator = null;
		private IVehicleInfoManager mVehicleInfoManager = null;
		private ICollisionEventManager mCollisionEventManager = null;
		private ICollisionEventDetector mCollisionEventDetector = null;
		private IVehicleControlManager mVehicleControlManager = null;
		private ICollisionEventHandler mCollisionEventHandler = null;
		private IVehicleControlHandler mVehicleControlHandler = null;
		private IVehicleControlUpdater mVehicleControlUpdater = null;
		private IMissionStateManager mMissionStateManager = null;
		private IVehicleInfoUpdater mVehicleInfoUpdater = null;
		private IHostCommunicator mHostCommunicator = null;
		private IHostMessageAnalyzer mHostMessageAnalyzer = null;
		private IMissionDispatcher mMissionDispatcher = null;
		private IMapFileManager mMapFileManager = null;
		private IMapManager mMapManager = null;
		private IMissionStateReporter mMissionStateReporter = null;
		private IMissionUpdater mMissionUpdater = null;
		private ICycleMissionGenerator mCycleMissionGenerator = null;

		public VehicleManagerProcess()
		{
			Constructor();
		}
		~VehicleManagerProcess()
		{
			Destructor();
		}
		/// <summary>
		/// 系統開始
		/// </summary>
		/// <remarks>
		/// 系統開始不放在建構式，而是需要外部(介面層)呼叫才開始的原因為：為了讓外部(介面層)訂閱完此物件的所有事件，再讓此物件開始執行，避免外部(介面層)遺漏事件
		/// </remarks>
		public void Start()
		{
			LoadConfigFileAndUpdateSystemConfig();

			mLogRecorder.Start();
			mEventRecorder.Start();
			mAccountManager.Read();
			mImportantEventRecorder.Start();
			mVehicleCommunicator.StartListen();
			mCollisionEventDetector.Start();
			mVehicleControlHandler.Start();
			mHostCommunicator.StartListen();
			mMissionDispatcher.Start();
			mCycleMissionGenerator.Start();
		}
		public void Stop()
		{
			if (mIsAnyUserLoggedIn) mAccessControl.LogOut();
			mCycleMissionGenerator.Stop();
			mMissionDispatcher.Stop();
			mHostCommunicator.StopListen();
			mVehicleControlHandler.Stop();
			mCollisionEventDetector.Stop();
			mVehicleCommunicator.StopListen();
			mImportantEventRecorder.Stop();
			mAccountManager.Save();

			DateTime tmp = DateTime.Now;
			while (!mIsAllSystemStopped)
			{
				if (DateTime.Now.Subtract(tmp).TotalSeconds > 5) break;
				System.Threading.Thread.Sleep(100);
			}

			mEventRecorder.Stop();
			mLogRecorder.Stop();
			tmp = DateTime.Now;
			while (mEventRecorder.mIsExecuting || mLogRecorder.mIsExecuting)
			{
				if (DateTime.Now.Subtract(tmp).TotalSeconds > 5) break;
				System.Threading.Thread.Sleep(100);
			}

			LoadSystemConfigAndUpdateConfigFile();
		}
		public IConfigurator GetReferenceOfIConfigurator()
		{
			return mConfigurator;
		}
		public ILogExporter GetReferenceOfILogExporter()
		{
			return mLogExporter;
		}
		public IImportantEventRecorder GetReferenceOfIImportantEventRecorder()
		{
			return mImportantEventRecorder;
		}
		public IVehicleCommunicator GetReferenceOfIVehicleCommunicator()
		{
			return mVehicleCommunicator;
		}
		public IVehicleInfoManager GetReferenceOfIVehicleInfoManager()
		{
			return mVehicleInfoManager;
		}
		public ICollisionEventManager GetReferenceOfICollisionEventManager()
		{
			return mCollisionEventManager;
		}
		public ICollisionEventDetector GetReferenceOfICollisionEventDetector()
		{
			return mCollisionEventDetector;
		}
		public IVehicleControlHandler GetReferenceOfIVehicleControlHandler()
		{
			return mVehicleControlHandler;
		}
		public IMissionStateManager GetReferenceOfIMissionStateManager()
		{
			return mMissionStateManager;
		}
		public IHostCommunicator GetReferenceOfIHostCommunicator()
		{
			return mHostCommunicator;
		}
		public IMissionDispatcher GetReferenceOfIMissionDispatcher()
		{
			return mMissionDispatcher;
		}
		public IMapFileManager GetReferenceOfIMapFileManager()
		{
			return mMapFileManager;
		}
		public IMapManager GetReferenceOfIMapManager()
		{
			return mMapManager;
		}
		public ICycleMissionGenerator GetReferenceOfCycleMissionGenerator()
		{
			return mCycleMissionGenerator;
		}
		public DatabaseAdapter GetReferenceOfDatabaseAdapterOfLogRecord()
		{
			return mDatabaseAdapterOfLogRecord;
		}
		public DatabaseAdapter GetReferenceOfDatabaseAdapterOfEventRecord()
		{
			return mDatabaseAdapterOfEventRecord;
		}
		public bool AccessControlLogIn(string Password)
		{
			return mAccessControl.LogIn(Password);
		}
		public bool AccessControlLogOut()
		{
			return mAccessControl.LogOut();
		}
		public int GetCountOfOnlineVehicle()
		{
			return mVehicleInfoManager.mCount;
		}

		private void Constructor()
		{
			mDatabaseAdapterOfLogRecord = GenerateDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\Log.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			mDatabaseAdapterOfEventRecord = GenerateDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\Event.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			mDatabaseAdapterOfSystemData = GenerateDatabaseAdapter($"{DatabaseAdapter.mDirectoryNameOfFiles}\\SystemData.db", string.Empty, string.Empty, string.Empty, string.Empty, false);
			mLogRecorder = GenerateILogRecorder(mDatabaseAdapterOfLogRecord);
			mEventRecorder = GenerateIEventRecorder(mDatabaseAdapterOfEventRecord);
			mAccountManager = GenerateIAccountManager(mDatabaseAdapterOfSystemData);

			SubscribeEvent_Exception();

			UnsubscribeEvent_IConfigurator(mConfigurator);
			mConfigurator = GenerateIConfigurator("Application.config");
			SubscribeEvent_IConfigurator(mConfigurator);

			UnsubscribeEvent_ILogExporter(mLogExporter);
			mLogExporter = GenerateILogExporter();
			mLogExporter.AddDirectoryPaths(new List<string> { ".\\Database", ".\\Map", ".\\Exception", ".\\VMLog" });
			mLogExporter.AddFilePaths(new List<string> { ".\\Application.config" });
			SubscribeEvent_ILogExporter(mLogExporter);

			UnsubscribeEvent_IAccessControl(mAccessControl);
			mAccessControl = GenerateIAccessControl(mAccountManager);
			SubscribeEvent_IAccessControl(mAccessControl);

			UnsubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);
			mVehicleCommunicator = GenerateIVehicleCommunicator();
			SubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);

			UnsubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);
			mVehicleInfoManager = GenerateIVehicleInfoManager();
			SubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);

			UnsubscribeEvent_ICollisionEventManager(mCollisionEventManager);
			mCollisionEventManager = GenerateICollisionEventManager();
			SubscribeEvent_ICollisionEventManager(mCollisionEventManager);

			UnsubscribeEvent_ICollisionEventDetector(mCollisionEventDetector);
			mCollisionEventDetector = GenerateICollisionEventDetector(mVehicleInfoManager, mCollisionEventManager);
			SubscribeEvent_ICollisionEventDetector(mCollisionEventDetector);

			UnsubscribeEvent_IVehicleControlManager(mVehicleControlManager);
			mVehicleControlManager = GenerateIVehicleControlManager();
			SubscribeEvent_IVehicleControlManager(mVehicleControlManager);

			UnsubscribeEvent_ICollisionEventHandler(mCollisionEventHandler);
			mCollisionEventHandler = GenerateICollisionEventHandler(mCollisionEventManager, mVehicleControlManager, mVehicleInfoManager);
			SubscribeEvent_ICollisionEventHandler(mCollisionEventHandler);

			UnsubscribeEvent_IVehicleControlHandler(mVehicleControlHandler);
			mVehicleControlHandler = GenerateIVehicleControlHandler(mVehicleControlManager, mVehicleInfoManager, mVehicleCommunicator);
			SubscribeEvent_IVehicleControlHandler(mVehicleControlHandler);

			UnsubscribeEvent_IVehicleControlUpdater(mVehicleControlUpdater);
			mVehicleControlUpdater = GenerateIVehicleControlUpdater(mVehicleControlManager, mVehicleInfoManager, mVehicleCommunicator);
			SubscribeEvent_IVehicleControlUpdater(mVehicleControlUpdater);

			UnsubscribeEvent_IMissionStateManager(mMissionStateManager);
			mMissionStateManager = GenerateIMissionStateManager();
			SubscribeEvent_IMissionStateManager(mMissionStateManager);

			UnsubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);
			mVehicleInfoUpdater = GenerateIVehicleInfoUpdater(mVehicleCommunicator, mMissionStateManager, mVehicleInfoManager);
			SubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);

			UnsubscribeEvent_IHostCommunicator(mHostCommunicator);
			mHostCommunicator = GenerateIHostCommunicator();
			SubscribeEvent_IHostCommunicator(mHostCommunicator);

			UnsubscribeEvent_IImportantEventRecorder(mImportantEventRecorder);
			mImportantEventRecorder = GenerateIImportantEventRecorder(mEventRecorder, mVehicleInfoManager, mMissionStateManager, mHostCommunicator);
			SubscribeEvent_IImportantEventRecorder(mImportantEventRecorder);

			UnsubscribeEvent_IHostMessageAnalyzer(mHostMessageAnalyzer);
			mHostMessageAnalyzer = GenerateIHostMessageAnalyzer(mHostCommunicator, mVehicleInfoManager, mMissionStateManager, GetMissionAnalyzers());
			SubscribeEvent_IHostMessageAnalyzer(mHostMessageAnalyzer);

			UnsubscribeEvent_IMissionDispatcher(mMissionDispatcher);
			mMissionDispatcher = GenerateIMissionDispatcher(mMissionStateManager, mVehicleInfoManager, mVehicleCommunicator);
			SubscribeEvent_IMissionDispatcher(mMissionDispatcher);

			UnsubscribeEvent_IMapFileManager(mMapFileManager);
			mMapFileManager = GenerateIMapFileManager();
			SubscribeEvent_IMapFileManager(mMapFileManager);

			UnsubscribeEvent_IMapManager(mMapManager);
			mMapManager = GenerateIMapManager(mVehicleCommunicator, mVehicleInfoManager, mMapFileManager);
			SubscribeEvent_IMapManager(mMapManager);

			UnsubscribeEvent_IMissionStateReporter(mMissionStateReporter);
			mMissionStateReporter = GenerateIMissionStateReporter(mMissionStateManager, mHostCommunicator);
			SubscribeEvent_IMissionStateReporter(mMissionStateReporter);

			UnsubscribeEvent_IMissionUpdater(mMissionUpdater);
			mMissionUpdater = GenerateIMissionUpdater(mVehicleCommunicator, mVehicleInfoManager, mMissionStateManager, mMapManager);
			SubscribeEvent_IMissionUpdater(mMissionUpdater);

			UnsubscribeEvent_ICycleMissionGenerator(mCycleMissionGenerator);
			mCycleMissionGenerator = GenerateICycleMissionGenerator(mVehicleInfoManager, mMissionStateManager);
			SubscribeEvent_ICycleMissionGenerator(mCycleMissionGenerator);
		}
		private void Destructor()
		{
			UnsubscribeEvent_IAccessControl(mAccessControl);
			mAccessControl = null;

			UnsubscribeEvent_IVehicleCommunicator(mVehicleCommunicator);
			mVehicleCommunicator = null;

			UnsubscribeEvent_IVehicleInfoManager(mVehicleInfoManager);
			mVehicleInfoManager = null;

			UnsubscribeEvent_ICollisionEventManager(mCollisionEventManager);
			mCollisionEventManager = null;

			UnsubscribeEvent_ICollisionEventDetector(mCollisionEventDetector);
			mCollisionEventDetector = null;

			UnsubscribeEvent_IVehicleControlManager(mVehicleControlManager);
			mVehicleControlManager = null;

			UnsubscribeEvent_ICollisionEventHandler(mCollisionEventHandler);
			mCollisionEventHandler = null;

			UnsubscribeEvent_IVehicleControlHandler(mVehicleControlHandler);
			mVehicleControlHandler = null;

			UnsubscribeEvent_IVehicleControlUpdater(mVehicleControlUpdater);
			mVehicleControlUpdater = null;

			UnsubscribeEvent_IMissionStateManager(mMissionStateManager);
			mMissionStateManager = null;

			UnsubscribeEvent_IVehicleInfoUpdater(mVehicleInfoUpdater);
			mVehicleInfoUpdater = null;

			UnsubscribeEvent_IHostCommunicator(mHostCommunicator);
			mHostCommunicator = null;

			UnsubscribeEvent_IHostMessageAnalyzer(mHostMessageAnalyzer);
			mHostMessageAnalyzer = null;

			UnsubscribeEvent_IMissionDispatcher(mMissionDispatcher);
			mMissionDispatcher = null;

			UnsubscribeEvent_IMissionStateReporter(mMissionStateReporter);
			mMissionStateReporter = null;

			UnsubscribeEvent_IMissionUpdater(mMissionUpdater);
			mMissionUpdater = null;

			UnsubscribeEvent_ICycleMissionGenerator(mCycleMissionGenerator);
			mCycleMissionGenerator = null;

			UnsubscribeEvent_IImportantEventRecorder(mImportantEventRecorder);
			mImportantEventRecorder = null;

			UnsubscribeEvent_ILogExporter(mLogExporter);
			mLogExporter = null;

			UnsubscribeEvent_IConfigurator(mConfigurator);
			mConfigurator = null;

			mAccountManager = null;
			mEventRecorder = null;
			mLogRecorder = null;
			mDatabaseAdapterOfSystemData = null;
			mDatabaseAdapterOfEventRecord = null;
			mDatabaseAdapterOfLogRecord = null;
		}
		private void LoadConfigFileAndUpdateSystemConfig()
		{
			mConfigurator.Load();
			mLogExporter.SetConfig("BaseDirectory", mConfigurator.GetValue("LogExporter/BaseDirectory"));
			mLogExporter.SetConfig("ExportDirectoryNamePrefix", mConfigurator.GetValue("LogExporter/ExportDirectoryNamePrefix"));
			mLogExporter.SetConfig("ExportDirectoryNameTimeFormat", mConfigurator.GetValue("LogExporter/ExportDirectoryNameTimeFormat"));
			mImportantEventRecorder.SetConfig("TimePeriod", mConfigurator.GetValue("ImportantEventRecorder/TimePeriod"));
			mVehicleCommunicator.SetConfig("ListenPort", mConfigurator.GetValue("VehicleCommunicator/ListenPort"));
			mVehicleCommunicator.SetConfig("TimePeriod", mConfigurator.GetValue("VehicleCommunicator/TimePeriod"));
			mCollisionEventDetector.SetConfig("TimePeriod", mConfigurator.GetValue("CollisionEventDetector/TimePeriod"));
			mCollisionEventDetector.SetConfig("NeighborPointAmount", mConfigurator.GetValue("CollisionEventDetector/NeighborPointAmount"));
			mCollisionEventDetector.SetConfig("VehicleLocationScoreThreshold", mConfigurator.GetValue("CollisionEventDetector/VehicleLocationScoreThreshold"));
			mVehicleControlHandler.SetConfig("TimePeriod", mConfigurator.GetValue("VehicleControlHandler/TimePeriod"));
			mHostCommunicator.SetConfig("ListenPort", mConfigurator.GetValue("HostCommunicator/ListenPort"));
			mHostCommunicator.SetConfig("TimePeriod", mConfigurator.GetValue("HostCommunicator/TimePeriod"));
			mMissionDispatcher.SetConfig("TimePeriod", mConfigurator.GetValue("MissionDispatcher/TimePeriod"));
			mMapFileManager.SetConfig("MapFileDirectory", mConfigurator.GetValue("MapFileManager/MapFileDirectory"));
			mMapManager.SetConfig("AutoLoadMap", mConfigurator.GetValue("MapManager/AutoLoadMap"));
			mCycleMissionGenerator.SetConfig("TimePeriod", mConfigurator.GetValue("CycleMissionGenerator/TimePeriod"));
		}
		private void LoadSystemConfigAndUpdateConfigFile()
		{
			mConfigurator.SetValue("CycleMissionGenerator/TimePeriod", mCycleMissionGenerator.GetConfig("TimePeriod"));
			mConfigurator.SetValue("MapManager/AutoLoadMap", mMapManager.GetConfig("AutoLoadMap"));
			mConfigurator.SetValue("MapFileManager/MapFileDirectory", mMapFileManager.GetConfig("MapFileDirectory"));
			mConfigurator.SetValue("MissionDispatcher/TimePeriod", mMissionDispatcher.GetConfig("TimePeriod"));
			mConfigurator.SetValue("HostCommunicator/TimePeriod", mHostCommunicator.GetConfig("TimePeriod"));
			mConfigurator.SetValue("HostCommunicator/ListenPort", mHostCommunicator.GetConfig("ListenPort"));
			mConfigurator.SetValue("VehicleControlHandler/TimePeriod", mVehicleControlHandler.GetConfig("TimePeriod"));
			mConfigurator.SetValue("CollisionEventDetector/VehicleLocationScoreThreshold", mCollisionEventDetector.GetConfig("VehicleLocationScoreThreshold"));
			mConfigurator.SetValue("CollisionEventDetector/NeighborPointAmount", mCollisionEventDetector.GetConfig("NeighborPointAmount"));
			mConfigurator.SetValue("CollisionEventDetector/TimePeriod", mCollisionEventDetector.GetConfig("TimePeriod"));
			mConfigurator.SetValue("VehicleCommunicator/TimePeriod", mVehicleCommunicator.GetConfig("TimePeriod"));
			mConfigurator.SetValue("VehicleCommunicator/ListenPort", mVehicleCommunicator.GetConfig("ListenPort"));
			mConfigurator.SetValue("ImportantEventRecorder/TimePeriod", mImportantEventRecorder.GetConfig("TimePeriod"));
			mConfigurator.SetValue("LogExporter/ExportDirectoryNameTimeFormat", mLogExporter.GetConfig("ExportDirectoryNameTimeFormat"));
			mConfigurator.SetValue("LogExporter/ExportDirectoryNamePrefix", mLogExporter.GetConfig("ExportDirectoryNamePrefix"));
			mConfigurator.SetValue("LogExporter/BaseDirectory", mLogExporter.GetConfig("BaseDirectory"));
			mConfigurator.Save();
		}
		private void SubscribeEvent_Exception()
		{
			System.Windows.Forms.Application.ThreadException += (sender, e) =>
			{
				string directory = ".\\Exception";
				string file = $".\\Exception\\ExceptionThread{DateTime.Now.ToString("yyyyMMdd")}.txt";
				string message = $"{DateTime.Now.ToString(TIME_FORMAT)} - [ThreadException] - {e.Exception.ToString()}\r\n";

				if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
				if (!System.IO.File.Exists(file)) System.IO.File.Create(file).Close();
				System.IO.File.AppendAllText(file, message);
			};
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				string directory = ".\\Exception";
				string file = $".\\Exception\\ExceptionUnhandled{DateTime.Now.ToString("yyyyMMdd")}.txt";
				string message = $"{DateTime.Now.ToString(TIME_FORMAT)} - [UnhandledException] - {e.ExceptionObject.ToString()}\r\n";

				if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
				if (!System.IO.File.Exists(file)) System.IO.File.Create(file).Close();
				System.IO.File.AppendAllText(file, message);
			};
		}
		private void SubscribeEvent_IConfigurator(IConfigurator Configurator)
		{
			if (Configurator != null)
			{
				Configurator.ConfigFileLoaded += HandleEvent_ConfiguratorConfigFileLoaded;
				Configurator.ConfigFileSaved += HandleEvent_ConfiguratorConfigFileSaved;
				Configurator.ConfigurationUpdated += HandleEvent_ConfiguratorConfigurationUpdated;
			}
		}
		private void UnsubscribeEvent_IConfigurator(IConfigurator Configurator)
		{
			if (Configurator != null)
			{
				Configurator.ConfigFileLoaded -= HandleEvent_ConfiguratorConfigFileLoaded;
				Configurator.ConfigFileSaved -= HandleEvent_ConfiguratorConfigFileSaved;
				Configurator.ConfigurationUpdated -= HandleEvent_ConfiguratorConfigurationUpdated;
			}
		}
		private void SubscribeEvent_ILogExporter(ILogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ConfigUpdated += HandleEvent_LogExporterConfigUpdated;
				LogExporter.ExportStarted += HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted += HandleEvent_LogExporterExportCompleted;
			}
		}
		private void UnsubscribeEvent_ILogExporter(ILogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ConfigUpdated -= HandleEvent_LogExporterConfigUpdated;
				LogExporter.ExportStarted -= HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted -= HandleEvent_LogExporterExportCompleted;
			}
		}
		private void SubscribeEvent_IAccessControl(IAccessControl AccessControl)
		{
			if (AccessControl != null)
			{
				AccessControl.UserLogChanged += HandleEvent_AccessControlUserLogChanged;
			}
		}
		private void UnsubscribeEvent_IAccessControl(IAccessControl AccessControl)
		{
			if (AccessControl != null)
			{
				AccessControl.UserLogChanged -= HandleEvent_AccessControlUserLogChanged;
			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SystemStatusChanged += HandleEvent_VehicleCommunicatorSystemStatusChanged;
				VehicleCommunicator.ConfigUpdated += HandleEvent_VehicleCommunicatorConfigUpdated;
				VehicleCommunicator.LocalListenStateChanged += HandleEvent_VehicleCommunicatorLocalListenStateChagned;
				VehicleCommunicator.RemoteConnectStateChanged += HandleEvent_VehicleCommunicatorRemoteConnectStateChagned;
				VehicleCommunicator.SentSerializableData += HandleEvent_VehicleCommunicatorSentSerializableData;
				VehicleCommunicator.ReceivedSerializableData += HandleEvent_VehicleCommunicatorReceivedSerializableData;
				VehicleCommunicator.SentSerializableDataSuccessed += HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed;
				VehicleCommunicator.SentSerializableDataFailed += HandleEvent_VehicleCommunicatorSentSerializableDataFailed;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.SystemStatusChanged -= HandleEvent_VehicleCommunicatorSystemStatusChanged;
				VehicleCommunicator.ConfigUpdated -= HandleEvent_VehicleCommunicatorConfigUpdated;
				VehicleCommunicator.LocalListenStateChanged -= HandleEvent_VehicleCommunicatorLocalListenStateChagned;
				VehicleCommunicator.RemoteConnectStateChanged -= HandleEvent_VehicleCommunicatorRemoteConnectStateChagned;
				VehicleCommunicator.SentSerializableData -= HandleEvent_VehicleCommunicatorSentSerializableData;
				VehicleCommunicator.ReceivedSerializableData -= HandleEvent_VehicleCommunicatorReceivedSerializableData;
				VehicleCommunicator.SentSerializableDataSuccessed -= HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed;
				VehicleCommunicator.SentSerializableDataFailed -= HandleEvent_VehicleCommunicatorSentSerializableDataFailed;
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
		private void SubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStatusChanged += HandleEvent_CollisionEventDetectorSystemStatusChanged;
				CollisionEventDetector.ConfigUpdated += HandleEvent_CollisionEventDetectorConfigUpdated;
			}
		}
		private void UnsubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStatusChanged -= HandleEvent_CollisionEventDetectorSystemStatusChanged;
				CollisionEventDetector.ConfigUpdated -= HandleEvent_CollisionEventDetectorConfigUpdated;
			}
		}
		private void SubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemAdded += HandleEvent_VehicleControlManagerItemAdded;
				VehicleControlManager.ItemRemoved += HandleEvent_VehicleControlManagerItemRemoved;
				VehicleControlManager.ItemUpdated += HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{
				VehicleControlManager.ItemAdded -= HandleEvent_VehicleControlManagerItemAdded;
				VehicleControlManager.ItemRemoved -= HandleEvent_VehicleControlManagerItemRemoved;
				VehicleControlManager.ItemUpdated -= HandleEvent_VehicleControlManagerItemUpdated;
			}
		}
		private void SubscribeEvent_ICollisionEventHandler(ICollisionEventHandler CollisionEventHandler)
		{
			if (CollisionEventHandler != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_ICollisionEventHandler(ICollisionEventHandler CollisionEventHandler)
		{
			if (CollisionEventHandler != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IVehicleControlHandler(IVehicleControlHandler VehicleControlHandler)
		{
			if (VehicleControlHandler != null)
			{
				VehicleControlHandler.SystemStatusChanged += HandleEvent_VehicleControlHandlerSystemStatusChanged;
				VehicleControlHandler.ConfigUpdated += HandleEvent_VehicleControlHandlerConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControlHandler(IVehicleControlHandler VehicleControlHandler)
		{
			if (VehicleControlHandler != null)
			{
				VehicleControlHandler.SystemStatusChanged -= HandleEvent_VehicleControlHandlerSystemStatusChanged;
				VehicleControlHandler.ConfigUpdated -= HandleEvent_VehicleControlHandlerConfigUpdated;
			}
		}
		private void SubscribeEvent_IVehicleControlUpdater(IVehicleControlUpdater VehicleControlUpdater)
		{
			if (VehicleControlUpdater != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehicleControlUpdater(IVehicleControlUpdater VehicleControlUpdater)
		{
			if (VehicleControlUpdater != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded += HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemRemoved += HandleEvent_MissionStateManagerItemRemoved;
				MissionStateManager.ItemUpdated += HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded -= HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemRemoved -= HandleEvent_MissionStateManagerItemRemoved;
				MissionStateManager.ItemUpdated -= HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVehicleInfoUpdater(IVehicleInfoUpdater VehicleInfoUpdater)
		{
			if (VehicleInfoUpdater != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IVehicleInfoUpdater(IVehicleInfoUpdater VehicleInfoUpdater)
		{
			if (VehicleInfoUpdater != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.SystemStatusChanged += HandleEvent_HostCommunicatorSystemStatusChanged;
				HostCommunicator.ConfigUpdated += HandleEvent_HostCommunicatorConfigUpdated;
				HostCommunicator.LocalListenStateChanged += HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.RemoteConnectStateChanged += HandleEvent_HostCommunicatorRemoteConnectStateChanged;
				HostCommunicator.SentString += HandleEvent_HostCommunicatorSentString;
				HostCommunicator.ReceivedString += HandleEvent_HostCommunicatorReceivedString;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.SystemStatusChanged -= HandleEvent_HostCommunicatorSystemStatusChanged;
				HostCommunicator.ConfigUpdated -= HandleEvent_HostCommunicatorConfigUpdated;
				HostCommunicator.LocalListenStateChanged -= HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.RemoteConnectStateChanged -= HandleEvent_HostCommunicatorRemoteConnectStateChanged;
				HostCommunicator.SentString -= HandleEvent_HostCommunicatorSentString;
				HostCommunicator.ReceivedString -= HandleEvent_HostCommunicatorReceivedString;
			}
		}
		private void SubscribeEvent_IHostMessageAnalyzer(IHostMessageAnalyzer HostMessageAnalyzer)
		{
			if (HostMessageAnalyzer != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IHostMessageAnalyzer(IHostMessageAnalyzer HostMessageAnalyzer)
		{
			if (HostMessageAnalyzer != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStatusChanged += HandleEvent_MissionDispatcherSystemStatusChanged;
				MissionDispatcher.ConfigUpdated += HandleEvent_MissionDispatcherConfigUpdated;
				MissionDispatcher.MissionDispatched += HandleEvent_MissionDispatcherMissionDispatched;
			}
		}
		private void UnsubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStatusChanged -= HandleEvent_MissionDispatcherSystemStatusChanged;
				MissionDispatcher.ConfigUpdated -= HandleEvent_MissionDispatcherConfigUpdated;
				MissionDispatcher.MissionDispatched -= HandleEvent_MissionDispatcherMissionDispatched;
			}
		}
		private void SubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.ConfigUpdated += HandleEvent_MapFileManagerConfigUpdated;
				MapFileManager.MapFileAdded += HandleEvent_MapFileManagerMapFileAdded;
				MapFileManager.MapFileRemoved += HandleEvent_MapFileManagerMapFileRemoved;
			}
		}
		private void UnsubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.ConfigUpdated -= HandleEvent_MapFileManagerConfigUpdated;
				MapFileManager.MapFileAdded -= HandleEvent_MapFileManagerMapFileAdded;
				MapFileManager.MapFileRemoved -= HandleEvent_MapFileManagerMapFileRemoved;
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.ConfigUpdated += HandleEvent_MapManagerConfigUpdated;
				MapManager.LoadMapSuccessed += HandleEvent_MapManagerLoadMapSuccessed;
				MapManager.LoadMapFailed += HandleEvent_MapManagerLoadMapFailed;
				MapManager.SynchronizeMapStarted += HandleEvent_MapManagerSynchronizeMapStarted;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.ConfigUpdated -= HandleEvent_MapManagerConfigUpdated;
				MapManager.LoadMapSuccessed -= HandleEvent_MapManagerLoadMapSuccessed;
				MapManager.LoadMapFailed -= HandleEvent_MapManagerLoadMapFailed;
				MapManager.SynchronizeMapStarted -= HandleEvent_MapManagerSynchronizeMapStarted;
			}
		}
		private void SubscribeEvent_IMissionStateReporter(IMissionStateReporter MissionStateReporter)
		{
			if (MissionStateReporter != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IMissionStateReporter(IMissionStateReporter MissionStateReporter)
		{
			if (MissionStateReporter != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMissionUpdater(IMissionUpdater MissionUpdater)
		{
			if (MissionUpdater != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IMissionUpdater(IMissionUpdater MissionUpdater)
		{
			if (MissionUpdater != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.SystemStatusChanged += HandleEvent_CycleMissionGeneratorSystemStatusChanged;
				CycleMissionGenerator.ConfigUpdated += HandleEvent_CycleMissionGeneratorConfigUpdated;
				CycleMissionGenerator.CycleMissionAssigned += HandleEvent_CycleMissionGeneratorCycleMissionAssigned;
				CycleMissionGenerator.CycleMissionUnassigned += HandleEvent_CycleMissionGeneratorCycleMissionUnassigned;
				CycleMissionGenerator.CycleMissionExecutedIndexChanged += HandleEvent_CycleMissionGeneratorCycleExecutedIndexChanged;
			}
		}
		private void UnsubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.SystemStatusChanged -= HandleEvent_CycleMissionGeneratorSystemStatusChanged;
				CycleMissionGenerator.ConfigUpdated -= HandleEvent_CycleMissionGeneratorConfigUpdated;
				CycleMissionGenerator.CycleMissionAssigned -= HandleEvent_CycleMissionGeneratorCycleMissionAssigned;
				CycleMissionGenerator.CycleMissionUnassigned -= HandleEvent_CycleMissionGeneratorCycleMissionUnassigned;
				CycleMissionGenerator.CycleMissionExecutedIndexChanged -= HandleEvent_CycleMissionGeneratorCycleExecutedIndexChanged;
			}
		}
		private void SubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStatusChanged += HandleEvent_ImportantEventRecorderSystemStatusChanged;
				ImportantEventRecorder.ConfigUpdated += HandleEvent_ImportantEventRecorderConfigUpdated;
			}
		}
		private void UnsubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStatusChanged -= HandleEvent_ImportantEventRecorderSystemStatusChanged;
				ImportantEventRecorder.ConfigUpdated -= HandleEvent_ImportantEventRecorderConfigUpdated;
			}
		}
		protected virtual void RaiseEvent_DebugMessage(string OccurTime, string Category, string SubCategory, string Message, bool Sync = true)
		{
			if (Sync)
			{
				DebugMessage?.Invoke(this, new DebugMessageEventArgs(OccurTime, Category, SubCategory, Message));
			}
			else
			{
				Task.Run(() => { DebugMessage?.Invoke(this, new DebugMessageEventArgs(OccurTime, Category, SubCategory, Message)); });
			}
		}
		protected virtual void RaiseEvent_SignificantEvent(DateTime OccurTime, SignificantEventCategory Category, string Info, bool Sync = true)
		{
			RaiseEvent_SignificantEvent(OccurTime.ToString(TIME_FORMAT), Category.ToString(), Info, Sync);
		}
		protected virtual void RaiseEvent_SignificantEvent(string OccurTime, string Category, string Info, bool Sync = true)
		{
			if (Sync)
			{
				SignificantEvent?.Invoke(this, new SignificantEventEventArgs(OccurTime, Category, Info));
			}
			else
			{
				Task.Run(() => { SignificantEvent?.Invoke(this, new SignificantEventEventArgs(OccurTime, Category, Info)); });
			}
		}
		protected virtual void RaiseEvent_AccessControlUserLogChanged(DateTime OccurTime, string UserName, AccountRank UserRank, bool IsLogin, bool Sync = true)
		{
			if (Sync)
			{
				AccessControlUserLogChanged?.Invoke(this, new UserLogChangedEventArgs(OccurTime, UserName, UserRank, IsLogin));
			}
			else
			{
				Task.Run(() => { AccessControlUserLogChanged?.Invoke(this, new UserLogChangedEventArgs(OccurTime, UserName, UserRank, IsLogin)); });
			}
		}
		private void HandleEvent_ConfiguratorConfigFileLoaded(object Sender, ConfigFileLoadedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "Configurator", "ConfigFileLoaded", $"FilePath: {Args.FilePath}");
		}
		private void HandleEvent_ConfiguratorConfigFileSaved(object Sender, ConfigFileSavedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "Configurator", "ConfigFileSaved", $"FilePath: {Args.FilePath}");
		}
		private void HandleEvent_ConfiguratorConfigurationUpdated(object Sender, ConfigurationUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "Configurator", "ConfigurationUpdated", $"Name: {Args.ConfigName}, ConfigNewValue: {Args.Configuration.mValue}");
			switch (Args.ConfigName)
			{
				case "LogExporter/BaseDirectory":
					mLogExporter.SetConfig("BaseDirectory", mConfigurator.GetValue("LogExporter/BaseDirectory"));
					break;
				case "LogExporter/ExportDirectoryNamePrefix":
					mLogExporter.SetConfig("ExportDirectoryNamePrefix", mConfigurator.GetValue("LogExporter/ExportDirectoryNamePrefix"));
					break;
				case "LogExporter/ExportDirectoryNameTimeFormat":
					mLogExporter.SetConfig("ExportDirectoryNameTimeFormat", mConfigurator.GetValue("LogExporter/ExportDirectoryNameTimeFormat"));
					break;
				case "ImportantEventRecorder/TimePeriod":
					mImportantEventRecorder.SetConfig("TimePeriod", mConfigurator.GetValue("ImportantEventRecorder/TimePeriod"));
					break;
				case "VehicleCommunicator/ListenPort":
					mVehicleCommunicator.SetConfig("ListenPort", mConfigurator.GetValue("VehicleCommunicator/ListenPort"));
					break;
				case "VehicleCommunicator/TimePeriod":
					mVehicleCommunicator.SetConfig("TimePeriod", mConfigurator.GetValue("VehicleCommunicator/TimePeriod"));
					break;
				case "CollisionEventDetector/TimePeriod":
					mCollisionEventDetector.SetConfig("TimePeriod", mConfigurator.GetValue("CollisionEventDetector/TimePeriod"));
					break;
				case "CollisionEventDetector/NeighborPointAmount":
					mCollisionEventDetector.SetConfig("NeighborPointAmount", mConfigurator.GetValue("CollisionEventDetector/NeighborPointAmount"));
					break;
				case "CollisionEventDetector/VehicleLocationScoreThreshold":
					mCollisionEventDetector.SetConfig("VehicleLocationScoreThreshold", mConfigurator.GetValue("CollisionEventDetector/VehicleLocationScoreThreshold"));
					break;
				case "VehicleControlHandler/TimePeriod":
					mVehicleControlHandler.SetConfig("TimePeriod", mConfigurator.GetValue("VehicleControlHandler/TimePeriod"));
					break;
				case "HostCommunicator/ListenPort":
					mHostCommunicator.SetConfig("ListenPort", mConfigurator.GetValue("HostCommunicator/ListenPort"));
					break;
				case "HostCommunicator/TimePeriod":
					mHostCommunicator.SetConfig("TimePeriod", mConfigurator.GetValue("HostCommunicator/TimePeriod"));
					break;
				case "MissionDispatcher/TimePeriod":
					mMissionDispatcher.SetConfig("TimePeriod", mConfigurator.GetValue("MissionDispatcher/TimePeriod"));
					break;
				case "MapFileManager/MapFileDirectory":
					mMapFileManager.SetConfig("MapFileDirectory", mConfigurator.GetValue("MapFileManager/MapFileDirectory"));
					break;
				case "MapManager/AutoLoadMap":
					mMapManager.SetConfig("AutoLoadMap", mConfigurator.GetValue("MapManager/AutoLoadMap"));
					break;
				case "CycleMissionGenerator/TimePeriod":
					mCycleMissionGenerator.SetConfig("TimePeriod", mConfigurator.GetValue("CycleMissionGenerator/TimePeriod"));
					break;
			}
		}
		private void HandleEvent_LogExporterConfigUpdated(object sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "LogExporter", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_LogExporterExportStarted(object Sender, LogExportedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "LogExporter", "ExportStarted", $"Directory: {Args.DirectoryPath}, Items: {string.Join(", ", Args.Items)}");
		}
		private void HandleEvent_LogExporterExportCompleted(object Sender, LogExportedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "LogExporter", "ExportCompleted", $"Directory: {Args.DirectoryPath}, Items: {string.Join(", ", Args.Items)}");
		}
		private void HandleEvent_AccessControlUserLogChanged(object Sender, UserLogChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "AccessControl", "UserLogChanged", $"Name: {Args.UserName}, Rank: {Args.UserRank.ToString()}, IsLogin: {Args.IsLogin.ToString()}");
			RaiseEvent_AccessControlUserLogChanged(Args.OccurTime, Args.UserName, Args.UserRank, Args.IsLogin);
		}
		private void HandleEvent_VehicleCommunicatorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleCommunicator", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_VehicleCommunicatorConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleCommunicator", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_VehicleCommunicatorLocalListenStateChagned(DateTime OccurTime, ListenState NewState, int Port)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "LocalListenStateChanged", $"State: {NewState.ToString()}, Port: {Port}");
		}
		private void HandleEvent_VehicleCommunicatorRemoteConnectStateChagned(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "RemoteConnectStateChanged", $"IPPort: {IpPort}, State: {NewState}");
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SentData", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
		}
		private void HandleEvent_VehicleCommunicatorReceivedSerializableData(DateTime OccurTime, string IpPort, object Data)
		{
			// 常態事件不做 General Log 記錄(避免資料庫儲存太多的資訊)，也不使用 Console.WriteLine() 顯示(避免資訊過多)
			if (!(Data is SerialData.AGVStatus) && !(Data is SerialData.AGVPath))
			{
				HandleDebugMessage(OccurTime, "VehicleCommunicator", "ReceivedData", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
			}
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataSuccessed(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SentDataSuccessed", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
		}
		private void HandleEvent_VehicleCommunicatorSentSerializableDataFailed(DateTime OccurTime, string IpPort, object Data)
		{
			HandleDebugMessage(OccurTime, "VehicleCommunicator", "SentDataFailed", $"IPPort: {IpPort}, DataType: {Data.ToString()}");
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleInfoManager", "ItemAdded", $"Name: {Args.ItemName}, Info: {Args.Item.ToString()}");
			RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.VehicleSystem, $"Vehicle [ {Args.ItemName} ] Connected");
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleInfoManager", "ItemRemoved", $"Name: {Args.ItemName}, Info: {Args.Item.ToString()}");
			RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.VehicleSystem, $"Vehicle [ {Args.ItemName} ] Disconnected");
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			// 僅有重要的狀態 (CurrentState, CurrentTarget, AlarmMessage, CurrentMissionId, CurrentInterveneCommand, CurrentMapName) 變化時才做 General Log 記錄與使用 Console.WriteLine() 顯示
			if (Args.StatusName.Contains("CurrentState") || Args.StatusName.Contains("CurrentTarget") || Args.StatusName.Contains("AlarmMessage") || Args.StatusName.Contains("CurrentMissionId") || Args.StatusName.Contains("CurrentInterveneCommand") || Args.StatusName.Contains("CurrentMapName") || Args.StatusName.Contains("IsTranslating") || Args.StatusName.Contains("IsRotating"))
			{
				HandleDebugMessage(Args.OccurTime, "VehicleInfoManager", "ItemUpdated", $"Name: {Args.ItemName}, StatusName:{Args.StatusName}, Info: {Args.Item.ToString()}");
			}
		}
		private void HandleEvent_CollisionEventManagerItemAdded(object Sender, ItemCountChangedEventArgs<ICollisionPair> Args)
		{
			HandleDebugMessage(Args.OccurTime, "CollisionEventManager", "ItemAdded", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_CollisionEventManagerItemRemoved(object Sender, ItemCountChangedEventArgs<ICollisionPair> Args)
		{
			HandleDebugMessage(Args.OccurTime, "CollisionEventManager", "ItemRemoved", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_CollisionEventManagerItemUpdated(object Sender, ItemUpdatedEventArgs<ICollisionPair> Args)
		{
			HandleDebugMessage(Args.OccurTime, "CollisionEventManager", "ItemUpdated", $"Name: {Args.ItemName}, StatusName:{Args.StatusName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_CollisionEventDetectorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CollisionEventDetector", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_CollisionEventDetectorConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CollisionEventDetector", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_VehicleControlManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleControl> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlManager", "ItemAdded", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_VehicleControlManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleControl> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlManager", "ItemRemoved", $"Name: {Args.ItemName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_VehicleControlManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleControl> Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlManager", "ItemUpdated", $"Name: {Args.ItemName}, StatusName:{Args.StatusName}, Info:{Args.Item.ToString()}");
		}
		private void HandleEvent_VehicleControlHandlerSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlHandler", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_VehicleControlHandlerConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "VehicleControlHandler", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_MissionStateManagerItemAdded(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionStateManager", "ItemAdded", $"MissionID: {Args.ItemName}, Info: {Args.Item.ToString()}");
			RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.MissionSystem, $"Mission [ {Args.Item.GetMissionId()} ] Created");
		}
		private void HandleEvent_MissionStateManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionStateManager", "ItemRemoved", $"MissionID: {Args.ItemName}, Info: {Args.Item.ToString()}");
		}
		private void HandleEvent_MissionStateManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IMissionState> Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionStateManager", "ItemUpdated", $"MissionID: {Args.ItemName}, StatusName:{Args.StatusName}, Info: {Args.Item.ToString()}");
			if (Args.StatusName.Contains("ExecutionStartTimestamp"))
			{
				RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.MissionSystem, $"Mission [ {Args.Item.GetMissionId()} ] Started by Vehicle [ {Args.Item.mExecutorId} ]");
			}
			else if (Args.StatusName.Contains("ExecutionStopTimestamp"))
			{
				RaiseEvent_SignificantEvent(Args.OccurTime, SignificantEventCategory.MissionSystem, $"Mission [ {Args.Item.GetMissionId()} ] Completed [ {Args.Item.mExecuteState.ToString().Replace("Execute", string.Empty)} ] by Vehicle [ {Args.Item.mExecutorId} ]");
			}
		}
		private void HandleEvent_HostCommunicatorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "HostCommunicator", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_HostCommunicatorConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "HostCommunicator", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_HostCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, int Port)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "LocalListenStateChanged", $"State: {NewState.ToString()}, Port: {Port}");
		}
		private void HandleEvent_HostCommunicatorRemoteConnectStateChanged(DateTime OccurTime, string IpPort, ConnectState NewState)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "RemoteConnectStateChanged", $"IPPort: {IpPort}, State: {NewState}");
			if (NewState == ConnectState.Connected)
			{
				RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.HostSystem, $"Host [ {IpPort} ] Connected");
			}
			else if (NewState == ConnectState.Disconnected)
			{
				RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.HostSystem, $"Host [ {IpPort} ] Disconnected");
			}
		}
		private void HandleEvent_HostCommunicatorSentString(DateTime OccurTime, string IpPort, string Data)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "SentString", $"IPPort: {IpPort}, Data: {Data}");
			RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.HostSystem, $"Sent Message [ {IpPort} ] [ {Data} ]");
		}
		private void HandleEvent_HostCommunicatorReceivedString(DateTime OccurTime, string IpPort, string Data)
		{
			HandleDebugMessage(OccurTime, "HostCommunicator", "ReceivedString", $"IPPort: {IpPort}, Data: {Data}");
			RaiseEvent_SignificantEvent(OccurTime, SignificantEventCategory.HostSystem, $"Received Message [ {IpPort} ] [ {Data} ]");
		}
		private void HandleEvent_MissionDispatcherSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionDispatcher", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_MissionDispatcherConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionDispatcher", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_MissionDispatcherMissionDispatched(object Sender, MissionDispatchedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MissionDispatcher", "MissionDispatched", $"MissionName: {Args.MissionState.mName} Dispatched To VehicleName: {Args.VehicleInfo.mName}");
		}
		private void HandleEvent_MapFileManagerConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapFileManager", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_MapFileManagerMapFileAdded(object Sender, MapFileCountChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapFileManager", "ItemAdded", $"MapFileName: {Args.MapFileName}");
		}
		private void HandleEvent_MapFileManagerMapFileRemoved(object Sender, MapFileCountChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapFileManager", "ItemRemoved", $"MapFileName: {Args.MapFileName}");
		}
		private void HandleEvent_MapManagerConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapManager", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_MapManagerLoadMapSuccessed(object Sender, LoadMapSuccessedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapManager", "LoadMapSuccessed", $"MapName: {Args.MapFileName}");
		}
		private void HandleEvent_MapManagerLoadMapFailed(object Sender, LoadMapFailedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapManager", "LoadMapFailed", $"MapName: {Args.MapFileName}, Reason: {Args.Reason.ToString()}");
		}
		private void HandleEvent_MapManagerSynchronizeMapStarted(object Sender, SynchronizeMapStartedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "MapFileManager", "SynchronizeMapStarted", $"MapFileName: {Args.MapFileName}, VehicleNames: {string.Join(",", Args.VehicleNames)}");
		}
		private void HandleEvent_CycleMissionGeneratorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CycleMissionGenerator", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_CycleMissionGeneratorCycleMissionAssigned(object Sender, CycleMissionAssignedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CycleMissionGenerator", "CycleMissionAssigned", $"VehicleID: {Args.VehicleId}, Missions: ({string.Join(")(", Args.Missions)})");
		}
		private void HandleEvent_CycleMissionGeneratorCycleMissionUnassigned(object Sender, CycleMissionUnassignedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CycleMissionGenerator", "CycleMissionUnassigned", $"VehicleID: {Args.VehicleId}");
		}
		private void HandleEvent_CycleMissionGeneratorCycleExecutedIndexChanged(object Sender, CycleMissionExecutedIndexChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CycleMissionGenerator", "CycleMissionExecutedIndexChanged", $"VehicleID: {Args.VehicleId}, Index: {Args.Index.ToString()}");
		}
		private void HandleEvent_CycleMissionGeneratorConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "CycleMissionGenerator", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleEvent_ImportantEventRecorderSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "ImportantEventRecorder", "SystemStatusChanged", $"SystemStatus: {Args.SystemNewStatus.ToString()}");
		}
		private void HandleEvent_ImportantEventRecorderConfigUpdated(object Sender, ConfigUpdatedEventArgs Args)
		{
			HandleDebugMessage(Args.OccurTime, "ImportantEventRecorder", "ConfigUpdated", $"ConfigName: {Args.ConfigName}, ConfigNewValue: {Args.ConfigNewValue}");
		}
		private void HandleDebugMessage(string Message)
		{
			HandleDebugMessage("None", Message);
		}
		private void HandleDebugMessage(string Category, string Message)
		{
			HandleDebugMessage(DateTime.Now, Category, Message);
		}
		private void HandleDebugMessage(DateTime OccurTime, string Category, string Message)
		{
			HandleDebugMessage(OccurTime, Category, "None", Message);
		}
		private void HandleDebugMessage(DateTime OccurTime, string Category, string SubCategory, string Message)
		{
			HandleDebugMessage(OccurTime.ToString(TIME_FORMAT), Category, SubCategory, Message);
		}
		private void HandleDebugMessage(string OccurTime, string Category, string SubCategory, string Message)
		{
			Console.WriteLine($"{OccurTime} [{Category}] [{SubCategory}] - {Message}");
			mLogRecorder.RecordGeneralLog(OccurTime, Category, SubCategory, Message);
			RaiseEvent_DebugMessage(OccurTime, Category, SubCategory, Message);
		}
	}

	public class DebugMessageEventArgs : EventArgs
	{
		public string OccurTime { get; private set; }
		public string Category { get; private set; }
		public string SubCategory { get; private set; }
		public string Message { get; private set; }

		public DebugMessageEventArgs(string OccurTime, string Category, string SubCategory, string Message) : base()
		{
			this.OccurTime = OccurTime;
			this.Category = Category;
			this.SubCategory = SubCategory;
			this.Message = Message;
		}
	}
	public class SignificantEventEventArgs : EventArgs
	{
		public string OccurTime { get; private set; }
		public string Category { get; private set; }
		public string Info { get; private set; }

		public SignificantEventEventArgs(string OccurTime, string Category, string Info) : base()
		{
			this.OccurTime = OccurTime;
			this.Category = Category;
			this.Info = Info;
		}
	}
}
