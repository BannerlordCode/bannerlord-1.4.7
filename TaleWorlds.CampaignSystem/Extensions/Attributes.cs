using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x0200016C RID: 364
	public static class Attributes
	{
		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06001B38 RID: 6968 RVA: 0x0008D4CE File Offset: 0x0008B6CE
		public static MBReadOnlyList<CharacterAttribute> All
		{
			get
			{
				return Campaign.Current.AllCharacterAttributes;
			}
		}
	}
}
