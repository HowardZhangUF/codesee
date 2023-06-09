﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForVM
{
	public static class FileOperation
	{
		public static void CreateDirectory(string DirectoryPath)
		{
			if (!Directory.Exists(DirectoryPath))
			{
				Directory.CreateDirectory(DirectoryPath);
			}
		}
		public static void DeleteDirectory(string DirectoryPath)
		{
			if (Directory.Exists(DirectoryPath))
			{
				Directory.Delete(DirectoryPath, true);
			}
		}
		public static void CopyFile(string SrcFilePath, string DstDirectoryPath)
		{
			if (File.Exists(SrcFilePath))
			{
				FileInfo srcFileInfo = new FileInfo(SrcFilePath);
				DirectoryInfo dstDirectoryInfo = new DirectoryInfo(DstDirectoryPath);

				CopyFile(srcFileInfo, dstDirectoryInfo);
			}
		}
		public static void CopyFile(FileInfo SrcFileInfo, DirectoryInfo DstDirectoryInfo)
		{
			Directory.CreateDirectory(DstDirectoryInfo.FullName);

			SrcFileInfo.CopyTo(Path.Combine(DstDirectoryInfo.FullName, SrcFileInfo.Name), true);
		}
		public static void CopyFileViaCommandPrompt(string SrcFilePath, string DstDirectoryPath)
		{
			// [參考資料](https://ss64.com/nt/copy.html)
			// 假設輸入參數為 srcFileName 與 dst :
			// + 如果 dst 是一個已存在的資料夾，則會將 srcFileName 複製至該資料夾下
			// + 如果 dst 是一個已存在的檔案，則會將 srcFileName 複製並取代該檔案
			// + 如果 dst 是不存在的檔案/資料夾，則會將 srcFileName 檔案複製並重新命名為 dst
			if (File.Exists(SrcFilePath))
			{
				string fullCmd = $"COPY \"{SrcFilePath}\" \"{DstDirectoryPath}\"";
				ShellCommandExecutor.ExecuteInBackground(fullCmd);
			}
		}
		public static void CopyFilesViaCommandPrompt(IEnumerable<string> SrcFilePaths, string DstDirectoryPath)
		{
			foreach (string filePath in SrcFilePaths)
			{
				CopyFileViaCommandPrompt(filePath, DstDirectoryPath);
			}
		}
		public static void CopyDirectory(string SrcDirectoryPath, string DstDirectoryPath)
		{
			if (Directory.Exists(SrcDirectoryPath))
			{
				DirectoryInfo srcDirectoryInfo = new DirectoryInfo(SrcDirectoryPath);
				DirectoryInfo dstDirectoryInfo = new DirectoryInfo(DstDirectoryPath);

				CopyDirectory(srcDirectoryInfo, dstDirectoryInfo);
			}
		}
		public static void CopyDirectory(DirectoryInfo SrcDirectoryInfo, DirectoryInfo DstDirectoryInfo)
		{
			// Reference: https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo?redirectedfrom=MSDN&view=netframework-4.8

			Directory.CreateDirectory(DstDirectoryInfo.FullName);

			// Copy each file into the new directory
			foreach (FileInfo fileInfo in SrcDirectoryInfo.GetFiles())
			{
				//Console.WriteLine(@"Copying {0}\{1}", DstDirectoryInfo.FullName, fileInfo.Name);
				fileInfo.CopyTo(Path.Combine(DstDirectoryInfo.FullName, fileInfo.Name), true);
			}

			// Copy each subdirectory using recursion
			foreach (DirectoryInfo srcSubDirectoryInfo in SrcDirectoryInfo.GetDirectories())
			{
				DirectoryInfo dstSubDirectoryInfo = DstDirectoryInfo.CreateSubdirectory(srcSubDirectoryInfo.Name);
				CopyDirectory(srcSubDirectoryInfo, dstSubDirectoryInfo);
			}
		}
		public static void CopyDirectoryViaCommandPrompt(string SrcDirectoryPath, string DstDirectoryPath)
		{
			if (Directory.Exists(SrcDirectoryPath))
			{
				DeleteFile(DstDirectoryPath);
				string fullCmd = $"XCOPY \"{SrcDirectoryPath}\" \"{DstDirectoryPath}\" /I /S /E";
				ShellCommandExecutor.ExecuteInCommadPrompt(fullCmd);
			}
		}
		public static void CopyDirectoryUnder(string SrcDirectoryPath, string DstDirectoryPath)
		{
			if (Directory.Exists(SrcDirectoryPath))
			{
				DirectoryInfo srcDirectoryInfo = new DirectoryInfo(SrcDirectoryPath);
				DirectoryInfo dstDirectoryInfo = new DirectoryInfo(Path.Combine(DstDirectoryPath, srcDirectoryInfo.Name));

				CopyDirectory(srcDirectoryInfo, dstDirectoryInfo);
			}
		}
		public static void CopyDirectoriesUnder(IEnumerable<string> SrcDirectoryPaths, string DstDirectoryPath)
		{
			foreach (string dirPath in SrcDirectoryPaths)
			{
				CopyDirectoryUnder(dirPath, DstDirectoryPath);
			}
		}
		public static void CopyDirectoryUnderViaCommandPrompt(string SrcDirectoryPath, string DstDirectoryPath)
		{
			if (Directory.Exists(SrcDirectoryPath))
			{
				DirectoryInfo srcDirectoryInfo = new DirectoryInfo(SrcDirectoryPath);
				DirectoryInfo dstDirectoryInfo = new DirectoryInfo(Path.Combine(DstDirectoryPath, srcDirectoryInfo.Name));

				CopyDirectoryViaCommandPrompt(srcDirectoryInfo.FullName, dstDirectoryInfo.FullName);
			}
		}
		public static void CopyDirectoriesUnderViaCommandPrompt(IEnumerable<string> SrcDirectoryPaths, string DstDirectoryPath)
		{
			foreach (string dirPath in SrcDirectoryPaths)
			{
				CopyDirectoryUnderViaCommandPrompt(dirPath, DstDirectoryPath);
			}
		}
		public static void DeleteFile(string FilePath)
		{
			if (File.Exists(FilePath))
			{
				File.Delete(FilePath);
			}
		}
		public static void CompressDirectory(string DirectoryPath)
		{
			if (Directory.Exists(DirectoryPath))
			{
				string pathOf7z = "C:\\Program Files\\7-Zip\\7z.exe";
				string compressedFilePath = $"{DirectoryPath}.7z";
				string fullCmd = $"\"{pathOf7z}\" a {compressedFilePath} {DirectoryPath} -p27635744 -mhe";
				if (File.Exists(pathOf7z))
				{
					DeleteFile(compressedFilePath);
					ShellCommandExecutor.ExecuteInCommadPrompt(fullCmd);
				}
			}
		}
		/// <summary>將該資料夾清空</summary>
		public static void CleanDirectory(string Dstdir)
        {
			try
			{
				var entryPaths = Directory.EnumerateFileSystemEntries(Dstdir);
				foreach (string entryPath in entryPaths)
				{
					if (File.Exists(entryPath))
					{
						File.Delete(entryPath);
					}
					else if (Directory.Exists(entryPath))
					{
						Directory.Delete(entryPath, true);
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionHandling.HandleException(ex);
			}
		}
		/// <summary>只複製Map跟HistoryLog.db </summary>
		public static void JeffHandleFile(string srcdir, string dstdir,string logbatdstdir)
		{
            
			if (!Directory.Exists(dstdir))
			{
				Directory.CreateDirectory(dstdir);
			}
			string logbatch = $@".\..\VehicleManagerData\LogExport.\LogExport.bat";
			string LogExportPath= $@".\..\VehicleManagerData\LogExport";
			string pathOf7z = "C:\\Program Files\\7-Zip\\7z.exe";
			
			if (!File.Exists(logbatch))
			{
				CleanDirectory(LogExportPath);
				using (StreamWriter sw = new StreamWriter(logbatch,append:true))  
				{
					// Add some text to the file.
					sw.WriteLine($@"robocopy {srcdir}\Map {logbatdstdir}\Map /MIR");//複製目錄(鏡像)
					sw.WriteLine($@"robocopy {srcdir}\Database {logbatdstdir} HistoryLog.db /W:10");//複製HistoryLog檔案																									
					sw.WriteLine($"\"{pathOf7z}\" a {logbatdstdir}.7z {logbatdstdir} -p27635744 -mhe -mx=1");//壓縮資料夾並加密碼 且為最快速壓縮
					sw.WriteLine($@"del %0");
				}
				
				
			}

			
			//List<string> fullcmds = new List<string>();
			//fullcmds.Add($@"Xcopy {srcdir}\Map {dstdir}\Map /E/H/C/I/Y");//複製目錄
			//fullcmds.Add($"\"{pathOf7z}\" a  {dstdir}\\HistoryLog.7z {srcdir}\\Database\\HistoryLog.db ");//解壓縮檔案
			//foreach (var fullcmd in fullcmds)
			//	ShellCommandExecutor.ExecuteInCommadPrompt(fullcmd);               
		}
	}
}
