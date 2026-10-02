using System;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000DE RID: 222
	public class EncyclopediaListSortControllerVM : ViewModel
	{
		// Token: 0x06001544 RID: 5444 RVA: 0x000542C4 File Offset: 0x000524C4
		public EncyclopediaListSortControllerVM(EncyclopediaPage page, MBBindingList<EncyclopediaListItemVM> items)
		{
			this._page = page;
			this._items = items;
			this.UpdateSortItemsFromPage(page);
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00054312 File Offset: 0x00052512
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent evnt)
		{
			this.IsHighlightEnabled = evnt.NewNotificationElementID == "EncyclopediaSortButton";
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0005432C File Offset: 0x0005252C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameLabel = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.SortByLabel = GameTexts.FindText("str_sort_by_label", null).ToString();
			this.SortedValueLabelText = this._sortedValueLabel.ToString();
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0005437C File Offset: 0x0005257C
		public override void OnFinalize()
		{
			base.OnFinalize();
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0005439F File Offset: 0x0005259F
		public void SetSortSelection(int index)
		{
			this.SortSelection.SelectedIndex = index;
			this.OnSortSelectionChanged(this.SortSelection);
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x000543BC File Offset: 0x000525BC
		private void UpdateSortItemsFromPage(EncyclopediaPage page)
		{
			this.SortSelection = new EncyclopediaListSelectorVM(0, new Action<SelectorVM<EncyclopediaListSelectorItemVM>>(this.OnSortSelectionChanged), new Action(this.OnSortSelectionActivated));
			foreach (EncyclopediaSortController encyclopediaSortController in page.GetSortControllers())
			{
				EncyclopediaListItemComparer encyclopediaListItemComparer = new EncyclopediaListItemComparer(encyclopediaSortController);
				this.SortSelection.AddItem(new EncyclopediaListSelectorItemVM(encyclopediaListItemComparer));
			}
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0005443C File Offset: 0x0005263C
		private void UpdateAlternativeSortState(EncyclopediaListItemComparerBase comparer)
		{
			CampaignUIHelper.SortState sortState = (comparer.IsAscending ? CampaignUIHelper.SortState.Ascending : CampaignUIHelper.SortState.Descending);
			this.AlternativeSortState = (int)sortState;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x00054460 File Offset: 0x00052660
		private void OnSortSelectionChanged(SelectorVM<EncyclopediaListSelectorItemVM> s)
		{
			EncyclopediaListItemComparer comparer = s.SelectedItem.Comparer;
			comparer.SortController.Comparer.SetDefaultSortOrder();
			this._items.Sort(comparer);
			this._items.ApplyActionOnAllItems(delegate(EncyclopediaListItemVM x)
			{
				x.SetComparedValue(comparer.SortController.Comparer);
			});
			this._sortedValueLabel = comparer.SortController.Name;
			this.SortedValueLabelText = this._sortedValueLabel.ToString();
			this.IsAlternativeSortVisible = this.SortSelection.SelectedIndex != 0;
			this.UpdateAlternativeSortState(comparer.SortController.Comparer);
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00054514 File Offset: 0x00052714
		public void ExecuteSwitchSortOrder()
		{
			EncyclopediaListItemComparer comparer = this.SortSelection.SelectedItem.Comparer;
			comparer.SortController.Comparer.SwitchSortOrder();
			this._items.Sort(comparer);
			this.UpdateAlternativeSortState(comparer.SortController.Comparer);
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x00054560 File Offset: 0x00052760
		public void SetSortOrder(bool isAscending)
		{
			EncyclopediaListItemComparer comparer = this.SortSelection.SelectedItem.Comparer;
			if (comparer.SortController.Comparer.IsAscending != isAscending)
			{
				comparer.SortController.Comparer.SetSortOrder(isAscending);
				this._items.Sort(comparer);
				this.UpdateAlternativeSortState(comparer.SortController.Comparer);
			}
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x000545BF File Offset: 0x000527BF
		public bool GetSortOrder()
		{
			return this.SortSelection.SelectedItem.Comparer.SortController.Comparer.IsAscending;
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x000545E0 File Offset: 0x000527E0
		private void OnSortSelectionActivated()
		{
			Game.Current.EventManager.TriggerEvent<OnEncyclopediaListSortedEvent>(new OnEncyclopediaListSortedEvent());
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x000545F6 File Offset: 0x000527F6
		// (set) Token: 0x06001551 RID: 5457 RVA: 0x000545FE File Offset: 0x000527FE
		[DataSourceProperty]
		public EncyclopediaListSelectorVM SortSelection
		{
			get
			{
				return this._sortSelection;
			}
			set
			{
				if (value != this._sortSelection)
				{
					this._sortSelection = value;
					base.OnPropertyChangedWithValue<EncyclopediaListSelectorVM>(value, "SortSelection");
				}
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x0005461C File Offset: 0x0005281C
		// (set) Token: 0x06001553 RID: 5459 RVA: 0x00054624 File Offset: 0x00052824
		[DataSourceProperty]
		public string NameLabel
		{
			get
			{
				return this._nameLabel;
			}
			set
			{
				if (value != this._nameLabel)
				{
					this._nameLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "NameLabel");
				}
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x00054647 File Offset: 0x00052847
		// (set) Token: 0x06001555 RID: 5461 RVA: 0x0005464F File Offset: 0x0005284F
		[DataSourceProperty]
		public string SortedValueLabelText
		{
			get
			{
				return this._sortedValueLabelText;
			}
			set
			{
				if (value != this._sortedValueLabelText)
				{
					this._sortedValueLabelText = value;
					base.OnPropertyChangedWithValue<string>(value, "SortedValueLabelText");
				}
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x00054672 File Offset: 0x00052872
		// (set) Token: 0x06001557 RID: 5463 RVA: 0x0005467A File Offset: 0x0005287A
		[DataSourceProperty]
		public string SortByLabel
		{
			get
			{
				return this._sortByLabel;
			}
			set
			{
				if (value != this._sortByLabel)
				{
					this._sortByLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "SortByLabel");
				}
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x0005469D File Offset: 0x0005289D
		// (set) Token: 0x06001559 RID: 5465 RVA: 0x000546A5 File Offset: 0x000528A5
		[DataSourceProperty]
		public int AlternativeSortState
		{
			get
			{
				return this._alternativeSortState;
			}
			set
			{
				if (value != this._alternativeSortState)
				{
					this._alternativeSortState = value;
					base.OnPropertyChangedWithValue(value, "AlternativeSortState");
				}
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x000546C3 File Offset: 0x000528C3
		// (set) Token: 0x0600155B RID: 5467 RVA: 0x000546CB File Offset: 0x000528CB
		[DataSourceProperty]
		public bool IsAlternativeSortVisible
		{
			get
			{
				return this._isAlternativeSortVisible;
			}
			set
			{
				if (value != this._isAlternativeSortVisible)
				{
					this._isAlternativeSortVisible = value;
					base.OnPropertyChangedWithValue(value, "IsAlternativeSortVisible");
				}
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x000546E9 File Offset: 0x000528E9
		// (set) Token: 0x0600155D RID: 5469 RVA: 0x000546F1 File Offset: 0x000528F1
		[DataSourceProperty]
		public bool IsHighlightEnabled
		{
			get
			{
				return this._isHighlightEnabled;
			}
			set
			{
				if (value != this._isHighlightEnabled)
				{
					this._isHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHighlightEnabled");
				}
			}
		}

		// Token: 0x040009B2 RID: 2482
		private TextObject _sortedValueLabel = TextObject.GetEmpty();

		// Token: 0x040009B3 RID: 2483
		private MBBindingList<EncyclopediaListItemVM> _items;

		// Token: 0x040009B4 RID: 2484
		private EncyclopediaPage _page;

		// Token: 0x040009B5 RID: 2485
		private EncyclopediaListSelectorVM _sortSelection;

		// Token: 0x040009B6 RID: 2486
		private string _nameLabel;

		// Token: 0x040009B7 RID: 2487
		private string _sortedValueLabelText;

		// Token: 0x040009B8 RID: 2488
		private string _sortByLabel;

		// Token: 0x040009B9 RID: 2489
		private int _alternativeSortState;

		// Token: 0x040009BA RID: 2490
		private bool _isAlternativeSortVisible;

		// Token: 0x040009BB RID: 2491
		private bool _isHighlightEnabled;
	}
}
