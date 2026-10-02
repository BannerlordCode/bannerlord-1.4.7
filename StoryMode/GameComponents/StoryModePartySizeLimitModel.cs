using System;
using System.Collections.Generic;
using System.Linq;
using StoryMode.Quests.SecondPhase.ConspiracyQuests;
using StoryMode.Quests.ThirdPhase;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;

namespace StoryMode.GameComponents
{
	// Token: 0x02000047 RID: 71
	public class StoryModePartySizeLimitModel : PartySizeLimitModel
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x000192B6 File Offset: 0x000174B6
		public override int MinimumNumberOfVillagersAtVillagerParty
		{
			get
			{
				return base.BaseModel.MinimumNumberOfVillagersAtVillagerParty;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x000192C3 File Offset: 0x000174C3
		private DefeatTheConspiracyQuestBehavior DefeatTheConspiracyQuestBehavior
		{
			get
			{
				if (this._defeatTheConspiracyQuestBehavior != null)
				{
					return this._defeatTheConspiracyQuestBehavior;
				}
				this._defeatTheConspiracyQuestBehavior = Campaign.Current.GetCampaignBehavior<DefeatTheConspiracyQuestBehavior>();
				return this._defeatTheConspiracyQuestBehavior;
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000192EA File Offset: 0x000174EA
		public override ExplainedNumber CalculateGarrisonPartySizeLimit(Settlement settlement, bool includeDescriptions = false)
		{
			return base.BaseModel.CalculateGarrisonPartySizeLimit(settlement, includeDescriptions);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000192F9 File Offset: 0x000174F9
		public override TroopRoster FindAppropriateInitialRosterForMobileParty(MobileParty party, PartyTemplateObject partyTemplate)
		{
			return base.BaseModel.FindAppropriateInitialRosterForMobileParty(party, partyTemplate);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00019308 File Offset: 0x00017508
		public override List<Ship> FindAppropriateInitialShipsForMobileParty(MobileParty party, PartyTemplateObject partyTemplate)
		{
			return base.BaseModel.FindAppropriateInitialShipsForMobileParty(party, partyTemplate);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00019317 File Offset: 0x00017517
		public override int GetAssumedPartySizeForLordParty(Hero leaderHero, IFaction partyMapFaction, Clan actualClan)
		{
			return base.BaseModel.GetAssumedPartySizeForLordParty(leaderHero, partyMapFaction, actualClan);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00019327 File Offset: 0x00017527
		public override int GetClanTierPartySizeEffectForHero(Hero hero)
		{
			return base.BaseModel.GetClanTierPartySizeEffectForHero(hero);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00019335 File Offset: 0x00017535
		public override int GetIdealVillagerPartySize(Village village)
		{
			return base.BaseModel.GetIdealVillagerPartySize(village);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00019343 File Offset: 0x00017543
		public override int GetNextClanTierPartySizeEffectChangeForHero(Hero hero)
		{
			return base.BaseModel.GetNextClanTierPartySizeEffectChangeForHero(hero);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00019354 File Offset: 0x00017554
		public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			if (party.IsMobile)
			{
				QuestBase questBase = Campaign.Current.QuestManager.Quests.FirstOrDefault<QuestBase>((QuestBase q) => !q.IsFinalized && q.GetType() == typeof(DisruptSupplyLinesConspiracyQuest));
				if (questBase != null)
				{
					MobileParty conspiracyCaravan = ((DisruptSupplyLinesConspiracyQuest)questBase).ConspiracyCaravan;
					if (((conspiracyCaravan != null) ? conspiracyCaravan.Party : null) == party)
					{
						return new ExplainedNumber((float)((DisruptSupplyLinesConspiracyQuest)questBase).CaravanPartySize, false, null);
					}
				}
				if (this.DefeatTheConspiracyQuestBehavior != null && this.DefeatTheConspiracyQuestBehavior.IsMobilePartyCreatedForQuest(party.MobileParty))
				{
					return new ExplainedNumber(600f, false, null);
				}
			}
			return base.BaseModel.GetPartyMemberSizeLimit(party, includeDescriptions);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00019406 File Offset: 0x00017606
		public override ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			return base.BaseModel.GetPartyPrisonerSizeLimit(party, includeDescriptions);
		}

		// Token: 0x04000192 RID: 402
		private DefeatTheConspiracyQuestBehavior _defeatTheConspiracyQuestBehavior;
	}
}
