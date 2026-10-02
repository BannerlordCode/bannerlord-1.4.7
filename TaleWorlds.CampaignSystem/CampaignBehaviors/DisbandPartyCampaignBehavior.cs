using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E8 RID: 1000
	public class DisbandPartyCampaignBehavior : CampaignBehaviorBase, IDisbandPartyCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x06003E01 RID: 15873 RVA: 0x0010EEC0 File Offset: 0x0010D0C0
		public override void RegisterEvents()
		{
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.OnPartyDisbandStartedEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyDisbandStarted));
			CampaignEvents.OnPartyDisbandCanceledEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyDisbandCanceled));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.OnHeroTeleportationRequestedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail>(this.OnHeroTeleportationRequested));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
			CampaignEvents.OnPartyDisbandedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnPartyDisbanded));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x0010EFE1 File Offset: 0x0010D1E1
		public bool IsPartyWaitingForDisband(MobileParty party)
		{
			return this._partiesThatWaitingToDisband.ContainsKey(party);
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x0010EFEF File Offset: 0x0010D1EF
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<MobileParty, CampaignTime>>("_partiesThatWaitingToDisband", ref this._partiesThatWaitingToDisband);
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x0010F004 File Offset: 0x0010D204
		private void OnGameLoadFinished()
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				for (int i = kingdom.Armies.Count - 1; i >= 0; i--)
				{
					Army army = kingdom.Armies[i];
					for (int j = army.Parties.Count - 1; j >= 0; j--)
					{
						MobileParty mobileParty = army.Parties[j];
						if (army.LeaderParty != mobileParty && mobileParty.LeaderHero == null)
						{
							DisbandPartyAction.StartDisband(mobileParty);
							mobileParty.Army = null;
						}
					}
					if (army.LeaderParty.LeaderHero == null)
					{
						DisbandPartyAction.StartDisband(army.LeaderParty);
					}
				}
			}
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x0010F0E0 File Offset: 0x0010D2E0
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x0010F0EC File Offset: 0x0010D2EC
		private void OnPartyDisbandStarted(MobileParty party)
		{
			if (party.ActualClan == Clan.PlayerClan || party.MemberRoster.Count < 10)
			{
				if (party.IsCaravan && party.ActualClan == Clan.PlayerClan)
				{
					party.Ai.SetDoNotMakeNewDecisions(true);
					Settlement settlement;
					MobileParty.NavigationType navigationType;
					bool flag;
					this.GetTargetSettlementForDisbandingParty(party, out settlement, out navigationType, out flag);
					if (settlement != null)
					{
						party.SetMoveGoToSettlement(settlement, navigationType, flag);
					}
				}
				if (party.ActualClan == Clan.PlayerClan && party.LeaderHero != null)
				{
					Hero leaderHero = party.LeaderHero;
					party.RemovePartyLeader();
					Debug.FailedAssert("Player Clan's party should not have a leader hero after party disband started!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\DisbandPartyCampaignBehavior.cs", "OnPartyDisbandStarted", 137);
				}
				CampaignTime campaignTime = (party.IsCurrentlyAtSea ? CampaignTime.Never : CampaignTime.DaysFromNow(1f));
				this._partiesThatWaitingToDisband.Add(party, campaignTime);
				return;
			}
			Hero hero = null;
			foreach (Hero hero2 in party.ActualClan.Heroes)
			{
				if (hero2.PartyBelongedTo == null && hero2.IsActive && hero2.DeathMark == KillCharacterAction.KillCharacterActionDetail.None && hero2.CurrentSettlement != null && hero2.GovernorOf == null && (!hero2.CurrentSettlement.IsUnderSiege || !hero2.CurrentSettlement.IsUnderRaid))
				{
					hero = hero2;
					break;
				}
			}
			if (hero != null)
			{
				TeleportHeroAction.ApplyDelayedTeleportToPartyAsPartyLeader(hero, party);
				return;
			}
			this._partiesThatWaitingToDisband.Add(party, CampaignTime.DaysFromNow(1f));
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x0010F26C File Offset: 0x0010D46C
		private void OnPartyDisbandCanceled(MobileParty party)
		{
			if (this._partiesThatWaitingToDisband.ContainsKey(party))
			{
				this._partiesThatWaitingToDisband.Remove(party);
			}
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x0010F28C File Offset: 0x0010D48C
		private void HourlyTick()
		{
			List<MobileParty> list = new List<MobileParty>();
			foreach (KeyValuePair<MobileParty, CampaignTime> keyValuePair in this._partiesThatWaitingToDisband)
			{
				if (keyValuePair.Value.IsPast || (keyValuePair.Value == CampaignTime.Never && (!keyValuePair.Key.IsCurrentlyAtSea || keyValuePair.Key.CurrentSettlement != null)))
				{
					keyValuePair.Key.IsDisbanding = true;
					list.Add(keyValuePair.Key);
				}
			}
			foreach (MobileParty mobileParty in list)
			{
				this._partiesThatWaitingToDisband.Remove(mobileParty);
			}
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x0010F38C File Offset: 0x0010D58C
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._partiesThatWaitingToDisband.ContainsKey(mobileParty))
			{
				this._partiesThatWaitingToDisband.Remove(mobileParty);
			}
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x0010F3A9 File Offset: 0x0010D5A9
		private void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
			if (targetParty != null && detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader && this._partiesThatWaitingToDisband.ContainsKey(targetParty))
			{
				this._partiesThatWaitingToDisband.Remove(targetParty);
			}
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0010F3D0 File Offset: 0x0010D5D0
		private void OnHeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
			if (prisoner == Hero.MainHero)
			{
				foreach (WarPartyComponent warPartyComponent in Clan.PlayerClan.WarPartyComponents)
				{
					if (warPartyComponent.MobileParty != null && warPartyComponent.MobileParty.LeaderHero == null)
					{
						CampaignEventDispatcher.Instance.OnPartyLeaderChangeOfferCanceled(warPartyComponent.MobileParty);
					}
				}
			}
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0010F450 File Offset: 0x0010D650
		private void DailyTickParty(MobileParty mobileParty)
		{
			if (mobileParty.IsDisbanding && mobileParty.MapEvent == null && mobileParty.IsActive)
			{
				this.CheckDisbandedPartyDaily(mobileParty, mobileParty.TargetSettlement);
			}
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x0010F478 File Offset: 0x0010D678
		private void OnSettlementLeft(MobileParty mobileParty, Settlement settlement)
		{
			if (mobileParty.IsCaravan && mobileParty.ActualClan == Clan.PlayerClan && !mobileParty.IsDisbanding && this._partiesThatWaitingToDisband.ContainsKey(mobileParty) && mobileParty.CurrentSettlement == null && mobileParty.TargetSettlement != null)
			{
				Settlement settlement2;
				MobileParty.NavigationType navigationType;
				bool flag;
				this.GetTargetSettlementForDisbandingParty(mobileParty, out settlement2, out navigationType, out flag);
				if (settlement2 != null)
				{
					mobileParty.SetMoveGoToSettlement(settlement2, navigationType, flag);
				}
			}
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x0010F4DC File Offset: 0x0010D6DC
		private void HourlyTickParty(MobileParty party)
		{
			if (this._partiesThatWaitingToDisband.ContainsKey(party) && !party.IsCurrentlyAtSea && this._partiesThatWaitingToDisband[party] == CampaignTime.Never)
			{
				this._partiesThatWaitingToDisband[party] = CampaignTime.DaysFromNow(1f);
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new PartyLeaderChangeNotification(party, new TextObject("{=QSaufZ9i}One of your parties has lost its leader. It will disband after a day has passed. You can assign a new clan member to lead it, if you wish to keep the party.", null)));
			}
			if (party.DefaultBehavior == AiBehavior.Hold && party.Ai.DoNotMakeNewDecisions && (party.IsDisbanding || this._partiesThatWaitingToDisband.ContainsKey(party)))
			{
				Settlement settlement;
				MobileParty.NavigationType navigationType;
				bool flag;
				this.GetTargetSettlementForDisbandingParty(party, out settlement, out navigationType, out flag);
				if (settlement != null)
				{
					party.SetMoveGoToSettlement(settlement, navigationType, flag);
				}
			}
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0010F594 File Offset: 0x0010D794
		private void GetTargetSettlementForDisbandingParty(MobileParty mobileParty, out Settlement targetSettlement, out MobileParty.NavigationType bestNavigationType, out bool isTargetingPort)
		{
			float num = 0f;
			targetSettlement = null;
			bestNavigationType = MobileParty.NavigationType.None;
			isTargetingPort = false;
			foreach (Settlement settlement in mobileParty.MapFaction.Settlements)
			{
				if (settlement.IsFortification)
				{
					if (settlement == mobileParty.CurrentSettlement)
					{
						bestNavigationType = mobileParty.NavigationCapability;
						isTargetingPort = !mobileParty.HasLandNavigationCapability;
						targetSettlement = settlement;
						break;
					}
					MobileParty.NavigationType navigationType;
					float num2;
					bool flag;
					this.CalculateTargetSettlementScore(mobileParty, settlement, out navigationType, out num2, out flag);
					if (num2 > num)
					{
						targetSettlement = settlement;
						num = num2;
						bestNavigationType = navigationType;
						isTargetingPort = flag;
					}
				}
			}
			if (targetSettlement == null)
			{
				float num3 = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(mobileParty.IsCurrentlyAtSea ? MobileParty.NavigationType.Naval : MobileParty.NavigationType.Default) * 2f;
				int num4 = -1;
				Func<Settlement, bool> <>9__1;
				do
				{
					MobileParty mobileParty2 = mobileParty;
					MobileParty.NavigationType navigationCapability = mobileParty.NavigationCapability;
					float num5 = num3;
					int num6 = num4;
					Func<Settlement, bool> func;
					if ((func = <>9__1) == null)
					{
						func = (<>9__1 = (Settlement s) => s.OwnerClan != null && !s.OwnerClan.IsAtWarWith(mobileParty.MapFaction) && s.IsFortification);
					}
					num4 = SettlementHelper.FindNextSettlementAroundMobileParty(mobileParty2, navigationCapability, num5, num6, func);
					if (num4 >= 0)
					{
						Settlement settlement2 = Settlement.All[num4];
						MobileParty.NavigationType navigationType2;
						float num7;
						bool flag2;
						this.CalculateTargetSettlementScore(mobileParty, settlement2, out navigationType2, out num7, out flag2);
						if (num7 > num)
						{
							targetSettlement = settlement2;
							num = num7;
							bestNavigationType = navigationType2;
							isTargetingPort = flag2;
						}
					}
				}
				while (num4 >= 0);
			}
			if (targetSettlement == null)
			{
				targetSettlement = SettlementHelper.FindNearestFortificationToMobileParty(mobileParty, mobileParty.NavigationCapability, (Settlement x) => x.OwnerClan != null && !x.OwnerClan.IsAtWarWith(mobileParty.MapFaction));
			}
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0010F73C File Offset: 0x0010D93C
		private void CalculateTargetSettlementScore(MobileParty disbandParty, Settlement settlement, out MobileParty.NavigationType bestNavigationType, out float bestScore, out bool isTargetingPort)
		{
			isTargetingPort = false;
			float num;
			bool flag;
			AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(disbandParty, settlement, false, out bestNavigationType, out num, out flag);
			if (settlement.HasPort && disbandParty.HasNavalNavigationCapability)
			{
				MobileParty.NavigationType navigationType;
				float num2;
				AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(disbandParty, settlement, true, out navigationType, out num2, out flag);
				if (num2 < num)
				{
					num = num2;
					bestNavigationType = navigationType;
					isTargetingPort = true;
				}
			}
			float num3 = MathF.Pow(1f - 0.95f * (MathF.Min(Campaign.MapDiagonal, num) / Campaign.MapDiagonal), 3f);
			Hero owner = disbandParty.Party.Owner;
			float num4;
			if (((owner != null) ? owner.Clan : null) != settlement.OwnerClan)
			{
				Hero owner2 = disbandParty.Party.Owner;
				num4 = ((((owner2 != null) ? owner2.MapFaction : null) == settlement.MapFaction) ? 0.1f : 0.01f);
			}
			else
			{
				num4 = 1f;
			}
			float num5 = num4;
			float num6 = ((disbandParty.DefaultBehavior == AiBehavior.GoToSettlement && disbandParty.TargetSettlement == settlement) ? 1f : 0.3f);
			bestScore = num3 * num5 * num6;
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x0010F82C File Offset: 0x0010DA2C
		private void OnPartyDisbanded(MobileParty disbandParty, Settlement relatedSettlement)
		{
			if (relatedSettlement != null && !disbandParty.IsCustomParty)
			{
				if (relatedSettlement.IsFortification)
				{
					if (!relatedSettlement.IsUnderSiege)
					{
						this.MergeDisbandPartyToFortification(disbandParty, relatedSettlement);
						return;
					}
				}
				else if (relatedSettlement.IsVillage && relatedSettlement.Village.VillageState == Village.VillageStates.Normal)
				{
					this.MergeDisbandPartyToVillage(disbandParty, relatedSettlement);
				}
			}
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0010F87C File Offset: 0x0010DA7C
		private void MergeDisbandPartyToFortification(MobileParty disbandParty, Settlement relatedSettlement)
		{
			if (disbandParty.PrisonRoster.TotalHeroes > 0)
			{
				TroopRoster troopRoster = null;
				foreach (TroopRosterElement troopRosterElement in disbandParty.PrisonRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character.HeroObject != null)
					{
						if (troopRosterElement.Character.HeroObject.MapFaction.IsAtWarWith(relatedSettlement.MapFaction))
						{
							if (troopRoster == null)
							{
								troopRoster = TroopRoster.CreateDummyTroopRoster();
							}
							TransferPrisonerAction.Apply(troopRosterElement.Character, disbandParty.Party, relatedSettlement.Party);
							troopRoster.Add(troopRosterElement);
						}
						else
						{
							EndCaptivityAction.ApplyByEscape(troopRosterElement.Character.HeroObject, null, true);
						}
					}
				}
				if (troopRoster != null)
				{
					CampaignEventDispatcher.Instance.OnPrisonerDonatedToSettlement(disbandParty, troopRoster.ToFlattenedRoster(), relatedSettlement);
				}
			}
			if (disbandParty.PrisonRoster.TotalManCount > 0)
			{
				SellPrisonersAction.ApplyForAllPrisoners(disbandParty.Party, relatedSettlement.Party);
			}
			if (disbandParty.MemberRoster.TotalManCount > 0)
			{
				if (disbandParty.MapFaction == relatedSettlement.MapFaction)
				{
					if (relatedSettlement.Town.GarrisonParty == null)
					{
						relatedSettlement.AddGarrisonParty();
					}
					float num = 0f;
					foreach (TroopRosterElement troopRosterElement2 in disbandParty.MemberRoster.GetTroopRoster())
					{
						num += (float)troopRosterElement2.Number * Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterTroopDonation(disbandParty.Party, troopRosterElement2.Character, relatedSettlement);
					}
					relatedSettlement.Town.GarrisonParty.Party.MemberRoster.Add(disbandParty.MemberRoster);
					GainKingdomInfluenceAction.ApplyForDonatePrisoners(disbandParty, num);
				}
				disbandParty.MemberRoster.Clear();
			}
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x0010FA58 File Offset: 0x0010DC58
		private void MergeDisbandPartyToVillage(MobileParty disbandParty, Settlement settlement)
		{
			if (disbandParty.PrisonRoster.TotalHeroes > 0)
			{
				foreach (TroopRosterElement troopRosterElement in disbandParty.PrisonRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character.HeroObject != null)
					{
						EndCaptivityAction.ApplyByEscape(troopRosterElement.Character.HeroObject, null, true);
					}
				}
			}
			if (disbandParty.PrisonRoster.TotalManCount > 0)
			{
				disbandParty.PrisonRoster.Clear();
			}
			if (disbandParty.MemberRoster.TotalManCount > 0)
			{
				float num = (float)disbandParty.MemberRoster.TotalManCount * 0.5f;
				settlement.Militia += num;
			}
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x0010FB20 File Offset: 0x0010DD20
		private void CheckDisbandedPartyDaily(MobileParty disbandParty, Settlement settlement)
		{
			if (disbandParty.MemberRoster.Count == 0)
			{
				DestroyPartyAction.Apply(null, disbandParty);
				return;
			}
			if (settlement != null || disbandParty.StationaryStartTime.ElapsedDaysUntilNow < 0.125f)
			{
				if (settlement != null && settlement == disbandParty.CurrentSettlement)
				{
					DestroyPartyAction.ApplyForDisbanding(disbandParty, settlement);
				}
				return;
			}
			Settlement currentSettlementOfMobilePartyForAICalculation = MobilePartyHelper.GetCurrentSettlementOfMobilePartyForAICalculation(disbandParty);
			if (currentSettlementOfMobilePartyForAICalculation != null)
			{
				DestroyPartyAction.ApplyForDisbanding(disbandParty, currentSettlementOfMobilePartyForAICalculation);
				return;
			}
			if (disbandParty.MemberRoster.TotalHeroes > 0)
			{
				foreach (TroopRosterElement troopRosterElement in disbandParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character.IsHero && !troopRosterElement.Character.IsPlayerCharacter && !troopRosterElement.Character.HeroObject.IsDead)
					{
						MakeHeroFugitiveAction.Apply(troopRosterElement.Character.HeroObject, false);
					}
				}
			}
			DestroyPartyAction.Apply(null, disbandParty);
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x0010FC1C File Offset: 0x0010DE1C
		private void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("disbanding_leaderless_party_start", "start", "disbanding_leaderless_party_start_response", "{=!}{EXPLANATION}", new ConversationSentence.OnConditionDelegate(this.disbanding_leaderless_party_start_on_condition), null, 500, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_take_party", "disbanding_leaderless_party_start_response", "close_window", "{=eyZo8ZTk}Let me inspect the party troops.", new ConversationSentence.OnConditionDelegate(this.disbanding_leaderless_party_join_main_party_answer_condition), new ConversationSentence.OnConsequenceDelegate(this.disbanding_leaderless_party_join_main_party_answer_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_attack_neutral", "disbanding_leaderless_party_start_response", "attack_disbanding_party_neutral_response", "{=SXgm2b1M}You're not going anywhere. Not with your valuables, anyway.", new ConversationSentence.OnConditionDelegate(this.attack_neutral_disbanding_party_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("disbanding_leaderless_party_answer_attack_neutral_di", "attack_disbanding_party_neutral_response", "attack_disbanding_party_neutral_player_response", "{=CgS44dOE}Are you mad? We're not your enemy.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_attack_neutral_2", "attack_disbanding_party_neutral_player_response", "close_window", "{=Mt5F4wE2}No, you're my prey. Prepare to fight!", null, new ConversationSentence.OnConsequenceDelegate(this.attack_disbanding_party_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_attack_neutral_3", "attack_disbanding_party_neutral_player_response", "close_window", "{=XrQBTVis}I don't know what I was thinking. Go on, then...", null, new ConversationSentence.OnConsequenceDelegate(this.disbanding_leaderless_party_answer_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_attack_enemy", "disbanding_leaderless_party_start_response", "attack_disbanding_enemy_response", "{=WwLy9Src}You know we're at war. I can't just let you go.", new ConversationSentence.OnConditionDelegate(this.attack_enemy_disbanding_party_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("disbanding_leaderless_party_answer", "attack_disbanding_enemy_response", "close_window", "{=jBN2LlgF}We'll fight to our last drop of blood!", null, new ConversationSentence.OnConsequenceDelegate(this.attack_disbanding_party_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_2", "disbanding_leaderless_party_start_response", "close_window", "{=disband_party_campaign_behaviorbdisbanding_leaderless_party_answer}Well... Go on, then.", null, new ConversationSentence.OnConsequenceDelegate(this.disbanding_leaderless_party_answer_on_consequence), 100, null, null);
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x0010FDB8 File Offset: 0x0010DFB8
		private bool disbanding_leaderless_party_start_on_condition()
		{
			bool flag = MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsLordParty && (MobileParty.ConversationParty.LeaderHero == null || MobileParty.ConversationParty.IsDisbanding || this.IsPartyWaitingForDisband(MobileParty.ConversationParty));
			if (flag)
			{
				if (MobileParty.ConversationParty.LeaderHero == null)
				{
					if (MobileParty.ConversationParty.TargetSettlement != null)
					{
						TextObject textObject = new TextObject("{=9IwzVbJf}We recently lost our leader, now we are traveling to {TARGET_SETTLEMENT}. We will rejoin the garrison unless we are assigned a new leader.", null);
						textObject.SetTextVariable("TARGET_SETTLEMENT", MobileParty.ConversationParty.TargetSettlement.EncyclopediaLinkWithName);
						MBTextManager.SetTextVariable("EXPLANATION", textObject, false);
						return flag;
					}
					MBTextManager.SetTextVariable("EXPLANATION", new TextObject("{=COEifaao}We recently lost our leader. We are now waiting for new orders.", null), false);
					return flag;
				}
				else
				{
					if (MobileParty.ConversationParty.TargetSettlement != null)
					{
						TextObject textObject2 = new TextObject("{=uZIlfFa2}We're disbanding. We're all going to {TARGET_SETTLEMENT_LINK}, then we're going our separate ways.", null);
						textObject2.SetTextVariable("TARGET_SETTLEMENT_LINK", MobileParty.ConversationParty.TargetSettlement.EncyclopediaLinkWithName);
						MBTextManager.SetTextVariable("EXPLANATION", textObject2, false);
						return flag;
					}
					MBTextManager.SetTextVariable("EXPLANATION", new TextObject("{=G1PN6ku4}We're disbanding.", null), false);
				}
			}
			return flag;
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x0010FEC0 File Offset: 0x0010E0C0
		private bool disbanding_leaderless_party_join_main_party_answer_condition()
		{
			MobileParty conversationParty = MobileParty.ConversationParty;
			return conversationParty != null && ((conversationParty.Party.Owner != null && conversationParty.Party.Owner.Clan != null && conversationParty.Party.Owner.Clan == Clan.PlayerClan) || (conversationParty.ActualClan != null && conversationParty.ActualClan == Clan.PlayerClan));
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x0010FF27 File Offset: 0x0010E127
		private void disbanding_leaderless_party_join_main_party_answer_on_consequence()
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
			PartyScreenHelper.OpenScreenAsManageTroopsAndPrisoners(MobileParty.ConversationParty, new PartyScreenClosedDelegate(PartyScreenHelper.OpenScreenAsManagePlayerClanPartyClosed));
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x0010FF4C File Offset: 0x0010E14C
		private void disbanding_leaderless_party_answer_on_consequence()
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x0010FF5C File Offset: 0x0010E15C
		private bool attack_neutral_disbanding_party_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.MapFaction != Clan.PlayerClan.MapFaction && !FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, MobileParty.ConversationParty.MapFaction) && !MobileParty.MainParty.IsInRaftState;
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x0010FFB0 File Offset: 0x0010E1B0
		private bool attack_enemy_disbanding_party_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.MapFaction != Clan.PlayerClan.MapFaction && FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, MobileParty.ConversationParty.MapFaction) && !MobileParty.MainParty.IsInRaftState;
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x00110004 File Offset: 0x0010E204
		private void attack_disbanding_party_consequence()
		{
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, MobileParty.ConversationParty.Party);
		}

		// Token: 0x040012C4 RID: 4804
		private const int DisbandDelayTimeAsDays = 1;

		// Token: 0x040012C5 RID: 4805
		private const float RemoveDisbandingPartyAfterHoldForDays = 0.125f;

		// Token: 0x040012C6 RID: 4806
		private const int DisbandPartySizeLimitForAIParties = 10;

		// Token: 0x040012C7 RID: 4807
		private Dictionary<MobileParty, CampaignTime> _partiesThatWaitingToDisband = new Dictionary<MobileParty, CampaignTime>();
	}
}
