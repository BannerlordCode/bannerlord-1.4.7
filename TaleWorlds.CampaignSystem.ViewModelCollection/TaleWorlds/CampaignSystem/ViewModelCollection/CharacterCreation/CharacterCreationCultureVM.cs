using System;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x0200014E RID: 334
	public class CharacterCreationCultureVM : ViewModel
	{
		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06001FCD RID: 8141 RVA: 0x00074774 File Offset: 0x00072974
		public CultureObject Culture { get; }

		// Token: 0x06001FCE RID: 8142 RVA: 0x0007477C File Offset: 0x0007297C
		public CharacterCreationCultureVM(CultureObject culture, Action<CharacterCreationCultureVM> onSelection)
		{
			this._onSelection = onSelection;
			this.Culture = culture;
			CharacterCreationState characterCreationState = GameStateManager.Current.ActiveState as CharacterCreationState;
			CharacterCreationContent characterCreationContent = ((characterCreationState != null) ? characterCreationState.CharacterCreationManager.CharacterCreationContent : null);
			MBTextManager.SetTextVariable("FOCUS_VALUE", characterCreationContent.GetFocusToAddByCulture(culture));
			MBTextManager.SetTextVariable("EXP_VALUE", characterCreationContent.GetSkillLevelToAddByCulture(culture));
			this.DescriptionText = GameTexts.FindText("str_culture_description", this.Culture.StringId).ToString();
			this.ShortenedNameText = GameTexts.FindText("str_culture_rich_name", this.Culture.StringId).ToString();
			this.NameText = GameTexts.FindText("str_culture_rich_name", this.Culture.StringId).ToString();
			this.CultureID = ((culture != null) ? culture.StringId : null) ?? "";
			this.CultureColor1 = Color.FromUint((culture != null) ? culture.Color : Color.White.ToUnsignedInteger());
			this.Feats = new MBBindingList<CharacterCreationCultureFeatVM>();
			foreach (FeatObject featObject in this.Culture.GetCulturalFeats((FeatObject x) => x.IsPositive))
			{
				this.Feats.Add(new CharacterCreationCultureFeatVM(true, featObject.Description.ToString()));
			}
			foreach (FeatObject featObject2 in this.Culture.GetCulturalFeats((FeatObject x) => !x.IsPositive))
			{
				this.Feats.Add(new CharacterCreationCultureFeatVM(false, featObject2.Description.ToString()));
			}
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x00074984 File Offset: 0x00072B84
		public void ExecuteSelectCulture()
		{
			this._onSelection(this);
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x00074992 File Offset: 0x00072B92
		// (set) Token: 0x06001FD1 RID: 8145 RVA: 0x0007499A File Offset: 0x00072B9A
		[DataSourceProperty]
		public string CultureID
		{
			get
			{
				return this._cultureID;
			}
			set
			{
				if (value != this._cultureID)
				{
					this._cultureID = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureID");
				}
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x000749BD File Offset: 0x00072BBD
		// (set) Token: 0x06001FD3 RID: 8147 RVA: 0x000749C5 File Offset: 0x00072BC5
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x000749E8 File Offset: 0x00072BE8
		// (set) Token: 0x06001FD5 RID: 8149 RVA: 0x000749F0 File Offset: 0x00072BF0
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

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x00074A13 File Offset: 0x00072C13
		// (set) Token: 0x06001FD7 RID: 8151 RVA: 0x00074A1B File Offset: 0x00072C1B
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x00074A3E File Offset: 0x00072C3E
		// (set) Token: 0x06001FD9 RID: 8153 RVA: 0x00074A46 File Offset: 0x00072C46
		[DataSourceProperty]
		public string ShortenedNameText
		{
			get
			{
				return this._shortenedNameText;
			}
			set
			{
				if (value != this._shortenedNameText)
				{
					this._shortenedNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShortenedNameText");
				}
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x00074A69 File Offset: 0x00072C69
		// (set) Token: 0x06001FDB RID: 8155 RVA: 0x00074A71 File Offset: 0x00072C71
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

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x00074A8F File Offset: 0x00072C8F
		// (set) Token: 0x06001FDD RID: 8157 RVA: 0x00074A97 File Offset: 0x00072C97
		[DataSourceProperty]
		public MBBindingList<CharacterCreationCultureFeatVM> Feats
		{
			get
			{
				return this._feats;
			}
			set
			{
				if (value != this._feats)
				{
					this._feats = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterCreationCultureFeatVM>>(value, "Feats");
				}
			}
		}

		// Token: 0x04000ED0 RID: 3792
		private readonly Action<CharacterCreationCultureVM> _onSelection;

		// Token: 0x04000ED1 RID: 3793
		private string _descriptionText = "";

		// Token: 0x04000ED2 RID: 3794
		private string _nameText;

		// Token: 0x04000ED3 RID: 3795
		private string _shortenedNameText;

		// Token: 0x04000ED4 RID: 3796
		private bool _isSelected;

		// Token: 0x04000ED5 RID: 3797
		private string _cultureID;

		// Token: 0x04000ED6 RID: 3798
		private Color _cultureColor1;

		// Token: 0x04000ED7 RID: 3799
		private MBBindingList<CharacterCreationCultureFeatVM> _feats;
	}
}
