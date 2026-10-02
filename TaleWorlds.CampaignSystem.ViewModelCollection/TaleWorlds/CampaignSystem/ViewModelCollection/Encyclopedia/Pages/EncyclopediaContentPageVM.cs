using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D1 RID: 209
	public class EncyclopediaContentPageVM : EncyclopediaPageVM
	{
		// Token: 0x060013D3 RID: 5075 RVA: 0x0004FA96 File Offset: 0x0004DC96
		public EncyclopediaContentPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0004FAC1 File Offset: 0x0004DCC1
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PreviousButtonLabel = this._previousButtonLabelText.ToString();
			this.NextButtonLabel = this._nextButtonLabelText.ToString();
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0004FAEC File Offset: 0x0004DCEC
		public void InitializeQuickNavigation(EncyclopediaListVM list)
		{
			if (list != null && list.Items != null)
			{
				List<EncyclopediaListItemVM> list2 = list.Items.Where<EncyclopediaListItemVM>((EncyclopediaListItemVM x) => !x.IsFiltered).ToList<EncyclopediaListItemVM>();
				int count = list2.Count;
				int num = list2.FindIndex((EncyclopediaListItemVM x) => x.Object == base.Obj);
				if (count > 1 && num > -1)
				{
					if (num > 0)
					{
						this._previousItem = list2[num - 1];
						this.PreviousButtonHint = new HintViewModel(new TextObject(this._previousItem.Name, null), null);
						this.IsPreviousButtonEnabled = true;
					}
					if (num < count - 1)
					{
						this._nextItem = list2[num + 1];
						this.NextButtonHint = new HintViewModel(new TextObject(this._nextItem.Name, null), null);
						this.IsNextButtonEnabled = true;
					}
				}
			}
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0004FBCC File Offset: 0x0004DDCC
		public void ExecuteGoToNextItem()
		{
			if (this._nextItem != null)
			{
				this._nextItem.Execute();
				return;
			}
			Debug.FailedAssert("If the next button is enabled then next item should not be null.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Encyclopedia\\Pages\\EncyclopediaContentPageVM.cs", "ExecuteGoToNextItem", 66);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0004FBF8 File Offset: 0x0004DDF8
		public void ExecuteGoToPreviousItem()
		{
			if (this._previousItem != null)
			{
				this._previousItem.Execute();
				return;
			}
			Debug.FailedAssert("If the previous button is enabled then previous item should not be null.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Encyclopedia\\Pages\\EncyclopediaContentPageVM.cs", "ExecuteGoToPreviousItem", 78);
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x0004FC24 File Offset: 0x0004DE24
		// (set) Token: 0x060013D9 RID: 5081 RVA: 0x0004FC2C File Offset: 0x0004DE2C
		[DataSourceProperty]
		public bool IsPreviousButtonEnabled
		{
			get
			{
				return this._isPreviousButtonEnabled;
			}
			set
			{
				if (value != this._isPreviousButtonEnabled)
				{
					this._isPreviousButtonEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsPreviousButtonEnabled");
				}
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x0004FC4A File Offset: 0x0004DE4A
		// (set) Token: 0x060013DB RID: 5083 RVA: 0x0004FC52 File Offset: 0x0004DE52
		[DataSourceProperty]
		public bool IsNextButtonEnabled
		{
			get
			{
				return this._isNextButtonEnabled;
			}
			set
			{
				if (value != this._isNextButtonEnabled)
				{
					this._isNextButtonEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsNextButtonEnabled");
				}
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x0004FC70 File Offset: 0x0004DE70
		// (set) Token: 0x060013DD RID: 5085 RVA: 0x0004FC78 File Offset: 0x0004DE78
		[DataSourceProperty]
		public string PreviousButtonLabel
		{
			get
			{
				return this._previousButtonLabel;
			}
			set
			{
				if (value != this._previousButtonLabel)
				{
					this._previousButtonLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviousButtonLabel");
				}
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x0004FC9B File Offset: 0x0004DE9B
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x0004FCA3 File Offset: 0x0004DEA3
		[DataSourceProperty]
		public string NextButtonLabel
		{
			get
			{
				return this._nextButtonLabel;
			}
			set
			{
				if (value != this._nextButtonLabel)
				{
					this._nextButtonLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "NextButtonLabel");
				}
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x0004FCC6 File Offset: 0x0004DEC6
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x0004FCCE File Offset: 0x0004DECE
		[DataSourceProperty]
		public HintViewModel PreviousButtonHint
		{
			get
			{
				return this._previousButtonHint;
			}
			set
			{
				if (value != this._previousButtonHint)
				{
					this._previousButtonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PreviousButtonHint");
				}
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0004FCEC File Offset: 0x0004DEEC
		// (set) Token: 0x060013E3 RID: 5091 RVA: 0x0004FCF4 File Offset: 0x0004DEF4
		[DataSourceProperty]
		public HintViewModel NextButtonHint
		{
			get
			{
				return this._nextButtonHint;
			}
			set
			{
				if (value != this._nextButtonHint)
				{
					this._nextButtonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "NextButtonHint");
				}
			}
		}

		// Token: 0x04000911 RID: 2321
		private EncyclopediaListItemVM _previousItem;

		// Token: 0x04000912 RID: 2322
		private EncyclopediaListItemVM _nextItem;

		// Token: 0x04000913 RID: 2323
		private TextObject _previousButtonLabelText = new TextObject("{=zlcMGAbn}Previous Page", null);

		// Token: 0x04000914 RID: 2324
		private TextObject _nextButtonLabelText = new TextObject("{=QFfMd5q3}Next Page", null);

		// Token: 0x04000915 RID: 2325
		private bool _isPreviousButtonEnabled;

		// Token: 0x04000916 RID: 2326
		private bool _isNextButtonEnabled;

		// Token: 0x04000917 RID: 2327
		private string _previousButtonLabel;

		// Token: 0x04000918 RID: 2328
		private string _nextButtonLabel;

		// Token: 0x04000919 RID: 2329
		private HintViewModel _previousButtonHint;

		// Token: 0x0400091A RID: 2330
		private HintViewModel _nextButtonHint;
	}
}
