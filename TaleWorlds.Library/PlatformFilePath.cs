using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.Library
{
	// Token: 0x02000080 RID: 128
	public struct PlatformFilePath
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x00010024 File Offset: 0x0000E224
		public PlatformFilePath(PlatformDirectoryPath folderPath, string fileName)
		{
			this.FolderPath = folderPath;
			this.FileName = fileName;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00010034 File Offset: 0x0000E234
		public static PlatformFilePath operator +(PlatformFilePath path, string str)
		{
			return new PlatformFilePath(path.FolderPath, path.FileName + str);
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0001004D File Offset: 0x0000E24D
		public string FileFullPath
		{
			get
			{
				return Common.PlatformFileHelper.GetFileFullPath(this);
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00010060 File Offset: 0x0000E260
		public string GetFileNameWithoutExtension()
		{
			int num = this.FileName.LastIndexOf('.');
			if (num == -1)
			{
				return this.FileName;
			}
			return this.FileName.Substring(0, num);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00010093 File Offset: 0x0000E293
		public override string ToString()
		{
			return this.FolderPath.ToString() + " - " + this.FileName;
		}

		// Token: 0x0400016B RID: 363
		public PlatformDirectoryPath FolderPath;

		// Token: 0x0400016C RID: 364
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string FileName;
	}
}
