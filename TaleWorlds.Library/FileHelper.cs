using System;
using System.IO;
using System.Threading.Tasks;

namespace TaleWorlds.Library
{
	// Token: 0x02000031 RID: 49
	public static class FileHelper
	{
		// Token: 0x060001A3 RID: 419 RVA: 0x00006C8B File Offset: 0x00004E8B
		public static SaveResult SaveFile(PlatformFilePath path, byte[] data)
		{
			return Common.PlatformFileHelper.SaveFile(path, data);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00006C99 File Offset: 0x00004E99
		public static SaveResult SaveFileString(PlatformFilePath path, string data)
		{
			return Common.PlatformFileHelper.SaveFileString(path, data);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00006CA7 File Offset: 0x00004EA7
		public static string GetFileFullPath(PlatformFilePath path)
		{
			return Common.PlatformFileHelper.GetFileFullPath(path);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00006CB4 File Offset: 0x00004EB4
		public static SaveResult AppendLineToFileString(PlatformFilePath path, string data)
		{
			return Common.PlatformFileHelper.AppendLineToFileString(path, data);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00006CC2 File Offset: 0x00004EC2
		public static Task<SaveResult> SaveFileAsync(PlatformFilePath path, byte[] data)
		{
			return Common.PlatformFileHelper.SaveFileAsync(path, data);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00006CD0 File Offset: 0x00004ED0
		public static Task<SaveResult> SaveFileStringAsync(PlatformFilePath path, string data)
		{
			return Common.PlatformFileHelper.SaveFileStringAsync(path, data);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00006CDE File Offset: 0x00004EDE
		public static string GetError()
		{
			return Common.PlatformFileHelper.GetError();
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00006CEA File Offset: 0x00004EEA
		public static bool FileExists(PlatformFilePath path)
		{
			return Common.PlatformFileHelper.FileExists(path);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00006CF7 File Offset: 0x00004EF7
		public static Task<string> GetFileContentStringAsync(PlatformFilePath path)
		{
			return Common.PlatformFileHelper.GetFileContentStringAsync(path);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00006D04 File Offset: 0x00004F04
		public static string GetFileContentString(PlatformFilePath path)
		{
			return Common.PlatformFileHelper.GetFileContentString(path);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00006D11 File Offset: 0x00004F11
		public static void DeleteFile(PlatformFilePath path)
		{
			Common.PlatformFileHelper.DeleteFile(path);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006D1F File Offset: 0x00004F1F
		public static PlatformFilePath[] GetFiles(PlatformDirectoryPath path, string searchPattern, SearchOption searchOption)
		{
			return Common.PlatformFileHelper.GetFiles(path, searchPattern, searchOption);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00006D2E File Offset: 0x00004F2E
		public static byte[] GetFileContent(PlatformFilePath filePath)
		{
			return Common.PlatformFileHelper.GetFileContent(filePath);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006D3B File Offset: 0x00004F3B
		public static byte[] GetMetaDataContent(PlatformFilePath filePath)
		{
			return Common.PlatformFileHelper.GetMetaDataContent(filePath);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00006D48 File Offset: 0x00004F48
		public static void CopyFile(PlatformFilePath source, PlatformFilePath target)
		{
			byte[] fileContent = FileHelper.GetFileContent(source);
			FileHelper.SaveFile(target, fileContent);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00006D64 File Offset: 0x00004F64
		public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
			if (!directoryInfo.Exists)
			{
				return;
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			Directory.CreateDirectory(destinationDir);
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				string text = Path.Combine(destinationDir, fileInfo.Name);
				fileInfo.CopyTo(text);
			}
			if (recursive)
			{
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					string text2 = Path.Combine(destinationDir, directoryInfo2.Name);
					FileHelper.CopyDirectory(directoryInfo2.FullName, text2, true);
				}
			}
		}
	}
}
