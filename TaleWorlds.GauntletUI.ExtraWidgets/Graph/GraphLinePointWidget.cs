using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets.Graph
{
	// Token: 0x02000019 RID: 25
	public class GraphLinePointWidget : BrushWidget
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000075B3 File Offset: 0x000057B3
		// (set) Token: 0x0600014A RID: 330 RVA: 0x000075BB File Offset: 0x000057BB
		public float HorizontalValue { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000075C4 File Offset: 0x000057C4
		// (set) Token: 0x0600014C RID: 332 RVA: 0x000075CC File Offset: 0x000057CC
		public float VerticalValue { get; set; }

		// Token: 0x0600014D RID: 333 RVA: 0x000075D5 File Offset: 0x000057D5
		public GraphLinePointWidget(UIContext context)
			: base(context)
		{
		}
	}
}
