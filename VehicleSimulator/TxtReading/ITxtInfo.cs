using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
    interface ITxtInfo
    {
        string TxtChoose();
        List<string> TxtRead(string SimulatorTxtData);
    }
}
