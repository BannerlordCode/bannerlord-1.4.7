using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D6 RID: 214
	public class EncyclopediaPageVM : ViewModel
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x00051EC8 File Offset: 0x000500C8
		public object Obj
		{
			get
			{
				return this._args.Obj;
			}
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x00051ED5 File Offset: 0x000500D5
		public virtual string GetName()
		{
			return "";
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00051EDC File Offset: 0x000500DC
		public virtual string GetNavigationBarURL()
		{
			return "";
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00051EE3 File Offset: 0x000500E3
		public virtual void Refresh()
		{
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00051EE5 File Offset: 0x000500E5
		public EncyclopediaPageVM(EncyclopediaPageArgs args)
		{
			this._args = args;
			this.BookmarkHint = new HintViewModel();
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x00051EFF File Offset: 0x000500FF
		public virtual void OnTick()
		{
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x00051F01 File Offset: 0x00050101
		public virtual void ExecuteSwitchBookmarkedState()
		{
			this.IsBookmarked = !this.IsBookmarked;
			this.UpdateBookmarkHintText();
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00051F18 File Offset: 0x00050118
		protected void UpdateBookmarkHintText()
		{
			if (this.IsBookmarked)
			{
				this.BookmarkHint.HintText = new TextObject("{=BV5exuPf}Remove From Bookmarks", null);
				return;
			}
			this.BookmarkHint.HintText = new TextObject("{=d8jrv3nA}Add To Bookmarks", null);
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x00051F4F File Offset: 0x0005014F
		// (set) Token: 0x0600148E RID: 5262 RVA: 0x00051F57 File Offset: 0x00050157
		[DataSourceProperty]
		public bool IsLoadingOver
		{
			get
			{
				return this._isLoadingOver;
			}
			set
			{
				if (value != this._isLoadingOver)
				{
					this._isLoadingOver = value;
					base.OnPropertyChangedWithValue(value, "IsLoadingOver");
				}
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x00051F75 File Offset: 0x00050175
		// (set) Token: 0x06001490 RID: 5264 RVA: 0x00051F7D File Offset: 0x0005017D
		[DataSourceProperty]
		public bool IsBookmarked
		{
			get
			{
				return this._isBookmarked;
			}
			set
			{
				if (value != this._isBookmarked)
				{
					this._isBookmarked = value;
					base.OnPropertyChanged("IsBookmarked");
				}
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x00051F9A File Offset: 0x0005019A
		// (set) Token: 0x06001492 RID: 5266 RVA: 0x00051FA2 File Offset: 0x000501A2
		[DataSourceProperty]
		public HintViewModel BookmarkHint
		{
			get
			{
				return this._bookmarkHint;
			}
			set
			{
				if (value != this._bookmarkHint)
				{
					this._bookmarkHint = value;
					base.OnPropertyChanged("BookmarkHint");
				}
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x00051FBF File Offset: 0x000501BF
		// (set) Token: 0x06001494 RID: 5268 RVA: 0x00051FC2 File Offset: 0x000501C2
		[DataSourceProperty]
		public virtual MBBindingList<EncyclopediaListItemVM> Items
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x00051FC4 File Offset: 0x000501C4
		// (set) Token: 0x06001496 RID: 5270 RVA: 0x00051FC7 File Offset: 0x000501C7
		[DataSourceProperty]
		public virtual MBBindingList<EncyclopediaFilterGroupVM> FilterGroups
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x00051FC9 File Offset: 0x000501C9
		// (set) Token: 0x06001498 RID: 5272 RVA: 0x00051FCC File Offset: 0x000501CC
		[DataSourceProperty]
		public virtual EncyclopediaListSortControllerVM SortController
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x04000969 RID: 2409
		private EncyclopediaPageArgs _args;

		// Token: 0x0400096A RID: 2410
		private bool _isLoadingOver;

		// Token: 0x0400096B RID: 2411
		private bool _isBookmarked;

		// Token: 0x0400096C RID: 2412
		private HintViewModel _bookmarkHint;
	}
}
