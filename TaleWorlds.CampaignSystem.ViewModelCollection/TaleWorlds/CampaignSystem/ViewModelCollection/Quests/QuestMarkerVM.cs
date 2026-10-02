using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Quests
{
	// Token: 0x02000023 RID: 35
	public class QuestMarkerVM : ViewModel
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000227 RID: 551 RVA: 0x000132C6 File Offset: 0x000114C6
		// (set) Token: 0x06000228 RID: 552 RVA: 0x000132CE File Offset: 0x000114CE
		public TextObject QuestTitle { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000132D7 File Offset: 0x000114D7
		// (set) Token: 0x0600022A RID: 554 RVA: 0x000132DF File Offset: 0x000114DF
		public TextObject QuestHintText { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000132E8 File Offset: 0x000114E8
		// (set) Token: 0x0600022C RID: 556 RVA: 0x000132F0 File Offset: 0x000114F0
		public CampaignUIHelper.IssueQuestFlags IssueQuestFlag { get; private set; }

		// Token: 0x0600022D RID: 557 RVA: 0x000132F9 File Offset: 0x000114F9
		public QuestMarkerVM(CampaignUIHelper.IssueQuestFlags issueQuestFlag, TextObject questTitle = null, TextObject questHintText = null)
		{
			this.RefreshWith(issueQuestFlag, questTitle, questHintText);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0001330C File Offset: 0x0001150C
		public void RefreshWith(CampaignUIHelper.IssueQuestFlags issueQuestFlag, TextObject questTitle = null, TextObject questHintText = null)
		{
			this.IssueQuestFlag = issueQuestFlag;
			this.QuestMarkerType = (int)issueQuestFlag;
			this.QuestTitle = questTitle ?? TextObject.GetEmpty();
			this.QuestHintText = questHintText;
			if (this.QuestHintText != null)
			{
				this.QuestHint = new HintViewModel(this.QuestHintText, null);
			}
			this.IsTrackMarker = issueQuestFlag == CampaignUIHelper.IssueQuestFlags.TrackedIssue || issueQuestFlag == CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest;
			this.RefreshValues();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00013376 File Offset: 0x00011576
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (!TextObject.IsNullOrEmpty(this.QuestHintText))
			{
				this.QuestHint = new HintViewModel(this.QuestHintText, null);
				return;
			}
			this.QuestHint = new HintViewModel();
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000230 RID: 560 RVA: 0x000133A9 File Offset: 0x000115A9
		// (set) Token: 0x06000231 RID: 561 RVA: 0x000133B1 File Offset: 0x000115B1
		[DataSourceProperty]
		public bool IsTrackMarker
		{
			get
			{
				return this._isTrackMarker;
			}
			set
			{
				if (value != this._isTrackMarker)
				{
					this._isTrackMarker = value;
					base.OnPropertyChangedWithValue(value, "IsTrackMarker");
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000232 RID: 562 RVA: 0x000133CF File Offset: 0x000115CF
		// (set) Token: 0x06000233 RID: 563 RVA: 0x000133D7 File Offset: 0x000115D7
		[DataSourceProperty]
		public int QuestMarkerType
		{
			get
			{
				return this._questMarkerType;
			}
			set
			{
				if (value != this._questMarkerType)
				{
					this._questMarkerType = value;
					base.OnPropertyChangedWithValue(value, "QuestMarkerType");
				}
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000234 RID: 564 RVA: 0x000133F5 File Offset: 0x000115F5
		// (set) Token: 0x06000235 RID: 565 RVA: 0x000133FD File Offset: 0x000115FD
		[DataSourceProperty]
		public HintViewModel QuestHint
		{
			get
			{
				return this._questHint;
			}
			set
			{
				if (value != this._questHint)
				{
					this._questHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "QuestHint");
				}
			}
		}

		// Token: 0x04000101 RID: 257
		private bool _isTrackMarker;

		// Token: 0x04000102 RID: 258
		private int _questMarkerType;

		// Token: 0x04000103 RID: 259
		private HintViewModel _questHint;
	}
}
