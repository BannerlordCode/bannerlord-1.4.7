using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200007B RID: 123
	public class CampaignTutorial
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x0004E5BF File Offset: 0x0004C7BF
		public TextObject Description
		{
			get
			{
				return GameTexts.FindText("str_campaign_tutorial_description", this.TutorialTypeId);
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x0004E5D1 File Offset: 0x0004C7D1
		public TextObject Title
		{
			get
			{
				return GameTexts.FindText("str_campaign_tutorial_title", this.TutorialTypeId);
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0004E5E3 File Offset: 0x0004C7E3
		public CampaignTutorial(string tutorialType, int priority)
		{
			this.TutorialTypeId = tutorialType;
			this.Priority = priority;
		}

		// Token: 0x0400049F RID: 1183
		public readonly string TutorialTypeId;

		// Token: 0x040004A0 RID: 1184
		public readonly int Priority;
	}
}
