using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E4 RID: 228
	public class TakenDamageItemBrushWidget : BrushWidget
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x000209A2 File Offset: 0x0001EBA2
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x000209AA File Offset: 0x0001EBAA
		public float VerticalWidth { get; set; }

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x000209B3 File Offset: 0x0001EBB3
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x000209BB File Offset: 0x0001EBBB
		public float VerticalHeight { get; set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x000209C4 File Offset: 0x0001EBC4
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x000209CC File Offset: 0x0001EBCC
		public float HorizontalWidth { get; set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x000209D5 File Offset: 0x0001EBD5
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x000209DD File Offset: 0x0001EBDD
		public float HorizontalHeight { get; set; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x000209E6 File Offset: 0x0001EBE6
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x000209EE File Offset: 0x0001EBEE
		public float RangedOnScreenStayTime { get; set; } = 0.3f;

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x000209F7 File Offset: 0x0001EBF7
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x000209FF File Offset: 0x0001EBFF
		public float MeleeOnScreenStayTime { get; set; } = 1f;

		// Token: 0x06000BCC RID: 3020 RVA: 0x00020A08 File Offset: 0x0001EC08
		public TakenDamageItemBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00020A28 File Offset: 0x0001EC28
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.RegisterBrushStatesOfWidget();
				this._initialized = true;
				if (!this.IsRanged)
				{
					float num = (float)this.DamageAmount / 70f;
					num = MathF.Clamp(num, 0f, 1f);
					base.AlphaFactor = MathF.Lerp(0.3f, 1f, num, 1E-05f);
				}
			}
			this.UpdateAlpha(dt);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00020A9C File Offset: 0x0001EC9C
		private void UpdateAlpha(float dt)
		{
			if (base.AlphaFactor < 0.01f)
			{
				base.EventFired("OnRemove", Array.Empty<object>());
			}
			float num = (this.IsRanged ? this.RangedOnScreenStayTime : this.MeleeOnScreenStayTime);
			this.SetGlobalAlphaRecursively(MathF.Lerp(base.AlphaFactor, 0f, dt / num, 1E-05f));
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00020AFB File Offset: 0x0001ECFB
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			if (base.AlphaFactor > 0f)
			{
				base.OnRender(twoDimensionContext, drawContext);
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x00020B12 File Offset: 0x0001ED12
		// (set) Token: 0x06000BD1 RID: 3025 RVA: 0x00020B1A File Offset: 0x0001ED1A
		[DataSourceProperty]
		public int DamageAmount
		{
			get
			{
				return this._damageAmount;
			}
			set
			{
				if (this._damageAmount != value)
				{
					this._damageAmount = value;
					base.OnPropertyChanged(value, "DamageAmount");
				}
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x00020B38 File Offset: 0x0001ED38
		// (set) Token: 0x06000BD3 RID: 3027 RVA: 0x00020B40 File Offset: 0x0001ED40
		[DataSourceProperty]
		public bool IsBehind
		{
			get
			{
				return this._isBehind;
			}
			set
			{
				if (this._isBehind != value)
				{
					this._isBehind = value;
					base.OnPropertyChanged(value, "IsBehind");
				}
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x00020B5E File Offset: 0x0001ED5E
		// (set) Token: 0x06000BD5 RID: 3029 RVA: 0x00020B66 File Offset: 0x0001ED66
		[DataSourceProperty]
		public bool IsRanged
		{
			get
			{
				return this._isRanged;
			}
			set
			{
				if (this._isRanged != value)
				{
					this._isRanged = value;
					base.OnPropertyChanged(value, "IsRanged");
				}
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x00020B84 File Offset: 0x0001ED84
		// (set) Token: 0x06000BD7 RID: 3031 RVA: 0x00020B8C File Offset: 0x0001ED8C
		[DataSourceProperty]
		public Vec2 ScreenPosOfAffectorAgent
		{
			get
			{
				return this._screenPosOfAffectorAgent;
			}
			set
			{
				if (this._screenPosOfAffectorAgent != value)
				{
					this._screenPosOfAffectorAgent = value;
					base.OnPropertyChanged(value, "ScreenPosOfAffectorAgent");
				}
			}
		}

		// Token: 0x0400054F RID: 1359
		private bool _initialized;

		// Token: 0x04000556 RID: 1366
		private int _damageAmount;

		// Token: 0x04000557 RID: 1367
		private Vec2 _screenPosOfAffectorAgent;

		// Token: 0x04000558 RID: 1368
		private bool _isBehind;

		// Token: 0x04000559 RID: 1369
		private bool _isRanged;
	}
}
