using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Inquiries
{
	// Token: 0x02000046 RID: 70
	public class MultiSelectionQueryPopUpVM : PopUpBaseVM
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x000165EC File Offset: 0x000147EC
		public MultiSelectionQueryPopUpVM(Action closeQuery)
			: base(closeQuery)
		{
			this.InquiryElements = new MBBindingList<InquiryElementVM>();
			this.MaxSelectableOptionCount = 0;
			this.MinSelectableOptionCount = 0;
			this._selectedOptionCount = 0;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00016618 File Offset: 0x00014818
		public void SetData(MultiSelectionInquiryData data)
		{
			this._data = data;
			this.InquiryElements.Clear();
			foreach (InquiryElement inquiryElement in this._data.InquiryElements)
			{
				TextObject textObject = (string.IsNullOrEmpty(inquiryElement.Hint) ? TextObject.GetEmpty() : new TextObject("{=!}" + inquiryElement.Hint, null));
				InquiryElementVM inquiryElementVM = new InquiryElementVM(inquiryElement, textObject, new Action<InquiryElementVM, bool>(this.OnInquiryElementSelected));
				this.InquiryElements.Add(inquiryElementVM);
			}
			base.TitleText = this._data.TitleText;
			base.PopUpLabel = this._data.DescriptionText;
			this.MaxSelectableOptionCount = this._data.MaxSelectableOptionCount;
			this.MinSelectableOptionCount = this._data.MinSelectableOptionCount;
			base.ButtonOkLabel = this._data.AffirmativeText;
			base.ButtonCancelLabel = this._data.NegativeText;
			base.IsButtonOkShown = true;
			base.IsButtonCancelShown = this._data.IsExitShown;
			this.IsSearchAvailable = this._data.IsSeachAvailable;
			this.SearchPlaceholderText = new TextObject("{=tQOPRBFg}Search...", null).ToString();
			this.RefreshIsButtonOkEnabled();
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00016774 File Offset: 0x00014974
		private void OnInquiryElementSelected(InquiryElementVM elementVM, bool isSelected)
		{
			if (isSelected)
			{
				this._selectedOptionCount++;
				if (this.MaxSelectableOptionCount != 1)
				{
					goto IL_005C;
				}
				using (IEnumerator<InquiryElementVM> enumerator = this.InquiryElements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InquiryElementVM inquiryElementVM = enumerator.Current;
						if (inquiryElementVM != elementVM)
						{
							inquiryElementVM.IsSelected = false;
						}
					}
					goto IL_005C;
				}
			}
			this._selectedOptionCount--;
			IL_005C:
			this.RefreshIsButtonOkEnabled();
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000167F4 File Offset: 0x000149F4
		public override void ExecuteAffirmativeAction()
		{
			if (this._data.AffirmativeAction != null)
			{
				List<InquiryElement> list = new List<InquiryElement>();
				foreach (InquiryElementVM inquiryElementVM in this.InquiryElements)
				{
					if (inquiryElementVM.IsSelected)
					{
						list.Add(inquiryElementVM.InquiryElement);
					}
				}
				this._data.AffirmativeAction(list);
			}
			base.CloseQuery();
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00016878 File Offset: 0x00014A78
		public override void ExecuteNegativeAction()
		{
			Action<List<InquiryElement>> negativeAction = this._data.NegativeAction;
			if (negativeAction != null)
			{
				negativeAction(new List<InquiryElement>());
			}
			base.CloseQuery();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001689B File Offset: 0x00014A9B
		public override void OnClearData()
		{
			base.OnClearData();
			this._data = null;
			this.MaxSelectableOptionCount = 0;
			this.MinSelectableOptionCount = 0;
			this._selectedOptionCount = 0;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000168BF File Offset: 0x00014ABF
		private void RefreshIsButtonOkEnabled()
		{
			base.IsButtonOkEnabled = (this.MaxSelectableOptionCount <= 0 || this._selectedOptionCount <= this.MaxSelectableOptionCount) && this._selectedOptionCount >= this.MinSelectableOptionCount;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000168F4 File Offset: 0x00014AF4
		private void UpdateInquiryFilter(string searchText, bool isAppending)
		{
			string text = searchText.ToLower();
			for (int i = 0; i < this.InquiryElements.Count; i++)
			{
				InquiryElementVM inquiryElementVM = this.InquiryElements[i];
				if (!isAppending || !inquiryElementVM.IsFilteredOut)
				{
					inquiryElementVM.IsFilteredOut = !inquiryElementVM.Text.ToLower().Contains(text);
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00016950 File Offset: 0x00014B50
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x00016958 File Offset: 0x00014B58
		[DataSourceProperty]
		public MBBindingList<InquiryElementVM> InquiryElements
		{
			get
			{
				return this._inquiryElements;
			}
			set
			{
				if (value != this._inquiryElements)
				{
					this._inquiryElements = value;
					base.OnPropertyChangedWithValue<MBBindingList<InquiryElementVM>>(value, "InquiryElements");
				}
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x00016976 File Offset: 0x00014B76
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x0001697E File Offset: 0x00014B7E
		[DataSourceProperty]
		public int MaxSelectableOptionCount
		{
			get
			{
				return this._maxSelectableOptionCount;
			}
			set
			{
				if (value != this._maxSelectableOptionCount)
				{
					this._maxSelectableOptionCount = value;
					base.OnPropertyChangedWithValue(value, "MaxSelectableOptionCount");
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001699C File Offset: 0x00014B9C
		// (set) Token: 0x0600060C RID: 1548 RVA: 0x000169A4 File Offset: 0x00014BA4
		[DataSourceProperty]
		public int MinSelectableOptionCount
		{
			get
			{
				return this._minSelectableOptionCount;
			}
			set
			{
				if (value != this._minSelectableOptionCount)
				{
					this._minSelectableOptionCount = value;
					base.OnPropertyChangedWithValue(value, "MinSelectableOptionCount");
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x000169C2 File Offset: 0x00014BC2
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x000169CA File Offset: 0x00014BCA
		[DataSourceProperty]
		public bool IsSearchAvailable
		{
			get
			{
				return this._isSearchAvailable;
			}
			set
			{
				if (value != this._isSearchAvailable)
				{
					this._isSearchAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsSearchAvailable");
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x000169E8 File Offset: 0x00014BE8
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x000169F0 File Offset: 0x00014BF0
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
					bool flag = value.IndexOf(this._searchText ?? "") >= 0;
					this._searchText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchText");
					this.UpdateInquiryFilter(this._searchText, flag);
				}
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x00016A47 File Offset: 0x00014C47
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x00016A4F File Offset: 0x00014C4F
		[DataSourceProperty]
		public string SearchPlaceholderText
		{
			get
			{
				return this._searchPlaceholderText;
			}
			set
			{
				if (value != this._searchPlaceholderText)
				{
					this._searchPlaceholderText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchPlaceholderText");
				}
			}
		}

		// Token: 0x040002AF RID: 687
		private MultiSelectionInquiryData _data;

		// Token: 0x040002B0 RID: 688
		private int _selectedOptionCount;

		// Token: 0x040002B1 RID: 689
		private MBBindingList<InquiryElementVM> _inquiryElements;

		// Token: 0x040002B2 RID: 690
		private int _maxSelectableOptionCount;

		// Token: 0x040002B3 RID: 691
		private int _minSelectableOptionCount;

		// Token: 0x040002B4 RID: 692
		private bool _isSearchAvailable;

		// Token: 0x040002B5 RID: 693
		private string _searchText;

		// Token: 0x040002B6 RID: 694
		private string _searchPlaceholderText;
	}
}
