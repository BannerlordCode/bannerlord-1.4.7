using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.PlayerClanQuests
{
	// Token: 0x0200002F RID: 47
	public class RebuildPlayerClanQuest : StoryModeQuestBase
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000E7A7 File Offset: 0x0000C9A7
		private static int _partySizeGoal
		{
			get
			{
				return Campaign.Current.Models.BanditDensityModel.GetMinimumTroopCountForHideoutMission(MobileParty.MainParty, false);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000E7C3 File Offset: 0x0000C9C3
		private TextObject _startQuestLogText
		{
			get
			{
				return new TextObject("{=IITkXnnU}Calradia is a land full of peril - but also opportunities. To face the challenges that await, you will need to build up your clan.{newline}Your brother told you that there are many ways to go about this but that none forego coin. Trade would be one means to this end, fighting and selling off captured bandits in town another. Whatever path you choose to pursue, travelling alone would make you easy pickings for whomever came across your trail.{newline}You know that you can recruit men to follow you from the notables of villages and towns, though they may ask you for a favor or two of their own before they allow you access to their more valued fighters.{newline}Naturally, you may also find more unique characters in the taverns of Calradia. However, these tend to favor more established clans.", null);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000E7D0 File Offset: 0x0000C9D0
		private TextObject _goldGoalLogText
		{
			get
			{
				return new TextObject("{=bXYFXLgg}Increase your denars by 1000", null);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000E7DD File Offset: 0x0000C9DD
		private TextObject _partySizeGoalLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=b6hQWKHe}Grow your party to {PARTY_SIZE} men", null);
				textObject.SetTextVariable("PARTY_SIZE", RebuildPlayerClanQuest._partySizeGoal);
				return textObject;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000E7FB File Offset: 0x0000C9FB
		private TextObject _clanTierGoalLogText
		{
			get
			{
				return new TextObject("{=RbXiEdXk}Reach Clan Tier 1", null);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000E808 File Offset: 0x0000CA08
		private TextObject _hireCompanionGoalLogText
		{
			get
			{
				return new TextObject("{=e8Tjf8Ph}Hire 1 Companion", null);
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000E815 File Offset: 0x0000CA15
		private TextObject _successLogText
		{
			get
			{
				return new TextObject("{=eJX7rhch}You have successfully rebuilt your clan.", null);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000E822 File Offset: 0x0000CA22
		public override TextObject Title
		{
			get
			{
				return new TextObject("{=bESRdcRo}Establish Your Clan", null);
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000E82F File Offset: 0x0000CA2F
		public RebuildPlayerClanQuest()
			: base("rebuild_player_clan_storymode_quest", null, CampaignTime.Never)
		{
			this._finishQuest = false;
			this.SetDialogs();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000E84F File Offset: 0x0000CA4F
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000E858 File Offset: 0x0000CA58
		protected override void RegisterEvents()
		{
			CampaignEvents.HeroOrPartyTradedGold.AddNonSerializedListener(this, new Action<ValueTuple<Hero, PartyBase>, ValueTuple<Hero, PartyBase>, ValueTuple<int, string>, bool>(this.HeroOrPartyTradedGold));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnTroopRecruitedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, Hero, CharacterObject, int>(this.OnTroopRecruited));
			CampaignEvents.RenownGained.AddNonSerializedListener(this, new Action<Hero, int, bool>(this.OnRenownGained));
			CampaignEvents.NewCompanionAdded.AddNonSerializedListener(this, new Action<Hero>(this.OnNewCompanionAdded));
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000E908 File Offset: 0x0000CB08
		protected override void OnStartQuest()
		{
			base.AddLog(this._startQuestLogText, true);
			this._goldGoalLog = base.AddDiscreteLog(this._goldGoalLogText, new TextObject("{=hYgmzZJX}Denars", null), Hero.MainHero.Gold, 2000, null, true);
			this._partySizeGoalLog = base.AddDiscreteLog(this._partySizeGoalLogText, new TextObject("{=DO4PE3Oo}Current Party Size", null), 1, RebuildPlayerClanQuest._partySizeGoal, null, true);
			this._clanTierGoalLog = base.AddDiscreteLog(this._clanTierGoalLogText, new TextObject("{=aZxHIra4}Renown", null), (int)Clan.PlayerClan.Renown, 50, null, true);
			this._hireCompanionGoalLog = base.AddDiscreteLog(this._hireCompanionGoalLogText, new TextObject("{=VLD5416o}Companion Hired", null), 0, 1, null, true);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000E9C3 File Offset: 0x0000CBC3
		protected override void OnCompleteWithSuccess()
		{
			GainRenownAction.Apply(Hero.MainHero, 25f, false);
			base.AddLog(this._successLogText, false);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000E9E3 File Offset: 0x0000CBE3
		protected override void SetDialogs()
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000E9E5 File Offset: 0x0000CBE5
		private void HeroOrPartyTradedGold(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> recipient, ValueTuple<int, string> goldAmount, bool showNotification)
		{
			this.UpdateProgresses();
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000E9ED File Offset: 0x0000CBED
		protected override void HourlyTick()
		{
			this.UpdateProgresses();
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000E9F5 File Offset: 0x0000CBF5
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			this.UpdateProgresses();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000E9FD File Offset: 0x0000CBFD
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			this.UpdateProgresses();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000EA05 File Offset: 0x0000CC05
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			this.UpdateProgresses();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000EA0D File Offset: 0x0000CC0D
		private void OnTroopRecruited(Hero recruiterHero, Settlement recruitmentSettlement, Hero recruitmentSource, CharacterObject troop, int amount)
		{
			this.UpdateProgresses();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000EA15 File Offset: 0x0000CC15
		private void OnRenownGained(Hero hero, int gainedRenown, bool doNotNotify)
		{
			this.UpdateProgresses();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000EA1D File Offset: 0x0000CC1D
		private void OnNewCompanionAdded(Hero newCompanion)
		{
			this.UpdateProgresses();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000EA28 File Offset: 0x0000CC28
		private void UpdateProgresses()
		{
			this._goldGoalLog.UpdateCurrentProgress((Hero.MainHero.Gold > 2000) ? 2000 : Hero.MainHero.Gold);
			this._partySizeGoalLog.UpdateCurrentProgress((PartyBase.MainParty.MemberRoster.TotalManCount > RebuildPlayerClanQuest._partySizeGoal) ? RebuildPlayerClanQuest._partySizeGoal : PartyBase.MainParty.MemberRoster.TotalManCount);
			this._clanTierGoalLog.UpdateCurrentProgress((Clan.PlayerClan.Renown > 50f) ? 50 : ((int)Clan.PlayerClan.Renown));
			this._hireCompanionGoalLog.UpdateCurrentProgress((Clan.PlayerClan.Companions.Count > 1) ? 1 : Clan.PlayerClan.Companions.Count);
			if (this._goldGoalLog.CurrentProgress >= 2000 && this._partySizeGoalLog.CurrentProgress >= RebuildPlayerClanQuest._partySizeGoal && this._clanTierGoalLog.CurrentProgress >= 50 && this._hireCompanionGoalLog.CurrentProgress >= 1 && !this._finishQuest)
			{
				this._finishQuest = true;
				base.CompleteQuestWithSuccess();
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000EB49 File Offset: 0x0000CD49
		internal static void AutoGeneratedStaticCollectObjectsRebuildPlayerClanQuest(object o, List<object> collectedObjects)
		{
			((RebuildPlayerClanQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000EB57 File Offset: 0x0000CD57
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._goldGoalLog);
			collectedObjects.Add(this._partySizeGoalLog);
			collectedObjects.Add(this._clanTierGoalLog);
			collectedObjects.Add(this._hireCompanionGoalLog);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000EB90 File Offset: 0x0000CD90
		internal static object AutoGeneratedGetMemberValue_goldGoalLog(object o)
		{
			return ((RebuildPlayerClanQuest)o)._goldGoalLog;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000EB9D File Offset: 0x0000CD9D
		internal static object AutoGeneratedGetMemberValue_partySizeGoalLog(object o)
		{
			return ((RebuildPlayerClanQuest)o)._partySizeGoalLog;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000EBAA File Offset: 0x0000CDAA
		internal static object AutoGeneratedGetMemberValue_clanTierGoalLog(object o)
		{
			return ((RebuildPlayerClanQuest)o)._clanTierGoalLog;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000EBB7 File Offset: 0x0000CDB7
		internal static object AutoGeneratedGetMemberValue_hireCompanionGoalLog(object o)
		{
			return ((RebuildPlayerClanQuest)o)._hireCompanionGoalLog;
		}

		// Token: 0x040000D8 RID: 216
		private const int GoldGoal = 2000;

		// Token: 0x040000D9 RID: 217
		private const int ClanTierRenownGoal = 50;

		// Token: 0x040000DA RID: 218
		private const int RenownReward = 25;

		// Token: 0x040000DB RID: 219
		private const int HiredCompanionGoal = 1;

		// Token: 0x040000DC RID: 220
		private bool _finishQuest;

		// Token: 0x040000DD RID: 221
		[SaveableField(1)]
		private JournalLog _goldGoalLog;

		// Token: 0x040000DE RID: 222
		[SaveableField(2)]
		private JournalLog _partySizeGoalLog;

		// Token: 0x040000DF RID: 223
		[SaveableField(3)]
		private JournalLog _clanTierGoalLog;

		// Token: 0x040000E0 RID: 224
		[SaveableField(4)]
		private JournalLog _hireCompanionGoalLog;
	}
}
