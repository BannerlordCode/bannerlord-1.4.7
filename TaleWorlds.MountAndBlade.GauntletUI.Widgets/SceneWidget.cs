using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003B RID: 59
	public class SceneWidget : TextureWidget
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000ABFD File Offset: 0x00008DFD
		private bool _isClickToContinueActive
		{
			get
			{
				return this._clickToContinueStartTime != -1f && base.EventManager.Time - this._clickToContinueStartTime >= this._clickToContinueDelayInSeconds;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000AC2B File Offset: 0x00008E2B
		private float _clickToContinueDelayInSeconds
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000AC34 File Offset: 0x00008E34
		public SceneWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "SceneTextureProvider";
			this._isRenderRequestedPreviousFrame = true;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000AC88 File Offset: 0x00008E88
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.Scene != null && !this._initialized)
			{
				this.DetermineInitContinueState();
				this._initialized = true;
			}
			if (this.Scene != null && !this.IsReady)
			{
				bool? textureProviderProperty = base.GetTextureProviderProperty<bool>("IsReady");
				bool flag = true;
				this.IsReady = (textureProviderProperty.GetValueOrDefault() == flag) & (textureProviderProperty != null);
				if (this.IsReady)
				{
					this._fadeInTimer = this.FadeInDuration;
					this._isFadingIn = true;
				}
			}
			if (this._isInClickToContinueState)
			{
				if (this._isClickToContinueActive)
				{
					this.ClickToContinueTextWidget.SetGlobalAlphaRecursively(Mathf.Lerp(this.ClickToContinueTextWidget.ReadOnlyBrush.GlobalAlphaFactor, 1f, dt * 10f));
					if (!this._prevIsClickToContinueActive)
					{
						this.ClickToContinueTextWidget.BrushRenderer.RestartAnimation();
					}
				}
				this.CancelButton.SetGlobalAlphaRecursively(Mathf.Lerp(this.CancelButton.ReadOnlyBrush.GlobalAlphaFactor, 0f, dt * 10f));
				this.AffirmativeButton.SetGlobalAlphaRecursively(Mathf.Lerp(this.AffirmativeButton.ReadOnlyBrush.GlobalAlphaFactor, 0f, dt * 10f));
			}
			else
			{
				this.ClickToContinueTextWidget.SetGlobalAlphaRecursively(Mathf.Lerp(this.ClickToContinueTextWidget.ReadOnlyBrush.GlobalAlphaFactor, 0f, dt * 10f));
				this.CancelButton.SetGlobalAlphaRecursively(Mathf.Lerp(this.CancelButton.ReadOnlyBrush.GlobalAlphaFactor, (float)(this.IsCancelShown ? 1 : 0), dt * 10f));
				this.AffirmativeButton.SetGlobalAlphaRecursively(Mathf.Lerp(this.AffirmativeButton.ReadOnlyBrush.GlobalAlphaFactor, (float)(this.IsOkShown ? 1 : 0), dt * 10f));
			}
			this.UpdateVisibilityOfWidgetBasedOnAlpha(this.ClickToContinueTextWidget);
			this.UpdateVisibilityOfWidgetBasedOnAlpha(this.CancelButton);
			this.UpdateVisibilityOfWidgetBasedOnAlpha(this.AffirmativeButton);
			this.HandleTitleTextChange();
			this.FadeImageWidget.AlphaFactor = (this.IsReady ? this.EndProgress : 1f);
			this.PreparingVisualWidget.IsVisible = !this.IsReady;
			this._prevIsClickToContinueActive = this._isClickToContinueActive;
			if (this.FadeInDuration == 0f || this._fadeInTimer < 0f)
			{
				this._isFadingIn = false;
				return;
			}
			this._fadeInTimer -= dt;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000AEEC File Offset: 0x000090EC
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			base.OnRender(twoDimensionContext, drawContext);
			if (this._isFadingIn)
			{
				float num = Mathf.Clamp(this._fadeInTimer / this.FadeInDuration, 0f, 1f);
				num = AnimationInterpolation.Ease(AnimationInterpolation.Type.EaseOut, AnimationInterpolation.Function.Quint, num);
				this.RenderFadeOverlay(twoDimensionContext, drawContext, num);
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000AF38 File Offset: 0x00009138
		private void RenderFadeOverlay(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext, float ratio)
		{
			if (this._cachedOverlaySprite == null)
			{
				this._cachedOverlaySprite = base.Context.SpriteData.GetSprite("BlankWhiteSquare_9");
			}
			if (this._cachedOverlaySprite == null)
			{
				Debug.FailedAssert("Failed to find overlay sprite for scene fade", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\SceneWidget.cs", "RenderFadeOverlay", 118);
				return;
			}
			Texture texture = this._cachedOverlaySprite.Texture;
			SimpleMaterial simpleMaterial = drawContext.CreateSimpleMaterial();
			simpleMaterial.Texture = texture;
			simpleMaterial.Color = Color.FromUint(4278190080U);
			simpleMaterial.ColorFactor = 1f;
			simpleMaterial.AlphaFactor = ratio;
			simpleMaterial.HueFactor = 0f;
			simpleMaterial.SaturationFactor = 0f;
			simpleMaterial.ValueFactor = 0f;
			drawContext.DrawSprite(this._cachedOverlaySprite, simpleMaterial, in this.AreaRect, base._scaleToUse);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000AFFD File Offset: 0x000091FD
		private void UpdateVisibilityOfWidgetBasedOnAlpha(BrushWidget widget)
		{
			widget.IsVisible = !widget.ReadOnlyBrush.GlobalAlphaFactor.ApproximatelyEqualsTo(0f, 0.01f);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000B024 File Offset: 0x00009224
		protected override void OnMouseReleased(bool isFromInput)
		{
			base.OnMouseReleased(isFromInput);
			EventManager eventManager = base.EventManager;
			Vector2 mousePosition = base.EventManager.MousePosition;
			if (!eventManager.AreaRectangle.IsPointInside(in mousePosition))
			{
				return;
			}
			if (this._isClickToContinueActive && isFromInput)
			{
				base.EventFired("Close", Array.Empty<object>());
				this.ResetStates();
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000B079 File Offset: 0x00009279
		private void OnAnyActionButtonClick()
		{
			this._isInClickToContinueState = true;
			base.DoNotAcceptEvents = false;
			base.DoNotPassEventsToChildren = true;
			this.ClickToContinueTextWidget.BrushRenderer.RestartAnimation();
			this._clickToContinueStartTime = base.EventManager.Time;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000B0B4 File Offset: 0x000092B4
		private void ResetStates()
		{
			this._isInClickToContinueState = false;
			base.DoNotAcceptEvents = true;
			base.DoNotPassEventsToChildren = false;
			this._initialized = false;
			this.IsReady = false;
			this._titleChangeStartTime = -1f;
			this._currentTitleTextToUpdateTo = string.Empty;
			this.TitleTextWidget.SetAlpha(1f);
			this._clickToContinueStartTime = base.EventManager.Time;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000B11B File Offset: 0x0000931B
		private void OnAffirmativeButtonClick(Widget obj)
		{
			this.SetNewTitleText(this.AffirmativeTitleText);
			this.OnAnyActionButtonClick();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000B12F File Offset: 0x0000932F
		private void OnCancelButtonClick(Widget obj)
		{
			this.SetNewTitleText(this.NegativeTitleText);
			this.OnAnyActionButtonClick();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000B143 File Offset: 0x00009343
		private void SetNewTitleText(string newText)
		{
			if (!string.IsNullOrEmpty(newText))
			{
				this._currentTitleTextToUpdateTo = newText;
				this._titleChangeStartTime = base.EventManager.Time;
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000B168 File Offset: 0x00009368
		private void DetermineInitContinueState()
		{
			this.CancelButton.IsVisible = this.IsCancelShown;
			this.CancelButton.SetGlobalAlphaRecursively((float)(this.IsCancelShown ? 1 : 0));
			this.AffirmativeButton.IsVisible = this.IsOkShown;
			this.AffirmativeButton.SetGlobalAlphaRecursively((float)(this.IsOkShown ? 1 : 0));
			this.ClickToContinueTextWidget.SetGlobalAlphaRecursively(0f);
			this._isInClickToContinueState = !this.IsCancelShown && !this.IsOkShown;
			if (this._isInClickToContinueState)
			{
				this._clickToContinueStartTime = base.EventManager.Time;
			}
			base.DoNotAcceptEvents = !this._isInClickToContinueState;
			base.DoNotPassEventsToChildren = this._isInClickToContinueState;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000B228 File Offset: 0x00009428
		private void HandleTitleTextChange()
		{
			if (this._titleChangeStartTime != -1f)
			{
				if (!string.IsNullOrEmpty(this._currentTitleTextToUpdateTo) && this.TitleTextWidget != null && base.EventManager.Time - this._titleChangeStartTime < this._titleChangeTotalTimeInSeconds)
				{
					if (base.EventManager.Time - this._titleChangeStartTime >= this._titleChangeTotalTimeInSeconds / 2f)
					{
						this.TitleTextWidget.Text = this._currentTitleTextToUpdateTo;
					}
					float num = 1f - MathF.PingPong(0f, 1f, base.EventManager.Time - this._titleChangeStartTime);
					this.TitleTextWidget.SetAlpha(num);
					return;
				}
				this.TitleTextWidget.SetAlpha(1f);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0000B2F0 File Offset: 0x000094F0
		// (set) Token: 0x0600036D RID: 877 RVA: 0x0000B2F8 File Offset: 0x000094F8
		[Editor(false)]
		public object Scene
		{
			get
			{
				return this._scene;
			}
			set
			{
				if (value != this._scene)
				{
					this._scene = value;
					base.OnPropertyChanged<object>(value, "Scene");
					base.SetTextureProviderProperty("Scene", value);
					if (value != null)
					{
						this._isTargetSizeDirty = true;
						this._clickToContinueStartTime = base.EventManager.Time;
						this.ResetStates();
					}
				}
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0000B34E File Offset: 0x0000954E
		// (set) Token: 0x0600036F RID: 879 RVA: 0x0000B356 File Offset: 0x00009556
		[Editor(false)]
		public ButtonWidget AffirmativeButton
		{
			get
			{
				return this._affirmativeButton;
			}
			set
			{
				if (value != this._affirmativeButton)
				{
					this._affirmativeButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "AffirmativeButton");
					ButtonWidget affirmativeButton = this._affirmativeButton;
					if (affirmativeButton == null)
					{
						return;
					}
					affirmativeButton.ClickEventHandlers.Add(new Action<Widget>(this.OnAffirmativeButtonClick));
				}
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000B395 File Offset: 0x00009595
		// (set) Token: 0x06000371 RID: 881 RVA: 0x0000B39D File Offset: 0x0000959D
		[Editor(false)]
		public ButtonWidget CancelButton
		{
			get
			{
				return this._cancelButton;
			}
			set
			{
				if (value != this._cancelButton)
				{
					this._cancelButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "CancelButton");
					ButtonWidget cancelButton = this._cancelButton;
					if (cancelButton == null)
					{
						return;
					}
					cancelButton.ClickEventHandlers.Add(new Action<Widget>(this.OnCancelButtonClick));
				}
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000B3DC File Offset: 0x000095DC
		// (set) Token: 0x06000373 RID: 883 RVA: 0x0000B3E4 File Offset: 0x000095E4
		[Editor(false)]
		public RichTextWidget ClickToContinueTextWidget
		{
			get
			{
				return this._clickToContinueTextWidget;
			}
			set
			{
				if (value != this._clickToContinueTextWidget)
				{
					this._clickToContinueTextWidget = value;
					base.OnPropertyChanged<RichTextWidget>(value, "ClickToContinueTextWidget");
				}
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000B402 File Offset: 0x00009602
		// (set) Token: 0x06000375 RID: 885 RVA: 0x0000B40A File Offset: 0x0000960A
		[Editor(false)]
		public TextWidget TitleTextWidget
		{
			get
			{
				return this._titleTextWidget;
			}
			set
			{
				if (value != this._titleTextWidget)
				{
					this._titleTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "TitleTextWidget");
				}
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0000B428 File Offset: 0x00009628
		// (set) Token: 0x06000377 RID: 887 RVA: 0x0000B430 File Offset: 0x00009630
		[Editor(false)]
		public Widget FadeImageWidget
		{
			get
			{
				return this._fadeImageWidget;
			}
			set
			{
				if (value != this._fadeImageWidget)
				{
					this._fadeImageWidget = value;
					base.OnPropertyChanged<Widget>(value, "FadeImageWidget");
				}
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0000B44E File Offset: 0x0000964E
		// (set) Token: 0x06000379 RID: 889 RVA: 0x0000B456 File Offset: 0x00009656
		[Editor(false)]
		public Widget PreparingVisualWidget
		{
			get
			{
				return this._preparingVisualWidget;
			}
			set
			{
				if (value != this._preparingVisualWidget)
				{
					this._preparingVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "PreparingVisualWidget");
				}
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000B474 File Offset: 0x00009674
		// (set) Token: 0x0600037B RID: 891 RVA: 0x0000B47C File Offset: 0x0000967C
		[Editor(false)]
		public float EndProgress
		{
			get
			{
				return this._endProgress;
			}
			set
			{
				if (value != this._endProgress)
				{
					this._endProgress = value;
					base.OnPropertyChanged(value, "EndProgress");
				}
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000B49A File Offset: 0x0000969A
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0000B4A2 File Offset: 0x000096A2
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

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0000B4C0 File Offset: 0x000096C0
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0000B4C8 File Offset: 0x000096C8
		[Editor(false)]
		public bool IsOkShown
		{
			get
			{
				return this._isOkShown;
			}
			set
			{
				if (value != this._isOkShown)
				{
					this._isOkShown = value;
					base.OnPropertyChanged(value, "IsOkShown");
					this.DetermineInitContinueState();
				}
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000B4EC File Offset: 0x000096EC
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000B4F4 File Offset: 0x000096F4
		[Editor(false)]
		public bool IsCancelShown
		{
			get
			{
				return this._isCancelShown;
			}
			set
			{
				if (value != this._isCancelShown)
				{
					this._isCancelShown = value;
					base.OnPropertyChanged(value, "IsCancelShown");
					this.DetermineInitContinueState();
				}
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000B518 File Offset: 0x00009718
		// (set) Token: 0x06000383 RID: 899 RVA: 0x0000B520 File Offset: 0x00009720
		[Editor(false)]
		public bool IsReady
		{
			get
			{
				return this._isReady;
			}
			set
			{
				if (value != this._isReady)
				{
					this._isReady = value;
					base.OnPropertyChanged(value, "IsReady");
				}
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0000B53E File Offset: 0x0000973E
		// (set) Token: 0x06000385 RID: 901 RVA: 0x0000B546 File Offset: 0x00009746
		[Editor(false)]
		public string AffirmativeTitleText
		{
			get
			{
				return this._affirmativeTitleText;
			}
			set
			{
				if (value != this._affirmativeTitleText)
				{
					this._affirmativeTitleText = value;
					base.OnPropertyChanged<string>(value, "AffirmativeTitleText");
				}
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000B569 File Offset: 0x00009769
		// (set) Token: 0x06000387 RID: 903 RVA: 0x0000B571 File Offset: 0x00009771
		[Editor(false)]
		public string NegativeTitleText
		{
			get
			{
				return this._negativeTitleText;
			}
			set
			{
				if (value != this._negativeTitleText)
				{
					this._negativeTitleText = value;
					base.OnPropertyChanged<string>(value, "NegativeTitleText");
				}
			}
		}

		// Token: 0x0400015E RID: 350
		private bool _isInClickToContinueState;

		// Token: 0x0400015F RID: 351
		private bool _prevIsClickToContinueActive;

		// Token: 0x04000160 RID: 352
		private bool _initialized;

		// Token: 0x04000161 RID: 353
		private float _clickToContinueStartTime = -1f;

		// Token: 0x04000162 RID: 354
		private string _currentTitleTextToUpdateTo = string.Empty;

		// Token: 0x04000163 RID: 355
		private float _titleChangeStartTime = -1f;

		// Token: 0x04000164 RID: 356
		private float _titleChangeTotalTimeInSeconds = 2f;

		// Token: 0x04000165 RID: 357
		private float _fadeInTimer;

		// Token: 0x04000166 RID: 358
		private bool _isFadingIn;

		// Token: 0x04000167 RID: 359
		private Sprite _cachedOverlaySprite;

		// Token: 0x04000168 RID: 360
		private object _scene;

		// Token: 0x04000169 RID: 361
		private ButtonWidget _cancelButton;

		// Token: 0x0400016A RID: 362
		private ButtonWidget _affirmativeButton;

		// Token: 0x0400016B RID: 363
		private RichTextWidget _clickToContinueTextWidget;

		// Token: 0x0400016C RID: 364
		private Widget _fadeImageWidget;

		// Token: 0x0400016D RID: 365
		private TextWidget _titleTextWidget;

		// Token: 0x0400016E RID: 366
		private float _endProgress;

		// Token: 0x0400016F RID: 367
		private float _fadeInDuration;

		// Token: 0x04000170 RID: 368
		private string _affirmativeTitleText;

		// Token: 0x04000171 RID: 369
		private string _negativeTitleText;

		// Token: 0x04000172 RID: 370
		private Widget _preparingVisualWidget;

		// Token: 0x04000173 RID: 371
		private bool _isCancelShown;

		// Token: 0x04000174 RID: 372
		private bool _isOkShown;

		// Token: 0x04000175 RID: 373
		private bool _isReady;
	}
}
