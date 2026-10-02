using System;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003A1 RID: 929
	public class QuestsState : GameState
	{
		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06003592 RID: 13714 RVA: 0x000DA040 File Offset: 0x000D8240
		// (set) Token: 0x06003593 RID: 13715 RVA: 0x000DA048 File Offset: 0x000D8248
		public IssueBase InitialSelectedIssue { get; private set; }

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06003594 RID: 13716 RVA: 0x000DA051 File Offset: 0x000D8251
		// (set) Token: 0x06003595 RID: 13717 RVA: 0x000DA059 File Offset: 0x000D8259
		public QuestBase InitialSelectedQuest { get; private set; }

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06003596 RID: 13718 RVA: 0x000DA062 File Offset: 0x000D8262
		// (set) Token: 0x06003597 RID: 13719 RVA: 0x000DA06A File Offset: 0x000D826A
		public JournalLogEntry InitialSelectedLog { get; private set; }

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06003598 RID: 13720 RVA: 0x000DA073 File Offset: 0x000D8273
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06003599 RID: 13721 RVA: 0x000DA076 File Offset: 0x000D8276
		// (set) Token: 0x0600359A RID: 13722 RVA: 0x000DA07E File Offset: 0x000D827E
		public IQuestsStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x000DA087 File Offset: 0x000D8287
		public QuestsState()
		{
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000DA08F File Offset: 0x000D828F
		public QuestsState(IssueBase initialSelectedIssue)
		{
			this.InitialSelectedIssue = initialSelectedIssue;
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x000DA09E File Offset: 0x000D829E
		public QuestsState(QuestBase initialSelectedQuest)
		{
			this.InitialSelectedQuest = initialSelectedQuest;
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x000DA0AD File Offset: 0x000D82AD
		public QuestsState(JournalLogEntry initialSelectedLog)
		{
			this.InitialSelectedLog = initialSelectedLog;
		}

		// Token: 0x04000F4E RID: 3918
		private IQuestsStateHandler _handler;
	}
}
