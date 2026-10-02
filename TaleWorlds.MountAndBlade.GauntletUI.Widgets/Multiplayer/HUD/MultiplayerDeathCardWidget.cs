using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C7 RID: 199
	public class MultiplayerDeathCardWidget : Widget
	{
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0001D4AB File Offset: 0x0001B6AB
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x0001D4B3 File Offset: 0x0001B6B3
		public TextWidget WeaponTextWidget { get; set; }

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0001D4BC File Offset: 0x0001B6BC
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x0001D4C4 File Offset: 0x0001B6C4
		public TextWidget TitleTextWidget { get; set; }

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0001D4CD File Offset: 0x0001B6CD
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x0001D4D5 File Offset: 0x0001B6D5
		public ScrollingRichTextWidget KillerNameTextWidget { get; set; }

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0001D4DE File Offset: 0x0001B6DE
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x0001D4E6 File Offset: 0x0001B6E6
		public Widget KillCountContainer { get; set; }

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0001D4EF File Offset: 0x0001B6EF
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x0001D4F7 File Offset: 0x0001B6F7
		public Brush SelfInflictedTitleBrush { get; set; }

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0001D500 File Offset: 0x0001B700
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x0001D508 File Offset: 0x0001B708
		public Brush NormalBrushTitleBrush { get; set; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0001D511 File Offset: 0x0001B711
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x0001D519 File Offset: 0x0001B719
		public float FadeInModifier { get; set; } = 2f;

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0001D522 File Offset: 0x0001B722
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x0001D52A File Offset: 0x0001B72A
		public float FadeOutModifier { get; set; } = 10f;

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0001D533 File Offset: 0x0001B733
		// (set) Token: 0x06000A7A RID: 2682 RVA: 0x0001D53B File Offset: 0x0001B73B
		public float StayTime { get; set; } = 7f;

		// Token: 0x06000A7B RID: 2683 RVA: 0x0001D544 File Offset: 0x0001B744
		public MultiplayerDeathCardWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0001D570 File Offset: 0x0001B770
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this._initialized = true;
				base.IsEnabled = false;
				this._initAlpha = base.AlphaFactor;
				this.SetGlobalAlphaRecursively(this._targetAlpha);
			}
			if (Math.Abs(base.AlphaFactor - this._targetAlpha) > 1E-45f)
			{
				float num = ((base.AlphaFactor > this._targetAlpha) ? this.FadeOutModifier : this.FadeInModifier);
				float num2 = Mathf.Lerp(base.AlphaFactor, this._targetAlpha, dt * num);
				this.SetGlobalAlphaRecursively(num2);
			}
			if ((this.IsActive && base.AlphaFactor < 1E-45f) || base.Context.EventManager.Time - this._activeTimeStart > this.StayTime)
			{
				this.IsActive = false;
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0001D640 File Offset: 0x0001B840
		private void HandleIsActiveToggle(bool isActive)
		{
			this._targetAlpha = (isActive ? 1f : 0f);
			if (isActive)
			{
				this._activeTimeStart = base.Context.EventManager.Time;
			}
			this.KillCountContainer.IsVisible = !this.IsSelfInflicted && this.KillCountsEnabled;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0001D698 File Offset: 0x0001B898
		private void HandleSelfInflictedToggle(bool isSelfInflicted)
		{
			this.TitleTextWidget.IsVisible = true;
			this.TitleTextWidget.Brush = (isSelfInflicted ? this.SelfInflictedTitleBrush : this.NormalBrushTitleBrush);
			this.KillerNameTextWidget.IsVisible = !isSelfInflicted;
			this.WeaponTextWidget.IsVisible = !isSelfInflicted;
			this.KillCountContainer.IsVisible = !this.IsSelfInflicted && this.KillCountsEnabled;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0001D707 File Offset: 0x0001B907
		private void HandleKillCountsEnabledSwitch(bool killCountsEnabled)
		{
			this.KillCountContainer.IsVisible = killCountsEnabled;
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x0001D715 File Offset: 0x0001B915
		// (set) Token: 0x06000A81 RID: 2689 RVA: 0x0001D71D File Offset: 0x0001B91D
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
					this.HandleIsActiveToggle(value);
				}
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x0001D742 File Offset: 0x0001B942
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x0001D74A File Offset: 0x0001B94A
		public bool IsSelfInflicted
		{
			get
			{
				return this._isSelfInflicted;
			}
			set
			{
				if (value != this._isSelfInflicted)
				{
					this._isSelfInflicted = value;
					base.OnPropertyChanged(value, "IsSelfInflicted");
					this.HandleSelfInflictedToggle(value);
				}
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x0001D76F File Offset: 0x0001B96F
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x0001D777 File Offset: 0x0001B977
		public bool KillCountsEnabled
		{
			get
			{
				return this._killCountsEnabled;
			}
			set
			{
				if (value != this._killCountsEnabled)
				{
					this._killCountsEnabled = value;
					base.OnPropertyChanged(value, "KillCountsEnabled");
					this.HandleKillCountsEnabledSwitch(value);
				}
			}
		}

		// Token: 0x040004C5 RID: 1221
		private float _targetAlpha;

		// Token: 0x040004C6 RID: 1222
		private float _initAlpha;

		// Token: 0x040004CA RID: 1226
		private float _activeTimeStart;

		// Token: 0x040004CB RID: 1227
		private bool _initialized;

		// Token: 0x040004CC RID: 1228
		private bool _isActive;

		// Token: 0x040004CD RID: 1229
		private bool _isSelfInflicted;

		// Token: 0x040004CE RID: 1230
		private bool _killCountsEnabled;
	}
}
