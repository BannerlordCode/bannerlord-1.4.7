using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000433 RID: 1075
	public class PlayerVariablesBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004503 RID: 17667 RVA: 0x001526C8 File Offset: 0x001508C8
		public override void RegisterEvents()
		{
			CampaignEvents.PlayerDesertedBattleEvent.AddNonSerializedListener(this, new Action<int>(this.OnPlayerDesertedBattle));
			CampaignEvents.VillageLooted.AddNonSerializedListener(this, new Action<Village>(this.OnVillageLooted));
			CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, new Action<MapEvent>(this.OnPlayerBattleEnd));
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x0015271A File Offset: 0x0015091A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x0015271C File Offset: 0x0015091C
		private void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
			SkillLevelingManager.OnTacticsUsed(MobileParty.MainParty, (float)(sacrificedMenCount * 50));
			TraitLevelingHelper.OnTroopsSacrificed();
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x00152732 File Offset: 0x00150932
		private void OnVillageLooted(Village village)
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.PlayerIsAttacker && PlayerEncounter.EncounterSettlement != null && PlayerEncounter.EncounterSettlement.Village == village)
			{
				TraitLevelingHelper.OnVillageRaided();
			}
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x0015275B File Offset: 0x0015095B
		private void OnPlayerBattleEnd(MapEvent mapEvent)
		{
			TraitLevelingHelper.OnBattleWon(mapEvent, mapEvent.GetPlayerBattleContributionRate());
		}
	}
}
