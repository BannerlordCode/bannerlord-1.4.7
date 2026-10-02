using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000023 RID: 35
	public class HintWidget : Widget
	{
		// Token: 0x060001DD RID: 477 RVA: 0x0000727A File Offset: 0x0000547A
		public HintWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00007283 File Offset: 0x00005483
		protected override void OnConnectedToRoot()
		{
			base.ParentWidget.EventFire += this.ParentWidgetEventFired;
			base.OnConnectedToRoot();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000072A2 File Offset: 0x000054A2
		protected override void OnDisconnectedFromRoot()
		{
			base.ParentWidget.EventFire -= this.ParentWidgetEventFired;
			base.OnDisconnectedFromRoot();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000072C4 File Offset: 0x000054C4
		private void ParentWidgetEventFired(Widget widget, string eventName, object[] args)
		{
			if (base.IsVisible)
			{
				if (eventName == "HoverBegin")
				{
					base.EventFired("HoverBegin", Array.Empty<object>());
					return;
				}
				if (eventName == "HoverEnd")
				{
					base.EventFired("HoverEnd", Array.Empty<object>());
					return;
				}
				if (eventName == "DragHoverBegin")
				{
					base.EventFired("DragHoverBegin", Array.Empty<object>());
					return;
				}
				if (eventName == "DragHoverEnd")
				{
					base.EventFired("DragHoverEnd", Array.Empty<object>());
				}
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00007350 File Offset: 0x00005550
		protected override bool OnPreviewMousePressed()
		{
			return false;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00007353 File Offset: 0x00005553
		protected override bool OnPreviewDragBegin()
		{
			return false;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00007356 File Offset: 0x00005556
		protected override bool OnPreviewDrop()
		{
			return false;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00007359 File Offset: 0x00005559
		protected override bool OnPreviewMouseScroll()
		{
			return false;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000735C File Offset: 0x0000555C
		protected override bool OnPreviewMouseReleased()
		{
			return false;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000735F File Offset: 0x0000555F
		protected override bool OnPreviewMouseMove()
		{
			return true;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007362 File Offset: 0x00005562
		protected override bool OnPreviewDragHover()
		{
			return false;
		}
	}
}
