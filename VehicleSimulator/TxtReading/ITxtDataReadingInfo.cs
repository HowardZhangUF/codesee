using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
    interface ITxtDataReadingInfo
    {
        string TxtDataChoose();
        List<string> TxtDataRead(string SimulatorTxtData);
    }
}
