using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000088 RID: 136
	public class MultiplayerEndOfBattleScreenWidget : Widget
	{
		// Token: 0x06000797 RID: 1943 RVA: 0x000161DE File Offset: 0x000143DE
		public MultiplayerEndOfBattleScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00016200 File Offset: 0x00014400
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isAnimationStarted)
			{
				this.SetGlobalAlphaRecursively(MathF.Lerp(this._initialAlpha, this._targetAlpha, this._fadeInTimeElapsed / this.FadeInDuration, 1E-05f));
				this._fadeInTimeElapsed += dt;
				if (this._fadeInTimeElapsed >= this.FadeInDuration)
				{
					this._isAnimationStarted = false;
				}
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00016268 File Offset: 0x00014468
		// (set) Token: 0x0600079A RID: 1946 RVA: 0x00016270 File Offset: 0x00014470
		[Editor(false)]
		public bool IsShown
		{
			get
			{
				return this._isShown;
			}
			set
			{
				if (value != this._isShown)
				{
					this._isShown = value;
					base.OnPropertyChanged(value, "IsShown");
					this.SetGlobalAlphaRecursively(0f);
					this._isAnimationStarted = value;
					base.IsVisible = value;
					this._fadeInTimeElapsed = 0f;
				}
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x000162BD File Offset: 0x000144BD
		// (set) Token: 0x0600079C RID: 1948 RVA: 0x000162C5 File Offset: 0x000144C5
		[Editor(false)]
		public float FadeInDuration
		{
			get
			{
				return this._fadeInDuration;
			}
			set
			{
				if (value != this._fadeInDuration)
				{
					this._fadeInDuration = value;
					base.OnPropertyChanged(value, "FadeInDuration");
				}
			}
		}

		// Token: 0x04000352 RID: 850
		private float _initialAlpha;

		// Token: 0x04000353 RID: 851
		private float _targetAlpha = 1f;

		// Token: 0x04000354 RID: 852
		private bool _isAnimationStarted;

		// Token: 0x04000355 RID: 853
		private float _fadeInTimeElapsed;

		// Token: 0x04000356 RID: 854
		private bool _isShown;

		// Token: 0x04000357 RID: 855
		private float _fadeInDuration = 0.3f;
	}
}
