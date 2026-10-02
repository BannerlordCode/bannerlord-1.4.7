using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x0200007F RID: 127
	public class PartyPlayerNameplateWidget : PartyNameplateWidget
	{
		// Token: 0x06000714 RID: 1812 RVA: 0x00014680 File Offset: 0x00012880
		public PartyPlayerNameplateWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001468C File Offset: 0x0001288C
		protected override void UpdateNameplatesVisibility(float dt)
		{
			bool flag = base.IsPositionOutsideScreen() || base.IsBehind || base.IsHigh;
			bool flag2 = flag || base.IsInArmy || this.IsPrisoner || base.IsInSettlement;
			base.NameplateTextWidget.IsVisible = !flag2;
			base.NameplateFullNameTextWidget.IsVisible = !flag2;
			base.SpeedTextWidget.IsVisible = !flag2;
			base.SpeedIconWidget.IsVisible = !flag2;
			base.PartyBannerWidget.IsVisible = !flag2;
			base.NameplateExtraInfoTextWidget.IsVisible = !flag2;
			base.DisorganizedWidget.IsVisible = !flag2 && base.IsDisorganized;
			float num = (float)(flag2 ? 0 : 1);
			this.MainPartyArrowWidget.IsVisible = flag;
			base.TrackerFrame.IsVisible = flag;
			base.IsEnabled = flag;
			float num2;
			if (this._initialDelayAmount <= 0f)
			{
				num2 = (float)(base.ShouldShowFullName ? 1 : 0);
			}
			else
			{
				this._initialDelayAmount -= dt;
				num2 = 1f;
			}
			base.NameplateTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(base.NameplateTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num, dt * base._animSpeedModifier, 1E-05f);
			base.NameplateFullNameTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(base.NameplateFullNameTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num2, dt * base._animSpeedModifier, 1E-05f);
			base.SpeedTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(base.SpeedTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num2, dt * base._animSpeedModifier, 1E-05f);
			float num3 = MathF.Lerp(base.SpeedIconWidget.AlphaFactor, num2, dt * base._animSpeedModifier, 1E-05f);
			base.SpeedIconWidget.SetGlobalAlphaRecursively(num3);
			base.NameplateExtraInfoTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(base.NameplateExtraInfoTextWidget.ReadOnlyBrush.GlobalAlphaFactor, (float)(base.ShouldShowFullName ? 1 : 0), dt * base._animSpeedModifier, 1E-05f);
			base.PartyBannerWidget.Brush.GlobalAlphaFactor = MathF.Lerp(base.PartyBannerWidget.ReadOnlyBrush.GlobalAlphaFactor, num, dt * base._animSpeedModifier, 1E-05f);
			base.ParleyIconWidget.AlphaFactor = MathF.Lerp(base.ParleyIconWidget.AlphaFactor, (float)(base.CanParley ? 1 : 0), dt * base._animSpeedModifier, 1E-05f);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00014914 File Offset: 0x00012B14
		protected override void UpdateNameplatesScreenPosition()
		{
			this._screenWidth = base.Context.EventManager.PageSize.X;
			this._screenHeight = base.Context.EventManager.PageSize.Y;
			bool flag = base.IsBehind || base.IsPositionOutsideScreen();
			bool flag2 = base.IsHigh || base.IsBehind || base.IsPositionOutsideScreen();
			if (flag)
			{
				Vec2 vec = new Vec2(this._screenWidth / 2f, this._screenHeight / 2f);
				Vec2 vec2 = base.HeadPosition;
				vec2 -= vec;
				if (base.IsBehind)
				{
					vec2 *= -1f;
				}
				float num = Mathf.Atan2(vec2.y, vec2.x) - 1.5707964f;
				float num2 = Mathf.Cos(num);
				float num3 = Mathf.Sin(num);
				float num4 = num2 / num3;
				Vec2 vec3 = vec * 1f;
				vec2 = ((num2 > 0f) ? new Vec2(-vec3.y / num4, vec.y) : new Vec2(vec3.y / num4, -vec.y));
				if (vec2.x > vec3.x)
				{
					vec2 = new Vec2(vec3.x, -vec3.x * num4);
				}
				else if (vec2.x < -vec3.x)
				{
					vec2 = new Vec2(-vec3.x, vec3.x * num4);
				}
				vec2 += vec;
				base.ScaledPositionXOffset = vec2.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = vec2.y - base.Size.Y / 2f;
			}
			else
			{
				Widget headGroupWidget = base.HeadGroupWidget;
				float num5 = ((headGroupWidget != null) ? headGroupWidget.Size.Y : 0f);
				base.NameplateLayoutListPanel.ScaledPositionXOffset = base.Size.X / 2f - base.PartyBannerWidget.Size.X;
				base.NameplateLayoutListPanel.ScaledPositionYOffset = base.Position.y - base.HeadPosition.y + num5;
				base.ScaledPositionXOffset = base.HeadPosition.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = base.HeadPosition.y - num5;
			}
			if (flag2)
			{
				base.ScaledPositionXOffset = MathF.Clamp(base.ScaledPositionXOffset, 0f, this._screenWidth - base.Size.X);
				base.ScaledPositionYOffset = MathF.Clamp(base.ScaledPositionYOffset, 0f, this._screenHeight - base.Size.Y);
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x00014BCD File Offset: 0x00012DCD
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x00014BD5 File Offset: 0x00012DD5
		public bool IsPrisoner
		{
			get
			{
				return this._isPrisoner;
			}
			set
			{
				if (this._isPrisoner != value)
				{
					this._isPrisoner = value;
					base.OnPropertyChanged(value, "IsPrisoner");
				}
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x00014BF3 File Offset: 0x00012DF3
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x00014BFB File Offset: 0x00012DFB
		public Widget MainPartyArrowWidget
		{
			get
			{
				return this._mainPartyArrowWidget;
			}
			set
			{
				if (this._mainPartyArrowWidget != value)
				{
					this._mainPartyArrowWidget = value;
					base.OnPropertyChanged<Widget>(value, "MainPartyArrowWidget");
				}
			}
		}

		// Token: 0x04000314 RID: 788
		private bool _isPrisoner;

		// Token: 0x04000315 RID: 789
		private Widget _mainPartyArrowWidget;
	}
}
