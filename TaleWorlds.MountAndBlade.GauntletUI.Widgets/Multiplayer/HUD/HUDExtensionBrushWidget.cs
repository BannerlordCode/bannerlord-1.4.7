using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C4 RID: 196
	public class HUDExtensionBrushWidget : BrushWidget
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x0001C8FF File Offset: 0x0001AAFF
		// (set) Token: 0x06000A31 RID: 2609 RVA: 0x0001C907 File Offset: 0x0001AB07
		public float AlphaChangeDuration { get; set; } = 0.15f;

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x0001C910 File Offset: 0x0001AB10
		// (set) Token: 0x06000A33 RID: 2611 RVA: 0x0001C918 File Offset: 0x0001AB18
		public float OrderEnabledAlpha { get; set; } = 0.3f;

		// Token: 0x06000A34 RID: 2612 RVA: 0x0001C921 File Offset: 0x0001AB21
		public HUDExtensionBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0001C964 File Offset: 0x0001AB64
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._currentAlpha - this._targetAlpha > 1E-45f)
			{
				if (this._alphaChangeTimeElapsed < this.AlphaChangeDuration)
				{
					this._currentAlpha = MathF.Lerp(this._initialAlpha, this._targetAlpha, this._alphaChangeTimeElapsed / this.AlphaChangeDuration, 1E-05f);
					this.SetGlobalAlphaRecursively(this._currentAlpha);
					this._alphaChangeTimeElapsed += dt;
					return;
				}
			}
			else if (this._currentAlpha != this._targetAlpha)
			{
				this._currentAlpha = this._targetAlpha;
				this.SetGlobalAlphaRecursively(this._targetAlpha);
			}
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0001CA04 File Offset: 0x0001AC04
		private void OnIsOrderEnabledChanged()
		{
			this._alphaChangeTimeElapsed = 0f;
			this._targetAlpha = (this.IsOrderActive ? this.OrderEnabledAlpha : 1f);
			this._initialAlpha = this._currentAlpha;
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0001CA38 File Offset: 0x0001AC38
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x0001CA40 File Offset: 0x0001AC40
		[Editor(false)]
		public bool IsOrderActive
		{
			get
			{
				return this._isOrderActive;
			}
			set
			{
				if (this._isOrderActive != value)
				{
					this._isOrderActive = value;
					base.OnPropertyChanged(value, "IsOrderActive");
					this.OnIsOrderEnabledChanged();
				}
			}
		}

		// Token: 0x0400049C RID: 1180
		private float _alphaChangeTimeElapsed;

		// Token: 0x0400049D RID: 1181
		private float _initialAlpha = 1f;

		// Token: 0x0400049E RID: 1182
		private float _targetAlpha = 1f;

		// Token: 0x0400049F RID: 1183
		private float _currentAlpha = 1f;

		// Token: 0x040004A0 RID: 1184
		private bool _isOrderActive;
	}
}
