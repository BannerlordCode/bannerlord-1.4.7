using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information
{
	// Token: 0x02000145 RID: 325
	public class GameNotificationWidget : BrushWidget
	{
		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x0002F2E6 File Offset: 0x0002D4E6
		// (set) Token: 0x06001124 RID: 4388 RVA: 0x0002F2EE File Offset: 0x0002D4EE
		public float RampUpInSeconds { get; set; } = 0.2f;

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x0002F2F7 File Offset: 0x0002D4F7
		// (set) Token: 0x06001126 RID: 4390 RVA: 0x0002F2FF File Offset: 0x0002D4FF
		public float RampDownInSeconds { get; set; } = 0.2f;

		// Token: 0x06001127 RID: 4391 RVA: 0x0002F308 File Offset: 0x0002D508
		public GameNotificationWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0002F330 File Offset: 0x0002D530
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible && this._textWidgetAlignmentDirty)
			{
				ImageIdentifierWidget announcerImageIdentifier = this.AnnouncerImageIdentifier;
				if (announcerImageIdentifier != null && announcerImageIdentifier.IsVisible)
				{
					this.TextWidget.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Left;
				}
				else
				{
					this.TextWidget.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Center;
				}
				this._textWidgetAlignmentDirty = false;
			}
			if (base.IsVisible && !this.IsPaused)
			{
				this._notificationElapsedTimeInSeconds += dt;
				if (this.MustFadeOutCurrentNotification)
				{
					this._notificationElapsedTimeInSeconds = this.RampUpInSeconds + this.NotificationDurationInSeconds - this.NotificationFadeOutDelayInSeconds;
					this.MustFadeOutCurrentNotification = false;
					this.NotificationFadeOutDelayInSeconds = 0f;
				}
				if (this._notificationElapsedTimeInSeconds <= this.RampUpInSeconds)
				{
					float num = Mathf.Lerp(0f, 1f, this._notificationElapsedTimeInSeconds / this.RampUpInSeconds);
					this.SetGlobalAlphaRecursively(num);
					return;
				}
				if (this._notificationElapsedTimeInSeconds <= this.RampUpInSeconds + this.NotificationDurationInSeconds)
				{
					this.SetGlobalAlphaRecursively(1f);
					return;
				}
				if (this._notificationElapsedTimeInSeconds < this.RampUpInSeconds + this.NotificationDurationInSeconds + this.RampDownInSeconds)
				{
					float num2 = Mathf.Lerp(1f, 0f, (this._notificationElapsedTimeInSeconds - this.RampUpInSeconds - this.NotificationDurationInSeconds) / this.RampDownInSeconds);
					this.SetGlobalAlphaRecursively(num2);
					return;
				}
				this.SetGlobalAlphaRecursively(0f);
				base.EventFired("NotificationFinished", Array.Empty<object>());
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001129 RID: 4393 RVA: 0x0002F4A9 File Offset: 0x0002D6A9
		// (set) Token: 0x0600112A RID: 4394 RVA: 0x0002F4B1 File Offset: 0x0002D6B1
		public ImageIdentifierWidget AnnouncerImageIdentifier
		{
			get
			{
				return this._announcerImageIdentifier;
			}
			set
			{
				if (this._announcerImageIdentifier != value)
				{
					this._announcerImageIdentifier = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "AnnouncerImageIdentifier");
				}
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x0600112B RID: 4395 RVA: 0x0002F4CF File Offset: 0x0002D6CF
		// (set) Token: 0x0600112C RID: 4396 RVA: 0x0002F4D7 File Offset: 0x0002D6D7
		public int NotificationId
		{
			get
			{
				return this._notificationId;
			}
			set
			{
				if (this._notificationId != value)
				{
					this._notificationId = value;
					base.OnPropertyChanged(value, "NotificationId");
					this._textWidgetAlignmentDirty = true;
					this._notificationElapsedTimeInSeconds = 0f;
				}
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x0002F507 File Offset: 0x0002D707
		// (set) Token: 0x0600112E RID: 4398 RVA: 0x0002F50F File Offset: 0x0002D70F
		public float NotificationDurationInSeconds
		{
			get
			{
				return this._notificationDurationInSeconds;
			}
			set
			{
				if (this._notificationDurationInSeconds != value)
				{
					this._notificationDurationInSeconds = value;
				}
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x0600112F RID: 4399 RVA: 0x0002F521 File Offset: 0x0002D721
		// (set) Token: 0x06001130 RID: 4400 RVA: 0x0002F529 File Offset: 0x0002D729
		public RichTextWidget TextWidget
		{
			get
			{
				return this._textWidget;
			}
			set
			{
				if (this._textWidget != value)
				{
					this._textWidget = value;
					base.OnPropertyChanged<RichTextWidget>(value, "TextWidget");
				}
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x0002F547 File Offset: 0x0002D747
		// (set) Token: 0x06001132 RID: 4402 RVA: 0x0002F54F File Offset: 0x0002D74F
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
			set
			{
				if (this._isPaused != value)
				{
					this._isPaused = value;
					base.OnPropertyChanged(value, "IsPaused");
				}
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x0002F56D File Offset: 0x0002D76D
		// (set) Token: 0x06001134 RID: 4404 RVA: 0x0002F575 File Offset: 0x0002D775
		public bool MustFadeOutCurrentNotification
		{
			get
			{
				return this._mustFadeOutCurrentNotification;
			}
			set
			{
				if (this._mustFadeOutCurrentNotification != value)
				{
					this._mustFadeOutCurrentNotification = value;
					base.OnPropertyChanged(value, "MustFadeOutCurrentNotification");
				}
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x0002F593 File Offset: 0x0002D793
		// (set) Token: 0x06001136 RID: 4406 RVA: 0x0002F59B File Offset: 0x0002D79B
		public float NotificationFadeOutDelayInSeconds
		{
			get
			{
				return this._notificationFadeOutDelayInSeconds;
			}
			set
			{
				if (this._notificationFadeOutDelayInSeconds != value)
				{
					this._notificationFadeOutDelayInSeconds = value;
					base.OnPropertyChanged(value, "NotificationFadeOutDelayInSeconds");
				}
			}
		}

		// Token: 0x040007C4 RID: 1988
		private bool _textWidgetAlignmentDirty = true;

		// Token: 0x040007C5 RID: 1989
		private float _notificationElapsedTimeInSeconds;

		// Token: 0x040007C6 RID: 1990
		private int _notificationId;

		// Token: 0x040007C7 RID: 1991
		private RichTextWidget _textWidget;

		// Token: 0x040007C8 RID: 1992
		private ImageIdentifierWidget _announcerImageIdentifier;

		// Token: 0x040007C9 RID: 1993
		private float _notificationDurationInSeconds;

		// Token: 0x040007CA RID: 1994
		private bool _isPaused;

		// Token: 0x040007CB RID: 1995
		private bool _mustFadeOutCurrentNotification;

		// Token: 0x040007CC RID: 1996
		private float _notificationFadeOutDelayInSeconds;
	}
}
