using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.AdminMessage
{
	// Token: 0x020000D1 RID: 209
	public class MultiplayerAdminMessageItemWidget : Widget
	{
		// Token: 0x06000ACA RID: 2762 RVA: 0x0001E2D8 File Offset: 0x0001C4D8
		public MultiplayerAdminMessageItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0001E2E1 File Offset: 0x0001C4E1
		public void Remove()
		{
			base.EventFired("Remove", Array.Empty<object>());
		}
	}
}
