using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000036 RID: 54
	public class ParallaxItemBrushWidget : BrushWidget
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000A48F File Offset: 0x0000868F
		// (set) Token: 0x06000336 RID: 822 RVA: 0x0000A497 File Offset: 0x00008697
		public bool IsEaseInOutEnabled { get; set; } = true;

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000A4A0 File Offset: 0x000086A0
		// (set) Token: 0x06000338 RID: 824 RVA: 0x0000A4A8 File Offset: 0x000086A8
		public float OneDirectionDuration { get; set; } = 1f;

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000A4B1 File Offset: 0x000086B1
		// (set) Token: 0x0600033A RID: 826 RVA: 0x0000A4B9 File Offset: 0x000086B9
		public float OneDirectionDistance { get; set; } = 1f;

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000A4C2 File Offset: 0x000086C2
		// (set) Token: 0x0600033C RID: 828 RVA: 0x0000A4CA File Offset: 0x000086CA
		public ParallaxItemBrushWidget.ParallaxMovementDirection InitialDirection { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000A4D3 File Offset: 0x000086D3
		private float _centerOffset
		{
			get
			{
				return this.OneDirectionDuration / 2f;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000A4E1 File Offset: 0x000086E1
		private float _localTime
		{
			get
			{
				return base.Context.EventManager.Time + this._centerOffset;
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000A4FA File Offset: 0x000086FA
		public ParallaxItemBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000A520 File Offset: 0x00008720
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				this.OneDirectionDuration = MathF.Max(float.Epsilon, this.OneDirectionDuration);
				this._initialized = true;
			}
			if (this.InitialDirection != ParallaxItemBrushWidget.ParallaxMovementDirection.None)
			{
				bool flag = this._localTime % (this.OneDirectionDuration * 4f) > this.OneDirectionDuration * 2f;
				float num3;
				if (this.IsEaseInOutEnabled)
				{
					float num = this._localTime % (this.OneDirectionDuration * 4f);
					float oneDirectionDuration = this.OneDirectionDuration;
					float num2 = MathF.PingPong(0f, this.OneDirectionDuration * 4f, this._localTime) / (this.OneDirectionDuration * 4f);
					float quadEaseInOut = this.GetQuadEaseInOut(num2);
					num3 = MathF.Lerp(-this.OneDirectionDistance, this.OneDirectionDistance, quadEaseInOut, 1E-05f);
				}
				else
				{
					float num4 = MathF.PingPong(0f, this.OneDirectionDuration, this._localTime) / this.OneDirectionDuration;
					num3 = this.OneDirectionDistance * num4;
					num3 = (flag ? (-num3) : num3);
				}
				switch (this.InitialDirection)
				{
				case ParallaxItemBrushWidget.ParallaxMovementDirection.Left:
					base.PositionXOffset = num3;
					return;
				case ParallaxItemBrushWidget.ParallaxMovementDirection.Right:
					base.PositionXOffset = -num3;
					return;
				case ParallaxItemBrushWidget.ParallaxMovementDirection.Up:
					base.PositionYOffset = -num3;
					return;
				case ParallaxItemBrushWidget.ParallaxMovementDirection.Down:
					base.PositionYOffset = num3;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000A674 File Offset: 0x00008874
		private float GetCubicEaseInOut(float t)
		{
			if (t < 0.5f)
			{
				return 4f * t * t * t;
			}
			float num = 2f * t - 2f;
			return 0.5f * num * num * num + 1f;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000A6B4 File Offset: 0x000088B4
		private float GetElasticEaseInOut(float t)
		{
			if (t < 0.5f)
			{
				return (float)(0.5 * Math.Sin(20.420352248333657 * (double)(2f * t)) * Math.Pow(2.0, (double)(10f * (2f * t - 1f))));
			}
			return (float)(0.5 * (Math.Sin(-20.420352248333657 * (double)(2f * t - 1f + 1f)) * Math.Pow(2.0, (double)(-10f * (2f * t - 1f))) + 2.0));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000A76C File Offset: 0x0000896C
		private float ExponentialEaseInOut(float t)
		{
			if (t == 0f || t == 1f)
			{
				return t;
			}
			if (t < 0.5f)
			{
				return (float)(0.5 * Math.Pow(2.0, (double)(20f * t - 10f)));
			}
			return (float)(-0.5 * Math.Pow(2.0, (double)(-20f * t + 10f)) + 1.0);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000A7EC File Offset: 0x000089EC
		private float GetQuadEaseInOut(float t)
		{
			if (t < 0.5f)
			{
				return 2f * t * t;
			}
			return -2f * t * t + 4f * t - 1f;
		}

		// Token: 0x04000152 RID: 338
		private bool _initialized;

		// Token: 0x020001A1 RID: 417
		public enum ParallaxMovementDirection
		{
			// Token: 0x040009AC RID: 2476
			None,
			// Token: 0x040009AD RID: 2477
			Left,
			// Token: 0x040009AE RID: 2478
			Right,
			// Token: 0x040009AF RID: 2479
			Up,
			// Token: 0x040009B0 RID: 2480
			Down
		}
	}
}
