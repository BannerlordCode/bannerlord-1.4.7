using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x02000158 RID: 344
	public class EncyclopediaFilterListItemButtonWidget : ButtonWidget
	{
		// Token: 0x0600124D RID: 4685 RVA: 0x000326E9 File Offset: 0x000308E9
		public EncyclopediaFilterListItemButtonWidget(UIContext context)
			: base(context)
		{
			base.OverrideDefaultStateSwitchingEnabled = true;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x000326FC File Offset: 0x000308FC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.IsDisabled)
			{
				this.SetState("Disabled");
				return;
			}
			if (base.IsHovered)
			{
				this.SetState("Hovered");
				return;
			}
			if (base.IsSelected)
			{
				this.SetState("Selected");
				return;
			}
			if (base.IsPressed)
			{
				this.SetState("Pressed");
				return;
			}
			this.SetState("Default");
		}
	}
}
