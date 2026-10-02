using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000DE RID: 222
	public class RecruitmentAgentSpawnBehavior : CampaignBehaviorBase
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0004EDEE File Offset: 0x0004CFEE
		private RecruitmentCampaignBehavior RecruitmentBehavior
		{
			get
			{
				return Campaign.Current.CampaignBehaviorManager.GetBehavior<RecruitmentCampaignBehavior>();
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0004EE00 File Offset: 0x0004D000
		public override void RegisterEvents()
		{
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
			CampaignEvents.MercenaryNumberChangedInTown.AddNonSerializedListener(this, new Action<Town, int, int>(this.OnMercenaryNumberChanged));
			CampaignEvents.MercenaryTroopChangedInTown.AddNonSerializedListener(this, new Action<Town, CharacterObject, CharacterObject>(this.OnMercenaryTroopChanged));
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0004EE52 File Offset: 0x0004D052
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0004EE54 File Offset: 0x0004D054
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			Location locationWithId = settlement.LocationComplex.GetLocationWithId("tavern");
			if (CampaignMission.Current.Location == locationWithId)
			{
				this.AddMercenaryCharacterToTavern(settlement);
			}
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0004EE94 File Offset: 0x0004D094
		private void CheckIfMercenaryCharacterNeedsToRefresh(Settlement settlement, CharacterObject oldTroopType)
		{
			if (settlement.IsTown && settlement == Settlement.CurrentSettlement && PlayerEncounter.LocationEncounter != null && settlement.LocationComplex != null && (CampaignMission.Current == null || GameStateManager.Current.ActiveState != CampaignMission.Current.State))
			{
				if (oldTroopType != null)
				{
					Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("tavern").RemoveAllCharacters((LocationCharacter x) => x.Character.Occupation == oldTroopType.Occupation);
				}
				this.AddMercenaryCharacterToTavern(settlement);
			}
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0004EF1E File Offset: 0x0004D11E
		private void OnMercenaryNumberChanged(Town town, int oldNumber, int newNumber)
		{
			if (this.RecruitmentBehavior != null)
			{
				this.CheckIfMercenaryCharacterNeedsToRefresh(town.Owner.Settlement, this.RecruitmentBehavior.GetMercenaryData(town).TroopType);
			}
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0004EF4A File Offset: 0x0004D14A
		private void OnMercenaryTroopChanged(Town town, CharacterObject oldTroopType, CharacterObject newTroopType)
		{
			this.CheckIfMercenaryCharacterNeedsToRefresh(town.Owner.Settlement, oldTroopType);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0004EF60 File Offset: 0x0004D160
		private void AddMercenaryCharacterToTavern(Settlement settlement)
		{
			if (settlement.LocationComplex != null && settlement.IsTown && this.RecruitmentBehavior != null && this.RecruitmentBehavior.GetMercenaryData(settlement.Town).HasAvailableMercenary(Occupation.NotAssigned))
			{
				Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("tavern");
				if (locationWithId != null)
				{
					locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateMercenary), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
				}
			}
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0004EFD0 File Offset: 0x0004D1D0
		private LocationCharacter CreateMercenary(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject troopType = this.RecruitmentBehavior.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town).TroopType;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(troopType.Race, "_settlement");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(troopType, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).NoHorses(true), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "spawnpoint_mercenary", true, relation, null, false, false, null, false, false, true, null, false);
		}
	}
}
