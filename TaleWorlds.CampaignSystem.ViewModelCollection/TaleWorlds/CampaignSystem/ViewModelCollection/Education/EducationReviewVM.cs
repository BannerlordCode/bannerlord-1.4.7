using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000F4 RID: 244
	public class EducationReviewVM : ViewModel
	{
		// Token: 0x06001623 RID: 5667 RVA: 0x00056CCC File Offset: 0x00054ECC
		public EducationReviewVM(int pageCount)
		{
			this._pageCount = pageCount;
			this.ReviewList = new MBBindingList<EducationReviewItemVM>();
			for (int i = 0; i < this._pageCount - 1; i++)
			{
				this.ReviewList.Add(new EducationReviewItemVM());
			}
			this.RefreshValues();
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00056D3C File Offset: 0x00054F3C
		public override void RefreshValues()
		{
			for (int i = 0; i < this.ReviewList.Count; i++)
			{
				this._educationPageTitle.SetTextVariable("NUMBER", i + 1);
				this.ReviewList[i].Title = this._educationPageTitle.ToString();
			}
			this.StageCompleteText = this._stageCompleteTextObject.ToString();
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00056DA0 File Offset: 0x00054FA0
		public void SetGainForStage(int pageIndex, string gainText)
		{
			if (pageIndex >= 0 && pageIndex < this._pageCount)
			{
				this.ReviewList[pageIndex].UpdateWith(gainText);
			}
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00056DC1 File Offset: 0x00054FC1
		public void SetCurrentPage(int currentPageIndex)
		{
			this.IsEnabled = currentPageIndex == this._pageCount - 1;
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001627 RID: 5671 RVA: 0x00056DD4 File Offset: 0x00054FD4
		// (set) Token: 0x06001628 RID: 5672 RVA: 0x00056DDC File Offset: 0x00054FDC
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

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001629 RID: 5673 RVA: 0x00056DFA File Offset: 0x00054FFA
		// (set) Token: 0x0600162A RID: 5674 RVA: 0x00056E02 File Offset: 0x00055002
		[DataSourceProperty]
		public string StageCompleteText
		{
			get
			{
				return this._stageCompleteText;
			}
			set
			{
				if (value != this._stageCompleteText)
				{
					this._stageCompleteText = value;
					base.OnPropertyChangedWithValue<string>(value, "StageCompleteText");
				}
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x0600162B RID: 5675 RVA: 0x00056E25 File Offset: 0x00055025
		// (set) Token: 0x0600162C RID: 5676 RVA: 0x00056E2D File Offset: 0x0005502D
		[DataSourceProperty]
		public MBBindingList<EducationReviewItemVM> ReviewList
		{
			get
			{
				return this._reviewList;
			}
			set
			{
				if (value != this._reviewList)
				{
					this._reviewList = value;
					base.OnPropertyChangedWithValue<MBBindingList<EducationReviewItemVM>>(value, "ReviewList");
				}
			}
		}

		// Token: 0x04000A10 RID: 2576
		private readonly int _pageCount;

		// Token: 0x04000A11 RID: 2577
		private readonly TextObject _educationPageTitle = new TextObject("{=m1Yynagz}Page {NUMBER}", null);

		// Token: 0x04000A12 RID: 2578
		private readonly TextObject _stageCompleteTextObject = new TextObject("{=flxDkoMh}Stage Complete", null);

		// Token: 0x04000A13 RID: 2579
		private MBBindingList<EducationReviewItemVM> _reviewList;

		// Token: 0x04000A14 RID: 2580
		private bool _isEnabled;

		// Token: 0x04000A15 RID: 2581
		private string _stageCompleteText;
	}
}
