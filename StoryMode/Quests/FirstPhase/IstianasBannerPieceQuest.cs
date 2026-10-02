using System;
using System.Collections.Generic;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.FirstPhase
{
	// Token: 0x02000035 RID: 53
	public class IstianasBannerPieceQuest : StoryModeQuestBase
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00011C91 File Offset: 0x0000FE91
		private TextObject _startQuestLog
		{
			get
			{
				return new TextObject("{=GxlPj4GC}Find the hideout that Istiana told you about and get the next banner piece.", null);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00011C9E File Offset: 0x0000FE9E
		public override TextObject Title
		{
			get
			{
				return new TextObject("{=WTjAYUoD}Find Another Piece of the Banner for Istiana", null);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00011CAB File Offset: 0x0000FEAB
		public override bool IsRemainingTimeHidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00011CB0 File Offset: 0x0000FEB0
		public IstianasBannerPieceQuest(Hero questGiver, Settlement hideout)
			: base("istiana_banner_piece_quest", questGiver, StoryModeManager.Current.MainStoryLine.FirstPhase.FirstPhaseEndTime)
		{
			this._hideout = hideout;
			this._raiderParties = new List<MobileParty>();
			this.InitializeHideout();
			base.AddTrackedObject(this._hideout);
			this.SetDialogs();
			base.InitializeQuestOnCreation();
			base.AddLog(this._startQuestLog, false);
			this._hideoutBattleEndState = IstianasBannerPieceQuest.HideoutBattleEndState.None;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00011D22 File Offset: 0x0000FF22
		protected override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00011D2A File Offset: 0x0000FF2A
		protected override void OnStartQuest()
		{
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00011D2C File Offset: 0x0000FF2C
		protected override void HourlyTick()
		{
			if (!this._hideout.Hideout.IsInfested || !this._hideout.Hideout.IsSpotted || !this._hideout.IsVisible)
			{
				this.InitializeHideout();
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00011D68 File Offset: 0x0000FF68
		protected override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.IsSettlementBusyEvent.AddNonSerializedListener(this, new ReferenceAction<Settlement, object, int>(this.IsSettlementBusy));
			CampaignEvents.OnHideoutDeactivatedEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnHideoutCleared));
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00011DD1 File Offset: 0x0000FFD1
		private void IsSettlementBusy(Settlement settlement, object asker, ref int priority)
		{
			if (asker != this && settlement == this._hideout)
			{
				priority = Math.Max(priority, 400);
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00011DF0 File Offset: 0x0000FFF0
		private void OnHideoutCleared(Settlement hideout)
		{
			if (hideout == this._hideout)
			{
				MobileParty lastAttackerParty = hideout.LastAttackerParty;
				if (lastAttackerParty != null && lastAttackerParty.IsMainParty && (this._hideoutBattleEndState == IstianasBannerPieceQuest.HideoutBattleEndState.None || PlayerEncounter.Current.ForceHideoutSendTroops))
				{
					this._hideoutBattleEndState = IstianasBannerPieceQuest.HideoutBattleEndState.Victory;
					FirstPhase.Instance.CollectBannerPiece();
					base.CompleteQuestWithSuccess();
					this._hideoutBattleEndState = IstianasBannerPieceQuest.HideoutBattleEndState.None;
				}
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00011E4C File Offset: 0x0001004C
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("hero_main_options", 100).PlayerLine(new TextObject("{=dlBFVkDj}About the task you gave me...", null), null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.conversation_lord_task_given_on_condition))
				.NpcLine(new TextObject("{=F26iH45g}What happened? Have you assembled the banner?", null), null, null, null, null)
				.Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
				.PlayerLine(new TextObject("{=rY0fdQSb}No, I am still working on it...", null), null, null, null)
				.CloseDialog(), this);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00011ED6 File Offset: 0x000100D6
		private bool conversation_lord_task_given_on_condition()
		{
			return Hero.OneToOneConversationHero == base.QuestGiver && base.IsOngoing;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00011EED File Offset: 0x000100ED
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00011EF8 File Offset: 0x000100F8
		private void InitializeHideout()
		{
			this._hideout.Hideout.IsSpotted = true;
			this._hideout.IsVisible = true;
			this._hideoutBattleEndState = IstianasBannerPieceQuest.HideoutBattleEndState.None;
			if (!this._hideout.Hideout.IsInfested)
			{
				for (int i = 0; i < 2; i++)
				{
					if (!this._hideout.Hideout.IsInfested)
					{
						this._raiderParties.Add(this.CreateRaiderParty(i));
					}
				}
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00011F6C File Offset: 0x0001016C
		private MobileParty CreateRaiderParty(int number)
		{
			Clan hideoutClan = this.GetHideoutClan(this._hideout);
			MobileParty mobileParty = BanditPartyComponent.CreateBanditParty("istiana_banner_piece_quest_raider_party_" + number, hideoutClan, this._hideout.Hideout, false, null, this._hideout.GatePosition);
			CharacterObject @object = Campaign.Current.ObjectManager.GetObject<CharacterObject>(this._hideout.Culture.StringId + "_bandit");
			mobileParty.MemberRoster.AddToCounts(@object, 5, false, 0, 0, true, -1);
			mobileParty.Party.SetCustomName(new TextObject("{=u1Pkt4HC}Raiders", null));
			mobileParty.ActualClan = hideoutClan;
			mobileParty.Position = this._hideout.Position;
			mobileParty.Party.SetVisualAsDirty();
			float num = mobileParty.Party.CalculateCurrentStrength();
			int num2 = (int)(1f * MBRandom.RandomFloat * 20f * num + 50f);
			mobileParty.InitializePartyTrade(num2);
			mobileParty.SetMoveGoToSettlement(this._hideout, MobileParty.NavigationType.Default, false);
			mobileParty.Ai.SetDoNotMakeNewDecisions(true);
			mobileParty.SetPartyUsedByQuest(true);
			EnterSettlementAction.ApplyForParty(mobileParty, this._hideout);
			return mobileParty;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00012088 File Offset: 0x00010288
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (PlayerEncounter.Current != null && mapEvent.IsPlayerMapEvent && Settlement.CurrentSettlement == this._hideout)
			{
				if (mapEvent.WinningSide == mapEvent.PlayerSide)
				{
					this._hideoutBattleEndState = IstianasBannerPieceQuest.HideoutBattleEndState.Victory;
					return;
				}
				if (mapEvent.WinningSide == BattleSideEnum.None)
				{
					this._hideoutBattleEndState = IstianasBannerPieceQuest.HideoutBattleEndState.Retreated;
					if (Hero.MainHero.IsPrisoner && this._raiderParties.Contains(Hero.MainHero.PartyBelongedToAsPrisoner.MobileParty))
					{
						EndCaptivityAction.ApplyByPeace(Hero.MainHero, null);
						if (Hero.MainHero.HitPoints < 50)
						{
							Hero.MainHero.Heal(50 - Hero.MainHero.HitPoints, false);
						}
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=FPhWhjq7}Defeated", null).ToString(), new TextObject("{=uZ4afkTU}You were defeated by the raiders in the hideout but you managed to escape. You need to wait to be able to attack again.", null).ToString(), true, false, new TextObject("{=yQtzabbe}Close", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
					}
					if (this._hideout.Parties.Count == 0)
					{
						this.InitializeHideout();
					}
					this._hideout.Hideout.SetNextPossibleAttackTime(StoryModeData.StorylineQuestHideoutHiddenDuration);
					this._hideoutBattleEndState = IstianasBannerPieceQuest.HideoutBattleEndState.None;
					return;
				}
				this._hideout.Hideout.SetNextPossibleAttackTime(StoryModeData.StorylineQuestHideoutHiddenDuration);
				this._hideoutBattleEndState = IstianasBannerPieceQuest.HideoutBattleEndState.Defeated;
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000121E0 File Offset: 0x000103E0
		private void OnGameMenuOpened(MenuCallbackArgs args)
		{
			if (this._hideoutBattleEndState != IstianasBannerPieceQuest.HideoutBattleEndState.Victory && Settlement.CurrentSettlement == this._hideout && !this._hideout.Hideout.IsInfested)
			{
				this.InitializeHideout();
			}
			if (this._hideoutBattleEndState == IstianasBannerPieceQuest.HideoutBattleEndState.Victory)
			{
				FirstPhase.Instance.CollectBannerPiece();
				base.CompleteQuestWithSuccess();
				this._hideoutBattleEndState = IstianasBannerPieceQuest.HideoutBattleEndState.None;
			}
			else if (this._hideoutBattleEndState == IstianasBannerPieceQuest.HideoutBattleEndState.Retreated || this._hideoutBattleEndState == IstianasBannerPieceQuest.HideoutBattleEndState.Defeated)
			{
				if (Hero.MainHero.IsPrisoner)
				{
					EndCaptivityAction.ApplyByPeace(Hero.MainHero, null);
				}
				if (Hero.MainHero.HitPoints < 50)
				{
					Hero.MainHero.Heal(50 - Hero.MainHero.HitPoints, false);
				}
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=FPhWhjq7}Defeated", null).ToString(), new TextObject("{=uZ4afkTU}You were defeated by the raiders in the hideout but you managed to escape. You need to wait to be able to attack again.", null).ToString(), true, false, new TextObject("{=yQtzabbe}Close", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
				if (this._hideout.Parties.Count == 0)
				{
					this.InitializeHideout();
				}
			}
			this._hideoutBattleEndState = IstianasBannerPieceQuest.HideoutBattleEndState.None;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00012300 File Offset: 0x00010500
		private Clan GetHideoutClan(Settlement hideout)
		{
			foreach (Clan clan in Clan.All)
			{
				if (clan.Culture == this._hideout.Culture && clan.IsBanditFaction && !clan.StringId.Equals("looters"))
				{
					return clan;
				}
			}
			return null;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00012380 File Offset: 0x00010580
		internal static void AutoGeneratedStaticCollectObjectsIstianasBannerPieceQuest(object o, List<object> collectedObjects)
		{
			((IstianasBannerPieceQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0001238E File Offset: 0x0001058E
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._hideout);
			collectedObjects.Add(this._raiderParties);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000123AF File Offset: 0x000105AF
		internal static object AutoGeneratedGetMemberValue_hideout(object o)
		{
			return ((IstianasBannerPieceQuest)o)._hideout;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000123BC File Offset: 0x000105BC
		internal static object AutoGeneratedGetMemberValue_raiderParties(object o)
		{
			return ((IstianasBannerPieceQuest)o)._raiderParties;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x000123C9 File Offset: 0x000105C9
		internal static object AutoGeneratedGetMemberValue_hideoutBattleEndState(object o)
		{
			return ((IstianasBannerPieceQuest)o)._hideoutBattleEndState;
		}

		// Token: 0x0400010E RID: 270
		private const int MainPartyHealHitPointLimit = 50;

		// Token: 0x0400010F RID: 271
		private const int RaiderPartySize = 10;

		// Token: 0x04000110 RID: 272
		private const int RaiderPartyCount = 2;

		// Token: 0x04000111 RID: 273
		private const string IstianaRaiderPartyStringId = "istiana_banner_piece_quest_raider_party_";

		// Token: 0x04000112 RID: 274
		[SaveableField(1)]
		private readonly Settlement _hideout;

		// Token: 0x04000113 RID: 275
		[SaveableField(2)]
		private readonly List<MobileParty> _raiderParties;

		// Token: 0x04000114 RID: 276
		[SaveableField(3)]
		private IstianasBannerPieceQuest.HideoutBattleEndState _hideoutBattleEndState;

		// Token: 0x0200007D RID: 125
		public enum HideoutBattleEndState
		{
			// Token: 0x0400025B RID: 603
			None,
			// Token: 0x0400025C RID: 604
			Retreated,
			// Token: 0x0400025D RID: 605
			Defeated,
			// Token: 0x0400025E RID: 606
			Victory
		}
	}
}
