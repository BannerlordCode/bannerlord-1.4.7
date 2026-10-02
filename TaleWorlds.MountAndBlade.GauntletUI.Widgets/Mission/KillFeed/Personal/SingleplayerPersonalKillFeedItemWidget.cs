using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.KillFeed.Personal
{
	// Token: 0x020000FB RID: 251
	public class SingleplayerPersonalKillFeedItemWidget : Widget
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x0002451C File Offset: 0x0002271C
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x00024524 File Offset: 0x00022724
		public Widget NotificationTypeIconWidget { get; set; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x0002452D File Offset: 0x0002272D
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x00024535 File Offset: 0x00022735
		public Widget NotificationBackgroundWidget { get; set; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000D47 RID: 3399 RVA: 0x0002453E File Offset: 0x0002273E
		// (set) Token: 0x06000D48 RID: 3400 RVA: 0x00024546 File Offset: 0x00022746
		public TextWidget AmountTextWidget { get; set; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x0002454F File Offset: 0x0002274F
		// (set) Token: 0x06000D4A RID: 3402 RVA: 0x00024557 File Offset: 0x00022757
		public RichTextWidget MessageTextWidget { get; set; }

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00024560 File Offset: 0x00022760
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x00024568 File Offset: 0x00022768
		public float FadeInTime { get; set; } = 0.2f;

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x00024571 File Offset: 0x00022771
		// (set) Token: 0x06000D4E RID: 3406 RVA: 0x00024579 File Offset: 0x00022779
		public float StayTime { get; set; } = 2f;

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00024582 File Offset: 0x00022782
		// (set) Token: 0x06000D50 RID: 3408 RVA: 0x0002458A File Offset: 0x0002278A
		public float FadeOutTime { get; set; } = 0.2f;

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00024593 File Offset: 0x00022793
		// (set) Token: 0x06000D52 RID: 3410 RVA: 0x0002459B File Offset: 0x0002279B
		public float TimeSinceCreation { get; private set; }

		// Token: 0x06000D53 RID: 3411 RVA: 0x000245A4 File Offset: 0x000227A4
		public SingleplayerPersonalKillFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000245D0 File Offset: 0x000227D0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				this.SetGlobalAlphaRecursively(0f);
				this.UpdateNotificationBackgroundWidget();
				this.UpdateNotificationTypeIconWidget();
				this.UpdateNotificationMessageWidget();
				this.UpdateNotificationAmountWidget();
				this.UpdateTroopTypeVisualWidget();
				this._initialized = true;
			}
			this.UpdateAlphaValues(dt);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00024624 File Offset: 0x00022824
		private void UpdateAlphaValues(float dt)
		{
			if (!this.IsPaused)
			{
				this.TimeSinceCreation += dt * this._speedModifier;
			}
			if (this.TimeSinceCreation <= this.FadeInTime)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 1f, this.TimeSinceCreation / this.FadeInTime));
				return;
			}
			if (this.TimeSinceCreation - this.FadeInTime <= this.StayTime)
			{
				this.SetGlobalAlphaRecursively(1f);
				return;
			}
			if (this.TimeSinceCreation - (this.FadeInTime + this.StayTime) <= this.FadeOutTime)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 0f, (this.TimeSinceCreation - (this.FadeInTime + this.StayTime)) / this.FadeOutTime));
				if (base.AlphaFactor <= 0.1f)
				{
					base.EventFired("OnRemove", Array.Empty<object>());
					return;
				}
			}
			else
			{
				base.EventFired("OnRemove", Array.Empty<object>());
			}
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0002471C File Offset: 0x0002291C
		public void SetSpeedModifier(float newSpeed)
		{
			if (newSpeed > this._speedModifier)
			{
				this._speedModifier = newSpeed;
			}
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00024730 File Offset: 0x00022930
		private void UpdateNotificationTypeIconWidget()
		{
			if (this.ItemType == 0 || this.ItemType == 9)
			{
				this.NotificationTypeIconWidget.IsVisible = false;
				return;
			}
			switch (this.ItemType)
			{
			case 1:
				this.NotificationTypeIconWidget.SetState("FriendlyFireDamage");
				return;
			case 2:
				this.NotificationTypeIconWidget.SetState("FriendlyFireKill");
				return;
			case 3:
				this.NotificationTypeIconWidget.SetState("MountDamage");
				return;
			case 4:
				this.NotificationTypeIconWidget.SetState("NormalKill");
				return;
			case 5:
				this.NotificationTypeIconWidget.SetState("Assist");
				return;
			case 6:
				this.NotificationTypeIconWidget.SetState("MakeUnconscious");
				return;
			case 7:
				this.NotificationTypeIconWidget.SetState("NormalKillHeadshot");
				return;
			case 8:
				this.NotificationTypeIconWidget.SetState("MakeUnconsciousHeadshot");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Mission\\KillFeed\\Personal\\SingleplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationTypeIconWidget", 128);
				this.NotificationTypeIconWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00024840 File Offset: 0x00022A40
		private void UpdateNotificationAmountWidget()
		{
			if (this.ItemType != 6 && this.Amount == -1)
			{
				this.AmountTextWidget.IsVisible = false;
				return;
			}
			switch (this.ItemType)
			{
			case 0:
			case 3:
			case 4:
			case 6:
			case 7:
			case 8:
				this.AmountTextWidget.SetState("Normal");
				this.AmountTextWidget.IntText = this.Amount;
				return;
			case 1:
			case 2:
				this.AmountTextWidget.SetState("FriendlyFire");
				this.AmountTextWidget.IntText = this.Amount;
				return;
			case 5:
			case 9:
				this.AmountTextWidget.IsVisible = false;
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Mission\\KillFeed\\Personal\\SingleplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationAmountWidget", 166);
				this.AmountTextWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0002491C File Offset: 0x00022B1C
		private void UpdateNotificationMessageWidget()
		{
			this.MessageTextWidget.Text = this.Message;
			if (string.IsNullOrEmpty(this.Message))
			{
				this.MessageTextWidget.IsVisible = false;
				return;
			}
			switch (this.ItemType)
			{
			case 0:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				this.MessageTextWidget.SetState("Normal");
				return;
			case 1:
			case 2:
				this.MessageTextWidget.SetState("FriendlyFire");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Mission\\KillFeed\\Personal\\SingleplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationMessageWidget", 200);
				this.MessageTextWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x000249D4 File Offset: 0x00022BD4
		private void UpdateNotificationBackgroundWidget()
		{
			switch (this.ItemType)
			{
			case 0:
			case 1:
			case 3:
				this.NotificationBackgroundWidget.SetState("Hidden");
				return;
			case 2:
				this.NotificationBackgroundWidget.SetState("FriendlyFire");
				return;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				this.NotificationBackgroundWidget.SetState("Normal");
				return;
			case 9:
				this.NotificationBackgroundWidget.SetState("Message");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Mission\\KillFeed\\Personal\\SingleplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationBackgroundWidget", 230);
				this.NotificationBackgroundWidget.SetState("Hidden");
				return;
			}
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00024A88 File Offset: 0x00022C88
		private void UpdateTroopTypeVisualWidget()
		{
			if (this.TroopTypeWidget != null)
			{
				if (string.IsNullOrEmpty(this.TypeID))
				{
					this.TroopTypeWidget.IsVisible = false;
					return;
				}
				Widget troopTypeWidget = this.TroopTypeWidget;
				Brush troopTypeIconBrush = this.TroopTypeIconBrush;
				Sprite sprite;
				if (troopTypeIconBrush == null)
				{
					sprite = null;
				}
				else
				{
					BrushLayer layer = troopTypeIconBrush.GetLayer(this._typeID);
					sprite = ((layer != null) ? layer.Sprite : null);
				}
				troopTypeWidget.Sprite = sprite;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00024AE6 File Offset: 0x00022CE6
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x00024AEE File Offset: 0x00022CEE
		public bool IsDamage
		{
			get
			{
				return this._isDamage;
			}
			set
			{
				if (value != this._isDamage)
				{
					this._isDamage = value;
					base.OnPropertyChanged(value, "IsDamage");
				}
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00024B0C File Offset: 0x00022D0C
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x00024B14 File Offset: 0x00022D14
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				if (value != this._amount)
				{
					this._amount = value;
					base.OnPropertyChanged(value, "Amount");
				}
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00024B32 File Offset: 0x00022D32
		// (set) Token: 0x06000D61 RID: 3425 RVA: 0x00024B3A File Offset: 0x00022D3A
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChanged(value, "ItemType");
				}
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00024B58 File Offset: 0x00022D58
		// (set) Token: 0x06000D63 RID: 3427 RVA: 0x00024B60 File Offset: 0x00022D60
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChanged<string>(value, "Message");
				}
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00024B83 File Offset: 0x00022D83
		// (set) Token: 0x06000D65 RID: 3429 RVA: 0x00024B8B File Offset: 0x00022D8B
		public string TypeID
		{
			get
			{
				return this._typeID;
			}
			set
			{
				if (value != this._typeID)
				{
					this._typeID = value;
					base.OnPropertyChanged<string>(value, "TypeID");
				}
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00024BAE File Offset: 0x00022DAE
		// (set) Token: 0x06000D67 RID: 3431 RVA: 0x00024BB6 File Offset: 0x00022DB6
		public Brush TroopTypeIconBrush
		{
			get
			{
				return this._troopTypeIconBrush;
			}
			set
			{
				if (value != this._troopTypeIconBrush)
				{
					this._troopTypeIconBrush = value;
				}
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x00024BC8 File Offset: 0x00022DC8
		// (set) Token: 0x06000D69 RID: 3433 RVA: 0x00024BD0 File Offset: 0x00022DD0
		public Widget TroopTypeWidget
		{
			get
			{
				return this._troopTypeWidget;
			}
			set
			{
				if (value != this._troopTypeWidget)
				{
					this._troopTypeWidget = value;
					if (!string.IsNullOrEmpty(this._typeID))
					{
						Widget troopTypeWidget = this._troopTypeWidget;
						Brush troopTypeIconBrush = this.TroopTypeIconBrush;
						Sprite sprite;
						if (troopTypeIconBrush == null)
						{
							sprite = null;
						}
						else
						{
							BrushLayer layer = troopTypeIconBrush.GetLayer(this._typeID);
							sprite = ((layer != null) ? layer.Sprite : null);
						}
						troopTypeWidget.Sprite = sprite;
					}
				}
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00024C29 File Offset: 0x00022E29
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x00024C31 File Offset: 0x00022E31
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
			set
			{
				if (value != this._isPaused)
				{
					this._isPaused = value;
					base.OnPropertyChanged(value, "IsPaused");
				}
			}
		}

		// Token: 0x04000608 RID: 1544
		private bool _initialized;

		// Token: 0x04000609 RID: 1545
		private float _speedModifier;

		// Token: 0x0400060A RID: 1546
		private bool _isDamage;

		// Token: 0x0400060B RID: 1547
		private int _amount;

		// Token: 0x0400060C RID: 1548
		private int _itemType;

		// Token: 0x0400060D RID: 1549
		private string _message;

		// Token: 0x0400060E RID: 1550
		private string _typeID;

		// Token: 0x0400060F RID: 1551
		private Brush _troopTypeIconBrush;

		// Token: 0x04000610 RID: 1552
		private Widget _troopTypeWidget;

		// Token: 0x04000611 RID: 1553
		private bool _isPaused;
	}
}
