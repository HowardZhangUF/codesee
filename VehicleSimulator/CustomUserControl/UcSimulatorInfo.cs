using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LibraryForVM;
using System.Collections.Generic;
using System.Threading;

namespace VehicleSimulator
{
	public partial class UcSimulatorInfo : UserControl
	{
		private SimulatorProcess rSimulatorProcess = null;
		private ISimulatorInfo rSimulatorInfo = null;
		private ISimulatorControl rSimulatorControl = null;
		private IMoveRequestCalculator rMoveRequestCalculator = null;
		private IHostCommunicator rHostCommunicator = null;

		private Thread mDoGoalsMoveLoop;
		private Thread mDoGoalsMove;

		/// <summary>
		/// 按鈕動作狀態
		/// </summary>
		private  enum   ButtonActionStatus
        {
			
			SetLocation,
			Move,
			SetAndMove,
			Stop,
			
        }

		/// <summary>
		/// 設定SetAndMoveTextBox值(白色字)
		/// </summary>
		public string Set_SetAndMoveTextBoxValue
		{
			set
			{
				SetAndMoveTextBox.ForeColor = Color.White;
				SetAndMoveTextBox.Text = value;
			}

		}

		public UcSimulatorInfo(SimulatorProcess SimulatorProcess)
		{
			InitializeComponent();
			UpdateGui_InitializeDgvSimulatorInfo();
			Set(SimulatorProcess);
			Set_BottonBackColor((int)ButtonActionStatus.Stop);
		}


		#region --Set Method--

		public void Set(SimulatorProcess SimulatorProcess)
		{
			UnsubscribeEvent_SimulatorProcess(rSimulatorProcess);
			rSimulatorProcess = SimulatorProcess;
			SubscribeEvent_SimulatorProcess(rSimulatorProcess);
			UpdateGui_UpdateSimulatorInfo();
		}
		protected void Set(ISimulatorInfo SimulatorInfo)
		{
			UnsubscribeEvent_ISimulatorInfo(rSimulatorInfo);
			rSimulatorInfo = SimulatorInfo;
			SubscribeEvent_ISimulatorInfo(rSimulatorInfo);
		}
		protected void Set(ISimulatorControl SimulatorControl)
		{
			UnsubscribeEvent_ISimulatorControl(rSimulatorControl);
			rSimulatorControl = SimulatorControl;
			SubscribeEvent_ISimulatorControl(rSimulatorControl);
		}
		protected void Set(IMoveRequestCalculator MoveRequestCalculator)
		{
			UnsubscribeEvent_IMoveRequestCalculator(rMoveRequestCalculator);
			rMoveRequestCalculator = MoveRequestCalculator;
			SubscribeEvent_IMoveRequestCalculator(rMoveRequestCalculator);
		}
		protected void Set(IHostCommunicator HostCommunicator)
		{
			UnsubscribeEvent_IHostCommunicator(rHostCommunicator);
			rHostCommunicator = HostCommunicator;
			SubscribeEvent_IHostCommunicator(rHostCommunicator);
		}

		/// <summary>
		/// 移動相關之按鈕顏色更換(button:SetLocation、Move、SetAndMove、Stop)
		/// </summary>
		private void Set_BottonBackColor(int btnActionStatus)
		{
			Color Green = Color.Green;
			Color _67= Color.FromArgb(67, 67, 67); ;
			switch (btnActionStatus)
			{
				case (int)ButtonActionStatus.SetLocation:
					btnSimulatorSetLocation.BackColor = Green;
					btnSimulatorMove.BackColor = _67;
					btnSimulatorSetAndMove.BackColor = _67;
					btnSimulatorStop.BackColor = _67;
					break;
				case (int)ButtonActionStatus.Move:
					btnSimulatorSetLocation.BackColor = _67;
					btnSimulatorMove.BackColor = Green;
					btnSimulatorSetAndMove.BackColor = _67;
					btnSimulatorStop.BackColor = _67;
					break;
				case (int)ButtonActionStatus.SetAndMove:
					btnSimulatorSetLocation.BackColor = _67;
					btnSimulatorMove.BackColor = _67;
					btnSimulatorSetAndMove.BackColor = Green;
					btnSimulatorStop.BackColor = _67;
					break;
				case (int)ButtonActionStatus.Stop:
					btnSimulatorSetLocation.BackColor = _67;
					btnSimulatorMove.BackColor = _67;
					btnSimulatorSetAndMove.BackColor = _67;
					btnSimulatorStop.BackColor = Green;
					break;
			}
		}
		#endregion

