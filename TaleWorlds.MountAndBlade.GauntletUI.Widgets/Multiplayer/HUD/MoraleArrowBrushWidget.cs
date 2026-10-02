using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C5 RID: 197
	public class MoraleArrowBrushWidget : BrushWidget
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0001CA64 File Offset: 0x0001AC64
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x0001CA6C File Offset: 0x0001AC6C
		public bool LeftSideArrow { get; set; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0001CA75 File Offset: 0x0001AC75
		public float BaseHorizontalExtendRange
		{
			get
			{
				return 3.3f;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0001CA7C File Offset: 0x0001AC7C
		private float BaseSpeedModifier
		{
			get
			{
				return 13f;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0001CA83 File Offset: 0x0001AC83
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x0001CA8B File Offset: 0x0001AC8B
		public bool AreMoralesIndependent { get; set; }

		// Token: 0x06000A3F RID: 2623 RVA: 0x0001CA94 File Offset: 0x0001AC94
		public MoraleArrowBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0001CAA0 File Offset: 0x0001ACA0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				base.Brush.GlobalAlphaFactor = 0f;
				this._initialized = true;
			}
			base.IsVisible = this._currentFlow > 0 && !this.AreMoralesIndependent;
			if (base.IsVisible)
			{
				float num = this.BaseSpeedModifier * (float)Math.Sqrt((double)this._currentFlow);
				float num2 = this.BaseHorizontalExtendRange * (float)this._currentFlow;
				if (this._currentAnimState == MoraleArrowBrushWidget.AnimStates.FadeIn)
				{
					if (base.ReadOnlyBrush.GlobalAlphaFactor < 1f)
					{
						this.SetGlobalAlphaRecursively(Mathf.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, 1f, dt * num));
					}
					if ((double)base.ReadOnlyBrush.GlobalAlphaFactor >= 0.99)
					{
						this._currentAnimState = MoraleArrowBrushWidget.AnimStates.Move;
					}
				}
				else if (this._currentAnimState == MoraleArrowBrushWidget.AnimStates.Move)
				{
					if (Math.Abs(base.PositionXOffset) < num2)
					{
						int num3 = (this.LeftSideArrow ? (-1) : 1);
						base.PositionXOffset = Mathf.Lerp(base.PositionXOffset, num2 * (float)num3, dt * num);
					}
					if ((double)Math.Abs(base.PositionXOffset) >= (double)num2 - 0.01)
					{
						this._currentAnimState = MoraleArrowBrushWidget.AnimStates.FadeOut;
					}
				}
				else if (this._currentAnimState == MoraleArrowBrushWidget.AnimStates.FadeOut)
				{
					if (base.ReadOnlyBrush.GlobalAlphaFactor > 0f)
					{
						this.SetGlobalAlphaRecursively(Mathf.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, 0f, dt * num));
					}
					if ((double)base.ReadOnlyBrush.GlobalAlphaFactor <= 0.01)
					{
						this._currentAnimState = MoraleArrowBrushWidget.AnimStates.GoToInitPos;
					}
				}
				else
				{
					base.PositionXOffset = 0f;
					this._currentAnimState = MoraleArrowBrushWidget.AnimStates.FadeIn;
				}
			}
			else
			{
				base.PositionXOffset = 0f;
				this._currentAnimState = MoraleArrowBrushWidget.AnimStates.FadeIn;
			}
			this._timeSinceCreation += dt;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0001CC6A File Offset: 0x0001AE6A
		public void SetFlowLevel(int flow)
		{
			this._currentFlow = flow;
			base.IsVisible = this._currentFlow > 0 && !this.AreMoralesIndependent;
		}

		// Token: 0x040004A1 RID: 1185
		private float _timeSinceCreation;

		// Token: 0x040004A2 RID: 1186
		private bool _initialized;

		// Token: 0x040004A3 RID: 1187
		private int _currentFlow;

		// Token: 0x040004A6 RID: 1190
		private MoraleArrowBrushWidget.AnimStates _currentAnimState;

		// Token: 0x020001BC RID: 444
		private enum AnimStates
		{
			// Token: 0x04000A03 RID: 2563
			FadeIn,
			// Token: 0x04000A04 RID: 2564
			Move,
			// Token: 0x04000A05 RID: 2565
			FadeOut,
			// Token: 0x04000A06 RID: 2566
			GoToInitPos
		}
	}
}
