using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011E RID: 286
	public class DefaultHideoutModel : HideoutModel
	{
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001838 RID: 6200 RVA: 0x000754DF File Offset: 0x000736DF
		public override CampaignTime HideoutHiddenDuration
		{
			get
			{
				return CampaignTime.Days(10f);
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001839 RID: 6201 RVA: 0x000754EB File Offset: 0x000736EB
		public override int CanAttackHideoutStartTime
		{
			get
			{
				return CampaignTime.SunSet + 1;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x0600183A RID: 6202 RVA: 0x000754F4 File Offset: 0x000736F4
		public override int CanAttackHideoutEndTime
		{
			get
			{
				return CampaignTime.SunRise;
			}
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x000754FB File Offset: 0x000736FB
		public override float GetRogueryXpGainAsGhost()
		{
			return MBRandom.RandomFloatRanged(1000f, 1400f);
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0007550C File Offset: 0x0007370C
		public override float GetRogueryXpGainOnHideoutMissionEnd(bool isSucceeded)
		{
			return (float)(isSucceeded ? MBRandom.RandomInt(700, 1000) : MBRandom.RandomInt(225, 400));
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x00075534 File Offset: 0x00073734
		public override float GetSendTroopsSuccessChance(Hideout hideout)
		{
			int skillValue = Hero.MainHero.GetSkillValue(DefaultSkills.Tactics);
			int skillValue2 = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
			return 0.3f + (float)(skillValue + skillValue2) / (325f + (float)skillValue + (float)skillValue2);
		}
	}
}
