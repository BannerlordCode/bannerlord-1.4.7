using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DC RID: 476
	public abstract class PlayerProgressionModel : MBGameModel<PlayerProgressionModel>
	{
		// Token: 0x06001EA8 RID: 7848
		public abstract float GetPlayerProgress();
	}
}
