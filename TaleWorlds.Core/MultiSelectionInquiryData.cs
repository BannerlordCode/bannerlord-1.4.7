using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x020000C1 RID: 193
	public class MultiSelectionInquiryData
	{
		// Token: 0x06000ABF RID: 2751 RVA: 0x00022CD0 File Offset: 0x00020ED0
		public MultiSelectionInquiryData(string titleText, string descriptionText, List<InquiryElement> inquiryElements, bool isExitShown, int minSelectableOptionCount, int maxSelectableOptionCount, string affirmativeText, string negativeText, Action<List<InquiryElement>> affirmativeAction, Action<List<InquiryElement>> negativeAction, string soundEventPath = "", bool isSeachAvailable = false)
		{
			this.TitleText = titleText;
			this.DescriptionText = descriptionText;
			this.InquiryElements = inquiryElements;
			this.IsExitShown = isExitShown;
			this.AffirmativeText = affirmativeText;
			this.NegativeText = negativeText;
			this.AffirmativeAction = affirmativeAction;
			this.NegativeAction = negativeAction;
			this.MinSelectableOptionCount = minSelectableOptionCount;
			this.MaxSelectableOptionCount = maxSelectableOptionCount;
			this.SoundEventPath = soundEventPath;
			this.IsSeachAvailable = isSeachAvailable;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00022D40 File Offset: 0x00020F40
		public bool HasSameContentWith(object other)
		{
			MultiSelectionInquiryData multiSelectionInquiryData;
			if ((multiSelectionInquiryData = other as MultiSelectionInquiryData) != null)
			{
				bool flag = true;
				if (this.InquiryElements.Count == multiSelectionInquiryData.InquiryElements.Count)
				{
					for (int i = 0; i < this.InquiryElements.Count; i++)
					{
						if (!this.InquiryElements[i].HasSameContentWith(multiSelectionInquiryData.InquiryElements[i]))
						{
							flag = false;
						}
					}
				}
				else
				{
					flag = false;
				}
				return this.TitleText == multiSelectionInquiryData.TitleText && this.DescriptionText == multiSelectionInquiryData.DescriptionText && flag && this.IsExitShown == multiSelectionInquiryData.IsExitShown && this.AffirmativeText == multiSelectionInquiryData.AffirmativeText && this.NegativeText == multiSelectionInquiryData.NegativeText && this.AffirmativeAction == multiSelectionInquiryData.AffirmativeAction && this.NegativeAction == multiSelectionInquiryData.NegativeAction && this.MinSelectableOptionCount == multiSelectionInquiryData.MinSelectableOptionCount && this.MaxSelectableOptionCount == multiSelectionInquiryData.MaxSelectableOptionCount && this.SoundEventPath == multiSelectionInquiryData.SoundEventPath;
			}
			return false;
		}

		// Token: 0x040005EE RID: 1518
		public readonly string TitleText;

		// Token: 0x040005EF RID: 1519
		public readonly string DescriptionText;

		// Token: 0x040005F0 RID: 1520
		public readonly List<InquiryElement> InquiryElements;

		// Token: 0x040005F1 RID: 1521
		public readonly bool IsExitShown;

		// Token: 0x040005F2 RID: 1522
		public readonly int MaxSelectableOptionCount;

		// Token: 0x040005F3 RID: 1523
		public readonly int MinSelectableOptionCount;

		// Token: 0x040005F4 RID: 1524
		public readonly string SoundEventPath;

		// Token: 0x040005F5 RID: 1525
		public readonly string AffirmativeText;

		// Token: 0x040005F6 RID: 1526
		public readonly string NegativeText;

		// Token: 0x040005F7 RID: 1527
		public readonly Action<List<InquiryElement>> AffirmativeAction;

		// Token: 0x040005F8 RID: 1528
		public readonly Action<List<InquiryElement>> NegativeAction;

		// Token: 0x040005F9 RID: 1529
		public readonly bool IsSeachAvailable;
	}
}
