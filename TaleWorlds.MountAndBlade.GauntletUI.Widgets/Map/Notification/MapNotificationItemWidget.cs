using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Notification
{
	// Token: 0x02000121 RID: 289
	public class MapNotificationItemWidget : BrushWidget
	{
		// Token: 0x06000F40 RID: 3904 RVA: 0x0002A003 File Offset: 0x00028203
		public MapNotificationItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0002A018 File Offset: 0x00028218
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._imageDetermined)
			{
				this.NotificationRingImageWidget.RegisterBrushStatesOfWidget();
				this.NotificationRingImageWidget.SetState(this.NotificationType);
				this._imageDetermined = true;
			}
			if (!this._sizeDetermined && this.NotificationDescriptionText != null)
			{
				this.DetermineSize();
			}
			bool flag = this._ringHoverBegan || this._extensionHoverBegan || this._removeHoverBegan;
			this._isExtended = flag;
			if (this.RemoveButtonVisualWidget != null)
			{
				this.RemoveButtonVisualWidget.IsVisible = this._isExtended && base.EventManager.IsControllerActive;
			}
			this.NotificationRingWidget.IsEnabled = !this._removeInitiated;
			this.NotificationExtensionWidget.IsEnabled = !this._removeInitiated;
			this.RemoveNotificationButtonWidget.IsVisible = flag && !this.IsInspectionForced;
			this.NotificationTextContainerWidget.IsVisible = flag;
			this.RefreshHorizontalPositioning(dt, flag);
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0002A110 File Offset: 0x00028310
		private void DetermineSize()
		{
			if (this.NotificationDescriptionText.Size.Y > base.Size.Y - 45f * base._scaleToUse)
			{
				this.NotificationExtensionWidget.Sprite = this.ExtendedWidthSprite;
				this.NotificationExtensionWidget.SuggestedWidth = this.ExtendedWidth;
			}
			else
			{
				this.NotificationExtensionWidget.Sprite = this.DefaultWidthSprite;
			}
			this._sizeDetermined = true;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0002A184 File Offset: 0x00028384
		private void RefreshHorizontalPositioning(float dt, bool shouldExtend)
		{
			float num = this.NotificationExtensionWidget.Size.X - this.NotificationRingWidget.Size.X + 20f * base._scaleToUse;
			float num2 = -(this.NotificationExtensionWidget.Size.X - (this.NotificationExtensionWidget.Size.X - this.NotificationRingWidget.Size.X)) + 35f * base._scaleToUse;
			float num3 = (shouldExtend ? num2 : num);
			this.NotificationExtensionWidget.ScaledPositionXOffset = this.LocalLerp(this.NotificationExtensionWidget.ScaledPositionXOffset, num3, dt * 18f);
			float num4 = 0f;
			if (this._removeInitiated)
			{
				num4 = this.NotificationRingWidget.Size.X;
			}
			else if (!base.IsVisible)
			{
				num4 = this.NotificationRingWidget.Size.X;
			}
			base.ScaledPositionXOffset = this.LocalLerp(base.ScaledPositionXOffset, num4, dt * 18f);
			if (this._removeInitiated && MathF.Abs(base.ScaledPositionXOffset - num4) < 0.7f)
			{
				base.EventFired("OnRemove", Array.Empty<object>());
			}
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0002A2AD File Offset: 0x000284AD
		private void OnRemoveClick(Widget button)
		{
			if (!this.IsInspectionForced)
			{
				this._removeInitiated = true;
				base.EventFired("OnRemoveBegin", Array.Empty<object>());
			}
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0002A2CE File Offset: 0x000284CE
		private void OnInspectionClick(Widget button)
		{
			base.EventFired("OnInspection", Array.Empty<object>());
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x0002A2E0 File Offset: 0x000284E0
		// (set) Token: 0x06000F47 RID: 3911 RVA: 0x0002A2E8 File Offset: 0x000284E8
		[Editor(false)]
		public bool IsFocusItem
		{
			get
			{
				return this._isFocusItem;
			}
			set
			{
				if (value != this._isFocusItem)
				{
					this._isFocusItem = value;
					base.OnPropertyChanged(value, "IsFocusItem");
				}
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x0002A306 File Offset: 0x00028506
		// (set) Token: 0x06000F49 RID: 3913 RVA: 0x0002A30E File Offset: 0x0002850E
		[Editor(false)]
		public float DefaultWidth
		{
			get
			{
				return this._defaultWidth;
			}
			set
			{
				if (value != this._defaultWidth)
				{
					this._defaultWidth = value;
					base.OnPropertyChanged(value, "DefaultWidth");
				}
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0002A32C File Offset: 0x0002852C
		// (set) Token: 0x06000F4B RID: 3915 RVA: 0x0002A334 File Offset: 0x00028534
		[Editor(false)]
		public float ExtendedWidth
		{
			get
			{
				return this._extendedWidth;
			}
			set
			{
				if (value != this._extendedWidth)
				{
					this._extendedWidth = value;
					base.OnPropertyChanged(value, "ExtendedWidth");
				}
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06000F4C RID: 3916 RVA: 0x0002A352 File Offset: 0x00028552
		// (set) Token: 0x06000F4D RID: 3917 RVA: 0x0002A35C File Offset: 0x0002855C
		[Editor(false)]
		public ButtonWidget RemoveNotificationButtonWidget
		{
			get
			{
				return this._removeNotificationButtonWidget;
			}
			set
			{
				if (this._removeNotificationButtonWidget != value)
				{
					this._removeNotificationButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "RemoveNotificationButtonWidget");
					value.ClickEventHandlers.Add(new Action<Widget>(this.OnRemoveClick));
					value.boolPropertyChanged += this.RemoveButtonWidgetPropertyChanged;
				}
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06000F4E RID: 3918 RVA: 0x0002A3AE File Offset: 0x000285AE
		// (set) Token: 0x06000F4F RID: 3919 RVA: 0x0002A3B6 File Offset: 0x000285B6
		[Editor(false)]
		public Widget NotificationRingImageWidget
		{
			get
			{
				return this._notificationRingImageWidget;
			}
			set
			{
				if (this._notificationRingImageWidget != value)
				{
					this._notificationRingImageWidget = value;
					base.OnPropertyChanged<Widget>(value, "NotificationRingImageWidget");
				}
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0002A3D4 File Offset: 0x000285D4
		// (set) Token: 0x06000F51 RID: 3921 RVA: 0x0002A3DC File Offset: 0x000285DC
		[Editor(false)]
		public bool IsInspectionForced
		{
			get
			{
				return this._isInspectionForced;
			}
			set
			{
				if (this._isInspectionForced != value)
				{
					this._isInspectionForced = value;
					base.OnPropertyChanged(value, "IsInspectionForced");
				}
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06000F52 RID: 3922 RVA: 0x0002A3FA File Offset: 0x000285FA
		// (set) Token: 0x06000F53 RID: 3923 RVA: 0x0002A402 File Offset: 0x00028602
		[Editor(false)]
		public string NotificationType
		{
			get
			{
				return this._notificationType;
			}
			set
			{
				if (this._notificationType != value)
				{
					this._notificationType = value;
					base.OnPropertyChanged<string>(value, "NotificationType");
				}
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x0002A425 File Offset: 0x00028625
		// (set) Token: 0x06000F55 RID: 3925 RVA: 0x0002A42D File Offset: 0x0002862D
		[Editor(false)]
		public Sprite DefaultWidthSprite
		{
			get
			{
				return this._defaultWidthSprite;
			}
			set
			{
				if (this._defaultWidthSprite != value)
				{
					this._defaultWidthSprite = value;
					base.OnPropertyChanged<Sprite>(value, "DefaultWidthSprite");
				}
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0002A44B File Offset: 0x0002864B
		// (set) Token: 0x06000F57 RID: 3927 RVA: 0x0002A453 File Offset: 0x00028653
		[Editor(false)]
		public Sprite ExtendedWidthSprite
		{
			get
			{
				return this._extendedWidthSprite;
			}
			set
			{
				if (this._extendedWidthSprite != value)
				{
					this._extendedWidthSprite = value;
					base.OnPropertyChanged<Sprite>(value, "ExtendedWidthSprite");
				}
			}
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0002A471 File Offset: 0x00028671
		private void RemoveButtonWidgetPropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				this._removeHoverBegan = propertyValue;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0002A487 File Offset: 0x00028687
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x0002A490 File Offset: 0x00028690
		[Editor(false)]
		public Widget NotificationRingWidget
		{
			get
			{
				return this._notificationRingWidget;
			}
			set
			{
				if (this._notificationRingWidget != value)
				{
					this._notificationRingWidget = value;
					base.OnPropertyChanged<Widget>(value, "NotificationRingWidget");
					value.boolPropertyChanged += this.RingWidgetOnPropertyChanged;
					value.EventFire += this.InspectionWidgetsEventFire;
				}
			}
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0002A4DD File Offset: 0x000286DD
		private void RingWidgetOnPropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				this._ringHoverBegan = propertyValue;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0002A4F3 File Offset: 0x000286F3
		// (set) Token: 0x06000F5D RID: 3933 RVA: 0x0002A4FC File Offset: 0x000286FC
		[Editor(false)]
		public Widget NotificationExtensionWidget
		{
			get
			{
				return this._notificationExtensionWidget;
			}
			set
			{
				if (this._notificationExtensionWidget != value)
				{
					this._notificationExtensionWidget = value;
					base.OnPropertyChanged<Widget>(value, "NotificationExtensionWidget");
					value.boolPropertyChanged += this.ExtensionWidgetOnPropertyChanged;
					value.EventFire += this.InspectionWidgetsEventFire;
				}
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x0002A549 File Offset: 0x00028749
		// (set) Token: 0x06000F5F RID: 3935 RVA: 0x0002A551 File Offset: 0x00028751
		[Editor(false)]
		public Widget NotificationTextContainerWidget
		{
			get
			{
				return this._notificationTextContainerWidget;
			}
			set
			{
				if (this._notificationTextContainerWidget != value)
				{
					this._notificationTextContainerWidget = value;
					base.OnPropertyChanged<Widget>(value, "NotificationTextContainerWidget");
				}
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0002A56F File Offset: 0x0002876F
		// (set) Token: 0x06000F61 RID: 3937 RVA: 0x0002A577 File Offset: 0x00028777
		[Editor(false)]
		public RichTextWidget NotificationDescriptionText
		{
			get
			{
				return this._notificationDescriptionText;
			}
			set
			{
				if (this._notificationDescriptionText != value)
				{
					this._notificationDescriptionText = value;
					base.OnPropertyChanged<RichTextWidget>(value, "NotificationDescriptionText");
				}
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0002A595 File Offset: 0x00028795
		// (set) Token: 0x06000F63 RID: 3939 RVA: 0x0002A59D File Offset: 0x0002879D
		[Editor(false)]
		public InputKeyVisualWidget RemoveButtonVisualWidget
		{
			get
			{
				return this._removeButtonVisualWidget;
			}
			set
			{
				if (this._removeButtonVisualWidget != value)
				{
					this._removeButtonVisualWidget = value;
					base.OnPropertyChanged<InputKeyVisualWidget>(value, "RemoveButtonVisualWidget");
				}
			}
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0002A5BB File Offset: 0x000287BB
		private void InspectionWidgetsEventFire(Widget widget, string eventName, object[] eventParameters)
		{
			if (eventName == "Click")
			{
				this.OnInspectionClick(widget);
				return;
			}
			if (eventName == "AlternateClick")
			{
				this.OnRemoveClick(this);
			}
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0002A5E6 File Offset: 0x000287E6
		private void ExtensionWidgetOnPropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				this._extensionHoverBegan = propertyValue;
			}
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0002A5FC File Offset: 0x000287FC
		private float LocalLerp(float start, float end, float delta)
		{
			if (MathF.Abs(start - end) > 1E-45f)
			{
				return (end - start) * delta + start;
			}
			return end;
		}

		// Token: 0x040006EE RID: 1774
		private bool _ringHoverBegan;

		// Token: 0x040006EF RID: 1775
		private bool _extensionHoverBegan;

		// Token: 0x040006F0 RID: 1776
		private bool _removeHoverBegan;

		// Token: 0x040006F1 RID: 1777
		private bool _removeInitiated;

		// Token: 0x040006F2 RID: 1778
		private bool _imageDetermined;

		// Token: 0x040006F3 RID: 1779
		private bool _sizeDetermined;

		// Token: 0x040006F4 RID: 1780
		private bool _isExtended;

		// Token: 0x040006F5 RID: 1781
		private bool _isFocusItem;

		// Token: 0x040006F6 RID: 1782
		private float _defaultWidth;

		// Token: 0x040006F7 RID: 1783
		private float _extendedWidth;

		// Token: 0x040006F8 RID: 1784
		private bool _isInspectionForced;

		// Token: 0x040006F9 RID: 1785
		private string _notificationType = "Default";

		// Token: 0x040006FA RID: 1786
		private Sprite _defaultWidthSprite;

		// Token: 0x040006FB RID: 1787
		private Sprite _extendedWidthSprite;

		// Token: 0x040006FC RID: 1788
		private Widget _notificationRingWidget;

		// Token: 0x040006FD RID: 1789
		private Widget _notificationRingImageWidget;

		// Token: 0x040006FE RID: 1790
		private Widget _notificationExtensionWidget;

		// Token: 0x040006FF RID: 1791
		private Widget _notificationTextContainerWidget;

		// Token: 0x04000700 RID: 1792
		private ButtonWidget _removeNotificationButtonWidget;

		// Token: 0x04000701 RID: 1793
		private RichTextWidget _notificationDescriptionText;

		// Token: 0x04000702 RID: 1794
		private InputKeyVisualWidget _removeButtonVisualWidget;
	}
}
