using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x0200016B RID: 363
	public static class Skills
	{
		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001B36 RID: 6966 RVA: 0x0008D4B5 File Offset: 0x0008B6B5
		public static MBReadOnlyList<SkillObject> All
		{
			get
			{
				return Campaign.Current.AllSkills;
			}
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x0008D4C1 File Offset: 0x0008B6C1
		public static SkillObject GetSkill(int i)
		{
			return Skills.All[i];
		}
	}
}
