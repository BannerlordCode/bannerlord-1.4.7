using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C8 RID: 456
	public abstract class DisguiseDetectionModel : MBGameModel<DisguiseDetectionModel>
	{
		// Token: 0x06001E11 RID: 7697
		public abstract float CalculateDisguiseDetectionProbability(Settlement settlement);
	}
}
