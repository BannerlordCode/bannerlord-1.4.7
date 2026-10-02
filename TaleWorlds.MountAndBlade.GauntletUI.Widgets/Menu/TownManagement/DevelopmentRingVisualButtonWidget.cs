using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x0200010C RID: 268
	public class DevelopmentRingVisualButtonWidget : ButtonWidget
	{
		// Token: 0x06000E47 RID: 3655 RVA: 0x0002767F File Offset: 0x0002587F
		public DevelopmentRingVisualButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00027688 File Offset: 0x00025888
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!base.IsSelected)
			{
				this.SetState(base.ParentWidget.CurrentState);
				return;
			}
			this.SetState("Selected");
		}
	}
}
