using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace VehicleSimulator
{
    class TxtInfo:ITxtInfo
    {
       public TxtInfo()
        {

        }

        public string TxtChoose()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "請選擇.txt檔";
            dialog.Filter = "Txt Files(*.txt*)|*.txt*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string TxtFilePath = dialog.FileName;

                return TxtFilePath;
            }
            else //如果沒有選擇.txt檔時 回傳null
            {
                return null;
            }
        }
        public List<string> TxtRead(string SimulatorTxtData)
        {

			StreamReader rStreamReader = new StreamReader(SimulatorTxtData);
			SimulatorProcessContainer rCore = new SimulatorProcessContainer();
            List<string> ListReadLine = new List<string>();
            string ReadLine;

			do
			{
				ReadLine = rStreamReader.ReadLine();
				ListReadLine.Add(ReadLine);
                
            }
			while 
            (ReadLine != "End" && ReadLine != null);

            
            for(int index = 0; index < ListReadLine.Count; index++)
            {
                Console.WriteLine(ListReadLine[index]);
            }
               
            
			rStreamReader.Close();

            return ListReadLine;

		}
    }

    
}
