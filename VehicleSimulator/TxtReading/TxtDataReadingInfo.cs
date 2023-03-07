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

        /// <summary>
        /// 獲取文字檔檔案完整路徑
        /// </summary>
        /// <returns>若有選取文字檔(文字檔路徑)，若無(null)。</returns>
        public string Choose_Txt_Data_Path()
        {
            using (var ofd=new OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.Title = "請選擇.txt檔";
                ofd.Filter = "Txt Files(*.txt*)|*.txt*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string TxtFilePath = ofd.FileName;

                    return TxtFilePath;
                }
                else //如果沒有選擇.txt檔時 回傳null
                {
                    return null;
                }
            } 
        }

        /// <summary>
        /// 讀取文字檔內容
        /// </summary>
        /// <param name="SimulatorTxtData">文字檔檔案完整路徑</param>
        /// <returns>若有內文(回傳其内容)，若路徑不存在(null)</returns>
        public List<string> Read_Txt_Data(string SimulatorTxtData)
        {
           using (var rStreamReader=new StreamReader(SimulatorTxtData))
           {
                List<string> ListReadLine = new List<string>();
                string ReadLine;

                do
                {
                    ReadLine = rStreamReader.ReadLine();
                    ListReadLine.Add(ReadLine);
                }
                while
                (ReadLine != "End" && ReadLine != null);

                return ListReadLine;
            }
		}
    }
}
