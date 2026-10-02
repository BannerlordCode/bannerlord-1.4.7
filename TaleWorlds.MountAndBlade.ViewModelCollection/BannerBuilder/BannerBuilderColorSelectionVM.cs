using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.BannerBuilder
{
	// Token: 0x02000086 RID: 134
	public class BannerBuilderColorSelectionVM : ViewModel
	{
		// Token: 0x06000AEC RID: 2796 RVA: 0x00027434 File Offset: 0x00025634
		public BannerBuilderColorSelectionVM()
		{
			this.Items = new MBBindingList<BannerBuilderColorItemVM>();
			this.PopulateItems();
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00027450 File Offset: 0x00025650
		public void EnableWith(int selectedColorID, Action<BannerBuilderColorItemVM> onSelection)
		{
			this._onSelection = onSelection;
			this.Items.ApplyActionOnAllItems(delegate(BannerBuilderColorItemVM i)
			{
				i.IsSelected = i.ColorID == selectedColorID;
			});
			this.IsEnabled = true;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0002748F File Offset: 0x0002568F
		private void OnItemSelection(BannerBuilderColorItemVM item)
		{
			Action<BannerBuilderColorItemVM> onSelection = this._onSelection;
			if (onSelection != null)
			{
				onSelection(item);
			}
			this._onSelection = null;
			this.IsEnabled = false;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x000274B4 File Offset: 0x000256B4
		private void PopulateItems()
		{
			this.Items.Clear();
			MBReadOnlyDictionary<int, BannerColor> readOnlyColorPalette = BannerManager.Instance.ReadOnlyColorPalette;
			for (int i = 0; i < readOnlyColorPalette.Count; i++)
			{
				KeyValuePair<int, BannerColor> keyValuePair = readOnlyColorPalette.ElementAt<KeyValuePair<int, BannerColor>>(i);
				this.Items.Add(new BannerBuilderColorItemVM(new Action<BannerBuilderColorItemVM>(this.OnItemSelection), keyValuePair.Key, keyValuePair.Value));
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x0002751A File Offset: 0x0002571A
		// (set) Token: 0x06000AF1 RID: 2801 RVA: 0x00027522 File Offset: 0x00025722
		[DataSourceProperty]
		public MBBindingList<BannerBuilderColorItemVM> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				if (value != this._items)
				{
					this._items = value;
					base.OnPropertyChangedWithValue<MBBindingList<BannerBuilderColorItemVM>>(value, "Items");
				}
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00027540 File Offset: 0x00025740
		// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x00027548 File Offset: 0x00025748
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

		// Token: 0x040004FB RID: 1275
		private Action<BannerBuilderColorItemVM> _onSelection;

		// Token: 0x040004FC RID: 1276
		private MBBindingList<BannerBuilderColorItemVM> _items;

		// Token: 0x040004FD RID: 1277
		private bool _isEnabled;
	}
}
