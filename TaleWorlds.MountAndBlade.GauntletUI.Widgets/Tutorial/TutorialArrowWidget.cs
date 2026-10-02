using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x02000049 RID: 73
	public class TutorialArrowWidget : Widget
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000CB27 File Offset: 0x0000AD27
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0000CB2F File Offset: 0x0000AD2F
		public bool IsArrowEnabled { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000CB38 File Offset: 0x0000AD38
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0000CB40 File Offset: 0x0000AD40
		public float FadeInTime { get; set; } = 1f;

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000CB49 File Offset: 0x0000AD49
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x0000CB51 File Offset: 0x0000AD51
		public float BigCircleRadius { get; set; } = 2f;

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0000CB5A File Offset: 0x0000AD5A
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x0000CB62 File Offset: 0x0000AD62
		public float SmallCircleRadius { get; set; } = 2f;

		// Token: 0x0600040F RID: 1039 RVA: 0x0000CB6B File Offset: 0x0000AD6B
		public TutorialArrowWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000CB98 File Offset: 0x0000AD98
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this.IsArrowEnabled)
			{
				base.IsVisible = false;
				this.SetGlobalAlphaRecursively(0f);
				return;
			}
			base.IsVisible = true;
			base.ScaledSuggestedWidth = this._localWidth;
			base.ScaledSuggestedHeight = this._localHeight;
			if (this._startTime > -1f)
			{
				float num = Mathf.Lerp(0f, 1f, Mathf.Clamp((base.EventManager.Time - this._startTime) / this.FadeInTime, 0f, 1f));
				this.SetGlobalAlphaRecursively(num);
				return;
			}
			this.SetGlobalAlphaRecursively(0f);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000CC40 File Offset: 0x0000AE40
		public void SetArrowProperties(float width, float height, bool isDirectionDown, bool isDirectionRight)
		{
			if (this._localWidth != width || this._localHeight != height || this._isDirectionDown != isDirectionDown || this._isDirectionRight != isDirectionRight)
			{
				base.RemoveAllChildren();
				float num = (float)Math.Sqrt((double)(width * width + height * height));
				float num2 = (this.BigCircleRadius + this.SmallCircleRadius) / 2f;
				int num3 = (int)(num / num2);
				float num4 = 0f;
				float num5 = 0f;
				float num6;
				float num7;
				if (isDirectionDown)
				{
					num6 = width;
					num7 = height;
				}
				else
				{
					num6 = width;
					num5 = height;
					num7 = 0f;
				}
				float num8 = (isDirectionRight ? this.BigCircleRadius : this.SmallCircleRadius);
				float num9 = (isDirectionRight ? this.SmallCircleRadius : this.BigCircleRadius);
				for (int i = 0; i < num3; i++)
				{
					Widget defaultCircleWidgetTemplate = this.GetDefaultCircleWidgetTemplate();
					base.AddChild(defaultCircleWidgetTemplate);
					float num10 = num2 * (float)i / MathF.Abs(num4 - num6);
					float num11 = Mathf.Lerp(num8, num9, num10);
					defaultCircleWidgetTemplate.PositionXOffset = Mathf.Lerp(num4, num6, num10);
					defaultCircleWidgetTemplate.PositionYOffset = Mathf.Lerp(num5, num7, num10);
					defaultCircleWidgetTemplate.SuggestedHeight = num11;
					defaultCircleWidgetTemplate.SuggestedWidth = num11;
				}
				this._localWidth = width;
				this._localHeight = height;
				this._isDirectionDown = isDirectionDown;
				this._isDirectionRight = isDirectionRight;
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000CD89 File Offset: 0x0000AF89
		public void ResetFade()
		{
			this._startTime = base.EventManager.Time;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000CD9C File Offset: 0x0000AF9C
		public void DisableFade()
		{
			this._startTime = base.EventManager.Time;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000CDAF File Offset: 0x0000AFAF
		private Widget GetDefaultCircleWidgetTemplate()
		{
			return new Widget(base.Context)
			{
				WidthSizePolicy = SizePolicy.Fixed,
				HeightSizePolicy = SizePolicy.Fixed,
				Sprite = base.Context.SpriteData.GetSprite("BlankWhiteCircle"),
				IsEnabled = false
			};
		}

		// Token: 0x040001AE RID: 430
		private float _localWidth;

		// Token: 0x040001AF RID: 431
		private float _localHeight;

		// Token: 0x040001B0 RID: 432
		private bool _isDirectionDown;

		// Token: 0x040001B1 RID: 433
		private bool _isDirectionRight;

		// Token: 0x040001B2 RID: 434
		private float _startTime;
	}
}
