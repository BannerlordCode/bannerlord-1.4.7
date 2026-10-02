using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000E9 RID: 233
	public class GameMenuOption
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x00062745 File Offset: 0x00060945
		// (set) Token: 0x060015AD RID: 5549 RVA: 0x0006274D File Offset: 0x0006094D
		public GameMenu.MenuAndOptionType Type { get; private set; }

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060015AE RID: 5550 RVA: 0x00062756 File Offset: 0x00060956
		// (set) Token: 0x060015AF RID: 5551 RVA: 0x0006275E File Offset: 0x0006095E
		public GameMenuOption.LeaveType OptionLeaveType { get; set; }

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060015B0 RID: 5552 RVA: 0x00062767 File Offset: 0x00060967
		// (set) Token: 0x060015B1 RID: 5553 RVA: 0x0006276F File Offset: 0x0006096F
		public GameMenuOption.IssueQuestFlags OptionQuestData { get; set; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060015B2 RID: 5554 RVA: 0x00062778 File Offset: 0x00060978
		// (set) Token: 0x060015B3 RID: 5555 RVA: 0x00062780 File Offset: 0x00060980
		public string IdString { get; private set; }

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060015B4 RID: 5556 RVA: 0x00062789 File Offset: 0x00060989
		// (set) Token: 0x060015B5 RID: 5557 RVA: 0x00062791 File Offset: 0x00060991
		public TextObject Text { get; private set; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x0006279A File Offset: 0x0006099A
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x000627A2 File Offset: 0x000609A2
		public TextObject Text2 { get; private set; }

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x000627AB File Offset: 0x000609AB
		// (set) Token: 0x060015B9 RID: 5561 RVA: 0x000627B3 File Offset: 0x000609B3
		public TextObject Tooltip { get; private set; }

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x000627BC File Offset: 0x000609BC
		// (set) Token: 0x060015BB RID: 5563 RVA: 0x000627C4 File Offset: 0x000609C4
		public bool IsLeave { get; private set; }

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x000627CD File Offset: 0x000609CD
		// (set) Token: 0x060015BD RID: 5565 RVA: 0x000627D5 File Offset: 0x000609D5
		public bool IsRepeatable { get; private set; }

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x000627DE File Offset: 0x000609DE
		// (set) Token: 0x060015BF RID: 5567 RVA: 0x000627E6 File Offset: 0x000609E6
		public bool IsEnabled { get; private set; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060015C0 RID: 5568 RVA: 0x000627EF File Offset: 0x000609EF
		// (set) Token: 0x060015C1 RID: 5569 RVA: 0x000627F7 File Offset: 0x000609F7
		public object RelatedObject { get; private set; }

		// Token: 0x060015C2 RID: 5570 RVA: 0x00062800 File Offset: 0x00060A00
		internal GameMenuOption()
		{
			this.Text = null;
			this.Tooltip = null;
			this.IsEnabled = true;
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00062820 File Offset: 0x00060A20
		public GameMenuOption(GameMenu.MenuAndOptionType type, string idString, TextObject text, TextObject text2, GameMenuOption.OnConditionDelegate condition, GameMenuOption.OnConsequenceDelegate consequence, bool isLeave = false, bool isRepeatable = false, object relatedObject = null)
		{
			this.Type = type;
			this.IdString = idString;
			this.Text = text;
			this.Text2 = text2;
			this.OnCondition = condition;
			this.OnConsequence = consequence;
			this.Tooltip = null;
			this.IsRepeatable = isRepeatable;
			this.IsEnabled = true;
			this.IsLeave = isLeave;
			this.RelatedObject = relatedObject;
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x00062888 File Offset: 0x00060A88
		public bool GetConditionsHold(Game game, MenuContext menuContext)
		{
			if (this.OnCondition != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.Text);
				bool flag = this.OnCondition(menuCallbackArgs);
				this.IsEnabled = menuCallbackArgs.IsEnabled;
				this.Tooltip = menuCallbackArgs.Tooltip;
				this.OptionQuestData = menuCallbackArgs.OptionQuestData;
				this.OptionLeaveType = menuCallbackArgs.optionLeaveType;
				return flag;
			}
			return true;
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x000628E8 File Offset: 0x00060AE8
		public void RunConsequence(MenuContext menuContext)
		{
			if (this.OnConsequence != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.Text);
				this.OnConsequence(menuCallbackArgs);
			}
			menuContext.OnConsequence(this);
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0006291D File Offset: 0x00060B1D
		public void SetEnable(bool isEnable)
		{
			this.IsEnabled = isEnable;
		}

		// Token: 0x04000722 RID: 1826
		public static GameMenuOption.IssueQuestFlags[] IssueQuestFlagsValues = (GameMenuOption.IssueQuestFlags[])Enum.GetValues(typeof(GameMenuOption.IssueQuestFlags));

		// Token: 0x0400072A RID: 1834
		public GameMenuOption.OnConditionDelegate OnCondition;

		// Token: 0x0400072B RID: 1835
		public GameMenuOption.OnConsequenceDelegate OnConsequence;

		// Token: 0x02000565 RID: 1381
		// (Invoke) Token: 0x06004DBB RID: 19899
		public delegate bool OnConditionDelegate(MenuCallbackArgs args);

		// Token: 0x02000566 RID: 1382
		// (Invoke) Token: 0x06004DBF RID: 19903
		public delegate void OnConsequenceDelegate(MenuCallbackArgs args);

		// Token: 0x02000567 RID: 1383
		public enum LeaveType
		{
			// Token: 0x04001709 RID: 5897
			Default,
			// Token: 0x0400170A RID: 5898
			Mission,
			// Token: 0x0400170B RID: 5899
			Submenu,
			// Token: 0x0400170C RID: 5900
			BribeAndEscape,
			// Token: 0x0400170D RID: 5901
			Escape,
			// Token: 0x0400170E RID: 5902
			Craft,
			// Token: 0x0400170F RID: 5903
			ForceToGiveGoods,
			// Token: 0x04001710 RID: 5904
			ForceToGiveTroops,
			// Token: 0x04001711 RID: 5905
			Bribe,
			// Token: 0x04001712 RID: 5906
			LeaveTroopsAndFlee,
			// Token: 0x04001713 RID: 5907
			OrderTroopsToAttack,
			// Token: 0x04001714 RID: 5908
			Raid,
			// Token: 0x04001715 RID: 5909
			HostileAction,
			// Token: 0x04001716 RID: 5910
			Recruit,
			// Token: 0x04001717 RID: 5911
			Trade,
			// Token: 0x04001718 RID: 5912
			Wait,
			// Token: 0x04001719 RID: 5913
			Leave,
			// Token: 0x0400171A RID: 5914
			Continue,
			// Token: 0x0400171B RID: 5915
			Manage,
			// Token: 0x0400171C RID: 5916
			TroopSelection,
			// Token: 0x0400171D RID: 5917
			WaitQuest,
			// Token: 0x0400171E RID: 5918
			Surrender,
			// Token: 0x0400171F RID: 5919
			Conversation,
			// Token: 0x04001720 RID: 5920
			DefendAction,
			// Token: 0x04001721 RID: 5921
			Devastate,
			// Token: 0x04001722 RID: 5922
			Pillage,
			// Token: 0x04001723 RID: 5923
			ShowMercy,
			// Token: 0x04001724 RID: 5924
			Leaderboard,
			// Token: 0x04001725 RID: 5925
			OpenStash,
			// Token: 0x04001726 RID: 5926
			ManageGarrison,
			// Token: 0x04001727 RID: 5927
			StagePrisonBreak,
			// Token: 0x04001728 RID: 5928
			ManagePrisoners,
			// Token: 0x04001729 RID: 5929
			Ransom,
			// Token: 0x0400172A RID: 5930
			PracticeFight,
			// Token: 0x0400172B RID: 5931
			BesiegeTown,
			// Token: 0x0400172C RID: 5932
			SneakIn,
			// Token: 0x0400172D RID: 5933
			LeadAssault,
			// Token: 0x0400172E RID: 5934
			DonateTroops,
			// Token: 0x0400172F RID: 5935
			DonatePrisoners,
			// Token: 0x04001730 RID: 5936
			SiegeAmbush,
			// Token: 0x04001731 RID: 5937
			Warehouse,
			// Token: 0x04001732 RID: 5938
			VisitPort,
			// Token: 0x04001733 RID: 5939
			VisitTown,
			// Token: 0x04001734 RID: 5940
			SetSail,
			// Token: 0x04001735 RID: 5941
			ManageFleet,
			// Token: 0x04001736 RID: 5942
			CallFleet,
			// Token: 0x04001737 RID: 5943
			OrderShipsToAttack,
			// Token: 0x04001738 RID: 5944
			RepairShips
		}

		// Token: 0x02000568 RID: 1384
		[Flags]
		public enum IssueQuestFlags
		{
			// Token: 0x0400173A RID: 5946
			None = 0,
			// Token: 0x0400173B RID: 5947
			AvailableIssue = 1,
			// Token: 0x0400173C RID: 5948
			ActiveIssue = 2,
			// Token: 0x0400173D RID: 5949
			ActiveStoryQuest = 4,
			// Token: 0x0400173E RID: 5950
			TrackedIssue = 8,
			// Token: 0x0400173F RID: 5951
			TrackedStoryQuest = 16
		}
	}
}
