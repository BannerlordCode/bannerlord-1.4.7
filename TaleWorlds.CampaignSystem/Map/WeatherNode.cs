using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x02000224 RID: 548
	public class WeatherNode
	{
		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060020EB RID: 8427 RVA: 0x00091F94 File Offset: 0x00090194
		// (set) Token: 0x060020EC RID: 8428 RVA: 0x00091F9C File Offset: 0x0009019C
		public bool IsVisuallyDirty { get; private set; }

		// Token: 0x060020ED RID: 8429 RVA: 0x00091FA5 File Offset: 0x000901A5
		public WeatherNode(CampaignVec2 position)
		{
			this.Position = position;
			this.CurrentWeatherEvent = MapWeatherModel.WeatherEvent.Clear;
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x00091FBB File Offset: 0x000901BB
		public void SetVisualDirty()
		{
			this.IsVisuallyDirty = true;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00091FC4 File Offset: 0x000901C4
		public void OnVisualUpdated()
		{
			this.IsVisuallyDirty = false;
		}

		// Token: 0x040009B5 RID: 2485
		public CampaignVec2 Position;

		// Token: 0x040009B7 RID: 2487
		public MapWeatherModel.WeatherEvent CurrentWeatherEvent;
	}
}
