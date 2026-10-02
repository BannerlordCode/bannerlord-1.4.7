using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.GameComponents
{
	// Token: 0x02000045 RID: 69
	public class StoryModeKingdomDecisionPermissionModel : KingdomDecisionPermissionModel
	{
		// Token: 0x06000450 RID: 1104 RVA: 0x00019134 File Offset: 0x00017334
		public override bool IsPolicyDecisionAllowed(PolicyObject policy)
		{
			return base.BaseModel.IsPolicyDecisionAllowed(policy);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00019142 File Offset: 0x00017342
		public override bool IsAnnexationDecisionAllowed(Settlement annexedSettlement)
		{
			return base.BaseModel.IsAnnexationDecisionAllowed(annexedSettlement);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00019150 File Offset: 0x00017350
		public override bool IsExpulsionDecisionAllowed(Clan expelledClan)
		{
			return base.BaseModel.IsExpulsionDecisionAllowed(expelledClan);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0001915E File Offset: 0x0001735E
		public override bool IsKingSelectionDecisionAllowed(Kingdom kingdom)
		{
			return base.BaseModel.IsKingSelectionDecisionAllowed(kingdom);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0001916C File Offset: 0x0001736C
		public override bool IsWarDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			if (StoryModeManager.Current.MainStoryLine.ThirdPhase != null)
			{
				MBReadOnlyList<Kingdom> oppositionKingdoms = StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms;
				if (oppositionKingdoms.IndexOf(kingdom1) >= 0 && oppositionKingdoms.IndexOf(kingdom2) >= 0)
				{
					reason = GameTexts.FindText("str_kingdom_diplomacy_war_truce_disabled_reason_story", null);
					return false;
				}
			}
			return base.BaseModel.IsWarDecisionAllowedBetweenKingdoms(kingdom1, kingdom2, out reason);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x000191D0 File Offset: 0x000173D0
		public override bool IsPeaceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			if (StoryModeManager.Current.MainStoryLine.ThirdPhase != null)
			{
				MBReadOnlyList<Kingdom> oppositionKingdoms = StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms;
				MBReadOnlyList<Kingdom> allyKingdoms = StoryModeManager.Current.MainStoryLine.ThirdPhase.AllyKingdoms;
				if ((oppositionKingdoms.IndexOf(kingdom1) >= 0 && allyKingdoms.IndexOf(kingdom2) >= 0) || (oppositionKingdoms.IndexOf(kingdom2) >= 0 && allyKingdoms.IndexOf(kingdom1) >= 0))
				{
					reason = GameTexts.FindText("str_kingdom_diplomacy_war_truce_disabled_reason_story", null);
					return false;
				}
			}
			return base.BaseModel.IsPeaceDecisionAllowedBetweenKingdoms(kingdom1, kingdom2, out reason);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001925D File Offset: 0x0001745D
		public override bool IsStartAllianceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			return base.BaseModel.IsStartAllianceDecisionAllowedBetweenKingdoms(kingdom1, kingdom2, out reason);
		}
	}
}
