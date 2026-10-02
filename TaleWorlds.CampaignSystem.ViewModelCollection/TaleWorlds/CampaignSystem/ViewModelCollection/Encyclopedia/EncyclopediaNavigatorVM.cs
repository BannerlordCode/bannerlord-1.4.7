using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000CB RID: 203
	public class EncyclopediaNavigatorVM : ViewModel
	{
		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001347 RID: 4935 RVA: 0x0004DE53 File Offset: 0x0004C053
		public Tuple<string, object> LastActivePage
		{
			get
			{
				if (!this.History.IsEmpty<Tuple<string, object>>())
				{
					return this.History[this.HistoryIndex];
				}
				return null;
			}
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0004DE78 File Offset: 0x0004C078
		public EncyclopediaNavigatorVM(Func<string, object, bool, EncyclopediaPageVM> goToLink, Action closeEncyclopedia)
		{
			this._closeEncyclopedia = closeEncyclopedia;
			this.History = new List<Tuple<string, object>>();
			this.HistoryIndex = 0;
			this.MinCharAmountToShowResults = 3;
			this.SearchResults = new MBBindingList<EncyclopediaSearchResultVM>();
			Campaign.Current.EncyclopediaManager.SetLinkCallback(new Action<string, object>(this.ExecuteLink));
			this._goToLink = goToLink;
			this._searchResultComparer = new EncyclopediaNavigatorVM.SearchResultComparer(string.Empty);
			this.AddHistory("Home", null);
			this.RefreshValues();
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0004DF20 File Offset: 0x0004C120
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent evnt)
		{
			this.IsHighlightEnabled = evnt.NewNotificationElementID == "EncyclopediaSearchButton";
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0004DF38 File Offset: 0x0004C138
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM previousPageInputKey = this.PreviousPageInputKey;
			if (previousPageInputKey != null)
			{
				previousPageInputKey.OnFinalize();
			}
			InputKeyItemVM nextPageInputKey = this.NextPageInputKey;
			if (nextPageInputKey == null)
			{
				return;
			}
			nextPageInputKey.OnFinalize();
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0004DF61 File Offset: 0x0004C161
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.LeaderText = GameTexts.FindText("str_done", null).ToString();
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0004DF95 File Offset: 0x0004C195
		public void ExecuteHome()
		{
			Campaign.Current.EncyclopediaManager.GoToLink("Home", "-1");
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0004DFB0 File Offset: 0x0004C1B0
		public void ExecuteBarLink(string targetID)
		{
			if (targetID.Contains("Home"))
			{
				this.ExecuteHome();
				return;
			}
			if (targetID.Contains("ListPage"))
			{
				string text = targetID.Split(new char[] { '-' })[1];
				if (text == "Clans")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "Faction");
					return;
				}
				if (text == "Kingdoms")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "Kingdom");
					return;
				}
				if (text == "Heroes")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "Hero");
					return;
				}
				if (text == "Settlements")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "Settlement");
					return;
				}
				if (text == "Units")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "NPCCharacter");
					return;
				}
				if (text == "Concept")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "Concept");
					return;
				}
				if (text == "Ships")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "ShipHull");
				}
			}
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0004E105 File Offset: 0x0004C305
		public void ExecuteCloseEncyclopedia()
		{
			this._closeEncyclopedia();
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0004E114 File Offset: 0x0004C314
		private void ExecuteLink(string pageId, object target)
		{
			if (pageId != "LastPage" && target != this.LastActivePage.Item2)
			{
				if (!(pageId != "Home"))
				{
					pageId != this.LastActivePage.Item1;
				}
				this.AddHistory(pageId, target);
			}
			this._goToLink(pageId, target, true);
			this.PageName = GameTexts.FindText("str_encyclopedia_name", null).ToString();
			this.ResetSearch();
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0004E199 File Offset: 0x0004C399
		public void ResetHistory()
		{
			this.HistoryIndex = 0;
			this.History.Clear();
			this.AddHistory("Home", null);
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0004E1BC File Offset: 0x0004C3BC
		public void ExecuteBack()
		{
			if (this.HistoryIndex == 0)
			{
				return;
			}
			int num = this.HistoryIndex - 1;
			Tuple<string, object> tuple = this.History[num];
			if (tuple.Item1 != "LastPage" && (tuple.Item1 != this.LastActivePage.Item1 || tuple.Item2 != this.LastActivePage.Item2))
			{
				if (!(tuple.Item1 != "Home"))
				{
					tuple.Item1 != this.LastActivePage.Item1;
				}
			}
			this.UpdateHistoryIndex(num);
			this._goToLink(tuple.Item1, tuple.Item2, true);
			this.PageName = GameTexts.FindText("str_encyclopedia_name", null).ToString();
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0004E28C File Offset: 0x0004C48C
		public void ExecuteForward()
		{
			if (this.HistoryIndex == this.History.Count - 1)
			{
				return;
			}
			int num = this.HistoryIndex + 1;
			Tuple<string, object> tuple = this.History[num];
			if (tuple.Item1 != "LastPage" && (tuple.Item1 != this.LastActivePage.Item1 || tuple.Item2 != this.LastActivePage.Item2))
			{
				if (!(tuple.Item1 != "Home"))
				{
					tuple.Item1 != this.LastActivePage.Item1;
				}
			}
			this.UpdateHistoryIndex(num);
			this._goToLink(tuple.Item1, tuple.Item2, true);
			this.PageName = GameTexts.FindText("str_encyclopedia_name", null).ToString();
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0004E367 File Offset: 0x0004C567
		public Tuple<string, object> GetLastPage()
		{
			return this.History[this.HistoryIndex];
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0004E37C File Offset: 0x0004C57C
		public void AddHistory(string pageId, object obj)
		{
			if (this.HistoryIndex < this.History.Count - 1)
			{
				Tuple<string, object> tuple = this.History[this.HistoryIndex];
				if (tuple.Item1 == pageId && tuple.Item2 == obj)
				{
					return;
				}
				this.History.RemoveRange(this.HistoryIndex + 1, this.History.Count - this.HistoryIndex - 1);
			}
			this.History.Add(new Tuple<string, object>(pageId, obj));
			this.UpdateHistoryIndex(this.History.Count - 1);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0004E414 File Offset: 0x0004C614
		private void UpdateHistoryIndex(int newIndex)
		{
			this.HistoryIndex = newIndex;
			this.IsBackEnabled = newIndex > 0;
			this.IsForwardEnabled = newIndex < this.History.Count - 1;
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0004E43D File Offset: 0x0004C63D
		public void UpdatePageName(string value)
		{
			this.PageName = GameTexts.FindText("str_encyclopedia_name", null).ToString();
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0004E458 File Offset: 0x0004C658
		private void RefreshSearch(bool isAppending, bool isPasted)
		{
			int firstAsianCharIndex = EncyclopediaNavigatorVM.GetFirstAsianCharIndex(this.SearchText);
			this.MinCharAmountToShowResults = ((firstAsianCharIndex > -1 && firstAsianCharIndex < 3) ? (firstAsianCharIndex + 1) : 3);
			if (this.SearchText.Length < this.MinCharAmountToShowResults)
			{
				this.SearchResults.Clear();
				return;
			}
			string text = StringHelpers.RemoveDiacritics(this._searchText);
			if (!isAppending || this.SearchText.Length == this.MinCharAmountToShowResults || isPasted)
			{
				this.SearchResults.Clear();
				foreach (EncyclopediaPage encyclopediaPage in Campaign.Current.EncyclopediaManager.GetEncyclopediaPages())
				{
					foreach (EncyclopediaListItem encyclopediaListItem in encyclopediaPage.GetListItems())
					{
						int num = StringHelpers.RemoveDiacritics(encyclopediaListItem.Name).IndexOf(text, StringComparison.InvariantCultureIgnoreCase);
						if (num >= 0)
						{
							this.SearchResults.Add(new EncyclopediaSearchResultVM(encyclopediaListItem, text, num));
						}
					}
				}
				this._searchResultComparer.SearchText = text;
				this.SearchResults.Sort(this._searchResultComparer);
				return;
			}
			if (isAppending)
			{
				foreach (EncyclopediaSearchResultVM encyclopediaSearchResultVM in this.SearchResults.ToList<EncyclopediaSearchResultVM>())
				{
					if (StringHelpers.RemoveDiacritics(encyclopediaSearchResultVM.OrgNameText).IndexOf(text, StringComparison.InvariantCultureIgnoreCase) == -1)
					{
						this.SearchResults.Remove(encyclopediaSearchResultVM);
					}
					else
					{
						encyclopediaSearchResultVM.UpdateSearchedText(text);
					}
				}
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0004E614 File Offset: 0x0004C814
		private static int GetFirstAsianCharIndex(string searchText)
		{
			for (int i = 0; i < searchText.Length; i++)
			{
				if (Common.IsCharAsian(searchText[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0004E643 File Offset: 0x0004C843
		public void ResetSearch()
		{
			this.SearchText = string.Empty;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0004E650 File Offset: 0x0004C850
		public void ExecuteOnSearchActivated()
		{
			Game.Current.EventManager.TriggerEvent<OnEncyclopediaSearchActivatedEvent>(new OnEncyclopediaSearchActivatedEvent());
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600135B RID: 4955 RVA: 0x0004E666 File Offset: 0x0004C866
		// (set) Token: 0x0600135C RID: 4956 RVA: 0x0004E66E File Offset: 0x0004C86E
		[DataSourceProperty]
		public bool CanSwitchTabs
		{
			get
			{
				return this._canSwitchTabs;
			}
			set
			{
				if (value != this._canSwitchTabs)
				{
					this._canSwitchTabs = value;
					base.OnPropertyChangedWithValue(value, "CanSwitchTabs");
				}
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x0004E68C File Offset: 0x0004C88C
		// (set) Token: 0x0600135E RID: 4958 RVA: 0x0004E694 File Offset: 0x0004C894
		[DataSourceProperty]
		public bool IsBackEnabled
		{
			get
			{
				return this._isBackEnabled;
			}
			set
			{
				if (value != this._isBackEnabled)
				{
					this._isBackEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsBackEnabled");
				}
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x0004E6B2 File Offset: 0x0004C8B2
		// (set) Token: 0x06001360 RID: 4960 RVA: 0x0004E6BA File Offset: 0x0004C8BA
		[DataSourceProperty]
		public bool IsForwardEnabled
		{
			get
			{
				return this._isForwardEnabled;
			}
			set
			{
				if (value != this._isForwardEnabled)
				{
					this._isForwardEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsForwardEnabled");
				}
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x0004E6D8 File Offset: 0x0004C8D8
		// (set) Token: 0x06001362 RID: 4962 RVA: 0x0004E6E0 File Offset: 0x0004C8E0
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

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x0004E6FE File Offset: 0x0004C8FE
		// (set) Token: 0x06001364 RID: 4964 RVA: 0x0004E706 File Offset: 0x0004C906
		[DataSourceProperty]
		public bool IsSearchResultsShown
		{
			get
			{
				return this._isSearchResultsShown;
			}
			set
			{
				if (value != this._isSearchResultsShown)
				{
					this._isSearchResultsShown = value;
					base.OnPropertyChangedWithValue(value, "IsSearchResultsShown");
				}
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001365 RID: 4965 RVA: 0x0004E724 File Offset: 0x0004C924
		// (set) Token: 0x06001366 RID: 4966 RVA: 0x0004E72C File Offset: 0x0004C92C
		[DataSourceProperty]
		public string NavBarString
		{
			get
			{
				return this._navBarString;
			}
			set
			{
				if (value != this._navBarString)
				{
					this._navBarString = value;
					base.OnPropertyChangedWithValue<string>(value, "NavBarString");
				}
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x0004E74F File Offset: 0x0004C94F
		// (set) Token: 0x06001368 RID: 4968 RVA: 0x0004E757 File Offset: 0x0004C957
		[DataSourceProperty]
		public string PageName
		{
			get
			{
				return this._pageName;
			}
			set
			{
				if (value != this._pageName)
				{
					this._pageName = value;
					base.OnPropertyChangedWithValue<string>(value, "PageName");
				}
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x0004E77A File Offset: 0x0004C97A
		// (set) Token: 0x0600136A RID: 4970 RVA: 0x0004E782 File Offset: 0x0004C982
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x0004E7A5 File Offset: 0x0004C9A5
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x0004E7AD File Offset: 0x0004C9AD
		[DataSourceProperty]
		public string LeaderText
		{
			get
			{
				return this._leaderText;
			}
			set
			{
				if (value != this._leaderText)
				{
					this._leaderText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderText");
				}
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x0004E7D0 File Offset: 0x0004C9D0
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x0004E7D8 File Offset: 0x0004C9D8
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSearchResultVM> SearchResults
		{
			get
			{
				return this._searchResults;
			}
			set
			{
				if (value != this._searchResults)
				{
					this._searchResults = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSearchResultVM>>(value, "SearchResults");
				}
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x0004E7F6 File Offset: 0x0004C9F6
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x0004E800 File Offset: 0x0004CA00
		[DataSourceProperty]
		public string SearchText
		{
			get
			{
				return this._searchText;
			}
			set
			{
				if (value != this._searchText)
				{
					bool flag = value.ToLower().Contains(this._searchText);
					bool flag2 = string.IsNullOrEmpty(this._searchText) && !string.IsNullOrEmpty(value);
					this._searchText = value.ToLower();
					Debug.Print("isAppending: " + flag.ToString() + " isPasted: " + flag2.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
					this.RefreshSearch(flag, flag2);
					base.OnPropertyChangedWithValue<string>(value, "SearchText");
				}
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x0004E895 File Offset: 0x0004CA95
		// (set) Token: 0x06001372 RID: 4978 RVA: 0x0004E89D File Offset: 0x0004CA9D
		[DataSourceProperty]
		public int MinCharAmountToShowResults
		{
			get
			{
				return this._minCharAmountToShowResults;
			}
			set
			{
				if (value != this._minCharAmountToShowResults)
				{
					this._minCharAmountToShowResults = value;
					base.OnPropertyChangedWithValue(value, "MinCharAmountToShowResults");
				}
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x0004E8BB File Offset: 0x0004CABB
		// (set) Token: 0x06001374 RID: 4980 RVA: 0x0004E8C3 File Offset: 0x0004CAC3
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x0004E8E1 File Offset: 0x0004CAE1
		// (set) Token: 0x06001376 RID: 4982 RVA: 0x0004E8E9 File Offset: 0x0004CAE9
		[DataSourceProperty]
		public InputKeyItemVM PreviousPageInputKey
		{
			get
			{
				return this._previousPageInputKey;
			}
			set
			{
				if (value != this._previousPageInputKey)
				{
					this._previousPageInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviousPageInputKey");
				}
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x0004E907 File Offset: 0x0004CB07
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x0004E90F File Offset: 0x0004CB0F
		[DataSourceProperty]
		public InputKeyItemVM NextPageInputKey
		{
			get
			{
				return this._nextPageInputKey;
			}
			set
			{
				if (value != this._nextPageInputKey)
				{
					this._nextPageInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextPageInputKey");
				}
			}
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0004E92D File Offset: 0x0004CB2D
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0004E93C File Offset: 0x0004CB3C
		public void SetPreviousPageInputKey(HotKey hotkey)
		{
			this.PreviousPageInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0004E94B File Offset: 0x0004CB4B
		public void SetNextPageInputKey(HotKey hotkey)
		{
			this.NextPageInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x040008D3 RID: 2259
		private List<Tuple<string, object>> History;

		// Token: 0x040008D4 RID: 2260
		private int HistoryIndex;

		// Token: 0x040008D5 RID: 2261
		private readonly Func<string, object, bool, EncyclopediaPageVM> _goToLink;

		// Token: 0x040008D6 RID: 2262
		private readonly Action _closeEncyclopedia;

		// Token: 0x040008D7 RID: 2263
		private EncyclopediaNavigatorVM.SearchResultComparer _searchResultComparer;

		// Token: 0x040008D8 RID: 2264
		private MBBindingList<EncyclopediaSearchResultVM> _searchResults;

		// Token: 0x040008D9 RID: 2265
		private string _searchText = "";

		// Token: 0x040008DA RID: 2266
		private string _pageName;

		// Token: 0x040008DB RID: 2267
		private string _doneText;

		// Token: 0x040008DC RID: 2268
		private string _leaderText;

		// Token: 0x040008DD RID: 2269
		private bool _canSwitchTabs;

		// Token: 0x040008DE RID: 2270
		private bool _isBackEnabled;

		// Token: 0x040008DF RID: 2271
		private bool _isForwardEnabled;

		// Token: 0x040008E0 RID: 2272
		private bool _isHighlightEnabled;

		// Token: 0x040008E1 RID: 2273
		private bool _isSearchResultsShown;

		// Token: 0x040008E2 RID: 2274
		private string _navBarString;

		// Token: 0x040008E3 RID: 2275
		private int _minCharAmountToShowResults;

		// Token: 0x040008E4 RID: 2276
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040008E5 RID: 2277
		private InputKeyItemVM _previousPageInputKey;

		// Token: 0x040008E6 RID: 2278
		private InputKeyItemVM _nextPageInputKey;

		// Token: 0x02000240 RID: 576
		private class SearchResultComparer : IComparer<EncyclopediaSearchResultVM>
		{
			// Token: 0x17000BB0 RID: 2992
			// (get) Token: 0x06002508 RID: 9480 RVA: 0x000814AB File Offset: 0x0007F6AB
			// (set) Token: 0x06002509 RID: 9481 RVA: 0x000814B3 File Offset: 0x0007F6B3
			public string SearchText
			{
				get
				{
					return this._searchText;
				}
				set
				{
					if (value != this._searchText)
					{
						this._searchText = value;
					}
				}
			}

			// Token: 0x0600250A RID: 9482 RVA: 0x000814CA File Offset: 0x0007F6CA
			public SearchResultComparer(string searchText)
			{
				this.SearchText = searchText;
			}

			// Token: 0x0600250B RID: 9483 RVA: 0x000814DC File Offset: 0x0007F6DC
			private int CompareBasedOnCapitalization(EncyclopediaSearchResultVM x, EncyclopediaSearchResultVM y)
			{
				int num = ((x.NameText.Length > 0 && char.IsUpper(x.NameText[0])) ? 1 : (-1));
				int num2 = ((y.NameText.Length > 0 && char.IsUpper(y.NameText[0])) ? 1 : (-1));
				return num.CompareTo(num2);
			}

			// Token: 0x0600250C RID: 9484 RVA: 0x00081540 File Offset: 0x0007F740
			public int Compare(EncyclopediaSearchResultVM x, EncyclopediaSearchResultVM y)
			{
				if (x.MatchStartIndex != y.MatchStartIndex)
				{
					return y.MatchStartIndex.CompareTo(x.MatchStartIndex);
				}
				int num = this.CompareBasedOnCapitalization(x, y);
				if (num == 0)
				{
					return y.NameText.Length.CompareTo(x.NameText.Length);
				}
				return num;
			}

			// Token: 0x0400124C RID: 4684
			private string _searchText;
		}
	}
}
