using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000110 RID: 272
	public class GameMenuPartyItemButtonWidget : ButtonWidget
	{
		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x00027E7F File Offset: 0x0002607F
		// (set) Token: 0x06000E74 RID: 3700 RVA: 0x00027E87 File Offset: 0x00026087
		public Brush PartyBackgroundBrush { get; set; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00027E90 File Offset: 0x00026090
		// (set) Token: 0x06000E76 RID: 3702 RVA: 0x00027E98 File Offset: 0x00026098
		public Brush CharacterBackgroundBrush { get; set; }

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00027EA1 File Offset: 0x000260A1
		// (set) Token: 0x06000E78 RID: 3704 RVA: 0x00027EA9 File Offset: 0x000260A9
		public ImageWidget BackgroundImageWidget { get; set; }

		// Token: 0x06000E79 RID: 3705 RVA: 0x00027EB4 File Offset: 0x000260B4
		public GameMenuPartyItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00027F0D File Offset: 0x0002610D
		private string GetRelationBackgroundName(int relation)
		{
			return "";
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00027F14 File Offset: 0x00026114
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._popupWidget == null)
			{
				Widget widget = this;
				while (widget != base.EventManager.Root && this._popupWidget == null && this._parentKnowsPopup)
				{
					if (widget is OverlayBaseWidget)
					{
						OverlayBaseWidget overlayBaseWidget = (OverlayBaseWidget)widget;
						if (overlayBaseWidget.PopupWidget == null)
						{
							this._parentKnowsPopup = false;
							break;
						}
						this._popupWidget = overlayBaseWidget.PopupWidget;
					}
					else
					{
						widget = widget.ParentWidget;
					}
				}
			}
			if (this.CurrentCharacterImageWidget != null)
			{
				this.CurrentCharacterImageWidget.Brush.SaturationFactor = (float)(this.IsMergedWithArmy ? 0 : (-100));
				this.CurrentCharacterImageWidget.Brush.ValueFactor = (float)(this.IsMergedWithArmy ? 0 : (-20));
			}
			if (!this._initialized)
			{
				this.BackgroundImageWidget.Brush = (this.IsPartyItem ? this.PartyBackgroundBrush : this.CharacterBackgroundBrush);
				this._initialized = true;
			}
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00027FFB File Offset: 0x000261FB
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this._popupWidget != null)
			{
				this._popupWidget.SetCurrentCharacter(this);
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x00028017 File Offset: 0x00026217
		// (set) Token: 0x06000E7E RID: 3710 RVA: 0x0002801F File Offset: 0x0002621F
		[Editor(false)]
		public int Relation
		{
			get
			{
				return this._relation;
			}
			set
			{
				if (this._relation != value)
				{
					this._relation = value;
					base.OnPropertyChanged(value, "Relation");
				}
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0002803D File Offset: 0x0002623D
		// (set) Token: 0x06000E80 RID: 3712 RVA: 0x00028045 File Offset: 0x00026245
		[Editor(false)]
		public string Location
		{
			get
			{
				return this._location;
			}
			set
			{
				if (this._location != value)
				{
					this._location = value;
					base.OnPropertyChanged<string>(value, "Location");
				}
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x00028068 File Offset: 0x00026268
		// (set) Token: 0x06000E82 RID: 3714 RVA: 0x00028070 File Offset: 0x00026270
		[Editor(false)]
		public string Power
		{
			get
			{
				return this._power;
			}
			set
			{
				if (this._power != value)
				{
					this._power = value;
					base.OnPropertyChanged<string>(value, "Power");
				}
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x00028093 File Offset: 0x00026293
		// (set) Token: 0x06000E84 RID: 3716 RVA: 0x0002809B File Offset: 0x0002629B
		[Editor(false)]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (this._description != value)
				{
					this._description = value;
					base.OnPropertyChanged<string>(value, "Description");
				}
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x000280BE File Offset: 0x000262BE
		// (set) Token: 0x06000E86 RID: 3718 RVA: 0x000280C6 File Offset: 0x000262C6
		[Editor(false)]
		public string Profession
		{
			get
			{
				return this._profession;
			}
			set
			{
				if (this._profession != value)
				{
					this._profession = value;
					base.OnPropertyChanged<string>(value, "Profession");
				}
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x000280E9 File Offset: 0x000262E9
		// (set) Token: 0x06000E88 RID: 3720 RVA: 0x000280F1 File Offset: 0x000262F1
		[Editor(false)]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (this._name != value)
				{
					this._name = value;
					base.OnPropertyChanged<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x00028114 File Offset: 0x00026314
		// (set) Token: 0x06000E8A RID: 3722 RVA: 0x0002811C File Offset: 0x0002631C
		[Editor(false)]
		public bool IsMergedWithArmy
		{
			get
			{
				return this._isMergedWithArmy;
			}
			set
			{
				if (this._isMergedWithArmy != value)
				{
					this._isMergedWithArmy = value;
				}
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0002812E File Offset: 0x0002632E
		// (set) Token: 0x06000E8C RID: 3724 RVA: 0x00028136 File Offset: 0x00026336
		[Editor(false)]
		public bool IsPartyItem
		{
			get
			{
				return this._isPartyItem;
			}
			set
			{
				if (this._isPartyItem != value)
				{
					this._isPartyItem = value;
				}
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00028148 File Offset: 0x00026348
		// (set) Token: 0x06000E8E RID: 3726 RVA: 0x00028150 File Offset: 0x00026350
		[Editor(false)]
		public Widget ContextMenu
		{
			get
			{
				return this._contextMenu;
			}
			set
			{
				if (this._contextMenu != value)
				{
					this._contextMenu = value;
					base.OnPropertyChanged<Widget>(value, "ContextMenu");
				}
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0002816E File Offset: 0x0002636E
		// (set) Token: 0x06000E90 RID: 3728 RVA: 0x00028176 File Offset: 0x00026376
		[Editor(false)]
		public ImageIdentifierWidget CurrentCharacterImageWidget
		{
			get
			{
				return this._currentCharacterImageWidget;
			}
			set
			{
				if (this._currentCharacterImageWidget != value)
				{
					this._currentCharacterImageWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "CurrentCharacterImageWidget");
				}
			}
		}

		// Token: 0x04000690 RID: 1680
		private bool _initialized;

		// Token: 0x04000691 RID: 1681
		private int _relation;

		// Token: 0x04000692 RID: 1682
		private string _location = "";

		// Token: 0x04000693 RID: 1683
		private string _description = "";

		// Token: 0x04000694 RID: 1684
		private string _profession = "";

		// Token: 0x04000695 RID: 1685
		private string _power = "";

		// Token: 0x04000696 RID: 1686
		private string _name = "";

		// Token: 0x04000697 RID: 1687
		private Widget _contextMenu;

		// Token: 0x04000698 RID: 1688
		private ImageIdentifierWidget _currentCharacterImageWidget;

		// Token: 0x04000699 RID: 1689
		private OverlayPopupWidget _popupWidget;

		// Token: 0x0400069A RID: 1690
		private bool _parentKnowsPopup = true;

		// Token: 0x0400069B RID: 1691
		private bool _isMergedWithArmy = true;

		// Token: 0x0400069C RID: 1692
		private bool _isPartyItem;
	}
}
