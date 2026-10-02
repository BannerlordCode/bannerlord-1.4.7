using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace StoryMode.Extensions
{
	// Token: 0x02000058 RID: 88
	public static class Extensions
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x0001FDB4 File Offset: 0x0001DFB4
		public static bool IsTrainingField(this Settlement settlement)
		{
			return settlement.SettlementComponent is TrainingField;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001FDC4 File Offset: 0x0001DFC4
		public static TrainingField TrainingField(this Settlement settlement)
		{
			return settlement.SettlementComponent as TrainingField;
		}
	}
}
