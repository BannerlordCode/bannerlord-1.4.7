using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Quest
{
	// Token: 0x0200005F RID: 95
	public class QuestProgressVisualWidget : Widget
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0000FBF1 File Offset: 0x0000DDF1
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x0000FBF9 File Offset: 0x0000DDF9
		public Widget BarWidget { get; set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0000FC02 File Offset: 0x0000DE02
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x0000FC0A File Offset: 0x0000DE0A
		public Widget SliderWidget { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x0000FC13 File Offset: 0x0000DE13
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x0000FC1B File Offset: 0x0000DE1B
		public Widget CheckboxVisualWidget { get; set; }

		// Token: 0x0600052C RID: 1324 RVA: 0x0000FC24 File Offset: 0x0000DE24
		public QuestProgressVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000FC30 File Offset: 0x0000DE30
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				bool flag = this.CurrentProgress >= this.TargetProgress;
				base.IsVisible = !flag && this.IsValid;
				this.CheckboxVisualWidget.IsVisible = flag && this.IsValid;
				this.BarWidget.IsVisible = false;
				this.SliderWidget.IsVisible = false;
				if (base.IsVisible)
				{
					if (this.TargetProgress < 20)
					{
						for (int i = 0; i < this.TargetProgress; i++)
						{
							BrushWidget brushWidget = new BrushWidget(base.Context)
							{
								WidthSizePolicy = SizePolicy.Fixed,
								SuggestedWidth = this.ProgressStoneWidth,
								HeightSizePolicy = SizePolicy.Fixed,
								SuggestedHeight = this.ProgressStoneHeight,
								MarginRight = (float)this.HorizontalSpacingBetweenStones / 2f,
								MarginLeft = (float)this.HorizontalSpacingBetweenStones / 2f,
								IsEnabled = false
							};
							if (i < this.CurrentProgress)
							{
								brushWidget.Brush = base.Context.GetBrush("StageTask.ProgressStone");
								brushWidget.Brush.AlphaFactor = 0.8f;
							}
							this.BarWidget.AddChild(brushWidget);
						}
						this.BarWidget.IsVisible = true;
					}
					else if (this.TargetProgress >= 20)
					{
						this.SliderWidget.IsVisible = true;
						this.SliderWidget.IsDisabled = true;
					}
				}
				this._initialized = true;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0000FDA2 File Offset: 0x0000DFA2
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x0000FDAA File Offset: 0x0000DFAA
		public bool IsValid
		{
			get
			{
				return this._isValid;
			}
			set
			{
				if (this._isValid != value)
				{
					this._isValid = value;
				}
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0000FDBC File Offset: 0x0000DFBC
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x0000FDC4 File Offset: 0x0000DFC4
		public float ProgressStoneWidth
		{
			get
			{
				return this._progressStoneWidth;
			}
			set
			{
				if (this._progressStoneWidth != value)
				{
					this._progressStoneWidth = value;
				}
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x0000FDD6 File Offset: 0x0000DFD6
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x0000FDDE File Offset: 0x0000DFDE
		public float ProgressStoneHeight
		{
			get
			{
				return this._progressStoneHeight;
			}
			set
			{
				if (this._progressStoneHeight != value)
				{
					this._progressStoneHeight = value;
				}
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0000FDF0 File Offset: 0x0000DFF0
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x0000FDF8 File Offset: 0x0000DFF8
		public int CurrentProgress
		{
			get
			{
				return this._currentProgress;
			}
			set
			{
				if (this._currentProgress != value)
				{
					this._currentProgress = value;
				}
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0000FE0A File Offset: 0x0000E00A
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x0000FE12 File Offset: 0x0000E012
		public int TargetProgress
		{
			get
			{
				return this._targetProgress;
			}
			set
			{
				if (this._targetProgress != value)
				{
					this._targetProgress = value;
				}
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0000FE24 File Offset: 0x0000E024
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x0000FE2C File Offset: 0x0000E02C
		public int HorizontalSpacingBetweenStones
		{
			get
			{
				return this._horizontalSpacingBetweenStones;
			}
			set
			{
				if (this._horizontalSpacingBetweenStones != value)
				{
					this._horizontalSpacingBetweenStones = value;
				}
			}
		}

		// Token: 0x04000236 RID: 566
		private bool _initialized;

		// Token: 0x0400023A RID: 570
		private int _currentProgress;

		// Token: 0x0400023B RID: 571
		private int _targetProgress;

		// Token: 0x0400023C RID: 572
		private float _progressStoneWidth;

		// Token: 0x0400023D RID: 573
		private float _progressStoneHeight;

		// Token: 0x0400023E RID: 574
		private int _horizontalSpacingBetweenStones;

		// Token: 0x0400023F RID: 575
		private bool _isValid;
	}
}
