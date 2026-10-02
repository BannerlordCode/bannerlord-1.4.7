using System;
using TaleWorlds.GauntletUI;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.Radial
{
	// Token: 0x020000E7 RID: 231
	public class MissionRadialCircleActionSelectorWidget : CircleActionSelectorWidget
	{
		// Token: 0x06000BDF RID: 3039 RVA: 0x00020CA2 File Offset: 0x0001EEA2
		public MissionRadialCircleActionSelectorWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00020CAC File Offset: 0x0001EEAC
		protected override void OnSelectedIndexChanged(int selectedIndex)
		{
			base.OnSelectedIndexChanged(selectedIndex);
			for (int i = 0; i < base.Children.Count; i++)
			{
				MissionRadialButtonWidget missionRadialButtonWidget;
				if ((missionRadialButtonWidget = base.Children[i] as MissionRadialButtonWidget) != null)
				{
					if (i == selectedIndex)
					{
						missionRadialButtonWidget.ExecuteFocused();
					}
					else
					{
						missionRadialButtonWidget.ExecuteUnfocused();
					}
				}
			}
		}
	}
}
