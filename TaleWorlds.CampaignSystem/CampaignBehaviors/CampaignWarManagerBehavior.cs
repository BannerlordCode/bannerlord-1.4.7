using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003D9 RID: 985
	public class CampaignWarManagerBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003B14 RID: 15124 RVA: 0x000F5746 File Offset: 0x000F3946
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventEnded));
			CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, RaidEventComponent>(this.OnRaidCompleted));
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x000F5778 File Offset: 0x000F3978
		private void OnRaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
			if (raidEvent.AttackerSide.LeaderParty.MapFaction != null && !raidEvent.AttackerSide.LeaderParty.MapFaction.IsBanditFaction && raidEvent.DefenderSide.LeaderParty.MapFaction != null && !raidEvent.DefenderSide.LeaderParty.MapFaction.IsBanditFaction)
			{
				IFaction mapFaction = raidEvent.AttackerSide.MapFaction;
				IFaction mapFaction2 = raidEvent.DefenderSide.MapFaction;
				if (mapFaction.MapFaction != mapFaction2.MapFaction)
				{
					StanceLink stanceWith = mapFaction.GetStanceWith(mapFaction2);
					if (raidEvent.MapEventSettlement != null && raidEvent.BattleState == BattleState.AttackerVictory && raidEvent.MapEventSettlement.IsVillage && raidEvent.MapEventSettlement.Village.VillageState == Village.VillageStates.Looted)
					{
						int num;
						if (mapFaction == stanceWith.Faction1)
						{
							StanceLink stanceLink = stanceWith;
							num = stanceLink.SuccessfulRaids1;
							stanceLink.SuccessfulRaids1 = num + 1;
							return;
						}
						StanceLink stanceLink2 = stanceWith;
						num = stanceLink2.SuccessfulRaids2;
						stanceLink2.SuccessfulRaids2 = num + 1;
					}
				}
			}
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x000F586C File Offset: 0x000F3A6C
		private void MapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.AttackerSide.LeaderParty.MapFaction != null && !mapEvent.AttackerSide.LeaderParty.MapFaction.IsBanditFaction && mapEvent.DefenderSide.LeaderParty.MapFaction != null && !mapEvent.DefenderSide.LeaderParty.MapFaction.IsBanditFaction)
			{
				IFaction mapFaction = mapEvent.AttackerSide.MapFaction;
				IFaction mapFaction2 = mapEvent.DefenderSide.MapFaction;
				if (mapFaction.MapFaction != mapFaction2.MapFaction)
				{
					StanceLink stanceWith = mapFaction.GetStanceWith(mapFaction2);
					stanceWith.TroopCasualties1 += ((stanceWith.Faction1 == mapFaction) ? mapEvent.AttackerSide.TroopCasualties : mapEvent.DefenderSide.TroopCasualties);
					stanceWith.TroopCasualties2 += ((stanceWith.Faction2 == mapFaction) ? mapEvent.AttackerSide.TroopCasualties : mapEvent.DefenderSide.TroopCasualties);
					stanceWith.ShipCasualties1 += ((stanceWith.Faction1 == mapFaction) ? mapEvent.AttackerSide.ShipCasualties : mapEvent.DefenderSide.ShipCasualties);
					stanceWith.ShipCasualties2 += ((stanceWith.Faction2 == mapFaction) ? mapEvent.AttackerSide.ShipCasualties : mapEvent.DefenderSide.ShipCasualties);
					if (mapEvent.MapEventSettlement != null && mapEvent.BattleState == BattleState.AttackerVictory && mapEvent.MapEventSettlement.IsFortification && mapEvent.EventType == MapEvent.BattleTypes.Siege)
					{
						if (mapFaction == stanceWith.Faction1)
						{
							StanceLink stanceLink = stanceWith;
							int num = stanceLink.SuccessfulSieges1;
							stanceLink.SuccessfulSieges1 = num + 1;
							if (mapEvent.MapEventSettlement.IsTown)
							{
								StanceLink stanceLink2 = stanceWith;
								num = stanceLink2.SuccessfulTownSieges1;
								stanceLink2.SuccessfulTownSieges1 = num + 1;
								return;
							}
						}
						else
						{
							StanceLink stanceLink3 = stanceWith;
							int num = stanceLink3.SuccessfulSieges2;
							stanceLink3.SuccessfulSieges2 = num + 1;
							if (mapEvent.MapEventSettlement.IsTown)
							{
								StanceLink stanceLink4 = stanceWith;
								num = stanceLink4.SuccessfulTownSieges2;
								stanceLink4.SuccessfulTownSieges2 = num + 1;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x000F5A4E File Offset: 0x000F3C4E
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
