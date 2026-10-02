using System;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x0200037E RID: 894
	public struct PotentialIssueData
	{
		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x0600346C RID: 13420 RVA: 0x000D8988 File Offset: 0x000D6B88
		public PotentialIssueData.StartIssueDelegate OnStartIssue { get; }

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x0600346D RID: 13421 RVA: 0x000D8990 File Offset: 0x000D6B90
		public string IssueId { get; }

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x0600346E RID: 13422 RVA: 0x000D8998 File Offset: 0x000D6B98
		public Type IssueType { get; }

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x0600346F RID: 13423 RVA: 0x000D89A0 File Offset: 0x000D6BA0
		public IssueBase.IssueFrequency Frequency { get; }

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06003470 RID: 13424 RVA: 0x000D89A8 File Offset: 0x000D6BA8
		public object RelatedObject { get; }

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06003471 RID: 13425 RVA: 0x000D89B0 File Offset: 0x000D6BB0
		public bool IsValid
		{
			get
			{
				return this.OnStartIssue != null;
			}
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x000D89BB File Offset: 0x000D6BBB
		public PotentialIssueData(PotentialIssueData.StartIssueDelegate onStartIssue, Type issueType, IssueBase.IssueFrequency frequency, object relatedObject = null)
		{
			this.OnStartIssue = onStartIssue;
			this.IssueId = issueType.Name;
			this.IssueType = issueType;
			this.Frequency = frequency;
			this.RelatedObject = relatedObject;
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x000D89E6 File Offset: 0x000D6BE6
		public PotentialIssueData(Type issueType, IssueBase.IssueFrequency frequency)
		{
			this.OnStartIssue = null;
			this.IssueId = issueType.Name;
			this.IssueType = issueType;
			this.Frequency = frequency;
			this.RelatedObject = null;
		}

		// Token: 0x0200076B RID: 1899
		// (Invoke) Token: 0x06006101 RID: 24833
		public delegate IssueBase StartIssueDelegate(in PotentialIssueData pid, Hero issueOwner);
	}
}