		#region --Button Click--
		private void btnSimulatorConnect_Click(object sender, EventArgs e)
		{
			if (rHostCommunicator != null)
			{

				rHostCommunicator.SetConfig("RemoteIpPort", txtHostIpPort.Text);
				if (rHostCommunicator.mIsConnected)
				{
					rHostCommunicator.Disconnect();
				}
				else
				{
					rHostCommunicator.Connect();
				}
			}
		}
		private void btnSimulatorSetLocation_Click(object sender, EventArgs e)
		{
			if (cbGoalList.Items.Count > 0 && cbGoalList.SelectedItem != null)
			{
				StopAllDoGoalsMove();

				string Selected_Item_String = cbGoalList.SelectedItem.ToString();
				Dictionary<string,string> test = SeperateGoalAndPoint(Selected_Item_String);
				string[] Point_X_Y_Toward;
				char[] Separator = { '(', ',', ')' };

				foreach (var i in test)
                {
					Point_X_Y_Toward = string_Separator(Separator,i.Value);
					rSimulatorInfo.SetLocation(int.Parse(Point_X_Y_Toward[0]), int.Parse(Point_X_Y_Toward[1]), int.Parse(Point_X_Y_Toward[2]));
					Set_BottonBackColor((int)ButtonActionStatus.SetLocation);
					Dispaly_From_Here_To_There(Selected_Item_String, "None");
				}
				

				
			}
		}
		private void btnSimulatorMove_Click(object sender, EventArgs e)
		{
			if (rSimulatorControl != null&&cbMoveTarget.SelectedItem!=null)
			{
				if (btnSimulatorSetLocation.BackColor==Color.Green)
				{
					StopAllDoGoalsMove();

					
					string targetName=null;
					string targetName_X_Y_Toward = null;

					Dictionary<string,string>goalInfo= SeperateGoalAndPoint(cbMoveTarget.Text);
					foreach (var i in goalInfo)
					{
						targetName = i.Key;
						targetName_X_Y_Toward = i.Value;
					}
					var moveRequests = rMoveRequestCalculator.Calculate(new Point(rSimulatorInfo.mX, rSimulatorInfo.mY), targetName, rSimulatorInfo.mWidth, rSimulatorInfo.mRotationDiameter);
					string selectedItemString = cbGoalList.SelectedItem.ToString();
					Dispaly_From_Here_To_There(selectedItemString, targetName + targetName_X_Y_Toward);
					
					rSimulatorControl.StartMove(targetName, moveRequests);

					Set_BottonBackColor((int)ButtonActionStatus.Move);
				}
				else
                {
					StopAllDoGoalsMove();

					int mX=Convert.ToInt32(dgvSimulatorInfo.Rows[1].Cells[1].Value?.ToString());
					int mY= Convert.ToInt32(dgvSimulatorInfo.Rows[2].Cells[1].Value?.ToString());
					int mToward= Convert.ToInt32(dgvSimulatorInfo.Rows[3].Cells[1].Value?.ToString());
					
					string targetName = null;
					string targetName_X_Y_Toward = null;
					Dictionary<string, string> goalInfo = SeperateGoalAndPoint(cbMoveTarget.Text);
					foreach (var i in goalInfo)
					{
						targetName = i.Key;
						targetName_X_Y_Toward = i.Value;
					}
					var moveRequests = rMoveRequestCalculator.Calculate(new Point(mX,mY), targetName, rSimulatorInfo.mWidth, rSimulatorInfo.mRotationDiameter);

					string StartPoint ="("+ mX + ","+ mY + ","+ mToward + ")";
					string EndPoint = targetName + targetName_X_Y_Toward;
					Dispaly_From_Here_To_There(StartPoint, EndPoint);

					rSimulatorControl.StartMove(targetName, moveRequests);

					Set_BottonBackColor((int)ButtonActionStatus.Move);
				}
                
			}
		}
		
		
		private void btnSimulatorSetAndMove_Click(object sender, EventArgs e)
		{
			if (rSimulatorInfo.mMapData != null)
            {
				StopAllDoGoalsMove();

				bool IsLoop = false;
				char[] Separator = { '(', ')' };
				string input = SetAndMoveTextBox.Text;
				string[] Newinput = string_Separator(Separator,input);
				
				foreach (var v in Newinput)
				{
					if (v == "Loop")
					{
						IsLoop = true;
					}
				}
				
				string[] goalStrings = GetMapGoalFromMapData(rSimulatorInfo.mMapData);
				Dictionary<String, String> GoalInfo = SeperateGoalAndPoint(goalStrings);//ex. key:A---(Goal Name), Valus:(100,100)---(Goal X,Y)

				bool bIsGoalsExist = IsGoalsExist(IsLoop, Newinput, GoalInfo);

				if (IsLoop == true&& bIsGoalsExist == true)
				{
					mDoGoalsMoveLoop = new Thread(o => DoGoalsMoveLoop(GoalInfo, Newinput));
					mDoGoalsMoveLoop.Name = "mDoGoalsMoveLoop";
					mDoGoalsMoveLoop.IsBackground = true;
					mDoGoalsMoveLoop.Start();
					Set_BottonBackColor((int)ButtonActionStatus.SetAndMove);
				}
				else if (IsLoop == false && bIsGoalsExist == true)
				{
					mDoGoalsMove = new Thread(o => DoGoalsMove(GoalInfo, Newinput));
					mDoGoalsMove.Name = "mDoGoalsMove";
					mDoGoalsMove.IsBackground = true;
					mDoGoalsMove.Start();
					
					Set_BottonBackColor((int)ButtonActionStatus.SetAndMove);
				}
				else if (bIsGoalsExist == false)
                {
					MessageBox.Show("有未知的目標點\r\r解決辦法:\r1.選擇其他.map檔\r2.刪除未知目標點");
				}
			}
		}


