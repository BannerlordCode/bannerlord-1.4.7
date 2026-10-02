using System;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E4 RID: 484
	public abstract class SiegeStrategyActionModel : MBGameModel<SiegeStrategyActionModel>
	{
		// Token: 0x06001EE2 RID: 7906
		public abstract void GetLogicalActionForStrategy(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex);

		// Token: 0x02000605 RID: 1541
		public enum SiegeAction
		{
			// Token: 0x04001921 RID: 6433
			ConstructNewSiegeEngine,
			// Token: 0x04001922 RID: 6434
			DeploySiegeEngineFromReserve,
			// Token: 0x04001923 RID: 6435
			MoveSiegeEngineToReserve,
			// Token: 0x04001924 RID: 6436
			RemoveDeployedSiegeEngine,
			// Token: 0x04001925 RID: 6437
			Hold
		}
	}
}
