using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public interface IMoveRequestCalculator
	{
		void SetMap(string FilePath);

		List<MoveRequest> Calculate(Point Start, string TargetName,int Width,int RotationDiameter);
		List<MoveRequest> Calculate(Point Start, Point End, int Width, int RotationDiameter, bool IsMoveBackward = false);
		List<MoveRequest> Calculate(Point Start, Point End, int EndToward, int Width, int RotationDiameter, bool IsMoveBackward = false);
	}
}
