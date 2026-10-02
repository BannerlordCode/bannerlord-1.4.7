using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000062 RID: 98
	public static class ManagedDllFolder
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x000086D8 File Offset: 0x000068D8
		public static string Name
		{
			get
			{
				if (!string.IsNullOrEmpty(ManagedDllFolder._overridenFolder))
				{
					return ManagedDllFolder._overridenFolder;
				}
				if (ApplicationPlatform.CurrentPlatform == Platform.Orbis)
				{
					return "/app0/";
				}
				if (ApplicationPlatform.CurrentPlatform == Platform.Durango)
				{
					return "/";
				}
				return "";
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000870D File Offset: 0x0000690D
		public static void OverrideManagedDllFolder(string overridenFolder)
		{
			ManagedDllFolder._overridenFolder = overridenFolder;
		}

		// Token: 0x04000122 RID: 290
		private static string _overridenFolder;
	}
}
