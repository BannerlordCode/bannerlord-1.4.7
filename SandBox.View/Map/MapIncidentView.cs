using System;
using TaleWorlds.CampaignSystem.Incidents;

namespace SandBox.View.Map
{
	// Token: 0x02000050 RID: 80
	public class MapIncidentView : MapView
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x00017FE5 File Offset: 0x000161E5
		public MapIncidentView()
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00017FED File Offset: 0x000161ED
		public MapIncidentView(Incident incident)
		{
			this.Incident = incident;
		}

		// Token: 0x04000173 RID: 371
		public readonly Incident Incident;
	}
}
