using System;
using TaleWorlds.GauntletUI.Layout;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000056 RID: 86
	public class DragCarrierWidget : Widget
	{
		// Token: 0x060005C0 RID: 1472 RVA: 0x00017F47 File Offset: 0x00016147
		public DragCarrierWidget(UIContext context)
			: base(context)
		{
			base.LayoutImp = new DragCarrierLayout();
			base.DoNotAcceptEvents = true;
			base.DoNotPassEventsToChildren = true;
			base.IsDisabled = true;
		}
	}
}
