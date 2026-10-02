using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000A4 RID: 164
	public class SettlementGovernorSelectionItemVM : ViewModel
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06000FEB RID: 4075 RVA: 0x0004191C File Offset: 0x0003FB1C
		public Hero Governor { get; }

		// Token: 0x06000FEC RID: 4076 RVA: 0x00041924 File Offset: 0x0003FB24
		public SettlementGovernorSelectionItemVM(Hero governor, Action<SettlementGovernorSelectionItemVM> onSelection)
		{
			this.Governor = governor;
			this._onSelection = onSelection;
			if (governor != null)
			{
				this.Visual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode(this.Governor.CharacterObject, true));
				this.GovernorHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetHeroGovernorEffectsTooltip(this.Governor, Settlement.CurrentSettlement));
			}
			else
			{
				this.Visual = new CharacterImageIdentifierVM(null);
				this.GovernorHint = new BasicTooltipViewModel();
			}
			this.RefreshValues();
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0004199C File Offset: 0x0003FB9C
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Governor != null)
			{
				this.Name = this.Governor.Name.ToString();
				return;
			}
			this.Visual = new CharacterImageIdentifierVM(null);
			this.Name = new TextObject("{=koX9okuG}None", null).ToString();
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x000419F0 File Offset: 0x0003FBF0
		public void OnSelection()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			Hero hero;
			if (currentSettlement == null)
			{
				hero = null;
			}
			else
			{
				Town town = currentSettlement.Town;
				hero = ((town != null) ? town.Governor : null);
			}
			Hero hero2 = hero;
			bool flag = this.Governor == null;
			if (hero2 != this.Governor && (!flag || hero2 != null))
			{
				ValueTuple<TextObject, TextObject> governorSelectionConfirmationPopupTexts = CampaignUIHelper.GetGovernorSelectionConfirmationPopupTexts(hero2, this.Governor, currentSettlement);
				InformationManager.ShowInquiry(new InquiryData(governorSelectionConfirmationPopupTexts.Item1.ToString(), governorSelectionConfirmationPopupTexts.Item2.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					this._onSelection(this);
				}, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x00041AA1 File Offset: 0x0003FCA1
		// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x00041AA9 File Offset: 0x0003FCA9
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x00041AC7 File Offset: 0x0003FCC7
		// (set) Token: 0x06000FF2 RID: 4082 RVA: 0x00041ACF File Offset: 0x0003FCCF
		[DataSourceProperty]
		public BasicTooltipViewModel GovernorHint
		{
			get
			{
				return this._governorHint;
			}
			set
			{
				if (value != this._governorHint)
				{
					this._governorHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "GovernorHint");
				}
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x00041AED File Offset: 0x0003FCED
		// (set) Token: 0x06000FF4 RID: 4084 RVA: 0x00041AF5 File Offset: 0x0003FCF5
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

		// Token: 0x04000743 RID: 1859
		private readonly Action<SettlementGovernorSelectionItemVM> _onSelection;

		// Token: 0x04000745 RID: 1861
		private CharacterImageIdentifierVM _visual;

		// Token: 0x04000746 RID: 1862
		private string _name;

		// Token: 0x04000747 RID: 1863
		private BasicTooltipViewModel _governorHint;
	}
}
