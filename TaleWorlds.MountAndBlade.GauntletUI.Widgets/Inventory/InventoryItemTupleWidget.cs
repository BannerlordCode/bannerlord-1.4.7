using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000140 RID: 320
	public class InventoryItemTupleWidget : InventoryItemButtonWidget
	{
		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001097 RID: 4247 RVA: 0x0002D91C File Offset: 0x0002BB1C
		// (set) Token: 0x06001098 RID: 4248 RVA: 0x0002D924 File Offset: 0x0002BB24
		public InventoryImageIdentifierWidget ItemImageIdentifier { get; set; }

		// Token: 0x06001099 RID: 4249 RVA: 0x0002D92D File Offset: 0x0002BB2D
		public InventoryItemTupleWidget(UIContext context)
			: base(context)
		{
			base.OverrideDefaultStateSwitchingEnabled = false;
			base.AddState("Selected");
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0002D948 File Offset: 0x0002BB48
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			base.ScreenWidget.intPropertyChanged += this.InventoryScreenWidgetOnPropertyChanged;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0002D967 File Offset: 0x0002BB67
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			base.ScreenWidget.intPropertyChanged -= this.InventoryScreenWidgetOnPropertyChanged;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0002D988 File Offset: 0x0002BB88
		private void SetWidgetsState(string state)
		{
			this.SetState(state);
			string currentState = this.ExtendedControlsContainer.CurrentState;
			this.ExtendedControlsContainer.SetState(base.IsSelected ? "Selected" : "Default");
			this.MainContainer.SetState(state);
			this.NameTextWidget.SetState((state == "Pressed") ? state : "Default");
			if (currentState == "Default" && base.IsSelected)
			{
				base.EventFired("Opened", Array.Empty<object>());
				this.Slider.IsExtended = true;
				return;
			}
			if (currentState == "Selected" && !base.IsSelected)
			{
				base.EventFired("Closed", Array.Empty<object>());
				this.Slider.IsExtended = false;
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0002DA58 File Offset: 0x0002BC58
		private void OnExtendedHiddenUpdate(float dt)
		{
			if (!base.IsSelected)
			{
				this._extendedUpdateTimer += dt;
				if (this._extendedUpdateTimer > 2f)
				{
					this.ExtendedControlsContainer.IsVisible = false;
					return;
				}
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.OnExtendedHiddenUpdate), 1);
			}
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0002DAB0 File Offset: 0x0002BCB0
		protected override void RefreshState()
		{
			base.RefreshState();
			bool isVisible = this.ExtendedControlsContainer.IsVisible;
			this.ExtendedControlsContainer.IsExtended = base.IsSelected;
			if (base.IsSelected)
			{
				this.ExtendedControlsContainer.IsVisible = true;
			}
			else if (this.ExtendedControlsContainer.IsVisible)
			{
				this._extendedUpdateTimer = 0f;
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.OnExtendedHiddenUpdate), 1);
			}
			if (base.IsDisabled)
			{
				this.SetWidgetsState("Disabled");
				return;
			}
			if (base.IsPressed)
			{
				this.SetWidgetsState("Pressed");
				return;
			}
			if (base.IsHovered)
			{
				this.SetWidgetsState("Hovered");
				return;
			}
			if (base.IsSelected)
			{
				this.SetWidgetsState("Selected");
				return;
			}
			this.SetWidgetsState("Default");
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0002DB84 File Offset: 0x0002BD84
		private void UpdateEquipmentTypeState()
		{
			if (base.ScreenWidget != null)
			{
				bool flag = base.ScreenWidget.EquipmentMode == 0 && !this.IsCivilian && this.IsEquipable;
				bool flag2 = base.ScreenWidget.EquipmentMode == 2 && !this.IsStealth && this.IsEquipable;
				if (this.IsEquipable && !this.CanCharacterUseItem)
				{
					if (!this.MainContainer.Brush.IsCloneRelated(this.CharacterCantUseBrush))
					{
						this.MainContainer.Brush = this.CharacterCantUseBrush;
						this.EquipButton.IsVisible = true;
						this.EquipButton.IsEnabled = false;
						return;
					}
				}
				else if (flag || flag2)
				{
					if (!this.MainContainer.Brush.IsCloneRelated(this.CantUseInSetBrush))
					{
						this.MainContainer.Brush = this.CantUseInSetBrush;
						this.EquipButton.IsVisible = true;
						this.EquipButton.IsEnabled = false;
						return;
					}
				}
				else if (!this.MainContainer.Brush.IsCloneRelated(this.DefaultBrush))
				{
					this.MainContainer.Brush = this.DefaultBrush;
					this.EquipButton.IsVisible = this.IsEquipable;
					this.EquipButton.IsEnabled = this.IsEquipable;
				}
			}
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0002DCC2 File Offset: 0x0002BEC2
		private void SliderIntPropertyChanged(PropertyOwnerObject owner, string propertyName, int value)
		{
			if (propertyName == "ValueInt")
			{
				this.TransactionCount = this._slider.ValueInt;
			}
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0002DCE2 File Offset: 0x0002BEE2
		private void CountTextWidgetOnPropertyChanged(PropertyOwnerObject owner, string propertyName, int value)
		{
			if (propertyName == "IntText")
			{
				this.UpdateCountText();
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0002DCF7 File Offset: 0x0002BEF7
		private void InventoryScreenWidgetOnPropertyChanged(PropertyOwnerObject owner, string propertyName, int value)
		{
			if (propertyName == "EquipmentMode")
			{
				this.UpdateEquipmentTypeState();
			}
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0002DD0C File Offset: 0x0002BF0C
		private void UpdateCountText()
		{
			if (this.SliderTextWidget != null)
			{
				this.SliderTextWidget.IsHidden = this.CountTextWidget.IsHidden;
			}
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0002DD2C File Offset: 0x0002BF2C
		private void UpdateCostText()
		{
			if (this.CostTextWidget == null)
			{
				return;
			}
			switch (this.ProfitState)
			{
			case -2:
				this.CostTextWidget.SetState("VeryBad");
				return;
			case -1:
				this.CostTextWidget.SetState("Bad");
				return;
			case 0:
				this.CostTextWidget.SetState("Default");
				return;
			case 1:
				this.CostTextWidget.SetState("Good");
				return;
			case 2:
				this.CostTextWidget.SetState("VeryGood");
				return;
			default:
				return;
			}
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0002DDBB File Offset: 0x0002BFBB
		private void UpdateDragAvailability()
		{
			base.AcceptDrag = this.ItemCount > 0 && (this.IsTransferable || this.IsEquipable);
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x0002DDE0 File Offset: 0x0002BFE0
		// (set) Token: 0x060010A7 RID: 4263 RVA: 0x0002DDE8 File Offset: 0x0002BFE8
		[Editor(false)]
		public string ItemID
		{
			get
			{
				return this._itemID;
			}
			set
			{
				if (this._itemID != value)
				{
					this._itemID = value;
					base.OnPropertyChanged<string>(value, "ItemID");
				}
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x0002DE0B File Offset: 0x0002C00B
		// (set) Token: 0x060010A9 RID: 4265 RVA: 0x0002DE13 File Offset: 0x0002C013
		[Editor(false)]
		public TextWidget NameTextWidget
		{
			get
			{
				return this._nameTextWidget;
			}
			set
			{
				if (this._nameTextWidget != value)
				{
					this._nameTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NameTextWidget");
					this.NameTextWidget.AddState("Pressed");
				}
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x0002DE41 File Offset: 0x0002C041
		// (set) Token: 0x060010AB RID: 4267 RVA: 0x0002DE4C File Offset: 0x0002C04C
		[Editor(false)]
		public TextWidget CountTextWidget
		{
			get
			{
				return this._countTextWidget;
			}
			set
			{
				if (this._countTextWidget != value)
				{
					if (this._countTextWidget != null)
					{
						this._countTextWidget.intPropertyChanged -= this.CountTextWidgetOnPropertyChanged;
					}
					this._countTextWidget = value;
					if (this._countTextWidget != null)
					{
						this._countTextWidget.intPropertyChanged += this.CountTextWidgetOnPropertyChanged;
					}
					base.OnPropertyChanged<TextWidget>(value, "CountTextWidget");
					this.UpdateCountText();
				}
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x0002DEB9 File Offset: 0x0002C0B9
		// (set) Token: 0x060010AD RID: 4269 RVA: 0x0002DEC1 File Offset: 0x0002C0C1
		[Editor(false)]
		public TextWidget CostTextWidget
		{
			get
			{
				return this._costTextWidget;
			}
			set
			{
				if (this._costTextWidget != value)
				{
					this._costTextWidget = value;
					this.UpdateCostText();
					base.OnPropertyChanged<TextWidget>(value, "CostTextWidget");
				}
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x0002DEE5 File Offset: 0x0002C0E5
		// (set) Token: 0x060010AF RID: 4271 RVA: 0x0002DEED File Offset: 0x0002C0ED
		public int ProfitState
		{
			get
			{
				return this._profitState;
			}
			set
			{
				if (value != this._profitState)
				{
					this._profitState = value;
					this.UpdateCostText();
					base.OnPropertyChanged(value, "ProfitState");
				}
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x0002DF11 File Offset: 0x0002C111
		// (set) Token: 0x060010B1 RID: 4273 RVA: 0x0002DF19 File Offset: 0x0002C119
		[Editor(false)]
		public BrushListPanel MainContainer
		{
			get
			{
				return this._mainContainer;
			}
			set
			{
				if (this._mainContainer != value)
				{
					this._mainContainer = value;
					base.OnPropertyChanged<BrushListPanel>(value, "MainContainer");
				}
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x0002DF37 File Offset: 0x0002C137
		// (set) Token: 0x060010B3 RID: 4275 RVA: 0x0002DF3F File Offset: 0x0002C13F
		[Editor(false)]
		public InventoryTupleExtensionControlsWidget ExtendedControlsContainer
		{
			get
			{
				return this._extendedControlsContainer;
			}
			set
			{
				if (this._extendedControlsContainer != value)
				{
					this._extendedControlsContainer = value;
					base.OnPropertyChanged<InventoryTupleExtensionControlsWidget>(value, "ExtendedControlsContainer");
				}
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x0002DF5D File Offset: 0x0002C15D
		// (set) Token: 0x060010B5 RID: 4277 RVA: 0x0002DF68 File Offset: 0x0002C168
		[Editor(false)]
		public InventoryTwoWaySliderWidget Slider
		{
			get
			{
				return this._slider;
			}
			set
			{
				if (this._slider != value)
				{
					if (this._slider != null)
					{
						this._slider.intPropertyChanged -= this.SliderIntPropertyChanged;
					}
					this._slider = value;
					if (this._slider != null)
					{
						this._slider.intPropertyChanged += this.SliderIntPropertyChanged;
					}
					base.OnPropertyChanged<InventoryTwoWaySliderWidget>(value, "Slider");
					this.Slider.AddState("Selected");
					this.Slider.OverrideDefaultStateSwitchingEnabled = true;
				}
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x0002DFEB File Offset: 0x0002C1EB
		// (set) Token: 0x060010B7 RID: 4279 RVA: 0x0002DFF3 File Offset: 0x0002C1F3
		[Editor(false)]
		public Widget SliderParent
		{
			get
			{
				return this._sliderParent;
			}
			set
			{
				if (this._sliderParent != value)
				{
					this._sliderParent = value;
					base.OnPropertyChanged<Widget>(value, "SliderParent");
					this.SliderParent.AddState("Selected");
				}
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x0002E021 File Offset: 0x0002C221
		// (set) Token: 0x060010B9 RID: 4281 RVA: 0x0002E029 File Offset: 0x0002C229
		[Editor(false)]
		public TextWidget SliderTextWidget
		{
			get
			{
				return this._sliderTextWidget;
			}
			set
			{
				if (this._sliderTextWidget != value)
				{
					this._sliderTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "SliderTextWidget");
					this.SliderTextWidget.AddState("Selected");
				}
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x0002E057 File Offset: 0x0002C257
		// (set) Token: 0x060010BB RID: 4283 RVA: 0x0002E05F File Offset: 0x0002C25F
		[Editor(false)]
		public bool IsTransferable
		{
			get
			{
				return this._isTransferable;
			}
			set
			{
				if (this._isTransferable != value)
				{
					this._isTransferable = value;
					base.OnPropertyChanged(value, "IsTransferable");
					this.UpdateDragAvailability();
				}
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0002E083 File Offset: 0x0002C283
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x0002E08B File Offset: 0x0002C28B
		[Editor(false)]
		public ButtonWidget EquipButton
		{
			get
			{
				return this._equipButton;
			}
			set
			{
				if (this._equipButton != value)
				{
					this._equipButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "EquipButton");
				}
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0002E0A9 File Offset: 0x0002C2A9
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x0002E0B1 File Offset: 0x0002C2B1
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

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x0002E0CF File Offset: 0x0002C2CF
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x0002E0D7 File Offset: 0x0002C2D7
		[Editor(false)]
		public int ItemCount
		{
			get
			{
				return this._itemCount;
			}
			set
			{
				if (this._itemCount != value)
				{
					this._itemCount = value;
					base.OnPropertyChanged(value, "ItemCount");
					this.UpdateDragAvailability();
				}
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x0002E0FB File Offset: 0x0002C2FB
		// (set) Token: 0x060010C3 RID: 4291 RVA: 0x0002E103 File Offset: 0x0002C303
		[Editor(false)]
		public bool IsCivilian
		{
			get
			{
				return this._isCivilian;
			}
			set
			{
				if (this._isCivilian != value || !this._isCivilianStateSet)
				{
					this._isCivilian = value;
					base.OnPropertyChanged(value, "IsCivilian");
					this._isCivilianStateSet = true;
					this.UpdateEquipmentTypeState();
				}
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x0002E136 File Offset: 0x0002C336
		// (set) Token: 0x060010C5 RID: 4293 RVA: 0x0002E13E File Offset: 0x0002C33E
		[Editor(false)]
		public bool IsStealth
		{
			get
			{
				return this._isStealth;
			}
			set
			{
				if (this._isStealth != value || !this._isStealthStateSet)
				{
					this._isStealth = value;
					base.OnPropertyChanged(value, "IsStealth");
					this._isStealthStateSet = true;
					this.UpdateEquipmentTypeState();
				}
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x0002E171 File Offset: 0x0002C371
		// (set) Token: 0x060010C7 RID: 4295 RVA: 0x0002E179 File Offset: 0x0002C379
		[Editor(false)]
		public bool IsGenderDifferent
		{
			get
			{
				return this._isGenderDifferent;
			}
			set
			{
				if (this._isGenderDifferent != value)
				{
					this._isGenderDifferent = value;
					base.OnPropertyChanged(value, "IsGenderDifferent");
					this.UpdateEquipmentTypeState();
				}
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x0002E19D File Offset: 0x0002C39D
		// (set) Token: 0x060010C9 RID: 4297 RVA: 0x0002E1A5 File Offset: 0x0002C3A5
		[Editor(false)]
		public bool IsEquipable
		{
			get
			{
				return this._isEquipable;
			}
			set
			{
				if (this._isEquipable != value)
				{
					this._isEquipable = value;
					base.OnPropertyChanged(value, "IsEquipable");
					this.UpdateDragAvailability();
				}
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x0002E1C9 File Offset: 0x0002C3C9
		// (set) Token: 0x060010CB RID: 4299 RVA: 0x0002E1D1 File Offset: 0x0002C3D1
		[Editor(false)]
		public bool IsNewlyAdded
		{
			get
			{
				return this._isNewlyAdded;
			}
			set
			{
				if (this._isNewlyAdded != value)
				{
					this._isNewlyAdded = value;
					base.OnPropertyChanged(value, "IsNewlyAdded");
					this.ItemImageIdentifier.SetRenderRequestedPreviousFrame(value);
				}
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x0002E1FB File Offset: 0x0002C3FB
		// (set) Token: 0x060010CD RID: 4301 RVA: 0x0002E203 File Offset: 0x0002C403
		[Editor(false)]
		public bool CanCharacterUseItem
		{
			get
			{
				return this._canCharacterUseItem;
			}
			set
			{
				if (this._canCharacterUseItem != value)
				{
					this._canCharacterUseItem = value;
					base.OnPropertyChanged(value, "CanCharacterUseItem");
					this.UpdateEquipmentTypeState();
				}
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x0002E227 File Offset: 0x0002C427
		// (set) Token: 0x060010CF RID: 4303 RVA: 0x0002E22F File Offset: 0x0002C42F
		[Editor(false)]
		public Brush DefaultBrush
		{
			get
			{
				return this._defaultBrush;
			}
			set
			{
				if (this._defaultBrush != value)
				{
					this._defaultBrush = value;
					base.OnPropertyChanged<Brush>(value, "DefaultBrush");
				}
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x0002E24D File Offset: 0x0002C44D
		// (set) Token: 0x060010D1 RID: 4305 RVA: 0x0002E255 File Offset: 0x0002C455
		[Editor(false)]
		public Brush CantUseInSetBrush
		{
			get
			{
				return this._cantUseInSetBrush;
			}
			set
			{
				if (this._cantUseInSetBrush != value)
				{
					this._cantUseInSetBrush = value;
					base.OnPropertyChanged<Brush>(value, "CantUseInSetBrush");
				}
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x0002E273 File Offset: 0x0002C473
		// (set) Token: 0x060010D3 RID: 4307 RVA: 0x0002E27B File Offset: 0x0002C47B
		[Editor(false)]
		public Brush CharacterCantUseBrush
		{
			get
			{
				return this._characterCantUseBrush;
			}
			set
			{
				if (this._characterCantUseBrush != value)
				{
					this._characterCantUseBrush = value;
					base.OnPropertyChanged<Brush>(value, "CharacterCantUseBrush");
				}
			}
		}

		// Token: 0x0400077C RID: 1916
		private bool _isCivilianStateSet;

		// Token: 0x0400077D RID: 1917
		private bool _isStealthStateSet;

		// Token: 0x0400077E RID: 1918
		private float _extendedUpdateTimer;

		// Token: 0x0400077F RID: 1919
		private TextWidget _nameTextWidget;

		// Token: 0x04000780 RID: 1920
		private TextWidget _countTextWidget;

		// Token: 0x04000781 RID: 1921
		private TextWidget _costTextWidget;

		// Token: 0x04000782 RID: 1922
		private int _profitState;

		// Token: 0x04000783 RID: 1923
		private BrushListPanel _mainContainer;

		// Token: 0x04000784 RID: 1924
		private InventoryTupleExtensionControlsWidget _extendedControlsContainer;

		// Token: 0x04000785 RID: 1925
		private InventoryTwoWaySliderWidget _slider;

		// Token: 0x04000786 RID: 1926
		private Widget _sliderParent;

		// Token: 0x04000787 RID: 1927
		private TextWidget _sliderTextWidget;

		// Token: 0x04000788 RID: 1928
		private bool _isTransferable;

		// Token: 0x04000789 RID: 1929
		private ButtonWidget _equipButton;

		// Token: 0x0400078A RID: 1930
		private int _transactionCount;

		// Token: 0x0400078B RID: 1931
		private int _itemCount;

		// Token: 0x0400078C RID: 1932
		private bool _isCivilian;

		// Token: 0x0400078D RID: 1933
		private bool _isStealth;

		// Token: 0x0400078E RID: 1934
		private bool _isGenderDifferent;

		// Token: 0x0400078F RID: 1935
		private bool _isEquipable;

		// Token: 0x04000790 RID: 1936
		private bool _canCharacterUseItem;

		// Token: 0x04000791 RID: 1937
		private bool _isNewlyAdded;

		// Token: 0x04000792 RID: 1938
		private Brush _defaultBrush;

		// Token: 0x04000793 RID: 1939
		private Brush _cantUseInSetBrush;

		// Token: 0x04000794 RID: 1940
		private Brush _characterCantUseBrush;

		// Token: 0x04000795 RID: 1941
		private string _itemID;
	}
}
