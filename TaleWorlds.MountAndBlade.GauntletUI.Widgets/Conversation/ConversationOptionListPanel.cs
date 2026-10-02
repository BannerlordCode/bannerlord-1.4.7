using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x0200016F RID: 367
	public class ConversationOptionListPanel : ListPanel
	{
		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001341 RID: 4929 RVA: 0x0003452F File Offset: 0x0003272F
		// (set) Token: 0x06001342 RID: 4930 RVA: 0x00034537 File Offset: 0x00032737
		public ButtonWidget OptionButtonWidget { get; set; }

		// Token: 0x06001343 RID: 4931 RVA: 0x00034540 File Offset: 0x00032740
		public ConversationOptionListPanel(UIContext context)
			: base(context)
		{
		}
	}
}
