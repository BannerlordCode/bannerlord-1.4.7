using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D9 RID: 473
	public abstract class WallHitPointCalculationModel : MBGameModel<WallHitPointCalculationModel>
	{
		// Token: 0x06001E98 RID: 7832
		public abstract float CalculateMaximumWallHitPoint(Town town);
	}
}
