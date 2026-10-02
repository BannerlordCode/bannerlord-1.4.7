using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets.Graph
{
	// Token: 0x0200001A RID: 26
	public class GraphLineWidget : Widget
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000075DE File Offset: 0x000057DE
		// (set) Token: 0x0600014F RID: 335 RVA: 0x000075E6 File Offset: 0x000057E6
		public string LineBrushStateName { get; set; }

		// Token: 0x06000150 RID: 336 RVA: 0x000075EF File Offset: 0x000057EF
		public GraphLineWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000075F8 File Offset: 0x000057F8
		private void OnPointContainerEventFire(Widget widget, string eventName, object[] eventArgs)
		{
			GraphLinePointWidget graphLinePointWidget;
			if (eventName == "ItemAdd" && eventArgs.Length != 0 && (graphLinePointWidget = eventArgs[0] as GraphLinePointWidget) != null)
			{
				Action<GraphLineWidget, GraphLinePointWidget> onPointAdded = this.OnPointAdded;
				if (onPointAdded == null)
				{
					return;
				}
				onPointAdded(this, graphLinePointWidget);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00007634 File Offset: 0x00005834
		// (set) Token: 0x06000153 RID: 339 RVA: 0x0000763C File Offset: 0x0000583C
		public Widget PointContainerWidget
		{
			get
			{
				return this._pointContainerWidget;
			}
			set
			{
				if (value != this._pointContainerWidget)
				{
					if (this._pointContainerWidget != null)
					{
						this._pointContainerWidget.EventFire -= this.OnPointContainerEventFire;
					}
					this._pointContainerWidget = value;
					if (this._pointContainerWidget != null)
					{
						this._pointContainerWidget.EventFire += this.OnPointContainerEventFire;
					}
					base.OnPropertyChanged<Widget>(value, "PointContainerWidget");
				}
			}
		}

		// Token: 0x040000A1 RID: 161
		public Action<GraphLineWidget, GraphLinePointWidget> OnPointAdded;

		// Token: 0x040000A2 RID: 162
		private Widget _pointContainerWidget;
	}
}
