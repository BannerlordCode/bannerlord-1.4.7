using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x0200012D RID: 301
	public class MapInfoBarWidget : Widget
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000FB7 RID: 4023 RVA: 0x0002B560 File Offset: 0x00029760
		// (remove) Token: 0x06000FB8 RID: 4024 RVA: 0x0002B598 File Offset: 0x00029798
		public event MapInfoBarWidget.MapBarExtendStateChangeEvent OnMapInfoBarExtendStateChange;

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0002B5CD File Offset: 0x000297CD
		public MapInfoBarWidget(UIContext context)
			: base(context)
		{
			base.AddState("Disabled");
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0002B5E1 File Offset: 0x000297E1
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.RefreshBarExtendState();
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0002B5F0 File Offset: 0x000297F0
		private void OnExtendButtonClick(Widget widget)
		{
			this.IsInfoBarExtended = !this.IsInfoBarExtended;
			this.RefreshBarExtendState();
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0002B608 File Offset: 0x00029808
		private void RefreshBarExtendState()
		{
			if (this.IsInfoBarExtended && base.CurrentState != "Extended")
			{
				this.SetState("Extended");
				this.RefreshVerticalVisual();
				return;
			}
			if (!this.IsInfoBarExtended && base.CurrentState != "Default")
			{
				this.SetState("Default");
				this.RefreshVerticalVisual();
			}
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0002B66C File Offset: 0x0002986C
		private void RefreshVerticalVisual()
		{
			foreach (Style style in this.ExtendButtonWidget.Brush.Styles)
			{
				for (int i = 0; i < style.LayerCount; i++)
				{
					style.GetLayer(i).VerticalFlip = this.IsInfoBarExtended;
				}
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0002B6E8 File Offset: 0x000298E8
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x0002B6F0 File Offset: 0x000298F0
		[Editor(false)]
		public ButtonWidget ExtendButtonWidget
		{
			get
			{
				return this._extendButtonWidget;
			}
			set
			{
				if (this._extendButtonWidget != value)
				{
					this._extendButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ExtendButtonWidget");
					if (!this._extendButtonWidget.ClickEventHandlers.Contains(new Action<Widget>(this.OnExtendButtonClick)))
					{
						this._extendButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnExtendButtonClick));
					}
				}
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0002B753 File Offset: 0x00029953
		// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x0002B75B File Offset: 0x0002995B
		[Editor(false)]
		public bool IsInfoBarExtended
		{
			get
			{
				return this._isInfoBarExtended;
			}
			set
			{
				if (this._isInfoBarExtended != value)
				{
					this._isInfoBarExtended = value;
					base.OnPropertyChanged(value, "IsInfoBarExtended");
					MapInfoBarWidget.MapBarExtendStateChangeEvent onMapInfoBarExtendStateChange = this.OnMapInfoBarExtendStateChange;
					if (onMapInfoBarExtendStateChange == null)
					{
						return;
					}
					onMapInfoBarExtendStateChange(this.IsInfoBarExtended);
				}
			}
		}

		// Token: 0x04000724 RID: 1828
		private ButtonWidget _extendButtonWidget;

		// Token: 0x04000725 RID: 1829
		private bool _isInfoBarExtended;

		// Token: 0x020001CB RID: 459
		// (Invoke) Token: 0x0600155B RID: 5467
		public delegate void MapBarExtendStateChangeEvent(bool newState);
	}
}
