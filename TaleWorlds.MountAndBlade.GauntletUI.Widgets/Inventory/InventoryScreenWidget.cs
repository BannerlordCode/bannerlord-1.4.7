using System;
using System.Linq;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000143 RID: 323
	public class InventoryScreenWidget : Widget
	{
		// Token: 0x060010E5 RID: 4325 RVA: 0x0002E597 File Offset: 0x0002C797
		public InventoryScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x0002E5BC File Offset: 0x0002C7BC
		private T IsWidgetChildOfType<T>(Widget currentWidget) where T : Widget
		{
			while (currentWidget != null)
			{
				if (currentWidget is T)
				{
					return (T)((object)currentWidget);
				}
				currentWidget = currentWidget.ParentWidget;
			}
			return default(T);
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0002E5EE File Offset: 0x0002C7EE
		private bool IsWidgetChildOf(Widget parentWidget, Widget currentWidget)
		{
			while (currentWidget != null)
			{
				if (currentWidget == parentWidget)
				{
					return true;
				}
				currentWidget = currentWidget.ParentWidget;
			}
			return false;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0002E604 File Offset: 0x0002C804
		private bool IsWidgetChildOfId(string parentId, Widget currentWidget)
		{
			while (currentWidget != null)
			{
				if (currentWidget.Id == parentId)
				{
					return true;
				}
				currentWidget = currentWidget.ParentWidget;
			}
			return false;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0002E624 File Offset: 0x0002C824
		private InventoryListPanel GetCurrentHoveredListPanel()
		{
			for (int i = 0; i < base.EventManager.MouseOveredWidgets.Count; i++)
			{
				InventoryListPanel inventoryListPanel;
				if ((inventoryListPanel = base.EventManager.MouseOveredWidgets[i] as InventoryListPanel) != null)
				{
					return inventoryListPanel;
				}
			}
			return null;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0002E669 File Offset: 0x0002C869
		private Widget GetFirstBannerItem()
		{
			ListPanel listPanel = this.OtherInventoryListWidget.InnerPanel as ListPanel;
			ListPanel listPanel2 = ((listPanel != null) ? listPanel.GetChild(0) : null) as ListPanel;
			if (listPanel2 == null)
			{
				return null;
			}
			return listPanel2.FindChild((Widget x) => (x as InventoryItemTupleWidget).ItemType == this.BannerTypeName);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0002E6A4 File Offset: 0x0002C8A4
		private Widget GetItemWithId(ScrollablePanel listWidget, string id)
		{
			ListPanel listPanel = listWidget.InnerPanel as ListPanel;
			ListPanel listPanel2 = ((listPanel != null) ? listPanel.GetChild(0) : null) as ListPanel;
			if (listPanel2 == null)
			{
				return null;
			}
			return listPanel2.FindChild((Widget x) => (x as InventoryItemTupleWidget).ItemID == id);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0002E6F4 File Offset: 0x0002C8F4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.EventManager.DraggedWidget == null)
			{
				this.TargetEquipmentIndex = -1;
				this._currentDraggedItemWidget = null;
			}
			if (this._latestMouseDownWidget != base.EventManager.LatestMouseDownWidget)
			{
				this._latestMouseDownWidget = base.EventManager.LatestMouseDownWidget;
				bool flag;
				if (this._latestMouseDownWidget != null)
				{
					if (!(this._latestMouseDownWidget is InventoryItemButtonWidget) && !(this._latestMouseDownWidget is InventoryEquippedItemControlsBrushWidget))
					{
						flag = this._latestMouseDownWidget.GetAllParents().Any<Widget>((Widget x) => x is InventoryItemButtonWidget || x is InventoryEquippedItemControlsBrushWidget);
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				bool flag3 = this.IsWidgetChildOf(this.InventoryTooltip, this._latestMouseDownWidget);
				if (this._latestMouseDownWidget == null || (!flag2 && !flag3 && !this.ItemPreviewWidget.IsVisible))
				{
					base.EventFired("OnEmptyClick", Array.Empty<object>());
				}
			}
			Widget hoveredWidget = base.EventManager.HoveredWidget;
			if (hoveredWidget != null)
			{
				InventoryItemButtonWidget inventoryItemButtonWidget = this.IsWidgetChildOfType<InventoryItemButtonWidget>(hoveredWidget);
				bool flag4 = this.IsWidgetChildOfId("InventoryTooltip", hoveredWidget);
				if (inventoryItemButtonWidget != null)
				{
					this.ItemWidgetHoverBegin(inventoryItemButtonWidget);
				}
				else if (flag4 && GauntletGamepadNavigationManager.Instance.IsCursorMovingForNavigation)
				{
					this.ItemWidgetHoverEnd(null);
				}
				else if (!flag4 && hoveredWidget.ParentWidget != null)
				{
					this.ItemWidgetHoverEnd(null);
				}
			}
			else
			{
				this.ItemWidgetHoverEnd(null);
			}
			this.UpdateControllerTransferKeyVisuals();
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0002E84C File Offset: 0x0002CA4C
		private void UpdateControllerTransferKeyVisuals()
		{
			InventoryListPanel currentHoveredListPanel = this.GetCurrentHoveredListPanel();
			this.IsFocusedOnItemList = currentHoveredListPanel != null;
			if (!base.EventManager.IsControllerActive || !this.IsFocusedOnItemList)
			{
				this.PreviousCharacterInputVisualParent.IsVisible = true;
				this.NextCharacterInputVisualParent.IsVisible = true;
				this.TransferInputKeyVisualWidget.IsVisible = false;
				return;
			}
			this.PreviousCharacterInputVisualParent.IsVisible = false;
			this.NextCharacterInputVisualParent.IsVisible = false;
			InventoryItemTupleWidget inventoryItemTupleWidget;
			if ((inventoryItemTupleWidget = this._currentHoveredItemWidget as InventoryItemTupleWidget) != null && inventoryItemTupleWidget.IsHovered && inventoryItemTupleWidget.IsTransferable)
			{
				this.TransferInputKeyVisualWidget.IsVisible = true;
				Vector2 vector;
				if (inventoryItemTupleWidget.IsRightSide)
				{
					InputKeyVisualWidget transferInputKeyVisualWidget = this.TransferInputKeyVisualWidget;
					InputKeyVisualWidget nextCharacterInputKeyVisual = this._nextCharacterInputKeyVisual;
					transferInputKeyVisualWidget.KeyID = ((nextCharacterInputKeyVisual != null) ? nextCharacterInputKeyVisual.KeyID : null) ?? "";
					vector = this._currentHoveredItemWidget.GlobalPosition - new Vector2(0f, 20f * base._scaleToUse);
				}
				else
				{
					InputKeyVisualWidget transferInputKeyVisualWidget2 = this.TransferInputKeyVisualWidget;
					InputKeyVisualWidget previousCharacterInputKeyVisual = this._previousCharacterInputKeyVisual;
					transferInputKeyVisualWidget2.KeyID = ((previousCharacterInputKeyVisual != null) ? previousCharacterInputKeyVisual.KeyID : null) ?? "";
					vector = this._currentHoveredItemWidget.GlobalPosition - new Vector2(60f * base._scaleToUse - this._currentHoveredItemWidget.Size.X, 20f * base._scaleToUse);
				}
				this.TransferInputKeyVisualWidget.ScaledPositionXOffset = vector.X;
				this.TransferInputKeyVisualWidget.ScaledPositionYOffset = vector.Y;
				return;
			}
			this.TransferInputKeyVisualWidget.IsVisible = false;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0002E9E4 File Offset: 0x0002CBE4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._scrollToBannersInFrames > -1)
			{
				if (this._scrollToBannersInFrames == 0)
				{
					ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters(0f, 0f, 0f, 0f, -1f, 0.2f, 0.35f);
					this.OtherInventoryListWidget.ScrollToChild(this.GetFirstBannerItem(), autoScrollParameters);
				}
				this._scrollToBannersInFrames--;
			}
			if (this.ScrollToItem)
			{
				this._scrollToItemInSeconds = 0.2f;
				this.ScrollToItem = false;
			}
			if (this._scrollToItemInSeconds >= 0f)
			{
				this._scrollToItemInSeconds -= dt;
				if (this._scrollToItemInSeconds <= 0f)
				{
					ScrollablePanel.AutoScrollParameters autoScrollParameters2 = new ScrollablePanel.AutoScrollParameters(100f, 100f, 0f, 0f, -1f, -1f, 0.35f);
					this.OtherInventoryListWidget.ScrollToChild(this.GetItemWithId(this.OtherInventoryListWidget, this.ScrollItemId), autoScrollParameters2);
					this.PlayerInventoryListWidget.ScrollToChild(this.GetItemWithId(this.PlayerInventoryListWidget, this.ScrollItemId), autoScrollParameters2);
				}
			}
			if (this._focusLostThisFrame)
			{
				base.EventFired("OnFocusLose", Array.Empty<object>());
				this._focusLostThisFrame = false;
			}
			this.UpdateTooltipPosition();
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0002EB24 File Offset: 0x0002CD24
		private void UpdateTooltipPosition()
		{
			if (base.EventManager.DraggedWidget != null)
			{
				this.InventoryTooltip.IsHidden = true;
			}
			InventoryItemButtonWidget currentHoveredItemWidget = this._currentHoveredItemWidget;
			if (((currentHoveredItemWidget != null) ? currentHoveredItemWidget.ParentWidget : null) == null)
			{
				this._lastDisplayedTooltipItem = null;
				return;
			}
			if (this._tooltipHiddenFrameCount < this.TooltipHideFrameLength)
			{
				this._tooltipHiddenFrameCount++;
				this.InventoryTooltip.PositionXOffset = 5000f;
				this.InventoryTooltip.PositionYOffset = 5000f;
				return;
			}
			if (this._currentHoveredItemWidget.IsRightSide)
			{
				this.InventoryTooltip.ScaledPositionXOffset = this._currentHoveredItemWidget.ParentWidget.GlobalPosition.X - this.InventoryTooltip.Size.X + 10f * base._scaleToUse;
			}
			else
			{
				this.InventoryTooltip.ScaledPositionXOffset = this._currentHoveredItemWidget.ParentWidget.GlobalPosition.X + this._currentHoveredItemWidget.ParentWidget.Size.X - 10f * base._scaleToUse;
			}
			float num = base.EventManager.PageSize.Y - this.InventoryTooltip.MeasuredSize.Y;
			this.InventoryTooltip.ScaledPositionYOffset = Mathf.Clamp(this._currentHoveredItemWidget.GlobalPosition.Y, 0f, num);
			this._lastDisplayedTooltipItem = this._currentHoveredItemWidget;
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0002EC8A File Offset: 0x0002CE8A
		private void TradeLabelOnPropertyChanged(PropertyOwnerObject owner, string propertyName, object value)
		{
			if (propertyName == "Text")
			{
				this.TradeLabel.IsDisabled = string.IsNullOrEmpty(this.TradeLabel.Text);
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0002ECB4 File Offset: 0x0002CEB4
		private void ItemWidgetHoverBegin(InventoryItemButtonWidget itemWidget)
		{
			if (this._currentHoveredItemWidget != itemWidget)
			{
				this._currentHoveredItemWidget = itemWidget;
				this._tooltipHiddenFrameCount = 0;
				Widget widget = this.InventoryTooltip.FindChild("TargetItemTooltip");
				if (this._currentHoveredItemWidget.IsRightSide)
				{
					widget.SetSiblingIndex(1, false);
				}
				else
				{
					widget.SetSiblingIndex(0, false);
				}
				this.InventoryTooltip.IsHidden = false;
				base.EventFired("ItemHoverBegin", new object[] { itemWidget });
			}
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0002ED29 File Offset: 0x0002CF29
		private void ItemWidgetHoverEnd(InventoryItemButtonWidget itemWidget)
		{
			if (this._currentHoveredItemWidget != null && itemWidget == null)
			{
				this._currentHoveredItemWidget = null;
				this.InventoryTooltip.IsHidden = true;
				base.EventFired("ItemHoverEnd", Array.Empty<object>());
			}
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0002ED5C File Offset: 0x0002CF5C
		public void ItemWidgetDragBegin(InventoryItemButtonWidget itemWidget)
		{
			base.EventFired("OnEmptyClick", Array.Empty<object>());
			this._currentDraggedItemWidget = itemWidget;
			InventoryEquippedItemSlotWidget inventoryEquippedItemSlotWidget = itemWidget as InventoryEquippedItemSlotWidget;
			if (inventoryEquippedItemSlotWidget != null)
			{
				this.TargetEquipmentIndex = inventoryEquippedItemSlotWidget.TargetEquipmentIndex;
				return;
			}
			this.TargetEquipmentIndex = itemWidget.EquipmentIndex;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0002EDA3 File Offset: 0x0002CFA3
		public void ItemWidgetDrop(InventoryItemButtonWidget itemWidget)
		{
			if (this._currentDraggedItemWidget == itemWidget)
			{
				this._currentDraggedItemWidget = null;
				this.TargetEquipmentIndex = -1;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x0002EDBC File Offset: 0x0002CFBC
		// (set) Token: 0x060010F6 RID: 4342 RVA: 0x0002EDC4 File Offset: 0x0002CFC4
		[Editor(false)]
		public InputKeyVisualWidget TransferInputKeyVisualWidget
		{
			get
			{
				return this._transferInputKeyVisualWidget;
			}
			set
			{
				if (this._transferInputKeyVisualWidget != value)
				{
					this._transferInputKeyVisualWidget = value;
					base.OnPropertyChanged<InputKeyVisualWidget>(value, "TransferInputKeyVisualWidget");
				}
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x0002EDE2 File Offset: 0x0002CFE2
		// (set) Token: 0x060010F8 RID: 4344 RVA: 0x0002EDEC File Offset: 0x0002CFEC
		public Widget PreviousCharacterInputVisualParent
		{
			get
			{
				return this._previousCharacterInputVisualParent;
			}
			set
			{
				if (value != this._previousCharacterInputVisualParent)
				{
					this._previousCharacterInputVisualParent = value;
					if (this._previousCharacterInputVisualParent != null)
					{
						this._previousCharacterInputKeyVisual = this._previousCharacterInputVisualParent.Children.FirstOrDefault<Widget>((Widget x) => x is InputKeyVisualWidget) as InputKeyVisualWidget;
					}
				}
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x0002EE4B File Offset: 0x0002D04B
		// (set) Token: 0x060010FA RID: 4346 RVA: 0x0002EE54 File Offset: 0x0002D054
		public Widget NextCharacterInputVisualParent
		{
			get
			{
				return this._nextCharacterInputVisualParent;
			}
			set
			{
				if (value != this._nextCharacterInputVisualParent)
				{
					this._nextCharacterInputVisualParent = value;
					if (this._nextCharacterInputVisualParent != null)
					{
						this._nextCharacterInputKeyVisual = this._nextCharacterInputVisualParent.Children.FirstOrDefault<Widget>((Widget x) => x is InputKeyVisualWidget) as InputKeyVisualWidget;
					}
				}
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x0002EEB3 File Offset: 0x0002D0B3
		// (set) Token: 0x060010FC RID: 4348 RVA: 0x0002EEBC File Offset: 0x0002D0BC
		[Editor(false)]
		public RichTextWidget TradeLabel
		{
			get
			{
				return this._tradeLabel;
			}
			set
			{
				if (this._tradeLabel != value)
				{
					if (this._tradeLabel != null)
					{
						this._tradeLabel.PropertyChanged -= this.TradeLabelOnPropertyChanged;
					}
					this._tradeLabel = value;
					if (this._tradeLabel != null)
					{
						this._tradeLabel.PropertyChanged += this.TradeLabelOnPropertyChanged;
					}
					base.OnPropertyChanged<RichTextWidget>(value, "TradeLabel");
				}
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060010FD RID: 4349 RVA: 0x0002EF23 File Offset: 0x0002D123
		// (set) Token: 0x060010FE RID: 4350 RVA: 0x0002EF2B File Offset: 0x0002D12B
		[Editor(false)]
		public Widget InventoryTooltip
		{
			get
			{
				return this._inventoryTooltip;
			}
			set
			{
				if (this._inventoryTooltip != value)
				{
					this._inventoryTooltip = value;
					base.OnPropertyChanged<Widget>(value, "InventoryTooltip");
				}
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x0002EF49 File Offset: 0x0002D149
		// (set) Token: 0x06001100 RID: 4352 RVA: 0x0002EF51 File Offset: 0x0002D151
		[Editor(false)]
		public InventoryItemPreviewWidget ItemPreviewWidget
		{
			get
			{
				return this._itemPreviewWidget;
			}
			set
			{
				if (this._itemPreviewWidget != value)
				{
					this._itemPreviewWidget = value;
					base.OnPropertyChanged<InventoryItemPreviewWidget>(value, "ItemPreviewWidget");
				}
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x0002EF6F File Offset: 0x0002D16F
		// (set) Token: 0x06001102 RID: 4354 RVA: 0x0002EF77 File Offset: 0x0002D177
		[Editor(false)]
		public int TransactionCount
		{
			get
			{
				return this._transactionCount;
			}
			set
			{
				if (this._transactionCount != value)
				{
					this._transactionCount = value;
					base.OnPropertyChanged(value, "TransactionCount");
				}
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x0002EF95 File Offset: 0x0002D195
		// (set) Token: 0x06001104 RID: 4356 RVA: 0x0002EF9D File Offset: 0x0002D19D
		[Editor(false)]
		public int EquipmentMode
		{
			get
			{
				return this._equipmentMode;
			}
			set
			{
				if (this._equipmentMode != value)
				{
					this._equipmentMode = value;
					base.OnPropertyChanged(value, "EquipmentMode");
				}
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x0002EFBB File Offset: 0x0002D1BB
		// (set) Token: 0x06001106 RID: 4358 RVA: 0x0002EFC3 File Offset: 0x0002D1C3
		[Editor(false)]
		public int TargetEquipmentIndex
		{
			get
			{
				return this._targetEquipmentIndex;
			}
			set
			{
				if (this._targetEquipmentIndex != value)
				{
					this._targetEquipmentIndex = value;
					base.OnPropertyChanged(value, "TargetEquipmentIndex");
				}
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x0002EFE1 File Offset: 0x0002D1E1
		// (set) Token: 0x06001108 RID: 4360 RVA: 0x0002EFE9 File Offset: 0x0002D1E9
		[Editor(false)]
		public ScrollablePanel OtherInventoryListWidget
		{
			get
			{
				return this._otherInventoryListWidget;
			}
			set
			{
				if (value != this._otherInventoryListWidget)
				{
					this._otherInventoryListWidget = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "OtherInventoryListWidget");
				}
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x0002F007 File Offset: 0x0002D207
		// (set) Token: 0x0600110A RID: 4362 RVA: 0x0002F00F File Offset: 0x0002D20F
		[Editor(false)]
		public ScrollablePanel PlayerInventoryListWidget
		{
			get
			{
				return this._playerInventoryListWidget;
			}
			set
			{
				if (value != this._playerInventoryListWidget)
				{
					this._playerInventoryListWidget = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "PlayerInventoryListWidget");
				}
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x0002F02D File Offset: 0x0002D22D
		// (set) Token: 0x0600110C RID: 4364 RVA: 0x0002F035 File Offset: 0x0002D235
		[Editor(false)]
		public bool IsFocusedOnItemList
		{
			get
			{
				return this._isFocusedOnItemList;
			}
			set
			{
				if (value != this._isFocusedOnItemList)
				{
					this._isFocusedOnItemList = value;
					base.OnPropertyChanged(value, "IsFocusedOnItemList");
				}
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x0002F053 File Offset: 0x0002D253
		// (set) Token: 0x0600110E RID: 4366 RVA: 0x0002F05B File Offset: 0x0002D25B
		[Editor(false)]
		public bool IsBannerTutorialActive
		{
			get
			{
				return this._isBannerTutorialActive;
			}
			set
			{
				if (value != this._isBannerTutorialActive)
				{
					this._isBannerTutorialActive = value;
					base.OnPropertyChanged(value, "IsBannerTutorialActive");
					if (value)
					{
						this._scrollToBannersInFrames = 1;
					}
				}
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x0002F083 File Offset: 0x0002D283
		// (set) Token: 0x06001110 RID: 4368 RVA: 0x0002F08B File Offset: 0x0002D28B
		[Editor(false)]
		public string BannerTypeName
		{
			get
			{
				return this._bannerTypeName;
			}
			set
			{
				if (value != this._bannerTypeName)
				{
					this._bannerTypeName = value;
					base.OnPropertyChanged<string>(value, "BannerTypeName");
				}
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x0002F0AE File Offset: 0x0002D2AE
		// (set) Token: 0x06001112 RID: 4370 RVA: 0x0002F0B6 File Offset: 0x0002D2B6
		[Editor(false)]
		public bool ScrollToItem
		{
			get
			{
				return this._scrollToItem;
			}
			set
			{
				if (value != this._scrollToItem)
				{
					this._scrollToItem = value;
					base.OnPropertyChanged(value, "ScrollToItem");
				}
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x0002F0D4 File Offset: 0x0002D2D4
		// (set) Token: 0x06001114 RID: 4372 RVA: 0x0002F0DC File Offset: 0x0002D2DC
		[Editor(false)]
		public string ScrollItemId
		{
			get
			{
				return this._scrollItemId;
			}
			set
			{
				if (value != this._scrollItemId)
				{
					this._scrollItemId = value;
					base.OnPropertyChanged<string>(value, "ScrollItemId");
				}
			}
		}

		// Token: 0x040007A0 RID: 1952
		private readonly int TooltipHideFrameLength = 2;

		// Token: 0x040007A1 RID: 1953
		private Widget _latestMouseDownWidget;

		// Token: 0x040007A2 RID: 1954
		private InventoryItemButtonWidget _currentHoveredItemWidget;

		// Token: 0x040007A3 RID: 1955
		private InventoryItemButtonWidget _currentDraggedItemWidget;

		// Token: 0x040007A4 RID: 1956
		private InventoryItemButtonWidget _lastDisplayedTooltipItem;

		// Token: 0x040007A5 RID: 1957
		private int _tooltipHiddenFrameCount;

		// Token: 0x040007A6 RID: 1958
		private int _scrollToBannersInFrames = -1;

		// Token: 0x040007A7 RID: 1959
		private float _scrollToItemInSeconds = -1f;

		// Token: 0x040007A8 RID: 1960
		private InputKeyVisualWidget _previousCharacterInputKeyVisual;

		// Token: 0x040007A9 RID: 1961
		private InputKeyVisualWidget _nextCharacterInputKeyVisual;

		// Token: 0x040007AA RID: 1962
		private Widget _previousCharacterInputVisualParent;

		// Token: 0x040007AB RID: 1963
		private Widget _nextCharacterInputVisualParent;

		// Token: 0x040007AC RID: 1964
		private InputKeyVisualWidget _transferInputKeyVisualWidget;

		// Token: 0x040007AD RID: 1965
		private RichTextWidget _tradeLabel;

		// Token: 0x040007AE RID: 1966
		private Widget _inventoryTooltip;

		// Token: 0x040007AF RID: 1967
		private InventoryItemPreviewWidget _itemPreviewWidget;

		// Token: 0x040007B0 RID: 1968
		private int _transactionCount;

		// Token: 0x040007B1 RID: 1969
		private int _equipmentMode;

		// Token: 0x040007B2 RID: 1970
		private int _targetEquipmentIndex;

		// Token: 0x040007B3 RID: 1971
		private ScrollablePanel _otherInventoryListWidget;

		// Token: 0x040007B4 RID: 1972
		private ScrollablePanel _playerInventoryListWidget;

		// Token: 0x040007B5 RID: 1973
		private bool _focusLostThisFrame;

		// Token: 0x040007B6 RID: 1974
		private bool _isFocusedOnItemList;

		// Token: 0x040007B7 RID: 1975
		private bool _isBannerTutorialActive;

		// Token: 0x040007B8 RID: 1976
		private bool _scrollToItem;

		// Token: 0x040007B9 RID: 1977
		private string _bannerTypeName;

		// Token: 0x040007BA RID: 1978
		private string _scrollItemId;
	}
}
