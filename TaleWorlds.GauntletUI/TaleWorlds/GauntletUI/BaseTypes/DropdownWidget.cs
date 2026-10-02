using System;
using System.Collections.Generic;
using System.Numerics;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.InputSystem;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000057 RID: 87
	public class DropdownWidget : Widget
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00017F70 File Offset: 0x00016170
		// (set) Token: 0x060005C2 RID: 1474 RVA: 0x00017F78 File Offset: 0x00016178
		[Editor(false)]
		public Widget TextWidget { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x00017F81 File Offset: 0x00016181
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x00017F89 File Offset: 0x00016189
		[Editor(false)]
		public bool DoNotHandleDropdownListPanel { get; set; }

		// Token: 0x060005C5 RID: 1477 RVA: 0x00017F94 File Offset: 0x00016194
		public DropdownWidget(UIContext context)
			: base(context)
		{
			this._clickHandler = new Action<Widget>(this.OnButtonClick);
			this._listSelectionHandler = new Action<Widget>(this.OnSelectionChanged);
			this._listItemRemovedHandler = new Action<Widget, Widget>(this.OnListItemRemoved);
			this._listItemAddedHandler = new Action<Widget, Widget>(this.OnListItemAdded);
			base.UsedNavigationMovements = GamepadNavigationTypes.Horizontal;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00018000 File Offset: 0x00016200
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this.DoNotHandleDropdownListPanel)
			{
				this.UpdateListPanelPosition();
			}
			if (this._buttonClicked)
			{
				if (this.ListPanel != null && !this._changedByControllerNavigation)
				{
					if (this._isOpen)
					{
						this.ClosePanel();
					}
					else
					{
						this.OpenPanel();
					}
				}
				this._buttonClicked = false;
			}
			else if (this._closeNextFrame && this._isOpen)
			{
				this.ClosePanel();
				this._closeNextFrame = false;
			}
			else if (base.EventManager.LatestMouseUpWidget != this._button && this._isOpen)
			{
				if (this.ListPanel.IsVisible)
				{
					this._closeNextFrame = true;
				}
			}
			else if (this._isOpen)
			{
				this._openFrameCounter++;
				if (this._openFrameCounter > 5)
				{
					if (Vector2.Distance(this.ListPanel.AreaRect.TopLeft, this._listPanelOpenPosition) > 20f && !this.DoNotHandleDropdownListPanel)
					{
						this._closeNextFrame = true;
					}
				}
				else
				{
					this._listPanelOpenPosition = this.ListPanel.AreaRect.TopLeft;
				}
			}
			this.RefreshSelectedItem();
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001811D File Offset: 0x0001631D
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			this.ScrollablePanel = this.GetParentScrollablePanelOfWidget(this);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00018132 File Offset: 0x00016332
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this.DoNotHandleDropdownListPanel)
			{
				this.UpdateListPanelPosition();
			}
			this.UpdateGamepadNavigationControls();
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00018150 File Offset: 0x00016350
		private void UpdateGamepadNavigationControls()
		{
			if (this._isOpen && base.EventManager.IsControllerActive && (Input.IsKeyPressed(InputKey.ControllerLBumper) || Input.IsKeyPressed(InputKey.ControllerLTrigger) || Input.IsKeyPressed(InputKey.ControllerRBumper) || Input.IsKeyPressed(InputKey.ControllerRTrigger)))
			{
				this.ClosePanel();
			}
			if (!this._isOpen && (base.IsPressed || this._button.IsPressed) && base.IsRecursivelyVisible() && base.EventManager.GetIsHitThisFrame())
			{
				if (Input.IsKeyReleased(InputKey.ControllerLLeft))
				{
					if (this.CurrentSelectedIndex > 0)
					{
						int num = this.CurrentSelectedIndex;
						this.CurrentSelectedIndex = num - 1;
					}
					else
					{
						this.CurrentSelectedIndex = this.ListPanel.ChildCount - 1;
					}
					this._isSelectedItemDirty = true;
					this._changedByControllerNavigation = true;
				}
				else if (Input.IsKeyReleased(InputKey.ControllerLRight))
				{
					if (this.CurrentSelectedIndex < this.ListPanel.ChildCount - 1)
					{
						int num = this.CurrentSelectedIndex;
						this.CurrentSelectedIndex = num + 1;
					}
					else
					{
						this.CurrentSelectedIndex = 0;
					}
					this._isSelectedItemDirty = true;
					this._changedByControllerNavigation = true;
				}
				base.IsUsingNavigation = true;
				return;
			}
			this._changedByControllerNavigation = false;
			base.IsUsingNavigation = false;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00018290 File Offset: 0x00016490
		private void UpdateListPanelPosition()
		{
			this.ListPanel.HorizontalAlignment = HorizontalAlignment.Left;
			this.ListPanel.VerticalAlignment = VerticalAlignment.Top;
			float num = (base.Size.X - this._listPanel.Size.X) * 0.5f;
			this.ListPanel.MarginTop = (base.GlobalPosition.Y + this.Button.Size.Y) * base._inverseScaleToUse;
			this.ListPanel.MarginLeft = (base.GlobalPosition.X + num) * base._inverseScaleToUse;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00018328 File Offset: 0x00016528
		protected virtual void OpenPanel()
		{
			if (this.Button != null)
			{
				this.Button.IsSelected = true;
			}
			this.ListPanel.IsVisible = true;
			this._listPanelOpenPosition = this.ListPanel.AreaRect.TopLeft;
			this._openFrameCounter = 0;
			this._isOpen = true;
			Action<DropdownWidget> onOpenStateChanged = this.OnOpenStateChanged;
			if (onOpenStateChanged != null)
			{
				onOpenStateChanged(this);
			}
			this.CreateGamepadNavigationScopeData();
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00018394 File Offset: 0x00016594
		protected virtual void ClosePanel()
		{
			if (this.Button != null)
			{
				this.Button.IsSelected = false;
			}
			this.ListPanel.IsVisible = false;
			this._buttonClicked = false;
			this._isOpen = false;
			Action<DropdownWidget> onOpenStateChanged = this.OnOpenStateChanged;
			if (onOpenStateChanged != null)
			{
				onOpenStateChanged(this);
			}
			this.ClearGamepadScopeData();
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000183E8 File Offset: 0x000165E8
		private void CreateGamepadNavigationScopeData()
		{
			if (this._navigationScope != null)
			{
				base.GamepadNavigationContext.RemoveNavigationScope(this._navigationScope);
			}
			this._scopeCollection = new GamepadNavigationForcedScopeCollection();
			this._scopeCollection.ParentWidget = base.ParentWidget ?? this;
			this._scopeCollection.CollectionOrder = 999;
			this._navigationScope = this.BuildGamepadNavigationScopeData();
			base.GamepadNavigationContext.AddNavigationScope(this._navigationScope, true);
			this._button.GamepadNavigationIndex = 0;
			this._navigationScope.AddWidgetAtIndex(this._button, 0);
			ButtonWidget button = this._button;
			button.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Combine(button.OnGamepadNavigationFocusGained, new Action<Widget>(this.OnWidgetGainedNavigationFocus));
			for (int i = 0; i < this.ListPanel.Children.Count; i++)
			{
				this.ListPanel.Children[i].GamepadNavigationIndex = i + 1;
				this._navigationScope.AddWidgetAtIndex(this.ListPanel.Children[i], i + 1);
				Widget widget = this.ListPanel.Children[i];
				widget.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Combine(widget.OnGamepadNavigationFocusGained, new Action<Widget>(this.OnWidgetGainedNavigationFocus));
			}
			base.GamepadNavigationContext.AddForcedScopeCollection(this._scopeCollection);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001853B File Offset: 0x0001673B
		private void OnWidgetGainedNavigationFocus(Widget widget)
		{
			ScrollablePanel scrollablePanel = this.ScrollablePanel;
			if (scrollablePanel == null)
			{
				return;
			}
			scrollablePanel.ScrollToChild(widget, null);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00018550 File Offset: 0x00016750
		private ScrollablePanel GetParentScrollablePanelOfWidget(Widget widget)
		{
			for (Widget widget2 = widget; widget2 != null; widget2 = widget2.ParentWidget)
			{
				ScrollablePanel scrollablePanel;
				if ((scrollablePanel = widget2 as ScrollablePanel) != null)
				{
					return scrollablePanel;
				}
			}
			return null;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00018578 File Offset: 0x00016778
		private GamepadNavigationScope BuildGamepadNavigationScopeData()
		{
			return new GamepadNavigationScope
			{
				ScopeMovements = GamepadNavigationTypes.Vertical,
				DoNotAutomaticallyFindChildren = true,
				DoNotAutoNavigateAfterSort = true,
				HasCircularMovement = true,
				ParentWidget = (base.ParentWidget ?? this),
				ScopeID = "DropdownScope"
			};
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000185B8 File Offset: 0x000167B8
		private void ClearGamepadScopeData()
		{
			if (this._navigationScope != null)
			{
				base.GamepadNavigationContext.RemoveNavigationScope(this._navigationScope);
				for (int i = 0; i < this.ListPanel.Children.Count; i++)
				{
					this.ListPanel.Children[i].GamepadNavigationIndex = -1;
					Widget widget = this.ListPanel.Children[i];
					widget.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Remove(widget.OnGamepadNavigationFocusGained, new Action<Widget>(this.OnWidgetGainedNavigationFocus));
				}
				this._button.GamepadNavigationIndex = -1;
				ButtonWidget button = this._button;
				button.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Remove(button.OnGamepadNavigationFocusGained, new Action<Widget>(this.OnWidgetGainedNavigationFocus));
				this._navigationScope = null;
			}
			if (this._scopeCollection != null)
			{
				base.GamepadNavigationContext.RemoveForcedScopeCollection(this._scopeCollection);
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00018698 File Offset: 0x00016898
		public void OnButtonClick(Widget widget)
		{
			this._buttonClicked = true;
			this._closeNextFrame = false;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000186A8 File Offset: 0x000168A8
		public void UpdateButtonText(string text)
		{
			TextWidget textWidget;
			if ((textWidget = this.TextWidget as TextWidget) != null)
			{
				textWidget.Text = ((!string.IsNullOrEmpty(text)) ? text : " ");
				return;
			}
			RichTextWidget richTextWidget;
			if ((richTextWidget = this.TextWidget as RichTextWidget) != null)
			{
				richTextWidget.Text = ((!string.IsNullOrEmpty(text)) ? text : " ");
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00018700 File Offset: 0x00016900
		public void OnListItemAdded(Widget parentWidget, Widget newChild)
		{
			this._isSelectedItemDirty = true;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00018709 File Offset: 0x00016909
		public void OnListItemRemoved(Widget removedItem, Widget removedChild)
		{
			this._isSelectedItemDirty = true;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00018712 File Offset: 0x00016912
		public void OnSelectionChanged(Widget widget)
		{
			this.CurrentSelectedIndex = this.ListPanelValue;
			this._isSelectedItemDirty = true;
			base.OnPropertyChanged(this.CurrentSelectedIndex, "CurrentSelectedIndex");
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00018738 File Offset: 0x00016938
		private void RefreshSelectedItem()
		{
			if (this._isSelectedItemDirty)
			{
				this.ListPanelValue = this.CurrentSelectedIndex;
				if (this.ListPanelValue >= 0)
				{
					string text = "";
					ListPanel listPanel = this.ListPanel;
					Widget widget = ((listPanel != null) ? listPanel.GetChild(this.ListPanelValue) : null);
					if (widget != null)
					{
						List<Widget> allChildrenRecursive = widget.GetAllChildrenRecursive(null);
						for (int i = 0; i < allChildrenRecursive.Count; i++)
						{
							TextWidget textWidget;
							RichTextWidget richTextWidget;
							if ((textWidget = allChildrenRecursive[i] as TextWidget) != null)
							{
								text = textWidget.Text;
							}
							else if ((richTextWidget = allChildrenRecursive[i] as RichTextWidget) != null)
							{
								text = richTextWidget.Text;
							}
						}
					}
					this.UpdateButtonText(text);
				}
				if (this.ListPanel != null)
				{
					for (int j = 0; j < this.ListPanel.ChildCount; j++)
					{
						ButtonWidget buttonWidget;
						if ((buttonWidget = this.ListPanel.GetChild(j) as ButtonWidget) != null)
						{
							buttonWidget.IsSelected = this.CurrentSelectedIndex == j;
						}
					}
				}
				this._isSelectedItemDirty = false;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001882C File Offset: 0x00016A2C
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x00018834 File Offset: 0x00016A34
		[Editor(false)]
		public ScrollablePanel ScrollablePanel
		{
			get
			{
				return this._scrollablePanel;
			}
			set
			{
				if (value != this._scrollablePanel)
				{
					this._scrollablePanel = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "ScrollablePanel");
				}
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x00018852 File Offset: 0x00016A52
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x0001885C File Offset: 0x00016A5C
		[Editor(false)]
		public ButtonWidget Button
		{
			get
			{
				return this._button;
			}
			set
			{
				ButtonWidget button = this._button;
				if (button != null)
				{
					button.ClickEventHandlers.Remove(this._clickHandler);
				}
				this._button = value;
				ButtonWidget button2 = this._button;
				if (button2 != null)
				{
					button2.ClickEventHandlers.Add(this._clickHandler);
				}
				this._isSelectedItemDirty = true;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x000188B0 File Offset: 0x00016AB0
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x000188B8 File Offset: 0x00016AB8
		[Editor(false)]
		public ListPanel ListPanel
		{
			get
			{
				return this._listPanel;
			}
			set
			{
				if (this._listPanel != null)
				{
					this._listPanel.SelectEventHandlers.Remove(this._listSelectionHandler);
					this._listPanel.ItemAddEventHandlers.Remove(this._listItemAddedHandler);
					this._listPanel.ItemRemoveEventHandlers.Remove(this._listItemRemovedHandler);
				}
				this._listPanel = value;
				if (this._listPanel != null)
				{
					if (!this.DoNotHandleDropdownListPanel)
					{
						this._listPanel.ParentWidget = base.EventManager.Root;
						this._listPanel.HorizontalAlignment = HorizontalAlignment.Left;
						this._listPanel.VerticalAlignment = VerticalAlignment.Top;
					}
					this._listPanel.SelectEventHandlers.Add(this._listSelectionHandler);
					this._listPanel.ItemAddEventHandlers.Add(this._listItemAddedHandler);
					this._listPanel.ItemRemoveEventHandlers.Add(this._listItemRemovedHandler);
				}
				this._isSelectedItemDirty = true;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x000189A0 File Offset: 0x00016BA0
		// (set) Token: 0x060005DF RID: 1503 RVA: 0x000189A8 File Offset: 0x00016BA8
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
			set
			{
				if (value != this._isOpen && !this._buttonClicked)
				{
					if (this._isOpen)
					{
						this.ClosePanel();
						return;
					}
					this.OpenPanel();
				}
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x000189D0 File Offset: 0x00016BD0
		// (set) Token: 0x060005E1 RID: 1505 RVA: 0x000189E7 File Offset: 0x00016BE7
		[Editor(false)]
		public int ListPanelValue
		{
			get
			{
				if (this.ListPanel != null)
				{
					return this.ListPanel.IntValue;
				}
				return -1;
			}
			set
			{
				if (this.ListPanel != null && this.ListPanel.IntValue != value)
				{
					this.ListPanel.IntValue = value;
				}
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x00018A0B File Offset: 0x00016C0B
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x00018A13 File Offset: 0x00016C13
		[Editor(false)]
		public int CurrentSelectedIndex
		{
			get
			{
				return this._currentSelectedIndex;
			}
			set
			{
				if (this._currentSelectedIndex != value)
				{
					this._currentSelectedIndex = value;
					this._isSelectedItemDirty = true;
				}
			}
		}

		// Token: 0x040002BC RID: 700
		public Action<DropdownWidget> OnOpenStateChanged;

		// Token: 0x040002BD RID: 701
		private readonly Action<Widget> _clickHandler;

		// Token: 0x040002BE RID: 702
		private readonly Action<Widget> _listSelectionHandler;

		// Token: 0x040002BF RID: 703
		private readonly Action<Widget, Widget> _listItemRemovedHandler;

		// Token: 0x040002C0 RID: 704
		private readonly Action<Widget, Widget> _listItemAddedHandler;

		// Token: 0x040002C1 RID: 705
		private Vector2 _listPanelOpenPosition;

		// Token: 0x040002C2 RID: 706
		private int _openFrameCounter;

		// Token: 0x040002C3 RID: 707
		private bool _isSelectedItemDirty = true;

		// Token: 0x040002C4 RID: 708
		private bool _changedByControllerNavigation;

		// Token: 0x040002C5 RID: 709
		private GamepadNavigationScope _navigationScope;

		// Token: 0x040002C6 RID: 710
		private GamepadNavigationForcedScopeCollection _scopeCollection;

		// Token: 0x040002C9 RID: 713
		private ScrollablePanel _scrollablePanel;

		// Token: 0x040002CA RID: 714
		private ButtonWidget _button;

		// Token: 0x040002CB RID: 715
		private ListPanel _listPanel;

		// Token: 0x040002CC RID: 716
		private int _currentSelectedIndex;

		// Token: 0x040002CD RID: 717
		private bool _closeNextFrame;

		// Token: 0x040002CE RID: 718
		private bool _isOpen;

		// Token: 0x040002CF RID: 719
		private bool _buttonClicked;
	}
}
