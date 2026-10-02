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
	// Token: 0x02000031 RID: 49
	public class ArzagosBannerPieceQuest : StoryModeQuestBase
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000EE54 File Offset: 0x0000D054
		private TextObject _startQuestLog
		{
			get
			{
				return new TextObject("{=wvHvnEog}Find the hideout that Arzagos told you about and get the next banner piece.", null);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000EE61 File Offset: 0x0000D061
		public override TextObject Title
		{
			get
			{
				return new TextObject("{=ay1gPPsP}Find Another Piece of the Banner for Arzagos", null);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000EE6E File Offset: 0x0000D06E
		public override bool IsRemainingTimeHidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000EE74 File Offset: 0x0000D074
		public ArzagosBannerPieceQuest(Hero questGiver, Settlement hideout)
			: base("arzagos_banner_piece_quest", questGiver, StoryModeManager.Current.MainStoryLine.FirstPhase.FirstPhaseEndTime)
		{
			this._hideout = hideout;
			this._raiderParties = new List<MobileParty>();
			this.InitializeHideout();
			base.AddTrackedObject(this._hideout);
			this.SetDialogs();
			base.InitializeQuestOnCreation();
			base.AddLog(this._startQuestLog, false);
			this._hideoutBattleEndState = ArzagosBannerPieceQuest.HideoutBattleEndState.None;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000EEE6 File Offset: 0x0000D0E6
		protected override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000EEEE File Offset: 0x0000D0EE
		protected override void HourlyTick()
		{
			if (!this._hideout.Hideout.IsInfested || !this._hideout.Hideout.IsSpotted || !this._hideout.IsVisible)
			{
				this.InitializeHideout();
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000EF28 File Offset: 0x0000D128
		protected override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.IsSettlementBusyEvent.AddNonSerializedListener(this, new ReferenceAction<Settlement, object, int>(this.IsSettlementBusy));
			CampaignEvents.OnHideoutDeactivatedEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnHideoutCleared));
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000EF91 File Offset: 0x0000D191
		private void IsSettlementBusy(Settlement settlement, object asker, ref int priority)
		{
			if (asker != this && settlement == this._hideout)
			{
				priority = Math.Max(priority, 400);
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
		private void OnHideoutCleared(Settlement hideout)
		{
			if (hideout == this._hideout)
			{
				MobileParty lastAttackerParty = hideout.LastAttackerParty;
				if (lastAttackerParty != null && lastAttackerParty.IsMainParty && (this._hideoutBattleEndState == ArzagosBannerPieceQuest.HideoutBattleEndState.None || PlayerEncounter.Current.ForceHideoutSendTroops))
				{
					this._hideoutBattleEndState = ArzagosBannerPieceQuest.HideoutBattleEndState.Victory;
					FirstPhase.Instance.CollectBannerPiece();
					base.CompleteQuestWithSuccess();
					this._hideoutBattleEndState = ArzagosBannerPieceQuest.HideoutBattleEndState.None;
				}
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000F00C File Offset: 0x0000D20C
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("hero_main_options", 100).PlayerLine(new TextObject("{=dlBFVkDj}About the task you gave me...", null), null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.conversation_lord_task_given_on_condition))
				.NpcLine(new TextObject("{=a0JxUMgo}What happened? Did you find the piece of the banner?", null), null, null, null, null)
				.Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
				.PlayerLine(new TextObject("{=rY0fdQSb}No, I am still working on it...", null), null, null, null)
				.CloseDialog(), this);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000F096 File Offset: 0x0000D296
		private bool conversation_lord_task_given_on_condition()
		{
			return Hero.OneToOneConversationHero == base.QuestGiver && base.IsOngoing;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000F0AD File Offset: 0x0000D2AD
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000F0B8 File Offset: 0x0000D2B8
		private void InitializeHideout()
		{
			this._hideout.Hideout.IsSpotted = true;
			this._hideout.IsVisible = true;
			this._hideoutBattleEndState = ArzagosBannerPieceQuest.HideoutBattleEndState.None;
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

		// Token: 0x060002DC RID: 732 RVA: 0x0000F12C File Offset: 0x0000D32C
		private MobileParty CreateRaiderParty(int number)
		{
			Clan hideoutClan = this.GetHideoutClan(this._hideout);
			MobileParty mobileParty = BanditPartyComponent.CreateBanditParty("arzagos_banner_piece_quest_raider_party_" + number, hideoutClan, this._hideout.Hideout, false, null, this._hideout.GatePosition);
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

		// Token: 0x060002DD RID: 733 RVA: 0x0000F248 File Offset: 0x0000D448
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (PlayerEncounter.Current != null && mapEvent.IsPlayerMapEvent && Settlement.CurrentSettlement == this._hideout)
			{
				if (mapEvent.WinningSide == mapEvent.PlayerSide)
				{
					this._hideoutBattleEndState = ArzagosBannerPieceQuest.HideoutBattleEndState.Victory;
					return;
				}
				if (mapEvent.WinningSide == BattleSideEnum.None)
				{
					this._hideoutBattleEndState = ArzagosBannerPieceQuest.HideoutBattleEndState.Retreated;
					if (Hero.MainHero.IsPrisoner && this._raiderParties.Contains(Hero.MainHero.PartyBelongedToAsPrisoner.MobileParty))
					{
						EndCaptivityAction.ApplyByPeace(Hero.MainHero, null);
						if (Hero.MainHero.HitPoints < 50)
						{
							Hero.MainHero.Heal(50 - Hero.MainHero.HitPoints, false);
						}
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=FPhWhjq7}Defeated", null).ToString(), new TextObject("{=6iytd81P}You are defeated by the bandits in the hideout but you managed to escape. You need to wait a while before attacking again.", null).ToString(), true, false, new TextObject("{=yQtzabbe}Close", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
					}
					if (this._hideout.Parties.Count == 0)
					{
						this.InitializeHideout();
					}
					this._hideout.Hideout.SetNextPossibleAttackTime(StoryModeData.StorylineQuestHideoutHiddenDuration);
					this._hideoutBattleEndState = ArzagosBannerPieceQuest.HideoutBattleEndState.None;
					return;
				}
				this._hideout.Hideout.SetNextPossibleAttackTime(StoryModeData.StorylineQuestHideoutHiddenDuration);
				this._hideoutBattleEndState = ArzagosBannerPieceQuest.HideoutBattleEndState.Defeated;
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F3A0 File Offset: 0x0000D5A0
		private void OnGameMenuOpened(MenuCallbackArgs args)
		{
			if (this._hideoutBattleEndState != ArzagosBannerPieceQuest.HideoutBattleEndState.Victory && Settlement.CurrentSettlement == this._hideout && !this._hideout.Hideout.IsInfested)
			{
				this.InitializeHideout();
			}
			if (this._hideoutBattleEndState == ArzagosBannerPieceQuest.HideoutBattleEndState.Victory)
			{
				FirstPhase.Instance.CollectBannerPiece();
				base.CompleteQuestWithSuccess();
				this._hideoutBattleEndState = ArzagosBannerPieceQuest.HideoutBattleEndState.None;
				return;
			}
			if (this._hideoutBattleEndState == ArzagosBannerPieceQuest.HideoutBattleEndState.Retreated || this._hideoutBattleEndState == ArzagosBannerPieceQuest.HideoutBattleEndState.Defeated)
			{
				if (Hero.MainHero.IsPrisoner)
				{
					EndCaptivityAction.ApplyByPeace(Hero.MainHero, null);
					if (Hero.MainHero.HitPoints < 50)
					{
						Hero.MainHero.Heal(50 - Hero.MainHero.HitPoints, false);
					}
					InformationManager.ShowInquiry(new InquiryData(new TextObject("{=FPhWhjq7}Defeated", null).ToString(), new TextObject("{=btAV7mmq}You are defeated by the raiders in the hideout but you managed to escape. You need to wait a while before attacking again.", null).ToString(), true, false, new TextObject("{=yQtzabbe}Close", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
					if (this._hideout.Parties.Count == 0)
					{
						this.InitializeHideout();
					}
				}
				this._hideoutBattleEndState = ArzagosBannerPieceQuest.HideoutBattleEndState.None;
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000F4C0 File Offset: 0x0000D6C0
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

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F540 File Offset: 0x0000D740
		internal static void AutoGeneratedStaticCollectObjectsArzagosBannerPieceQuest(object o, List<object> collectedObjects)
		{
			((ArzagosBannerPieceQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F54E File Offset: 0x0000D74E
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._hideout);
			collectedObjects.Add(this._raiderParties);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F56F File Offset: 0x0000D76F
		internal static object AutoGeneratedGetMemberValue_hideout(object o)
		{
			return ((ArzagosBannerPieceQuest)o)._hideout;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F57C File Offset: 0x0000D77C
		internal static object AutoGeneratedGetMemberValue_raiderParties(object o)
		{
			return ((ArzagosBannerPieceQuest)o)._raiderParties;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F589 File Offset: 0x0000D789
		internal static object AutoGeneratedGetMemberValue_hideoutBattleEndState(object o)
		{
			return ((ArzagosBannerPieceQuest)o)._hideoutBattleEndState;
		}

		// Token: 0x040000E2 RID: 226
		private const int MainPartyHealHitPointLimit = 50;

		// Token: 0x040000E3 RID: 227
		private const int RaiderPartySize = 10;

		// Token: 0x040000E4 RID: 228
		private const int RaiderPartyCount = 2;

		// Token: 0x040000E5 RID: 229
		private const string ArzagosRaiderPartyStringId = "arzagos_banner_piece_quest_raider_party_";

		// Token: 0x040000E6 RID: 230
		[SaveableField(1)]
		private readonly Settlement _hideout;

		// Token: 0x040000E7 RID: 231
		[SaveableField(2)]
		private readonly List<MobileParty> _raiderParties;

		// Token: 0x040000E8 RID: 232
		[SaveableField(3)]
		private ArzagosBannerPieceQuest.HideoutBattleEndState _hideoutBattleEndState;

		// Token: 0x02000078 RID: 120
		public enum HideoutBattleEndState
		{
			// Token: 0x04000245 RID: 581
			None,
			// Token: 0x04000246 RID: 582
			Retreated,
			// Token: 0x04000247 RID: 583
			Defeated,
			// Token: 0x04000248 RID: 584
			Victory
		}
	}
}
