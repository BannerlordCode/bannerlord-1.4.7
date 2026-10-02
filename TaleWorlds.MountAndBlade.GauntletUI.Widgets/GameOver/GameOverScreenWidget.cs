using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameOver
{
	// Token: 0x02000151 RID: 337
	public class GameOverScreenWidget : Widget
	{
		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00031776 File Offset: 0x0002F976
		// (set) Token: 0x060011D1 RID: 4561 RVA: 0x0003177E File Offset: 0x0002F97E
		public BrushWidget ConceptVisualWidget { get; set; }

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x00031787 File Offset: 0x0002F987
		// (set) Token: 0x060011D3 RID: 4563 RVA: 0x0003178F File Offset: 0x0002F98F
		public BrushWidget BannerBrushWidget { get; set; }

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x00031798 File Offset: 0x0002F998
		// (set) Token: 0x060011D5 RID: 4565 RVA: 0x000317A0 File Offset: 0x0002F9A0
		public BrushWidget BannerFrameBrushWidget1 { get; set; }

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x000317A9 File Offset: 0x0002F9A9
		// (set) Token: 0x060011D7 RID: 4567 RVA: 0x000317B1 File Offset: 0x0002F9B1
		public BrushWidget BannerFrameBrushWidget2 { get; set; }

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x000317BA File Offset: 0x0002F9BA
		// (set) Token: 0x060011D9 RID: 4569 RVA: 0x000317C2 File Offset: 0x0002F9C2
		public string GameOverReason { get; set; }

		// Token: 0x060011DA RID: 4570 RVA: 0x000317CB File Offset: 0x0002F9CB
		public GameOverScreenWidget(UIContext context)
			: base(context)
		{
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.OnManualLateUpdate), 4);
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x000317F0 File Offset: 0x0002F9F0
		private void OnManualLateUpdate(float obj)
		{
			if (this.ConceptVisualWidget != null)
			{
				this.ConceptVisualWidget.Brush = base.Context.GetBrush("GameOver.Mask." + this.GameOverReason);
			}
			if (this.BannerBrushWidget != null)
			{
				this.BannerBrushWidget.Brush = base.Context.GetBrush("GameOver.Banner." + this.GameOverReason);
			}
			if (this.BannerFrameBrushWidget1 != null)
			{
				this.BannerFrameBrushWidget1.Brush = base.Context.GetBrush("GameOver.Banner.Frame." + this.GameOverReason);
			}
			if (this.BannerFrameBrushWidget2 != null)
			{
				this.BannerFrameBrushWidget2.Brush = base.Context.GetBrush("GameOver.Banner.Frame." + this.GameOverReason);
			}
		}
	}
}
