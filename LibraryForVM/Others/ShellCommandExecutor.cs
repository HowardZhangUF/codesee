﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForVM
{
	public static class ShellCommandExecutor
	{
		public static string ExecuteInBackground(string shellCommand) // 使用此方法執行"不會"跳出 Command Prompt 視窗
		{
			string result = string.Empty;
			using (System.Diagnostics.Process process = new System.Diagnostics.Process())
			{
				process.StartInfo = new ProcessStartInfo();
				process.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
				process.StartInfo.Arguments = $"/C {shellCommand.ToString()}";
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.Start();
				process.StandardOutput.ReadToEnd();//加入此行可以避免大檔案時卡住
				process.WaitForExit(); // 會等 process 執行完才繼續往下
				result = process.ExitCode == 0 ? process.StandardOutput.ReadToEnd() : $"{process.ExitCode.ToString()} : {process.StandardError.ReadToEnd()}";
			}
			return result;
		}
		public static void ExecuteInCommadPrompt(string shellCommand) // 使用此方法執行"會"跳出 Command Prompt 視窗
		{
			string result = string.Empty;
			using (System.Diagnostics.Process process = new System.Diagnostics.Process())
			{
				process.StartInfo = new ProcessStartInfo();
				process.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
				process.StartInfo.Arguments = $"/C {shellCommand.ToString()}";
				process.Start();
				process.WaitForExit(); // 會等 process 執行完才繼續往下
			}
		}

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
