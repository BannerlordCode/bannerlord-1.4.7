using System;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D8 RID: 472
	public abstract class BuildingEffectModel : MBGameModel<BuildingEffectModel>
	{
		// Token: 0x06001E96 RID: 7830
		public abstract ExplainedNumber GetBuildingEffect(Building building, BuildingEffectEnum effect);
	}
}
