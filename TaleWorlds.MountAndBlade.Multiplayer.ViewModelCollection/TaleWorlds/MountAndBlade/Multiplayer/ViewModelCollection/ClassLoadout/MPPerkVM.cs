using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A6 RID: 166
	public class MPPerkVM : ViewModel
	{
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x00030CF2 File Offset: 0x0002EEF2
		// (set) Token: 0x06000FDC RID: 4060 RVA: 0x00030CFA File Offset: 0x0002EEFA
		public int PerkIndex { get; private set; }

		// Token: 0x06000FDD RID: 4061 RVA: 0x00030D03 File Offset: 0x0002EF03
		public MPPerkVM(Action<MPPerkVM> onSelectPerk, IReadOnlyPerkObject perk, bool isSelectable, int perkIndex)
		{
			this.Perk = perk;
			this.PerkIndex = perkIndex;
			this._onSelectPerk = onSelectPerk;
			this.IconType = perk.IconId;
			this.IsSelectable = isSelectable;
			this.RefreshValues();
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00030D3C File Offset: 0x0002EF3C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Perk.Name.ToString();
			this.Description = this.Perk.Description.ToString();
			GameTexts.SetVariable("newline", "\n");
			this.Hint = new HintViewModel(this.Perk.Description, null);
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00030DA1 File Offset: 0x0002EFA1
		public void ExecuteSelectPerk()
		{
			this._onSelectPerk(this);
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x00030DAF File Offset: 0x0002EFAF
		// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x00030DB7 File Offset: 0x0002EFB7
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
				}
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x00030DDA File Offset: 0x0002EFDA
		// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x00030DE2 File Offset: 0x0002EFE2
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x00030E00 File Offset: 0x0002F000
		// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x00030E08 File Offset: 0x0002F008
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x00030E2B File Offset: 0x0002F02B
		// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x00030E33 File Offset: 0x0002F033
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x00030E56 File Offset: 0x0002F056
		// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x00030E5E File Offset: 0x0002F05E
		[DataSourceProperty]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (value != this._isSelectable)
				{
					this._isSelectable = value;
					base.OnPropertyChangedWithValue(value, "IsSelectable");
				}
			}
		}

		// Token: 0x04000760 RID: 1888
		public readonly IReadOnlyPerkObject Perk;

		// Token: 0x04000761 RID: 1889
		private readonly Action<MPPerkVM> _onSelectPerk;

		// Token: 0x04000763 RID: 1891
		private string _iconType;

		// Token: 0x04000764 RID: 1892
		private string _name;

		// Token: 0x04000765 RID: 1893
		private string _description;

		// Token: 0x04000766 RID: 1894
		private bool _isSelectable;

		// Token: 0x04000767 RID: 1895
		private HintViewModel _hint;
	}
}