		private void btnSimulatorStop_Click(object sender, EventArgs e)
		{
			if (rSimulatorControl != null)
			{
				StopAllDoGoalsMove();
				Set_BottonBackColor((int)ButtonActionStatus.Stop);
				Dispaly_From_Here_To_There("None", "None");
			}
		}
		#endregion

		#region  --btnSimulatorSetAndMove_Click Method--

		/// <summary>
		/// 取得地圖上Goal Names
		/// </summary>
		/// <param name="mapData"></param>
		/// <returns></returns>
		private string[] GetMapGoalFromMapData(MapData mapData)
		{
			return mapData.mGoals.OrderBy(o => o.mName).Select(o => $"{o.mName} ({o.mX},{o.mY},{o.mToward})").ToArray();
		}

		/// <summary>
		/// 將目標點名稱與座標點分開
		/// </summary>
		private static Dictionary<string, string> SeperateGoalAndPoint(string[] goalString)
		{
			Dictionary<string, string> GoalPoint = new Dictionary<string, string>();

			foreach (var goalitem in goalString)
			{
				string[] GoalItem = goalitem.ToString().Split(' '); //ex: G1 (5,3,90)
				GoalPoint[GoalItem[0]] = GoalItem[1];// GoalPoint["G1"]="(5,3,90)"
			}
			return GoalPoint;
		}

		/// <summary>
		/// 將目標點名稱與座標點分開
		/// </summary>
		private static Dictionary<string, string> SeperateGoalAndPoint(string goalString)
		{
			Dictionary<string, string> GoalPoint = new Dictionary<string, string>();
			string[] GoalItem=goalString.ToString().Split(' '); //ex: G1 (5,3,90)
			GoalPoint[GoalItem[0]] = GoalItem[1];// GoalPoint["G1"]="(5,3,90)"
			
			return GoalPoint;
		}


		/// <summary>
		/// 字元分隔器
		/// </summary>
		/// <param name="text">要分離的字串</param>
		/// <returns>分隔後的字串</returns>
		private string[] string_Separator(char[] Separator,string text) //ex. (0,0) => 0,0
        {
			return	text.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
		}

		

