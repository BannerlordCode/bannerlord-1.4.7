using System;
using System.Diagnostics;
using TaleWorlds.Library;
using TaleWorlds.Library.NewsManager;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x02000050 RID: 80
	public class MPNewsItemVM : ViewModel
	{
		// Token: 0x0600070E RID: 1806 RVA: 0x000166C0 File Offset: 0x000148C0
		public MPNewsItemVM(NewsItem item)
		{
			this.NewsImageUrl = item.ImageSourcePath;
			this.Category = item.Title;
			this.Title = item.Description;
			this._link = item.NewsLink + "?referrer=lobby";
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00016711 File Offset: 0x00014911
		private void ExecuteOpenLink()
		{
			if (!string.IsNullOrEmpty(this._link) && !PlatformServices.Instance.ShowOverlayForWebPage(this._link).Result)
			{
				Process.Start(new ProcessStartInfo(this._link)
				{
					UseShellExecute = true
				});
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x0001674F File Offset: 0x0001494F
		// (set) Token: 0x06000711 RID: 1809 RVA: 0x00016757 File Offset: 0x00014957
		[DataSourceProperty]
		public string NewsImageUrl
		{
			get
			{
				return this._newsImageUrl;
			}
			set
			{
				if (value != this._newsImageUrl)
				{
					this._newsImageUrl = value;
					base.OnPropertyChangedWithValue<string>(value, "NewsImageUrl");
				}
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x0001677A File Offset: 0x0001497A
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x00016782 File Offset: 0x00014982
		[DataSourceProperty]
		public string Category
		{
			get
			{
				return this._category;
			}
			set
			{
				if (value != this._category)
				{
					this._category = value;
					base.OnPropertyChangedWithValue<string>(value, "Category");
				}
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x000167A5 File Offset: 0x000149A5
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x000167AD File Offset: 0x000149AD
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x04000349 RID: 841
		private readonly string _link;

		// Token: 0x0400034A RID: 842
		private string _newsImageUrl;

		// Token: 0x0400034B RID: 843
		private string _category;

		// Token: 0x0400034C RID: 844
		private string _title;
	}
}
