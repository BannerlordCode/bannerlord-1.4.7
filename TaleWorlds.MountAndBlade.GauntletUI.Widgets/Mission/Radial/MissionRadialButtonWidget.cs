using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.Radial
{
	// Token: 0x020000E6 RID: 230
	public class MissionRadialButtonWidget : ButtonWidget
	{
		// Token: 0x06000BDC RID: 3036 RVA: 0x00020C53 File Offset: 0x0001EE53
		public MissionRadialButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00020C5C File Offset: 0x0001EE5C
		public void ExecuteFocused()
		{
			if (base.IsDisabled)
			{
				this.SetState("DisabledSelected");
			}
			base.EventFired("OnFocused", Array.Empty<object>());
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00020C81 File Offset: 0x0001EE81
		public void ExecuteUnfocused()
		{
			if (base.IsDisabled)
			{
				this.SetState("Disabled");
				return;
			}
			this.SetState("Default");
		}
	}
}
