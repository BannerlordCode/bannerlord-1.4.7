using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000CA RID: 202
	public class EncyclopediaLinkVM : ViewModel
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001343 RID: 4931 RVA: 0x0004DDFC File Offset: 0x0004BFFC
		// (set) Token: 0x06001344 RID: 4932 RVA: 0x0004DE04 File Offset: 0x0004C004
		[DataSourceProperty]
		public string ActiveLink
		{
			get
			{
				return this._activeLink;
			}
			set
			{
				if (this._activeLink != value)
				{
					this._activeLink = value;
					base.OnPropertyChangedWithValue<string>(value, "ActiveLink");
				}
			}
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0004DE27 File Offset: 0x0004C027
		public void ExecuteActiveLink()
		{
			if (!string.IsNullOrEmpty(this.ActiveLink))
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.ActiveLink);
			}
		}

		// Token: 0x040008D2 RID: 2258
		private string _activeLink;
	}
}
