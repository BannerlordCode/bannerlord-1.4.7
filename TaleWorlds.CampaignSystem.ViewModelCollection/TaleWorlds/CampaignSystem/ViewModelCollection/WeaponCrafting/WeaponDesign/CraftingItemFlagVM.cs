using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000100 RID: 256
	public class CraftingItemFlagVM : ItemFlagVM
	{
		// Token: 0x0600175D RID: 5981 RVA: 0x0005A348 File Offset: 0x00058548
		public CraftingItemFlagVM(string iconPath, TextObject hint, bool isDisplayed)
			: base(iconPath, hint)
		{
			this.IsDisplayed = isDisplayed;
			this.IconPath = "SPGeneral\\" + iconPath;
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x0005A36A File Offset: 0x0005856A
		// (set) Token: 0x0600175F RID: 5983 RVA: 0x0005A372 File Offset: 0x00058572
		[DataSourceProperty]
		public bool IsDisplayed
		{
			get
			{
				return this._isDisplayed;
			}
			set
			{
				if (value != this._isDisplayed)
				{
					this._isDisplayed = value;
					base.OnPropertyChangedWithValue(value, "IsDisplayed");
				}
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06001760 RID: 5984 RVA: 0x0005A390 File Offset: 0x00058590
		// (set) Token: 0x06001761 RID: 5985 RVA: 0x0005A398 File Offset: 0x00058598
		[DataSourceProperty]
		public string IconPath
		{
			get
			{
				return this._iconPath;
			}
			set
			{
				if (value != this._iconPath)
				{
					this._iconPath = value;
					base.OnPropertyChangedWithValue<string>(value, "IconPath");
				}
			}
		}

		// Token: 0x04000AAD RID: 2733
		private bool _isDisplayed;

		// Token: 0x04000AAE RID: 2734
		private string _iconPath;
	}
}
