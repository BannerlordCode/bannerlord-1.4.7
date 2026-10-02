using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A2 RID: 162
	public class MultiplayerLobbyCosmeticAnimationPartWidget : Widget
	{
		// Token: 0x060008B4 RID: 2228 RVA: 0x00018F7D File Offset: 0x0001717D
		public MultiplayerLobbyCosmeticAnimationPartWidget(UIContext context)
			: base(context)
		{
			this.StopAnimation();
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00018F8C File Offset: 0x0001718C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isAnimationPlaying)
			{
				return;
			}
			if (this._alphaChangeTimeElapsed >= this._alphaChangeDuration)
			{
				this.InvertAnimationDirection();
				this.InitializeAnimationParameters();
			}
			this._currentAlpha = MathF.Lerp(this._currentAlpha, this._targetAlpha, this._alphaChangeTimeElapsed / this._alphaChangeDuration, 1E-05f);
			base.AlphaFactor = this._currentAlpha;
			this._alphaChangeTimeElapsed += dt;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00019006 File Offset: 0x00017206
		public void InitializeAnimationParameters()
		{
			this._currentAlpha = this._minAlpha;
			this._targetAlpha = this._maxAlpha;
			this._alphaChangeTimeElapsed = 0f;
			base.AlphaFactor = this._currentAlpha;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00019038 File Offset: 0x00017238
		private void InvertAnimationDirection()
		{
			float minAlpha = this._minAlpha;
			this._minAlpha = this._maxAlpha;
			this._maxAlpha = minAlpha;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0001905F File Offset: 0x0001725F
		public void StartAnimation(float alphaChangeDuration, float minAlpha, float maxAlpha)
		{
			this._alphaChangeDuration = alphaChangeDuration;
			this._minAlpha = minAlpha;
			this._maxAlpha = maxAlpha;
			this.InitializeAnimationParameters();
			this._isAnimationPlaying = true;
			base.IsVisible = true;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0001908A File Offset: 0x0001728A
		public void StopAnimation()
		{
			this.InitializeAnimationParameters();
			this._isAnimationPlaying = false;
			base.IsVisible = false;
		}

		// Token: 0x040003F0 RID: 1008
		private float _alphaChangeDuration;

		// Token: 0x040003F1 RID: 1009
		private float _minAlpha;

		// Token: 0x040003F2 RID: 1010
		private float _maxAlpha;

		// Token: 0x040003F3 RID: 1011
		private float _currentAlpha;

		// Token: 0x040003F4 RID: 1012
		private float _targetAlpha;

		// Token: 0x040003F5 RID: 1013
		private float _alphaChangeTimeElapsed;

		// Token: 0x040003F6 RID: 1014
		private bool _isAnimationPlaying;
	}
}
