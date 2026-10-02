using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.Missions.MissionLogics.Hideout;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.SecondPhase.ConspiracyQuests
{
	// Token: 0x0200002A RID: 42
	public class ConspiracyBaseOfOperationsDiscoveredConspiracyQuest : ConspiracyQuestBase
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000BA15 File Offset: 0x00009C15
		public override TextObject Title
		{
			get
			{
				return new TextObject("{=3Pq58i2u}Conspiracy Base of Operations Discovered", null);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000BA24 File Offset: 0x00009C24
		public override TextObject SideNotificationText
		{
			get
			{
				TextObject textObject = new TextObject("{=aY4zWYpg}You have have received an important message from {MENTOR.LINK}.", null);
				StringHelpers.SetCharacterProperties("MENTOR", base.Mentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000BA58 File Offset: 0x00009C58
		public override TextObject StartMessageLogFromMentor
		{
			get
			{
				TextObject textObject = new TextObject("{=XQrmVPKL}{PLAYER.LINK} I hope this letter finds you well. I have learned from a spy in {LOCATION_LINK} that our adversaries have set up a camp in its environs. She could not tell me what they plan to do, but if you raided the camp, stole some of their supplies, and brought it back to me, we could get some idea of their wicked intentions. Search around {LOCATION_LINK} to find the hideout.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				textObject.SetTextVariable("LOCATION_LINK", this._baseLocation.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000BA9C File Offset: 0x00009C9C
		public override TextObject StartLog
		{
			get
			{
				TextObject textObject = new TextObject("{=rTYNL1LB}{MENTOR.LINK} told you about a group of conspirators operating in a hideout in the vicinity of {LOCATION_LINK}. You should go there and raid the hideout with a small group of fighters and take the bandits by surprise.", null);
				StringHelpers.SetCharacterProperties("MENTOR", base.Mentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("LOCATION_LINK", this._baseLocation.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000BAE5 File Offset: 0x00009CE5
		public override float ConspiracyStrengthDecreaseAmount
		{
			get
			{
				return this._conspiracyStrengthDecreaseAmount;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000BAF0 File Offset: 0x00009CF0
		private TextObject HideoutBossName
		{
			get
			{
				MobileParty mobileParty = this._hideout.Parties.FirstOrDefault<MobileParty>((MobileParty p) => p.IsBanditBossParty);
				if (mobileParty != null && mobileParty.MemberRoster.TotalManCount > 0)
				{
					return mobileParty.MemberRoster.GetCharacterAtIndex(0).Name;
				}
				return new TextObject("{=izCbZEZg}Conspiracy Commander{%Commander is male.}", null);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000BB5B File Offset: 0x00009D5B
		private TextObject HideoutSpottedLog
		{
			get
			{
				return new TextObject("{=nrdl5QaF}My spy spotted some conspirators at the camp, and some local bandits have joined them. My spy does not know if they are expecting an attack, so I implore you to be cautious and to be ready for anything. Needless to say, I'm sure you will send any documents you can find to me so I can study them. Go quickly and return safely.", null);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000BB68 File Offset: 0x00009D68
		private TextObject HideoutRemovedLog
		{
			get
			{
				return new TextObject("{=cLZWjrZP}They have moved to another hiding place.", null);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000BB78 File Offset: 0x00009D78
		private TextObject NotDueledWithHideoutBossAndDefeatLog
		{
			get
			{
				TextObject textObject = new TextObject("{=nOLFHL3x}You and your men have defeated {BOSS_NAME} and the rest of the conspirators as {MENTOR.LINK} asked you to do.", null);
				StringHelpers.SetCharacterProperties("MENTOR", base.Mentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("BOSS_NAME", this.HideoutBossName);
				return textObject;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000BBBC File Offset: 0x00009DBC
		private TextObject NotDueledWithHideoutBossAndDefeatedLog
		{
			get
			{
				TextObject textObject = new TextObject("{=EV5ykPuT}You and your men were defeated by {BOSS_NAME} and his conspirators. Rest of your men finds your broken body among the bloodied pile of corpses. Yet you live to fight another day.", null);
				textObject.SetTextVariable("BOSS_NAME", this.HideoutBossName);
				return textObject;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000BBDB File Offset: 0x00009DDB
		private TextObject DueledWithHideoutBossAndDefeatLog
		{
			get
			{
				TextObject textObject = new TextObject("{=LKiREaFZ}You have defeated {BOSS_NAME} in a fair duel his men the conspirators scatters and runs away in shame.", null);
				textObject.SetTextVariable("BOSS_NAME", this.HideoutBossName);
				return textObject;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000BBFA File Offset: 0x00009DFA
		private TextObject DueledWithHideoutBossAndDefeatedLog
		{
			get
			{
				TextObject textObject = new TextObject("{=Uk7F483P}You were defeated by the {BOSS_NAME} in the duel. Your men takes your wounded body to the safety. As agreed, conspirators quickly leave and disappear without a trace.", null);
				textObject.SetTextVariable("BOSS_NAME", this.HideoutBossName);
				return textObject;
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000BC1C File Offset: 0x00009E1C
		public ConspiracyBaseOfOperationsDiscoveredConspiracyQuest(string questId, Hero questGiver)
			: base(questId, questGiver)
		{
			this._raiderParties = new List<MobileParty>();
			this._hideout = this.SelectHideout();
			if (this._hideout.Hideout.IsSpotted)
			{
				base.AddLog(this.HideoutSpottedLog, false);
				base.AddTrackedObject(this._hideout);
			}
			this._baseLocation = SettlementHelper.FindNearestSettlementToSettlement(this._hideout, MobileParty.NavigationType.Default, (Settlement p) => p.IsFortification);
			this._conspiracyStrengthDecreaseAmount = 50f;
			this.InitializeHideout();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000BCB8 File Offset: 0x00009EB8
		private Settlement SelectHideout()
		{
			Settlement centralSettlement = StoryModeHeroes.ImperialMentor.HomeSettlement;
			Settlement settlement = SettlementHelper.FindRandomHideout(delegate(Settlement s)
			{
				if (s.Hideout.IsInfested)
				{
					MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
					CampaignVec2 gatePosition = s.GatePosition;
					CampaignVec2 gatePosition2 = centralSettlement.GatePosition;
					if (mapDistanceModel.PathExistBetweenPoints(in gatePosition, in gatePosition2, MobileParty.NavigationType.Default))
					{
						if (!StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine)
						{
							return !StoryModeData.IsKingdomImperial(SettlementHelper.FindNearestFortificationToSettlement(s, MobileParty.NavigationType.Default, null).OwnerClan.Kingdom);
						}
						return StoryModeData.IsKingdomImperial(SettlementHelper.FindNearestFortificationToSettlement(s, MobileParty.NavigationType.Default, null).OwnerClan.Kingdom);
					}
				}
				return false;
			});
			if (settlement == null)
			{
				settlement = SettlementHelper.FindRandomHideout(delegate(Settlement s)
				{
					MapDistanceModel mapDistanceModel2 = Campaign.Current.Models.MapDistanceModel;
					CampaignVec2 gatePosition3 = s.GatePosition;
					CampaignVec2 gatePosition4 = centralSettlement.GatePosition;
					if (!mapDistanceModel2.PathExistBetweenPoints(in gatePosition3, in gatePosition4, MobileParty.NavigationType.Default))
					{
						return false;
					}
					if (!StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine)
					{
						return !StoryModeData.IsKingdomImperial(SettlementHelper.FindNearestFortificationToSettlement(s, MobileParty.NavigationType.Default, null).OwnerClan.Kingdom);
					}
					return StoryModeData.IsKingdomImperial(SettlementHelper.FindNearestFortificationToSettlement(s, MobileParty.NavigationType.Default, null).OwnerClan.Kingdom);
				});
				if (settlement == null)
				{
					settlement = SettlementHelper.FindRandomHideout(delegate(Settlement s)
					{
						MapDistanceModel mapDistanceModel3 = Campaign.Current.Models.MapDistanceModel;
						CampaignVec2 gatePosition5 = s.GatePosition;
						CampaignVec2 gatePosition6 = centralSettlement.GatePosition;
						return mapDistanceModel3.PathExistBetweenPoints(in gatePosition5, in gatePosition6, MobileParty.NavigationType.Default) && s.Hideout.IsInfested;
					});
					if (settlement == null)
					{
						settlement = SettlementHelper.FindRandomHideout(delegate(Settlement s)
						{
							MapDistanceModel mapDistanceModel4 = Campaign.Current.Models.MapDistanceModel;
							CampaignVec2 gatePosition7 = s.GatePosition;
							CampaignVec2 gatePosition8 = centralSettlement.GatePosition;
							return mapDistanceModel4.PathExistBetweenPoints(in gatePosition7, in gatePosition8, MobileParty.NavigationType.Default);
						});
					}
				}
			}
			if (!settlement.Hideout.IsInfested)
			{
				for (int i = 0; i < 2; i++)
				{
					if (!settlement.Hideout.IsInfested)
					{
						this._raiderParties.Add(this.CreateRaiderParty(settlement, false, i));
					}
				}
			}
			return settlement;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000BD68 File Offset: 0x00009F68
		private MobileParty CreateRaiderParty(Settlement hideout, bool isBanditBossParty, int partyIndex)
		{
			MobileParty mobileParty = BanditPartyComponent.CreateBanditParty("conspiracy_discovered_quest_raider_party_" + partyIndex, hideout.OwnerClan, hideout.Hideout, isBanditBossParty, null, hideout.GatePosition);
			CharacterObject @object = Campaign.Current.ObjectManager.GetObject<CharacterObject>(hideout.Culture.StringId + "_bandit");
			mobileParty.MemberRoster.AddToCounts(@object, 6, false, 0, 0, true, -1);
			mobileParty.Party.SetCustomName(new TextObject("{=u1Pkt4HC}Raiders", null));
			mobileParty.ActualClan = hideout.OwnerClan;
			mobileParty.Position = hideout.Position;
			mobileParty.Party.SetVisualAsDirty();
			EnterSettlementAction.ApplyForParty(mobileParty, hideout);
			float num = mobileParty.Party.CalculateCurrentStrength();
			int num2 = (int)(1f * MBRandom.RandomFloat * 20f * num + 50f);
			mobileParty.InitializePartyTrade(num2);
			mobileParty.SetMoveGoToSettlement(hideout, MobileParty.NavigationType.Default, false);
			EnterSettlementAction.ApplyForParty(mobileParty, hideout);
			mobileParty.SetPartyUsedByQuest(true);
			return mobileParty;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000BE5B File Offset: 0x0000A05B
		protected override void InitializeQuestOnGameLoad()
		{
			this._conspiracyStrengthDecreaseAmount = 50f;
			this._baseLocation = SettlementHelper.FindNearestFortificationToSettlement(this._hideout, MobileParty.NavigationType.Default, null);
			this.SetDialogs();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000BE81 File Offset: 0x0000A081
		protected override void HourlyTick()
		{
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000BE83 File Offset: 0x0000A083
		private void InitializeHideout()
		{
			base.AddTrackedObject(this._baseLocation);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000BE94 File Offset: 0x0000A094
		private void ChangeHideoutParties()
		{
			PartyTemplateObject partyTemplateObject = (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? Campaign.Current.ObjectManager.GetObject<PartyTemplateObject>("conspiracy_anti_imperial_special_raider_party_template") : Campaign.Current.ObjectManager.GetObject<PartyTemplateObject>("conspiracy_imperial_special_raider_party_template"));
			foreach (MobileParty mobileParty in this._hideout.Parties)
			{
				if (mobileParty.IsBandit)
				{
					mobileParty.Party.SetCustomName(new TextObject("{=FRSas4xT}Conspiracy Troops", null));
					mobileParty.SetPartyUsedByQuest(true);
					if (mobileParty.IsBanditBossParty)
					{
						int num = mobileParty.MemberRoster.TotalManCount - 1;
						mobileParty.MemberRoster.Clear();
						base.DistributeConspiracyRaiderTroopsByLevel(partyTemplateObject, mobileParty.Party, num);
						CharacterObject characterObject = (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? Campaign.Current.ObjectManager.GetObject<CharacterObject>("anti_imperial_conspiracy_boss") : Campaign.Current.ObjectManager.GetObject<CharacterObject>("imperial_conspiracy_boss"));
						characterObject.SetTransferableInPartyScreen(false);
						mobileParty.MemberRoster.AddToCounts(characterObject, 1, true, 0, 0, true, -1);
					}
					else
					{
						int totalManCount = mobileParty.MemberRoster.TotalManCount;
						mobileParty.MemberRoster.Clear();
						base.DistributeConspiracyRaiderTroopsByLevel(partyTemplateObject, mobileParty.Party, totalManCount);
					}
				}
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000C004 File Offset: 0x0000A204
		protected override void RegisterEvents()
		{
			base.RegisterEvents();
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.OnMissionStartedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionStarted));
			CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionEnded));
			CampaignEvents.OnHideoutSpottedEvent.AddNonSerializedListener(this, new Action<PartyBase, PartyBase>(this.OnHideoutSpotted));
			CampaignEvents.OnHideoutDeactivatedEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnHideoutCleared));
			CampaignEvents.OnHideoutBattleCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, HideoutEventComponent, HideoutEventComponent.HideoutBattleEndState>(this.OnHideoutBattleCompleted));
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000C0A1 File Offset: 0x0000A2A1
		private void OnHideoutBattleCompleted(BattleSideEnum winnerSide, HideoutEventComponent hideoutEventComponent, HideoutEventComponent.HideoutBattleEndState battleEndState)
		{
			this._isSuccess = hideoutEventComponent.MapEvent.InvolvedParties.Contains(PartyBase.MainParty) && winnerSide == hideoutEventComponent.MapEvent.PlayerSide;
			this.HandleHideoutBattleEnd();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		private void OnGameMenuOpened(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement == this._hideout && base.IsOngoing)
			{
				MobileParty mobileParty = this._hideout.Parties.FirstOrDefault<MobileParty>((MobileParty p) => p.IsBanditBossParty);
				if (mobileParty != null && mobileParty.IsActive)
				{
					if (mobileParty.MemberRoster.TotalManCount <= 0)
					{
						this.ChangeHideoutParties();
						return;
					}
					string text = (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? "anti_imperial_conspiracy_boss" : "imperial_conspiracy_boss");
					bool flag = false;
					using (List<TroopRosterElement>.Enumerator enumerator = mobileParty.MemberRoster.GetTroopRoster().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.Character.StringId == text)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						this.ChangeHideoutParties();
					}
				}
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000C1D8 File Offset: 0x0000A3D8
		private void HandleHideoutBattleEnd()
		{
			if (base.IsOngoing)
			{
				if (Hero.MainHero.IsPrisoner)
				{
					EndCaptivityAction.ApplyByPeace(Hero.MainHero, null);
				}
				foreach (MobileParty mobileParty in this._hideout.Parties.ToList<MobileParty>())
				{
					if (mobileParty.IsBandit)
					{
						DestroyPartyAction.Apply(null, mobileParty);
					}
				}
				if (this._isSuccess)
				{
					base.CompleteQuestWithSuccess();
					return;
				}
				base.AddLog(this.HideoutRemovedLog, false);
				base.CompleteQuestWithFail(null);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000C284 File Offset: 0x0000A484
		private void OnMissionStarted(IMission mission)
		{
			if (Settlement.CurrentSettlement == this._hideout && PlayerEncounter.Current != null)
			{
				CharacterObject characterObject = (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? Campaign.Current.ObjectManager.GetObject<CharacterObject>("anti_imperial_conspiracy_boss") : Campaign.Current.ObjectManager.GetObject<CharacterObject>("imperial_conspiracy_boss"));
				Mission mission2 = (Mission)mission;
				HideoutAmbushMissionController missionBehavior = mission2.GetMissionBehavior<HideoutAmbushMissionController>();
				if (missionBehavior != null)
				{
					missionBehavior.SetOverriddenHideoutBossCharacterObject(characterObject);
					return;
				}
				HideoutMissionController missionBehavior2 = mission2.GetMissionBehavior<HideoutMissionController>();
				if (missionBehavior2 != null)
				{
					missionBehavior2.SetOverriddenHideoutBossCharacterObject(characterObject);
					return;
				}
				Debug.FailedAssert("Hideout boss can not be set!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Quests\\SecondPhase\\ConspiracyQuests\\ConspiracyBaseOfOperationsDiscoveredConspiracyQuest.cs", "OnMissionStarted", 415);
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000C32C File Offset: 0x0000A52C
		private void OnMissionEnded(IMission mission)
		{
			if (Settlement.CurrentSettlement == this._hideout && PlayerEncounter.Current != null)
			{
				MapEvent playerMapEvent = MapEvent.PlayerMapEvent;
				if (playerMapEvent != null)
				{
					if (playerMapEvent.WinningSide == playerMapEvent.PlayerSide)
					{
						if (this._dueledWithHideoutBoss)
						{
							this.DueledWithHideoutBossAndDefeatedCaravan();
						}
						else
						{
							this.NotDueledWithHideoutBossAndDefeatedCaravan();
						}
						this._isSuccess = true;
					}
					else
					{
						if (playerMapEvent.WinningSide != BattleSideEnum.None)
						{
							if (this._dueledWithHideoutBoss)
							{
								this.DueledWithHideoutBossAndDefeatedByCaravan();
							}
							else
							{
								this.NotDueledWithHideoutBossAndDefeatedByCaravan();
							}
						}
						this._isSuccess = false;
					}
					this.HandleHideoutBattleEnd();
				}
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000C3AF File Offset: 0x0000A5AF
		private void OnHideoutSpotted(PartyBase party, PartyBase hideoutParty)
		{
			if (party == PartyBase.MainParty && hideoutParty.Settlement == this._hideout)
			{
				base.AddLog(this.HideoutSpottedLog, false);
				base.AddTrackedObject(this._hideout);
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000C3E1 File Offset: 0x0000A5E1
		private void OnHideoutCleared(Settlement hideout)
		{
			if (hideout == this._hideout)
			{
				MobileParty lastAttackerParty = hideout.LastAttackerParty;
				if (lastAttackerParty != null && lastAttackerParty.IsMainParty)
				{
					this.NotDueledWithHideoutBossAndDefeatedCaravan();
					this._isSuccess = true;
					this.HandleHideoutBattleEnd();
				}
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000C413 File Offset: 0x0000A613
		private void NotDueledWithHideoutBossAndDefeatedCaravan()
		{
			base.AddLog(this.NotDueledWithHideoutBossAndDefeatLog, false);
			this._conspiracyStrengthDecreaseAmount = 50f;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000C42E File Offset: 0x0000A62E
		private void NotDueledWithHideoutBossAndDefeatedByCaravan()
		{
			base.AddLog(this.NotDueledWithHideoutBossAndDefeatedLog, false);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000C43E File Offset: 0x0000A63E
		private void DueledWithHideoutBossAndDefeatedCaravan()
		{
			base.AddLog(this.DueledWithHideoutBossAndDefeatLog, false);
			this._conspiracyStrengthDecreaseAmount = 75f;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000C459 File Offset: 0x0000A659
		private void DueledWithHideoutBossAndDefeatedByCaravan()
		{
			base.AddLog(this.DueledWithHideoutBossAndDefeatedLog, false);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000C46C File Offset: 0x0000A66C
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000015).NpcLine(new TextObject("{=UdHL9YZC}Well well, isn't this the famous {PLAYER.LINK}! You have been a thorn at our side for a while now. It's good that you are here now. It spares us from searching for you.[if:convo_confused_annoyed][ib:hip]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.bandit_hideout_boss_fight_start_on_condition))
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=bZI82WMt}Let's get this over with! Men Attack!", null), null, null, null)
				.ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.bandit_hideout_continue_battle_on_clickable_condition))
				.NpcLine(new TextObject("{=H2FMIJmw}My wolves! Kill them![ib:aggressive][if:convo_furious]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.bandit_hideout_continue_battle_on_consequence))
				.CloseDialog()
				.PlayerOption(new TextObject("{=5PGokzW1}Talk is cheap. If you really want me that bad, I challenge you to a duel.", null), null, null, null)
				.NpcLine(new TextObject("{=karjORwI}To hell with that! Why would I want to duel with you?", null), null, null, null, null)
				.PlayerLine(new TextObject("{=MU2O1SaZ}There is an army waiting for you outside.", null), null, null, null)
				.PlayerLine(new TextObject("{=tF6VeYaA}If you win, I promise my army won't crush you.", null), null, null, null)
				.PlayerLine(new TextObject("{=fUcwKbW8}If I win I will just kill you and let these poor excuses you call conspirators run away.", null), null, null, null)
				.NpcLine(new TextObject("{=C0xbbPqE}I will duel you for your insolence! Die dog![ib:warrior][if:convo_furious]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.bandit_hideout_start_duel_fight_on_consequence))
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog(), this);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000C5A8 File Offset: 0x0000A7A8
		private bool bandit_hideout_boss_fight_start_on_condition()
		{
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			return encounteredParty != null && !encounteredParty.IsMobile && encounteredParty.MapFaction != null && encounteredParty.MapFaction.IsBanditFaction && (encounteredParty.IsSettlement && encounteredParty.Settlement.IsHideout && encounteredParty.Settlement == this._hideout && Mission.Current != null && CharacterObject.OneToOneConversationCharacter != null && CharacterObject.OneToOneConversationCharacter.StringId == (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? "anti_imperial_conspiracy_boss" : "imperial_conspiracy_boss")) && (Mission.Current.GetMissionBehavior<HideoutAmbushMissionController>() != null || Mission.Current.GetMissionBehavior<HideoutMissionController>() != null);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000C66C File Offset: 0x0000A86C
		private void bandit_hideout_start_duel_fight_on_consequence()
		{
			this._dueledWithHideoutBoss = true;
			if (Mission.Current.GetMissionBehavior<HideoutAmbushMissionController>() != null)
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutAmbushMissionController.StartBossFightDuelMode;
				return;
			}
			Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutMissionController.StartBossFightDuelMode;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C6C4 File Offset: 0x0000A8C4
		private bool bandit_hideout_continue_battle_on_clickable_condition(out TextObject explanation)
		{
			bool flag = false;
			foreach (Agent agent in Mission.Current.PlayerTeam.ActiveAgents)
			{
				if (!agent.IsMount && agent.Character != CharacterObject.PlayerCharacter)
				{
					flag = true;
					break;
				}
			}
			explanation = TextObject.GetEmpty();
			if (!flag)
			{
				explanation = new TextObject("{=F9HxO1iS}You don't have any men.", null);
			}
			return flag;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000C74C File Offset: 0x0000A94C
		private void bandit_hideout_continue_battle_on_consequence()
		{
			this._dueledWithHideoutBoss = false;
			if (Mission.Current.GetMissionBehavior<HideoutAmbushMissionController>() != null)
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutAmbushMissionController.StartBossFightBattleMode;
				return;
			}
			Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutMissionController.StartBossFightBattleMode;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000C7A3 File Offset: 0x0000A9A3
		protected override void OnStartQuest()
		{
			base.OnStartQuest();
			this.SetDialogs();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000C7B4 File Offset: 0x0000A9B4
		protected override void OnCompleteWithSuccess()
		{
			base.OnCompleteWithSuccess();
			base.AddLog(new TextObject("{=6Dd3Pa07}You managed to thwart the conspiracy.", null), false);
			foreach (MobileParty mobileParty in this._raiderParties)
			{
				if (mobileParty.IsActive)
				{
					DestroyPartyAction.Apply(null, mobileParty);
				}
			}
			this._raiderParties.Clear();
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000C834 File Offset: 0x0000AA34
		protected override void OnTimedOut()
		{
			base.OnTimedOut();
			base.AddLog(new TextObject("{=S5Dn2K3m}You couldn't stop the conspiracy.", null), false);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000C84F File Offset: 0x0000AA4F
		internal static void AutoGeneratedStaticCollectObjectsConspiracyBaseOfOperationsDiscoveredConspiracyQuest(object o, List<object> collectedObjects)
		{
			((ConspiracyBaseOfOperationsDiscoveredConspiracyQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000C85D File Offset: 0x0000AA5D
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._hideout);
			collectedObjects.Add(this._raiderParties);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000C87E File Offset: 0x0000AA7E
		internal static object AutoGeneratedGetMemberValue_hideout(object o)
		{
			return ((ConspiracyBaseOfOperationsDiscoveredConspiracyQuest)o)._hideout;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000C88B File Offset: 0x0000AA8B
		internal static object AutoGeneratedGetMemberValue_raiderParties(object o)
		{
			return ((ConspiracyBaseOfOperationsDiscoveredConspiracyQuest)o)._raiderParties;
		}

		// Token: 0x040000B0 RID: 176
		private const string AntiImperialHideoutBossStringId = "anti_imperial_conspiracy_boss";

		// Token: 0x040000B1 RID: 177
		private const string ImperialHideoutBossStringId = "imperial_conspiracy_boss";

		// Token: 0x040000B2 RID: 178
		private const int RaiderPartySize = 6;

		// Token: 0x040000B3 RID: 179
		private const int RaiderPartyCount = 2;

		// Token: 0x040000B4 RID: 180
		private const float DefaultConspiracyReductionAmount = 50f;

		// Token: 0x040000B5 RID: 181
		[SaveableField(1)]
		private readonly Settlement _hideout;

		// Token: 0x040000B6 RID: 182
		private Settlement _baseLocation;

		// Token: 0x040000B7 RID: 183
		private bool _dueledWithHideoutBoss;

		// Token: 0x040000B8 RID: 184
		private bool _isSuccess;

		// Token: 0x040000B9 RID: 185
		private float _conspiracyStrengthDecreaseAmount;

		// Token: 0x040000BA RID: 186
		[SaveableField(2)]
		private readonly List<MobileParty> _raiderParties;
	}
}
