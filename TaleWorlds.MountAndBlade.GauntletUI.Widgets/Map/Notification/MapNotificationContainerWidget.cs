using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Notification
{
	// Token: 0x02000120 RID: 288
	public class MapNotificationContainerWidget : Widget
	{
		// Token: 0x06000F31 RID: 3889 RVA: 0x00029CD8 File Offset: 0x00027ED8
		public MapNotificationContainerWidget(UIContext context)
			: base(context)
		{
			this._newChildren = new List<Widget>();
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00029CF4 File Offset: 0x00027EF4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._newChildren.Count > 0)
			{
				foreach (Widget widget in this._newChildren)
				{
					widget.PositionYOffset = this.DetermineChildTargetYOffset(widget, base.GetChildIndex(widget));
				}
				this.DetermineChildrenVisibility();
				this.DetermineMoreTextStatus();
				this.DetermineNavigationIndicies();
				this._newChildren.Clear();
			}
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				if (i < this.MaxAmountOfNotificationsToShow)
				{
					float num = this.DetermineChildTargetYOffset(child, i);
					child.PositionYOffset = this.LocalLerp(child.PositionYOffset, num, dt * 18f);
				}
			}
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00029DD0 File Offset: 0x00027FD0
		private void DetermineNavigationIndicies()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				MapNotificationItemWidget mapNotificationItemWidget = base.GetChild(i) as MapNotificationItemWidget;
				if (i < this.MaxAmountOfNotificationsToShow)
				{
					mapNotificationItemWidget.NotificationRingWidget.GamepadNavigationIndex = base.ChildCount - 1 - i;
				}
				else
				{
					mapNotificationItemWidget.NotificationRingWidget.GamepadNavigationIndex = -1;
				}
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00029E27 File Offset: 0x00028027
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			this._newChildren.Add(child);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00029E3C File Offset: 0x0002803C
		protected override void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			base.OnAfterChildRemoved(child, previousIndexOfChild);
			if (this._newChildren.Contains(child))
			{
				this._newChildren.Remove(child);
			}
			this.DetermineChildrenVisibility();
			this.DetermineMoreTextStatus();
			this.DetermineNavigationIndicies();
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00029E74 File Offset: 0x00028074
		private void DetermineChildrenVisibility()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				bool isVisible = child.IsVisible;
				child.IsVisible = i < this.MaxAmountOfNotificationsToShow;
				if (!isVisible)
				{
					child.PositionYOffset = this.DetermineChildTargetYOffset(child, i);
				}
			}
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00029EC0 File Offset: 0x000280C0
		private void DetermineMoreTextStatus()
		{
			this.MoreTextWidgetContainer.IsVisible = base.ChildCount > this.MaxAmountOfNotificationsToShow;
			if (this.MoreTextWidgetContainer.IsVisible)
			{
				this.MoreTextWidget.Text = "+" + (base.ChildCount - this.MaxAmountOfNotificationsToShow);
				this.MoreTextWidgetContainer.BrushRenderer.RestartAnimation();
				this.MoreTextWidget.BrushRenderer.RestartAnimation();
			}
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00029F3A File Offset: 0x0002813A
		private float DetermineChildTargetYOffset(Widget child, int childIndex)
		{
			if (childIndex < this.MaxAmountOfNotificationsToShow)
			{
				return -child.Size.Y * (float)childIndex * base._inverseScaleToUse;
			}
			return -child.Size.Y * (float)this.MaxAmountOfNotificationsToShow * base._inverseScaleToUse;
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06000F39 RID: 3897 RVA: 0x00029F77 File Offset: 0x00028177
		// (set) Token: 0x06000F3A RID: 3898 RVA: 0x00029F7F File Offset: 0x0002817F
		[Editor(false)]
		public BrushWidget MoreTextWidgetContainer
		{
			get
			{
				return this._moreTextWidgetContainer;
			}
			set
			{
				if (this._moreTextWidgetContainer != value)
				{
					this._moreTextWidgetContainer = value;
					base.OnPropertyChanged<BrushWidget>(value, "MoreTextWidgetContainer");
				}
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x00029F9D File Offset: 0x0002819D
		// (set) Token: 0x06000F3C RID: 3900 RVA: 0x00029FA5 File Offset: 0x000281A5
		[Editor(false)]
		public TextWidget MoreTextWidget
		{
			get
			{
				return this._moreTextWidget;
			}
			set
			{
				if (this._moreTextWidget != value)
				{
					this._moreTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "MoreTextWidget");
				}
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x00029FC3 File Offset: 0x000281C3
		// (set) Token: 0x06000F3E RID: 3902 RVA: 0x00029FCB File Offset: 0x000281CB
		[Editor(false)]
		public int MaxAmountOfNotificationsToShow
		{
			get
			{
				return this._maxAmountOfNotificationsToShow;
			}
			set
			{
				if (this._maxAmountOfNotificationsToShow != value)
				{
					this._maxAmountOfNotificationsToShow = value;
					base.OnPropertyChanged(value, "MaxAmountOfNotificationsToShow");
				}
			}
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00029FE9 File Offset: 0x000281E9
		private float LocalLerp(float start, float end, float delta)
		{
			if (MathF.Abs(start - end) > 1E-45f)
			{
				return (end - start) * delta + start;
			}
			return end;
		}

		// Token: 0x040006EA RID: 1770
		private List<Widget> _newChildren;

		// Token: 0x040006EB RID: 1771
		private TextWidget _moreTextWidget;

		// Token: 0x040006EC RID: 1772
		private BrushWidget _moreTextWidgetContainer;

		// Token: 0x040006ED RID: 1773
		private int _maxAmountOfNotificationsToShow = 5;
	}
}
