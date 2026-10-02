using System;
using TaleWorlds.CampaignSystem.MapEvents;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000AF RID: 175
	public interface IMapEventVisualCreator
	{
		// Token: 0x06001383 RID: 4995
		IMapEventVisual CreateMapEventVisual(MapEvent mapEvent);
	}
}
