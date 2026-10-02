using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameOver
{
	// Token: 0x02000150 RID: 336
	public class GameOverCategoryIconBrushWidget : BrushWidget
	{
		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x00031720 File Offset: 0x0002F920
		// (set) Token: 0x060011CD RID: 4557 RVA: 0x00031728 File Offset: 0x0002F928
		public string CategoryID { get; set; }

		// Token: 0x060011CE RID: 4558 RVA: 0x00031731 File Offset: 0x0002F931
		public GameOverCategoryIconBrushWidget(UIContext context)
			: base(context)
		{
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.OnManualLateUpdate), 4);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x00031753 File Offset: 0x0002F953
		private void OnManualLateUpdate(float obj)
		{
			base.Brush = base.Context.GetBrush("GameOver.Category.Visual." + this.CategoryID);
		}
	}
}
