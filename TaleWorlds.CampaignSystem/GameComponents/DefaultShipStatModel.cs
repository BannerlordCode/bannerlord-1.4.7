using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000155 RID: 341
	public class DefaultShipStatModel : ShipStatModel
	{
		// Token: 0x06001A7A RID: 6778 RVA: 0x00086583 File Offset: 0x00084783
		public override float GetShipFlagshipScore(Ship ship)
		{
			return 0f;
		}
	}
}
