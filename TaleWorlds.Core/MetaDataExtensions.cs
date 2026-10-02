using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000B7 RID: 183
	public static class MetaDataExtensions
	{
		// Token: 0x06000993 RID: 2451 RVA: 0x0001F454 File Offset: 0x0001D654
		public static DateTime GetCreationTime(this MetaData metaData)
		{
			string text = ((metaData != null) ? metaData["CreationTime"] : null);
			if (text != null)
			{
				DateTime dateTime;
				if (DateTime.TryParse(text, out dateTime))
				{
					return dateTime;
				}
				long num;
				if (long.TryParse(text, out num))
				{
					return new DateTime(num);
				}
			}
			return DateTime.MinValue;
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0001F498 File Offset: 0x0001D698
		public static string[] GetModules(this MetaData metaData)
		{
			string text;
			if (metaData == null || !metaData.TryGetValue("Modules", out text))
			{
				return new string[0];
			}
			return text.Split(new char[] { ';' });
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0001F4D0 File Offset: 0x0001D6D0
		public static ApplicationVersion GetModuleVersion(this MetaData metaData, string moduleName)
		{
			string text = "Module_" + moduleName;
			string text2;
			if (metaData != null && metaData.TryGetValue(text, out text2))
			{
				try
				{
					return ApplicationVersion.FromString(text2, 0);
				}
				catch (Exception ex)
				{
					Debug.FailedAssert(ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\MetaDataExtensions.cs", "GetModuleVersion", 45);
				}
			}
			return ApplicationVersion.Empty;
		}
	}
}
