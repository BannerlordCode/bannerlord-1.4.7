using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000F5 RID: 245
	public class EducationReviewItemVM : ViewModel
	{
		// Token: 0x0600162E RID: 5678 RVA: 0x00056E53 File Offset: 0x00055053
		public void UpdateWith(string gainText)
		{
			this.GainText = gainText;
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x0600162F RID: 5679 RVA: 0x00056E5C File Offset: 0x0005505C
		// (set) Token: 0x06001630 RID: 5680 RVA: 0x00056E64 File Offset: 0x00055064
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

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001631 RID: 5681 RVA: 0x00056E87 File Offset: 0x00055087
		// (set) Token: 0x06001632 RID: 5682 RVA: 0x00056E8F File Offset: 0x0005508F
		[DataSourceProperty]
		public string GainText
		{
			get
			{
				return this._gainText;
			}
			set
			{
				if (value != this._gainText)
				{
					this._gainText = value;
					base.OnPropertyChangedWithValue<string>(value, "GainText");
				}
			}
		}

		// Token: 0x04000A16 RID: 2582
		private string _title;

		// Token: 0x04000A17 RID: 2583
		private string _gainText;
	}
}
