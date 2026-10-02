using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000044 RID: 68
	public class ToggleButtonWidget : ButtonWidget
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x0000C2BC File Offset: 0x0000A4BC
		public ToggleButtonWidget(UIContext context)
			: base(context)
		{
			this.ClickEventHandlers.Add(new Action<Widget>(this.OnClick));
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000C2DD File Offset: 0x0000A4DD
		protected virtual void OnClick(Widget widget)
		{
			if (this._widgetToClose != null)
			{
				this.IsTargetVisible = !this._widgetToClose.IsVisible;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0000C2FB File Offset: 0x0000A4FB
		// (set) Token: 0x060003DA RID: 986 RVA: 0x0000C30E File Offset: 0x0000A50E
		public bool IsTargetVisible
		{
			get
			{
				Widget widgetToClose = this._widgetToClose;
				return widgetToClose != null && widgetToClose.IsVisible;
			}
			set
			{
				if (this._widgetToClose != null && this._widgetToClose.IsVisible != value)
				{
					this._widgetToClose.IsVisible = value;
					base.OnPropertyChanged(value, "IsTargetVisible");
				}
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060003DB RID: 987 RVA: 0x0000C33E File Offset: 0x0000A53E
		// (set) Token: 0x060003DC RID: 988 RVA: 0x0000C346 File Offset: 0x0000A546
		[Editor(false)]
		public Widget WidgetToClose
		{
			get
			{
				return this._widgetToClose;
			}
			set
			{
				if (this._widgetToClose != value)
				{
					this._widgetToClose = value;
					base.OnPropertyChanged<Widget>(value, "WidgetToClose");
				}
			}
		}

		// Token: 0x0400019C RID: 412
		private Widget _widgetToClose;
	}
}
