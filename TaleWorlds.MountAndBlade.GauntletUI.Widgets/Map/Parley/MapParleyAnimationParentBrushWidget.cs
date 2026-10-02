using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Parley
{
	// Token: 0x0200011F RID: 287
	public class MapParleyAnimationParentBrushWidget : BrushWidget
	{
		// Token: 0x06000F2D RID: 3885 RVA: 0x00029B74 File Offset: 0x00027D74
		public MapParleyAnimationParentBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00029B84 File Offset: 0x00027D84
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._firstFrame)
			{
				this._firstFrame = false;
				this._targetYOffset = base.PositionYOffset;
				this._minYOffset = this._targetYOffset - 50f;
			}
			this._animationDelta += dt;
			if (this._animationDelta < 0.1f)
			{
				float num = this._animationDelta / 0.1f;
				base.PositionYOffset = MathF.Lerp(this._minYOffset, this._targetYOffset, num, 1E-05f);
				this.SetGlobalAlphaRecursively(MathF.Lerp(0f, 1f, num, 1E-05f));
				return;
			}
			if (this._animationDelta < this.AnimationDuration - 0.1f)
			{
				base.PositionYOffset = this._targetYOffset;
				this.SetGlobalAlphaRecursively(1f);
				return;
			}
			if (this._animationDelta < this.AnimationDuration)
			{
				float num2 = (this._animationDelta - (this.AnimationDuration - 0.1f)) / 0.1f;
				base.PositionYOffset = MathF.Lerp(this._targetYOffset, this._minYOffset, num2, 1E-05f);
				this.SetGlobalAlphaRecursively(MathF.Lerp(1f, 0f, num2, 1E-05f));
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x00029CB2 File Offset: 0x00027EB2
		// (set) Token: 0x06000F30 RID: 3888 RVA: 0x00029CBA File Offset: 0x00027EBA
		[Editor(false)]
		public float AnimationDuration
		{
			get
			{
				return this._animationDuration;
			}
			set
			{
				if (this._animationDuration != value)
				{
					this._animationDuration = value;
					base.OnPropertyChanged(value, "AnimationDuration");
				}
			}
		}

		// Token: 0x040006E3 RID: 1763
		private bool _firstFrame = true;

		// Token: 0x040006E4 RID: 1764
		private const float _fadeInOutDuration = 0.1f;

		// Token: 0x040006E5 RID: 1765
		private float _animationDelta;

		// Token: 0x040006E6 RID: 1766
		private float _targetYOffset;

		// Token: 0x040006E7 RID: 1767
		private float _minYOffset;

		// Token: 0x040006E8 RID: 1768
		private const float _fadeInOutYMovement = 50f;

		// Token: 0x040006E9 RID: 1769
		private float _animationDuration;
	}
}
