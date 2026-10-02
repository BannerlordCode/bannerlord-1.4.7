using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x0200006D RID: 109
	public class KingdomDiplomacyFactionItemVM : ViewModel
	{
		// Token: 0x060008E0 RID: 2272 RVA: 0x00027BEE File Offset: 0x00025DEE
		public KingdomDiplomacyFactionItemVM(IFaction faction)
		{
			this.Hint = new HintViewModel(faction.Name, null);
			this.Visual = new BannerImageIdentifierVM(faction.Banner, true);
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00027C1A File Offset: 0x00025E1A
		// (set) Token: 0x060008E2 RID: 2274 RVA: 0x00027C22 File Offset: 0x00025E22
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

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00027C40 File Offset: 0x00025E40
		// (set) Token: 0x060008E4 RID: 2276 RVA: 0x00027C48 File Offset: 0x00025E48
		[DataSourceProperty]
		public BannerImageIdentifierVM Visual
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
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x040003EA RID: 1002
		private HintViewModel _hint;

		// Token: 0x040003EB RID: 1003
		private BannerImageIdentifierVM _visual;
	}
}
