using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x0200016E RID: 366
	public static class SiegeEngineTypes
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x0008D4E6 File Offset: 0x0008B6E6
		public static MBReadOnlyList<SiegeEngineType> All
		{
			get
			{
				return Campaign.Current.AllSiegeEngineTypes;
			}
		}
	}
}
