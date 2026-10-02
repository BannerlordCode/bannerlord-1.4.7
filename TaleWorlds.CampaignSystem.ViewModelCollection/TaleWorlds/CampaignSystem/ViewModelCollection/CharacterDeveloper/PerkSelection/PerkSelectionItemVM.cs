using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection
{
	// Token: 0x02000147 RID: 327
	public class PerkSelectionItemVM : ViewModel
	{
		// Token: 0x06001F73 RID: 8051 RVA: 0x0007387E File Offset: 0x00071A7E
		public PerkSelectionItemVM(PerkObject perk, Action<PerkSelectionItemVM> onSelection)
		{
			this.Perk = perk;
			this._onSelection = onSelection;
			this.RefreshValues();
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0007389C File Offset: 0x00071A9C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PickText = new TextObject("{=1CXlqb2U}Pick:", null).ToString();
			this.PerkName = this.Perk.Name.ToString();
			this.PerkDescription = this.Perk.Description.ToString();
			TextObject combinedPerkRoleText = CampaignUIHelper.GetCombinedPerkRoleText(this.Perk);
			this.PerkRole = ((combinedPerkRoleText != null) ? combinedPerkRoleText.ToString() : null) ?? "";
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x00073917 File Offset: 0x00071B17
		public void ExecuteSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06001F76 RID: 8054 RVA: 0x00073925 File Offset: 0x00071B25
		// (set) Token: 0x06001F77 RID: 8055 RVA: 0x0007392D File Offset: 0x00071B2D
		[DataSourceProperty]
		public string PickText
		{
			get
			{
				return this._pickText;
			}
			set
			{
				if (value != this._pickText)
				{
					this._pickText = value;
					base.OnPropertyChangedWithValue<string>(value, "PickText");
				}
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06001F78 RID: 8056 RVA: 0x00073950 File Offset: 0x00071B50
		// (set) Token: 0x06001F79 RID: 8057 RVA: 0x00073958 File Offset: 0x00071B58
		[DataSourceProperty]
		public string PerkName
		{
			get
			{
				return this._perkName;
			}
			set
			{
				if (value != this._perkName)
				{
					this._perkName = value;
					base.OnPropertyChangedWithValue<string>(value, "PerkName");
				}
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06001F7A RID: 8058 RVA: 0x0007397B File Offset: 0x00071B7B
		// (set) Token: 0x06001F7B RID: 8059 RVA: 0x00073983 File Offset: 0x00071B83
		[DataSourceProperty]
		public string PerkDescription
		{
			get
			{
				return this._perkDescription;
			}
			set
			{
				if (value != this._perkDescription)
				{
					this._perkDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "PerkDescription");
				}
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06001F7C RID: 8060 RVA: 0x000739A6 File Offset: 0x00071BA6
		// (set) Token: 0x06001F7D RID: 8061 RVA: 0x000739AE File Offset: 0x00071BAE
		[DataSourceProperty]
		public string PerkRole
		{
			get
			{
				return this._perkRole;
			}
			set
			{
				if (value != this._perkRole)
				{
					this._perkRole = value;
					base.OnPropertyChangedWithValue<string>(value, "PerkRole");
				}
			}
		}

		// Token: 0x04000EAD RID: 3757
		private readonly Action<PerkSelectionItemVM> _onSelection;

		// Token: 0x04000EAE RID: 3758
		public readonly PerkObject Perk;

		// Token: 0x04000EAF RID: 3759
		private string _pickText;

		// Token: 0x04000EB0 RID: 3760
		private string _perkName;

		// Token: 0x04000EB1 RID: 3761
		private string _perkDescription;

		// Token: 0x04000EB2 RID: 3762
		private string _perkRole;
	}
}
