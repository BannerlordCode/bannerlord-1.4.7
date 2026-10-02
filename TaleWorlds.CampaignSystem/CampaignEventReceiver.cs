using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003D RID: 61
	public abstract class CampaignEventReceiver
	{
		// Token: 0x06000503 RID: 1283 RVA: 0x00021B8B File Offset: 0x0001FD8B
		public virtual void RemoveListeners(object o)
		{
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00021B8D File Offset: 0x0001FD8D
		public virtual void OnCharacterCreationIsOver()
		{
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00021B8F File Offset: 0x0001FD8F
		public virtual void OnHeroLevelledUp(Hero hero, bool shouldNotify = true)
		{
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00021B91 File Offset: 0x0001FD91
		public virtual void OnHomeHideoutChanged(BanditPartyComponent banditPartyComponent, Hideout oldHomeHideout)
		{
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00021B93 File Offset: 0x0001FD93
		public virtual void OnHeroGainedSkill(Hero hero, SkillObject skill, int change = 1, bool shouldNotify = true)
		{
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00021B95 File Offset: 0x0001FD95
		public virtual void OnHeroCreated(Hero hero, bool isBornNaturally = false)
		{
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00021B97 File Offset: 0x0001FD97
		public virtual void OnHeroActivated(Hero hero, Hero.CharacterStates previousState)
		{
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00021B99 File Offset: 0x0001FD99
		public virtual void OnHeroWounded(Hero woundedHero)
		{
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00021B9B File Offset: 0x0001FD9B
		public virtual void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00021B9D File Offset: 0x0001FD9D
		public virtual void OnQuestLogAdded(QuestBase quest, bool hideInformation)
		{
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00021B9F File Offset: 0x0001FD9F
		public virtual void OnIssueLogAdded(IssueBase issue, bool hideInformation)
		{
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00021BA1 File Offset: 0x0001FDA1
		public virtual void OnClanTierChanged(Clan clan, bool shouldNotify = true)
		{
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00021BA3 File Offset: 0x0001FDA3
		public virtual void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail actionDetail, bool showNotification = true)
		{
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00021BA5 File Offset: 0x0001FDA5
		public virtual void OnClanDefected(Clan clan, Kingdom oldKingdom, Kingdom newKingdom)
		{
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00021BA7 File Offset: 0x0001FDA7
		public virtual void OnClanCreated(Clan clan, bool isCompanion)
		{
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00021BA9 File Offset: 0x0001FDA9
		public virtual void OnHeroJoinedParty(Hero hero, MobileParty mobileParty)
		{
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00021BAB File Offset: 0x0001FDAB
		public virtual void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00021BAD File Offset: 0x0001FDAD
		public virtual void OnKingdomDecisionCancelled(KingdomDecision decision, bool isPlayerInvolved)
		{
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00021BAF File Offset: 0x0001FDAF
		public virtual void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00021BB1 File Offset: 0x0001FDB1
		public virtual void OnHeroOrPartyTradedGold(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> recipient, ValueTuple<int, string> goldAmount, bool showNotification)
		{
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00021BB3 File Offset: 0x0001FDB3
		public virtual void OnHeroOrPartyGaveItem(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> receiver, ItemRosterElement itemRosterElement, bool showNotification)
		{
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00021BB5 File Offset: 0x0001FDB5
		public virtual void OnBanditPartyRecruited(MobileParty banditParty)
		{
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00021BB7 File Offset: 0x0001FDB7
		public virtual void OnArmyCreated(Army army)
		{
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00021BB9 File Offset: 0x0001FDB9
		public virtual void OnPartyAttachedAnotherParty(MobileParty mobileParty)
		{
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00021BBB File Offset: 0x0001FDBB
		public virtual void OnNearbyPartyAddedToPlayerMapEvent(MobileParty mobileParty)
		{
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00021BBD File Offset: 0x0001FDBD
		public virtual void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00021BBF File Offset: 0x0001FDBF
		public virtual void OnArmyGathered(Army army, IMapPoint gatheringPoint)
		{
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00021BC1 File Offset: 0x0001FDC1
		public virtual void OnPerkOpened(Hero hero, PerkObject perk)
		{
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00021BC3 File Offset: 0x0001FDC3
		public virtual void OnPerkReset(Hero hero, PerkObject perk)
		{
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00021BC5 File Offset: 0x0001FDC5
		public virtual void OnPlayerTraitChanged(TraitObject trait, int previousLevel)
		{
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00021BC7 File Offset: 0x0001FDC7
		public virtual void OnVillageStateChanged(Village village, Village.VillageStates oldState, Village.VillageStates newState, MobileParty raiderParty)
		{
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00021BC9 File Offset: 0x0001FDC9
		public virtual void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00021BCB File Offset: 0x0001FDCB
		public virtual void OnAfterSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00021BCD File Offset: 0x0001FDCD
		public virtual void OnBeforeSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00021BCF File Offset: 0x0001FDCF
		public virtual void OnMercenaryTroopChangedInTown(Town town, CharacterObject oldTroopType, CharacterObject newTroopType)
		{
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00021BD1 File Offset: 0x0001FDD1
		public virtual void OnMercenaryNumberChangedInTown(Town town, int oldNumber, int newNumber)
		{
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00021BD3 File Offset: 0x0001FDD3
		public virtual void OnAlleyOwnerChanged(Alley alley, Hero newOwner, Hero oldOwner)
		{
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00021BD5 File Offset: 0x0001FDD5
		public virtual void OnAlleyClearedByPlayer(Alley alley)
		{
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00021BD7 File Offset: 0x0001FDD7
		public virtual void OnAlleyOccupiedByPlayer(Alley alley, TroopRoster troops)
		{
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00021BD9 File Offset: 0x0001FDD9
		public virtual void OnRomanticStateChanged(Hero hero1, Hero hero2, Romance.RomanceLevelEnum romanceLevel)
		{
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00021BDB File Offset: 0x0001FDDB
		public virtual void OnBeforeHeroesMarried(Hero hero1, Hero hero2, bool showNotification = true)
		{
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00021BDD File Offset: 0x0001FDDD
		public virtual void OnPlayerEliminatedFromTournament(int round, Town town)
		{
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00021BDF File Offset: 0x0001FDDF
		public virtual void OnPlayerStartedTournamentMatch(Town town)
		{
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00021BE1 File Offset: 0x0001FDE1
		public virtual void OnTournamentStarted(Town town)
		{
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00021BE3 File Offset: 0x0001FDE3
		public virtual void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00021BE5 File Offset: 0x0001FDE5
		public virtual void OnTournamentCancelled(Town town)
		{
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00021BE7 File Offset: 0x0001FDE7
		public virtual void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00021BE9 File Offset: 0x0001FDE9
		public virtual void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00021BEB File Offset: 0x0001FDEB
		public virtual void OnKingdomCreated(Kingdom createdKingdom)
		{
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00021BED File Offset: 0x0001FDED
		public virtual void OnHeroOccupationChanged(Hero hero, Occupation oldOccupation)
		{
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00021BEF File Offset: 0x0001FDEF
		public virtual void OnKingdomDestroyed(Kingdom kingdom)
		{
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00021BF1 File Offset: 0x0001FDF1
		public virtual void CanKingdomBeDiscontinued(Kingdom kingdom, ref bool result)
		{
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00021BF3 File Offset: 0x0001FDF3
		public virtual void OnBarterAccepted(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00021BF5 File Offset: 0x0001FDF5
		public virtual void OnBarterCanceled(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00021BF7 File Offset: 0x0001FDF7
		public virtual void OnStartBattle(PartyBase attackerParty, PartyBase defenderParty, object subject, bool showNotification)
		{
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00021BF9 File Offset: 0x0001FDF9
		public virtual void OnRebellionFinished(Settlement settlement, Clan oldOwnerClan)
		{
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00021BFB File Offset: 0x0001FDFB
		public virtual void TownRebelliousStateChanged(Town town, bool rebelliousState)
		{
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00021BFD File Offset: 0x0001FDFD
		public virtual void OnRebelliousClanDisbandedAtSettlement(Settlement settlement, Clan clan)
		{
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00021BFF File Offset: 0x0001FDFF
		public virtual void OnItemsLooted(MobileParty mobileParty, ItemRoster items)
		{
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00021C01 File Offset: 0x0001FE01
		public virtual void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00021C03 File Offset: 0x0001FE03
		public virtual void OnMobilePartyCreated(MobileParty party)
		{
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00021C05 File Offset: 0x0001FE05
		public virtual void OnMapInteractableCreated(IInteractablePoint interactable)
		{
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00021C07 File Offset: 0x0001FE07
		public virtual void OnMapInteractableDestroyed(IInteractablePoint interactable)
		{
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00021C09 File Offset: 0x0001FE09
		public virtual void OnMobilePartyQuestStatusChanged(MobileParty party, bool isUsedByQuest)
		{
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00021C0B File Offset: 0x0001FE0B
		public virtual void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00021C0D File Offset: 0x0001FE0D
		public virtual void OnBeforeHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00021C0F File Offset: 0x0001FE0F
		public virtual void OnChildEducationCompleted(Hero hero, int age)
		{
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00021C11 File Offset: 0x0001FE11
		public virtual void OnHeroComesOfAge(Hero hero)
		{
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00021C13 File Offset: 0x0001FE13
		public virtual void OnHeroReachesTeenAge(Hero hero)
		{
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00021C15 File Offset: 0x0001FE15
		public virtual void OnHeroGrowsOutOfInfancy(Hero hero)
		{
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00021C17 File Offset: 0x0001FE17
		public virtual void OnCharacterDefeated(Hero winner, Hero loser)
		{
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00021C19 File Offset: 0x0001FE19
		public virtual void OnHeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00021C1B File Offset: 0x0001FE1B
		public virtual void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00021C1D File Offset: 0x0001FE1D
		public virtual void OnCharacterBecameFugitive(Hero hero, bool showNotification)
		{
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00021C1F File Offset: 0x0001FE1F
		public virtual void OnPlayerMetHero(Hero hero)
		{
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00021C21 File Offset: 0x0001FE21
		public virtual void OnPlayerLearnsAboutHero(Hero hero)
		{
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00021C23 File Offset: 0x0001FE23
		public virtual void OnRenownGained(Hero hero, int gainedRenown, bool doNotNotify)
		{
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00021C25 File Offset: 0x0001FE25
		public virtual void OnCrimeRatingChanged(IFaction kingdom, float deltaCrimeAmount)
		{
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00021C27 File Offset: 0x0001FE27
		public virtual void OnNewCompanionAdded(Hero newCompanion)
		{
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00021C29 File Offset: 0x0001FE29
		public virtual void OnAfterMissionStarted(IMission iMission)
		{
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00021C2B File Offset: 0x0001FE2B
		public virtual void OnGameMenuOpened(MenuCallbackArgs args)
		{
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00021C2D File Offset: 0x0001FE2D
		public virtual void OnVillageBecomeNormal(Village village)
		{
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00021C2F File Offset: 0x0001FE2F
		public virtual void OnVillageBeingRaided(Village village)
		{
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00021C31 File Offset: 0x0001FE31
		public virtual void OnVillageLooted(Village village)
		{
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00021C33 File Offset: 0x0001FE33
		public virtual void OnAgentJoinedConversation(IAgent agent)
		{
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00021C35 File Offset: 0x0001FE35
		public virtual void OnConversationEnded(IEnumerable<CharacterObject> characters)
		{
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00021C37 File Offset: 0x0001FE37
		public virtual void OnMapEventEnded(MapEvent mapEvent)
		{
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00021C39 File Offset: 0x0001FE39
		public virtual void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00021C3B File Offset: 0x0001FE3B
		public virtual void OnRansomOfferedToPlayer(Hero captiveHero)
		{
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00021C3D File Offset: 0x0001FE3D
		public virtual void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster prisonerRoster, Hero prisonerHero, bool takenFromDungeon)
		{
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00021C3F File Offset: 0x0001FE3F
		public virtual void OnMissionStarted(IMission mission)
		{
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00021C41 File Offset: 0x0001FE41
		public virtual void OnRansomOfferCancelled(Hero captiveHero)
		{
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00021C43 File Offset: 0x0001FE43
		public virtual void OnPeaceOfferedToPlayer(IFaction opponentFaction, int tributeAmount, int tributeDuration)
		{
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00021C45 File Offset: 0x0001FE45
		public virtual void OnTradeAgreementSigned(Kingdom kingdom, Kingdom other)
		{
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00021C47 File Offset: 0x0001FE47
		public virtual void OnPeaceOfferResolved(IFaction opponentFaction)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00021C49 File Offset: 0x0001FE49
		public virtual void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden)
		{
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00021C4B File Offset: 0x0001FE4B
		public virtual void OnMarriageOfferCanceled(Hero suitor, Hero maiden)
		{
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00021C4D File Offset: 0x0001FE4D
		public virtual void OnVassalOrMercenaryServiceOfferedToPlayer(Kingdom offeredKingdom)
		{
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00021C4F File Offset: 0x0001FE4F
		public virtual void OnVassalOrMercenaryServiceOfferCanceled(Kingdom offeredKingdom)
		{
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00021C51 File Offset: 0x0001FE51
		public virtual void OnPlayerBoardGameOver(Hero opposingHero, BoardGameHelper.BoardGameState state)
		{
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00021C53 File Offset: 0x0001FE53
		public virtual void OnCommonAreaStateChanged(Alley alley, Alley.AreaState oldState, Alley.AreaState newState)
		{
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00021C55 File Offset: 0x0001FE55
		public virtual void BeforeMissionOpened()
		{
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00021C57 File Offset: 0x0001FE57
		public virtual void OnPartyRemoved(PartyBase party)
		{
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00021C59 File Offset: 0x0001FE59
		public virtual void OnPartySizeChanged(PartyBase party)
		{
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00021C5B File Offset: 0x0001FE5B
		public virtual void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00021C5D File Offset: 0x0001FE5D
		public virtual void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00021C5F File Offset: 0x0001FE5F
		public virtual void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00021C61 File Offset: 0x0001FE61
		public virtual void Tick(float dt)
		{
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00021C63 File Offset: 0x0001FE63
		public virtual void OnSessionStart(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00021C65 File Offset: 0x0001FE65
		public virtual void OnAfterSessionStart(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00021C67 File Offset: 0x0001FE67
		public virtual void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00021C69 File Offset: 0x0001FE69
		public virtual void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00021C6B File Offset: 0x0001FE6B
		public virtual void OnGameEarlyLoaded(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00021C6D File Offset: 0x0001FE6D
		public virtual void OnPlayerTradeProfit(int profit)
		{
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00021C6F File Offset: 0x0001FE6F
		public virtual void OnRulingClanChanged(Kingdom kingdom, Clan oldRulingClan)
		{
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00021C71 File Offset: 0x0001FE71
		public virtual void OnPrisonerReleased(FlattenedTroopRoster roster)
		{
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00021C73 File Offset: 0x0001FE73
		public virtual void OnGameLoadFinished()
		{
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00021C75 File Offset: 0x0001FE75
		public virtual void OnPartyJoinedArmy(MobileParty mobileParty)
		{
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00021C77 File Offset: 0x0001FE77
		public virtual void OnPartyRemovedFromArmy(MobileParty mobileParty)
		{
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00021C79 File Offset: 0x0001FE79
		public virtual void OnArmyOverlaySetDirty()
		{
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00021C7B File Offset: 0x0001FE7B
		public virtual void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00021C7D File Offset: 0x0001FE7D
		public virtual void OnPlayerArmyLeaderChangedBehavior()
		{
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00021C7F File Offset: 0x0001FE7F
		public virtual void MissionTick(float dt)
		{
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00021C81 File Offset: 0x0001FE81
		public virtual void OnChildConceived(Hero mother)
		{
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00021C83 File Offset: 0x0001FE83
		public virtual void OnGivenBirth(Hero mother, List<Hero> aliveChildren, int stillbornCount)
		{
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00021C85 File Offset: 0x0001FE85
		public virtual void OnUnitRecruited(CharacterObject character, int amount)
		{
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00021C87 File Offset: 0x0001FE87
		public virtual void OnPlayerBattleEnd(MapEvent mapEvent)
		{
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00021C89 File Offset: 0x0001FE89
		public virtual void OnMissionEnded(IMission mission)
		{
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00021C8B File Offset: 0x0001FE8B
		public virtual void TickPartialHourlyAi(MobileParty party)
		{
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00021C8D File Offset: 0x0001FE8D
		public virtual void QuarterDailyPartyTick(MobileParty party)
		{
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00021C8F File Offset: 0x0001FE8F
		public virtual void AiHourlyTick(MobileParty party, PartyThinkParams partyThinkParams)
		{
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00021C91 File Offset: 0x0001FE91
		public virtual void HourlyTick()
		{
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00021C93 File Offset: 0x0001FE93
		public virtual void QuarterHourlyTick()
		{
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00021C95 File Offset: 0x0001FE95
		public virtual void HourlyTickParty(MobileParty mobileParty)
		{
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00021C97 File Offset: 0x0001FE97
		public virtual void HourlyTickSettlement(Settlement settlement)
		{
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00021C99 File Offset: 0x0001FE99
		public virtual void HourlyTickClan(Clan clan)
		{
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00021C9B File Offset: 0x0001FE9B
		public virtual void DailyTick()
		{
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00021C9D File Offset: 0x0001FE9D
		public virtual void DailyTickParty(MobileParty mobileParty)
		{
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00021C9F File Offset: 0x0001FE9F
		public virtual void DailyTickTown(Town town)
		{
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00021CA1 File Offset: 0x0001FEA1
		public virtual void DailyTickSettlement(Settlement settlement)
		{
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00021CA3 File Offset: 0x0001FEA3
		public virtual void DailyTickClan(Clan clan)
		{
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00021CA5 File Offset: 0x0001FEA5
		public virtual void OnPlayerBodyPropertiesChanged()
		{
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00021CA7 File Offset: 0x0001FEA7
		public virtual void WeeklyTick()
		{
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00021CA9 File Offset: 0x0001FEA9
		public virtual void CollectAvailableTutorials(ref List<CampaignTutorial> tutorials)
		{
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00021CAB File Offset: 0x0001FEAB
		public virtual void DailyTickHero(Hero hero)
		{
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00021CAD File Offset: 0x0001FEAD
		public virtual void OnTutorialCompleted(string tutorial)
		{
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00021CAF File Offset: 0x0001FEAF
		public virtual void OnBuildingLevelChanged(Town town, Building building, int levelChange)
		{
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00021CB1 File Offset: 0x0001FEB1
		public virtual void BeforeGameMenuOpened(MenuCallbackArgs args)
		{
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00021CB3 File Offset: 0x0001FEB3
		public virtual void AfterGameMenuInitialized(MenuCallbackArgs args)
		{
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00021CB5 File Offset: 0x0001FEB5
		public virtual void OnBarterablesRequested(BarterData args)
		{
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00021CB7 File Offset: 0x0001FEB7
		public virtual void OnPartyVisibilityChanged(PartyBase party)
		{
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00021CB9 File Offset: 0x0001FEB9
		public virtual void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00021CBB File Offset: 0x0001FEBB
		public virtual void TrackDetected(Track track)
		{
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00021CBD File Offset: 0x0001FEBD
		public virtual void TrackLost(Track track)
		{
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00021CBF File Offset: 0x0001FEBF
		public virtual void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00021CC1 File Offset: 0x0001FEC1
		public virtual void LocationCharactersSimulated()
		{
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00021CC3 File Offset: 0x0001FEC3
		public virtual void OnBeforePlayerAgentSpawn(ref MatrixFrame spawnFrame)
		{
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00021CC5 File Offset: 0x0001FEC5
		public virtual void OnPlayerAgentSpawned()
		{
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00021CC7 File Offset: 0x0001FEC7
		public virtual void OnPlayerUpgradedTroops(CharacterObject upgradeFromTroop, CharacterObject upgradeToTroop, int number)
		{
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00021CC9 File Offset: 0x0001FEC9
		public virtual void OnHeroCombatHit(CharacterObject attackerTroop, CharacterObject attackedTroop, PartyBase party, WeaponComponentData usedWeapon, bool isFatal, int xp)
		{
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00021CCB File Offset: 0x0001FECB
		public virtual void OnCharacterPortraitPopUpOpened(CharacterObject character)
		{
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00021CCD File Offset: 0x0001FECD
		public virtual void OnCharacterPortraitPopUpClosed()
		{
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00021CCF File Offset: 0x0001FECF
		public virtual void OnPlayerStartTalkFromMenu(Hero hero)
		{
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00021CD1 File Offset: 0x0001FED1
		public virtual void OnGameMenuOptionSelected(GameMenu gameMenu, GameMenuOption gameMenuOption)
		{
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00021CD3 File Offset: 0x0001FED3
		public virtual void OnPlayerStartRecruitment(CharacterObject recruitTroopCharacter)
		{
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00021CD5 File Offset: 0x0001FED5
		public virtual void OnBeforePlayerCharacterChanged(Hero oldPlayer, Hero newPlayer)
		{
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00021CD7 File Offset: 0x0001FED7
		public virtual void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
		{
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00021CD9 File Offset: 0x0001FED9
		public virtual void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00021CDB File Offset: 0x0001FEDB
		public virtual void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00021CDD File Offset: 0x0001FEDD
		public virtual void OnPlayerSiegeStarted()
		{
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00021CDF File Offset: 0x0001FEDF
		public virtual void OnSiegeEventEnded(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00021CE1 File Offset: 0x0001FEE1
		public virtual void OnSiegeAftermathApplied(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00021CE3 File Offset: 0x0001FEE3
		public virtual void OnSiegeBombardmentHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, SiegeBombardTargets target)
		{
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00021CE5 File Offset: 0x0001FEE5
		public virtual void OnSiegeBombardmentWallHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, bool isWallCracked)
		{
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00021CE7 File Offset: 0x0001FEE7
		public virtual void OnSiegeEngineDestroyed(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType destroyedEngine)
		{
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00021CE9 File Offset: 0x0001FEE9
		public virtual void OnTradeRumorIsTaken(List<TradeRumor> newRumors, Settlement sourceSettlement = null)
		{
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00021CEB File Offset: 0x0001FEEB
		public virtual void OnCheckForIssue(Hero hero)
		{
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00021CED File Offset: 0x0001FEED
		public virtual void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver)
		{
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00021CEF File Offset: 0x0001FEEF
		public virtual void OnTroopsDeserted(MobileParty mobileParty, TroopRoster desertedTroops)
		{
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00021CF1 File Offset: 0x0001FEF1
		public virtual void OnTroopRecruited(Hero recruiterHero, Settlement recruitmentSettlement, Hero recruitmentSource, CharacterObject troop, int amount)
		{
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00021CF3 File Offset: 0x0001FEF3
		public virtual void OnTroopGivenToSettlement(Hero giverHero, Settlement recipientSettlement, TroopRoster roster)
		{
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00021CF5 File Offset: 0x0001FEF5
		public virtual void OnItemSold(PartyBase receiverParty, PartyBase payerParty, ItemRosterElement itemRosterElement, int number, Settlement currentSettlement)
		{
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00021CF7 File Offset: 0x0001FEF7
		public virtual void OnCaravanTransactionCompleted(MobileParty caravanParty, Town town, List<ValueTuple<EquipmentElement, int>> itemRosterElements)
		{
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00021CF9 File Offset: 0x0001FEF9
		public virtual void OnPrisonerSold(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners)
		{
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00021CFB File Offset: 0x0001FEFB
		public virtual void OnPartyDisbanded(MobileParty disbandParty, Settlement relatedSettlement)
		{
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00021CFD File Offset: 0x0001FEFD
		public virtual void OnPartyDisbandStarted(MobileParty disbandParty)
		{
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00021CFF File Offset: 0x0001FEFF
		public virtual void OnPartyDisbandCanceled(MobileParty disbandParty)
		{
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00021D01 File Offset: 0x0001FF01
		public virtual void OnHideoutSpotted(PartyBase party, PartyBase hideoutParty)
		{
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00021D03 File Offset: 0x0001FF03
		public virtual void OnHideoutDeactivated(Settlement hideout)
		{
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00021D05 File Offset: 0x0001FF05
		public virtual void OnHideoutBattleCompleted(BattleSideEnum winnerSide, HideoutEventComponent hideoutEventComponent, HideoutEventComponent.HideoutBattleEndState battleEndState)
		{
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00021D07 File Offset: 0x0001FF07
		public virtual void OnPlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00021D09 File Offset: 0x0001FF09
		public virtual void OnItemsDiscardedByPlayer(ItemRoster roster)
		{
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00021D0B File Offset: 0x0001FF0B
		public virtual void OnPersuasionProgressCommitted(Tuple<PersuasionOptionArgs, PersuasionOptionResult> progress)
		{
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00021D0D File Offset: 0x0001FF0D
		public virtual void OnHeroSharedFoodWithAnother(Hero supporterHero, Hero supportedHero, float influence)
		{
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00021D0F File Offset: 0x0001FF0F
		public virtual void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00021D11 File Offset: 0x0001FF11
		public virtual void OnQuestStarted(QuestBase quest)
		{
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00021D13 File Offset: 0x0001FF13
		public virtual void OnItemProduced(ItemObject itemObject, Settlement settlement, int count)
		{
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00021D15 File Offset: 0x0001FF15
		public virtual void OnItemConsumed(ItemObject itemObject, Settlement settlement, int count)
		{
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00021D17 File Offset: 0x0001FF17
		public virtual void OnPartyConsumedFood(MobileParty party)
		{
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00021D19 File Offset: 0x0001FF19
		public virtual void SiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00021D1B File Offset: 0x0001FF1B
		public virtual void AfterSiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00021D1D File Offset: 0x0001FF1D
		public virtual void SiegeEngineBuilt(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine)
		{
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00021D1F File Offset: 0x0001FF1F
		public virtual void RaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00021D21 File Offset: 0x0001FF21
		public virtual void ForceSuppliesCompleted(BattleSideEnum winnerSide, ForceSuppliesEventComponent forceSuppliesEvent)
		{
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00021D23 File Offset: 0x0001FF23
		public virtual void ForceVolunteersCompleted(BattleSideEnum winnerSide, ForceVolunteersEventComponent forceVolunteersEvent)
		{
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00021D25 File Offset: 0x0001FF25
		public virtual void OnBeforeMainCharacterDied(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00021D27 File Offset: 0x0001FF27
		public virtual void OnGameOver()
		{
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00021D29 File Offset: 0x0001FF29
		public virtual void OnClanDestroyed(Clan destroyedClan)
		{
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00021D2B File Offset: 0x0001FF2B
		public virtual void OnNewIssueCreated(IssueBase issue)
		{
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00021D2D File Offset: 0x0001FF2D
		public virtual void OnIssueOwnerChanged(IssueBase issue, Hero oldOwner)
		{
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00021D2F File Offset: 0x0001FF2F
		public virtual void OnNewItemCrafted(ItemObject itemObject)
		{
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00021D31 File Offset: 0x0001FF31
		public virtual void OnWorkshopInitialized(Workshop workshop)
		{
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00021D33 File Offset: 0x0001FF33
		public virtual void OnWorkshopOwnerChanged(Workshop workshop, Hero oldOwner)
		{
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00021D35 File Offset: 0x0001FF35
		public virtual void OnWorkshopTypeChanged(Workshop workshop)
		{
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00021D37 File Offset: 0x0001FF37
		public virtual void CraftingPartUnlocked(CraftingPiece craftingPiece)
		{
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00021D39 File Offset: 0x0001FF39
		public virtual void OnNewItemCrafted(ItemObject itemObject, ItemModifier overriddenItemModifier, bool isCraftingOrderItem)
		{
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00021D3B File Offset: 0x0001FF3B
		public virtual void OnEquipmentSmeltedByHero(Hero hero, EquipmentElement equipmentElement)
		{
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00021D3D File Offset: 0x0001FF3D
		public virtual void OnBeforeSave()
		{
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00021D3F File Offset: 0x0001FF3F
		public virtual void OnMainPartyPrisonerRecruited(FlattenedTroopRoster roster)
		{
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00021D41 File Offset: 0x0001FF41
		public virtual void OnPrisonerTaken(FlattenedTroopRoster roster)
		{
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00021D43 File Offset: 0x0001FF43
		public virtual void OnPrisonerDonatedToSettlement(MobileParty donatingParty, FlattenedTroopRoster donatedPrisoners, Settlement donatedSettlement)
		{
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00021D45 File Offset: 0x0001FF45
		public virtual void CanMoveToSettlement(Hero hero, ref bool result)
		{
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00021D47 File Offset: 0x0001FF47
		public virtual void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00021D49 File Offset: 0x0001FF49
		public virtual void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00021D4B File Offset: 0x0001FF4B
		public virtual void CanPlayerMeetWithHeroAfterConversation(Hero hero, ref bool result)
		{
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00021D4D File Offset: 0x0001FF4D
		public virtual void CanHeroBecomePrisoner(Hero hero, ref bool result)
		{
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00021D4F File Offset: 0x0001FF4F
		public virtual void CanBeGovernorOrHavePartyRole(Hero hero, ref bool result)
		{
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00021D51 File Offset: 0x0001FF51
		public virtual void OnSaveOver(bool isSuccessful, string saveName)
		{
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00021D53 File Offset: 0x0001FF53
		public virtual void CollectMetadataEntries(List<KeyValuePair<string, string>> pairs)
		{
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00021D55 File Offset: 0x0001FF55
		public virtual void OnSaveStarted()
		{
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00021D57 File Offset: 0x0001FF57
		public virtual void CanHeroMarry(Hero hero, ref bool result)
		{
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00021D59 File Offset: 0x0001FF59
		public virtual void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00021D5B File Offset: 0x0001FF5B
		public virtual void OnPartyLeaderChangeOfferCanceled(MobileParty party)
		{
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00021D5D File Offset: 0x0001FF5D
		public virtual void OnPartyLeaderChanged(MobileParty mobileParty, Hero oldLeader)
		{
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00021D5F File Offset: 0x0001FF5F
		public virtual void OnClanInfluenceChanged(Clan clan, float change)
		{
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00021D61 File Offset: 0x0001FF61
		public virtual void OnPlayerPartyKnockedOrKilledTroop(CharacterObject strikedTroop)
		{
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00021D63 File Offset: 0x0001FF63
		public virtual void OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType incomeType, int incomeAmount)
		{
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00021D65 File Offset: 0x0001FF65
		public virtual void OnClanEarnedGoldFromTribute(Clan receiverClan, IFaction payingFaction)
		{
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00021D67 File Offset: 0x0001FF67
		public virtual void OnCollectLootItems(PartyBase winnerParty, ItemRoster gainedLoots)
		{
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00021D69 File Offset: 0x0001FF69
		public virtual void OnLootDistributedToParty(PartyBase winnerParty, PartyBase defeatedParty, ItemRoster lootedItems)
		{
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00021D6B File Offset: 0x0001FF6B
		public virtual void OnPlayerJoinedTournament(Town town, bool isParticipant)
		{
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00021D6D File Offset: 0x0001FF6D
		public virtual void OnConfigChanged()
		{
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00021D6F File Offset: 0x0001FF6F
		public virtual void OnMobilePartyRaftStateChanged(MobileParty mobileParty)
		{
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00021D71 File Offset: 0x0001FF71
		public virtual void OnCharacterCreationInitialized(CharacterCreationManager characterCreationManager)
		{
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00021D73 File Offset: 0x0001FF73
		public virtual void OnShipDestroyed(PartyBase owner, Ship ship, DestroyShipAction.ShipDestroyDetail detail)
		{
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00021D75 File Offset: 0x0001FF75
		public virtual void OnShipOwnerChanged(Ship ship, PartyBase oldOwner, ChangeShipOwnerAction.ShipOwnerChangeDetail shipOwnerChangeDetail)
		{
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00021D77 File Offset: 0x0001FF77
		public virtual void OnFigureheadUnlocked(Figurehead figurehead)
		{
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00021D79 File Offset: 0x0001FF79
		public virtual void OnShipRepaired(Ship ship, Settlement repairPort)
		{
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00021D7B File Offset: 0x0001FF7B
		public virtual void OnPartyLeftArmy(MobileParty party, Army army)
		{
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00021D7D File Offset: 0x0001FF7D
		public virtual void OnIncidentResolved(Incident incident)
		{
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00021D7F File Offset: 0x0001FF7F
		public virtual void OnPartyAddedToMapEvent(PartyBase partyBase)
		{
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00021D81 File Offset: 0x0001FF81
		public virtual void OnMobilePartyNavigationStateChanged(MobileParty mobileParty)
		{
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00021D83 File Offset: 0x0001FF83
		public virtual void OnMobilePartyJoinedToSiegeEvent(MobileParty mobileParty)
		{
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00021D85 File Offset: 0x0001FF85
		public virtual void OnMobilePartyLeftSiegeEvent(MobileParty mobileParty)
		{
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00021D87 File Offset: 0x0001FF87
		public virtual void OnBlockadeActivated(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00021D89 File Offset: 0x0001FF89
		public virtual void OnBlockadeDeactivated(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00021D8B File Offset: 0x0001FF8B
		public virtual void OnShipCreated(Ship ship, Settlement createdSettlement)
		{
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00021D8D File Offset: 0x0001FF8D
		public virtual void OnMercenaryServiceStarted(Clan mercenaryClan, StartMercenaryServiceAction.StartMercenaryServiceActionDetails details)
		{
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00021D8F File Offset: 0x0001FF8F
		public virtual void OnMercenaryServiceEnded(Clan mercenaryClan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails details)
		{
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00021D91 File Offset: 0x0001FF91
		public virtual void OnMapMarkerCreated(MapMarker mapMarker)
		{
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00021D93 File Offset: 0x0001FF93
		public virtual void OnMapMarkerRemoved(MapMarker mapMarker)
		{
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00021D95 File Offset: 0x0001FF95
		public virtual void OnAllianceStarted(Kingdom kingdom1, Kingdom kingdom2)
		{
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00021D97 File Offset: 0x0001FF97
		public virtual void OnAllianceEnded(Kingdom kingdom1, Kingdom kingdom2)
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00021D99 File Offset: 0x0001FF99
		public virtual void OnCallToWarAgreementStarted(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00021D9B File Offset: 0x0001FF9B
		public virtual void OnCallToWarAgreementEnded(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00021D9D File Offset: 0x0001FF9D
		public virtual void CanHeroLeadParty(Hero hero, ref bool result)
		{
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00021D9F File Offset: 0x0001FF9F
		public virtual void OnCraftingOrderCompleted(Town town, CraftingOrder craftingOrder, ItemObject craftedItem, Hero completerHero)
		{
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00021DA1 File Offset: 0x0001FFA1
		public virtual void OnItemsRefined(Hero hero, Crafting.RefiningFormula refineFormula)
		{
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00021DA3 File Offset: 0x0001FFA3
		public virtual void OnMapEventContinuityNeedsUpdate(IFaction faction)
		{
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00021DA5 File Offset: 0x0001FFA5
		public virtual void OnHeirSelectionOver(Hero selectedHeir)
		{
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00021DA7 File Offset: 0x0001FFA7
		public virtual void OnHeirSelectionRequested(Dictionary<Hero, int> heirApparents)
		{
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00021DA9 File Offset: 0x0001FFA9
		public virtual void OnMainPartyStarving()
		{
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00021DAB File Offset: 0x0001FFAB
		public virtual void OnHeroGetsBusy(Hero hero, HeroGetsBusyReasons heroGetsBusyReason)
		{
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00021DAD File Offset: 0x0001FFAD
		public virtual void CanHeroEquipmentBeChanged(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00021DAF File Offset: 0x0001FFAF
		public virtual void CanHaveCampaignIssues(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00021DB1 File Offset: 0x0001FFB1
		public virtual void IsSettlementBusy(Settlement settlement, object asker, ref int flags)
		{
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00021DB3 File Offset: 0x0001FFB3
		public virtual void OnHeroUnregistered(Hero hero)
		{
		}
	}
}
