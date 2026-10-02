using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001A RID: 26
	public class EncyclopediaTroopScrollablePanel : ScrollablePanel
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00005D14 File Offset: 0x00003F14
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00005D1C File Offset: 0x00003F1C
		public bool PanWithMouseEnabled { get; set; }

		// Token: 0x0600015E RID: 350 RVA: 0x00005D25 File Offset: 0x00003F25
		public EncyclopediaTroopScrollablePanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005D30 File Offset: 0x00003F30
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.PanWithMouseEnabled)
			{
				bool flag = this.IsMouseOverWidget(this);
				if (flag)
				{
					List<Widget> allChildrenAndThisRecursive = base.GetAllChildrenAndThisRecursive();
					for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
					{
						if (this.IsMouseOverWidget(allChildrenAndThisRecursive[i]) && allChildrenAndThisRecursive[i] is ButtonWidget)
						{
							flag = false;
						}
					}
				}
				if (flag && base.HorizontalScrollbar != null && this._canScrollHorizontal)
				{
					base.SetActiveCursor(UIContext.MouseCursors.Move);
					if (Input.IsKeyPressed(InputKey.LeftMouseButton))
					{
						this._isDragging = true;
					}
				}
			}
			if (Input.IsKeyReleased(InputKey.LeftMouseButton))
			{
				this._isDragging = false;
			}
			if (this._isDragging)
			{
				base.HorizontalScrollbar.ValueFloat -= Input.MouseMoveX;
				base.VerticalScrollbar.ValueFloat -= Input.MouseMoveY;
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005E04 File Offset: 0x00004004
		private bool IsMouseOverWidget(Widget widget)
		{
			return widget.GlobalPosition.X <= Input.MousePositionPixel.X && Input.MousePositionPixel.X <= widget.GlobalPosition.X + widget.Size.X && widget.GlobalPosition.Y <= Input.MousePositionPixel.Y && Input.MousePositionPixel.Y <= widget.GlobalPosition.Y + widget.Size.Y;
		}

		// Token: 0x040000A3 RID: 163
		private bool _isDragging;
	}
}
