using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000030 RID: 48
	public static class UiStringHelper
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x00006C74 File Offset: 0x00004E74
		public static bool IsStringNoneOrEmptyForUi(string str)
		{
			return string.IsNullOrEmpty(str) || str == "none";
		}
	}
}
