using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.BannerBuilder
{
	// Token: 0x02000084 RID: 132
	public class BannerBuilderCategoryVM : ViewModel
	{
		// Token: 0x06000AD7 RID: 2775 RVA: 0x00027194 File Offset: 0x00025394
		public BannerBuilderCategoryVM(BannerIconGroup category, Action<BannerBuilderItemVM> onItemSelection)
		{
			this.ItemsList = new MBBindingList<BannerBuilderItemVM>();
			this._category = category;
			this._onItemSelection = onItemSelection;
			this.IsPattern = this._category.IsPattern;
			this.IsEnabled = true;
			this.PopulateItems();
			this.RefreshValues();
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000271E4 File Offset: 0x000253E4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Title = this._category.Name.ToString();
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00027204 File Offset: 0x00025404
		private void PopulateItems()
		{
			this.ItemsList.Clear();
			if (this.IsPattern)
			{
				for (int i = 0; i < this._category.AllBackgrounds.Count; i++)
				{
					KeyValuePair<int, string> keyValuePair = this._category.AllBackgrounds.ElementAt<KeyValuePair<int, string>>(i);
					this.ItemsList.Add(new BannerBuilderItemVM(keyValuePair.Key, keyValuePair.Value, this._onItemSelection));
				}
				return;
			}
			for (int j = 0; j < this._category.AllIcons.Count; j++)
			{
				KeyValuePair<int, BannerIconData> keyValuePair2 = this._category.AllIcons.ElementAt<KeyValuePair<int, BannerIconData>>(j);
				this.ItemsList.Add(new BannerBuilderItemVM(keyValuePair2.Key, keyValuePair2.Value, this._onItemSelection));
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x000272C7 File Offset: 0x000254C7
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x000272CF File Offset: 0x000254CF
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

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x000272F2 File Offset: 0x000254F2
		// (set) Token: 0x06000ADD RID: 2781 RVA: 0x000272FA File Offset: 0x000254FA
		[DataSourceProperty]
		public bool IsPattern
		{
			get
			{
				return this._isPattern;
			}
			set
			{
				if (value != this._isPattern)
				{
					this._isPattern = value;
					base.OnPropertyChangedWithValue(value, "IsPattern");
				}
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x00027318 File Offset: 0x00025518
		// (set) Token: 0x06000ADF RID: 2783 RVA: 0x00027320 File Offset: 0x00025520
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0002733E File Offset: 0x0002553E
		// (set) Token: 0x06000AE1 RID: 2785 RVA: 0x00027346 File Offset: 0x00025546
		[DataSourceProperty]
		public MBBindingList<BannerBuilderItemVM> ItemsList
		{
			get
			{
				return this._itemsList;
			}
			set
			{
				if (value != this._itemsList)
				{
					this._itemsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BannerBuilderItemVM>>(value, "ItemsList");
				}
			}
		}

		// Token: 0x040004F0 RID: 1264
		private readonly BannerIconGroup _category;

		// Token: 0x040004F1 RID: 1265
		private readonly Action<BannerBuilderItemVM> _onItemSelection;

		// Token: 0x040004F2 RID: 1266
		private string _title;

		// Token: 0x040004F3 RID: 1267
		private bool _isPattern;

		// Token: 0x040004F4 RID: 1268
		private bool _isEnabled;

		// Token: 0x040004F5 RID: 1269
		private MBBindingList<BannerBuilderItemVM> _itemsList;
	}
}
