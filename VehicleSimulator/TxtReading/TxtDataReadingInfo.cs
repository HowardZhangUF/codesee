using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace VehicleSimulator
{
    class TxtDataReadingInfo:ITxtDataReadingInfo
    {
       public TxtDataReadingInfo()
        {

        }

        public string TxtDataChoose()
        {
            OpenFileDialog rOpenFileDialog = new OpenFileDialog();
            rOpenFileDialog.Multiselect = false;
            rOpenFileDialog.Title = "請選擇.txt檔";
            rOpenFileDialog.Filter = "Txt Files(*.txt*)|*.txt*";

            if (rOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string TxtFilePath = rOpenFileDialog.FileName;

                return TxtFilePath;
            }
            else //如果沒有選擇.txt檔時 回傳null
            {
                return null;
            }
        }
        public List<string> TxtDataRead(string SimulatorTxtData)
        {

			StreamReader rStreamReader = new StreamReader(SimulatorTxtData);
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
