using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleSimulator
{
	public partial class UcContentOfAbout : UserControl
	{
		private int Major = 1;
		private int NewFunction = 1;
		private int Debug = 2;
		private int Date = 230306;
		private string Dot = " . ";


		public UcContentOfAbout() {
			InitializeComponent();
			lbVersionNumber.Text = ShowVersionNumber(Major, NewFunction, Debug, Date);
		}

		/// <summary>
		/// 合併所有版本號資訊並轉成字串
		/// </summary>
		/// <param name="Major">主版本號</param>
		/// <param name="NewFunction">新增新功能次數</param>
		/// <param name="Debug">修改錯誤次數</param>
		/// <param name="Date">日期:YY/MM/DD</param>
		/// <returns>完整版本號</returns>
		private string ShowVersionNumber(int Major,int NewFunction,int Debug,int Date)
        { 
			return (Convert.ToString(Major) + Dot+ Convert.ToString(NewFunction) + Dot + Convert.ToString(Debug) + Dot + Convert.ToString(Date));
        }

	}
}
