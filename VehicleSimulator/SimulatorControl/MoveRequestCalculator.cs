using Module.Map.Map;
using Module.Map.MapFactory;
using Module.Pathfinding.PathFinder;
using Module.Pathfinding.PathOptimizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public class MoveRequestCalculator : IMoveRequestCalculator
	{
		private IMap mMap = null;
		private IPathFinderUsingAStar mPathFinderUsingAStar = new PathFinderUsingAStar();
		private IPathOptimizer mPathOptimizer = new PathOptimizer();

		private string mFilePath = null;
		private int Step_Size = 70;



		public void SetMap(string FilePath)
		{
			if (!File.Exists(FilePath)) return;
			mFilePath = FilePath;
			mMap = MapFactory.CreateMapUsingQuadTree(EMapFileType.iTS, FilePath);
			MapFactory.CalculateInfluenceGrid(mMap, Step_Size, 3, 0.2f);
			mPathFinderUsingAStar.Set(mMap);
			mPathOptimizer.Set(mMap);
		}

		private void SetMap(string FilePath,int Step_Size)
        {
			if (!File.Exists(FilePath)) return;
			mMap = MapFactory.CreateMapUsingQuadTree(EMapFileType.iTS, FilePath);
			MapFactory.CalculateInfluenceGrid(mMap, Step_Size, 3, 0.2f);
			mPathFinderUsingAStar.Set(mMap);
			mPathOptimizer.Set(mMap);
		}

		public List<MoveRequest> Calculate(Point Start, string TargetName,int Width,int RotationDiameter)
		{
			if (mMap.mGoals.Any(o => o.mName == TargetName))
			{
				var Goal = mMap.mGoals.First(o => o.mName == TargetName);
				if (IsGoalRequestToward(Goal))
				{
					return Calculate(Start, new Point(Goal.mX, Goal.mY), (int)Goal.mToward,Width,RotationDiameter, IsGoalMoveBackward(Goal));
				}
				else
				{
					return Calculate(Start, new Point(Goal.mX, Goal.mY), Width,RotationDiameter,IsGoalMoveBackward(Goal));
				}
			}
			else
			{
				return null;

			}
		}
		public List<MoveRequest> Calculate(Point Start, Point End, int Width, int RotationDiameter, bool IsMoveBackward = false)
		{
			int SizeChoose;
			if (Width >= RotationDiameter) { SizeChoose=Width;SetMap(mFilePath,SizeChoose/10); } else { SizeChoose = RotationDiameter; SetMap(mFilePath, SizeChoose/10); }
			if (mMap == null)
			{
				return new List<MoveRequest>() { new MoveRequest(End.mX, End.mY, IsMoveBackward) };
			}
			else
			{
				int count = 1;
				Module.Pathfinding.Object.Path path = null;
				PathFindingResult pathfindingResult = PathFindingResult.None;

				do
				{
					mPathFinderUsingAStar.FindPath(new Module.Map.GeometricShape.Point(Start.mX, Start.mY), new Module.Map.GeometricShape.Point(End.mX, End.mY), SizeChoose, SizeChoose / (count + 1), out pathfindingResult, out path);
					count++;
				} while (pathfindingResult != PathFindingResult.PathFound && count < 10);
				path = mPathOptimizer.OptimizePath(path, SizeChoose);
				return Convert(path, IsMoveBackward);
			}
		}
		public List<MoveRequest> Calculate(Point Start, Point End, int EndToward,int Width,int RotationDiameter, bool IsMoveBackward = false)
		{
			int SizeChoose;
			if (Width >= RotationDiameter) { SizeChoose = Width; SetMap(mFilePath, SizeChoose / 10); } else { SizeChoose = RotationDiameter; SetMap(mFilePath, SizeChoose / 10); }
			if (mMap == null)
			{
				return new List<MoveRequest>() { new MoveRequest(End.mX, End.mY, EndToward, IsMoveBackward) };
			}
			else
			{
				int count = 1;
				Module.Pathfinding.Object.Path path = null;
				PathFindingResult pathfindingResult = PathFindingResult.None;


				do
                {
                    mPathFinderUsingAStar.FindPath(new Module.Map.GeometricShape.Point(Start.mX, Start.mY), new Module.Map.GeometricShape.Point(End.mX, End.mY), SizeChoose, SizeChoose / (count + 1), out pathfindingResult, out path);
                    count++; 
                } while (pathfindingResult != PathFindingResult.PathFound && count<10);

				

				//mPathFinderUsingAStar.FindPath(new Module.Map.GeometricShape.Point(Start.mX, Start.mY), new Module.Map.GeometricShape.Point(End.mX, End.mY), SizeChoose, SizeChoose / 5, out PathFindingResult pathfindingResult, out path);

				path = mPathOptimizer.OptimizePath(path, SizeChoose );
				return Convert(path, EndToward, IsMoveBackward);
				
			}
		}

		private List<MoveRequest> Convert(Module.Pathfinding.Object.Path Path, bool IsMoveBackward = false)
		{
			return Path.mPoints.Skip(1).Select(o => new MoveRequest(o.mX, o.mY, IsMoveBackward)).ToList();
		}
		private List<MoveRequest> Convert(Module.Pathfinding.Object.Path Path, int EndToward, bool IsMoveBackward = false)
		{
			List<MoveRequest> result = new List<MoveRequest>();
			for (int i = 1; i < Path.mPoints.Count - 1; ++i)
			{
				result.Add(new MoveRequest(Path.mPoints[i].mX, Path.mPoints[i].mY, IsMoveBackward));
			}
			result.Add(new MoveRequest(Path.mPoints.Last().mX, Path.mPoints.Last().mY, EndToward, IsMoveBackward));
			return result;
		}

		private static bool IsGoalRequestToward(Module.Map.MapObject.Goal Goal)
		{
			return Goal.mToward > 360 ? false : true;
		}
		private static bool IsGoalMoveBackward(Module.Map.MapObject.Goal Goal)
		{
			return Goal.mStyleName == "MovementInverse";
		}
	}
}
