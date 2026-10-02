using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000434 RID: 1076
	public class PoliticalStagnationAndBorderIncidentCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004509 RID: 17673 RVA: 0x00152774 File Offset: 0x00150974
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.NewGameCreated));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.LoadFinished));
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x001527DD File Offset: 0x001509DD
		private void LoadFinished()
		{
			if (this._lastUpdateTimePerSettlement == null)
			{
				this.InitializeDictionary();
			}
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x001527ED File Offset: 0x001509ED
		private void NewGameCreated(CampaignGameStarter obj)
		{
			this.InitializeDictionary();
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x001527F8 File Offset: 0x001509F8
		private void InitializeDictionary()
		{
			this._lastUpdateTimePerSettlement = new Dictionary<Settlement, CampaignTime>();
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsFortification || settlement.IsVillage)
				{
					this._lastUpdateTimePerSettlement.Add(settlement, CampaignTime.Now - CampaignTime.Hours(MBRandom.RandomFloat * 3f));
				}
			}
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x00152884 File Offset: 0x00150A84
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Settlement, CampaignTime>>("_lastUpdateTimePerSettlement", ref this._lastUpdateTimePerSettlement);
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x00152898 File Offset: 0x00150A98
		public void HourlyTickSettlement(Settlement settlement)
		{
			if (this._lastUpdateTimePerSettlement.ContainsKey(settlement) && this._lastUpdateTimePerSettlement[settlement].ElapsedHoursUntilNow > 3f)
			{
				this.UpdateNearbyValues(settlement, false);
				settlement.NearbyLandThreatIntensity *= 0.85f;
				settlement.NearbyLandAllyIntensity *= 0.8f;
				if (settlement.HasPort)
				{
					this.UpdateNearbyValues(settlement, true);
					settlement.NearbyNavalThreatIntensity *= 0.85f;
					settlement.NearbyNavalAllyIntensity *= 0.8f;
				}
				this._lastUpdateTimePerSettlement[settlement] = CampaignTime.Now;
			}
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x00152944 File Offset: 0x00150B44
		private void UpdateNearbyValues(Settlement settlement, bool isCheckingNavalValues)
		{
			float settlementNearbyThreatAndAllyCheckRadius = Campaign.Current.Models.MobilePartyAIModel.GetSettlementNearbyThreatAndAllyCheckRadius(settlement, isCheckingNavalValues);
			LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition((isCheckingNavalValues ? settlement.PortPosition : settlement.GatePosition).ToVec2(), settlementNearbyThreatAndAllyCheckRadius);
			for (MobileParty mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData); mobileParty != null; mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData))
			{
				if (!mobileParty.IsGarrison && !mobileParty.IsMilitia && mobileParty.IsActive)
				{
					if (mobileParty.Ai.IsAlerted && mobileParty.MapFaction == settlement.MapFaction && (mobileParty.IsCaravan || mobileParty.IsVillager))
					{
						if (mobileParty.IsCurrentlyAtSea && isCheckingNavalValues)
						{
							settlement.NearbyNavalThreatIntensity += 0.6f;
						}
						else if (!isCheckingNavalValues)
						{
							settlement.NearbyLandThreatIntensity += 0.6f;
						}
					}
					if (mobileParty.Aggressiveness > 0f)
					{
						if (mobileParty.CurrentSettlement == null && mobileParty.Army == null && (mobileParty.IsBandit || FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, settlement.MapFaction)))
						{
							float threatValueOfEnemyToSettlement = this.GetThreatValueOfEnemyToSettlement(mobileParty, settlement);
							if (mobileParty.IsCurrentlyAtSea && isCheckingNavalValues)
							{
								settlement.NearbyNavalThreatIntensity += threatValueOfEnemyToSettlement * 3f;
							}
							else if (!isCheckingNavalValues)
							{
								settlement.NearbyLandThreatIntensity += threatValueOfEnemyToSettlement * 3f;
							}
						}
						else if (mobileParty.MapFaction == settlement.MapFaction)
						{
							bool flag = mobileParty.DefaultBehavior == AiBehavior.PatrolAroundPoint && !mobileParty.TargetPosition.IsOnLand;
							float num = this.GetThreatValueOfParty(mobileParty);
							if (flag)
							{
								num *= 0.5f;
							}
							if ((mobileParty.IsCurrentlyAtSea || flag) && isCheckingNavalValues)
							{
								settlement.NearbyNavalAllyIntensity += num * 3f;
							}
							else if (!isCheckingNavalValues)
							{
								settlement.NearbyLandAllyIntensity += num * 3f;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x00152B26 File Offset: 0x00150D26
		private float GetThreatValueOfParty(MobileParty mobileParty)
		{
			return MathF.Min(1f, mobileParty.Party.EstimatedStrength / 400f * MathF.Min(1f, mobileParty.Aggressiveness));
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x00152B54 File Offset: 0x00150D54
		private float GetThreatValueOfEnemyToSettlement(MobileParty mobileParty, Settlement settlement)
		{
			float num = this.GetThreatValueOfParty(mobileParty);
			if (mobileParty == MobileParty.MainParty)
			{
				num *= 2f;
			}
			if (!mobileParty.IsLordParty)
			{
				num *= 0.5f;
			}
			if (mobileParty.DefaultBehavior == AiBehavior.PatrolAroundPoint && mobileParty.TargetSettlement == settlement)
			{
				num *= 2f;
			}
			if (mobileParty.MapEvent != null && mobileParty.MapEvent.IsFieldBattle)
			{
				num = 3f * num;
			}
			return num;
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x00152BC4 File Offset: 0x00150DC4
		public void DailyTick()
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				PoliticalStagnationAndBorderIncidentCampaignBehavior.UpdatePoliticallyStagnation(kingdom);
			}
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x00152C14 File Offset: 0x00150E14
		private static void UpdatePoliticallyStagnation(Kingdom kingdom)
		{
			float num = 1f + (float)MathF.Min(60, kingdom.Fiefs.Count) * 0.2f;
			float num2 = 2f + (float)MathF.Min(60, kingdom.Fiefs.Count) * 0.6f;
			int num3 = 1;
			foreach (Kingdom kingdom2 in Kingdom.All)
			{
				if (FactionManager.IsAtWarAgainstFaction(kingdom, kingdom2))
				{
					if ((float)kingdom2.Fiefs.Count >= num2)
					{
						num3 = -2;
						break;
					}
					if ((float)kingdom2.Fiefs.Count >= num)
					{
						num3 = -1;
					}
				}
			}
			kingdom.PoliticalStagnation += num3;
			if (kingdom.PoliticalStagnation < 0)
			{
				kingdom.PoliticalStagnation = 0;
				return;
			}
			if (kingdom.PoliticalStagnation > 300)
			{
				kingdom.PoliticalStagnation = 300;
			}
		}

		// Token: 0x04001377 RID: 4983
		private const float ThreatUpdateTimeThresholdInHours = 3f;

		// Token: 0x04001378 RID: 4984
		private Dictionary<Settlement, CampaignTime> _lastUpdateTimePerSettlement;
	}
}
