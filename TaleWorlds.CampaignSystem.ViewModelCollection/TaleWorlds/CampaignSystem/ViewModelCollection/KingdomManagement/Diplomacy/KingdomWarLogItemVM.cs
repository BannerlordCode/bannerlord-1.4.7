using System;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000073 RID: 115
	public class KingdomWarLogItemVM : ViewModel
	{
		// Token: 0x06000969 RID: 2409 RVA: 0x00029E58 File Offset: 0x00028058
		public KingdomWarLogItemVM(IEncyclopediaLog log, IFaction effectorFaction)
		{
			this._log = log;
			this.Banner = new BannerImageIdentifierVM(effectorFaction.Banner, true);
			this.RefreshValues();
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00029E80 File Offset: 0x00028080
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.WarLogTimeText = this._log.GameTime.ToString();
			this.WarLogText = this._log.GetEncyclopediaText().ToString();
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00029EC8 File Offset: 0x000280C8
		private void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x00029EDA File Offset: 0x000280DA
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x00029EE2 File Offset: 0x000280E2
		[DataSourceProperty]
		public string WarLogTimeText
		{
			get
			{
				return this._warLogTimeText;
			}
			set
			{
				if (value != this._warLogTimeText)
				{
					this._warLogTimeText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarLogTimeText");
				}
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x00029F05 File Offset: 0x00028105
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x00029F0D File Offset: 0x0002810D
		[DataSourceProperty]
		public string WarLogText
		{
			get
			{
				return this._warLogText;
			}
			set
			{
				if (value != this._warLogText)
				{
					this._warLogText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarLogText");
				}
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x00029F30 File Offset: 0x00028130
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x00029F38 File Offset: 0x00028138
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner)
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x0400042A RID: 1066
		private readonly IEncyclopediaLog _log;

		// Token: 0x0400042B RID: 1067
		private string _warLogText;

		// Token: 0x0400042C RID: 1068
		private string _warLogTimeText;

		// Token: 0x0400042D RID: 1069
		private BannerImageIdentifierVM _banner;
	}
}
