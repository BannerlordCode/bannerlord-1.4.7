using System;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x02000352 RID: 850
	public interface IWarLog
	{
		// Token: 0x0600323A RID: 12858
		bool IsRelatedToWar(StanceLink stance, out IFaction effector, out IFaction effected);
	}
}
