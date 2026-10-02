using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000EC RID: 236
	public class EncyclopediaTraitItemVM : ViewModel
	{
		// Token: 0x060015CB RID: 5579 RVA: 0x00055904 File Offset: 0x00053B04
		public EncyclopediaTraitItemVM(TraitObject traitObj, int value)
		{
			this._traitObj = traitObj;
			this.TraitId = traitObj.StringId;
			this.Value = value;
			string traitTooltipText = CampaignUIHelper.GetTraitTooltipText(traitObj, this.Value);
			this.Hint = new HintViewModel(new TextObject("{=!}" + traitTooltipText, null), null);
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x0005595B File Offset: 0x00053B5B
		public EncyclopediaTraitItemVM(TraitObject traitObj, Hero hero)
			: this(traitObj, hero.GetTraitLevel(traitObj))
		{
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x0005596B File Offset: 0x00053B6B
		// (set) Token: 0x060015CE RID: 5582 RVA: 0x00055973 File Offset: 0x00053B73
		[DataSourceProperty]
		public string TraitId
		{
			get
			{
				return this._traitId;
			}
			set
			{
				if (value != this._traitId)
				{
					this._traitId = value;
					base.OnPropertyChangedWithValue<string>(value, "TraitId");
				}
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x00055996 File Offset: 0x00053B96
		// (set) Token: 0x060015D0 RID: 5584 RVA: 0x0005599E File Offset: 0x00053B9E
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

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x060015D1 RID: 5585 RVA: 0x000559BC File Offset: 0x00053BBC
		// (set) Token: 0x060015D2 RID: 5586 RVA: 0x000559C4 File Offset: 0x00053BC4
		[DataSourceProperty]
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue(value, "Value");
				}
			}
		}

		// Token: 0x040009E8 RID: 2536
		private readonly TraitObject _traitObj;

		// Token: 0x040009E9 RID: 2537
		private string _traitId;

		// Token: 0x040009EA RID: 2538
		private int _value;

		// Token: 0x040009EB RID: 2539
		private HintViewModel _hint;
	}
}
