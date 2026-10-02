using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200041C RID: 1052
	public class NotableHelperCharacterCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600434C RID: 17228 RVA: 0x00146100 File Offset: 0x00144300
		public override void RegisterEvents()
		{
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
			CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionEnded));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
		}

		// Token: 0x0600434D RID: 17229 RVA: 0x00146152 File Offset: 0x00144352
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x00146154 File Offset: 0x00144354
		private void OnMissionEnded(IMission mission)
		{
			if (LocationComplex.Current != null && PlayerEncounter.LocationEncounter != null && Settlement.CurrentSettlement != null && !Hero.MainHero.IsPrisoner && !Settlement.CurrentSettlement.IsUnderSiege)
			{
				this._addNotableHelperCharacters = true;
			}
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x0014618A File Offset: 0x0014438A
		private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (LocationComplex.Current != null && PlayerEncounter.LocationEncounter != null && mobileParty != null && mobileParty == MobileParty.MainParty)
			{
				this._addNotableHelperCharacters = true;
			}
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x001461AC File Offset: 0x001443AC
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			Location locationWithId = LocationComplex.Current.GetLocationWithId("center");
			Location locationWithId2 = LocationComplex.Current.GetLocationWithId("village_center");
			if (this._addNotableHelperCharacters && (CampaignMission.Current.Location == locationWithId || CampaignMission.Current.Location == locationWithId2))
			{
				this.SpawnNotableHelperCharacters(settlement);
				this._addNotableHelperCharacters = false;
			}
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x00146214 File Offset: 0x00144414
		private void SpawnNotableHelperCharacters(Settlement settlement)
		{
			int num = settlement.Notables.Count<Hero>((Hero x) => x.IsGangLeader);
			int num2 = settlement.Notables.Count<Hero>((Hero x) => x.IsPreacher);
			int num3 = settlement.Notables.Count<Hero>((Hero x) => x.IsArtisan);
			int num4 = settlement.Notables.Count<Hero>((Hero x) => x.IsRuralNotable || x.IsHeadman);
			int num5 = settlement.Notables.Count<Hero>((Hero x) => x.IsMerchant);
			this.SpawnNotableHelperCharacter(settlement.Culture.GangleaderBodyguard, "_gangleader_bodyguard", "sp_gangleader_bodyguard", num * 2);
			this.SpawnNotableHelperCharacter(settlement.Culture.PreacherNotary, "_merchant_notary", "sp_preacher_notary", num2);
			this.SpawnNotableHelperCharacter(settlement.Culture.ArtisanNotary, "_merchant_notary", "sp_artisan_notary", num3);
			this.SpawnNotableHelperCharacter(settlement.Culture.RuralNotableNotary, "_merchant_notary", "sp_rural_notable_notary", num4);
			this.SpawnNotableHelperCharacter(settlement.Culture.MerchantNotary, "_merchant_notary", "sp_merchant_notary", num5);
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x00146388 File Offset: 0x00144588
		private void SpawnNotableHelperCharacter(CharacterObject character, string actionSetSuffix, string tag, int characterToSpawnCount)
		{
			Location location = LocationComplex.Current.GetLocationWithId("center") ?? LocationComplex.Current.GetLocationWithId("village_center");
			while (characterToSpawnCount > 0)
			{
				Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(character.Race, "_settlement");
				int num;
				int num2;
				Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(character, out num, out num2, "Notary");
				AgentData agentData = new AgentData(new SimpleAgentOrigin(character, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).NoHorses(true).Age(MBRandom.RandomInt(num, num2));
				LocationCharacter locationCharacter = new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), tag, true, LocationCharacter.CharacterRelations.Neutral, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, actionSetSuffix), true, false, null, false, false, true, null, false);
				location.AddCharacter(locationCharacter);
				characterToSpawnCount--;
			}
		}

		// Token: 0x04001340 RID: 4928
		private bool _addNotableHelperCharacters;
	}
}
