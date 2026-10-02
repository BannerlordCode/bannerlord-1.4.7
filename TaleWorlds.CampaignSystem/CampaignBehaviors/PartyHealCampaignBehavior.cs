using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000428 RID: 1064
	public class PartyHealCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060043C0 RID: 17344 RVA: 0x00149768 File Offset: 0x00147968
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnClanHourlyTick));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnQuarterDailyPartyTick.AddNonSerializedListener(this, new Action<MobileParty>(this.OnQuarterDailyPartyTick));
			CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, new Action<MapEvent>(this.OnPlayerBattleEnd));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnDailyTickSettlement));
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x00149818 File Offset: 0x00147A18
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._overflowedHealingForRegulars.ContainsKey(mobileParty.Party))
			{
				this._overflowedHealingForRegulars.Remove(mobileParty.Party);
				if (this._overflowedHealingForHeroes.ContainsKey(mobileParty.Party))
				{
					this._overflowedHealingForHeroes.Remove(mobileParty.Party);
				}
				if (this._overflowedHealingForPrisonerRegulars.ContainsKey(mobileParty.Party))
				{
					this._overflowedHealingForPrisonerRegulars.Remove(mobileParty.Party);
				}
				if (this._overflowedHealingForPrisonerHeroes.ContainsKey(mobileParty.Party))
				{
					this._overflowedHealingForPrisonerHeroes.Remove(mobileParty.Party);
				}
			}
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x001498BC File Offset: 0x00147ABC
		public void OnMapEventEnded(MapEvent mapEvent)
		{
			if (!mapEvent.IsPlayerMapEvent)
			{
				this.OnBattleEndCheckPerkEffects(mapEvent);
			}
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x001498CD File Offset: 0x00147ACD
		private void OnPlayerBattleEnd(MapEvent mapEvent)
		{
			this.OnBattleEndCheckPerkEffects(mapEvent);
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x001498D8 File Offset: 0x00147AD8
		private void OnBattleEndCheckPerkEffects(MapEvent mapEvent)
		{
			if (mapEvent.HasWinner)
			{
				foreach (PartyBase partyBase in mapEvent.InvolvedParties)
				{
					if (partyBase.MemberRoster.TotalHeroes > 0)
					{
						foreach (TroopRosterElement troopRosterElement in partyBase.MemberRoster.GetTroopRoster())
						{
							if (troopRosterElement.Character.IsHero)
							{
								Hero heroObject = troopRosterElement.Character.HeroObject;
								int roundedResultNumber = Campaign.Current.Models.PartyHealingModel.GetBattleEndHealingAmount(partyBase, heroObject).RoundedResultNumber;
								if (roundedResultNumber > 0)
								{
									heroObject.Heal(roundedResultNumber, false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x001499C8 File Offset: 0x00147BC8
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<PartyBase, float>>("_overflowedHealingForRegulars", ref this._overflowedHealingForRegulars);
			dataStore.SyncData<Dictionary<PartyBase, float>>("_overflowedHealingForHeroes", ref this._overflowedHealingForHeroes);
			dataStore.SyncData<Dictionary<PartyBase, float>>("_overflowedHealingForPrisonerRegulars", ref this._overflowedHealingForPrisonerRegulars);
			dataStore.SyncData<Dictionary<PartyBase, float>>("_overflowedHealingForPrisonerHeroes", ref this._overflowedHealingForPrisonerHeroes);
		}

		// Token: 0x060043C6 RID: 17350 RVA: 0x00149A1D File Offset: 0x00147C1D
		private void OnHourlyTick()
		{
			this.TryHealOrWoundParty(MobileParty.MainParty.Party, (float)CampaignTime.HoursInDay);
		}

		// Token: 0x060043C7 RID: 17351 RVA: 0x00149A38 File Offset: 0x00147C38
		private void OnClanHourlyTick(Clan clan)
		{
			if (!clan.IsBanditFaction)
			{
				foreach (Hero hero in clan.Heroes)
				{
					float num = 0f;
					bool flag = hero.PartyBelongedTo == null && hero.PartyBelongedToAsPrisoner == null;
					bool flag2 = hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.NotSpawned || hero.HeroState == Hero.CharacterStates.Disabled;
					if (flag && !flag2)
					{
						num = Campaign.Current.Models.PartyHealingModel.GetDailyHealingHpForHeroes(null, false, false).ResultNumber / (float)CampaignTime.HoursInDay;
					}
					int num2 = MBRandom.RoundRandomized(num);
					if (!hero.IsHealthFull())
					{
						int num3 = MathF.Min(num2, hero.MaxHitPoints - hero.HitPoints);
						hero.HitPoints += num3;
					}
				}
			}
		}

		// Token: 0x060043C8 RID: 17352 RVA: 0x00149B38 File Offset: 0x00147D38
		private void OnQuarterDailyPartyTick(MobileParty mobileParty)
		{
			if (!mobileParty.IsMainParty)
			{
				this.TryHealOrWoundParty(mobileParty.Party, 4f);
			}
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x00149B53 File Offset: 0x00147D53
		private void OnDailyTickSettlement(Settlement settlement)
		{
			this.TryHealOrWoundParty(settlement.Party, 1f);
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x00149B66 File Offset: 0x00147D66
		private void TryHealOrWoundParty(PartyBase partyBase, float healFrequencyPerDay)
		{
			if (partyBase.IsActive && partyBase.MapEvent == null)
			{
				this.TryToHealOrWoundMembers(partyBase, healFrequencyPerDay);
				this.TryToHealOrWoundPrisoners(partyBase, healFrequencyPerDay);
			}
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x00149B88 File Offset: 0x00147D88
		private void TryToHealOrWoundPrisoners(PartyBase partyBase, float healFrequencyPerDay)
		{
			float num;
			if (!this._overflowedHealingForPrisonerHeroes.TryGetValue(partyBase, out num))
			{
				this._overflowedHealingForPrisonerHeroes.Add(partyBase, 0f);
			}
			float num2;
			if (!this._overflowedHealingForPrisonerRegulars.TryGetValue(partyBase, out num2))
			{
				this._overflowedHealingForPrisonerRegulars.Add(partyBase, 0f);
			}
			float num3 = Campaign.Current.Models.PartyHealingModel.GetDailyHealingHpForHeroes(partyBase, true, false).ResultNumber / healFrequencyPerDay;
			float num4 = Campaign.Current.Models.PartyHealingModel.GetDailyHealingForRegulars(partyBase, true, false).ResultNumber / healFrequencyPerDay;
			num += num3;
			num2 += num4;
			if ((int)num != 0)
			{
				this.ManageHealingOfPrisonerHeroes(partyBase, ref num);
			}
			if ((int)num2 != 0)
			{
				this.ManageHealingOfPrisonerRegulars(partyBase, ref num2);
			}
			this._overflowedHealingForPrisonerHeroes[partyBase] = num;
			this._overflowedHealingForPrisonerRegulars[partyBase] = num2;
		}

		// Token: 0x060043CC RID: 17356 RVA: 0x00149C5C File Offset: 0x00147E5C
		private void TryToHealOrWoundMembers(PartyBase partyBase, float healFrequencyPerDay)
		{
			float num;
			if (!this._overflowedHealingForHeroes.TryGetValue(partyBase, out num))
			{
				this._overflowedHealingForHeroes.Add(partyBase, 0f);
			}
			float num2;
			if (!this._overflowedHealingForRegulars.TryGetValue(partyBase, out num2))
			{
				this._overflowedHealingForRegulars.Add(partyBase, 0f);
			}
			float num3 = partyBase.HealingRateForMemberHeroes / healFrequencyPerDay;
			float num4 = partyBase.HealingRateForMemberRegulars / healFrequencyPerDay;
			num += num3;
			num2 += num4;
			if (num >= 1f)
			{
				PartyHealCampaignBehavior.HealMemberHeroes(partyBase, ref num);
			}
			else if (num <= -1f)
			{
				PartyHealCampaignBehavior.ReduceHpMemberHeroes(partyBase, ref num);
			}
			if (num2 >= 1f)
			{
				PartyHealCampaignBehavior.HealMemberRegulars(partyBase, ref num2);
			}
			else if (num2 <= -1f)
			{
				PartyHealCampaignBehavior.ReduceHpMemberRegulars(partyBase, ref num2);
			}
			this._overflowedHealingForHeroes[partyBase] = num;
			this._overflowedHealingForRegulars[partyBase] = num2;
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x00149D24 File Offset: 0x00147F24
		private void ManageHealingOfPrisonerRegulars(PartyBase partyBase, ref float prisonerRegularsHealingValue)
		{
			TroopRoster prisonRoster = partyBase.PrisonRoster;
			if (prisonRoster.TotalWoundedRegulars == 0)
			{
				prisonerRegularsHealingValue = 0f;
				return;
			}
			int num = MathF.Floor(prisonerRegularsHealingValue);
			prisonerRegularsHealingValue -= (float)num;
			int num2 = MBRandom.RandomInt(prisonRoster.Count);
			int num3 = 0;
			while (num3 < prisonRoster.Count && num > 0)
			{
				int num4 = (num2 + num3) % prisonRoster.Count;
				if (prisonRoster.GetCharacterAtIndex(num4).IsRegular && prisonRoster.GetElementWoundedNumber(num4) > 0)
				{
					int num5 = MathF.Min(num, prisonRoster.GetElementWoundedNumber(num4));
					if (num5 > 0)
					{
						prisonRoster.AddToCountsAtIndex(num4, 0, -num5, 0, true);
						num -= num5;
					}
				}
				num3++;
			}
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x00149DC8 File Offset: 0x00147FC8
		private void ManageHealingOfPrisonerHeroes(PartyBase partyBase, ref float prisonerHeroesHealingValue)
		{
			int num = MathF.Floor(prisonerHeroesHealingValue);
			prisonerHeroesHealingValue -= (float)num;
			TroopRoster prisonRoster = partyBase.PrisonRoster;
			if (prisonRoster.TotalHeroes > 0)
			{
				for (int i = 0; i < prisonRoster.Count; i++)
				{
					Hero heroObject = prisonRoster.GetCharacterAtIndex(i).HeroObject;
					if (heroObject != null && heroObject.HitPoints < heroObject.WoundedHealthLimit)
					{
						int num2 = Math.Min(num, heroObject.WoundedHealthLimit - heroObject.HitPoints);
						heroObject.Heal(num2, false);
					}
				}
			}
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x00149E44 File Offset: 0x00148044
		private static void HealMemberHeroes(PartyBase partyBase, ref float heroesHealingValue)
		{
			int num = MathF.Floor(heroesHealingValue);
			heroesHealingValue -= (float)num;
			TroopRoster memberRoster = partyBase.MemberRoster;
			if (memberRoster.TotalHeroes > 0)
			{
				for (int i = 0; i < memberRoster.Count; i++)
				{
					Hero heroObject = memberRoster.GetCharacterAtIndex(i).HeroObject;
					if (heroObject != null && !heroObject.IsHealthFull())
					{
						heroObject.Heal(num, true);
					}
				}
			}
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x00149EA4 File Offset: 0x001480A4
		private static void ReduceHpMemberHeroes(PartyBase partyBase, ref float heroesHealingValue)
		{
			int num = MathF.Ceiling(heroesHealingValue);
			heroesHealingValue = -(-heroesHealingValue % 1f);
			for (int i = 0; i < partyBase.MemberRoster.Count; i++)
			{
				Hero heroObject = partyBase.MemberRoster.GetCharacterAtIndex(i).HeroObject;
				if (heroObject != null && heroObject.HitPoints > 0)
				{
					int num2 = MathF.Min(num, heroObject.HitPoints);
					heroObject.HitPoints += num2;
				}
			}
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x00149F14 File Offset: 0x00148114
		private static void HealMemberRegulars(PartyBase partyBase, ref float regularsHealingValue)
		{
			TroopRoster memberRoster = partyBase.MemberRoster;
			if (memberRoster.TotalWoundedRegulars == 0)
			{
				regularsHealingValue = 0f;
				return;
			}
			int num = MathF.Floor(regularsHealingValue);
			regularsHealingValue -= (float)num;
			int num2 = 0;
			float num3 = 0f;
			int num4 = MBRandom.RandomInt(memberRoster.Count);
			int num5 = 0;
			while (num5 < memberRoster.Count && num > 0)
			{
				int num6 = (num4 + num5) % memberRoster.Count;
				CharacterObject characterAtIndex = memberRoster.GetCharacterAtIndex(num6);
				if (characterAtIndex.IsRegular)
				{
					int num7 = MathF.Min(num, memberRoster.GetElementWoundedNumber(num6));
					if (num7 > 0)
					{
						memberRoster.AddToCountsAtIndex(num6, 0, -num7, 0, true);
						num -= num7;
						num2 += num7;
						num3 += (float)(characterAtIndex.Tier * num7);
					}
				}
				num5++;
			}
			if (num2 > 0)
			{
				SkillLevelingManager.OnRegularTroopHealedWhileWaiting(partyBase.MobileParty, num2, num3 / (float)num2);
			}
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x00149FE8 File Offset: 0x001481E8
		private static void ReduceHpMemberRegulars(PartyBase partyBase, ref float regularsHealingValue)
		{
			TroopRoster memberRoster = partyBase.MemberRoster;
			if (memberRoster.TotalRegulars - memberRoster.TotalWoundedRegulars == 0)
			{
				regularsHealingValue = 0f;
				return;
			}
			int num = MathF.Floor(-regularsHealingValue);
			regularsHealingValue = -(-regularsHealingValue % 1f);
			int num2 = MBRandom.RandomInt(memberRoster.Count);
			int num3 = 0;
			while (num3 < memberRoster.Count && num > 0)
			{
				int num4 = (num2 + num3) % memberRoster.Count;
				if (memberRoster.GetCharacterAtIndex(num4).IsRegular)
				{
					int num5 = MathF.Min(memberRoster.GetElementNumber(num4) - memberRoster.GetElementWoundedNumber(num4), num);
					if (num5 > 0)
					{
						memberRoster.AddToCountsAtIndex(num4, 0, num5, 0, true);
						num -= num5;
					}
				}
				num3++;
			}
		}

		// Token: 0x04001350 RID: 4944
		private Dictionary<PartyBase, float> _overflowedHealingForRegulars = new Dictionary<PartyBase, float>();

		// Token: 0x04001351 RID: 4945
		private Dictionary<PartyBase, float> _overflowedHealingForHeroes = new Dictionary<PartyBase, float>();

		// Token: 0x04001352 RID: 4946
		private Dictionary<PartyBase, float> _overflowedHealingForPrisonerRegulars = new Dictionary<PartyBase, float>();

		// Token: 0x04001353 RID: 4947
		private Dictionary<PartyBase, float> _overflowedHealingForPrisonerHeroes = new Dictionary<PartyBase, float>();
	}
}
