using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200043F RID: 1087
	public class SallyOutsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060045F0 RID: 17904 RVA: 0x0015BAF5 File Offset: 0x00159CF5
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x0015BB25 File Offset: 0x00159D25
		private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			if (defenderParty.SiegeEvent != null)
			{
				this.CheckForSettlementSallyOut(defenderParty.SiegeEvent.BesiegedSettlement, false);
			}
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x0015BB41 File Offset: 0x00159D41
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x0015BB43 File Offset: 0x00159D43
		public void HourlyTickSettlement(Settlement settlement)
		{
			this.CheckForSettlementSallyOut(settlement, false);
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x0015BB50 File Offset: 0x00159D50
		private void CheckForSettlementSallyOut(Settlement settlement, bool forceForCheck = false)
		{
			if (settlement.IsFortification && settlement.SiegeEvent != null && settlement.Party.MapEvent == null && settlement.Town.GarrisonParty != null && settlement.Town.GarrisonParty.MapEvent == null && ((settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent != null && (settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsSiegeOutside || settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsBlockade)) || MathF.Floor(CampaignTime.Now.ToHours) % 4 == 0) && (Hero.MainHero.CurrentSettlement != settlement || Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(settlement.SiegeEvent, BattleSideEnum.Defender) != Hero.MainHero))
			{
				bool flag;
				this.CheckSallyOut(settlement, false, out flag);
				if (!flag && settlement.HasPort && settlement.SiegeEvent.IsBlockadeActive)
				{
					bool flag2;
					this.CheckSallyOut(settlement, true, out flag2);
				}
			}
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x0015BC74 File Offset: 0x00159E74
		private void CheckSallyOut(Settlement settlement, bool checkForNavalSallyOut, out bool salliedOut)
		{
			salliedOut = false;
			MobileParty leaderParty = settlement.SiegeEvent.BesiegerCamp.LeaderParty;
			bool flag = false;
			bool flag2 = false;
			if (settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent != null)
			{
				flag = settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsSiegeOutside;
				flag2 = settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsBlockade;
			}
			if ((flag2 && !checkForNavalSallyOut) || (flag && checkForNavalSallyOut))
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			float num3;
			if (!checkForNavalSallyOut)
			{
				num3 = settlement.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.SallyOut).Sum<PartyBase>((PartyBase x) => x.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.PlainBattle));
			}
			else
			{
				num3 = settlement.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.BlockadeSallyOutBattle).Sum<PartyBase>((PartyBase x) => x.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.SeaBattle));
			}
			float num4 = num3;
			LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(settlement.SiegeEvent.BesiegerCamp.LeaderParty.Position.ToVec2(), Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius);
			for (MobileParty mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData); mobileParty != null; mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData))
			{
				if (mobileParty.CurrentSettlement == null && mobileParty.Aggressiveness > 0f)
				{
					float num5 = ((mobileParty.Aggressiveness > 0.5f) ? 1f : (mobileParty.Aggressiveness * 2f));
					if (mobileParty.MapFaction.IsAtWarWith(settlement.Party.MapFaction))
					{
						BattleSideEnum battleSideEnum = BattleSideEnum.Defender;
						num += num5 * (checkForNavalSallyOut ? mobileParty.Party.GetCustomStrength(battleSideEnum, MapEvent.PowerCalculationContext.SeaBattle) : mobileParty.Party.GetCustomStrength(battleSideEnum, MapEvent.PowerCalculationContext.PlainBattle));
					}
					else if (mobileParty.MapFaction == settlement.MapFaction && checkForNavalSallyOut == mobileParty.IsCurrentlyAtSea)
					{
						BattleSideEnum battleSideEnum2 = BattleSideEnum.Attacker;
						num2 += num5 * (checkForNavalSallyOut ? mobileParty.Party.GetCustomStrength(battleSideEnum2, MapEvent.PowerCalculationContext.SeaBattle) : mobileParty.Party.GetCustomStrength(battleSideEnum2, MapEvent.PowerCalculationContext.PlainBattle));
					}
				}
			}
			float num6 = num4 + num2;
			float num7 = ((flag || flag2) ? 1.5f : 2f);
			if (num6 > num * num7)
			{
				if (flag || flag2)
				{
					using (IEnumerator<PartyBase> enumerator = settlement.GetInvolvedPartiesForEventType(checkForNavalSallyOut ? MapEvent.BattleTypes.BlockadeSallyOutBattle : MapEvent.BattleTypes.SallyOut).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PartyBase partyBase = enumerator.Current;
							if (partyBase.IsMobile && partyBase.NumberOfHealthyMembers > 0 && !partyBase.MobileParty.IsMainParty && partyBase.MapEventSide == null)
							{
								if (settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent == null)
								{
									break;
								}
								partyBase.MapEventSide = settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent.AttackerSide;
							}
						}
						goto IL_02F6;
					}
				}
				if (checkForNavalSallyOut)
				{
					settlement.Town.GarrisonParty.SetTargetSettlement(settlement, true);
				}
				EncounterManager.StartPartyEncounter(settlement.Town.GarrisonParty.Party, leaderParty.Party);
				IL_02F6:
				salliedOut = true;
			}
		}

		// Token: 0x040013A6 RID: 5030
		private const int SallyOutCheckPeriodInHours = 4;

		// Token: 0x040013A7 RID: 5031
		private const float SallyOutPowerRatioForHelpingReliefForce = 1.5f;

		// Token: 0x040013A8 RID: 5032
		private const float SallyOutPowerRatio = 2f;
	}
}
