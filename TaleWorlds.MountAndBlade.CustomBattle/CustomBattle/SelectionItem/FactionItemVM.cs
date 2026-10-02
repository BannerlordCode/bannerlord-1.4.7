using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000020 RID: 32
	public class FactionItemVM : ViewModel
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000A684 File Offset: 0x00008884
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x0000A68C File Offset: 0x0000888C
		public BasicCultureObject Faction { get; private set; }

		// Token: 0x060001BA RID: 442 RVA: 0x0000A695 File Offset: 0x00008895
		public FactionItemVM(BasicCultureObject faction, Action<FactionItemVM> onSelected)
		{
			this.Faction = faction;
			this._onSelected = onSelected;
			this.CultureCode = faction.StringId.ToLower();
			this.Hint = new HintViewModel(faction.Name, null);
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000A6CE File Offset: 0x000088CE
		// (set) Token: 0x060001BC RID: 444 RVA: 0x0000A6D6 File Offset: 0x000088D6
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

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000A6F4 File Offset: 0x000088F4
		// (set) Token: 0x060001BE RID: 446 RVA: 0x0000A6FC File Offset: 0x000088FC
		[DataSourceProperty]
		public string CultureCode
		{
			get
			{
				return this._cultureCode;
			}
			set
			{
				if (value != this._cultureCode)
				{
					this._cultureCode = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureCode");
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000A71F File Offset: 0x0000891F
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x0000A727 File Offset: 0x00008927
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
					if (value)
					{
						this._onSelected(this);
					}
				}
			}
		}

		// Token: 0x0400010A RID: 266
		private Action<FactionItemVM> _onSelected;

		// Token: 0x0400010B RID: 267
		private HintViewModel _hint;

		// Token: 0x0400010C RID: 268
		private string _cultureCode;

		// Token: 0x0400010D RID: 269
		private bool _isSelected;
	}
}
