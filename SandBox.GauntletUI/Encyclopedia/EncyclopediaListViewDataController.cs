using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.Library;

namespace SandBox.GauntletUI.Encyclopedia
{
	// Token: 0x02000045 RID: 69
	public class EncyclopediaListViewDataController
	{
		// Token: 0x0600032B RID: 811 RVA: 0x00012F10 File Offset: 0x00011110
		public EncyclopediaListViewDataController()
		{
			this._listData = new Dictionary<EncyclopediaPage, EncyclopediaListViewDataController.EncyclopediaListViewData>();
			foreach (EncyclopediaPage encyclopediaPage in Campaign.Current.EncyclopediaManager.GetEncyclopediaPages())
			{
				if (!this._listData.ContainsKey(encyclopediaPage))
				{
					this._listData.Add(encyclopediaPage, new EncyclopediaListViewDataController.EncyclopediaListViewData(new MBBindingList<EncyclopediaFilterGroupVM>(), 0, "", false));
				}
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00012F9C File Offset: 0x0001119C
		public void SaveListData(EncyclopediaListVM list, string id)
		{
			if (list != null && this._listData.ContainsKey(list.Page))
			{
				EncyclopediaListSortControllerVM sortController = list.SortController;
				int? num;
				if (sortController == null)
				{
					num = null;
				}
				else
				{
					EncyclopediaListSelectorVM sortSelection = sortController.SortSelection;
					num = ((sortSelection != null) ? new int?(sortSelection.SelectedIndex) : null);
				}
				int num2 = num ?? 0;
				Dictionary<EncyclopediaPage, EncyclopediaListViewDataController.EncyclopediaListViewData> listData = this._listData;
				EncyclopediaPage page = list.Page;
				MBBindingList<EncyclopediaFilterGroupVM> filterGroups = list.FilterGroups;
				int num3 = num2;
				EncyclopediaListSortControllerVM sortController2 = list.SortController;
				listData[page] = new EncyclopediaListViewDataController.EncyclopediaListViewData(filterGroups, num3, id, sortController2 != null && sortController2.GetSortOrder());
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0001303C File Offset: 0x0001123C
		public void LoadListData(EncyclopediaListVM list)
		{
			if (list != null && this._listData.ContainsKey(list.Page))
			{
				EncyclopediaListViewDataController.EncyclopediaListViewData encyclopediaListViewData = this._listData[list.Page];
				EncyclopediaListSortControllerVM sortController = list.SortController;
				if (sortController != null)
				{
					sortController.SetSortSelection(encyclopediaListViewData.SelectedSortIndex);
				}
				EncyclopediaListSortControllerVM sortController2 = list.SortController;
				if (sortController2 != null)
				{
					sortController2.SetSortOrder(encyclopediaListViewData.IsAscending);
				}
				list.CopyFiltersFrom(encyclopediaListViewData.Filters);
				list.LastSelectedItemId = encyclopediaListViewData.LastSelectedItemId;
			}
		}

		// Token: 0x0400013F RID: 319
		private Dictionary<EncyclopediaPage, EncyclopediaListViewDataController.EncyclopediaListViewData> _listData;

		// Token: 0x02000085 RID: 133
		private readonly struct EncyclopediaListViewData
		{
			// Token: 0x0600045F RID: 1119 RVA: 0x000184D0 File Offset: 0x000166D0
			public EncyclopediaListViewData(MBBindingList<EncyclopediaFilterGroupVM> filters, int selectedSortIndex, string lastSelectedItemId, bool isAscending)
			{
				Dictionary<EncyclopediaFilterItem, bool> dictionary = new Dictionary<EncyclopediaFilterItem, bool>();
				foreach (EncyclopediaFilterGroupVM encyclopediaFilterGroupVM in filters)
				{
					foreach (EncyclopediaListFilterVM encyclopediaListFilterVM in encyclopediaFilterGroupVM.Filters)
					{
						if (!dictionary.ContainsKey(encyclopediaListFilterVM.Filter))
						{
							dictionary.Add(encyclopediaListFilterVM.Filter, encyclopediaListFilterVM.IsSelected);
						}
					}
				}
				this.Filters = dictionary;
				this.SelectedSortIndex = selectedSortIndex;
				this.LastSelectedItemId = lastSelectedItemId;
				this.IsAscending = isAscending;
			}

			// Token: 0x04000207 RID: 519
			public readonly Dictionary<EncyclopediaFilterItem, bool> Filters;

			// Token: 0x04000208 RID: 520
			public readonly int SelectedSortIndex;

			// Token: 0x04000209 RID: 521
			public readonly string LastSelectedItemId;

			// Token: 0x0400020A RID: 522
			public readonly bool IsAscending;
		}
	}
}
