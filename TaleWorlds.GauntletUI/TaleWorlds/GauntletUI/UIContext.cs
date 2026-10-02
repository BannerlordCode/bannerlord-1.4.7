using System;
using System.Collections.Generic;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GauntletInput;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000033 RID: 51
	public class UIContext
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0000F1F9 File Offset: 0x0000D3F9
		// (set) Token: 0x0600036B RID: 875 RVA: 0x0000F201 File Offset: 0x0000D401
		public UIContext.MouseCursors ActiveCursorOfContext { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0000F20A File Offset: 0x0000D40A
		// (set) Token: 0x0600036D RID: 877 RVA: 0x0000F212 File Offset: 0x0000D412
		public bool IsDynamicScaleEnabled { get; set; } = true;

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0000F21B File Offset: 0x0000D41B
		// (set) Token: 0x0600036F RID: 879 RVA: 0x0000F223 File Offset: 0x0000D423
		public float ScaleModifier { get; set; } = 1f;

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000F22C File Offset: 0x0000D42C
		// (set) Token: 0x06000371 RID: 881 RVA: 0x0000F234 File Offset: 0x0000D434
		public string Name { get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000F23D File Offset: 0x0000D43D
		// (set) Token: 0x06000373 RID: 883 RVA: 0x0000F245 File Offset: 0x0000D445
		public bool IsActive { get; private set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000F24E File Offset: 0x0000D44E
		// (set) Token: 0x06000375 RID: 885 RVA: 0x0000F256 File Offset: 0x0000D456
		public float ContextAlpha { get; set; } = 1f;

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0000F25F File Offset: 0x0000D45F
		// (set) Token: 0x06000377 RID: 887 RVA: 0x0000F267 File Offset: 0x0000D467
		public float Scale { get; private set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0000F270 File Offset: 0x0000D470
		// (set) Token: 0x06000379 RID: 889 RVA: 0x0000F278 File Offset: 0x0000D478
		public float CustomScale { get; private set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000F281 File Offset: 0x0000D481
		// (set) Token: 0x0600037B RID: 891 RVA: 0x0000F289 File Offset: 0x0000D489
		public float CustomInverseScale { get; private set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000F292 File Offset: 0x0000D492
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0000F29A File Offset: 0x0000D49A
		public string CurrentLanugageCode { get; private set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0000F2A3 File Offset: 0x0000D4A3
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0000F2AB File Offset: 0x0000D4AB
		public Random UIRandom { get; private set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000F2B4 File Offset: 0x0000D4B4
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000F2BC File Offset: 0x0000D4BC
		public float InverseScale { get; private set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000F2C5 File Offset: 0x0000D4C5
		// (set) Token: 0x06000383 RID: 899 RVA: 0x0000F2CD File Offset: 0x0000D4CD
		public EventManager EventManager { get; private set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0000F2D6 File Offset: 0x0000D4D6
		public Widget Root
		{
			get
			{
				return this.EventManager.Root;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0000F2E3 File Offset: 0x0000D4E3
		public ResourceDepot ResourceDepot
		{
			get
			{
				return this.TwoDimensionContext.ResourceDepot;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000F2F0 File Offset: 0x0000D4F0
		// (set) Token: 0x06000387 RID: 903 RVA: 0x0000F2F8 File Offset: 0x0000D4F8
		public TwoDimensionContext TwoDimensionContext { get; private set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0000F301 File Offset: 0x0000D501
		public IEnumerable<Brush> Brushes
		{
			get
			{
				return this.BrushFactory.Brushes;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000F30E File Offset: 0x0000D50E
		public Brush DefaultBrush
		{
			get
			{
				return this.BrushFactory.DefaultBrush;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000F31B File Offset: 0x0000D51B
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0000F323 File Offset: 0x0000D523
		public SpriteData SpriteData { get; private set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0000F32C File Offset: 0x0000D52C
		// (set) Token: 0x0600038D RID: 909 RVA: 0x0000F334 File Offset: 0x0000D534
		public BrushFactory BrushFactory { get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000F33D File Offset: 0x0000D53D
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0000F345 File Offset: 0x0000D545
		public FontFactory FontFactory { get; private set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000F34E File Offset: 0x0000D54E
		public IReadonlyInputContext InputContext
		{
			get
			{
				return this._uiInputContext;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000F356 File Offset: 0x0000D556
		// (set) Token: 0x06000392 RID: 914 RVA: 0x0000F35E File Offset: 0x0000D55E
		public IGamepadNavigationContext GamepadNavigation { get; private set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000F367 File Offset: 0x0000D567
		// (set) Token: 0x06000394 RID: 916 RVA: 0x0000F36F File Offset: 0x0000D56F
		public ulong LocalFrameNumber { get; private set; }

		// Token: 0x06000395 RID: 917 RVA: 0x0000F378 File Offset: 0x0000D578
		public UIContext(TwoDimensionContext twoDimensionContext, IInputContext inputContext, SpriteData spriteData, FontFactory fontFactory, BrushFactory brushFactory)
		{
			this._isMouseEnabled = true;
			this._inputContext = inputContext;
			this._initializedWithExistingResources = true;
			this._uiInputContext = new GauntletInputContext(inputContext);
			this.TwoDimensionContext = twoDimensionContext;
			this.GamepadNavigation = new EmptyGamepadNavigationContext();
			this.SpriteData = spriteData;
			this.FontFactory = fontFactory;
			this.BrushFactory = brushFactory;
			this.ReferenceHeight = twoDimensionContext.Platform.ReferenceHeight;
			this.InverseReferenceHeight = 1f / this.ReferenceHeight;
			this.ReferenceAspectRatio = twoDimensionContext.Platform.ReferenceWidth / twoDimensionContext.Platform.ReferenceHeight;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000F434 File Offset: 0x0000D634
		public UIContext(TwoDimensionContext twoDimensionContext, IInputContext inputContext)
		{
			this._isMouseEnabled = true;
			this._initializedWithExistingResources = false;
			this._inputContext = inputContext;
			this._uiInputContext = new GauntletInputContext(inputContext);
			this.TwoDimensionContext = twoDimensionContext;
			this.GamepadNavigation = new EmptyGamepadNavigationContext();
			this.ReferenceHeight = twoDimensionContext.Platform.ReferenceHeight;
			this.InverseReferenceHeight = 1f / this.ReferenceHeight;
			this.ReferenceAspectRatio = twoDimensionContext.Platform.ReferenceWidth / twoDimensionContext.Platform.ReferenceHeight;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000F4D8 File Offset: 0x0000D6D8
		public void Initialize()
		{
			if (!this._initializedWithExistingResources)
			{
				this.SpriteData = new SpriteData("SpriteData");
				this.SpriteData.Load(this.ResourceDepot);
				this.FontFactory = new FontFactory(this.ResourceDepot);
				this.FontFactory.LoadAllFonts(this.SpriteData);
				this.BrushFactory = new BrushFactory(this.ResourceDepot, "Brushes", this.SpriteData, this.FontFactory);
				this.BrushFactory.Initialize();
			}
			this.EventManager = new EventManager(this);
			Widget root = this.Root;
			root.WidthSizePolicy = SizePolicy.Fixed;
			root.HeightSizePolicy = SizePolicy.Fixed;
			root.SuggestedWidth = this.TwoDimensionContext.Width;
			root.SuggestedHeight = this.TwoDimensionContext.Height;
			this.UIRandom = new Random();
			this.UpdateScale();
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000F5AF File Offset: 0x0000D7AF
		public Brush GetBrush(string name)
		{
			return this.BrushFactory.GetBrush(name);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000F5BD File Offset: 0x0000D7BD
		public void RefreshResources(SpriteData spriteData, FontFactory fontFactory, BrushFactory brushFactory)
		{
			this.SpriteData = spriteData;
			this.FontFactory = fontFactory;
			this.BrushFactory = brushFactory;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000F5D4 File Offset: 0x0000D7D4
		public void OnFinalize()
		{
			this.GamepadNavigation.OnFinalize();
			this.EventManager.OnFinalize();
			this.GamepadNavigation.OnFinalize();
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000F5F7 File Offset: 0x0000D7F7
		public void Deactivate()
		{
			this.IsActive = false;
			this.EventManager.OnContextDeactivated();
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000F60B File Offset: 0x0000D80B
		public void Activate()
		{
			this.IsActive = true;
			this.EventManager.OnContextActivated();
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000F620 File Offset: 0x0000D820
		public void Update(float dt)
		{
			this.ActiveCursorOfContext = UIContext.MouseCursors.Default;
			if (!this._initializedWithExistingResources)
			{
				this.BrushFactory.CheckForUpdates();
			}
			if (this.IsDynamicScaleEnabled)
			{
				this.UpdateScale();
			}
			Widget root = this.Root;
			root.SuggestedWidth = this.TwoDimensionContext.Width;
			root.SuggestedHeight = this.TwoDimensionContext.Height;
			this.EventManager.Update(dt);
			ulong localFrameNumber = this.LocalFrameNumber;
			this.LocalFrameNumber = localFrameNumber + 1UL;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000F69C File Offset: 0x0000D89C
		public void LateUpdate(float dt)
		{
			Vector2 vector = new Vector2(this.TwoDimensionContext.Width, this.TwoDimensionContext.Height);
			this.EventManager.CalculateCanvas(vector, dt);
			this.EventManager.LateUpdate(dt);
			this.EventManager.RecalculateCanvas();
			this.EventManager.DefragContainers();
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000F6F5 File Offset: 0x0000D8F5
		public void RenderTick(float dt)
		{
			this.EventManager.UpdateBrushes(dt);
			this.EventManager.Render(this.TwoDimensionContext);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000F714 File Offset: 0x0000D914
		public void OnOnScreenkeyboardTextInputDone(string inputText)
		{
			EditableTextWidget editableTextWidget;
			if ((editableTextWidget = this.EventManager.FocusedWidget as EditableTextWidget) != null)
			{
				editableTextWidget.SetAllText(inputText);
			}
			this.CancelMouseClick();
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000F742 File Offset: 0x0000D942
		public void InitializeGamepadNavigation(IGamepadNavigationContext context)
		{
			this.GamepadNavigation = context;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000F74C File Offset: 0x0000D94C
		private void UpdateScale()
		{
			float num;
			if (this.TwoDimensionContext != null)
			{
				num = this.TwoDimensionContext.Height * this.InverseReferenceHeight;
				float num2 = this.TwoDimensionContext.Width / this.TwoDimensionContext.Height;
				if (num2 < this.ReferenceAspectRatio * 0.98f)
				{
					float num3 = num2 / (this.ReferenceAspectRatio * 0.98f);
					num *= num3;
				}
			}
			else
			{
				num = 1f;
			}
			if (this.Scale != num || this.CustomScale != this.Scale * this.ScaleModifier)
			{
				this.Scale = num;
				this.CustomScale = this.Scale * this.ScaleModifier;
				this.InverseScale = 1f / num;
				this.CustomInverseScale = 1f / this.CustomScale;
				this.EventManager.UpdateLayout();
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000F81E File Offset: 0x0000DA1E
		public void OnOnScreenKeyboardCanceled()
		{
			this.CancelMouseClick();
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000F826 File Offset: 0x0000DA26
		public bool HitTest(Widget root, Vector2 position)
		{
			return EventManager.HitTest(root, position);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000F82F File Offset: 0x0000DA2F
		public bool HitTest(Widget root)
		{
			return root != null && EventManager.HitTest(root, this._uiInputContext.GetMousePosition());
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000F847 File Offset: 0x0000DA47
		public bool FocusTest(Widget root)
		{
			return this.EventManager.FocusTest(root);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000F855 File Offset: 0x0000DA55
		public void SetIsMouseEnabled(bool isMouseEnabled)
		{
			this._isMouseEnabled = isMouseEnabled;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000F860 File Offset: 0x0000DA60
		public void UpdateInput(InputType handleInputs)
		{
			if (this._isMouseEnabled || this.EventManager.FocusedWidget != null)
			{
				if (handleInputs.HasAnyFlag(InputType.MouseButton))
				{
					this.EventManager.MouseMove();
					foreach (InputKey inputKey in this._inputContext.GetClickKeys())
					{
						if (this._inputContext.IsKeyPressed(inputKey))
						{
							this.EventManager.MouseDown();
							break;
						}
					}
					InputKey[] clickKeys;
					foreach (InputKey inputKey2 in clickKeys)
					{
						if (this._inputContext.IsKeyReleased(inputKey2))
						{
							this.EventManager.MouseUp(true);
							break;
						}
					}
					if (this._inputContext.IsKeyPressed(InputKey.RightMouseButton))
					{
						this.EventManager.MouseAlternateDown();
					}
					if (this._inputContext.IsKeyReleased(InputKey.RightMouseButton))
					{
						this.EventManager.MouseAlternateUp(true);
					}
				}
				if (handleInputs.HasAnyFlag(InputType.MouseWheel))
				{
					this.EventManager.MouseScroll();
				}
				this.EventManager.RightStickMovement();
				this._previousFrameMouseEnabled = true;
				return;
			}
			if (this._previousFrameMouseEnabled)
			{
				this.CancelMouseClick();
				this._previousFrameMouseEnabled = false;
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000F97C File Offset: 0x0000DB7C
		public void OnMovieLoaded(string movieName)
		{
			this.GamepadNavigation.OnMovieLoaded(movieName);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000F98A File Offset: 0x0000DB8A
		public void OnMovieReleased(string movieName)
		{
			this.GamepadNavigation.OnMovieReleased(movieName);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000F998 File Offset: 0x0000DB98
		public void CancelMouseClick()
		{
			this.EventManager.MouseUp(false);
			this.EventManager.MouseAlternateUp(false);
			this.EventManager.ClearFocus();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000F9C0 File Offset: 0x0000DBC0
		public void DrawWidgetDebugInfo()
		{
			if (Input.IsKeyDown(InputKey.LeftShift) && Input.IsKeyPressed(InputKey.F))
			{
				this.IsDebugWidgetInformationFroze = !this.IsDebugWidgetInformationFroze;
				this._currentRootNode = new UIContext.DebugWidgetTreeNode(this.TwoDimensionContext, this.Root, 0);
			}
			if (this.IsDebugWidgetInformationFroze)
			{
				UIContext.DebugWidgetTreeNode currentRootNode = this._currentRootNode;
				if (currentRootNode == null)
				{
					return;
				}
				currentRootNode.DebugDraw();
			}
		}

		// Token: 0x040001AC RID: 428
		private readonly float ReferenceHeight;

		// Token: 0x040001AD RID: 429
		private readonly float InverseReferenceHeight;

		// Token: 0x040001AE RID: 430
		private readonly float ReferenceAspectRatio;

		// Token: 0x040001AF RID: 431
		private const float ReferenceAspectRatioCoeff = 0.98f;

		// Token: 0x040001C0 RID: 448
		private readonly GauntletInputContext _uiInputContext;

		// Token: 0x040001C1 RID: 449
		private readonly IInputContext _inputContext;

		// Token: 0x040001C4 RID: 452
		private bool _initializedWithExistingResources;

		// Token: 0x040001C5 RID: 453
		private bool _previousFrameMouseEnabled;

		// Token: 0x040001C6 RID: 454
		private bool _isMouseEnabled;

		// Token: 0x040001C7 RID: 455
		private bool IsDebugWidgetInformationFroze;

		// Token: 0x040001C8 RID: 456
		private UIContext.DebugWidgetTreeNode _currentRootNode;

		// Token: 0x02000081 RID: 129
		public enum MouseCursors
		{
			// Token: 0x0400044C RID: 1100
			System,
			// Token: 0x0400044D RID: 1101
			Default,
			// Token: 0x0400044E RID: 1102
			Attack,
			// Token: 0x0400044F RID: 1103
			Move,
			// Token: 0x04000450 RID: 1104
			HorizontalResize,
			// Token: 0x04000451 RID: 1105
			VerticalResize,
			// Token: 0x04000452 RID: 1106
			DiagonalRightResize,
			// Token: 0x04000453 RID: 1107
			DiagonalLeftResize,
			// Token: 0x04000454 RID: 1108
			Rotate,
			// Token: 0x04000455 RID: 1109
			Custom,
			// Token: 0x04000456 RID: 1110
			Disabled,
			// Token: 0x04000457 RID: 1111
			RightClickLink
		}

		// Token: 0x02000082 RID: 130
		private class DebugWidgetTreeNode
		{
			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00023377 File Offset: 0x00021577
			private string ID
			{
				get
				{
					return string.Format("{0}.{1}.{2}", this._depth, this._current.GetSiblingIndex(), this._fullIDPath);
				}
			}

			// Token: 0x060008F3 RID: 2291 RVA: 0x000233A4 File Offset: 0x000215A4
			public DebugWidgetTreeNode(TwoDimensionContext context, Widget current, int depth)
			{
				this._context = context;
				this._current = current;
				this._depth = depth;
				Widget current2 = this._current;
				this._fullIDPath = ((current2 != null) ? current2.GetFullIDPath() : null) ?? string.Empty;
				int num = this._fullIDPath.LastIndexOf('\\');
				if (num != -1)
				{
					this._displayedName = this._fullIDPath.Substring(num + 1);
				}
				if (string.IsNullOrEmpty(this._displayedName))
				{
					this._displayedName = this._current.Id;
				}
				this._children = new List<UIContext.DebugWidgetTreeNode>();
				this.AddChildren();
			}

			// Token: 0x060008F4 RID: 2292 RVA: 0x00023444 File Offset: 0x00021644
			private void AddChildren()
			{
				foreach (Widget widget in this._current.Children)
				{
					if (widget.ParentWidget == this._current)
					{
						UIContext.DebugWidgetTreeNode debugWidgetTreeNode = new UIContext.DebugWidgetTreeNode(this._context, widget, this._depth + 1);
						this._children.Add(debugWidgetTreeNode);
					}
				}
			}

			// Token: 0x060008F5 RID: 2293 RVA: 0x000234C4 File Offset: 0x000216C4
			public void DebugDraw()
			{
				if (this._context.DrawDebugTreeNode(this._displayedName + "###Root." + this.ID))
				{
					if (this._context.IsDebugItemHovered())
					{
						this.DrawArea();
					}
					this._context.DrawCheckbox("Show Area###Area." + this.ID, ref this._isShowingArea);
					if (this._isShowingArea)
					{
						this.DrawArea();
					}
					this.DrawProperties();
					this.DrawChildren();
					this._context.PopDebugTreeNode();
					return;
				}
				if (this._context.IsDebugItemHovered())
				{
					this.DrawArea();
				}
			}

			// Token: 0x060008F6 RID: 2294 RVA: 0x00023564 File Offset: 0x00021764
			private void DrawProperties()
			{
				if (this._context.DrawDebugTreeNode("Properties###Properties." + this.ID))
				{
					this._context.DrawDebugText("General");
					string text = (string.IsNullOrEmpty(this._current.Id) ? "_No ID_" : this._current.Id);
					this._context.DrawDebugText("\tID: " + text);
					this._context.DrawDebugText("\tPath: " + this._current.GetFullIDPath());
					this._context.DrawDebugText(string.Format("\tVisible: {0}", this._current.IsVisible));
					this._context.DrawDebugText(string.Format("\tEnabled: {0}", this._current.IsEnabled));
					this._context.DrawDebugText("\nSize");
					this._context.DrawDebugText(string.Format("\tWidth Size Policy: {0}", this._current.WidthSizePolicy));
					this._context.DrawDebugText(string.Format("\tHeight Size Policy: {0}", this._current.HeightSizePolicy));
					this._context.DrawDebugText(string.Format("\tSize: {0}", this._current.Size));
					this._context.DrawDebugText("\nPosition");
					this._context.DrawDebugText(string.Format("\tGlobal Position: {0}", this._current.GlobalPosition));
					this._context.DrawDebugText(string.Format("\tLocal Position: {0}", this._current.LocalPosition));
					this._context.DrawDebugText(string.Format("\tPosition Offset: <{0}, {1}>", this._current.PositionXOffset, this._current.PositionYOffset));
					this._context.DrawDebugText("\nEvents");
					this._context.DrawDebugText("\tCurrent State: " + this._current.CurrentState);
					this._context.DrawDebugText(string.Format("\tCan Accept Events: {0}", this._current.CanAcceptEvents));
					this._context.DrawDebugText(string.Format("\tPasses Events To Children: {0}", !this._current.DoNotPassEventsToChildren));
					this._context.DrawDebugText("\nVisuals");
					BrushWidget brushWidget = this._current as BrushWidget;
					if (brushWidget != null)
					{
						this._context.DrawDebugText("\tBrush: " + brushWidget.Brush.Name);
					}
					TextWidget textWidget;
					RichTextWidget richTextWidget;
					if ((textWidget = this._current as TextWidget) != null)
					{
						this._context.DrawDebugText("\tText: " + textWidget.Text);
					}
					else if ((richTextWidget = this._current as RichTextWidget) != null)
					{
						this._context.DrawDebugText("\tText: " + richTextWidget.Text);
					}
					else if (brushWidget != null)
					{
						TwoDimensionContext context = this._context;
						string text2 = "\tSprite: ";
						BrushRenderer brushRenderer = brushWidget.BrushRenderer;
						string text3;
						if (brushRenderer == null)
						{
							text3 = null;
						}
						else
						{
							Style currentStyle = brushRenderer.CurrentStyle;
							if (currentStyle == null)
							{
								text3 = null;
							}
							else
							{
								StyleLayer layer = currentStyle.GetLayer(brushWidget.BrushRenderer.CurrentState);
								if (layer == null)
								{
									text3 = null;
								}
								else
								{
									Sprite sprite = layer.Sprite;
									text3 = ((sprite != null) ? sprite.Name : null);
								}
							}
						}
						context.DrawDebugText(text2 + (text3 ?? "None"));
						TwoDimensionContext context2 = this._context;
						string text4 = "\tColor: ";
						Brush brush = brushWidget.Brush;
						string text5;
						if (brush == null)
						{
							text5 = null;
						}
						else
						{
							BrushLayer layer2 = brush.GetLayer(brushWidget.CurrentState);
							text5 = ((layer2 != null) ? layer2.ToString() : null);
						}
						context2.DrawDebugText(text4 + text5);
					}
					else
					{
						TwoDimensionContext context3 = this._context;
						string text6 = "\tSprite: ";
						Sprite sprite2 = this._current.Sprite;
						context3.DrawDebugText(text6 + (((sprite2 != null) ? sprite2.Name : null) ?? "None"));
						this._context.DrawDebugText("\tColor: " + this._current.Color.ToString());
					}
					this._context.PopDebugTreeNode();
				}
			}

			// Token: 0x060008F7 RID: 2295 RVA: 0x00023980 File Offset: 0x00021B80
			private void DrawChildren()
			{
				if (this._children.Count > 0 && this._context.DrawDebugTreeNode("Children###Children." + this.ID))
				{
					foreach (UIContext.DebugWidgetTreeNode debugWidgetTreeNode in this._children)
					{
						debugWidgetTreeNode.DebugDraw();
					}
					this._context.PopDebugTreeNode();
				}
			}

			// Token: 0x060008F8 RID: 2296 RVA: 0x00023A08 File Offset: 0x00021C08
			private void DrawArea()
			{
				float x = this._current.GlobalPosition.X;
				float y = this._current.GlobalPosition.Y;
				float num = this._current.GlobalPosition.X + this._current.Size.X;
				float num2 = this._current.GlobalPosition.Y + this._current.Size.Y;
				if (x == num || y == num2 || this._current.Size.X == 0f || this._current.Size.Y == 0f)
				{
					return;
				}
				float num3 = 2f;
				float num4 = num3 / 2f;
				float num5 = num3 / 2f;
				float num6 = num3 / 2f;
				float num7 = num3 / 2f;
				float num8 = num3 / 2f;
				float num9 = num3 / 2f;
				float num10 = num3 / 2f;
				float num11 = num3 / 2f;
			}

			// Token: 0x04000458 RID: 1112
			private readonly TwoDimensionContext _context;

			// Token: 0x04000459 RID: 1113
			private readonly Widget _current;

			// Token: 0x0400045A RID: 1114
			private readonly List<UIContext.DebugWidgetTreeNode> _children;

			// Token: 0x0400045B RID: 1115
			private readonly string _fullIDPath;

			// Token: 0x0400045C RID: 1116
			private readonly string _displayedName;

			// Token: 0x0400045D RID: 1117
			private readonly int _depth;

			// Token: 0x0400045E RID: 1118
			private bool _isShowingArea;
		}
	}
}
