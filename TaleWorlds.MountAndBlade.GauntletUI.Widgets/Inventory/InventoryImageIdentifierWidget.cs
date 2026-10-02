using System;
using TaleWorlds.GauntletUI;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013C RID: 316
	public class InventoryImageIdentifierWidget : ImageIdentifierWidget
	{
		// Token: 0x06001067 RID: 4199 RVA: 0x0002CDA3 File Offset: 0x0002AFA3
		public InventoryImageIdentifierWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0002CDAC File Offset: 0x0002AFAC
		public void SetRenderRequestedPreviousFrame(bool isRequested)
		{
			this._isRenderRequestedPreviousFrame = isRequested && base.IsRecursivelyVisible() && base.EventManager.AreaRectangle.IsCollide(in this.AreaRect);
		}
	}
}
