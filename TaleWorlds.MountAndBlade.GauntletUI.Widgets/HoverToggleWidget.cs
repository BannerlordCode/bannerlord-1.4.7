using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000024 RID: 36
	public class HoverToggleWidget : Widget
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00007365 File Offset: 0x00005565
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x0000736D File Offset: 0x0000556D
		public bool IsOverWidget { get; private set; }

		// Token: 0x060001EA RID: 490 RVA: 0x00007376 File Offset: 0x00005576
		public HoverToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00007380 File Offset: 0x00005580
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.IsVisible)
			{
				this.IsOverWidget = base.IsPointInsideMeasuredArea(base.EventManager.MousePosition);
				if (this.IsOverWidget && !this._hoverBegan)
				{
					base.EventFired("HoverBegin", Array.Empty<object>());
					this._hoverBegan = true;
				}
				else if (!this.IsOverWidget && this._hoverBegan)
				{
					base.EventFired("HoverEnd", Array.Empty<object>());
					this._hoverBegan = false;
				}
				if (this.WidgetToShow != null)
				{
					this.WidgetToShow.IsVisible = this._hoverBegan;
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000741F File Offset: 0x0000561F
		// (set) Token: 0x060001ED RID: 493 RVA: 0x00007427 File Offset: 0x00005627
		[Editor(false)]
		public Widget WidgetToShow
		{
			get
			{
				return this._widgetToShow;
			}
			set
			{
				if (this._widgetToShow != value)
				{
					this._widgetToShow = value;
					base.OnPropertyChanged<Widget>(value, "WidgetToShow");
				}
			}
		}

		// Token: 0x040000E9 RID: 233
		private bool _hoverBegan;

		// Token: 0x040000EA RID: 234
		private Widget _widgetToShow;
	}
}
