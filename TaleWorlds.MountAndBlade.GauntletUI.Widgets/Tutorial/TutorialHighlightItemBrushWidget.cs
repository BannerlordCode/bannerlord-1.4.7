using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004B RID: 75
	public class TutorialHighlightItemBrushWidget : BrushWidget
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x0000CF3F File Offset: 0x0000B13F
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x0000CF47 File Offset: 0x0000B147
		public Widget CustomSizeSyncTarget { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0000CF50 File Offset: 0x0000B150
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x0000CF58 File Offset: 0x0000B158
		public bool DoNotOverrideWidth { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x0000CF61 File Offset: 0x0000B161
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x0000CF69 File Offset: 0x0000B169
		public bool DoNotOverrideHeight { get; set; }

		// Token: 0x06000423 RID: 1059 RVA: 0x0000CF72 File Offset: 0x0000B172
		public TutorialHighlightItemBrushWidget(UIContext context)
			: base(context)
		{
			base.UseGlobalTimeForAnimation = true;
			base.DoNotAcceptEvents = true;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000CF8C File Offset: 0x0000B18C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._animState == TutorialHighlightItemBrushWidget.AnimState.Start)
			{
				this._animState = TutorialHighlightItemBrushWidget.AnimState.FirstFrame;
			}
			else if (this._animState == TutorialHighlightItemBrushWidget.AnimState.FirstFrame)
			{
				if (base.BrushRenderer.Brush == null)
				{
					this._animState = TutorialHighlightItemBrushWidget.AnimState.Start;
				}
				else
				{
					this._animState = TutorialHighlightItemBrushWidget.AnimState.Playing;
					base.BrushRenderer.RestartAnimation();
				}
			}
			if (this.IsHighlightEnabled && this._isDisabled)
			{
				this._isDisabled = false;
				this.SetState("Default");
			}
			else if (!this.IsHighlightEnabled && !this._isDisabled)
			{
				this.SetState("Disabled");
				this._isDisabled = true;
			}
			this.UpdateTargetSize();
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000D030 File Offset: 0x0000B230
		private void UpdateTargetSize()
		{
			Widget widget = this.CustomSizeSyncTarget ?? base.ParentWidget;
			if (widget == null)
			{
				return;
			}
			bool flag;
			if (widget.HeightSizePolicy == SizePolicy.CoverChildren || widget.WidthSizePolicy == SizePolicy.CoverChildren)
			{
				if (!this.DoNotOverrideWidth)
				{
					base.WidthSizePolicy = SizePolicy.Fixed;
				}
				if (!this.DoNotOverrideHeight)
				{
					base.HeightSizePolicy = SizePolicy.Fixed;
				}
				flag = true;
			}
			else
			{
				base.WidthSizePolicy = SizePolicy.StretchToParent;
				base.HeightSizePolicy = SizePolicy.StretchToParent;
				flag = false;
			}
			if (flag && widget.Size.X > 1f && widget.Size.Y > 1f)
			{
				if (!this.DoNotOverrideWidth)
				{
					base.ScaledSuggestedWidth = widget.Size.X - 1f;
				}
				if (!this.DoNotOverrideHeight)
				{
					base.ScaledSuggestedHeight = widget.Size.Y - 1f;
				}
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000D0FB File Offset: 0x0000B2FB
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x0000D104 File Offset: 0x0000B304
		[Editor(false)]
		public bool IsHighlightEnabled
		{
			get
			{
				return this._isHighlightEnabled;
			}
			set
			{
				if (this._isHighlightEnabled != value)
				{
					this._isHighlightEnabled = value;
					base.OnPropertyChanged(value, "IsHighlightEnabled");
					if (this.IsHighlightEnabled)
					{
						this._animState = TutorialHighlightItemBrushWidget.AnimState.Start;
					}
					base.IsVisible = value;
					TaleWorlds.GauntletUI.EventManager.UIEventManager.TriggerEvent<TutorialHighlightItemBrushWidget.HighlightElementToggledEvent>(new TutorialHighlightItemBrushWidget.HighlightElementToggledEvent(value, value ? this : null));
				}
			}
		}

		// Token: 0x040001BC RID: 444
		private TutorialHighlightItemBrushWidget.AnimState _animState;

		// Token: 0x040001BD RID: 445
		private bool _isDisabled;

		// Token: 0x040001BE RID: 446
		private bool _isHighlightEnabled;

		// Token: 0x020001A4 RID: 420
		public enum AnimState
		{
			// Token: 0x040009BC RID: 2492
			Idle,
			// Token: 0x040009BD RID: 2493
			Start,
			// Token: 0x040009BE RID: 2494
			FirstFrame,
			// Token: 0x040009BF RID: 2495
			Playing
		}

		// Token: 0x020001A5 RID: 421
		public class HighlightElementToggledEvent : EventBase
		{
			// Token: 0x1700075F RID: 1887
			// (get) Token: 0x060014FD RID: 5373 RVA: 0x000396B2 File Offset: 0x000378B2
			// (set) Token: 0x060014FE RID: 5374 RVA: 0x000396BA File Offset: 0x000378BA
			public bool IsEnabled { get; private set; }

			// Token: 0x17000760 RID: 1888
			// (get) Token: 0x060014FF RID: 5375 RVA: 0x000396C3 File Offset: 0x000378C3
			// (set) Token: 0x06001500 RID: 5376 RVA: 0x000396CB File Offset: 0x000378CB
			public TutorialHighlightItemBrushWidget HighlightFrameWidget { get; private set; }

			// Token: 0x06001501 RID: 5377 RVA: 0x000396D4 File Offset: 0x000378D4
			public HighlightElementToggledEvent(bool isEnabled, TutorialHighlightItemBrushWidget highlightFrameWidget)
			{
				this.IsEnabled = isEnabled;
				this.HighlightFrameWidget = highlightFrameWidget;
			}
		}
	}
}
