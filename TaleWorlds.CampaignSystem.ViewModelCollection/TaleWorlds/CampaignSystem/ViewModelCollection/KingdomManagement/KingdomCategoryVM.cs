using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement
{
	// Token: 0x02000062 RID: 98
	public abstract class KingdomCategoryVM : ViewModel
	{
		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x000233FD File Offset: 0x000215FD
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x00023405 File Offset: 0x00021605
		[DataSourceProperty]
		public string CategoryNameText
		{
			get
			{
				return this._categoryNameText;
			}
			set
			{
				if (value != this._categoryNameText)
				{
					this._categoryNameText = value;
					base.OnPropertyChanged("NameText");
				}
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x00023427 File Offset: 0x00021627
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x0002342F File Offset: 0x0002162F
		[DataSourceProperty]
		public string NoItemSelectedText
		{
			get
			{
				return this._noItemSelectedText;
			}
			set
			{
				if (value != this._noItemSelectedText)
				{
					this._noItemSelectedText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoItemSelectedText");
				}
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00023452 File Offset: 0x00021652
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x0002345A File Offset: 0x0002165A
		[DataSourceProperty]
		public bool IsAcceptableItemSelected
		{
			get
			{
				return this._isAcceptableItemSelected;
			}
			set
			{
				if (value != this._isAcceptableItemSelected)
				{
					this._isAcceptableItemSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAcceptableItemSelected");
				}
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00023478 File Offset: 0x00021678
		// (set) Token: 0x06000762 RID: 1890 RVA: 0x00023480 File Offset: 0x00021680
		[DataSourceProperty]
		public int NotificationCount
		{
			get
			{
				return this._notificationCount;
			}
			set
			{
				if (value != this._notificationCount)
				{
					this._notificationCount = value;
					base.OnPropertyChanged("NotificationCount");
				}
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x0002349D File Offset: 0x0002169D
		// (set) Token: 0x06000764 RID: 1892 RVA: 0x000234A5 File Offset: 0x000216A5
		[DataSourceProperty]
		public bool Show
		{
			get
			{
				return this._show;
			}
			set
			{
				if (value != this._show)
				{
					this._show = value;
					base.OnPropertyChanged("Show");
				}
			}
		}

		// Token: 0x04000334 RID: 820
		private int _notificationCount;

		// Token: 0x04000335 RID: 821
		private string _categoryNameText;

		// Token: 0x04000336 RID: 822
		private string _noItemSelectedText;

		// Token: 0x04000337 RID: 823
		private bool _show;

		// Token: 0x04000338 RID: 824
		private bool _isAcceptableItemSelected;
	}
}
