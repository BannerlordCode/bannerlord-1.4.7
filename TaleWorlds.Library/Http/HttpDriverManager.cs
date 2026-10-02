using System;
using System.Collections.Concurrent;

namespace TaleWorlds.Library.Http
{
	// Token: 0x020000AF RID: 175
	public static class HttpDriverManager
	{
		// Token: 0x0600069A RID: 1690 RVA: 0x00016D95 File Offset: 0x00014F95
		public static void AddHttpDriver(string name, IHttpDriver driver)
		{
			if (HttpDriverManager._httpDrivers.Count == 0)
			{
				HttpDriverManager._defaultHttpDriver = name;
			}
			HttpDriverManager._httpDrivers[name] = driver;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00016DB5 File Offset: 0x00014FB5
		public static void SetDefault(string name)
		{
			if (HttpDriverManager.GetHttpDriver(name) != null)
			{
				HttpDriverManager._defaultHttpDriver = name;
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00016DC8 File Offset: 0x00014FC8
		public static IHttpDriver GetHttpDriver(string name)
		{
			IHttpDriver httpDriver;
			HttpDriverManager._httpDrivers.TryGetValue(name, out httpDriver);
			if (httpDriver == null)
			{
				Debug.Print("HTTP driver not found:" + (name ?? "not set"), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			return httpDriver;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00016E0C File Offset: 0x0001500C
		public static IHttpDriver GetDefaultHttpDriver()
		{
			if (HttpDriverManager._defaultHttpDriver == null)
			{
				HttpDriverManager.AddHttpDriver("DotNet", new DotNetHttpDriver());
			}
			return HttpDriverManager.GetHttpDriver(HttpDriverManager._defaultHttpDriver);
		}

		// Token: 0x040001F8 RID: 504
		private static ConcurrentDictionary<string, IHttpDriver> _httpDrivers = new ConcurrentDictionary<string, IHttpDriver>();

		// Token: 0x040001F9 RID: 505
		private static string _defaultHttpDriver;
	}
}
