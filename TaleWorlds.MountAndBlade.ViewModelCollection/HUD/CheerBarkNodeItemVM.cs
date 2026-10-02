using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000053 RID: 83
	public class CheerBarkNodeItemVM : ViewModel
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060006CB RID: 1739 RVA: 0x00018E60 File Offset: 0x00017060
		// (remove) Token: 0x060006CC RID: 1740 RVA: 0x00018E94 File Offset: 0x00017094
		internal static event Action<CheerBarkNodeItemVM> OnSelection;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060006CD RID: 1741 RVA: 0x00018EC8 File Offset: 0x000170C8
		// (remove) Token: 0x060006CE RID: 1742 RVA: 0x00018EFC File Offset: 0x000170FC
		internal static event Action<CheerBarkNodeItemVM> OnNodeFocused;

		// Token: 0x060006CF RID: 1743 RVA: 0x00018F30 File Offset: 0x00017130
		public CheerBarkNodeItemVM(string tauntVisualName, TextObject nodeName, string nodeId, HotKey key, bool consoleOnlyShortcut = false, TauntUsageManager.TauntUsage.TauntUsageFlag disabledReason = TauntUsageManager.TauntUsage.TauntUsageFlag.None)
		{
			this._nodeName = nodeName;
			this.TauntVisualName = tauntVisualName;
			this.TypeAsString = nodeId;
			this.TauntUsageDisabledReason = disabledReason;
			this.IsDisabled = disabledReason != TauntUsageManager.TauntUsage.TauntUsageFlag.None && disabledReason != TauntUsageManager.TauntUsage.TauntUsageFlag.IsLeftStance;
			this.SubNodes = new MBBindingList<CheerBarkNodeItemVM>();
			if (key != null)
			{
				this.ShortcutKey = InputKeyItemVM.CreateFromHotKey(key, consoleOnlyShortcut);
			}
			this.RefreshValues();
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00018F9C File Offset: 0x0001719C
		public CheerBarkNodeItemVM(TextObject nodeName, string nodeId, HotKey key, bool consoleOnlyShortcut = false, TauntUsageManager.TauntUsage.TauntUsageFlag disabledReason = TauntUsageManager.TauntUsage.TauntUsageFlag.None)
		{
			this._nodeName = nodeName;
			this.TauntVisualName = string.Empty;
			this.TypeAsString = nodeId;
			this.TauntUsageDisabledReason = disabledReason;
			this.IsDisabled = disabledReason > TauntUsageManager.TauntUsage.TauntUsageFlag.None;
			this.SubNodes = new MBBindingList<CheerBarkNodeItemVM>();
			if (key != null)
			{
				this.ShortcutKey = InputKeyItemVM.CreateFromHotKey(key, consoleOnlyShortcut);
			}
			this.RefreshValues();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00019000 File Offset: 0x00017200
		public void ClearSelectionRecursive()
		{
			this.IsSelected = false;
			for (int i = 0; i < this.SubNodes.Count; i++)
			{
				this.SubNodes[i].ClearSelectionRecursive();
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001903B File Offset: 0x0001723B
		public void ExecuteFocused()
		{
			Action<CheerBarkNodeItemVM> onNodeFocused = CheerBarkNodeItemVM.OnNodeFocused;
			if (onNodeFocused == null)
			{
				return;
			}
			onNodeFocused(this);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001904D File Offset: 0x0001724D
		public override void RefreshValues()
		{
			TextObject nodeName = this._nodeName;
			this.CheerNameText = ((nodeName != null) ? nodeName.ToString() : null);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00019067 File Offset: 0x00017267
		public void AddSubNode(CheerBarkNodeItemVM subNode)
		{
			this.SubNodes.Add(subNode);
			this.HasSubNodes = true;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001907C File Offset: 0x0001727C
		public override void OnFinalize()
		{
			base.OnFinalize();
			MBBindingList<CheerBarkNodeItemVM> subNodes = this.SubNodes;
			if (subNodes != null)
			{
				subNodes.ApplyActionOnAllItems(delegate(CheerBarkNodeItemVM n)
				{
					n.OnFinalize();
				});
			}
			InputKeyItemVM shortcutKey = this.ShortcutKey;
			if (shortcutKey != null)
			{
				shortcutKey.OnFinalize();
			}
			this.ShortcutKey = null;
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x000190D7 File Offset: 0x000172D7
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x000190DF File Offset: 0x000172DF
		[DataSourceProperty]
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

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x000190FD File Offset: 0x000172FD
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x00019105 File Offset: 0x00017305
		[DataSourceProperty]
		public MBBindingList<CheerBarkNodeItemVM> SubNodes
		{
			get
			{
				return this._subNodes;
			}
			set
			{
				if (value != this._subNodes)
				{
					this._subNodes = value;
					base.OnPropertyChangedWithValue<MBBindingList<CheerBarkNodeItemVM>>(value, "SubNodes");
				}
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x00019123 File Offset: 0x00017323
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x0001912B File Offset: 0x0001732B
		[DataSourceProperty]
		public string CheerNameText
		{
			get
			{
				return this._cheerNameText;
			}
			set
			{
				if (value != this._cheerNameText)
				{
					this._cheerNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "CheerNameText");
				}
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001914E File Offset: 0x0001734E
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x00019156 File Offset: 0x00017356
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x00019174 File Offset: 0x00017374
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x0001917C File Offset: 0x0001737C
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
					if (this._isSelected)
					{
						Action<CheerBarkNodeItemVM> onSelection = CheerBarkNodeItemVM.OnSelection;
						if (onSelection == null)
						{
							return;
						}
						onSelection(this);
					}
				}
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x000191B2 File Offset: 0x000173B2
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x000191BA File Offset: 0x000173BA
		[DataSourceProperty]
		public bool HasSubNodes
		{
			get
			{
				return this._hasSubNodes;
			}
			set
			{
				if (value != this._hasSubNodes)
				{
					this._hasSubNodes = value;
					base.OnPropertyChanged("HasSubNodes");
				}
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x000191D7 File Offset: 0x000173D7
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x000191DF File Offset: 0x000173DF
		[DataSourceProperty]
		public string TypeAsString
		{
			get
			{
				return this._typeAsString;
			}
			set
			{
				if (value != this._typeAsString)
				{
					this._typeAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeAsString");
				}
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00019202 File Offset: 0x00017402
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0001920A File Offset: 0x0001740A
		[DataSourceProperty]
		public string TauntVisualName
		{
			get
			{
				return this._tauntVisualName;
			}
			set
			{
				if (value != this._tauntVisualName)
				{
					this._tauntVisualName = value;
					base.OnPropertyChangedWithValue<string>(value, "TauntVisualName");
				}
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001922D File Offset: 0x0001742D
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x00019235 File Offset: 0x00017435
		[DataSourceProperty]
		public string SelectedNodeText
		{
			get
			{
				return this._selectedNodeText;
			}
			set
			{
				if (value != this._selectedNodeText)
				{
					this._selectedNodeText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedNodeText");
				}
			}
		}

		// Token: 0x0400030A RID: 778
		public readonly TauntUsageManager.TauntUsage.TauntUsageFlag TauntUsageDisabledReason;

		// Token: 0x0400030B RID: 779
		private readonly TextObject _nodeName;

		// Token: 0x0400030C RID: 780
		private InputKeyItemVM _shortcutKey;

		// Token: 0x0400030D RID: 781
		private MBBindingList<CheerBarkNodeItemVM> _subNodes;

		// Token: 0x0400030E RID: 782
		private string _cheerNameText;

		// Token: 0x0400030F RID: 783
		private string _typeAsString;

		// Token: 0x04000310 RID: 784
		private string _tauntVisualName;

		// Token: 0x04000311 RID: 785
		private string _selectedNodeText;

		// Token: 0x04000312 RID: 786
		private bool _isDisabled;

		// Token: 0x04000313 RID: 787
		private bool _isSelected;

		// Token: 0x04000314 RID: 788
		private bool _hasSubNodes;
	}
}
