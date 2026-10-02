using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.FirstPhase
{
	// Token: 0x02000034 RID: 52
	public class CreateKingdomQuest : StoryModeQuestBase
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00011464 File Offset: 0x0000F664
		private TextObject _onQuestStartedImperialLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=N5Qg5ick}You told {MENTOR.LINK} that you will create your own imperial faction. You can do that by speaking to one of your governors once you fulfill the requirements. {?MENTOR.GENDER}She{?}He{\\?} expects to talk to you once you succeed.", null);
				StringHelpers.SetCharacterProperties("MENTOR", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00011498 File Offset: 0x0000F698
		private TextObject _onQuestStartedAntiImperialLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=AxKDQJ4G}You told {MENTOR.LINK} that you will create your own kingdom to defeat the Empire. You can do that by speaking to one of your governors once you fulfill the requirements. {?MENTOR.GENDER}She{?}He{\\?} expects to talk to you once you succeed.", null);
				StringHelpers.SetCharacterProperties("MENTOR", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600032B RID: 811 RVA: 0x000114CC File Offset: 0x0000F6CC
		private TextObject _imperialKingdomCreatedLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=UnjgFmnE}Heeding the advice of {MENTOR.LINK}, you have created an imperial faction. You can tell {?MENTOR.GENDER}her{?}him{\\?} that you will support your own kingdom.", null);
				StringHelpers.SetCharacterProperties("MENTOR", StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00011500 File Offset: 0x0000F700
		private TextObject _antiImperialKingdomCreatedLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=BekWpXmR}Heeding the advice of {MENTOR.LINK}, you have created a kingdom to oppose the Empire. You can tell {?MENTOR.GENDER}her{?}him{\\?} that you will support your own kingdom.", null);
				StringHelpers.SetCharacterProperties("MENTOR", StoryModeHeroes.AntiImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00011531 File Offset: 0x0000F731
		private TextObject _leftKingdomAfterCreatingLogText
		{
			get
			{
				return new TextObject("{=nNavD2NO}You left the kingdom you have created. You can only support kingdoms that you are a part of.", null);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0001153E File Offset: 0x0000F73E
		private TextObject _clanTierRequirementLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=QxeKZ3nE}Reach Clan Tier {CLAN_TIER}", null);
				textObject.SetTextVariable("CLAN_TIER", Campaign.Current.Models.KingdomCreationModel.MinimumClanTierToCreateKingdom);
				return textObject;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0001156B File Offset: 0x0000F76B
		private TextObject _partySizeRequirementLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=NzQq2qp1}Gather {PARTY_SIZE} Troops", null);
				textObject.SetTextVariable("PARTY_SIZE", 100);
				return textObject;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00011586 File Offset: 0x0000F786
		private TextObject _settlementOwnershipRequirementLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=Bo66bfTh}Own {?IS_IMPERIAL}an Imperial Settlement{?}a Settlement{\\?} ", null);
				textObject.SetTextVariable("IS_IMPERIAL", this._isImperial ? 1 : 0);
				return textObject;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000331 RID: 817 RVA: 0x000115AB File Offset: 0x0000F7AB
		private TextObject _clanIndependenceRequirementLogText
		{
			get
			{
				return new TextObject("{=a0ZKBj6P}Be an independent clan", null);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000332 RID: 818 RVA: 0x000115B8 File Offset: 0x0000F7B8
		private TextObject _questFailedLogText
		{
			get
			{
				return new TextObject("{=tVlZTOst}You have chosen a different path.", null);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000333 RID: 819 RVA: 0x000115C5 File Offset: 0x0000F7C5
		public override TextObject Title
		{
			get
			{
				TextObject textObject = new TextObject("{=HhFHRs7N}Create {?IS_IMPERIAL}an Imperial Faction{?}a Non-Imperial Kingdom{\\?}", null);
				textObject.SetTextVariable("IS_IMPERIAL", this._isImperial ? 1 : 0);
				return textObject;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000334 RID: 820 RVA: 0x000115EA File Offset: 0x0000F7EA
		public override bool IsRemainingTimeHidden
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000115F0 File Offset: 0x0000F7F0
		public CreateKingdomQuest(Hero questGiver)
			: base("main_storyline_create_kingdom_quest_" + ((StoryModeHeroes.ImperialMentor == questGiver) ? "1" : "0"), questGiver, StoryModeManager.Current.MainStoryLine.FirstPhase.FirstPhaseEndTime)
		{
			this._isImperial = StoryModeHeroes.ImperialMentor == questGiver;
			this.SetDialogs();
			if (this._isImperial)
			{
				base.AddLog(this._onQuestStartedImperialLogText, false);
			}
			else
			{
				base.AddLog(this._onQuestStartedAntiImperialLogText, false);
			}
			int minimumClanTierToCreateKingdom = Campaign.Current.Models.KingdomCreationModel.MinimumClanTierToCreateKingdom;
			this._clanTierRequirementLog = base.AddDiscreteLog(this._clanTierRequirementLogText, new TextObject("{=tTLvo8sM}Clan Tier", null), (int)MathF.Clamp((float)Clan.PlayerClan.Tier, 0f, (float)minimumClanTierToCreateKingdom), minimumClanTierToCreateKingdom, null, false);
			this._partySizeRequirementLog = base.AddDiscreteLog(this._partySizeRequirementLogText, new TextObject("{=aClquusd}Troop Count", null), (int)MathF.Clamp((float)(MobileParty.MainParty.MemberRoster.TotalManCount - MobileParty.MainParty.MemberRoster.TotalWounded), 0f, 100f), 100, null, false);
			this._clanIndependenceRequirementLog = base.AddDiscreteLog(this._clanIndependenceRequirementLogText, new TextObject("{=qa0o7xaj}Clan Independence", null), (Clan.PlayerClan.Kingdom == null) ? 1 : 0, 1, null, false);
			int num;
			if (!this._isImperial)
			{
				num = Clan.PlayerClan.Settlements.Count<Settlement>((Settlement t) => t.IsFortification);
			}
			else
			{
				num = Clan.PlayerClan.Settlements.Count<Settlement>((Settlement t) => t.IsFortification && t.Culture == StoryModeData.ImperialCulture);
			}
			int num2 = num;
			num2 = (int)MathF.Clamp((float)num2, 0f, 1f);
			this._settlementOwnershipRequirementLog = base.AddDiscreteLog(this._settlementOwnershipRequirementLogText, new TextObject("{=gL3WCqM5}Settlement Count", null), num2, 1, null, false);
			base.InitializeQuestOnCreation();
			this.CheckPlayerClanDiplomaticState(Clan.PlayerClan.Kingdom);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x000117EF File Offset: 0x0000F9EF
		protected override void SetDialogs()
		{
			this.DiscussDialogFlow = this.GetMentorDialogueFlow();
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000117FD File Offset: 0x0000F9FD
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00011805 File Offset: 0x0000FA05
		protected override void HourlyTick()
		{
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00011808 File Offset: 0x0000FA08
		private DialogFlow GetMentorDialogueFlow()
		{
			return DialogFlow.CreateDialogFlow("quest_discuss", 300).NpcLine("{=kbyqtszZ}I'm listening..", null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
				.PlayerLine("{=wErSpkjy}I'm still working on it.", null, null, null)
				.CloseDialog();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00011858 File Offset: 0x0000FA58
		private void OnClanTierIncreased(Clan clan, bool showNotification)
		{
			if (!this._hasPlayerCreatedKingdom && clan == Clan.PlayerClan)
			{
				base.UpdateQuestTaskStage(this._clanTierRequirementLog, (int)MathF.Clamp((float)Clan.PlayerClan.Tier, 0f, (float)Campaign.Current.Models.KingdomCreationModel.MinimumClanTierToCreateKingdom));
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000118AC File Offset: 0x0000FAAC
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			if (clan == Clan.PlayerClan)
			{
				this.CheckPlayerClanDiplomaticState(newKingdom);
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000118C0 File Offset: 0x0000FAC0
		private void CheckPlayerClanDiplomaticState(Kingdom newKingdom)
		{
			if (newKingdom == null)
			{
				if (this._hasPlayerCreatedKingdom)
				{
					this._leftKingdomLog = base.AddLog(this._leftKingdomAfterCreatingLogText, false);
					this._hasPlayerCreatedKingdom = false;
				}
				base.UpdateQuestTaskStage(this._clanIndependenceRequirementLog, 1);
				return;
			}
			if (newKingdom.RulingClan != Clan.PlayerClan)
			{
				if (this._playerCreatedKingdom == newKingdom && this._isImperial == StoryModeData.IsKingdomImperial(newKingdom))
				{
					base.RemoveLog(this._leftKingdomLog);
				}
				return;
			}
			this._playerCreatedKingdom = newKingdom;
			if (StoryModeData.IsKingdomImperial(newKingdom))
			{
				if (!this._isImperial)
				{
					base.UpdateQuestTaskStage(this._clanIndependenceRequirementLog, 0);
					return;
				}
				this._hasPlayerCreatedKingdom = true;
				if (this._leftKingdomLog != null)
				{
					base.RemoveLog(this._leftKingdomLog);
					return;
				}
				base.AddLog(this._imperialKingdomCreatedLogText, false);
				return;
			}
			else
			{
				if (this._isImperial)
				{
					base.UpdateQuestTaskStage(this._clanIndependenceRequirementLog, 0);
					return;
				}
				this._hasPlayerCreatedKingdom = true;
				if (this._leftKingdomLog != null)
				{
					base.RemoveLog(this._leftKingdomLog);
					return;
				}
				base.AddLog(this._antiImperialKingdomCreatedLogText, false);
				return;
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000119C4 File Offset: 0x0000FBC4
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (!this._hasPlayerCreatedKingdom && (newOwner == Hero.MainHero || oldOwner == Hero.MainHero))
			{
				int num = -1;
				if (this._isImperial && settlement.Culture == StoryModeData.ImperialCulture)
				{
					num = Clan.PlayerClan.Settlements.Count<Settlement>((Settlement t) => t.IsFortification && t.Culture == StoryModeData.ImperialCulture);
				}
				else if (!this._isImperial)
				{
					num = Clan.PlayerClan.Settlements.Count<Settlement>((Settlement t) => t.IsFortification);
				}
				if (num != -1)
				{
					base.UpdateQuestTaskStage(this._settlementOwnershipRequirementLog, (int)MathF.Clamp((float)num, 0f, 1f));
				}
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00011A94 File Offset: 0x0000FC94
		private void OnPartySizeChanged(PartyBase party)
		{
			if (!this._hasPlayerCreatedKingdom && party == PartyBase.MainParty)
			{
				int num = (int)MathF.Clamp((float)(MobileParty.MainParty.MemberRoster.TotalManCount - MobileParty.MainParty.MemberRoster.TotalWounded), 0f, 100f);
				base.UpdateQuestTaskStage(this._partySizeRequirementLog, num);
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00011AEF File Offset: 0x0000FCEF
		private void MainStoryLineChosen(MainStoryLineSide chosenSide)
		{
			if (this._hasPlayerCreatedKingdom && ((chosenSide == MainStoryLineSide.CreateImperialKingdom && this._isImperial) || (chosenSide == MainStoryLineSide.CreateAntiImperialKingdom && !this._isImperial)))
			{
				base.CompleteQuestWithSuccess();
				return;
			}
			base.CompleteQuestWithCancel(this._questFailedLogText);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00011B24 File Offset: 0x0000FD24
		protected override void RegisterEvents()
		{
			CampaignEvents.ClanTierIncrease.AddNonSerializedListener(this, new Action<Clan, bool>(this.OnClanTierIncreased));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.OnPartySizeChangedEvent.AddNonSerializedListener(this, new Action<PartyBase>(this.OnPartySizeChanged));
			StoryModeEvents.OnMainStoryLineSideChosenEvent.AddNonSerializedListener(this, new Action<MainStoryLineSide>(this.MainStoryLineChosen));
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00011BA4 File Offset: 0x0000FDA4
		internal static void AutoGeneratedStaticCollectObjectsCreateKingdomQuest(object o, List<object> collectedObjects)
		{
			((CreateKingdomQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00011BB4 File Offset: 0x0000FDB4
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._leftKingdomLog);
			collectedObjects.Add(this._playerCreatedKingdom);
			collectedObjects.Add(this._clanTierRequirementLog);
			collectedObjects.Add(this._partySizeRequirementLog);
			collectedObjects.Add(this._settlementOwnershipRequirementLog);
			collectedObjects.Add(this._clanIndependenceRequirementLog);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00011C10 File Offset: 0x0000FE10
		internal static object AutoGeneratedGetMemberValue_isImperial(object o)
		{
			return ((CreateKingdomQuest)o)._isImperial;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00011C22 File Offset: 0x0000FE22
		internal static object AutoGeneratedGetMemberValue_hasPlayerCreatedKingdom(object o)
		{
			return ((CreateKingdomQuest)o)._hasPlayerCreatedKingdom;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00011C34 File Offset: 0x0000FE34
		internal static object AutoGeneratedGetMemberValue_leftKingdomLog(object o)
		{
			return ((CreateKingdomQuest)o)._leftKingdomLog;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00011C41 File Offset: 0x0000FE41
		internal static object AutoGeneratedGetMemberValue_playerCreatedKingdom(object o)
		{
			return ((CreateKingdomQuest)o)._playerCreatedKingdom;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00011C4E File Offset: 0x0000FE4E
		internal static object AutoGeneratedGetMemberValue_clanTierRequirementLog(object o)
		{
			return ((CreateKingdomQuest)o)._clanTierRequirementLog;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00011C5B File Offset: 0x0000FE5B
		internal static object AutoGeneratedGetMemberValue_partySizeRequirementLog(object o)
		{
			return ((CreateKingdomQuest)o)._partySizeRequirementLog;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00011C68 File Offset: 0x0000FE68
		internal static object AutoGeneratedGetMemberValue_settlementOwnershipRequirementLog(object o)
		{
			return ((CreateKingdomQuest)o)._settlementOwnershipRequirementLog;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00011C75 File Offset: 0x0000FE75
		internal static object AutoGeneratedGetMemberValue_clanIndependenceRequirementLog(object o)
		{
			return ((CreateKingdomQuest)o)._clanIndependenceRequirementLog;
		}

		// Token: 0x04000104 RID: 260
		[SaveableField(1)]
		private readonly bool _isImperial;

		// Token: 0x04000105 RID: 261
		private const int PartySizeRequirement = 100;

		// Token: 0x04000106 RID: 262
		private const int SettlementCountRequirement = 1;

		// Token: 0x04000107 RID: 263
		[SaveableField(2)]
		private bool _hasPlayerCreatedKingdom;

		// Token: 0x04000108 RID: 264
		[SaveableField(9)]
		private JournalLog _leftKingdomLog;

		// Token: 0x04000109 RID: 265
		[SaveableField(10)]
		private Kingdom _playerCreatedKingdom;

		// Token: 0x0400010A RID: 266
		[SaveableField(4)]
		private readonly JournalLog _clanTierRequirementLog;

		// Token: 0x0400010B RID: 267
		[SaveableField(5)]
		private readonly JournalLog _partySizeRequirementLog;

		// Token: 0x0400010C RID: 268
		[SaveableField(6)]
		private readonly JournalLog _settlementOwnershipRequirementLog;

		// Token: 0x0400010D RID: 269
		[SaveableField(7)]
		private readonly JournalLog _clanIndependenceRequirementLog;
	}
}