		/// <summary>
		/// 輸入的點是否與地圖點資訊相同
		/// </summary>
		/// <param name="IsLoop">是否為迴圈</param>
		/// <param name="input">輸入的點名稱</param>
		/// <param name="GoalInfo">地圖點資訊</param>
		/// <returns>若相同(true)，不相同(false)</returns>
		private bool IsGoalsExist(bool IsLoop,string[] input, Dictionary<String, String> GoalInfo)
        {
			int inputLength = input.Length;
			if (IsLoop == true)
            {
				string[] Newinput = new string[inputLength - 1];
				for (int i = 0; i < (inputLength - 1); i++)
				{
					Newinput[i] = input[i];
				}
				if (Newinput.All(o => GoalInfo.ContainsKey(o)))
				{
					return true;
				}
                else
                {
					return false;
                }
			}
            else
            {
				if (input.All(o => GoalInfo.ContainsKey(o)))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		
		/// <summary>
		/// 進行多路徑點移動迴圈(2點以上)
		/// </summary>
		/// <param name="GoalInfo">地圖上點的相關資料(Key:名稱) (Valus:X軸、Y軸、相位角)</param>
		/// <param name="EndGoalInfo">地圖上終點的相關資料(Key:名稱) (Valus:X軸、Y軸、相位角)</param>
		/// <param name="input">想進行迴圈的Goal名稱</param>
		private void DoGoalsMoveLoop(Dictionary<String, String> GoalInfo, String[] input)
		{
			bool ContintueWhile = true;
			int index = 0;
			int inputLength = input.Length;

			string[] Newinput = new string[inputLength - 1];
			for (int i = 0; i < (inputLength - 1); i++)
			{
				Newinput[i] = input[i];
			}

			while (ContintueWhile)
			{

				if (rSimulatorInfo.mStatus == ESimulatorStatus.Idle)
				{
					char[] locationseperator = { '(', ')', ',' };
					String[] Point_X_Y_Toword = GoalInfo[Newinput[index]].Split(locationseperator, StringSplitOptions.RemoveEmptyEntries);


					if (index == Newinput.Length - 1)
					{

						rSimulatorInfo.SetLocation(int.Parse(Point_X_Y_Toword[0]), int.Parse(Point_X_Y_Toword[1]), int.Parse(Point_X_Y_Toword[2]));
						index = 0;
						Dispaly_From_Here_To_There(Newinput[Newinput.Length - 1] + GoalInfo[Newinput[Newinput.Length - 1]], Newinput[index] + GoalInfo[Newinput[index]]);
					}
					else if (index == 0)
                    {
						rSimulatorInfo.SetLocation(int.Parse(Point_X_Y_Toword[0]), int.Parse(Point_X_Y_Toword[1]), int.Parse(Point_X_Y_Toword[2]));
						index++;
					}
					else
					{
						index++;
						Dispaly_From_Here_To_There(Newinput[index - 1] + GoalInfo[Newinput[index - 1]], Newinput[index] + GoalInfo[Newinput[index]]);
					}
						var moveRequests = rMoveRequestCalculator.Calculate(new Point(rSimulatorInfo.mX, rSimulatorInfo.mY), Newinput[index], rSimulatorInfo.mWidth, rSimulatorInfo.mRotationDiameter);
						rSimulatorControl.StartMove(Newinput[index], moveRequests);

						
					

				}
				
			}
			
		}

		/// <summary>
		/// 設定起點(1點) or 進行路徑點移動(2點以上)
		/// </summary>
		/// <param name="GoalInfo">地圖上點的相關資料(Key:名稱) (Valus:X軸、Y軸、相位角)</param>
		/// <param name="EndGoalInfo">地圖上終點的相關資料(Key:名稱) (Valus:X軸、Y軸、相位角)</param>
		/// <param name="input">想進行迴圈的Goal名稱</param>
		private void DoGoalsMove(Dictionary<String, String> GoalInfo, String[] input)
        {
			int index = 0;
			bool ContintueWhile = true;
			if (input.Length == 1)
			{
				ContintueWhile = false;
				char[] locationseperator = { '(', ')', ',' };
				String[] Point_X_Y_Toword = GoalInfo[input[index]].Split(locationseperator, StringSplitOptions.RemoveEmptyEntries);
				rSimulatorInfo.SetLocation(int.Parse(Point_X_Y_Toword[0]), int.Parse(Point_X_Y_Toword[1]), int.Parse(Point_X_Y_Toword[2]));

				Dispaly_From_Here_To_There(input[index]+ GoalInfo[input[index]], "None");
			}
			while (ContintueWhile)
			{
				char[] locationseperator = { '(', ')', ',' };
				String[] Point_X_Y_Toword = GoalInfo[input[index]].Split(locationseperator, StringSplitOptions.RemoveEmptyEntries);

				if (index == 0)
                {
					rSimulatorInfo.SetLocation(int.Parse(Point_X_Y_Toword[0]), int.Parse(Point_X_Y_Toword[1]), int.Parse(Point_X_Y_Toword[2]));
				}
				
				

				if (rSimulatorInfo.mStatus == ESimulatorStatus.Idle)
				{
					index++;
					var moveRequests = rMoveRequestCalculator.Calculate(new Point(rSimulatorInfo.mX, rSimulatorInfo.mY), input[index], rSimulatorInfo.mWidth, rSimulatorInfo.mRotationDiameter);
					rSimulatorControl.StartMove(input[index], moveRequests);

					Dispaly_From_Here_To_There(input[index-1] + GoalInfo[input[index-1]], input[index] + GoalInfo[input[index]]);


					if (index == input.Length - 1)
					{
						ContintueWhile = false;
					}
				}
				
				
			}
			
		}

		/// <summary>
		/// 結束所有路徑點移動執行緒
		/// </summary>
		private void StopAllDoGoalsMove()
		{
			rSimulatorControl.StopMove();

			int mX = Convert.ToInt32(dgvSimulatorInfo.Rows[1].Cells[1].Value?.ToString());
			int mY = Convert.ToInt32(dgvSimulatorInfo.Rows[2].Cells[1].Value?.ToString());
			int mToward = Convert.ToInt32(dgvSimulatorInfo.Rows[3].Cells[1].Value?.ToString());

			rSimulatorInfo.SetLocation(mX, mY, mToward);

			if (mDoGoalsMoveLoop != null && mDoGoalsMoveLoop.IsAlive)
			{
				mDoGoalsMoveLoop.Abort();
			}
			if (mDoGoalsMove != null && mDoGoalsMove.IsAlive)
			{
				mDoGoalsMove.Abort();
			}
		}

		/// <summary>
		/// 顯示出移動過程中的起點與終點
		/// </summary>
		/// <param name="StartPoint">起點</param>
		/// <param name="EndPoints">終點</param>
		public void Dispaly_From_Here_To_There(string StartPoint, String EndPoints)
		{
			string a = "    =>    ";
			lbPoints.InvokeIfNecessary(() => { lbPoints.Text = StartPoint + a + EndPoints; });
		}


		#endregion

		#region --dgvSimulatorInfo Changed--

		private void dgvSimulatorInfo_SelectionChanged(object sender, EventArgs e)
		{
			if (dgvSimulatorInfo.CurrentCell.ColumnIndex == dgvSimulatorInfo.Columns["ItemKey1"].Index || dgvSimulatorInfo.CurrentCell.ColumnIndex == dgvSimulatorInfo.Columns["ItemKey2"].Index)
			{
				dgvSimulatorInfo.ClearSelection();
			}
			if (dgvSimulatorInfo.CurrentCell.RowIndex == 0 && dgvSimulatorInfo.CurrentCell.ColumnIndex == 1)
			{
				dgvSimulatorInfo.ClearSelection();
			}
			if (dgvSimulatorInfo.CurrentCell.RowIndex == 0 && dgvSimulatorInfo.CurrentCell.ColumnIndex == 3)
			{
				dgvSimulatorInfo.ClearSelection();
			}
			if (dgvSimulatorInfo.CurrentCell.RowIndex == 1 && dgvSimulatorInfo.CurrentCell.ColumnIndex == 3)
			{
				dgvSimulatorInfo.ClearSelection();
			}
		}
		private void dgvSimulatorInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (string.IsNullOrEmpty(lblSimulatorName.Text)) return;

			string cellValue = dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

			if(e.RowIndex == 0 && e.ColumnIndex == 1) //Name
            {

            }
			else if (e.RowIndex == 1 && e.ColumnIndex == 1) // X
			{
				if (int.TryParse(cellValue, out int newX))
				{
					rSimulatorInfo.SetLocation(newX, rSimulatorInfo.mY, rSimulatorInfo.mToward);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mX;
				}
			}
			else if (e.RowIndex == 2 && e.ColumnIndex == 1) // Y
			{
				if (int.TryParse(cellValue, out int newY))
				{
					rSimulatorInfo.SetLocation(rSimulatorInfo.mX, newY, rSimulatorInfo.mToward);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mY;
				}
			}
			else if (e.RowIndex == 3 && e.ColumnIndex == 1) // Toward
			{
				if (int.TryParse(cellValue, out int newToward))
				{
					rSimulatorInfo.SetLocation(rSimulatorInfo.mX, rSimulatorInfo.mY, newToward);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mToward;
				}
			}
			else if (e.RowIndex == 4 && e.ColumnIndex == 1) // Translate Velocity
			{
				if (int.TryParse(cellValue, out int newTranslateVelocity))
				{
					rSimulatorInfo.SetTranslateVelocity(newTranslateVelocity);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mTranslateVelocity;
				}
			}
			else if (e.RowIndex == 2 && e.ColumnIndex == 3) // Score
			{
				if (double.TryParse(cellValue, out double newScore))
				{
					rSimulatorInfo.SetScore(newScore);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mScore.ToString("F2");
				}
			}
			else if (e.RowIndex == 3 && e.ColumnIndex == 3) // Battery
			{
				if (double.TryParse(cellValue, out double newBattery))
				{
					rSimulatorInfo.SetBattery(newBattery);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mBattery.ToString("F2");
				}
			}
			else if (e.RowIndex == 4 && e.ColumnIndex == 3) // Rotate Velocity
			{
				if (int.TryParse(cellValue, out int newRotateVelocity))
				{
					rSimulatorInfo.SetRotateVelocity(newRotateVelocity);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mRotateVelocity;
				}
			}
			else if (e.RowIndex == 5 && e.ColumnIndex == 1) // Width
			{
				if (int.TryParse(cellValue, out int newWidth))
				{
					rSimulatorInfo.SetWidth(newWidth);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mWidth;
				}
			}
			else if (e.RowIndex == 5 && e.ColumnIndex == 3) // Rotation Diameter
			{
				if (int.TryParse(cellValue, out int newRotationDiameter))
				{
					rSimulatorInfo.SetRotationDiameter(newRotationDiameter);
				}
				else
				{
					dgvSimulatorInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rSimulatorInfo.mRotationDiameter;
				}
			}
		}

		#endregion

		#region --SubscribeEvent & UnsubscribeEvent--

		private void SubscribeEvent_SimulatorProcess(SimulatorProcess rSimulatorProcess)
		{
			if (rSimulatorProcess != null)
			{
				Set(rSimulatorProcess.GetReferenceOfISimulatorInfo());
				Set(rSimulatorProcess.GetReferenceOfISimulatorControl());
				Set(rSimulatorProcess.GetReferenceOfIMoveRequestCalculator());
				Set(rSimulatorProcess.GetReferenceOfIHostCommunicator());
			}
		}
		private void UnsubscribeEvent_SimulatorProcess(SimulatorProcess rSimulatorProcess)
		{
			if (rSimulatorProcess != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_ISimulatorInfo(ISimulatorInfo SimulatorInfo)
		{
			if (SimulatorInfo != null)
			{
				SimulatorInfo.StatusUpdated += HandleEvent_SimulatorInfoStatusUpdated;
			}
		}
		private void UnsubscribeEvent_ISimulatorInfo(ISimulatorInfo SimulatorInfo)
		{
			if (SimulatorInfo != null)
			{
				SimulatorInfo.StatusUpdated -= HandleEvent_SimulatorInfoStatusUpdated;
			}
		}
		private void SubscribeEvent_ISimulatorControl(ISimulatorControl SimulatorControl)
		{
			if (SimulatorControl != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_ISimulatorControl(ISimulatorControl SimulatorControl)
		{
			if (SimulatorControl != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMoveRequestCalculator(IMoveRequestCalculator MoveRequestCalculator)
		{
			if (MoveRequestCalculator != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IMoveRequestCalculator(IMoveRequestCalculator MoveRequestCalculator)
		{
			if (MoveRequestCalculator != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.ConnectStateChanged += HandleEvent_HostCommunicatorConnectStateChanged;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.ConnectStateChanged -= HandleEvent_HostCommunicatorConnectStateChanged;
			}
		}

		#endregion

		#region --HandleEvent--

		private void HandleEvent_SimulatorInfoStatusUpdated(object sender, StatusUpdatedEventArgs e)
		{
			if (e.StatusName.Contains("Status"))
			{
				UpdateGui_SetSimulatorStatus(rSimulatorInfo.mStatus.ToString());
			}
			else if (e.StatusName.Contains("X") || e.StatusName.Contains("Y") || e.StatusName.Contains("Toward"))
			{
				UpdateGui_SetSimulatorLocation(rSimulatorInfo.mX, rSimulatorInfo.mY, rSimulatorInfo.mToward);
			}
			else if (e.StatusName.Contains("Target"))
			{
				UpdateGui_SetSimulatorTarget(rSimulatorInfo.mTarget);
			}
			else if (e.StatusName.Contains("Score"))
			{
				UpdateGui_SetSimulatorScore(rSimulatorInfo.mScore);
			}
			else if (e.StatusName.Contains("Battery"))
			{
				UpdateGui_SetSimulatorBattery(rSimulatorInfo.mBattery);
			}
			else if (e.StatusName.Contains("TranslateVelocity"))
			{
				UpdateGui_SetSimulatorTranslateVelocity(rSimulatorInfo.mTranslateVelocity);
			}
			else if (e.StatusName.Contains("RotateVelocity"))
			{
				UpdateGui_SetSimulatorRotationVelocity(rSimulatorInfo.mRotateVelocity);
			}
			else if (e.StatusName.Contains("MapData"))
			{
				UpdateGui_SetSimulatorGoalList(rSimulatorInfo.mMapData);
			}
			else if (e.StatusName.Contains("Width"))
			{
				UpdateGui_SetSimulatorWidth(rSimulatorInfo.mWidth);
			}
			else if (e.StatusName.Contains("RotationDiameter"))
			{
				UpdateGui_SetSimulatorRotationDiameter(rSimulatorInfo.mRotationDiameter);
			}
		}
		private void HandleEvent_HostCommunicatorConnectStateChanged(object sender, ConnectStateChangedEventArgs e)
		{
			UpdateGui_SetSimulatorIsConnect(rHostCommunicator.mIsConnected);
			UpdateGui_SetSimulatorHostIpPort(rHostCommunicator.GetConfig("RemoteIpPort"));
		}

		#endregion

		#region --UpdateGUI--

		private void UpdateGui_InitializeDgvSimulatorInfo()
		{
			DataGridView dgv = dgvSimulatorInfo;

			dgv.RowHeadersVisible = false;
			dgv.ColumnHeadersVisible = false;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToResizeRows = false;
			dgv.AllowUserToResizeColumns = false;
			dgv.MultiSelect = false;
			dgv.GridColor = Color.FromArgb(41, 41, 41);

			dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
			dgv.DefaultCellStyle.BackColor = Color.FromArgb(67, 67, 67);
			dgv.DefaultCellStyle.ForeColor = Color.White;
			dgv.RowTemplate.Height = 30;

			dgv.Columns.Add("ItemKey1", "ItemKey1");
			dgv.Columns[0].Width = 450 / 4;
			dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgv.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(54, 54, 54);
			dgv.Columns[0].ReadOnly = true;
			dgv.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
			dgv.Columns.Add("ItemValue1", "ItemValue1");
			dgv.Columns[1].Width = 450 / 4;
			dgv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dgv.Columns[1].ReadOnly = true;
			dgv.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
			dgv.Columns.Add("ItemKey2", "ItemKey2");
			dgv.Columns[2].Width = 450 / 4;
			dgv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgv.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(54, 54, 54);
			dgv.Columns[2].ReadOnly = true;
			dgv.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
			dgv.Columns.Add("ItemValue2", "ItemValue2");
			dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dgv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dgv.Columns[3].ReadOnly = true;
			dgv.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			dgv.Rows.Add("Name    ", "", "Status    ", "");
			dgv.Rows.Add("X    ", "0", "Target    ", "");
			dgv.Rows.Add("Y    ", "0", "Score    ", "100.00");
			dgv.Rows.Add("Head    ", "0", "Battery    ", "100.00");
			dgv.Rows.Add("TranslateVelocity    ", "800", "RotateVelocity    ", "90");
			dgv.Rows.Add("Width (mm) ","700","RotationDiameter (mm) ","0");
			dgv.ClearSelection();

			dgv.Rows[0].Cells[1].ReadOnly = false;
			dgv.Rows[1].Cells[1].ReadOnly = false;
			dgv.Rows[2].Cells[1].ReadOnly = false;
			dgv.Rows[2].Cells[3].ReadOnly = false;
			dgv.Rows[3].Cells[1].ReadOnly = false;
			dgv.Rows[3].Cells[3].ReadOnly = false;
			dgv.Rows[4].Cells[1].ReadOnly = false;
			dgv.Rows[4].Cells[3].ReadOnly = false;
			dgv.Rows[5].Cells[1].ReadOnly = false;
			dgv.Rows[5].Cells[3].ReadOnly = false;
		}
		private void UpdateGui_UpdateSimulatorInfo()
		{
			if (rSimulatorProcess != null && rHostCommunicator != null)
			{
				UpdateGui_SetSimulatorName(rSimulatorInfo.mName);
				UpdateGui_SetSimulatorStatus(rSimulatorInfo.mStatus.ToString());
				UpdateGui_SetSimulatorLocation(rSimulatorInfo.mX, rSimulatorInfo.mY, rSimulatorInfo.mToward);
				UpdateGui_SetSimulatorTarget(rSimulatorInfo.mTarget);
				UpdateGui_SetSimulatorScore(rSimulatorInfo.mScore);
				UpdateGui_SetSimulatorBattery(rSimulatorInfo.mBattery);
				UpdateGui_SetSimulatorTranslateVelocity(rSimulatorInfo.mTranslateVelocity);
				UpdateGui_SetSimulatorRotationVelocity(rSimulatorInfo.mRotateVelocity);
				UpdateGui_SetSimulatorIsConnect(rHostCommunicator.mIsConnected);
				UpdateGui_SetSimulatorHostIpPort(rHostCommunicator.GetConfig("RemoteIpPort"));
				UpdateGui_SetSimulatorGoalList(rSimulatorInfo.mMapData);
				UpdateGui_SetSimulatorWidth(rSimulatorInfo.mWidth);
				UpdateGui_SetSimulatorRotationDiameter(rSimulatorInfo.mRotationDiameter);
			}
			else
			{
				UpdateGui_SetSimulatorName(string.Empty);
				UpdateGui_SetSimulatorStatus(string.Empty);
				UpdateGui_SetSimulatorLocation(default(int), default(int), default(int));
				UpdateGui_SetSimulatorTarget(string.Empty);
				UpdateGui_SetSimulatorScore(default(double));
				UpdateGui_SetSimulatorBattery(default(double));
				UpdateGui_SetSimulatorTranslateVelocity(default(int));
				UpdateGui_SetSimulatorRotationVelocity(default(int));
				UpdateGui_SetSimulatorIsConnect(default(bool));
				UpdateGui_SetSimulatorHostIpPort(string.Empty);
				UpdateGui_SetSimulatorGoalList(null);
				UpdateGui_SetSimulatorWidth(default(int));
				UpdateGui_SetSimulatorRotationDiameter(default(int));
			}
		}
		private void UpdateGui_SetSimulatorName(string Value)
		{
			lblSimulatorName.InvokeIfNecessary(() => { lblSimulatorName.Text = Value; });
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[0].Cells[1].Value = Value; });
		}
		private void UpdateGui_SetSimulatorStatus(string Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[0].Cells[3].Value = Value; });
		}
		private void UpdateGui_SetSimulatorLocation(int X, int Y, int Toward)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[1].Cells[1].Value = X; });
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[2].Cells[1].Value = Y; });
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[3].Cells[1].Value = Toward; });
		}
		private void UpdateGui_SetSimulatorTarget(string Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[1].Cells[3].Value = Value; });
		}
		private void UpdateGui_SetSimulatorScore(double Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[2].Cells[3].Value = Value.ToString("F2"); });
		}
		private void UpdateGui_SetSimulatorBattery(double Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[3].Cells[3].Value = Value.ToString("F2"); });
		}
		private void UpdateGui_SetSimulatorTranslateVelocity(int Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[4].Cells[1].Value = Value; });
		}
		private void UpdateGui_SetSimulatorRotationVelocity(int Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[4].Cells[3].Value = Value; });
		}
		private void UpdateGui_SetSimulatorIsConnect(bool Value)
		{
			if (Value == true)
			{
				btnSimulatorConnect.InvokeIfNecessary(() => { btnSimulatorConnect.BackColor = Color.FromArgb(0, 163, 0); });
			}
			else
			{
				btnSimulatorConnect.InvokeIfNecessary(() => { btnSimulatorConnect.BackColor = Color.FromArgb(163, 0, 0); });
			}
		}
		private void UpdateGui_SetSimulatorHostIpPort(string Value)
		{
			txtHostIpPort.InvokeIfNecessary(() => { txtHostIpPort.Text = Value; });
		}
		private void UpdateGui_SetSimulatorGoalList(MapData MapData)
		{
			if (MapData != null)
			{
				cbMoveTarget.InvokeIfNecessary(() =>
				{
					cbMoveTarget.SelectedIndex = -1;
					cbMoveTarget.SelectedItem = null;
					cbMoveTarget.Items.Clear();
				});
				cbGoalList.InvokeIfNecessary(() =>
				{
					cbGoalList.SelectedIndex = -1;
					cbGoalList.SelectedItem = null;
					cbGoalList.Items.Clear();
				});
				if (MapData.mGoals != null && MapData.mGoals.Count > 0)
				{
					string[] goalStrings = GetMapGoalFromMapData(MapData);
					cbMoveTarget.InvokeIfNecessary(() => { cbMoveTarget.Items.AddRange(goalStrings); });
					cbGoalList.InvokeIfNecessary(() => { cbGoalList.Items.AddRange(goalStrings); });
				}
			}
		}
		private void UpdateGui_SetSimulatorWidth(int Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[5].Cells[1].Value = Value; });
		}
		private void UpdateGui_SetSimulatorRotationDiameter(int Value)
		{
			dgvSimulatorInfo.InvokeIfNecessary(() => { dgvSimulatorInfo.Rows[5].Cells[3].Value = Value; });
		}

        #endregion
    }
}
