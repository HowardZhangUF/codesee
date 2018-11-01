﻿using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	class VehicleSimulator
	{
		public delegate void PositionUpdateEventHandler(string name, TowardPair position);
		public event PositionUpdateEventHandler PositionUpdate;

		public delegate void PathUpdateEventHandler(string name, List<Pair> path);
		public event PathUpdateEventHandler PathUpdate;

		public string Name { get; private set; } = "";
		private int X = 0;
		private int Y = 0;
		private double Toward = 0;
		public TowardPair Position { get { return new TowardPair(X, Y, Toward); } }
		public double TranslationSpeed { get; private set; } = 0; // mm/s
		public double RotationSpeed { get; private set; } = 0; // degree/s
		public string Status { get; private set; } = "Stopped";
		public List<Pair> Path { get; private set; } = null;
		private Thread MainThread;

		public VehicleSimulator(string name, double translationSpeed, double rotationSpeed)
		{
			Name = name;
			SetSpeed(translationSpeed, rotationSpeed);

			MainThread = new Thread(MainTask);
			MainThread.IsBackground = true;
			MainThread.Name = "Main";
			MainThread.Start();
		}

		public void SetSpeed(double translationSpeed, double rotationSpeed)
		{
			TranslationSpeed = translationSpeed;
			RotationSpeed = rotationSpeed;
		}

		public void SetPath(List<Pair> path)
		{
			Path = path;
		}
		
		public void StartMoving()
		{
			Status = "Moving";
		}

		public void PauseMoving()
		{
			Status = "Paused";
		}

		public void StopMoving()
		{
			Status = "Stopped";
		}

		private void MainTask()
		{
			int timeInterval = 500;
			while (true)
			{
				if (Status == "Moving")
				{
					MoveToNextPosition((double)timeInterval / 1000);
					if (Path == null || Path.Count() == 0)
						Status = "Stopped";
				}
				else if (Status == "Paused")
				{

				}
				else if (Status == "Stopped")
				{

				}

				Thread.Sleep(timeInterval);
			}
		}

		/// <summary>計算根據 Path 移動時， time 秒後的位置</summary>
		private void MoveToNextPosition(double time)
		{
			if (Path != null && Path.Count() > 0)
			{
				Pair targetPoint = Path[0];
				double targetToward = CalculateHorizontalAngle(new Pair(X, Y), Path[0]);
				
				// 進行旋轉
				if (RotationSpeed > 0 && !IsApproximatelyEqual(Toward, targetToward))
				{
					double diffAngle = targetToward - Toward;
					double rotationAngle = RotationSpeed * time;
					// 逆時針旋轉
					if ((diffAngle > 0 && diffAngle < 180) || (diffAngle < -180 && diffAngle > -360))
					{
						if (Math.Abs(diffAngle) < rotationAngle)
							Toward = targetToward;
						else
							Toward += rotationAngle;
					}
					// 順時針旋轉
					else
					{
						if (Math.Abs(diffAngle) < rotationAngle)
							Toward = targetToward;
						else
							Toward -= rotationAngle;
					}
				}
				// 進行平移
				else if (TranslationSpeed > 0 && IsApproximatelyEqual(Toward, targetToward) && !IsEqual(new Pair(X, Y), targetPoint))
				{
					double diffX = targetPoint.X - X;
					double diffY = targetPoint.Y - Y;
					double diffDistance = CalculateDistance(new Pair(X, Y), targetPoint);
					double towardInRadian = Toward * Math.PI / 180;
					double translateX = Math.Abs(Math.Cos(towardInRadian)) * TranslationSpeed * time;
					double translateY = Math.Abs(Math.Sin(towardInRadian)) * TranslationSpeed * time;

					// 向右向上移動
					if (diffX >= 0 && diffY >= 0)
					{
						if (X + translateX > targetPoint.X || Y + translateY > targetPoint.Y)
						{
							X = targetPoint.X;
							Y = targetPoint.Y;
						}
						else
						{
							X = (int)(X + translateX);
							Y = (int)(Y + translateY);
						}
					}
					// 向左向上移動
					else if (diffX < 0 && diffY >= 0)
					{
						if (X - translateX > targetPoint.X || Y + translateY > targetPoint.Y)
						{
							X = targetPoint.X;
							Y = targetPoint.Y;
						}
						else
						{
							X = (int)(X - translateX);
							Y = (int)(Y + translateY);
						}
					}
					// 向左向下移動
					else if (diffX < 0 && diffY < 0)
					{
						if (X - translateX > targetPoint.X || Y - translateY > targetPoint.Y)
						{
							X = targetPoint.X;
							Y = targetPoint.Y;
						}
						else
						{
							X = (int)(X - translateX);
							Y = (int)(Y - translateY);
						}
					}
					// 向右向下移動
					else if (diffX >= 0 && diffY < 0) 
					{
						if (X + translateX > targetPoint.X || Y - translateY > targetPoint.Y)
						{
							X = targetPoint.X;
							Y = targetPoint.Y;
						}
						else
						{
							X = (int)(X + translateX);
							Y = (int)(Y - translateY);
						}
					}

					// 判斷是否到達該點
					if (IsEqual(new Pair(X, Y), targetPoint))
					{
						Path.RemoveAt(0);
						PathUpdate?.Invoke(Name, Path);
					}
				}

				Console.WriteLine($"X:{X}, Y:{Y}, Toward:{Toward.ToString("F2")}");
				PositionUpdate?.Invoke(Name, new TowardPair(X, Y, Toward));
			}
		}

		private double CalculateHorizontalAngle(Pair point1, Pair point2)
		{
			double result = 0;
			if (point1 != null && point2 != null)
			{
				result = Math.Atan((point1.Y - point2.Y) / (point1.X - point2.X)) / Math.PI * 180;
			}
			return result;
		}

		private double CalculateDistance(Pair point1, Pair point2)
		{
			return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
		}

		private bool IsApproximatelyEqual(double num1, double num2)
		{
			return ((num1 + 0.1 > num2) && (num1 - 0.1 < num2));
		}

		private bool IsEqual(Pair point1, Pair point2)
		{
			return (point1.X == point2.X && point1.Y == point2.Y);
		}

		private void ChangePosition(int x, int y, double toward)
		{
			X = x;
			Y = y;
			Toward = toward;
		}
	}
}
