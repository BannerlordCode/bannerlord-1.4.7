using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000091 RID: 145
	public class MultiplayerScoreboardAnimatedFillBarWidget : FillBarWidget
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060007DB RID: 2011 RVA: 0x00016B30 File Offset: 0x00014D30
		// (remove) Token: 0x060007DC RID: 2012 RVA: 0x00016B68 File Offset: 0x00014D68
		public event MultiplayerScoreboardAnimatedFillBarWidget.FullFillFinishedHandler OnFullFillFinished;

		// Token: 0x060007DD RID: 2013 RVA: 0x00016B9D File Offset: 0x00014D9D
		public MultiplayerScoreboardAnimatedFillBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00016BBC File Offset: 0x00014DBC
		public void StartAnimation(float animationDelay = 0f)
		{
			if (base.FillWidget == null || base.ChangeWidget == null || MathF.Abs(this.AnimationFillSpeed) <= 1E-45f)
			{
				return;
			}
			this.AnimationDelay = animationDelay;
			this._ratioOfChangePerTick = this.AnimationFillSpeed;
			this._isXPIncreasing = this.TimesOfFullFill > 0 || (this.TimesOfFullFill == 0 && base.CurrentAmountAsFloat > base.InitialAmountAsFloat);
			if (!this._isStarted)
			{
				base.Context.TwoDimensionContext.CreateSoundEvent(this._xpBarSoundEventName);
				base.Context.TwoDimensionContext.PlaySoundEvent(this._xpBarSoundEventName);
			}
			this._isStarted = true;
			this.CalculateTargetValues();
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00016C6C File Offset: 0x00014E6C
		public void Reset()
		{
			this._normalFillFromValue = 0f;
			this._normalFillToValue = 0f;
			this._highlightedFillFromValue = 0f;
			this._highlightedFillToValue = 0f;
			this._ratioOfChangePerTick = this.AnimationFillSpeed;
			this._animationDelayTimer = 0f;
			this._lerpRatioPerTick = 0f;
			this.AnimationDelay = 0f;
			this._isStarted = false;
			this._shouldStopLerping = false;
			this._isFirstStepCalculated = false;
			this._isProgressCompleted = false;
			base.Context.TwoDimensionContext.StopAndRemoveSoundEvent(this._xpBarSoundEventName);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00016D04 File Offset: 0x00014F04
		protected override void OnUpdate(float dt)
		{
			if (!this._isStarted || this._isProgressCompleted)
			{
				return;
			}
			this._animationDelayTimer += dt;
			if (this._animationDelayTimer >= this.AnimationDelay)
			{
				this._lerpRatioPerTick += dt * this._ratioOfChangePerTick;
				this._lerpRatioPerTick = Mathf.Clamp(this._lerpRatioPerTick, 0f, 1f);
				if (this._lerpRatioPerTick >= 1f && !this._shouldStopLerping)
				{
					this._lerpRatioPerTick = 0f;
					this.CalculateTargetValues();
					return;
				}
				if (this._lerpRatioPerTick >= 1f)
				{
					this._isProgressCompleted = true;
				}
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00016DAC File Offset: 0x00014FAC
		protected override void OnLateUpdate(float dt)
		{
			if (base.FillWidget != null)
			{
				this.ChangeFillAmountOfWidget(base.FillWidget, this._normalFillFromValue, this._normalFillToValue, this._lerpRatioPerTick);
			}
			if (base.ChangeWidget != null)
			{
				this.ChangeFillAmountOfWidget(base.ChangeWidget, this._highlightedFillFromValue, this._highlightedFillToValue, this._lerpRatioPerTick);
			}
			if (base.DividerWidget != null)
			{
				if (this._isXPIncreasing && base.ChangeWidget != null)
				{
					base.DividerWidget.ScaledPositionXOffset = base.ChangeWidget.ScaledSuggestedWidth;
					base.DividerWidget.Color = Color.FromUint(uint.MaxValue);
				}
				else if (base.FillWidget != null)
				{
					base.DividerWidget.ScaledPositionXOffset = base.FillWidget.ScaledSuggestedWidth;
					base.DividerWidget.Color = Color.FromUint(4293185972U);
				}
				base.DividerWidget.ScaledPositionXOffset -= base.DividerWidget.Size.X;
			}
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00016EA0 File Offset: 0x000150A0
		private void CalculateTargetValues()
		{
			this.SetRegularFromValues(0f, false);
			bool flag = !this._isFirstStepCalculated;
			if (!this._isFirstStepCalculated)
			{
				float num = Mathf.Clamp(Mathf.Clamp(base.InitialAmountAsFloat, 0f, base.MaxAmountAsFloat) / base.MaxAmountAsFloat, 0f, 1f);
				this.SetRegularFromValues(num, true);
				this._isFirstStepCalculated = true;
			}
			this.DecideNextStep(flag);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00016F0E File Offset: 0x0001510E
		private void DecideNextStep(bool isFirstStep)
		{
			if (this.DoHaveFullFillStep())
			{
				this.FullFillStep();
			}
			else
			{
				this.LastFillStep();
			}
			if (!isFirstStep)
			{
				MultiplayerScoreboardAnimatedFillBarWidget.FullFillFinishedHandler onFullFillFinished = this.OnFullFillFinished;
				if (onFullFillFinished == null)
				{
					return;
				}
				onFullFillFinished(this._isXPIncreasing);
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00016F40 File Offset: 0x00015140
		private void LastFillStep()
		{
			float num = Mathf.Clamp(Mathf.Clamp(base.CurrentAmountAsFloat, 0f, base.MaxAmountAsFloat) / base.MaxAmountAsFloat, 0f, 1f);
			if (this._isXPIncreasing)
			{
				this._highlightedFillToValue = num;
			}
			else
			{
				this._normalFillToValue = num;
			}
			this._shouldStopLerping = true;
			base.Context.TwoDimensionContext.StopAndRemoveSoundEvent(this._xpBarSoundEventName);
			base.Context.TwoDimensionContext.PlaySound(this._xpBarStopSoundEventName);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00016FC8 File Offset: 0x000151C8
		private void FullFillStep()
		{
			if (this.DoHaveFullFillStep())
			{
				if (this._isXPIncreasing)
				{
					this._highlightedFillToValue = 1f;
				}
				else
				{
					this._normalFillToValue = 0f;
				}
				this.TimesOfFullFill -= Math.Sign(this.TimesOfFullFill);
			}
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00017018 File Offset: 0x00015218
		private void SetRegularFromValues(float inputFromValue = 0f, bool useInputFromValue = false)
		{
			float num = (useInputFromValue ? inputFromValue : ((float)(this._isXPIncreasing ? 0 : 1)));
			this._normalFillFromValue = num;
			this._highlightedFillFromValue = num;
			this.StopMovementOfFillAmountAtFromValue(!this._isXPIncreasing);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00017056 File Offset: 0x00015256
		private bool DoHaveFullFillStep()
		{
			return Math.Abs(this.TimesOfFullFill) > 0;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00017066 File Offset: 0x00015266
		private void StopMovementOfFillAmountAtFromValue(bool stopHighlightedFill)
		{
			if (stopHighlightedFill)
			{
				this._highlightedFillToValue = this._highlightedFillFromValue;
				return;
			}
			this._normalFillToValue = this._normalFillFromValue;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00017084 File Offset: 0x00015284
		private void ChangeFillAmountOfWidget(Widget fillWidget, float fromValue, float toValue, float stepSize)
		{
			float num = Mathf.Lerp(fromValue, toValue, stepSize);
			fillWidget.ScaledSuggestedWidth = num * fillWidget.ParentWidget.Size.X;
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x000170B3 File Offset: 0x000152B3
		// (set) Token: 0x060007EB RID: 2027 RVA: 0x000170BB File Offset: 0x000152BB
		[Editor(false)]
		public bool IsStartRequested
		{
			get
			{
				return this._isStartRequested;
			}
			set
			{
				if (value != this._isStartRequested)
				{
					this._isStartRequested = value;
					if (this._isStartRequested)
					{
						this.Reset();
						this.StartAnimation(0f);
					}
					base.OnPropertyChanged(value, "IsStartRequested");
				}
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x000170F2 File Offset: 0x000152F2
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x000170FA File Offset: 0x000152FA
		[Editor(false)]
		public float AnimationDelay
		{
			get
			{
				return this._animationDelay;
			}
			set
			{
				if (this._animationDelay != value)
				{
					this._animationDelay = value;
					base.OnPropertyChanged(value, "AnimationDelay");
				}
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x00017118 File Offset: 0x00015318
		// (set) Token: 0x060007EF RID: 2031 RVA: 0x00017120 File Offset: 0x00015320
		[Editor(false)]
		public float AnimationFillSpeed
		{
			get
			{
				return this._animationFillSpeed;
			}
			set
			{
				if (this._animationFillSpeed != value)
				{
					this._animationFillSpeed = value;
					base.OnPropertyChanged(value, "AnimationFillSpeed");
				}
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x0001713E File Offset: 0x0001533E
		// (set) Token: 0x060007F1 RID: 2033 RVA: 0x00017146 File Offset: 0x00015346
		[Editor(false)]
		public int TimesOfFullFill
		{
			get
			{
				return this._timesOfFullFill;
			}
			set
			{
				if (this._timesOfFullFill != value)
				{
					this._timesOfFullFill = value;
					base.OnPropertyChanged(value, "TimesOfFullFill");
				}
			}
		}

		// Token: 0x04000375 RID: 885
		private bool _isStarted;

		// Token: 0x04000376 RID: 886
		private bool _shouldStopLerping;

		// Token: 0x04000377 RID: 887
		private bool _isXPIncreasing;

		// Token: 0x04000378 RID: 888
		private bool _isFirstStepCalculated;

		// Token: 0x04000379 RID: 889
		private bool _isProgressCompleted;

		// Token: 0x0400037A RID: 890
		private float _ratioOfChangePerTick;

		// Token: 0x0400037B RID: 891
		private float _lerpRatioPerTick;

		// Token: 0x0400037C RID: 892
		private float _animationDelayTimer;

		// Token: 0x0400037D RID: 893
		private float _highlightedFillFromValue;

		// Token: 0x0400037E RID: 894
		private float _highlightedFillToValue;

		// Token: 0x0400037F RID: 895
		private float _normalFillFromValue;

		// Token: 0x04000380 RID: 896
		private float _normalFillToValue;

		// Token: 0x04000381 RID: 897
		private string _xpBarSoundEventName = "multiplayer/xpbar";

		// Token: 0x04000382 RID: 898
		private string _xpBarStopSoundEventName = "multiplayer/xpbar_stop";

		// Token: 0x04000384 RID: 900
		private bool _isStartRequested;

		// Token: 0x04000385 RID: 901
		private float _animationDelay;

		// Token: 0x04000386 RID: 902
		private float _animationFillSpeed;

		// Token: 0x04000387 RID: 903
		private int _timesOfFullFill;

		// Token: 0x020001B6 RID: 438
		// (Invoke) Token: 0x0600152C RID: 5420
		public delegate void FullFillFinishedHandler(bool isPositive);
	}
}
