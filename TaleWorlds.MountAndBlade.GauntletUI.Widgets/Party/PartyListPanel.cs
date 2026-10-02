using System;
using TaleWorlds.GauntletUI;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000066 RID: 102
	public class PartyListPanel : NavigatableListPanel
	{
		// Token: 0x06000572 RID: 1394 RVA: 0x0001065D File Offset: 0x0000E85D
		public PartyListPanel(UIContext context)
			: base(context)
		{
			base.ClearSelectedOnRemoval = true;
		}
	}
}
