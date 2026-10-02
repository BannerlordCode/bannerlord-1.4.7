using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.KillFeed
{
	// Token: 0x020000C1 RID: 193
	public class MultiplayerPersonalKillFeedItemWidget : Widget
	{
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0001BF38 File Offset: 0x0001A138
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x0001BF40 File Offset: 0x0001A140
		public Widget NotificationTypeIconWidget { get; set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0001BF49 File Offset: 0x0001A149
		// (set) Token: 0x06000A04 RID: 2564 RVA: 0x0001BF51 File Offset: 0x0001A151
		public Widget NotificationBackgroundWidget { get; set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0001BF5A File Offset: 0x0001A15A
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x0001BF62 File Offset: 0x0001A162
		public TextWidget AmountTextWidget { get; set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0001BF6B File Offset: 0x0001A16B
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x0001BF73 File Offset: 0x0001A173
		public RichTextWidget MessageTextWidget { get; set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0001BF7C File Offset: 0x0001A17C
		// (set) Token: 0x06000A0A RID: 2570 RVA: 0x0001BF84 File Offset: 0x0001A184
		public float FadeInTime { get; set; } = 1f;

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0001BF8D File Offset: 0x0001A18D
		// (set) Token: 0x06000A0C RID: 2572 RVA: 0x0001BF95 File Offset: 0x0001A195
		public float StayTime { get; set; } = 3f;

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0001BF9E File Offset: 0x0001A19E
		// (set) Token: 0x06000A0E RID: 2574 RVA: 0x0001BFA6 File Offset: 0x0001A1A6
		public float FadeOutTime { get; set; } = 0.5f;

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0001BFAF File Offset: 0x0001A1AF
		private float CurrentAlpha
		{
			get
			{
				return base.AlphaFactor;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0001BFB7 File Offset: 0x0001A1B7
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x0001BFBF File Offset: 0x0001A1BF
		public float TimeSinceCreation { get; private set; }

		// Token: 0x06000A12 RID: 2578 RVA: 0x0001BFC8 File Offset: 0x0001A1C8
		public MultiplayerPersonalKillFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0001C028 File Offset: 0x0001A228
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				base.PositionYOffset = 0f;
			}
			if (!this._initialized)
			{
				this.SetGlobalAlphaRecursively(0f);
				this.UpdateNotificationBackgroundWidget();
				this.UpdateNotificationTypeIconWidget();
				this.UpdateNotificationMessageWidget();
				this.UpdateNotificationAmountWidget();
				this.DetermineSoundEvent();
				this._initialized = true;
			}
			this.UpdateAlphaValues(dt);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0001C08E File Offset: 0x0001A28E
		private void DetermineSoundEvent()
		{
			if (this.ItemType == 6)
			{
				base.Context.TwoDimensionContext.PlaySound(this._goldGainedSound);
			}
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0001C0B0 File Offset: 0x0001A2B0
		private void UpdateAlphaValues(float dt)
		{
			float num = 0f;
			float num2 = 0f;
			this.TimeSinceCreation += dt;
			if (this._maxTargetAlpha == 0f)
			{
				base.EventFired("OnRemove", Array.Empty<object>());
				return;
			}
			if (this.TimeSinceCreation <= this.FadeInTime)
			{
				num = MathF.Min(1f, this._maxTargetAlpha);
				num2 = this.TimeSinceCreation / this.FadeInTime;
			}
			else if (this.TimeSinceCreation - this.FadeInTime <= this.StayTime)
			{
				num = MathF.Min(1f, this._maxTargetAlpha);
				num2 = 1f;
			}
			else if (this.TimeSinceCreation - (this.FadeInTime + this.StayTime) <= this.FadeOutTime)
			{
				num = 0f;
				num2 = (this.TimeSinceCreation - (this.FadeInTime + this.StayTime)) / this.FadeOutTime;
				if (this.CurrentAlpha <= 0.1f)
				{
					base.EventFired("OnRemove", Array.Empty<object>());
				}
			}
			else
			{
				base.EventFired("OnRemove", Array.Empty<object>());
			}
			this.SetGlobalAlphaRecursively(Mathf.Lerp(this.CurrentAlpha, num, num2));
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0001C1D5 File Offset: 0x0001A3D5
		public void SetSpeedModifier(float newSpeed)
		{
			if (newSpeed > this._speedModifier)
			{
				this._speedModifier = newSpeed;
			}
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0001C1E7 File Offset: 0x0001A3E7
		public void SetMaxAlphaValue(float newMaxAlpha)
		{
			if (newMaxAlpha < this._maxTargetAlpha)
			{
				this._maxTargetAlpha = newMaxAlpha;
			}
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0001C1FC File Offset: 0x0001A3FC
		private void UpdateNotificationTypeIconWidget()
		{
			if (this.ItemType == 0)
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
				this.NotificationTypeIconWidget.SetState("GoldChange");
				return;
			case 7:
				this.NotificationTypeIconWidget.SetState("NormalKillHeadshot");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Multiplayer\\KillFeed\\MultiplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationTypeIconWidget", 172);
				this.NotificationTypeIconWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0001C2E8 File Offset: 0x0001A4E8
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
			case 7:
				this.MessageTextWidget.SetState("Normal");
				return;
			case 1:
			case 2:
				this.MessageTextWidget.SetState("FriendlyFire");
				return;
			case 6:
				if (this.Amount >= 0)
				{
					this.MessageTextWidget.SetState("GoldChangePositive");
					return;
				}
				this.MessageTextWidget.SetState("GoldChangeNegative");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Multiplayer\\KillFeed\\MultiplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationMessageWidget", 213);
				this.MessageTextWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0001C3C4 File Offset: 0x0001A5C4
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
			case 7:
				this.AmountTextWidget.SetState("Normal");
				this.AmountTextWidget.IntText = this.Amount;
				return;
			case 1:
			case 2:
				this.AmountTextWidget.SetState("FriendlyFire");
				this.AmountTextWidget.IntText = this.Amount;
				return;
			case 5:
				this.AmountTextWidget.IsVisible = false;
				return;
			case 6:
				if (this.Amount >= 0)
				{
					this.AmountTextWidget.SetState("GoldChangePositive");
					this.AmountTextWidget.Text = "+" + this.Amount.ToString();
					return;
				}
				this.AmountTextWidget.SetState("GoldChangeNegative");
				this.AmountTextWidget.Text = this.Amount.ToString();
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Multiplayer\\KillFeed\\MultiplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationAmountWidget", 259);
				this.AmountTextWidget.IsVisible = false;
				return;
			}
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0001C500 File Offset: 0x0001A700
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
			case 7:
				this.NotificationBackgroundWidget.SetState("Normal");
				return;
			case 5:
				break;
			case 6:
				if (this.Amount >= 0)
				{
					this.NotificationBackgroundWidget.SetState("GoldChangePositive");
					return;
				}
				this.NotificationBackgroundWidget.SetState("GoldChangeNegative");
				return;
			default:
				Debug.FailedAssert("Undefined personal feed notification type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Multiplayer\\KillFeed\\MultiplayerPersonalKillFeedItemWidget.cs", "UpdateNotificationBackgroundWidget", 295);
				this.NotificationBackgroundWidget.SetState("Hidden");
				break;
			}
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0001C5C4 File Offset: 0x0001A7C4
		private float GetInitialVerticalPositionOfSelf()
		{
			float num = 0f;
			for (int i = 0; i < base.GetSiblingIndex(); i++)
			{
				num += base.ParentWidget.GetChild(i).Size.Y * base._inverseScaleToUse;
			}
			return num;
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x0001C609 File Offset: 0x0001A809
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x0001C611 File Offset: 0x0001A811
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

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x0001C62F File Offset: 0x0001A82F
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x0001C637 File Offset: 0x0001A837
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

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x0001C65A File Offset: 0x0001A85A
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x0001C662 File Offset: 0x0001A862
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

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x0001C680 File Offset: 0x0001A880
		// (set) Token: 0x06000A24 RID: 2596 RVA: 0x0001C688 File Offset: 0x0001A888
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

		// Token: 0x04000490 RID: 1168
		private float _speedModifier = 1f;

		// Token: 0x04000492 RID: 1170
		private float _maxTargetAlpha = 1f;

		// Token: 0x04000493 RID: 1171
		private bool _initialized;

		// Token: 0x04000494 RID: 1172
		private string _goldGainedSound = "multiplayer/coin_add";

		// Token: 0x04000495 RID: 1173
		private bool _isDamage;

		// Token: 0x04000496 RID: 1174
		private int _itemType;

		// Token: 0x04000497 RID: 1175
		private int _amount = -1;

		// Token: 0x04000498 RID: 1176
		private string _message;
	}
}
