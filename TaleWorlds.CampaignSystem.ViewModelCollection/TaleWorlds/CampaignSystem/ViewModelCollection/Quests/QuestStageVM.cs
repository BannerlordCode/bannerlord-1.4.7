using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Quests
{
	// Token: 0x02000025 RID: 37
	public class QuestStageVM : ViewModel
	{
		// Token: 0x06000244 RID: 580 RVA: 0x0001354C File Offset: 0x0001174C
		public QuestStageVM(JournalLog log, string dateString, bool isLastStage, Action onLogNotified, QuestStageTaskVM stageTask = null)
		{
			this.StageTask = new QuestStageTaskVM(TextObject.GetEmpty(), 0, 0, LogType.None);
			this._onLogNotified = onLogNotified;
			string text = log.LogText.ToString();
			GameTexts.SetVariable("ENTRY", text);
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this.DateText = dateString;
			this.DescriptionText = log.LogText.ToString();
			this.IsLastStage = isLastStage;
			this.Log = log;
			this.UpdateIsNew();
			if (stageTask != null)
			{
				this.StageTask = stageTask;
				this.StageTask.IsValid = true;
				this.HasATask = true;
				this.IsTaskCompleted = this.StageTask.CurrentProgress == this.StageTask.TargetProgress;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0001360C File Offset: 0x0001180C
		public QuestStageVM(JournalLog log, string description, string dateString, bool isLastStage, Action onLogNotified)
		{
			this.Log = log;
			this.StageTask = new QuestStageTaskVM(TextObject.GetEmpty(), 0, 0, LogType.None);
			this._onLogNotified = onLogNotified;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this.DateText = dateString;
			this.DescriptionText = description;
			this.IsLastStage = isLastStage;
			this.UpdateIsNew();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0001366D File Offset: 0x0001186D
		public void ExecuteResetUpdated()
		{
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0001366F File Offset: 0x0001186F
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00013681 File Offset: 0x00011881
		public void UpdateIsNew()
		{
			if (this.Log != null)
			{
				this.IsNew = this._viewDataTracker.UnExaminedQuestLogs.Any<JournalLog>((JournalLog l) => l == this.Log);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000136AD File Offset: 0x000118AD
		// (set) Token: 0x0600024A RID: 586 RVA: 0x000136B5 File Offset: 0x000118B5
		[DataSourceProperty]
		public string DateText
		{
			get
			{
				return this._dateText;
			}
			set
			{
				if (value != this._dateText)
				{
					this._dateText = value;
					base.OnPropertyChangedWithValue<string>(value, "DateText");
				}
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000136D8 File Offset: 0x000118D8
		// (set) Token: 0x0600024C RID: 588 RVA: 0x000136E0 File Offset: 0x000118E0
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00013703 File Offset: 0x00011903
		// (set) Token: 0x0600024E RID: 590 RVA: 0x0001370B File Offset: 0x0001190B
		[DataSourceProperty]
		public bool HasATask
		{
			get
			{
				return this._hasATask;
			}
			set
			{
				if (value != this._hasATask)
				{
					this._hasATask = value;
					base.OnPropertyChangedWithValue(value, "HasATask");
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00013729 File Offset: 0x00011929
		// (set) Token: 0x06000250 RID: 592 RVA: 0x00013731 File Offset: 0x00011931
		[DataSourceProperty]
		public bool IsNew
		{
			get
			{
				return this._isNew;
			}
			set
			{
				if (value != this._isNew)
				{
					this._isNew = value;
					base.OnPropertyChangedWithValue(value, "IsNew");
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0001374F File Offset: 0x0001194F
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00013757 File Offset: 0x00011957
		[DataSourceProperty]
		public bool IsLastStage
		{
			get
			{
				return this._isLastStage;
			}
			set
			{
				if (value != this._isLastStage)
				{
					this._isLastStage = value;
					base.OnPropertyChangedWithValue(value, "IsLastStage");
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00013775 File Offset: 0x00011975
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0001377D File Offset: 0x0001197D
		[DataSourceProperty]
		public bool IsTaskCompleted
		{
			get
			{
				return this._isTaskCompleted;
			}
			set
			{
				if (value != this._isTaskCompleted)
				{
					this._isTaskCompleted = value;
					base.OnPropertyChangedWithValue(value, "IsTaskCompleted");
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0001379B File Offset: 0x0001199B
		// (set) Token: 0x06000256 RID: 598 RVA: 0x000137A3 File Offset: 0x000119A3
		[DataSourceProperty]
		public QuestStageTaskVM StageTask
		{
			get
			{
				return this._stageTask;
			}
			set
			{
				if (value != this._stageTask)
				{
					this._stageTask = value;
					base.OnPropertyChangedWithValue<QuestStageTaskVM>(value, "StageTask");
				}
			}
		}

		// Token: 0x0400010A RID: 266
		public readonly JournalLog Log;

		// Token: 0x0400010B RID: 267
		private readonly Action _onLogNotified;

		// Token: 0x0400010C RID: 268
		private readonly IViewDataTracker _viewDataTracker;

		// Token: 0x0400010D RID: 269
		private string _descriptionText;

		// Token: 0x0400010E RID: 270
		private string _dateText;

		// Token: 0x0400010F RID: 271
		private bool _hasATask;

		// Token: 0x04000110 RID: 272
		private bool _isNew;

		// Token: 0x04000111 RID: 273
		private bool _isTaskCompleted;

		// Token: 0x04000112 RID: 274
		private bool _isLastStage;

		// Token: 0x04000113 RID: 275
		private QuestStageTaskVM _stageTask;
	}
}
