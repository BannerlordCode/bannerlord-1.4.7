using System;
using System.IO;
using System.Reflection;

namespace TaleWorlds.Library
{
	// Token: 0x02000033 RID: 51
	public static class BasePath
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00006EDC File Offset: 0x000050DC
		public static string Name
		{
			get
			{
				if (ApplicationPlatform.CurrentEngine == EngineType.UnrealEngine)
				{
					return Path.GetFullPath(Path.GetDirectoryName(typeof(BasePath).Assembly.Location) + "/../../");
				}
				if (ApplicationPlatform.CurrentPlatform == Platform.Orbis)
				{
					return "/app0/";
				}
				if (ApplicationPlatform.CurrentPlatform == Platform.Durango)
				{
					return "/";
				}
				if (ApplicationPlatform.CurrentPlatform == Platform.Web)
				{
					return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/../../";
				}
				return "../../";
			}
		}
	}
}
