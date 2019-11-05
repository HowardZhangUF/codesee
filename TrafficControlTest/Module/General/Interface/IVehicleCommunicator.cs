﻿using System.Collections.Generic;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Interface
{
	/// <summary>
	/// - 提供監聽方法以供車子建立連線
	/// - 提供傳送資料給車子的方法
	/// - 當建立/中斷連線、送出/收到資料時會拋出事件
	/// </summary>
	public interface IVehicleCommunicator
	{
		event EventHandlerDateTime SystemStarted;
		event EventHandlerDateTime SystemStopped;
		event EventHandlerRemoteConnectState RemoteConnectStateChanged;
		event EventHandlerLocalListenState LocalListenStateChanged;
		event EventHandlerSentSerializableData SentSerializableData;
		event EventHandlerReceivedSerializableData ReceivedSerializableData;
		event EventHandlerSentSerializableData SentSerializableDataSuccessed;
		event EventHandlerSentSerializableData SentSerializableDataFailed;
		
		/// <summary>監聽狀態</summary>
		ListenState mListenState { get; }
		/// <summary>連線中的 Client 數量</summary>
		int mClientCount { get; }
		/// <summary>連線中的 Client 地址資訊 (IP:Port)</summary>
		List<string> mClientAddressInfo { get; }

		/// <summary>設定監聽 Port 的值</summary>
		void SetConfigOfListenPort(int Port);
		/// <summary>取得監聽 Port 的值</summary>
		int GetConfigOfListenPort();
		/// <summary>開始監聽</summary>
		void StartListen();
		/// <summary>停止監聽</summary>
		void StopListen();
		/// <summary>向指定 IP:Port 傳送序列化資料</summary>
		void SendSerializableData(string IpPort, object Data);
		/// <summary>向指定 IP:Port 傳送序列化資料 Goto</summary>
		void SendSerializableData_Goto(string IpPort, string Target);
		/// <summary>向指定 IP:Port 傳送序列化資料 GotoPoint</summary>
		void SendSerializableData_GotoPoint(string IpPort, int X, int Y);
		/// <summary>向指定 IP:Port 傳送序列化資料 GotoTowardPoint</summary>
		void SendSerializableData_GotoTowardPoint(string IpPort, int X, int Y, int Toward);
		/// <summary>向指定 IP:Port 傳送序列化資料 Dock</summary>
		void SendSerializableData_Dock(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 InsertMovingBuffer</summary>
		void SendSerializableData_InsertMovingBuffer(string IpPort, string Buffer);
		/// <summary>向指定 IP:Port 傳送序列化資料 RemoveMovingBuffer</summary>
		void SendSerializableData_RemoveMovingBuffer(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 PauseMoving</summary>
		void SendSerializableData_PauseMoving(string IpPort);
		/// <summary>向指定 IP:Port 傳送序列化資料 ResumeMoving</summary>
		void SendSerializableData_ResumeMoving(string IpPort);
	}
}
