using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x02000050 RID: 80
	public class TutorialScreenWidget : Widget
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x0000DE90 File Offset: 0x0000C090
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x0000DE98 File Offset: 0x0000C098
		public TutorialPanelImageWidget LeftItem { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x0000DEA1 File Offset: 0x0000C0A1
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x0000DEA9 File Offset: 0x0000C0A9
		public TutorialPanelImageWidget RightItem { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x0000DEB2 File Offset: 0x0000C0B2
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x0000DEBA File Offset: 0x0000C0BA
		public TutorialPanelImageWidget BottomItem { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0000DEC3 File Offset: 0x0000C0C3
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x0000DECB File Offset: 0x0000C0CB
		public TutorialPanelImageWidget TopItem { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0000DED4 File Offset: 0x0000C0D4
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x0000DEDC File Offset: 0x0000C0DC
		public TutorialPanelImageWidget LeftTopItem { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000DEE5 File Offset: 0x0000C0E5
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x0000DEED File Offset: 0x0000C0ED
		public TutorialPanelImageWidget RightTopItem { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0000DEF6 File Offset: 0x0000C0F6
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x0000DEFE File Offset: 0x0000C0FE
		public TutorialPanelImageWidget LeftBottomItem { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x0000DF07 File Offset: 0x0000C107
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x0000DF0F File Offset: 0x0000C10F
		public TutorialPanelImageWidget RightBottomItem { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0000DF18 File Offset: 0x0000C118
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x0000DF20 File Offset: 0x0000C120
		public TutorialPanelImageWidget CenterItem { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x0000DF29 File Offset: 0x0000C129
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x0000DF31 File Offset: 0x0000C131
		public TutorialArrowWidget ArrowWidget { get; set; }

		// Token: 0x06000467 RID: 1127 RVA: 0x0000DF3A File Offset: 0x0000C13A
		public TutorialScreenWidget(UIContext context)
			: base(context)
		{
			EventManager.UIEventManager.RegisterEvent<TutorialHighlightItemBrushWidget.HighlightElementToggledEvent>(new Action<TutorialHighlightItemBrushWidget.HighlightElementToggledEvent>(this.OnHighlightElementToggleEvent));
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000DF5C File Offset: 0x0000C15C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initalized)
			{
				this.LeftItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.RightItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.BottomItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.TopItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.LeftTopItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.RightTopItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.LeftBottomItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.RightBottomItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this.CenterItem.boolPropertyChanged += this.OnTutorialItemPropertyChanged;
				this._initalized = true;
			}
			if (this._currentActiveHighligtFrame != null && this._currentActivePanelItem != null)
			{
				Tuple<Widget, Widget> leftAndRightElements = this.GetLeftAndRightElements();
				Tuple<Widget, Widget> topAndBottomElements = this.GetTopAndBottomElements();
				float num = leftAndRightElements.Item1.GlobalPosition.X + leftAndRightElements.Item1.Size.X;
				float x = leftAndRightElements.Item2.GlobalPosition.X;
				float y = topAndBottomElements.Item1.GlobalPosition.Y;
				float y2 = topAndBottomElements.Item2.GlobalPosition.Y;
				float num2 = MathF.Abs(num - x);
				float num3 = MathF.Abs(y - y2);
				this.ArrowWidget.ScaledPositionXOffset = num;
				this.ArrowWidget.ScaledPositionYOffset = y;
				this.ArrowWidget.SetArrowProperties(num2, num3, this.GetIsArrowDirectionIsDownwards(), this.GetIsArrowDirectionIsTowardsRight());
				this.ArrowWidget.IsVisible = true;
				return;
			}
			this.ArrowWidget.IsVisible = false;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000E12C File Offset: 0x0000C32C
		private bool GetIsArrowDirectionIsDownwards()
		{
			if (this._currentActiveHighligtFrame.GlobalPosition.Y < this._currentActivePanelItem.GlobalPosition.Y)
			{
				return this._currentActiveHighligtFrame.GlobalPosition.X < this._currentActivePanelItem.GlobalPosition.X;
			}
			return this._currentActivePanelItem.GlobalPosition.X < this._currentActiveHighligtFrame.GlobalPosition.X;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000E1A0 File Offset: 0x0000C3A0
		private bool GetIsArrowDirectionIsTowardsRight()
		{
			return this._currentActiveHighligtFrame.GlobalPosition.X > this._currentActivePanelItem.GlobalPosition.X;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000E1C4 File Offset: 0x0000C3C4
		private Tuple<Widget, Widget> GetLeftAndRightElements()
		{
			if (this._currentActiveHighligtFrame.GlobalPosition.X < this._currentActivePanelItem.GlobalPosition.X)
			{
				return new Tuple<Widget, Widget>(this._currentActiveHighligtFrame, this._currentActivePanelItem);
			}
			return new Tuple<Widget, Widget>(this._currentActivePanelItem, this._currentActiveHighligtFrame);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000E218 File Offset: 0x0000C418
		private Tuple<Widget, Widget> GetTopAndBottomElements()
		{
			if (this._currentActiveHighligtFrame.GlobalPosition.Y < this._currentActivePanelItem.GlobalPosition.Y)
			{
				return new Tuple<Widget, Widget>(this._currentActiveHighligtFrame, this._currentActivePanelItem);
			}
			return new Tuple<Widget, Widget>(this._currentActivePanelItem, this._currentActiveHighligtFrame);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000E26A File Offset: 0x0000C46A
		private void OnTutorialItemPropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsDisabled")
			{
				if (propertyValue)
				{
					this._currentActivePanelItem = null;
					this.ArrowWidget.ResetFade();
					return;
				}
				this._currentActivePanelItem = widget as TutorialPanelImageWidget;
				this.ArrowWidget.DisableFade();
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000E2A6 File Offset: 0x0000C4A6
		private void OnHighlightElementToggleEvent(TutorialHighlightItemBrushWidget.HighlightElementToggledEvent obj)
		{
			if (obj.IsEnabled)
			{
				this._currentActiveHighligtFrame = obj.HighlightFrameWidget;
				this.ArrowWidget.ResetFade();
				return;
			}
			this.ArrowWidget.DisableFade();
			this._currentActiveHighligtFrame = null;
		}

		// Token: 0x040001E2 RID: 482
		private bool _initalized;

		// Token: 0x040001E3 RID: 483
		private TutorialHighlightItemBrushWidget _currentActiveHighligtFrame;

		// Token: 0x040001E4 RID: 484
		private TutorialPanelImageWidget _currentActivePanelItem;
	}
}
