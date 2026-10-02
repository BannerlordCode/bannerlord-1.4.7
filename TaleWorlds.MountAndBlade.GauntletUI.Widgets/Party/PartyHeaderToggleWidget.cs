using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000064 RID: 100
	public class PartyHeaderToggleWidget : ToggleButtonWidget
	{
		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x000101E4 File Offset: 0x0000E3E4
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x000101EC File Offset: 0x0000E3EC
		public bool AutoToggleTransferButtonState { get; set; } = true;

		// Token: 0x06000556 RID: 1366 RVA: 0x000101F5 File Offset: 0x0000E3F5
		public PartyHeaderToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001020C File Offset: 0x0000E40C
		protected override void OnClick(Widget widget)
		{
			if (!this.BlockInputsWhenDisabled || this._listPanel == null || this._listPanel.ChildCount > 0)
			{
				base.OnClick(widget);
				this.UpdateCollapseIndicator();
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00010239 File Offset: 0x0000E439
		private void OnListSizeChange(Widget widget)
		{
			this.UpdateSize();
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00010241 File Offset: 0x0000E441
		private void OnListSizeChange(Widget parentWidget, Widget addedWidget)
		{
			this.UpdateSize();
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00010249 File Offset: 0x0000E449
		public override void SetState(string stateName)
		{
			if (!this.BlockInputsWhenDisabled || this._listPanel == null || this._listPanel.ChildCount > 0)
			{
				base.SetState(stateName);
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00010270 File Offset: 0x0000E470
		private void UpdateSize()
		{
			if (this.TransferButtonWidget != null && this.AutoToggleTransferButtonState)
			{
				this.TransferButtonWidget.IsEnabled = this._listPanel.ChildCount > 0;
			}
			if (this.IsRelevant)
			{
				base.IsVisible = true;
				if (this._listPanel.ChildCount > 0)
				{
					this._listPanel.IsVisible = true;
				}
				if (this._listPanel.ChildCount > this._latestChildCount && !base.WidgetToClose.IsVisible)
				{
					this.HandleClick();
				}
			}
			else
			{
				this._listPanel.IsVisible = false;
			}
			this._latestChildCount = this._listPanel.ChildCount;
			this.UpdateCollapseIndicator();
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001031C File Offset: 0x0000E51C
		private void ListPanelUpdated()
		{
			if (this.TransferButtonWidget != null)
			{
				this.TransferButtonWidget.IsEnabled = false;
			}
			this._listPanel.ItemAfterRemoveEventHandlers.Add(new Action<Widget>(this.OnListSizeChange));
			this._listPanel.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnListSizeChange));
			this.UpdateSize();
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001037B File Offset: 0x0000E57B
		private void TransferButtonUpdated()
		{
			this.TransferButtonWidget.IsEnabled = false;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00010389 File Offset: 0x0000E589
		private void CollapseIndicatorUpdated()
		{
			this.CollapseIndicator.AddState("Collapsed");
			this.CollapseIndicator.AddState("Expanded");
			this.UpdateCollapseIndicator();
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x000103B1 File Offset: 0x0000E5B1
		private void UpdateCollapseIndicator()
		{
			if (base.WidgetToClose != null && this.CollapseIndicator != null)
			{
				if (base.WidgetToClose.IsVisible)
				{
					this.CollapseIndicator.SetState("Expanded");
					return;
				}
				this.CollapseIndicator.SetState("Collapsed");
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x000103F1 File Offset: 0x0000E5F1
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x000103F9 File Offset: 0x0000E5F9
		[Editor(false)]
		public ListPanel ListPanel
		{
			get
			{
				return this._listPanel;
			}
			set
			{
				if (this._listPanel != value)
				{
					this._listPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "ListPanel");
					this.ListPanelUpdated();
				}
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0001041D File Offset: 0x0000E61D
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x00010425 File Offset: 0x0000E625
		[Editor(false)]
		public ButtonWidget TransferButtonWidget
		{
			get
			{
				return this._transferButtonWidget;
			}
			set
			{
				if (this._transferButtonWidget != value)
				{
					this._transferButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "TransferButtonWidget");
					this.TransferButtonUpdated();
				}
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00010449 File Offset: 0x0000E649
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00010451 File Offset: 0x0000E651
		[Editor(false)]
		public BrushWidget CollapseIndicator
		{
			get
			{
				return this._collapseIndicator;
			}
			set
			{
				if (this._collapseIndicator != value)
				{
					this._collapseIndicator = value;
					base.OnPropertyChanged<BrushWidget>(value, "CollapseIndicator");
					this.CollapseIndicatorUpdated();
				}
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00010475 File Offset: 0x0000E675
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x0001047D File Offset: 0x0000E67D
		[Editor(false)]
		public bool IsRelevant
		{
			get
			{
				return this._isRelevant;
			}
			set
			{
				if (this._isRelevant != value)
				{
					this._isRelevant = value;
					if (!this._isRelevant)
					{
						base.IsVisible = false;
					}
					this.UpdateSize();
					base.OnPropertyChanged(value, "IsRelevant");
				}
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x000104B0 File Offset: 0x0000E6B0
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x000104B8 File Offset: 0x0000E6B8
		[Editor(false)]
		public bool BlockInputsWhenDisabled
		{
			get
			{
				return this._blockInputsWhenDisabled;
			}
			set
			{
				if (this._blockInputsWhenDisabled != value)
				{
					this._blockInputsWhenDisabled = value;
					base.OnPropertyChanged(value, "BlockInputsWhenDisabled");
				}
			}
		}

		// Token: 0x04000249 RID: 585
		private int _latestChildCount;

		// Token: 0x0400024B RID: 587
		private ListPanel _listPanel;

		// Token: 0x0400024C RID: 588
		private ButtonWidget _transferButtonWidget;

		// Token: 0x0400024D RID: 589
		private BrushWidget _collapseIndicator;

		// Token: 0x0400024E RID: 590
		private bool _isRelevant = true;

		// Token: 0x0400024F RID: 591
		private bool _blockInputsWhenDisabled;
	}
}
