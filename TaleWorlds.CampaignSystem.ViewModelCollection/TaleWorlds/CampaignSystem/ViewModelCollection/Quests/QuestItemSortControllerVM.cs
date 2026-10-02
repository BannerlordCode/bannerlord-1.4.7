using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Quests
{
	// Token: 0x02000021 RID: 33
	public class QuestItemSortControllerVM : ViewModel
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x000128F2 File Offset: 0x00010AF2
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x000128FA File Offset: 0x00010AFA
		public QuestItemSortControllerVM.QuestItemSortOption? CurrentSortOption { get; private set; }

		// Token: 0x060001F2 RID: 498 RVA: 0x00012904 File Offset: 0x00010B04
		public QuestItemSortControllerVM(ref MBBindingList<QuestItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._dateStartedComparer = new QuestItemSortControllerVM.QuestItemDateStartedComparer();
			this._lastUpdatedComparer = new QuestItemSortControllerVM.QuestItemLastUpdatedComparer();
			this._timeDueComparer = new QuestItemSortControllerVM.QuestItemTimeDueComparer();
			this.IsThereAnyQuest = this._listToControl.Count > 0;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00012954 File Offset: 0x00010B54
		private void ExecuteSortByDateStarted()
		{
			this._listToControl.Sort(this._dateStartedComparer);
			this.CurrentSortOption = new QuestItemSortControllerVM.QuestItemSortOption?(QuestItemSortControllerVM.QuestItemSortOption.DateStarted);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00012973 File Offset: 0x00010B73
		private void ExecuteSortByLastUpdated()
		{
			this._listToControl.Sort(this._lastUpdatedComparer);
			this.CurrentSortOption = new QuestItemSortControllerVM.QuestItemSortOption?(QuestItemSortControllerVM.QuestItemSortOption.LastUpdated);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00012992 File Offset: 0x00010B92
		private void ExecuteSortByTimeDue()
		{
			this._listToControl.Sort(this._timeDueComparer);
			this.CurrentSortOption = new QuestItemSortControllerVM.QuestItemSortOption?(QuestItemSortControllerVM.QuestItemSortOption.TimeDue);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000129B1 File Offset: 0x00010BB1
		public void SortByOption(QuestItemSortControllerVM.QuestItemSortOption sortOption)
		{
			if (sortOption == QuestItemSortControllerVM.QuestItemSortOption.DateStarted)
			{
				this.ExecuteSortByDateStarted();
				return;
			}
			if (sortOption == QuestItemSortControllerVM.QuestItemSortOption.LastUpdated)
			{
				this.ExecuteSortByLastUpdated();
				return;
			}
			if (sortOption == QuestItemSortControllerVM.QuestItemSortOption.TimeDue)
			{
				this.ExecuteSortByTimeDue();
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x000129D2 File Offset: 0x00010BD2
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x000129DA File Offset: 0x00010BDA
		[DataSourceProperty]
		public bool IsThereAnyQuest
		{
			get
			{
				return this._isThereAnyQuest;
			}
			set
			{
				if (value != this._isThereAnyQuest)
				{
					this._isThereAnyQuest = value;
					base.OnPropertyChangedWithValue(value, "IsThereAnyQuest");
				}
			}
		}

		// Token: 0x040000E2 RID: 226
		private MBBindingList<QuestItemVM> _listToControl;

		// Token: 0x040000E3 RID: 227
		private QuestItemSortControllerVM.QuestItemDateStartedComparer _dateStartedComparer;

		// Token: 0x040000E4 RID: 228
		private QuestItemSortControllerVM.QuestItemLastUpdatedComparer _lastUpdatedComparer;

		// Token: 0x040000E5 RID: 229
		private QuestItemSortControllerVM.QuestItemTimeDueComparer _timeDueComparer;

		// Token: 0x040000E7 RID: 231
		private bool _isThereAnyQuest;

		// Token: 0x0200018C RID: 396
		public enum QuestItemSortOption
		{
			// Token: 0x04001081 RID: 4225
			DateStarted,
			// Token: 0x04001082 RID: 4226
			LastUpdated,
			// Token: 0x04001083 RID: 4227
			TimeDue
		}

		// Token: 0x0200018D RID: 397
		private abstract class QuestItemComparerBase : IComparer<QuestItemVM>
		{
			// Token: 0x060022BF RID: 8895
			public abstract int Compare(QuestItemVM x, QuestItemVM y);

			// Token: 0x060022C0 RID: 8896 RVA: 0x0007DB0C File Offset: 0x0007BD0C
			protected JournalLog GetJournalLogAt(QuestItemVM questItem, QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex logIndex)
			{
				if (questItem.Quest == null && questItem.Stages.Count > 0)
				{
					int num = ((logIndex == QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.First) ? 0 : (questItem.Stages.Count - 1));
					return questItem.Stages[num].Log;
				}
				if (questItem.Quest != null && questItem.Quest.JournalEntries.Count > 0)
				{
					int num2 = ((logIndex == QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.First) ? 0 : (questItem.Quest.JournalEntries.Count - 1));
					return questItem.Quest.JournalEntries[num2];
				}
				return null;
			}

			// Token: 0x020002F4 RID: 756
			protected enum JournalLogIndex
			{
				// Token: 0x04001408 RID: 5128
				First,
				// Token: 0x04001409 RID: 5129
				Last
			}
		}

		// Token: 0x0200018E RID: 398
		private class QuestItemDateStartedComparer : QuestItemSortControllerVM.QuestItemComparerBase
		{
			// Token: 0x060022C2 RID: 8898 RVA: 0x0007DBA4 File Offset: 0x0007BDA4
			public override int Compare(QuestItemVM first, QuestItemVM second)
			{
				JournalLog journalLogAt = base.GetJournalLogAt(first, QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.First);
				JournalLog journalLogAt2 = base.GetJournalLogAt(second, QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.First);
				if (journalLogAt != null && journalLogAt2 != null)
				{
					return journalLogAt.LogTime.CompareTo(journalLogAt2.LogTime);
				}
				if (journalLogAt == null && journalLogAt2 != null)
				{
					return -1;
				}
				if (journalLogAt != null && journalLogAt2 == null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x0200018F RID: 399
		private class QuestItemLastUpdatedComparer : QuestItemSortControllerVM.QuestItemComparerBase
		{
			// Token: 0x060022C4 RID: 8900 RVA: 0x0007DBF8 File Offset: 0x0007BDF8
			public override int Compare(QuestItemVM first, QuestItemVM second)
			{
				JournalLog journalLogAt = base.GetJournalLogAt(first, QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.Last);
				JournalLog journalLogAt2 = base.GetJournalLogAt(second, QuestItemSortControllerVM.QuestItemComparerBase.JournalLogIndex.Last);
				if (journalLogAt != null && journalLogAt2 != null)
				{
					return journalLogAt2.LogTime.CompareTo(journalLogAt.LogTime);
				}
				if (journalLogAt == null && journalLogAt2 != null)
				{
					return -1;
				}
				if (journalLogAt != null && journalLogAt2 == null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x02000190 RID: 400
		private class QuestItemTimeDueComparer : QuestItemSortControllerVM.QuestItemComparerBase
		{
			// Token: 0x060022C6 RID: 8902 RVA: 0x0007DC4C File Offset: 0x0007BE4C
			public override int Compare(QuestItemVM first, QuestItemVM second)
			{
				CampaignTime campaignTime = CampaignTime.Now;
				CampaignTime campaignTime2 = CampaignTime.Now;
				if (first.Quest != null)
				{
					campaignTime = first.Quest.QuestDueTime;
				}
				if (second.Quest != null)
				{
					campaignTime2 = second.Quest.QuestDueTime;
				}
				return campaignTime.CompareTo(campaignTime2);
			}
		}
	}
}
