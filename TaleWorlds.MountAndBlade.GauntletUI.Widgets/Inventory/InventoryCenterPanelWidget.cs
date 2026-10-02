using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000139 RID: 313
	public class InventoryCenterPanelWidget : Widget
	{
		// Token: 0x06001049 RID: 4169 RVA: 0x0002C900 File Offset: 0x0002AB00
		public InventoryCenterPanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0002C909 File Offset: 0x0002AB09
		protected override bool OnPreviewDragBegin()
		{
			return false;
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0002C90C File Offset: 0x0002AB0C
		protected override bool OnPreviewDrop()
		{
			return true;
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0002C90F File Offset: 0x0002AB0F
		protected override bool OnPreviewDragHover()
		{
			return false;
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0002C912 File Offset: 0x0002AB12
		protected override bool OnPreviewMouseMove()
		{
			return false;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0002C915 File Offset: 0x0002AB15
		protected override bool OnPreviewMousePressed()
		{
			return false;
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0002C918 File Offset: 0x0002AB18
		protected override bool OnPreviewMouseReleased()
		{
			return false;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0002C91B File Offset: 0x0002AB1B
		protected override bool OnPreviewMouseScroll()
		{
			return false;
		}
	}
}
