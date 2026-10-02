using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000039 RID: 57
	public abstract class WidgetComponent
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0000FCED File Offset: 0x0000DEED
		// (set) Token: 0x060003E5 RID: 997 RVA: 0x0000FCF5 File Offset: 0x0000DEF5
		public Widget Target { get; private set; }

		// Token: 0x060003E6 RID: 998 RVA: 0x0000FCFE File Offset: 0x0000DEFE
		protected WidgetComponent(Widget target)
		{
			this.Target = target;
		}
	}
}
