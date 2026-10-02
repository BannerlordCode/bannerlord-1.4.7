using System;
using System.Collections.Generic;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000020 RID: 32
	public class EventManager
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000C128 File Offset: 0x0000A328
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000C130 File Offset: 0x0000A330
		public float Time { get; private set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000C139 File Offset: 0x0000A339
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000C141 File Offset: 0x0000A341
		public Vec2 UsableArea { get; set; } = new Vec2(1f, 1f);

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000C14A File Offset: 0x0000A34A
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000C152 File Offset: 0x0000A352
		public float LeftUsableAreaStart { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000C15B File Offset: 0x0000A35B
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000C163 File Offset: 0x0000A363
		public float TopUsableAreaStart { get; private set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000C16C File Offset: 0x0000A36C
		// (set) Token: 0x06000261 RID: 609 RVA: 0x0000C174 File Offset: 0x0000A374
		public Vector2 PageSize { get; private set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000C17D File Offset: 0x0000A37D
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000C184 File Offset: 0x0000A384
		public static EventManager UIEventManager { get; private set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000C18C File Offset: 0x0000A38C
		public Vector2 MousePositionInReferenceResolution
		{
			get
			{
				return this.MousePosition * this.Context.CustomInverseScale;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000C1A4 File Offset: 0x0000A3A4
		// (set) Token: 0x06000266 RID: 614 RVA: 0x0000C1AC File Offset: 0x0000A3AC
		public bool IsControllerActive { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000C1B5 File Offset: 0x0000A3B5
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000C1BD File Offset: 0x0000A3BD
		public UIContext Context { get; private set; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000269 RID: 617 RVA: 0x0000C1C8 File Offset: 0x0000A3C8
		// (remove) Token: 0x0600026A RID: 618 RVA: 0x0000C200 File Offset: 0x0000A400
		public event Action OnDragStarted;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600026B RID: 619 RVA: 0x0000C238 File Offset: 0x0000A438
		// (remove) Token: 0x0600026C RID: 620 RVA: 0x0000C270 File Offset: 0x0000A470
		public event Action OnDragEnded;

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000C2A5 File Offset: 0x0000A4A5
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000C2AD File Offset: 0x0000A4AD
		public Widget Root { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000C2B6 File Offset: 0x0000A4B6
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000C2C0 File Offset: 0x0000A4C0
		public Widget FocusedWidget
		{
			get
			{
				return this._focusedWidget;
			}
			set
			{
				if (this._isOnScreenKeyboardRequested || (this._focusedWidget is EditableTextWidget && Input.IsOnScreenKeyboardActive))
				{
					return;
				}
				if (this._focusedWidget != value)
				{
					Widget focusedWidget = this._focusedWidget;
					if (focusedWidget != null)
					{
						focusedWidget.OnLoseFocus();
					}
					if (value != null && (!value.ConnectedToRoot || !value.IsFocusable))
					{
						this._focusedWidget = null;
					}
					else
					{
						this._focusedWidget = value;
						Widget focusedWidget2 = this._focusedWidget;
						if (focusedWidget2 != null)
						{
							focusedWidget2.OnGainFocus();
						}
						if (this._focusedWidget is EditableTextWidget && this.IsControllerActive)
						{
							this._isOnScreenKeyboardRequested = true;
						}
					}
					Action onFocusedWidgetChanged = this.OnFocusedWidgetChanged;
					if (onFocusedWidgetChanged == null)
					{
						return;
					}
					onFocusedWidgetChanged();
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000C364 File Offset: 0x0000A564
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000C36C File Offset: 0x0000A56C
		public Widget HoveredWidget
		{
			get
			{
				return this._hoveredWidget;
			}
			set
			{
				if (this._hoveredWidget != value)
				{
					Widget hoveredWidget = this._hoveredWidget;
					if (hoveredWidget != null)
					{
						hoveredWidget.OnHoverEnd();
					}
					if (value != null && !value.ConnectedToRoot)
					{
						this._hoveredWidget = null;
						return;
					}
					this._hoveredWidget = value;
					Widget hoveredWidget2 = this._hoveredWidget;
					if (hoveredWidget2 == null)
					{
						return;
					}
					hoveredWidget2.OnHoverBegin();
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000C3BD File Offset: 0x0000A5BD
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000C3C5 File Offset: 0x0000A5C5
		public List<Widget> MouseOveredWidgets
		{
			get
			{
				return this._mouseOveredWidgets;
			}
			private set
			{
				if (value != null)
				{
					this._mouseOveredWidgets = value;
					return;
				}
				this._mouseOveredWidgets = null;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000C3D9 File Offset: 0x0000A5D9
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000C3E4 File Offset: 0x0000A5E4
		public Widget DragHoveredWidget
		{
			get
			{
				return this._dragHoveredWidget;
			}
			private set
			{
				if (this._dragHoveredWidget != value)
				{
					Widget dragHoveredWidget = this._dragHoveredWidget;
					if (dragHoveredWidget != null)
					{
						dragHoveredWidget.OnDragHoverEnd();
					}
					if (value != null && (!value.ConnectedToRoot || !value.AcceptDrop))
					{
						this._dragHoveredWidget = null;
						return;
					}
					this._dragHoveredWidget = value;
					Widget dragHoveredWidget2 = this._dragHoveredWidget;
					if (dragHoveredWidget2 == null)
					{
						return;
					}
					dragHoveredWidget2.OnDragHoverBegin();
				}
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000C43D File Offset: 0x0000A63D
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000C445 File Offset: 0x0000A645
		public Widget DraggedWidget { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000C450 File Offset: 0x0000A650
		public Vector2 DraggedWidgetPosition
		{
			get
			{
				if (this.DraggedWidget != null)
				{
					return this._dragCarrier.AreaRect.TopLeft * this.Context.CustomScale - new Vector2(this.LeftUsableAreaStart, this.TopUsableAreaStart);
				}
				return this.MousePositionInReferenceResolution;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000C4A2 File Offset: 0x0000A6A2
		// (set) Token: 0x0600027B RID: 635 RVA: 0x0000C4AA File Offset: 0x0000A6AA
		public Widget LatestMouseDownWidget
		{
			get
			{
				return this._latestMouseDownWidget;
			}
			private set
			{
				if (value != null && value.ConnectedToRoot)
				{
					this._latestMouseDownWidget = value;
					return;
				}
				this._latestMouseDownWidget = null;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000C4C6 File Offset: 0x0000A6C6
		// (set) Token: 0x0600027D RID: 637 RVA: 0x0000C4CE File Offset: 0x0000A6CE
		public Widget LatestMouseUpWidget
		{
			get
			{
				return this._latestMouseUpWidget;
			}
			private set
			{
				if (value != null && value.ConnectedToRoot)
				{
					this._latestMouseUpWidget = value;
					return;
				}
				this._latestMouseUpWidget = null;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000C4EA File Offset: 0x0000A6EA
		// (set) Token: 0x0600027F RID: 639 RVA: 0x0000C4F2 File Offset: 0x0000A6F2
		public Widget LatestMouseAlternateDownWidget
		{
			get
			{
				return this._latestMouseAlternateDownWidget;
			}
			private set
			{
				if (value != null && value.ConnectedToRoot)
				{
					this._latestMouseAlternateDownWidget = value;
					return;
				}
				this._latestMouseAlternateDownWidget = null;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000C50E File Offset: 0x0000A70E
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000C516 File Offset: 0x0000A716
		public Widget LatestMouseAlternateUpWidget
		{
			get
			{
				return this._latestMouseAlternateUpWidget;
			}
			private set
			{
				if (value != null && value.ConnectedToRoot)
				{
					this._latestMouseAlternateUpWidget = value;
					return;
				}
				this._latestMouseAlternateUpWidget = null;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000C532 File Offset: 0x0000A732
		public Vector2 MousePosition
		{
			get
			{
				return this.Context.InputContext.GetMousePosition();
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000C544 File Offset: 0x0000A744
		public ulong LocalFrameNumber
		{
			get
			{
				return this.Context.LocalFrameNumber;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000C551 File Offset: 0x0000A751
		private bool IsDragging
		{
			get
			{
				return this.DraggedWidget != null;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000C55C File Offset: 0x0000A75C
		public float DeltaMouseScroll
		{
			get
			{
				return this.Context.InputContext.GetMouseScrollDelta();
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000C570 File Offset: 0x0000A770
		public float RightStickVerticalScrollAmount
		{
			get
			{
				float y = Input.GetKeyState(InputKey.ControllerRStick).Y;
				return 3000f * y * 0.4f * this.CachedDt;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000C5A4 File Offset: 0x0000A7A4
		public float RightStickHorizontalScrollAmount
		{
			get
			{
				float x = Input.GetKeyState(InputKey.ControllerRStick).X;
				return 3000f * x * 0.4f * this.CachedDt;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000C5D8 File Offset: 0x0000A7D8
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000C5E0 File Offset: 0x0000A7E0
		internal float CachedDt { get; private set; }

		// Token: 0x0600028A RID: 650 RVA: 0x0000C5EC File Offset: 0x0000A7EC
		internal EventManager(UIContext context)
		{
			this.Context = context;
			this.Root = new Widget(context)
			{
				Id = "Root"
			};
			if (EventManager.UIEventManager == null)
			{
				EventManager.UIEventManager = new EventManager();
			}
			this.AreaRectangle = Rectangle2D.Create();
			this._widgetContainers = new Dictionary<WidgetContainer.ContainerType, WidgetContainer>
			{
				{
					WidgetContainer.ContainerType.Update,
					new WidgetContainer(32)
				},
				{
					WidgetContainer.ContainerType.ParallelUpdate,
					new WidgetContainer(16)
				},
				{
					WidgetContainer.ContainerType.LateUpdate,
					new WidgetContainer(32)
				},
				{
					WidgetContainer.ContainerType.VisualDefinition,
					new WidgetContainer(16)
				},
				{
					WidgetContainer.ContainerType.UpdateBrushes,
					new WidgetContainer(64)
				}
			};
			this._lateUpdateActionLocker = new object();
			this._lateUpdateActions = new Dictionary<int, List<UpdateAction>>();
			this._lateUpdateActionsRunning = new Dictionary<int, List<UpdateAction>>();
			this._onAfterFinalizedCallbacks = new List<Action>();
			for (int i = 1; i <= 5; i++)
			{
				this._lateUpdateActions.Add(i, new List<UpdateAction>(32));
				this._lateUpdateActionsRunning.Add(i, new List<UpdateAction>(32));
			}
			this._drawContext = new TwoDimensionDrawContext();
			this.MouseOveredWidgets = new List<Widget>();
			this.ParallelUpdateWidgetPredicate = new TWParallel.ParallelForWithDtAuxPredicate(this.ParallelUpdateWidget);
			this.UpdateBrushesWidgetPredicate = new TWParallel.ParallelForWithDtAuxPredicate(this.UpdateBrushesWidget);
			this.IsControllerActive = Input.IsGamepadActive;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000C764 File Offset: 0x0000A964
		internal void OnFinalize()
		{
			if (!this._lastSetFrictionValue.ApproximatelyEqualsTo(1f, 1E-05f))
			{
				this._lastSetFrictionValue = 1f;
				Input.SetCursorFriction(this._lastSetFrictionValue);
			}
			foreach (KeyValuePair<WidgetContainer.ContainerType, WidgetContainer> keyValuePair in this._widgetContainers)
			{
				keyValuePair.Value.Clear();
			}
			for (int i = 0; i < this._onAfterFinalizedCallbacks.Count; i++)
			{
				Action action = this._onAfterFinalizedCallbacks[i];
				if (action != null)
				{
					action();
				}
			}
			this._onAfterFinalizedCallbacks.Clear();
			this._onAfterFinalizedCallbacks = null;
			this._widgetContainers = null;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000C830 File Offset: 0x0000AA30
		public void AddAfterFinalizedCallback(Action callback)
		{
			this._onAfterFinalizedCallbacks.Add(callback);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000C840 File Offset: 0x0000AA40
		internal void OnContextActivated()
		{
			List<Widget> allChildrenAndThisRecursive = this.Root.GetAllChildrenAndThisRecursive();
			for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
			{
				allChildrenAndThisRecursive[i].OnContextActivated();
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000C878 File Offset: 0x0000AA78
		internal void OnContextDeactivated()
		{
			List<Widget> allChildrenAndThisRecursive = this.Root.GetAllChildrenAndThisRecursive();
			for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
			{
				allChildrenAndThisRecursive[i].OnContextDeactivated();
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
		internal void OnWidgetConnectedToRoot(Widget widget)
		{
			widget.HandleOnConnectedToRoot();
			List<Widget> allChildrenAndThisRecursive = widget.GetAllChildrenAndThisRecursive();
			for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
			{
				Widget widget2 = allChildrenAndThisRecursive[i];
				widget2.HandleOnConnectedToRoot();
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.Update, widget2);
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.LateUpdate, widget2);
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.UpdateBrushes, widget2);
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.ParallelUpdate, widget2);
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.VisualDefinition, widget2);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000C914 File Offset: 0x0000AB14
		internal void OnWidgetDisconnectedFromRoot(Widget widget)
		{
			widget.HandleOnDisconnectedFromRoot();
			if (widget == this.DraggedWidget && this.DraggedWidget.DragWidget != null)
			{
				this.ReleaseDraggedWidget();
				this.ClearDragObject();
			}
			GauntletGamepadNavigationManager.Instance.OnWidgetDisconnectedFromRoot(widget);
			List<Widget> allChildrenAndThisRecursive = widget.GetAllChildrenAndThisRecursive();
			for (int i = 0; i < allChildrenAndThisRecursive.Count; i++)
			{
				Widget widget2 = allChildrenAndThisRecursive[i];
				widget2.HandleOnDisconnectedFromRoot();
				this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.Update, widget2);
				this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.LateUpdate, widget2);
				this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.UpdateBrushes, widget2);
				this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.ParallelUpdate, widget2);
				this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.VisualDefinition, widget2);
				GauntletGamepadNavigationManager instance = GauntletGamepadNavigationManager.Instance;
				if (instance != null)
				{
					instance.OnWidgetDisconnectedFromRoot(widget2);
				}
				widget2.GamepadNavigationIndex = -1;
				widget2.UsedNavigationMovements = GamepadNavigationTypes.None;
				widget2.IsUsingNavigation = false;
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000C9CC File Offset: 0x0000ABCC
		internal void RegisterWidgetForEvent(WidgetContainer.ContainerType type, Widget widget)
		{
			if ((type == WidgetContainer.ContainerType.Update && widget.WidgetInfo.GotCustomUpdate) || (type == WidgetContainer.ContainerType.ParallelUpdate && widget.WidgetInfo.GotCustomParallelUpdate) || (type == WidgetContainer.ContainerType.LateUpdate && widget.WidgetInfo.GotCustomLateUpdate) || (type == WidgetContainer.ContainerType.VisualDefinition && widget.VisualDefinition != null) || (type == WidgetContainer.ContainerType.UpdateBrushes && widget.WidgetInfo.GotUpdateBrushes))
			{
				this._widgetContainers[type].Add(widget);
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000CA3C File Offset: 0x0000AC3C
		internal void UnRegisterWidgetForEvent(WidgetContainer.ContainerType type, Widget widget)
		{
			if ((type == WidgetContainer.ContainerType.Update && widget.WidgetInfo.GotCustomUpdate) || (type == WidgetContainer.ContainerType.ParallelUpdate && widget.WidgetInfo.GotCustomParallelUpdate) || (type == WidgetContainer.ContainerType.LateUpdate && widget.WidgetInfo.GotCustomLateUpdate) || (type == WidgetContainer.ContainerType.VisualDefinition && widget.VisualDefinition == null) || (type == WidgetContainer.ContainerType.UpdateBrushes && widget.WidgetInfo.GotUpdateBrushes))
			{
				this._widgetContainers[type].Remove(widget);
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000CAAA File Offset: 0x0000ACAA
		internal void OnWidgetVisualDefinitionChanged(Widget widget)
		{
			if (widget.VisualDefinition != null)
			{
				this.RegisterWidgetForEvent(WidgetContainer.ContainerType.VisualDefinition, widget);
				return;
			}
			this.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.VisualDefinition, widget);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000CAC5 File Offset: 0x0000ACC5
		private void MeasureAll()
		{
			this.Root.Measure(this.PageSize);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000CAD8 File Offset: 0x0000ACD8
		private void LayoutAll(float left, float bottom, float right, float top)
		{
			this.Root.Layout(left, bottom, right, top);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000CAEC File Offset: 0x0000ACEC
		private void UpdatePositions()
		{
			this.AreaRectangle.LocalPosition = new Vector2(this.LeftUsableAreaStart, this.TopUsableAreaStart);
			this.AreaRectangle.LocalScale = new Vector2(this.PageSize.X, this.PageSize.Y);
			this.AreaRectangle.LocalPivot = new Vector2(0.5f, 0.5f);
			Rectangle2D invalid = Rectangle2D.Invalid;
			this.AreaRectangle.CalculateMatrixFrame(in invalid);
			this.Root.UpdatePosition();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000CB74 File Offset: 0x0000AD74
		internal void CalculateCanvas(Vector2 pageSize, float dt)
		{
			if (this._measureDirty > 0 || this._layoutDirty > 0)
			{
				this.PageSize = pageSize;
				Vec2 vec = new Vec2(pageSize.X / this.UsableArea.X, pageSize.Y / this.UsableArea.Y);
				this.LeftUsableAreaStart = (vec.X - vec.X * this.UsableArea.X) * 0.5f;
				this.TopUsableAreaStart = (vec.Y - vec.Y * this.UsableArea.Y) * 0.5f;
				this.AreaRectangle.LocalPosition = new Vector2(this.LeftUsableAreaStart, this.TopUsableAreaStart);
				this.AreaRectangle.LocalScale = new Vector2(this.PageSize.X, this.PageSize.Y);
				if (this._measureDirty > 0)
				{
					this.MeasureAll();
				}
				this.LayoutAll(0f, this.PageSize.Y, this.PageSize.X, 0f);
				this.UpdatePositions();
				if (this._measureDirty > 0)
				{
					this._measureDirty--;
				}
				if (this._layoutDirty > 0)
				{
					this._layoutDirty--;
				}
				this._positionsDirty = false;
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000CCD4 File Offset: 0x0000AED4
		internal void RecalculateCanvas()
		{
			if (this._measureDirty == 2 || this._layoutDirty == 2)
			{
				if (this._measureDirty == 2)
				{
					this.MeasureAll();
				}
				this.LayoutAll(0f, this.PageSize.Y, this.PageSize.X, 0f);
				if (this._positionsDirty)
				{
					this.UpdatePositions();
					this._positionsDirty = false;
				}
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000CD40 File Offset: 0x0000AF40
		internal void MouseDown()
		{
			this._mouseIsDown = true;
			this._lastClickPosition = this.MousePosition;
			Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MousePressed);
			if (widgetAtMousePositionForEvent != null)
			{
				this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.MousePressed, true);
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000CD74 File Offset: 0x0000AF74
		internal void MouseUp(bool isFromInput = true)
		{
			this._mouseIsDown = false;
			if (this.IsDragging)
			{
				if (this.DraggedWidget.PreviewEvent(GauntletEvent.DragEnd))
				{
					this.DispatchEvent(this.DraggedWidget, GauntletEvent.DragEnd, true);
				}
				Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.Drop);
				if (widgetAtMousePositionForEvent != null && isFromInput)
				{
					this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.Drop, true);
				}
				else
				{
					this.CancelAndReturnDrag();
				}
				if (this.DraggedWidget != null)
				{
					this.ClearDragObject();
					return;
				}
			}
			else
			{
				Widget widgetAtMousePositionForEvent2 = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MouseReleased);
				this.DispatchEvent(widgetAtMousePositionForEvent2, GauntletEvent.MouseReleased, isFromInput);
				this.LatestMouseUpWidget = widgetAtMousePositionForEvent2;
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000CDF4 File Offset: 0x0000AFF4
		internal void MouseAlternateDown()
		{
			this._mouseAlternateIsDown = true;
			Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MouseAlternatePressed);
			if (widgetAtMousePositionForEvent != null)
			{
				this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.MouseAlternatePressed, true);
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000CE1C File Offset: 0x0000B01C
		internal void MouseAlternateUp(bool isFromInput = true)
		{
			this._mouseAlternateIsDown = false;
			Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MouseAlternateReleased);
			this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.MouseAlternateReleased, isFromInput);
			this.LatestMouseAlternateUpWidget = widgetAtMousePositionForEvent;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000CE48 File Offset: 0x0000B048
		internal void MouseScroll()
		{
			if (MathF.Abs(this.DeltaMouseScroll) > 0.001f)
			{
				Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MouseScroll);
				if (widgetAtMousePositionForEvent != null)
				{
					this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.MouseScroll, true);
				}
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000CE80 File Offset: 0x0000B080
		internal void RightStickMovement()
		{
			if (Input.GetKeyState(InputKey.ControllerRStick).X != 0f || Input.GetKeyState(InputKey.ControllerRStick).Y != 0f)
			{
				Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.RightStickMovement);
				if (widgetAtMousePositionForEvent != null)
				{
					this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.RightStickMovement, true);
				}
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000CED5 File Offset: 0x0000B0D5
		public void ClearFocus()
		{
			this.FocusedWidget = null;
			this.HoveredWidget = null;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000CEE8 File Offset: 0x0000B0E8
		private void CancelAndReturnDrag()
		{
			if (this._draggedWidgetPreviousParent != null)
			{
				this.DraggedWidget.ParentWidget = this._draggedWidgetPreviousParent;
				this.DraggedWidget.SetSiblingIndex(this._draggedWidgetIndex, false);
				this.DraggedWidget.PosOffset = new Vector2(0f, 0f);
				if (this.DraggedWidget.DragWidget != null)
				{
					this.DraggedWidget.DragWidget.ParentWidget = this.DraggedWidget;
					this.DraggedWidget.DragWidget.IsVisible = false;
				}
			}
			else
			{
				this.ReleaseDraggedWidget();
			}
			this._draggedWidgetPreviousParent = null;
			this._draggedWidgetIndex = -1;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000CF88 File Offset: 0x0000B188
		private void ClearDragObject()
		{
			this.DraggedWidget = null;
			Action onDragEnded = this.OnDragEnded;
			if (onDragEnded != null)
			{
				onDragEnded();
			}
			this._dragOffset = new Vector2(0f, 0f);
			this._dragCarrier.ParentWidget = null;
			this._dragCarrier = null;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000CFD8 File Offset: 0x0000B1D8
		internal void MouseMove()
		{
			if (this._mouseIsDown)
			{
				if (this.IsDragging)
				{
					Widget widgetAtMousePositionForEvent = this.GetWidgetAtMousePositionForEvent(GauntletEvent.DragHover);
					if (widgetAtMousePositionForEvent != null)
					{
						this.DispatchEvent(widgetAtMousePositionForEvent, GauntletEvent.DragHover, true);
					}
					else
					{
						this.DragHoveredWidget = null;
					}
				}
				else if (this.LatestMouseDownWidget != null)
				{
					if (this.LatestMouseDownWidget.PreviewEvent(GauntletEvent.MouseMove))
					{
						this.DispatchEvent(this.LatestMouseDownWidget, GauntletEvent.MouseMove, true);
					}
					if (!this.IsDragging && this.LatestMouseDownWidget.PreviewEvent(GauntletEvent.DragBegin))
					{
						Vector2 vector = this._lastClickPosition - this.MousePosition;
						Vector2 vector2 = new Vector2(vector.X, vector.Y);
						if (vector2.LengthSquared() > 100f * this.Context.Scale)
						{
							this.DispatchEvent(this.LatestMouseDownWidget, GauntletEvent.DragBegin, true);
						}
					}
				}
			}
			else if (!this._mouseAlternateIsDown)
			{
				Widget widgetAtMousePositionForEvent2 = this.GetWidgetAtMousePositionForEvent(GauntletEvent.MouseMove);
				if (widgetAtMousePositionForEvent2 != null)
				{
					this.DispatchEvent(widgetAtMousePositionForEvent2, GauntletEvent.MouseMove, true);
				}
			}
			List<Widget> list = new List<Widget>();
			List<Widget> list2 = new List<Widget>();
			EventManager.CollectEnableWidgetsAt(this.Root, this.MousePosition, list2);
			for (int i = 0; i < list2.Count; i++)
			{
				Widget widget = list2[i];
				if (!this.MouseOveredWidgets.Contains(widget))
				{
					widget.OnMouseOverBegin();
					GauntletGamepadNavigationManager instance = GauntletGamepadNavigationManager.Instance;
					if (instance != null)
					{
						instance.OnWidgetHoverBegin(widget);
					}
				}
				list.Add(widget);
			}
			for (int j = 0; j < this.MouseOveredWidgets.Count; j++)
			{
				Widget widget2 = this.MouseOveredWidgets[j];
				if (!list.Contains(widget2))
				{
					widget2.OnMouseOverEnd();
					if (widget2.GamepadNavigationIndex != -1)
					{
						GauntletGamepadNavigationManager instance2 = GauntletGamepadNavigationManager.Instance;
						if (instance2 != null)
						{
							instance2.OnWidgetHoverEnd(widget2);
						}
					}
				}
			}
			this.MouseOveredWidgets = list;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000D192 File Offset: 0x0000B392
		private static bool IsPointInsideMeasuredArea(Widget w, Vector2 p)
		{
			return w.AreaRect.IsPointInside(in p);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000D1A1 File Offset: 0x0000B3A1
		public bool IsPointInsideUsableArea(Vector2 p)
		{
			return this.AreaRectangle.IsPointInside(in p);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
		private Widget GetWidgetAtMousePositionForEvent(GauntletEvent gauntletEvent)
		{
			if (!this.GetIsHitThisFrame())
			{
				return null;
			}
			return this.GetWidgetAtPositionForEvent(gauntletEvent, this.MousePosition);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000D1CC File Offset: 0x0000B3CC
		private Widget GetWidgetAtPositionForEvent(GauntletEvent gauntletEvent, Vector2 pointerPosition)
		{
			Widget widget = null;
			List<Widget> list = new List<Widget>();
			EventManager.CollectEnableWidgetsAt(this.Root, pointerPosition, list);
			for (int i = 0; i < list.Count; i++)
			{
				Widget widget2 = list[i];
				if (widget2.PreviewEvent(gauntletEvent))
				{
					widget = widget2;
					break;
				}
			}
			return widget;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002A7 RID: 679 RVA: 0x0000D218 File Offset: 0x0000B418
		// (remove) Token: 0x060002A8 RID: 680 RVA: 0x0000D250 File Offset: 0x0000B450
		public event Action OnFocusedWidgetChanged;

		// Token: 0x060002A9 RID: 681 RVA: 0x0000D288 File Offset: 0x0000B488
		private void DispatchEvent(Widget selectedWidget, GauntletEvent gauntletEvent, bool isFromInput = true)
		{
			if (gauntletEvent != GauntletEvent.MouseReleased)
			{
			}
			switch (gauntletEvent)
			{
			case GauntletEvent.MouseMove:
				selectedWidget.OnMouseMove();
				this.HoveredWidget = selectedWidget;
				return;
			case GauntletEvent.MousePressed:
				this.LatestMouseDownWidget = selectedWidget;
				selectedWidget.OnMousePressed();
				this.FocusedWidget = selectedWidget;
				return;
			case GauntletEvent.MouseReleased:
				if (this.LatestMouseDownWidget != selectedWidget)
				{
					Widget latestMouseDownWidget = this.LatestMouseDownWidget;
					if (latestMouseDownWidget != null)
					{
						latestMouseDownWidget.OnMouseReleased(isFromInput);
					}
				}
				if (selectedWidget != null)
				{
					selectedWidget.OnMouseReleased(isFromInput);
					return;
				}
				break;
			case GauntletEvent.MouseAlternatePressed:
				this.LatestMouseAlternateDownWidget = selectedWidget;
				selectedWidget.OnMouseAlternatePressed();
				this.FocusedWidget = selectedWidget;
				return;
			case GauntletEvent.MouseAlternateReleased:
				if (this.LatestMouseAlternateDownWidget != selectedWidget)
				{
					Widget latestMouseAlternateDownWidget = this.LatestMouseAlternateDownWidget;
					if (latestMouseAlternateDownWidget != null)
					{
						latestMouseAlternateDownWidget.OnMouseAlternateReleased(isFromInput);
					}
				}
				if (selectedWidget != null)
				{
					selectedWidget.OnMouseAlternateReleased(isFromInput);
					return;
				}
				break;
			case GauntletEvent.DragHover:
				this.DragHoveredWidget = selectedWidget;
				return;
			case GauntletEvent.DragBegin:
				selectedWidget.OnDragBegin();
				return;
			case GauntletEvent.DragEnd:
				selectedWidget.OnDragEnd();
				return;
			case GauntletEvent.Drop:
				selectedWidget.OnDrop();
				return;
			case GauntletEvent.MouseScroll:
				selectedWidget.OnMouseScroll();
				return;
			case GauntletEvent.RightStickMovement:
				selectedWidget.OnRightStickMovement();
				break;
			default:
				return;
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000D37F File Offset: 0x0000B57F
		public static bool HitTest(Widget widget, Vector2 position)
		{
			if (widget == null)
			{
				Debug.FailedAssert("Calling HitTest using null widget!", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\EventManager.cs", "HitTest", 977);
				return false;
			}
			return EventManager.AnyWidgetsAt(widget, position);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000D3A8 File Offset: 0x0000B5A8
		public bool FocusTest(Widget root)
		{
			for (Widget widget = this.FocusedWidget; widget != null; widget = widget.ParentWidget)
			{
				if (root == widget)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000D3D0 File Offset: 0x0000B5D0
		private static bool AnyWidgetsAt(Widget widget, Vector2 position)
		{
			if (widget.IsEnabled && widget.IsVisible)
			{
				if (!widget.DoNotAcceptEvents && EventManager.IsPointInsideMeasuredArea(widget, position))
				{
					return true;
				}
				if (!widget.DoNotPassEventsToChildren)
				{
					for (int i = widget.ChildCount - 1; i >= 0; i--)
					{
						Widget child = widget.GetChild(i);
						if (!child.IsHidden && !child.IsDisabled && EventManager.AnyWidgetsAt(child, position))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000D440 File Offset: 0x0000B640
		private static void CollectEnableWidgetsAt(Widget widget, Vector2 position, List<Widget> widgets)
		{
			if (widget.IsEnabled && widget.IsVisible)
			{
				if (!widget.DoNotPassEventsToChildren)
				{
					for (int i = widget.ChildCount - 1; i >= 0; i--)
					{
						Widget child = widget.GetChild(i);
						if (!child.IsHidden && !child.IsDisabled && EventManager.IsPointInsideMeasuredArea(child, position))
						{
							EventManager.CollectEnableWidgetsAt(child, position, widgets);
						}
					}
				}
				if (!widget.DoNotAcceptEvents && EventManager.IsPointInsideMeasuredArea(widget, position))
				{
					widgets.Add(widget);
				}
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000D4BC File Offset: 0x0000B6BC
		private static void CollectVisibleWidgetsAt(Widget widget, Vector2 position, List<Widget> widgets)
		{
			if (widget.IsVisible)
			{
				for (int i = widget.ChildCount - 1; i >= 0; i--)
				{
					Widget child = widget.GetChild(i);
					if (child.IsVisible && EventManager.IsPointInsideMeasuredArea(child, position))
					{
						EventManager.CollectVisibleWidgetsAt(child, position, widgets);
					}
				}
				if (EventManager.IsPointInsideMeasuredArea(widget, position))
				{
					widgets.Add(widget);
				}
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000D518 File Offset: 0x0000B718
		internal void ManualAddRange(List<Widget> list, LinkedList<Widget> linked_list)
		{
			if (list.Capacity < linked_list.Count)
			{
				list.Capacity = linked_list.Count;
			}
			for (LinkedListNode<Widget> linkedListNode = linked_list.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				list.Add(linkedListNode.Value);
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000D560 File Offset: 0x0000B760
		private void ParallelUpdateWidget(int startInclusive, int endExclusive, float dt)
		{
			MBReadOnlyList<Widget> activeList = this._widgetContainers[WidgetContainer.ContainerType.ParallelUpdate].GetActiveList();
			for (int i = startInclusive; i < endExclusive; i++)
			{
				activeList[i].ParallelUpdate(dt);
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000D598 File Offset: 0x0000B798
		internal void ParallelUpdateWidgets(float dt)
		{
			WidgetContainer widgetContainer = this._widgetContainers[WidgetContainer.ContainerType.ParallelUpdate];
			TWParallel.ForWithoutRenderThreadDt(0, widgetContainer.Count, dt, this.ParallelUpdateWidgetPredicate, 16);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
		internal void Update(float dt)
		{
			this.Time += dt;
			this.CachedDt = dt;
			this.IsControllerActive = Input.IsGamepadActive;
			this.DefragContainers();
			MBReadOnlyList<Widget> activeList = this._widgetContainers[WidgetContainer.ContainerType.VisualDefinition].GetActiveList();
			for (int i = 0; i < activeList.Count; i++)
			{
				activeList[i].UpdateVisualDefinitions(dt);
			}
			this.UpdateDragCarrier();
			Widget hoveredWidget = this.HoveredWidget;
			UIContext.MouseCursors mouseCursors = ((((hoveredWidget != null) ? hoveredWidget.HoveredCursorState : null) != null) ? ((UIContext.MouseCursors)Enum.Parse(typeof(UIContext.MouseCursors), this.HoveredWidget.HoveredCursorState)) : UIContext.MouseCursors.Default);
			this.Context.ActiveCursorOfContext = mouseCursors;
			MBReadOnlyList<Widget> activeList2 = this._widgetContainers[WidgetContainer.ContainerType.Update].GetActiveList();
			for (int j = 0; j < activeList2.Count; j++)
			{
				activeList2[j].Update(dt);
			}
			this._doingParallelTask = true;
			this.DefragContainers();
			WidgetContainer widgetContainer = this._widgetContainers[WidgetContainer.ContainerType.ParallelUpdate];
			if (widgetContainer.Count > 64)
			{
				this.ParallelUpdateWidgets(dt);
			}
			else
			{
				MBReadOnlyList<Widget> activeList3 = widgetContainer.GetActiveList();
				for (int k = 0; k < activeList3.Count; k++)
				{
					activeList3[k].ParallelUpdate(dt);
				}
			}
			this._doingParallelTask = false;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000D714 File Offset: 0x0000B914
		internal void DefragContainers()
		{
			foreach (KeyValuePair<WidgetContainer.ContainerType, WidgetContainer> keyValuePair in this._widgetContainers)
			{
				keyValuePair.Value.Defrag();
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000D76C File Offset: 0x0000B96C
		internal void ParallelUpdateBrushes(float dt)
		{
			WidgetContainer widgetContainer = this._widgetContainers[WidgetContainer.ContainerType.UpdateBrushes];
			TWParallel.ForWithoutRenderThreadDt(0, widgetContainer.Count, dt, this.UpdateBrushesWidgetPredicate, 16);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000D79C File Offset: 0x0000B99C
		internal void UpdateBrushes(float dt)
		{
			WidgetContainer widgetContainer = this._widgetContainers[WidgetContainer.ContainerType.UpdateBrushes];
			if (widgetContainer.Count > 64)
			{
				this.ParallelUpdateBrushes(dt);
				return;
			}
			MBReadOnlyList<Widget> activeList = widgetContainer.GetActiveList();
			for (int i = 0; i < activeList.Count; i++)
			{
				activeList[i].UpdateBrushes(dt);
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000D7F0 File Offset: 0x0000B9F0
		private void UpdateBrushesWidget(int startInclusive, int endExclusive, float dt)
		{
			MBReadOnlyList<Widget> activeList = this._widgetContainers[WidgetContainer.ContainerType.UpdateBrushes].GetActiveList();
			for (int i = startInclusive; i < endExclusive; i++)
			{
				activeList[i].UpdateBrushes(dt);
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000D828 File Offset: 0x0000BA28
		public void AddLateUpdateAction(Widget owner, Action<float> action, int order)
		{
			UpdateAction updateAction = default(UpdateAction);
			updateAction.Target = owner;
			updateAction.Action = action;
			updateAction.Order = order;
			if (this._doingParallelTask)
			{
				object lateUpdateActionLocker = this._lateUpdateActionLocker;
				lock (lateUpdateActionLocker)
				{
					this._lateUpdateActions[order].Add(updateAction);
					return;
				}
			}
			this._lateUpdateActions[order].Add(updateAction);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000D8B0 File Offset: 0x0000BAB0
		internal void LateUpdate(float dt)
		{
			this.DefragContainers();
			MBReadOnlyList<Widget> activeList = this._widgetContainers[WidgetContainer.ContainerType.LateUpdate].GetActiveList();
			for (int i = 0; i < activeList.Count; i++)
			{
				activeList[i].LateUpdate(dt);
			}
			Dictionary<int, List<UpdateAction>> lateUpdateActions = this._lateUpdateActions;
			this._lateUpdateActions = this._lateUpdateActionsRunning;
			this._lateUpdateActionsRunning = lateUpdateActions;
			for (int j = 1; j <= 5; j++)
			{
				List<UpdateAction> list = this._lateUpdateActionsRunning[j];
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k].Target.ConnectedToRoot)
					{
						list[k].Action(dt);
					}
				}
				list.Clear();
			}
			if (this.IsControllerActive)
			{
				if (this.HoveredWidget != null && this.HoveredWidget.IsRecursivelyVisible())
				{
					if (this.HoveredWidget.FrictionEnabled && this.DraggedWidget == null)
					{
						this._lastSetFrictionValue = 0.45f;
					}
					else
					{
						this._lastSetFrictionValue = 1f;
					}
					Input.SetCursorFriction(this._lastSetFrictionValue);
				}
				if (!this._lastSetFrictionValue.ApproximatelyEqualsTo(1f, 1E-05f) && this.HoveredWidget == null)
				{
					this._lastSetFrictionValue = 1f;
					Input.SetCursorFriction(this._lastSetFrictionValue);
				}
			}
			if (this._isOnScreenKeyboardRequested)
			{
				EditableTextWidget editableTextWidget;
				if (this.IsControllerActive && (editableTextWidget = this.FocusedWidget as EditableTextWidget) != null)
				{
					string text = editableTextWidget.Text ?? string.Empty;
					string text2 = editableTextWidget.KeyboardInfoText ?? string.Empty;
					int maxLength = editableTextWidget.MaxLength;
					int num = (editableTextWidget.IsObfuscationEnabled ? 2 : 0);
					if (this.FocusedWidget is IntegerInputTextWidget || this.FocusedWidget is FloatInputTextWidget)
					{
						num = 1;
					}
					this.Context.TwoDimensionContext.Platform.OpenOnScreenKeyboard(text, text2, maxLength, num);
				}
				this._isOnScreenKeyboardRequested = false;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000DAA0 File Offset: 0x0000BCA0
		private void UpdateDragCarrier()
		{
			if (this._dragCarrier != null)
			{
				this._dragCarrier.PosOffset = this.MousePositionInReferenceResolution + this._dragOffset - new Vector2(this.LeftUsableAreaStart, this.TopUsableAreaStart) * this.Context.InverseScale;
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
		internal void BeginDragging(Widget draggedObject)
		{
			if (this.DraggedWidget != null)
			{
				Debug.FailedAssert("Trying to BeginDragging while there is already a dragged object.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\EventManager.cs", "BeginDragging", 1382);
				this.ClearDragObject();
			}
			if (!draggedObject.ConnectedToRoot)
			{
				Debug.FailedAssert("Trying to drag a widget with no parent, possibly a widget which is already being dragged", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\EventManager.cs", "BeginDragging", 1388);
				return;
			}
			draggedObject.IsPressed = false;
			this._draggedWidgetPreviousParent = null;
			this._draggedWidgetIndex = -1;
			Widget parentWidget = draggedObject.ParentWidget;
			this.DraggedWidget = draggedObject;
			Vector2 globalPosition = this.DraggedWidget.GlobalPosition;
			this._dragCarrier = new DragCarrierWidget(this.Context);
			this._dragCarrier.ParentWidget = this.Root;
			if (draggedObject.DragWidget != null)
			{
				Widget dragWidget = draggedObject.DragWidget;
				this._dragCarrier.WidthSizePolicy = SizePolicy.CoverChildren;
				this._dragCarrier.HeightSizePolicy = SizePolicy.CoverChildren;
				this._dragOffset = Vector2.Zero;
				dragWidget.IsVisible = true;
				dragWidget.ParentWidget = this._dragCarrier;
				if (this.DraggedWidget.HideOnDrag)
				{
					this.DraggedWidget.IsVisible = false;
				}
				this._draggedWidgetPreviousParent = null;
			}
			else
			{
				this._dragOffset = (globalPosition - this.MousePosition) * this.Context.InverseScale;
				this._dragCarrier.WidthSizePolicy = SizePolicy.Fixed;
				this._dragCarrier.HeightSizePolicy = SizePolicy.Fixed;
				if (this.DraggedWidget.WidthSizePolicy == SizePolicy.StretchToParent)
				{
					this._dragCarrier.ScaledSuggestedWidth = this.DraggedWidget.Size.X + (this.DraggedWidget.MarginRight + this.DraggedWidget.MarginLeft) * this.Context.Scale;
					this._dragOffset += new Vector2(-this.DraggedWidget.MarginLeft, 0f);
				}
				else
				{
					this._dragCarrier.ScaledSuggestedWidth = this.DraggedWidget.Size.X;
				}
				if (this.DraggedWidget.HeightSizePolicy == SizePolicy.StretchToParent)
				{
					this._dragCarrier.ScaledSuggestedHeight = this.DraggedWidget.Size.Y + (this.DraggedWidget.MarginTop + this.DraggedWidget.MarginBottom) * this.Context.Scale;
					this._dragOffset += new Vector2(0f, -this.DraggedWidget.MarginTop);
				}
				else
				{
					this._dragCarrier.ScaledSuggestedHeight = this.DraggedWidget.Size.Y;
				}
				if (parentWidget != null)
				{
					this._draggedWidgetPreviousParent = parentWidget;
					this._draggedWidgetIndex = draggedObject.GetSiblingIndex();
				}
				this.DraggedWidget.ParentWidget = this._dragCarrier;
			}
			this._dragCarrier.PosOffset = this.MousePositionInReferenceResolution + this._dragOffset - new Vector2(this.LeftUsableAreaStart, this.TopUsableAreaStart) * this.Context.InverseScale;
			Action onDragStarted = this.OnDragStarted;
			if (onDragStarted == null)
			{
				return;
			}
			onDragStarted();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000DDDC File Offset: 0x0000BFDC
		internal Widget ReleaseDraggedWidget()
		{
			Widget draggedWidget = this.DraggedWidget;
			if (this._draggedWidgetPreviousParent != null)
			{
				this.DraggedWidget.ParentWidget = this._draggedWidgetPreviousParent;
				this._draggedWidgetIndex = MathF.Max(0, MathF.Min(MathF.Max(0, this.DraggedWidget.ParentWidget.ChildCount - 1), this._draggedWidgetIndex));
				this.DraggedWidget.SetSiblingIndex(this._draggedWidgetIndex, false);
			}
			else
			{
				this.DraggedWidget.IsVisible = true;
			}
			this.DragHoveredWidget = null;
			return draggedWidget;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000DE60 File Offset: 0x0000C060
		internal void Render(TwoDimensionContext twoDimensionContext)
		{
			twoDimensionContext.ResetScissor();
			SimpleRectangle boundingBox = this.AreaRectangle.GetBoundingBox();
			twoDimensionContext.SetScissor(new ScissorTestInfo(boundingBox.X, boundingBox.Y, boundingBox.X2, boundingBox.Y2));
			this._drawContext.Reset();
			this.Root.Render(twoDimensionContext, this._drawContext);
			this._drawContext.DrawTo(twoDimensionContext);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000DECB File Offset: 0x0000C0CB
		public void UpdateLayout()
		{
			this.SetMeasureDirty();
			this.SetLayoutDirty();
			this.Root.LayoutUpdated();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000DEE4 File Offset: 0x0000C0E4
		internal void SetMeasureDirty()
		{
			this._measureDirty = 2;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000DEED File Offset: 0x0000C0ED
		internal void SetLayoutDirty()
		{
			this._layoutDirty = 2;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000DEF6 File Offset: 0x0000C0F6
		internal void SetPositionsDirty()
		{
			this._positionsDirty = true;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000DEFF File Offset: 0x0000C0FF
		public bool GetIsHitThisFrame()
		{
			return this.OnGetIsHitThisFrame != null && this.OnGetIsHitThisFrame();
		}

		// Token: 0x04000137 RID: 311
		public const int MinParallelUpdateCount = 64;

		// Token: 0x04000138 RID: 312
		private const int DirtyCount = 2;

		// Token: 0x04000139 RID: 313
		private const float DragStartThreshold = 100f;

		// Token: 0x0400013A RID: 314
		private const float ScrollScale = 0.4f;

		// Token: 0x0400013E RID: 318
		public Rectangle2D AreaRectangle;

		// Token: 0x04000144 RID: 324
		private List<Action> _onAfterFinalizedCallbacks;

		// Token: 0x04000146 RID: 326
		private Widget _focusedWidget;

		// Token: 0x04000147 RID: 327
		private Widget _hoveredWidget;

		// Token: 0x04000148 RID: 328
		private List<Widget> _mouseOveredWidgets;

		// Token: 0x04000149 RID: 329
		private Widget _dragHoveredWidget;

		// Token: 0x0400014B RID: 331
		private Widget _latestMouseDownWidget;

		// Token: 0x0400014C RID: 332
		private Widget _latestMouseUpWidget;

		// Token: 0x0400014D RID: 333
		private Widget _latestMouseAlternateDownWidget;

		// Token: 0x0400014E RID: 334
		private Widget _latestMouseAlternateUpWidget;

		// Token: 0x0400014F RID: 335
		private int _measureDirty;

		// Token: 0x04000150 RID: 336
		private int _layoutDirty;

		// Token: 0x04000151 RID: 337
		private bool _positionsDirty;

		// Token: 0x04000152 RID: 338
		private const int _stickMovementScaleAmount = 3000;

		// Token: 0x04000154 RID: 340
		private Vector2 _lastClickPosition;

		// Token: 0x04000155 RID: 341
		private bool _mouseIsDown;

		// Token: 0x04000156 RID: 342
		private bool _mouseAlternateIsDown;

		// Token: 0x04000157 RID: 343
		private Vector2 _dragOffset = new Vector2(0f, 0f);

		// Token: 0x04000158 RID: 344
		private Widget _draggedWidgetPreviousParent;

		// Token: 0x04000159 RID: 345
		private int _draggedWidgetIndex;

		// Token: 0x0400015A RID: 346
		private DragCarrierWidget _dragCarrier;

		// Token: 0x0400015B RID: 347
		private object _lateUpdateActionLocker;

		// Token: 0x0400015C RID: 348
		private Dictionary<int, List<UpdateAction>> _lateUpdateActions;

		// Token: 0x0400015D RID: 349
		private Dictionary<int, List<UpdateAction>> _lateUpdateActionsRunning;

		// Token: 0x0400015E RID: 350
		private Dictionary<WidgetContainer.ContainerType, WidgetContainer> _widgetContainers;

		// Token: 0x0400015F RID: 351
		private const int UpdateActionOrderCount = 5;

		// Token: 0x04000160 RID: 352
		private volatile bool _doingParallelTask;

		// Token: 0x04000161 RID: 353
		private TwoDimensionDrawContext _drawContext;

		// Token: 0x04000162 RID: 354
		private readonly TWParallel.ParallelForWithDtAuxPredicate ParallelUpdateWidgetPredicate;

		// Token: 0x04000163 RID: 355
		private readonly TWParallel.ParallelForWithDtAuxPredicate UpdateBrushesWidgetPredicate;

		// Token: 0x04000164 RID: 356
		private float _lastSetFrictionValue = 1f;

		// Token: 0x04000165 RID: 357
		private bool _isOnScreenKeyboardRequested;

		// Token: 0x04000167 RID: 359
		public Func<bool> OnGetIsHitThisFrame;
	}
}
