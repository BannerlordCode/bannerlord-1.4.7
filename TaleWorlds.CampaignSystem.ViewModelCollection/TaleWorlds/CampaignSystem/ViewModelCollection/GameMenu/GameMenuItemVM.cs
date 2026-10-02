using System;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu
{
	// Token: 0x0200009C RID: 156
	public class GameMenuItemVM : ViewModel
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x0003EF46 File Offset: 0x0003D146
		// (set) Token: 0x06000F1E RID: 3870 RVA: 0x0003EF4E File Offset: 0x0003D14E
		public string OptionID { get; private set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0003EF57 File Offset: 0x0003D157
		// (set) Token: 0x06000F20 RID: 3872 RVA: 0x0003EF5F File Offset: 0x0003D15F
		public GameMenuOption GameMenuOption { get; private set; }

		// Token: 0x06000F21 RID: 3873 RVA: 0x0003EF68 File Offset: 0x0003D168
		public GameMenuItemVM()
		{
			this.ItemHint = new HintViewModel();
			this.Quests = new MBBindingList<QuestMarkerVM>();
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0003EF94 File Offset: 0x0003D194
		public void InitializeWith(in GameMenuItemVM.GameMenuItemCreationData data)
		{
			this.GameMenuOption = data.GameMenuOption;
			this.Index = data.Index;
			this._menuContext = data.MenuContext;
			this._itemType = (int)data.Type;
			this._tooltip = data.Tooltip;
			this._nonWaitText = data.Text;
			this._waitText = data.Text2;
			this.Item = this._nonWaitText.ToString();
			this.ItemHint.HintText = this._tooltip;
			this.OptionLeaveType = data.GameMenuOption.OptionLeaveType.ToString();
			this.OptionID = data.GameMenuOption.IdString;
			if (data.OptionQuestData != this._questFlags)
			{
				this.Quests.Clear();
				for (int i = 0; i < GameMenuOption.IssueQuestFlagsValues.Length; i++)
				{
					GameMenuOption.IssueQuestFlags issueQuestFlags = GameMenuOption.IssueQuestFlagsValues[i];
					if (issueQuestFlags != GameMenuOption.IssueQuestFlags.None && (data.OptionQuestData & issueQuestFlags) != GameMenuOption.IssueQuestFlags.None)
					{
						CampaignUIHelper.IssueQuestFlags issueQuestFlags2 = (CampaignUIHelper.IssueQuestFlags)issueQuestFlags;
						this.Quests.Add(new QuestMarkerVM(issueQuestFlags2, null, null));
					}
				}
				this._questFlags = data.OptionQuestData;
			}
			this.ShortcutKey = ((data.ShortcutKey != null) ? InputKeyItemVM.CreateFromGameKey(data.ShortcutKey, true) : null);
			this.RefreshValues();
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0003F0CB File Offset: 0x0003D2CB
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Refresh();
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0003F0D9 File Offset: 0x0003D2D9
		public void ExecuteAction()
		{
			MenuContext menuContext = this._menuContext;
			if (menuContext == null)
			{
				return;
			}
			menuContext.InvokeConsequence(this.Index);
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0003F0F1 File Offset: 0x0003D2F1
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this.ShortcutKey != null)
			{
				this.ShortcutKey.OnFinalize();
			}
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0003F10C File Offset: 0x0003D30C
		public void Refresh()
		{
			int itemType = this._itemType;
			if (itemType != 0)
			{
				int num = itemType - 1;
			}
			this.IsWaitActive = Campaign.Current.GameMenuManager.GetVirtualMenuIsWaitActive(this._menuContext);
			this.IsEnabled = Campaign.Current.GameMenuManager.GetVirtualMenuOptionIsEnabled(this._menuContext, this.Index);
			this.ItemHint.HintText = Campaign.Current.GameMenuManager.GetVirtualMenuOptionTooltip(this._menuContext, this.Index);
			this.GameMenuStringId = this._menuContext.GameMenu.StringId;
			if (PlayerEncounter.Battle != null)
			{
				this.BattleSize = PlayerEncounter.Battle.AttackerSide.TroopCount + PlayerEncounter.Battle.DefenderSide.TroopCount;
			}
			else
			{
				this.BattleSize = -1;
			}
			MapEvent battle = PlayerEncounter.Battle;
			this.IsNavalBattle = battle != null && battle.IsNavalMapEvent;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0003F1F0 File Offset: 0x0003D3F0
		public void UpdateWith(GameMenuItemVM newItem)
		{
			this.Item = newItem.Item;
			this.OptionLeaveType = newItem.OptionLeaveType;
			this.ItemHint = newItem.ItemHint;
			this.Quests = newItem.Quests;
			this.Index = newItem.Index;
			this.GameMenuOption = newItem.GameMenuOption;
			this.Refresh();
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x0003F24B File Offset: 0x0003D44B
		// (set) Token: 0x06000F29 RID: 3881 RVA: 0x0003F253 File Offset: 0x0003D453
		[DataSourceProperty]
		public MBBindingList<QuestMarkerVM> Quests
		{
			get
			{
				return this._quests;
			}
			set
			{
				if (value != this._quests)
				{
					this._quests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "Quests");
				}
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0003F271 File Offset: 0x0003D471
		// (set) Token: 0x06000F2B RID: 3883 RVA: 0x0003F279 File Offset: 0x0003D479
		[DataSourceProperty]
		public string OptionLeaveType
		{
			get
			{
				return this._optionLeaveType;
			}
			set
			{
				if (value != this._optionLeaveType)
				{
					this._optionLeaveType = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionLeaveType");
				}
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0003F29C File Offset: 0x0003D49C
		// (set) Token: 0x06000F2D RID: 3885 RVA: 0x0003F2A4 File Offset: 0x0003D4A4
		[DataSourceProperty]
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChangedWithValue(value, "ItemType");
				}
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0003F2C2 File Offset: 0x0003D4C2
		// (set) Token: 0x06000F2F RID: 3887 RVA: 0x0003F2CA File Offset: 0x0003D4CA
		[DataSourceProperty]
		public bool IsWaitActive
		{
			get
			{
				return this._isWaitActive;
			}
			set
			{
				if (value != this._isWaitActive)
				{
					this._isWaitActive = value;
					base.OnPropertyChangedWithValue(value, "IsWaitActive");
					this.Item = (value ? this._waitText.ToString() : this._nonWaitText.ToString());
				}
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x0003F309 File Offset: 0x0003D509
		// (set) Token: 0x06000F31 RID: 3889 RVA: 0x0003F311 File Offset: 0x0003D511
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0003F32F File Offset: 0x0003D52F
		// (set) Token: 0x06000F33 RID: 3891 RVA: 0x0003F337 File Offset: 0x0003D537
		[DataSourceProperty]
		public bool IsHighlightEnabled
		{
			get
			{
				return this._isHighlightEnabled;
			}
			set
			{
				if (value != this._isHighlightEnabled)
				{
					this._isHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHighlightEnabled");
				}
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x0003F355 File Offset: 0x0003D555
		// (set) Token: 0x06000F35 RID: 3893 RVA: 0x0003F35D File Offset: 0x0003D55D
		[DataSourceProperty]
		public HintViewModel ItemHint
		{
			get
			{
				return this._itemHint;
			}
			set
			{
				if (value != this._itemHint)
				{
					this._itemHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ItemHint");
				}
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x0003F37B File Offset: 0x0003D57B
		// (set) Token: 0x06000F37 RID: 3895 RVA: 0x0003F383 File Offset: 0x0003D583
		[DataSourceProperty]
		public HintViewModel QuestHint
		{
			get
			{
				return this._questHint;
			}
			set
			{
				if (value != this._questHint)
				{
					this._questHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "QuestHint");
				}
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0003F3A1 File Offset: 0x0003D5A1
		// (set) Token: 0x06000F39 RID: 3897 RVA: 0x0003F3A9 File Offset: 0x0003D5A9
		[DataSourceProperty]
		public HintViewModel IssueHint
		{
			get
			{
				return this._issueHint;
			}
			set
			{
				if (value != this._issueHint)
				{
					this._issueHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "IssueHint");
				}
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x0003F3C7 File Offset: 0x0003D5C7
		// (set) Token: 0x06000F3B RID: 3899 RVA: 0x0003F3CF File Offset: 0x0003D5CF
		[DataSourceProperty]
		public string GameMenuStringId
		{
			get
			{
				return this._gameMenuStringId;
			}
			set
			{
				if (value != this._gameMenuStringId)
				{
					this._gameMenuStringId = value;
					base.OnPropertyChangedWithValue<string>(value, "GameMenuStringId");
				}
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0003F3F2 File Offset: 0x0003D5F2
		// (set) Token: 0x06000F3D RID: 3901 RVA: 0x0003F3FA File Offset: 0x0003D5FA
		[DataSourceProperty]
		public string Item
		{
			get
			{
				return this._item;
			}
			set
			{
				if (value != this._item)
				{
					this._item = value;
					base.OnPropertyChangedWithValue<string>(value, "Item");
				}
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x0003F41D File Offset: 0x0003D61D
		// (set) Token: 0x06000F3F RID: 3903 RVA: 0x0003F425 File Offset: 0x0003D625
		[DataSourceProperty]
		public int BattleSize
		{
			get
			{
				return this._battleSize;
			}
			set
			{
				if (value != this._battleSize)
				{
					this._battleSize = value;
					base.OnPropertyChangedWithValue(value, "BattleSize");
				}
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0003F443 File Offset: 0x0003D643
		// (set) Token: 0x06000F41 RID: 3905 RVA: 0x0003F44B File Offset: 0x0003D64B
		[DataSourceProperty]
		public bool IsNavalBattle
		{
			get
			{
				return this._isNavalBattle;
			}
			set
			{
				if (value != this._isNavalBattle)
				{
					this._isNavalBattle = value;
					base.OnPropertyChangedWithValue(value, "IsNavalBattle");
				}
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0003F469 File Offset: 0x0003D669
		// (set) Token: 0x06000F43 RID: 3907 RVA: 0x0003F471 File Offset: 0x0003D671
		public InputKeyItemVM ShortcutKey
		{
			get
			{
				return this._shortcutKey;
			}
			set
			{
				if (value != this._shortcutKey)
				{
					this._shortcutKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ShortcutKey");
				}
			}
		}

		// Token: 0x040006D7 RID: 1751
		private MenuContext _menuContext;

		// Token: 0x040006D8 RID: 1752
		public int Index;

		// Token: 0x040006D9 RID: 1753
		private TextObject _nonWaitText;

		// Token: 0x040006DA RID: 1754
		private TextObject _waitText;

		// Token: 0x040006DB RID: 1755
		private TextObject _tooltip;

		// Token: 0x040006DD RID: 1757
		private GameMenuOption.IssueQuestFlags _questFlags;

		// Token: 0x040006DE RID: 1758
		private MBBindingList<QuestMarkerVM> _quests;

		// Token: 0x040006DF RID: 1759
		private int _itemType = -1;

		// Token: 0x040006E0 RID: 1760
		private bool _isWaitActive;

		// Token: 0x040006E1 RID: 1761
		private bool _isEnabled;

		// Token: 0x040006E2 RID: 1762
		private HintViewModel _itemHint;

		// Token: 0x040006E3 RID: 1763
		private HintViewModel _questHint;

		// Token: 0x040006E4 RID: 1764
		private HintViewModel _issueHint;

		// Token: 0x040006E5 RID: 1765
		private bool _isHighlightEnabled;

		// Token: 0x040006E6 RID: 1766
		private string _optionLeaveType;

		// Token: 0x040006E7 RID: 1767
		private string _gameMenuStringId;

		// Token: 0x040006E8 RID: 1768
		private string _item;

		// Token: 0x040006E9 RID: 1769
		private int _battleSize = -1;

		// Token: 0x040006EA RID: 1770
		private bool _isNavalBattle;

		// Token: 0x040006EB RID: 1771
		private InputKeyItemVM _shortcutKey;

		// Token: 0x02000210 RID: 528
		public readonly struct GameMenuItemCreationData
		{
			// Token: 0x17000BAB RID: 2987
			// (get) Token: 0x06002468 RID: 9320 RVA: 0x000805E0 File Offset: 0x0007E7E0
			public string OptionID
			{
				get
				{
					return this.GameMenuOption.IdString;
				}
			}

			// Token: 0x06002469 RID: 9321 RVA: 0x000805F0 File Offset: 0x0007E7F0
			public GameMenuItemCreationData(MenuContext menuContext, int index, TextObject text, TextObject text2, TextObject tooltip, GameMenu.MenuAndOptionType type, GameMenuOption.IssueQuestFlags questFlags, GameMenuOption gameMenuOption, GameKey shortcutKey)
			{
				this.OptionQuestData = questFlags;
				this.MenuContext = menuContext;
				this.Index = index;
				this.Text = text;
				this.Text2 = text2;
				this.Tooltip = tooltip;
				this.Type = type;
				this.GameMenuOption = gameMenuOption;
				this.ShortcutKey = shortcutKey;
			}

			// Token: 0x040011BA RID: 4538
			public readonly MenuContext MenuContext;

			// Token: 0x040011BB RID: 4539
			public readonly int Index;

			// Token: 0x040011BC RID: 4540
			public readonly TextObject Text;

			// Token: 0x040011BD RID: 4541
			public readonly TextObject Text2;

			// Token: 0x040011BE RID: 4542
			public readonly TextObject Tooltip;

			// Token: 0x040011BF RID: 4543
			public readonly GameMenu.MenuAndOptionType Type;

			// Token: 0x040011C0 RID: 4544
			public readonly GameMenuOption.IssueQuestFlags OptionQuestData;

			// Token: 0x040011C1 RID: 4545
			public readonly GameMenuOption GameMenuOption;

			// Token: 0x040011C2 RID: 4546
			public readonly GameKey ShortcutKey;
		}
	}
}
