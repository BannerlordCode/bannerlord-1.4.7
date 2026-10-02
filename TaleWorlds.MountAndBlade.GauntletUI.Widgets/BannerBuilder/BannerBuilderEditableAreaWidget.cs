using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.BannerBuilder
{
	// Token: 0x02000192 RID: 402
	public class BannerBuilderEditableAreaWidget : Widget
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x00038681 File Offset: 0x00036881
		// (set) Token: 0x060014B4 RID: 5300 RVA: 0x00038689 File Offset: 0x00036889
		public ButtonWidget DragWidgetTopRight { get; set; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x00038692 File Offset: 0x00036892
		// (set) Token: 0x060014B6 RID: 5302 RVA: 0x0003869A File Offset: 0x0003689A
		public ButtonWidget DragWidgetRight { get; set; }

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x000386A3 File Offset: 0x000368A3
		// (set) Token: 0x060014B8 RID: 5304 RVA: 0x000386AB File Offset: 0x000368AB
		public ButtonWidget DragWidgetTop { get; set; }

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x000386B4 File Offset: 0x000368B4
		// (set) Token: 0x060014BA RID: 5306 RVA: 0x000386BC File Offset: 0x000368BC
		public ButtonWidget RotateWidget { get; set; }

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x000386C5 File Offset: 0x000368C5
		// (set) Token: 0x060014BC RID: 5308 RVA: 0x000386CD File Offset: 0x000368CD
		public BannerTableauWidget BannerTableauWidget { get; set; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x000386D6 File Offset: 0x000368D6
		// (set) Token: 0x060014BE RID: 5310 RVA: 0x000386DE File Offset: 0x000368DE
		public Widget EditableAreaVisualWidget { get; set; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x000386E7 File Offset: 0x000368E7
		// (set) Token: 0x060014C0 RID: 5312 RVA: 0x000386EF File Offset: 0x000368EF
		public int LayerIndex { get; set; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x000386F8 File Offset: 0x000368F8
		// (set) Token: 0x060014C2 RID: 5314 RVA: 0x00038700 File Offset: 0x00036900
		public bool IsMirrorActive { get; set; }

		// Token: 0x060014C3 RID: 5315 RVA: 0x00038709 File Offset: 0x00036909
		public BannerBuilderEditableAreaWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00038712 File Offset: 0x00036912
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.BannerTableauWidget.MeshIndexToUpdate = this.LayerIndex;
			if (!this._initialized)
			{
				this.Initialize();
			}
			this.UpdateRequiredValues();
			this.UpdateEditableAreaVisual();
			this.HandleCursor();
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0003874C File Offset: 0x0003694C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				BannerBuilderEditableAreaWidget.BuilderMode currentMode = this._currentMode;
				if (currentMode != BannerBuilderEditableAreaWidget.BuilderMode.None && currentMode - BannerBuilderEditableAreaWidget.BuilderMode.Rotating <= 4)
				{
					base.EventFired("RefreshBanner", Array.Empty<object>());
				}
			}
			this.HandleRotation();
			this.HandlePositioning();
			this.HandleForEdge(BannerBuilderEditableAreaWidget.EdgeResizeType.Right);
			this.HandleForEdge(BannerBuilderEditableAreaWidget.EdgeResizeType.Top);
			this.HandleForCorner();
			this._latestMousePosition = base.EventManager.MousePosition;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x000387BD File Offset: 0x000369BD
		private void Initialize()
		{
			this._centerOfSigil = new Vec2(0f, 0f);
			this._sizeOfSigil = new Vec2(0f, 0f);
			this._initialized = true;
			this.OnIsLayerPatternChanged(false);
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x000387F8 File Offset: 0x000369F8
		private void UpdateRequiredValues()
		{
			float num = this._positionValue.X / (float)this.TotalAreaSize * base.Size.X;
			float num2 = this._positionValue.Y / (float)this.TotalAreaSize * base.Size.Y;
			this._centerOfSigil.x = num;
			this._centerOfSigil.y = num2;
			float num3 = this._sizeValue.X / (float)this.TotalAreaSize * base.Size.X;
			float num4 = this._sizeValue.Y / (float)this.TotalAreaSize * base.Size.Y;
			this._sizeOfSigil.x = num3;
			this._sizeOfSigil.y = num4;
			this._positionLimitMin = 0f;
			this._positionLimitMax = this._positionLimitMin + (float)this.TotalAreaSize;
			this._sizeLimitMax = this.TotalAreaSize;
			this._areaScale = (float)this.TotalAreaSize / base.Size.X;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x000388FC File Offset: 0x00036AFC
		private void HandlePositioning()
		{
			if (Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.Positioning)
				{
					Vector2 vector = base.EventManager.MousePosition - this._latestMousePosition;
					vector *= (float)this.TotalAreaSize / base.Size.X;
					Vector2 vector2 = new Vector2(this.PositionValue.X, this.PositionValue.Y);
					vector2 += vector;
					vector2 = new Vector2(MathF.Clamp(vector2.X, this._positionLimitMin, this._positionLimitMax), MathF.Clamp(vector2.Y, this._positionLimitMin, this._positionLimitMax));
					this.PositionValue = vector2;
					this.BannerTableauWidget.UpdatePositionValueManual = this.PositionValue;
				}
				if (this._currentMode != BannerBuilderEditableAreaWidget.BuilderMode.Positioning && base.EventManager.HoveredWidget == this)
				{
					this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.Positioning;
					return;
				}
			}
			else
			{
				this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.None;
			}
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x000389F8 File Offset: 0x00036BF8
		private void HandleRotation()
		{
			if (Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.Rotating)
				{
					Vec2 vec = base.GlobalPosition + this._centerOfSigil;
					Vector2 vector = base.EventManager.MousePosition - new Vector2(vec.X, vec.y);
					vector.Y *= -1f;
					float num = BannerBuilderEditableAreaWidget.AngleFromDir(vector);
					this.RotationValue = (float)Math.Round((double)num, 3);
					this.BannerTableauWidget.UpdateRotationValueManualWithMirror = new ValueTuple<float, bool>(this.RotationValue, this.IsMirrorActive);
				}
				if (this._currentMode != BannerBuilderEditableAreaWidget.BuilderMode.Rotating && base.EventManager.HoveredWidget == this.RotateWidget)
				{
					this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.Rotating;
				}
			}
			else
			{
				this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.None;
			}
			this.UpdatePositionOfWidget(this.RotateWidget, BannerBuilderEditableAreaWidget.WidgetPlacementType.Vertical, this.RotationValue, 55f, 30f);
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00038AEC File Offset: 0x00036CEC
		private void HandleForEdge(BannerBuilderEditableAreaWidget.EdgeResizeType resizeType)
		{
			ButtonWidget widgetFor = this.GetWidgetFor(resizeType);
			if (Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.HorizontalResizing || this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.VerticalResizing)
				{
					Vector2 vector = base.EventManager.MousePosition - this._resizeStartMousePosition;
					vector.Y *= -1f;
					Vec2 vec = BannerBuilderEditableAreaWidget.DirFromAngle(this.RotationValue);
					vec.y *= -1f;
					vector = BannerBuilderEditableAreaWidget.TransformToParent(vector, vec);
					vector.X *= -1f;
					vector.Y *= -1f;
					BannerBuilderEditableAreaWidget.BuilderMode currentMode = this._currentMode;
					if (currentMode != BannerBuilderEditableAreaWidget.BuilderMode.HorizontalResizing)
					{
						if (currentMode == BannerBuilderEditableAreaWidget.BuilderMode.VerticalResizing)
						{
							vector.X = 0f;
						}
					}
					else
					{
						vector.Y = 0f;
					}
					vector *= (float)this.TotalAreaSize / base.Size.X * 2f;
					Vec2 vec2 = new Vec2(this._resizeStartSize.X, this._resizeStartSize.Y);
					vec2 += vector;
					vec2 = new Vector2((float)((int)MathF.Clamp((float)((int)vec2.X), 2f, (float)this._sizeLimitMax)), (float)((int)MathF.Clamp((float)((int)vec2.Y), 2f, (float)this._sizeLimitMax)));
					Vec2 vec3 = this._resizeStartSize - vec2;
					if (vec3.x != 0f || vec3.y != 0f)
					{
						this.BannerTableauWidget.UpdateSizeValueManual = vec2;
						this.SizeValue = vec2;
					}
				}
				if ((this._currentMode != BannerBuilderEditableAreaWidget.BuilderMode.HorizontalResizing || this._currentMode != BannerBuilderEditableAreaWidget.BuilderMode.VerticalResizing) && base.EventManager.HoveredWidget == widgetFor)
				{
					this._resizeStartMousePosition = base.EventManager.MousePosition;
					this._resizeStartWidget = base.EventManager.HoveredWidget;
					this._resizeStartSize = this.SizeValue;
					this._currentMode = ((base.EventManager.HoveredWidget == this.GetWidgetFor(BannerBuilderEditableAreaWidget.EdgeResizeType.Right)) ? BannerBuilderEditableAreaWidget.BuilderMode.HorizontalResizing : BannerBuilderEditableAreaWidget.BuilderMode.VerticalResizing);
				}
			}
			else
			{
				this._resizeStartWidget = null;
				this._resizeStartSize = Vec2.Zero;
				this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.None;
			}
			this.UpdatePositionOfWidget(widgetFor, (resizeType == BannerBuilderEditableAreaWidget.EdgeResizeType.Right) ? BannerBuilderEditableAreaWidget.WidgetPlacementType.Horizontal : BannerBuilderEditableAreaWidget.WidgetPlacementType.Vertical, this.RotationValue + BannerBuilderEditableAreaWidget.AngleOffsetForEdge(resizeType), 15f, 0f);
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00038D34 File Offset: 0x00036F34
		private void HandleForCorner()
		{
			ButtonWidget dragWidgetTopRight = this.DragWidgetTopRight;
			if (Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				bool flag = Input.IsKeyDown(InputKey.LeftShift) || Input.IsKeyDown(InputKey.RightShift);
				if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.RightCornerResizing)
				{
					Vector2 vector = base.EventManager.MousePosition - this._resizeStartMousePosition;
					vector.Y *= -1f;
					vector *= (float)this.TotalAreaSize / base.Size.X * 2f;
					Vec2 vec = BannerBuilderEditableAreaWidget.DirFromAngle(this.RotationValue);
					vec.y *= -1f;
					vector = BannerBuilderEditableAreaWidget.TransformToParent(vector, vec);
					vector.X *= -1f;
					vector.Y *= -1f;
					Vec2 vec2 = new Vec2(this._resizeStartSize.X, this._resizeStartSize.Y);
					if (flag)
					{
						Vector2 vector2 = new Vector2(this._centerOfSigil.X, this._centerOfSigil.Y) + base.GlobalPosition;
						float num = (vector2 - this._resizeStartMousePosition).Length();
						bool flag2 = (vector2 - base.EventManager.MousePosition).Length() < num;
						float num2 = vector.Length() * this._areaScale * (float)(flag2 ? (-1) : 1);
						float length = this._resizeStartSize.Length;
						float num3 = num2 / length;
						vec2 += num3 * vec2 * this._areaScale / 4f;
					}
					else
					{
						vec2 += vector;
					}
					vec2 = new Vector2((float)((int)MathF.Clamp((float)((int)vec2.X), 2f, (float)this._sizeLimitMax)), (float)((int)MathF.Clamp((float)((int)vec2.Y), 2f, (float)this._sizeLimitMax)));
					Vec2 vec3 = this._resizeStartSize - vec2;
					if (vec3.x != 0f || vec3.y != 0f)
					{
						this.BannerTableauWidget.UpdateSizeValueManual = vec2;
						this.SizeValue = vec2;
					}
				}
				if (this._currentMode != BannerBuilderEditableAreaWidget.BuilderMode.RightCornerResizing && base.EventManager.HoveredWidget == dragWidgetTopRight)
				{
					if (!flag || this._resizeStartWidget == null)
					{
						this._resizeStartMousePosition = base.EventManager.MousePosition;
						this._resizeStartWidget = base.EventManager.HoveredWidget;
						this._resizeStartSize = this.SizeValue;
					}
					this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.RightCornerResizing;
				}
			}
			else
			{
				this._resizeStartWidget = null;
				this._resizeStartSize = Vec2.Zero;
				this._currentMode = BannerBuilderEditableAreaWidget.BuilderMode.None;
			}
			this.UpdatePositionOfWidget(dragWidgetTopRight, BannerBuilderEditableAreaWidget.WidgetPlacementType.Max, this.RotationValue + BannerBuilderEditableAreaWidget.AngleOffsetForCorner(), 20f, 0f);
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00038FF8 File Offset: 0x000371F8
		private void HandleCursor()
		{
			if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.Rotating || base.EventManager.HoveredWidget == this.RotateWidget)
			{
				base.Context.ActiveCursorOfContext = UIContext.MouseCursors.Rotate;
				return;
			}
			if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.Positioning || base.EventManager.HoveredWidget == this)
			{
				base.Context.ActiveCursorOfContext = UIContext.MouseCursors.Move;
				return;
			}
			if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.HorizontalResizing || base.EventManager.HoveredWidget == this.GetWidgetFor(BannerBuilderEditableAreaWidget.EdgeResizeType.Right))
			{
				base.Context.ActiveCursorOfContext = UIContext.MouseCursors.HorizontalResize;
				return;
			}
			if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.VerticalResizing || base.EventManager.HoveredWidget == this.GetWidgetFor(BannerBuilderEditableAreaWidget.EdgeResizeType.Top))
			{
				base.Context.ActiveCursorOfContext = UIContext.MouseCursors.VerticalResize;
				return;
			}
			if (this._currentMode == BannerBuilderEditableAreaWidget.BuilderMode.RightCornerResizing || base.EventManager.HoveredWidget == this.DragWidgetTopRight)
			{
				base.Context.ActiveCursorOfContext = UIContext.MouseCursors.DiagonalRightResize;
				return;
			}
			base.Context.ActiveCursorOfContext = UIContext.MouseCursors.Default;
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x000390DC File Offset: 0x000372DC
		private void UpdateEditableAreaVisual()
		{
			this.EditableAreaVisualWidget.HorizontalAlignment = HorizontalAlignment.Center;
			this.EditableAreaVisualWidget.VerticalAlignment = VerticalAlignment.Center;
			this.EditableAreaVisualWidget.WidthSizePolicy = SizePolicy.Fixed;
			this.EditableAreaVisualWidget.HeightSizePolicy = SizePolicy.Fixed;
			float num = (float)this.EditableAreaSize / (float)this.TotalAreaSize;
			this.EditableAreaVisualWidget.ScaledSuggestedWidth = base.Size.X * num;
			this.EditableAreaVisualWidget.ScaledSuggestedHeight = base.Size.Y * num;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00039159 File Offset: 0x00037359
		private ButtonWidget GetWidgetFor(BannerBuilderEditableAreaWidget.EdgeResizeType edgeResizeType)
		{
			if (edgeResizeType != BannerBuilderEditableAreaWidget.EdgeResizeType.Top)
			{
				return this.DragWidgetRight;
			}
			return this.DragWidgetTop;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0003916C File Offset: 0x0003736C
		private void UpdatePositionOfWidget(Widget widget, BannerBuilderEditableAreaWidget.WidgetPlacementType placementType, float directionFromCenter, float distanceFromCenterModifier, float distanceFromEdgesModifier = 0f)
		{
			Vec2 vec = BannerBuilderEditableAreaWidget.DirFromAngle(directionFromCenter);
			vec.y *= -1f;
			float num = 0f;
			switch (placementType)
			{
			case BannerBuilderEditableAreaWidget.WidgetPlacementType.Horizontal:
				num = this._sizeOfSigil.X;
				break;
			case BannerBuilderEditableAreaWidget.WidgetPlacementType.Vertical:
				num = this._sizeOfSigil.Y;
				break;
			case BannerBuilderEditableAreaWidget.WidgetPlacementType.Max:
				num = this._sizeOfSigil.Length;
				break;
			}
			float num2 = (num * base._inverseScaleToUse + distanceFromCenterModifier) * 0.5f * base._scaleToUse;
			Vec2 vec2 = this._centerOfSigil + vec * num2;
			vec2.x -= widget.Size.X / 2f;
			vec2.y -= widget.Size.Y / 2f;
			this.ApplyPositionOffsetToWidget(widget, vec2, distanceFromEdgesModifier * base._scaleToUse);
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00039248 File Offset: 0x00037448
		private void ApplyPositionOffsetToWidget(Widget widget, Vec2 pos, float additionalModifier = 0f)
		{
			widget.ScaledPositionXOffset = MathF.Clamp(pos.x, 12f + additionalModifier, base.Size.X - (12f + additionalModifier));
			widget.ScaledPositionYOffset = MathF.Clamp(pos.y, 12f + additionalModifier, base.Size.Y - (12f + additionalModifier));
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x000392AB File Offset: 0x000374AB
		private void OnIsLayerPatternChanged(bool isLayerPattern)
		{
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x000392AD File Offset: 0x000374AD
		private void OnPositionChanged(Vec2 newPosition)
		{
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x000392AF File Offset: 0x000374AF
		private void OnSizeChanged(Vec2 newSize)
		{
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x000392B1 File Offset: 0x000374B1
		private void OnRotationChanged(float newRotation)
		{
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060014D5 RID: 5333 RVA: 0x000392B3 File Offset: 0x000374B3
		// (set) Token: 0x060014D6 RID: 5334 RVA: 0x000392BB File Offset: 0x000374BB
		[Editor(false)]
		public bool IsLayerPattern
		{
			get
			{
				return this._isLayerPattern;
			}
			set
			{
				if (this._isLayerPattern != value)
				{
					this._isLayerPattern = value;
					base.OnPropertyChanged(value, "IsLayerPattern");
					this.OnIsLayerPatternChanged(value);
				}
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x060014D7 RID: 5335 RVA: 0x000392E0 File Offset: 0x000374E0
		// (set) Token: 0x060014D8 RID: 5336 RVA: 0x000392E8 File Offset: 0x000374E8
		[Editor(false)]
		public Vec2 PositionValue
		{
			get
			{
				return this._positionValue;
			}
			set
			{
				if (this._positionValue != value)
				{
					this._positionValue = value;
					base.OnPropertyChanged(value, "PositionValue");
					this.OnPositionChanged(value);
				}
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x060014D9 RID: 5337 RVA: 0x00039312 File Offset: 0x00037512
		// (set) Token: 0x060014DA RID: 5338 RVA: 0x0003931A File Offset: 0x0003751A
		[Editor(false)]
		public Vec2 SizeValue
		{
			get
			{
				return this._sizeValue;
			}
			set
			{
				if (this._sizeValue != value)
				{
					this._sizeValue = value;
					base.OnPropertyChanged(value, "SizeValue");
					this.OnSizeChanged(value);
				}
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x00039344 File Offset: 0x00037544
		// (set) Token: 0x060014DC RID: 5340 RVA: 0x0003934C File Offset: 0x0003754C
		[Editor(false)]
		public float RotationValue
		{
			get
			{
				return this._rotationValue;
			}
			set
			{
				if (this._rotationValue != value)
				{
					this._rotationValue = value;
					base.OnPropertyChanged(value, "RotationValue");
					this.OnRotationChanged(value);
				}
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x00039371 File Offset: 0x00037571
		// (set) Token: 0x060014DE RID: 5342 RVA: 0x00039379 File Offset: 0x00037579
		[Editor(false)]
		public int EditableAreaSize
		{
			get
			{
				return this._editableAreaSize;
			}
			set
			{
				if (this._editableAreaSize != value)
				{
					this._editableAreaSize = value;
					base.OnPropertyChanged(value, "EditableAreaSize");
				}
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x00039397 File Offset: 0x00037597
		// (set) Token: 0x060014E0 RID: 5344 RVA: 0x0003939F File Offset: 0x0003759F
		[Editor(false)]
		public int TotalAreaSize
		{
			get
			{
				return this._totalAreaSize;
			}
			set
			{
				if (this._totalAreaSize != value)
				{
					this._totalAreaSize = value;
					base.OnPropertyChanged(value, "TotalAreaSize");
				}
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x000393C0 File Offset: 0x000375C0
		private static Vec2 DirFromAngle(float angle)
		{
			float num = angle * 6.2831855f;
			return new Vec2(-MathF.Sin(num), MathF.Cos(num));
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x000393EC File Offset: 0x000375EC
		private static float AngleFromDir(Vec2 directionVector)
		{
			float num;
			if (directionVector.X < 0f)
			{
				num = (float)Math.Atan2((double)directionVector.X, (double)directionVector.Y) * 57.29578f * -1f;
			}
			else
			{
				num = 360f - (float)Math.Atan2((double)directionVector.X, (double)directionVector.Y) * 57.29578f;
			}
			return num / 360f;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00039456 File Offset: 0x00037656
		private static float AngleOffsetForEdge(BannerBuilderEditableAreaWidget.EdgeResizeType edge)
		{
			return 1f - (float)edge * 0.25f;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00039466 File Offset: 0x00037666
		private static float AngleOffsetForCorner()
		{
			return 0.875f;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00039470 File Offset: 0x00037670
		private static Vec2 TransformToParent(Vec2 a, Vec2 b)
		{
			return new Vec2(b.Y * a.X + b.X * a.Y, -b.X * a.X + b.Y * a.Y);
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x000394C4 File Offset: 0x000376C4
		private static Vector2 TransformToParent(Vector2 a, Vec2 b)
		{
			return new Vector2(b.Y * a.X + b.X * a.Y, -b.X * a.X + b.Y * a.Y);
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x00039514 File Offset: 0x00037714
		private static Vector2 TransformToParent(Vec2 a, Vector2 b)
		{
			return new Vector2(b.Y * a.X + b.X * a.Y, -b.X * a.X + b.Y * a.Y);
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x00039561 File Offset: 0x00037761
		private static Vector2 TransformToParent(Vector2 a, Vector2 b)
		{
			return new Vector2(b.Y * a.X + b.X * a.Y, -b.X * a.Y + b.Y * a.Y);
		}

		// Token: 0x0400096B RID: 2411
		private bool _initialized;

		// Token: 0x0400096C RID: 2412
		private Vec2 _centerOfSigil;

		// Token: 0x0400096D RID: 2413
		private Vec2 _sizeOfSigil;

		// Token: 0x0400096E RID: 2414
		private float _positionLimitMin;

		// Token: 0x0400096F RID: 2415
		private float _positionLimitMax;

		// Token: 0x04000970 RID: 2416
		private float _areaScale;

		// Token: 0x04000971 RID: 2417
		private const int _sizeLimitMin = 2;

		// Token: 0x04000972 RID: 2418
		private int _sizeLimitMax;

		// Token: 0x04000973 RID: 2419
		private BannerBuilderEditableAreaWidget.BuilderMode _currentMode;

		// Token: 0x04000974 RID: 2420
		private Vector2 _latestMousePosition;

		// Token: 0x04000975 RID: 2421
		private Vector2 _resizeStartMousePosition;

		// Token: 0x04000976 RID: 2422
		private Widget _resizeStartWidget;

		// Token: 0x04000977 RID: 2423
		private Vec2 _resizeStartSize;

		// Token: 0x04000978 RID: 2424
		private bool _isLayerPattern;

		// Token: 0x04000979 RID: 2425
		private Vec2 _positionValue;

		// Token: 0x0400097A RID: 2426
		private Vec2 _sizeValue;

		// Token: 0x0400097B RID: 2427
		private float _rotationValue;

		// Token: 0x0400097C RID: 2428
		private int _editableAreaSize;

		// Token: 0x0400097D RID: 2429
		private int _totalAreaSize;

		// Token: 0x020001D7 RID: 471
		private enum BuilderMode
		{
			// Token: 0x04000A5E RID: 2654
			None,
			// Token: 0x04000A5F RID: 2655
			Rotating,
			// Token: 0x04000A60 RID: 2656
			Positioning,
			// Token: 0x04000A61 RID: 2657
			HorizontalResizing,
			// Token: 0x04000A62 RID: 2658
			VerticalResizing,
			// Token: 0x04000A63 RID: 2659
			RightCornerResizing
		}

		// Token: 0x020001D8 RID: 472
		private enum WidgetPlacementType
		{
			// Token: 0x04000A65 RID: 2661
			Horizontal,
			// Token: 0x04000A66 RID: 2662
			Vertical,
			// Token: 0x04000A67 RID: 2663
			Max
		}

		// Token: 0x020001D9 RID: 473
		private enum EdgeResizeType
		{
			// Token: 0x04000A69 RID: 2665
			Top,
			// Token: 0x04000A6A RID: 2666
			Right
		}
	}
}
