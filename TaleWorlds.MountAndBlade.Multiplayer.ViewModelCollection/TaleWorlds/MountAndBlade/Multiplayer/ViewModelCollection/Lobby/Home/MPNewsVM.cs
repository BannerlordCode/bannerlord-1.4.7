using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Library.NewsManager;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x02000051 RID: 81
	public class MPNewsVM : ViewModel
	{
		// Token: 0x06000716 RID: 1814 RVA: 0x000167D0 File Offset: 0x000149D0
		public MPNewsVM(NewsManager newsManager)
		{
			this._newsManager = newsManager;
			this.ImportantNews = new MBBindingList<MPNewsItemVM>();
			this.GetNewsItems();
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000167F0 File Offset: 0x000149F0
		private async void GetNewsItems()
		{
			if (this._newsManager == null)
			{
				Debug.FailedAssert("News manager is null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Home\\MPNewsVM.cs", "GetNewsItems", 27);
			}
			else
			{
				MBReadOnlyList<NewsItem> mbreadOnlyList = await this._newsManager.GetNewsItems(false);
				this._newsItemsCached = mbreadOnlyList;
				this.RefreshNews();
			}
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001682C File Offset: 0x00014A2C
		private void RefreshNews()
		{
			this.MainNews = null;
			this.ImportantNews.Clear();
			this.HasValidNews = false;
			if (this._newsItemsCached == null)
			{
				Debug.FailedAssert("News items list is null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Home\\MPNewsVM.cs", "RefreshNews", 43);
				return;
			}
			List<IGrouping<int, NewsItem>> list = (from i in (from i in this._newsItemsCached.Where<NewsItem>((NewsItem n) => n.Feeds.Any<NewsType>((NewsType t) => t.Type == NewsItem.NewsTypes.MultiplayerLobby) && !string.IsNullOrEmpty(n.Title) && !string.IsNullOrEmpty(n.NewsLink) && !string.IsNullOrEmpty(n.ImageSourcePath)).ToList<NewsItem>()
					group i by i.Feeds.First<NewsType>((NewsType t) => t.Type == NewsItem.NewsTypes.MultiplayerLobby).Index).ToList<IGrouping<int, NewsItem>>()
				orderby i.Key
				select i).ToList<IGrouping<int, NewsItem>>();
			int num = 0;
			while (num < list.Count && this.ImportantNews.Count + 1 < 4)
			{
				NewsItem newsItem = list[num].First<NewsItem>();
				NewsItem newsItem2 = (newsItem.Equals(default(NewsItem)) ? default(NewsItem) : newsItem);
				if (num == 0)
				{
					this.MainNews = new MPNewsItemVM(newsItem2);
				}
				else
				{
					this.ImportantNews.Add(new MPNewsItemVM(newsItem2));
				}
				num++;
			}
			if (this.MainNews != null)
			{
				this.HasValidNews = true;
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001697E File Offset: 0x00014B7E
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._newsManager = null;
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0001698D File Offset: 0x00014B8D
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x00016995 File Offset: 0x00014B95
		[DataSourceProperty]
		public bool HasValidNews
		{
			get
			{
				return this._hasValidNews;
			}
			set
			{
				if (value != this._hasValidNews)
				{
					this._hasValidNews = value;
					base.OnPropertyChangedWithValue(value, "HasValidNews");
				}
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x000169B3 File Offset: 0x00014BB3
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x000169BB File Offset: 0x00014BBB
		[DataSourceProperty]
		public MPNewsItemVM MainNews
		{
			get
			{
				return this._mainNews;
			}
			set
			{
				if (value != this._mainNews)
				{
					this._mainNews = value;
					base.OnPropertyChangedWithValue<MPNewsItemVM>(value, "MainNews");
				}
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x000169D9 File Offset: 0x00014BD9
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x000169E1 File Offset: 0x00014BE1
		[DataSourceProperty]
		public MBBindingList<MPNewsItemVM> ImportantNews
		{
			get
			{
				return this._importantNews;
			}
			set
			{
				if (value != this._importantNews)
				{
					this._importantNews = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPNewsItemVM>>(value, "ImportantNews");
				}
			}
		}

		// Token: 0x0400034D RID: 845
		private NewsManager _newsManager;

		// Token: 0x0400034E RID: 846
		private const int _numOfNewsItemsToShow = 4;

		// Token: 0x0400034F RID: 847
		private MBReadOnlyList<NewsItem> _newsItemsCached;

		// Token: 0x04000350 RID: 848
		private bool _hasValidNews;

		// Token: 0x04000351 RID: 849
		private MPNewsItemVM _mainNews;

		// Token: 0x04000352 RID: 850
		private MBBindingList<MPNewsItemVM> _importantNews;
	}
}
