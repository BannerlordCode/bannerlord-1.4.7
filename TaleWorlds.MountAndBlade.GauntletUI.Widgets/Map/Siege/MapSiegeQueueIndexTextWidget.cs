using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Siege
{
	// Token: 0x0200011D RID: 285
	public class MapSiegeQueueIndexTextWidget : TextWidget
	{
		// Token: 0x06000F1A RID: 3866 RVA: 0x0002991A File Offset: 0x00027B1A
		public MapSiegeQueueIndexTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00029923 File Offset: 0x00027B23
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			base.IsVisible = base.IntText > 0;
		}
	}
}
