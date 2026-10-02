using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.Library
{
	// Token: 0x0200007D RID: 125
	public struct PlatformDirectoryPath
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
		public PlatformDirectoryPath(PlatformFileType type, string path)
		{
			this.Type = type;
			this.Path = path;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000FAB4 File Offset: 0x0000DCB4
		public static PlatformDirectoryPath operator +(PlatformDirectoryPath path, string str)
		{
			return new PlatformDirectoryPath(path.Type, path.Path + str);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000FACD File Offset: 0x0000DCCD
		public override string ToString()
		{
			return this.Type + " " + this.Path;
		}

		// Token: 0x04000163 RID: 355
		public PlatformFileType Type;

		// Token: 0x04000164 RID: 356
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string Path;
	}
}
