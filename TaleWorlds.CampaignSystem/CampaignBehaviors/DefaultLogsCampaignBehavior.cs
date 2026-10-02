using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E5 RID: 997
	public class DefaultLogsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003DC6 RID: 15814 RVA: 0x0010DC94 File Offset: 0x0010BE94
		public override void RegisterEvents()
		{
			CampaignEvents.AlleyOwnerChanged.AddNonSerializedListener(this, new Action<Alley, Hero, Hero>(this.OnAlleyOwnerChanged));
			CampaignEvents.ArmyGathered.AddNonSerializedListener(this, new Action<Army, IMapPoint>(this.OnArmyGathered));
			CampaignEvents.BattleStarted.AddNonSerializedListener(this, new Action<PartyBase, PartyBase, object, bool>(this.OnBattleStarted));
			CampaignEvents.CharacterBecameFugitiveEvent.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnCharacterBecameFugitive));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.ClanChangedKingdom));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnPrisonerTaken));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>(this.OnHeroPrisonerReleased));
			CampaignEvents.BeforeHeroesMarried.AddNonSerializedListener(this, new Action<Hero, Hero, bool>(this.OnHeroesMarried));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.ArmyCreated.AddNonSerializedListener(this, new Action<Army>(this.OnArmyCreated));
			CampaignEvents.OnTradeAgreementSignedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnTradeAgreementSigned));
			CampaignEvents.RebellionFinished.AddNonSerializedListener(this, new Action<Settlement, Clan>(this.OnRebellionFinished));
			CampaignEvents.KingdomDecisionAdded.AddNonSerializedListener(this, new Action<KingdomDecision, bool>(this.OnKingdomDecisionAdded));
			CampaignEvents.KingdomDecisionConcluded.AddNonSerializedListener(this, new Action<KingdomDecision, DecisionOutcome, bool>(this.OnKingdomDecisionConcluded));
			CampaignEvents.TournamentFinished.AddNonSerializedListener(this, new Action<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject>(this.OnTournamentFinished));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
			CampaignEvents.PlayerTraitChangedEvent.AddNonSerializedListener(this, new Action<TraitObject, int>(this.OnPlayerTraitChanged));
			CampaignEvents.OnPlayerCharacterChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero, MobileParty, bool>(this.OnPlayerCharacterChanged));
			CampaignEvents.OnSiegeAftermathAppliedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, SiegeAftermathAction.SiegeAftermath, Clan, Dictionary<MobileParty, float>>(this.OnSiegeAftermathApplied));
			CampaignEvents.OnAllianceStartedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceStartedEvent));
			CampaignEvents.OnAllianceEndedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceEndedEvent));
			CampaignEvents.OnCallToWarAgreementStartedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom, Kingdom>(this.OnCallToWarAgreementStarted));
			CampaignEvents.OnCallToWarAgreementEndedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom, Kingdom>(this.OnCallToWarAgreementEnded));
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x0010DEB2 File Offset: 0x0010C0B2
		private void OnSiegeAftermathApplied(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			LogEntry.AddLogEntry(new SiegeAftermathLogEntry(attackerParty, partyContributions.Keys, settlement, aftermathType));
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x0010DEC8 File Offset: 0x0010C0C8
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x0010DECA File Offset: 0x0010C0CA
		private void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMobileParty, bool isMainPartyChanged)
		{
			LogEntry.AddLogEntry(new PlayerCharacterChangedLogEntry(oldPlayer, newPlayer));
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x0010DED8 File Offset: 0x0010C0D8
		private void OnPrisonerTaken(PartyBase party, Hero hero)
		{
			LogEntry.AddLogEntry(new TakePrisonerLogEntry(party, hero));
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x0010DEE6 File Offset: 0x0010C0E6
		private void OnHeroPrisonerReleased(Hero hero, PartyBase party, IFaction captuererFaction, EndCaptivityDetail detail, bool showNotification)
		{
			if (showNotification)
			{
				LogEntry.AddLogEntry(new EndCaptivityLogEntry(hero, captuererFaction, detail));
			}
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x0010DEFA File Offset: 0x0010C0FA
		private void OnCommonAreaFightOccured(MobileParty attackerParty, MobileParty defenderParty, Hero attackerHero, Settlement settlement)
		{
			LogEntry.AddLogEntry(new CommonAreaFightLogEntry(attackerParty, defenderParty, attackerHero, settlement));
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x0010DF0B File Offset: 0x0010C10B
		private void ClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotifications)
		{
			if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinAsMercenary || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveAsMercenary)
			{
				LogEntry.AddLogEntry(new MercenaryClanChangedKingdomLogEntry(clan, oldKingdom, newKingdom));
			}
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x0010DF23 File Offset: 0x0010C123
		private void OnCharacterBecameFugitive(Hero hero, bool showNotification)
		{
			LogEntry.AddLogEntry(new CharacterBecameFugitiveLogEntry(hero));
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x0010DF30 File Offset: 0x0010C130
		private void OnBattleStarted(PartyBase attackerParty, PartyBase defenderParty, object subject, bool showNotification)
		{
			if (showNotification)
			{
				LogEntry.AddLogEntry(new BattleStartedLogEntry(attackerParty, defenderParty, subject));
			}
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x0010DF44 File Offset: 0x0010C144
		public void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
			if (isPlayersArmy)
			{
				ArmyDispersionLogEntry armyDispersionLogEntry = new ArmyDispersionLogEntry(army, reason);
				LogEntry.AddLogEntry(armyDispersionLogEntry);
				if (army.LeaderParty.MapFaction == Hero.MainHero.MapFaction && army.Parties.IndexOf(MobileParty.MainParty) < 0)
				{
					Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ArmyDispersionMapNotification(army, reason, armyDispersionLogEntry.GetEncyclopediaText()));
				}
			}
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x0010DFA8 File Offset: 0x0010C1A8
		private void OnArmyGathered(Army army, IMapPoint gatheringPoint)
		{
			LogEntry.AddLogEntry(new GatherArmyLogEntry(army, gatheringPoint));
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x0010DFB8 File Offset: 0x0010C1B8
		private void OnArmyCreated(Army army)
		{
			ArmyCreationLogEntry armyCreationLogEntry = new ArmyCreationLogEntry(army);
			LogEntry.AddLogEntry(armyCreationLogEntry);
			if (army.LeaderParty.MapFaction == MobileParty.MainParty.MapFaction && army.LeaderParty != MobileParty.MainParty)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ArmyCreationMapNotification(army, armyCreationLogEntry.GetEncyclopediaText()));
			}
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x0010E011 File Offset: 0x0010C211
		private void OnTradeAgreementSigned(Kingdom kingdom1, Kingdom kingdom2)
		{
			LogEntry.AddLogEntry(new TradeAgreementLogEntry(kingdom1, kingdom2));
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x0010E020 File Offset: 0x0010C220
		private void OnRebellionFinished(Settlement settlement, Clan oldOwnerClan)
		{
			RebellionStartedLogEntry rebellionStartedLogEntry = new RebellionStartedLogEntry(settlement, oldOwnerClan);
			LogEntry.AddLogEntry(rebellionStartedLogEntry);
			if (oldOwnerClan == Clan.PlayerClan)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new SettlementRebellionMapNotification(settlement, rebellionStartedLogEntry.GetNotificationText()));
			}
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x0010E060 File Offset: 0x0010C260
		private void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
			LogEntry.AddLogEntry(new KingdomDecisionAddedLogEntry(decision, isPlayerInvolved));
			if (decision.NotifyPlayer && isPlayerInvolved && !decision.IsEnforced)
			{
				TextObject textObject = (decision.DetermineChooser().Leader.IsHumanPlayerCharacter ? decision.GetChooseTitle() : decision.GetSupportTitle());
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new KingdomDecisionMapNotification(decision.Kingdom, decision, textObject));
			}
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x0010E0C8 File Offset: 0x0010C2C8
		private void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
			KingdomDecisionConcludedLogEntry kingdomDecisionConcludedLogEntry = new KingdomDecisionConcludedLogEntry(decision, chosenOutcome, isPlayerInvolved);
			LogEntry.AddLogEntry(kingdomDecisionConcludedLogEntry);
			if (decision.Kingdom == Hero.MainHero.MapFaction && decision.NotifyPlayer && !decision.IsEnforced && !isPlayerInvolved)
			{
				MBInformationManager.AddQuickInformation(kingdomDecisionConcludedLogEntry.GetNotificationText(), 0, null, null, "event:/ui/notification/kingdom_decision");
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new KingdomDecisionMapNotification(decision.Kingdom, decision, kingdomDecisionConcludedLogEntry.GetNotificationText()));
			}
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x0010E13D File Offset: 0x0010C33D
		private void OnAlleyOwnerChanged(Alley alley, Hero newOwner, Hero oldOwner)
		{
			if (Campaign.Current.GameStarted)
			{
				LogEntry.AddLogEntry(new ChangeAlleyOwnerLogEntry(alley, newOwner, oldOwner));
			}
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x0010E158 File Offset: 0x0010C358
		private void OnHeroesMarried(Hero marriedHero, Hero marriedTo, bool showNotification)
		{
			CharacterMarriedLogEntry characterMarriedLogEntry = new CharacterMarriedLogEntry(marriedHero, marriedTo);
			LogEntry.AddLogEntry(characterMarriedLogEntry);
			if (marriedHero.Clan == Clan.PlayerClan || marriedTo.Clan == Clan.PlayerClan)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new MarriageMapNotification(marriedHero, marriedTo, characterMarriedLogEntry.GetEncyclopediaText(), CampaignTime.Now));
			}
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x0010E1B0 File Offset: 0x0010C3B0
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			BesiegeSettlementLogEntry besiegeSettlementLogEntry = new BesiegeSettlementLogEntry(siegeEvent.BesiegerCamp.LeaderParty, siegeEvent.BesiegedSettlement);
			LogEntry.AddLogEntry(besiegeSettlementLogEntry);
			if (siegeEvent.BesiegedSettlement.OwnerClan == Clan.PlayerClan)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new SettlementUnderSiegeMapNotification(siegeEvent, besiegeSettlementLogEntry.GetEncyclopediaText()));
			}
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x0010E207 File Offset: 0x0010C407
		private void OnTournamentFinished(CharacterObject character, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
			if (character.IsHero)
			{
				LogEntry.AddLogEntry(new TournamentWonLogEntry(character.HeroObject, town, participants));
			}
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x0010E224 File Offset: 0x0010C424
		private void OnPlayerTraitChanged(TraitObject trait, int previousLevel)
		{
			int traitLevel = Hero.MainHero.GetTraitLevel(trait);
			TextObject traitChangedText = DefaultLogsCampaignBehavior.GetTraitChangedText(trait, traitLevel, previousLevel);
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new TraitChangedMapNotification(trait, traitLevel != 0, previousLevel, traitChangedText));
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x0010E261 File Offset: 0x0010C461
		private void OnCallToWarAgreementEnded(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			LogEntry.AddLogEntry(new EndCallToWarAgreementLogEntry(callingKingdom, calledKingdom, kingdomToCallToWarAgainst));
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x0010E270 File Offset: 0x0010C470
		private void OnCallToWarAgreementStarted(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			LogEntry.AddLogEntry(new StartCallToWarAgreementLogEntry(callingKingdom, calledKingdom, kingdomToCallToWarAgainst));
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x0010E27F File Offset: 0x0010C47F
		private void OnAllianceEndedEvent(Kingdom kingdom1, Kingdom kingdom2)
		{
			LogEntry.AddLogEntry(new EndAllianceLogEntry(kingdom1, kingdom2));
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x0010E28D File Offset: 0x0010C48D
		private void OnAllianceStartedEvent(Kingdom kingdom1, Kingdom kingdom2)
		{
			LogEntry.AddLogEntry(new StartAllianceLogEntry(kingdom1, kingdom2));
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x0010E29C File Offset: 0x0010C49C
		private static TextObject GetTraitChangedText(TraitObject traitObject, int level, int previousLevel)
		{
			TextObject textObject;
			TextObject textObject2;
			if (level != 0)
			{
				textObject = GameTexts.FindText("str_trait_name_" + traitObject.StringId.ToLower(), (level + MathF.Abs(traitObject.MinValue)).ToString());
				textObject2 = GameTexts.FindText("str_trait_gained_text", null);
			}
			else
			{
				textObject = GameTexts.FindText("str_trait_name_" + traitObject.StringId.ToLower(), (previousLevel + MathF.Abs(traitObject.MinValue)).ToString());
				textObject2 = GameTexts.FindText("str_trait_lost_text", null);
			}
			textObject2.SetCharacterProperties("HERO", Hero.MainHero.CharacterObject, false);
			textObject2.SetTextVariable("TRAIT_NAME", textObject);
			return textObject2;
		}
	}
}
