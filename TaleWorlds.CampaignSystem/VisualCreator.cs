using System;
using TaleWorlds.CampaignSystem.MapEvents;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000AE RID: 174
	public class VisualCreator
	{
		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x0005ADF0 File Offset: 0x00058FF0
		// (set) Token: 0x06001380 RID: 4992 RVA: 0x0005ADF8 File Offset: 0x00058FF8
		public IMapEventVisualCreator MapEventVisualCreator { get; set; }

		// Token: 0x06001381 RID: 4993 RVA: 0x0005AE01 File Offset: 0x00059001
		public IMapEventVisual CreateMapEventVisual(MapEvent mapEvent)
		{
			IMapEventVisualCreator mapEventVisualCreator = this.MapEventVisualCreator;
			if (mapEventVisualCreator == null)
			{
				return null;
			}
			return mapEventVisualCreator.CreateMapEventVisual(mapEvent);
		}
	}
}
