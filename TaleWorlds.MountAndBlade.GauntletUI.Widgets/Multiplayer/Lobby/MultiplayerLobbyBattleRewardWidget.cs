using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A0 RID: 160
	public class MultiplayerLobbyBattleRewardWidget : Widget
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x00018666 File Offset: 0x00016866
		// (set) Token: 0x06000888 RID: 2184 RVA: 0x0001866E File Offset: 0x0001686E
		public float AnimationDuration { get; set; } = 0.1f;

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x00018677 File Offset: 0x00016877
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x0001867F File Offset: 0x0001687F
		public float TextRevealAnimationDuration { get; set; } = 0.05f;

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00018688 File Offset: 0x00016888
		// (set) Token: 0x0600088C RID: 2188 RVA: 0x00018690 File Offset: 0x00016890
		public float AnimationInitialScaleMultiplier { get; set; } = 2f;

		// Token: 0x0600088D RID: 2189 RVA: 0x00018699 File Offset: 0x00016899
		public MultiplayerLobbyBattleRewardWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x000186C4 File Offset: 0x000168C4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isInPreAnimationState)
			{
				foreach (Widget widget in base.Children)
				{
					if (widget is ValueBasedVisibilityWidget)
					{
						widget.IsVisible = false;
					}
				}
			}
			bool flag = false;
			if (this._isAnimationStarted && base.EventManager.Time - this._animationStartTime < this.AnimationDuration)
			{
				float num = (base.EventManager.Time - this._animationStartTime) / this.AnimationDuration;
				this._rewardIconButton.SuggestedWidth = Mathf.Lerp(this._buttonAnimationStartWidth, this._buttonAnimationEndWidth, num);
				this._rewardIconButton.SuggestedHeight = Mathf.Lerp(this._buttonAnimationStartHeight, this._buttonAnimationEndHeight, num);
				this._rewardIcon.SuggestedWidth = Mathf.Lerp(this._iconAnimationStartWidget, this._iconAnimationEndWidth, num);
				this._rewardIcon.SuggestedHeight = Mathf.Lerp(this._iconAnimationStartHeight, this._iconAnimationEndHeight, num);
				this._rewardIconButton.SetGlobalAlphaRecursively(Mathf.Lerp(0f, 1f, num));
				this._rewardIcon.SetGlobalAlphaRecursively(Mathf.Lerp(0f, 1f, num));
				this._rewardToShow.IsVisible = true;
				flag = true;
			}
			if (!this._isTextAnimationStarted && this._isAnimationStarted && base.EventManager.Time - this._animationStartTime >= this.AnimationDuration)
			{
				this._textAnimationStartTime = base.EventManager.Time;
				this._isTextAnimationStarted = true;
			}
			if (this._isTextAnimationStarted && base.EventManager.Time - this._textAnimationStartTime < this.TextRevealAnimationDuration)
			{
				float num2 = (base.EventManager.Time - this._textAnimationStartTime) / this.TextRevealAnimationDuration;
				this._rewardTextDescription.SetGlobalAlphaRecursively(Mathf.Lerp(0f, 1f, num2));
				flag = true;
			}
			if (this._isAnimationStarted && this._isTextAnimationStarted && !flag)
			{
				this.EndAnimation();
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x000188E0 File Offset: 0x00016AE0
		public void StartAnimation()
		{
			this._isInPreAnimationState = false;
			foreach (Widget widget in base.Children)
			{
				ValueBasedVisibilityWidget valueBasedVisibilityWidget;
				if ((valueBasedVisibilityWidget = widget as ValueBasedVisibilityWidget) != null && valueBasedVisibilityWidget.IndexToWatch == valueBasedVisibilityWidget.IndexToBeVisible)
				{
					this._rewardToShow = valueBasedVisibilityWidget;
					this._rewardIconButton = widget.Children[0].Children[0] as ButtonWidget;
					this._rewardIcon = this._rewardIconButton.Children[0];
					this._rewardTextDescription = widget.Children[0].Children[1] as TextWidget;
					this._buttonAnimationStartWidth = this._rewardIconButton.SuggestedWidth * this.AnimationInitialScaleMultiplier;
					this._buttonAnimationStartHeight = this._rewardIconButton.SuggestedHeight * this.AnimationInitialScaleMultiplier;
					this._buttonAnimationEndWidth = this._rewardIconButton.SuggestedWidth;
					this._buttonAnimationEndHeight = this._rewardIconButton.SuggestedHeight;
					this._iconAnimationStartWidget = this._rewardIcon.SuggestedWidth * this.AnimationInitialScaleMultiplier;
					this._iconAnimationStartHeight = this._rewardIcon.SuggestedHeight * this.AnimationInitialScaleMultiplier;
					this._iconAnimationEndWidth = this._rewardIcon.SuggestedWidth;
					this._iconAnimationEndHeight = this._rewardIcon.SuggestedHeight;
					this._rewardTextDescription.SetGlobalAlphaRecursively(0f);
				}
			}
			this._isAnimationStarted = true;
			this._animationStartTime = base.EventManager.Time;
			base.Context.TwoDimensionContext.PlaySound("inventory/perk");
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00018AA8 File Offset: 0x00016CA8
		public void StartPreAnimation()
		{
			this._isInPreAnimationState = true;
			base.IsVisible = true;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00018AB8 File Offset: 0x00016CB8
		public void EndAnimation()
		{
			this._rewardIconButton.SetGlobalAlphaRecursively(1f);
			this._rewardIcon.SetGlobalAlphaRecursively(1f);
			this._rewardTextDescription.SetGlobalAlphaRecursively(1f);
			this._rewardIconButton.SuggestedWidth = this._buttonAnimationEndWidth;
			this._rewardIconButton.SuggestedHeight = this._buttonAnimationEndHeight;
			this._rewardIcon.SuggestedWidth = this._iconAnimationEndWidth;
			this._rewardIcon.SuggestedHeight = this._iconAnimationEndHeight;
			this._isAnimationStarted = false;
			this._isTextAnimationStarted = false;
		}

		// Token: 0x040003CF RID: 975
		private const string _rewardImpactSoundEventName = "inventory/perk";

		// Token: 0x040003D0 RID: 976
		private float _buttonAnimationStartWidth;

		// Token: 0x040003D1 RID: 977
		private float _buttonAnimationStartHeight;

		// Token: 0x040003D2 RID: 978
		private float _buttonAnimationEndWidth;

		// Token: 0x040003D3 RID: 979
		private float _buttonAnimationEndHeight;

		// Token: 0x040003D4 RID: 980
		private float _iconAnimationStartWidget;

		// Token: 0x040003D5 RID: 981
		private float _iconAnimationStartHeight;

		// Token: 0x040003D6 RID: 982
		private float _iconAnimationEndWidth;

		// Token: 0x040003D7 RID: 983
		private float _iconAnimationEndHeight;

		// Token: 0x040003DB RID: 987
		private ButtonWidget _rewardIconButton;

		// Token: 0x040003DC RID: 988
		private Widget _rewardIcon;

		// Token: 0x040003DD RID: 989
		private TextWidget _rewardTextDescription;

		// Token: 0x040003DE RID: 990
		private ValueBasedVisibilityWidget _rewardToShow;

		// Token: 0x040003DF RID: 991
		private bool _isAnimationStarted;

		// Token: 0x040003E0 RID: 992
		private bool _isTextAnimationStarted;

		// Token: 0x040003E1 RID: 993
		private bool _isInPreAnimationState;

		// Token: 0x040003E2 RID: 994
		private float _animationStartTime;

		// Token: 0x040003E3 RID: 995
		private float _textAnimationStartTime;
	}
}
