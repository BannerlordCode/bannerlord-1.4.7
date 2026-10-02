using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace StoryMode.GameComponents
{
	// Token: 0x0200003C RID: 60
	public class StoryModeBanditDensityModel : BanditDensityModel
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00018ACD File Offset: 0x00016CCD
		public override int NumberOfMaximumBanditPartiesAroundEachHideout
		{
			get
			{
				if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
				{
					return 0;
				}
				return base.BaseModel.NumberOfMaximumBanditPartiesAroundEachHideout;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00018AED File Offset: 0x00016CED
		public override int NumberOfMaximumBanditPartiesInEachHideout
		{
			get
			{
				if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
				{
					return 0;
				}
				return base.BaseModel.NumberOfMaximumBanditPartiesInEachHideout;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00018B0D File Offset: 0x00016D0D
		public override int NumberOfMaximumHideoutsAtEachBanditFaction
		{
			get
			{
				if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
				{
					return 0;
				}
				return base.BaseModel.NumberOfMaximumHideoutsAtEachBanditFaction;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00018B2D File Offset: 0x00016D2D
		public override int NumberOfInitialHideoutsAtEachBanditFaction
		{
			get
			{
				if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
				{
					return 0;
				}
				return base.BaseModel.NumberOfInitialHideoutsAtEachBanditFaction;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00018B4D File Offset: 0x00016D4D
		public override int NumberOfMinimumBanditPartiesInAHideoutToInfestIt
		{
			get
			{
				return base.BaseModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00018B5A File Offset: 0x00016D5A
		public override int NumberOfMinimumBanditTroopsInHideoutMission
		{
			get
			{
				return base.BaseModel.NumberOfMinimumBanditTroopsInHideoutMission;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00018B67 File Offset: 0x00016D67
		public override int NumberOfMaximumTroopCountForFirstFightInHideout
		{
			get
			{
				return base.BaseModel.NumberOfMaximumTroopCountForFirstFightInHideout;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00018B74 File Offset: 0x00016D74
		public override int NumberOfMaximumTroopCountForBossFightInHideout
		{
			get
			{
				return base.BaseModel.NumberOfMaximumTroopCountForBossFightInHideout;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00018B81 File Offset: 0x00016D81
		public override float SpawnPercentageForFirstFightInHideoutMission
		{
			get
			{
				return base.BaseModel.SpawnPercentageForFirstFightInHideoutMission;
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00018B8E File Offset: 0x00016D8E
		public override int GetMaximumTroopCountForHideoutMission(MobileParty party, bool isAssault)
		{
			return base.BaseModel.GetMaximumTroopCountForHideoutMission(party, isAssault);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00018B9D File Offset: 0x00016D9D
		public override bool IsPositionInsideNavalSafeZone(CampaignVec2 position)
		{
			return base.BaseModel.IsPositionInsideNavalSafeZone(position);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00018BAB File Offset: 0x00016DAB
		public override int GetMaxSupportedNumberOfLootersForClan(Clan clan)
		{
			if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
			{
				return 0;
			}
			return base.BaseModel.GetMaxSupportedNumberOfLootersForClan(clan);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00018BCC File Offset: 0x00016DCC
		public override int GetMinimumTroopCountForHideoutMission(MobileParty party, bool isAssault)
		{
			return base.BaseModel.GetMinimumTroopCountForHideoutMission(party, isAssault);
		}
	}
}
