using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x0200012B RID: 299
	public class MapBarUnreadBrushWidget : BrushWidget
	{
		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0002B316 File Offset: 0x00029516
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x0002B31E File Offset: 0x0002951E
		public bool IsBannerNotification { get; set; }

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0002B327 File Offset: 0x00029527
		public MapBarUnreadBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0002B330 File Offset: 0x00029530
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible && this._animState == MapBarUnreadBrushWidget.AnimState.Idle)
			{
				this._animState = MapBarUnreadBrushWidget.AnimState.Start;
			}
			if (this._animState == MapBarUnreadBrushWidget.AnimState.Start)
			{
				this._animState = MapBarUnreadBrushWidget.AnimState.FirstFrame;
			}
			else if (this._animState == MapBarUnreadBrushWidget.AnimState.FirstFrame)
			{
				if (base.BrushRenderer.Brush == null)
				{
					this._animState = MapBarUnreadBrushWidget.AnimState.Start;
				}
				else
				{
					this._animState = MapBarUnreadBrushWidget.AnimState.Playing;
					base.BrushRenderer.RestartAnimation();
				}
			}
			if (this.IsBannerNotification && base.IsVisible && this._animState == MapBarUnreadBrushWidget.AnimState.Idle)
			{
				this._animState = MapBarUnreadBrushWidget.AnimState.Start;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x0002B3BD File Offset: 0x000295BD
		// (set) Token: 0x06000FAB RID: 4011 RVA: 0x0002B3C5 File Offset: 0x000295C5
		[Editor(false)]
		public TextWidget UnreadTextWidget
		{
			get
			{
				return this._unreadTextWidget;
			}
			set
			{
				if (this._unreadTextWidget != value)
				{
					this._unreadTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "UnreadTextWidget");
					if (value != null)
					{
						value.boolPropertyChanged += this.UnreadTextWidgetOnPropertyChanged;
					}
				}
			}
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0002B3F8 File Offset: 0x000295F8
		private void UnreadTextWidgetOnPropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsVisible")
			{
				base.IsVisible = propertyValue;
				this._animState = (base.IsVisible ? MapBarUnreadBrushWidget.AnimState.Start : MapBarUnreadBrushWidget.AnimState.Idle);
			}
		}

		// Token: 0x0400071D RID: 1821
		private MapBarUnreadBrushWidget.AnimState _animState;

		// Token: 0x0400071E RID: 1822
		private TextWidget _unreadTextWidget;

		// Token: 0x020001CA RID: 458
		public enum AnimState
		{
			// Token: 0x04000A2E RID: 2606
			Idle,
			// Token: 0x04000A2F RID: 2607
			Start,
			// Token: 0x04000A30 RID: 2608
			FirstFrame,
			// Token: 0x04000A31 RID: 2609
			Playing
		}
	}
}
