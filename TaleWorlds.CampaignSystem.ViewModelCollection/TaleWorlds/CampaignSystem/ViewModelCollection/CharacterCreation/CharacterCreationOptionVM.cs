using System;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000154 RID: 340
	public class CharacterCreationOptionVM : ViewModel
	{
		// Token: 0x0600201C RID: 8220 RVA: 0x00075AD6 File Offset: 0x00073CD6
		public CharacterCreationOptionVM(Action<CharacterCreationOptionVM> onSelect, NarrativeMenuOption option)
		{
			this._onSelect = onSelect;
			this.Option = option;
			this.RefreshValues();
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x00075AF4 File Offset: 0x00073CF4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ActionText = this.Option.Text.ToString();
			this.PositiveEffectText = this.Option.PositiveEffectText.ToString();
			this.DescriptionText = this.Option.DescriptionText.ToString();
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x00075B49 File Offset: 0x00073D49
		public void ExecuteSelect()
		{
			Action<CharacterCreationOptionVM> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this);
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x0600201F RID: 8223 RVA: 0x00075B5C File Offset: 0x00073D5C
		// (set) Token: 0x06002020 RID: 8224 RVA: 0x00075B64 File Offset: 0x00073D64
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
				}
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x00075B82 File Offset: 0x00073D82
		// (set) Token: 0x06002022 RID: 8226 RVA: 0x00075B8A File Offset: 0x00073D8A
		[DataSourceProperty]
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionText");
				}
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x00075BAD File Offset: 0x00073DAD
		// (set) Token: 0x06002024 RID: 8228 RVA: 0x00075BB5 File Offset: 0x00073DB5
		[DataSourceProperty]
		public string PositiveEffectText
		{
			get
			{
				return this._positiveEffectText;
			}
			set
			{
				if (value != this._positiveEffectText)
				{
					this._positiveEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "PositiveEffectText");
				}
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x00075BD8 File Offset: 0x00073DD8
		// (set) Token: 0x06002026 RID: 8230 RVA: 0x00075BE0 File Offset: 0x00073DE0
		[DataSourceProperty]
		public string NegativeEffectText
		{
			get
			{
				return this._negativeEffectText;
			}
			set
			{
				if (value != this._negativeEffectText)
				{
					this._negativeEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "NegativeEffectText");
				}
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x00075C03 File Offset: 0x00073E03
		// (set) Token: 0x06002028 RID: 8232 RVA: 0x00075C0B File Offset: 0x00073E0B
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x04000EF1 RID: 3825
		public readonly NarrativeMenuOption Option;

		// Token: 0x04000EF2 RID: 3826
		private readonly Action<CharacterCreationOptionVM> _onSelect;

		// Token: 0x04000EF3 RID: 3827
		private bool _isSelected;

		// Token: 0x04000EF4 RID: 3828
		private string _actionText;

		// Token: 0x04000EF5 RID: 3829
		private string _positiveEffectText;

		// Token: 0x04000EF6 RID: 3830
		private string _negativeEffectText;

		// Token: 0x04000EF7 RID: 3831
		private string _descriptionText;
	}
}
