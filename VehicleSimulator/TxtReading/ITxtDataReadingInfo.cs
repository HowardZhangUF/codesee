using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
    interface ITxtDataReadingInfo
    {
        string Choose_Txt_Data_Path();
        List<string> Read_Txt_Data(string SimulatorTxtData);
    }
}
