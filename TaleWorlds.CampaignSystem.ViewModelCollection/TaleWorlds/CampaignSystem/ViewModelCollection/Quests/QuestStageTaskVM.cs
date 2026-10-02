using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Quests
{
	// Token: 0x02000024 RID: 36
	public class QuestStageTaskVM : ViewModel
	{
		// Token: 0x06000236 RID: 566 RVA: 0x0001341B File Offset: 0x0001161B
		public QuestStageTaskVM(TextObject taskName, int currentProgress, int targetProgress, LogType type)
		{
			this._taskNameObj = taskName;
			this.CurrentProgress = currentProgress;
			this.TargetProgress = targetProgress;
			base.OnPropertyChanged("NegativeTargetProgress");
			this.ProgressType = (int)type;
			this.RefreshValues();
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00013451 File Offset: 0x00011651
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TaskName = this._taskNameObj.ToString();
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0001346A File Offset: 0x0001166A
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0001347C File Offset: 0x0001167C
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00013484 File Offset: 0x00011684
		[DataSourceProperty]
		public string TaskName
		{
			get
			{
				return this._taskName;
			}
			set
			{
				if (value != this._taskName)
				{
					this._taskName = value;
					base.OnPropertyChangedWithValue<string>(value, "TaskName");
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600023B RID: 571 RVA: 0x000134A7 File Offset: 0x000116A7
		// (set) Token: 0x0600023C RID: 572 RVA: 0x000134AF File Offset: 0x000116AF
		[DataSourceProperty]
		public bool IsValid
		{
			get
			{
				return this._isValid;
			}
			set
			{
				if (value != this._isValid)
				{
					this._isValid = value;
					base.OnPropertyChangedWithValue(value, "IsValid");
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600023D RID: 573 RVA: 0x000134CD File Offset: 0x000116CD
		// (set) Token: 0x0600023E RID: 574 RVA: 0x000134D5 File Offset: 0x000116D5
		[DataSourceProperty]
		public int CurrentProgress
		{
			get
			{
				return this._currentProgress;
			}
			set
			{
				if (value != this._currentProgress)
				{
					this._currentProgress = value;
					base.OnPropertyChangedWithValue(value, "CurrentProgress");
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600023F RID: 575 RVA: 0x000134F3 File Offset: 0x000116F3
		// (set) Token: 0x06000240 RID: 576 RVA: 0x000134FB File Offset: 0x000116FB
		[DataSourceProperty]
		public int TargetProgress
		{
			get
			{
				return this._targetProgress;
			}
			set
			{
				if (value != this._targetProgress)
				{
					this._targetProgress = value;
					base.OnPropertyChangedWithValue(value, "TargetProgress");
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00013519 File Offset: 0x00011719
		[DataSourceProperty]
		public int NegativeTargetProgress
		{
			get
			{
				return this._targetProgress * -1;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00013523 File Offset: 0x00011723
		// (set) Token: 0x06000243 RID: 579 RVA: 0x0001352B File Offset: 0x0001172B
		[DataSourceProperty]
		public int ProgressType
		{
			get
			{
				return this._progressType;
			}
			set
			{
				if (value != this._progressType)
				{
					this._progressType = value;
					base.OnPropertyChangedWithValue(value, "ProgressType");
				}
			}
		}

		// Token: 0x04000104 RID: 260
		private readonly TextObject _taskNameObj;

		// Token: 0x04000105 RID: 261
		private string _taskName;

		// Token: 0x04000106 RID: 262
		private int _currentProgress;

		// Token: 0x04000107 RID: 263
		private int _targetProgress;

		// Token: 0x04000108 RID: 264
		private int _progressType;

		// Token: 0x04000109 RID: 265
		private bool _isValid;
	}
}
