using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameMenu
{
	// Token: 0x02000153 RID: 339
	public class GameMenuWidget : Widget
	{
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x00031D25 File Offset: 0x0002FF25
		// (set) Token: 0x060011F7 RID: 4599 RVA: 0x00031D2D File Offset: 0x0002FF2D
		public int EncounterModeMenuWidth { get; set; }

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x00031D36 File Offset: 0x0002FF36
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x00031D3E File Offset: 0x0002FF3E
		public int EncounterModeMenuHeight { get; set; }

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x00031D47 File Offset: 0x0002FF47
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x00031D4F File Offset: 0x0002FF4F
		public int EncounterModeMenuMarginTop { get; set; }

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x00031D58 File Offset: 0x0002FF58
		// (set) Token: 0x060011FD RID: 4605 RVA: 0x00031D60 File Offset: 0x0002FF60
		public int NormalModeMenuWidth { get; set; }

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x00031D69 File Offset: 0x0002FF69
		// (set) Token: 0x060011FF RID: 4607 RVA: 0x00031D71 File Offset: 0x0002FF71
		public int NormalModeMenuHeight { get; set; }

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x00031D7A File Offset: 0x0002FF7A
		// (set) Token: 0x06001201 RID: 4609 RVA: 0x00031D82 File Offset: 0x0002FF82
		public int NormalModeMenuMarginTop { get; set; }

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x00031D8B File Offset: 0x0002FF8B
		// (set) Token: 0x06001203 RID: 4611 RVA: 0x00031D93 File Offset: 0x0002FF93
		public bool IsOverlayExtended
		{
			get
			{
				return this._isOverlayExtended;
			}
			private set
			{
				if (value != this._isOverlayExtended)
				{
					this._isOverlayExtended = value;
					this.UpdateOverlayState();
				}
			}
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00031DAB File Offset: 0x0002FFAB
		public GameMenuWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00031DC4 File Offset: 0x0002FFC4
		protected override void OnLateUpdate(float dt)
		{
			if (!this._firstFrame)
			{
				if (this.IsNight)
				{
					base.Color = Color.Lerp(base.Color, new Color(0.23921569f, 0.4509804f, 0.8f, 1f), dt);
				}
				else
				{
					base.Color = Color.Lerp(base.Color, Color.White, dt);
				}
			}
			else
			{
				if (this.IsNight)
				{
					base.Color = new Color(0.23921569f, 0.4509804f, 0.8f, 1f);
				}
				else
				{
					base.Color = Color.White;
				}
				this._firstFrame = false;
				this.RefreshSize();
			}
			if (base.Sprite == null && this.OverriddenSpriteMapBrush != null && this.SpriteName != null)
			{
				BrushLayer layer = this.OverriddenSpriteMapBrush.GetLayer(this.SpriteName);
				base.Sprite = ((layer != null) ? layer.Sprite : null);
			}
			base.OnLateUpdate(dt);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00031EAC File Offset: 0x000300AC
		private void RefreshSize()
		{
			base.SuggestedWidth = (float)(this.IsEncounterMenu ? this.EncounterModeMenuWidth : this.NormalModeMenuWidth);
			base.SuggestedHeight = (float)(this.IsEncounterMenu ? this.EncounterModeMenuHeight : this.NormalModeMenuHeight);
			base.ScaledSuggestedWidth = base.SuggestedWidth * base._scaleToUse;
			base.ScaledSuggestedHeight = base.SuggestedHeight * base._scaleToUse;
			base.MarginTop = (float)(this.IsEncounterMenu ? this.EncounterModeMenuMarginTop : this.NormalModeMenuMarginTop);
			this.ExtendButtonWidget.MarginTop = base.MarginTop;
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x00031F47 File Offset: 0x00030147
		private void OnExtendButtonClick(Widget button)
		{
			this.IsOverlayExtended = !this.IsOverlayExtended;
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x00031F58 File Offset: 0x00030158
		public void UpdateOverlayState()
		{
			this.ScopeTargeter.IsScopeEnabled = this._isOverlayExtended;
			string text = (this._isOverlayExtended ? "Default" : "Disabled");
			this.Overlay.SetState(text);
			foreach (Style style in this.ExtendButtonArrowWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].HorizontalFlip = !this._isOverlayExtended;
				}
			}
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00032004 File Offset: 0x00030204
		private void TitleTextWidget_PropertyChanged(PropertyOwnerObject widget, string propertyName, object propertyValue)
		{
			if (propertyName == "Text")
			{
				this.TitleContainerWidget.IsVisible = !string.IsNullOrEmpty((string)propertyValue);
				this.IsOverlayExtended = true;
			}
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x00032033 File Offset: 0x00030233
		private void OnOptionAdded(Widget parentWidget, Widget childWidget)
		{
			GameMenuItemWidget gameMenuItemWidget = childWidget as GameMenuItemWidget;
			gameMenuItemWidget.OnOptionStateChanged = (Action)Delegate.Combine(gameMenuItemWidget.OnOptionStateChanged, new Action(this.OnOptionStateChanged));
			this.IsOverlayExtended = true;
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00032063 File Offset: 0x00030263
		public void OnOptionStateChanged()
		{
			this.IsOverlayExtended = true;
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0003206C File Offset: 0x0003026C
		private void OnOptionRemoved(Widget widget, Widget child)
		{
			this.IsOverlayExtended = true;
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x00032075 File Offset: 0x00030275
		// (set) Token: 0x0600120E RID: 4622 RVA: 0x0003207D File Offset: 0x0003027D
		[Editor(false)]
		public NavigationScopeTargeter ScopeTargeter
		{
			get
			{
				return this._scopeTargeter;
			}
			set
			{
				if (this._scopeTargeter != value)
				{
					this._scopeTargeter = value;
					base.OnPropertyChanged<NavigationScopeTargeter>(value, "ScopeTargeter");
				}
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x0003209B File Offset: 0x0003029B
		// (set) Token: 0x06001210 RID: 4624 RVA: 0x000320A3 File Offset: 0x000302A3
		[Editor(false)]
		public TextWidget TitleTextWidget
		{
			get
			{
				return this._titleTextWidget;
			}
			set
			{
				if (this._titleTextWidget != value)
				{
					this._titleTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "TitleTextWidget");
					if (value != null)
					{
						value.PropertyChanged += this.TitleTextWidget_PropertyChanged;
					}
				}
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x000320D6 File Offset: 0x000302D6
		// (set) Token: 0x06001212 RID: 4626 RVA: 0x000320DE File Offset: 0x000302DE
		[Editor(false)]
		public Widget TitleContainerWidget
		{
			get
			{
				return this._titleContainerWidget;
			}
			set
			{
				if (this._titleContainerWidget != value)
				{
					this._titleContainerWidget = value;
					base.OnPropertyChanged<Widget>(value, "TitleContainerWidget");
				}
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x000320FC File Offset: 0x000302FC
		// (set) Token: 0x06001214 RID: 4628 RVA: 0x00032104 File Offset: 0x00030304
		[Editor(false)]
		public bool IsNight
		{
			get
			{
				return this._isNight;
			}
			set
			{
				if (this._isNight != value)
				{
					this._isNight = value;
					base.OnPropertyChanged(value, "IsNight");
				}
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x00032122 File Offset: 0x00030322
		// (set) Token: 0x06001216 RID: 4630 RVA: 0x0003212A File Offset: 0x0003032A
		[Editor(false)]
		public bool IsEncounterMenu
		{
			get
			{
				return this._isEncounterMenu;
			}
			set
			{
				if (this._isEncounterMenu != value)
				{
					this._isEncounterMenu = value;
					base.OnPropertyChanged(value, "IsEncounterMenu");
					this.RefreshSize();
				}
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x0003214E File Offset: 0x0003034E
		// (set) Token: 0x06001218 RID: 4632 RVA: 0x00032156 File Offset: 0x00030356
		[Editor(false)]
		public Widget Overlay
		{
			get
			{
				return this._overlay;
			}
			set
			{
				if (value != this._overlay)
				{
					this._overlay = value;
					base.OnPropertyChanged<Widget>(value, "Overlay");
				}
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x00032174 File Offset: 0x00030374
		// (set) Token: 0x0600121A RID: 4634 RVA: 0x0003217C File Offset: 0x0003037C
		[Editor(false)]
		public ButtonWidget ExtendButtonWidget
		{
			get
			{
				return this._extendButtonWidget;
			}
			set
			{
				if (this._extendButtonWidget != value)
				{
					this._extendButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ExtendButtonWidget");
					if (this._extendButtonWidget != null)
					{
						this._extendButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnExtendButtonClick));
					}
				}
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600121B RID: 4635 RVA: 0x000321C9 File Offset: 0x000303C9
		// (set) Token: 0x0600121C RID: 4636 RVA: 0x000321D1 File Offset: 0x000303D1
		[Editor(false)]
		public BrushWidget ExtendButtonArrowWidget
		{
			get
			{
				return this._extendButtonArrowWidget;
			}
			set
			{
				if (value != this._extendButtonArrowWidget)
				{
					this._extendButtonArrowWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "ExtendButtonArrowWidget");
				}
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600121D RID: 4637 RVA: 0x000321EF File Offset: 0x000303EF
		// (set) Token: 0x0600121E RID: 4638 RVA: 0x000321F8 File Offset: 0x000303F8
		[Editor(false)]
		public ListPanel OptionItemsList
		{
			get
			{
				return this._optionItemsList;
			}
			set
			{
				if (value != this._optionItemsList)
				{
					this._optionItemsList = value;
					this._optionItemsList.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnOptionAdded));
					this._optionItemsList.ItemRemoveEventHandlers.Add(new Action<Widget, Widget>(this.OnOptionRemoved));
					base.OnPropertyChanged<ListPanel>(value, "OptionItemsList");
				}
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x00032259 File Offset: 0x00030459
		// (set) Token: 0x06001220 RID: 4640 RVA: 0x00032261 File Offset: 0x00030461
		[Editor(false)]
		public string SpriteName
		{
			get
			{
				return this._spriteName;
			}
			set
			{
				if (value != this._spriteName)
				{
					this._spriteName = value;
					base.OnPropertyChanged<string>(value, "SpriteName");
				}
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00032284 File Offset: 0x00030484
		// (set) Token: 0x06001222 RID: 4642 RVA: 0x0003228C File Offset: 0x0003048C
		[Editor(false)]
		public string MenuId
		{
			get
			{
				return this._menuId;
			}
			set
			{
				if (value != this._menuId)
				{
					this._menuId = value;
					base.OnPropertyChanged<string>(value, "MenuId");
				}
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x000322AF File Offset: 0x000304AF
		// (set) Token: 0x06001224 RID: 4644 RVA: 0x000322B7 File Offset: 0x000304B7
		[Editor(false)]
		public Brush OverriddenSpriteMapBrush
		{
			get
			{
				return this._overriddenSpriteMapBrush;
			}
			set
			{
				if (value != this._overriddenSpriteMapBrush)
				{
					this._overriddenSpriteMapBrush = value;
					base.OnPropertyChanged<Brush>(value, "OverriddenSpriteMapBrush");
				}
			}
		}

		// Token: 0x0400082F RID: 2095
		private bool _firstFrame = true;

		// Token: 0x04000836 RID: 2102
		private const string _extendedState = "Default";

		// Token: 0x04000837 RID: 2103
		private const string _hiddenState = "Disabled";

		// Token: 0x04000838 RID: 2104
		private bool _isOverlayExtended = true;

		// Token: 0x04000839 RID: 2105
		private NavigationScopeTargeter _scopeTargeter;

		// Token: 0x0400083A RID: 2106
		private TextWidget _titleTextWidget;

		// Token: 0x0400083B RID: 2107
		private Widget _titleContainerWidget;

		// Token: 0x0400083C RID: 2108
		private bool _isNight;

		// Token: 0x0400083D RID: 2109
		private bool _isEncounterMenu;

		// Token: 0x0400083E RID: 2110
		private Widget _overlay;

		// Token: 0x0400083F RID: 2111
		private ButtonWidget _extendButtonWidget;

		// Token: 0x04000840 RID: 2112
		private BrushWidget _extendButtonArrowWidget;

		// Token: 0x04000841 RID: 2113
		private ListPanel _optionItemsList;

		// Token: 0x04000842 RID: 2114
		private string _spriteName;

		// Token: 0x04000843 RID: 2115
		private string _menuId;

		// Token: 0x04000844 RID: 2116
		private Brush _overriddenSpriteMapBrush;
	}
}
