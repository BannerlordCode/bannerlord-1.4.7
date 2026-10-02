using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006B RID: 107
	public class PartyTroopTupleButtonWidget : ButtonWidget
	{
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x000114A8 File Offset: 0x0000F6A8
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x000114B0 File Offset: 0x0000F6B0
		public string CharacterID { get; set; }

		// Token: 0x060005C9 RID: 1481 RVA: 0x000114B9 File Offset: 0x0000F6B9
		public PartyTroopTupleButtonWidget(UIContext context)
			: base(context)
		{
			base.OverrideDefaultStateSwitchingEnabled = true;
			base.AddState("Selected");
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000114D4 File Offset: 0x0000F6D4
		private void SetWidgetsState(string state)
		{
			this.SetState(state);
			string currentState = this._extendedControlsContainer.CurrentState;
			this._extendedControlsContainer.SetState(base.IsSelected ? "Selected" : "Default");
			this._main.SetState(state);
			if (currentState == "Default" && base.IsSelected)
			{
				base.EventFired("Opened", Array.Empty<object>());
				this.TransferSlider.IsExtended = true;
				this._extendedControlsContainer.IsExtended = true;
				return;
			}
			if (currentState == "Selected" && !base.IsSelected)
			{
				base.EventFired("Closed", Array.Empty<object>());
				this.TransferSlider.IsExtended = false;
				this._extendedControlsContainer.IsExtended = false;
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001159C File Offset: 0x0000F79C
		protected override void RefreshState()
		{
			base.RefreshState();
			this._extendedControlsContainer.IsEnabled = base.IsSelected;
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

		// Token: 0x060005CC RID: 1484 RVA: 0x0001161C File Offset: 0x0000F81C
		private void AssignScreenWidget()
		{
			Widget widget = this;
			while (widget != base.EventManager.Root && this._screenWidget == null)
			{
				PartyScreenWidget partyScreenWidget;
				if ((partyScreenWidget = widget as PartyScreenWidget) != null)
				{
					this._screenWidget = partyScreenWidget;
				}
				else
				{
					widget = widget.ParentWidget;
				}
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001165D File Offset: 0x0000F85D
		private void OnValueChanged(PropertyOwnerObject arg1, string arg2, int arg3)
		{
			if (arg2 == "ValueInt")
			{
				base.AcceptDrag = arg3 > 0;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x00011676 File Offset: 0x0000F876
		public PartyScreenWidget ScreenWidget
		{
			get
			{
				if (this._screenWidget == null)
				{
					this.AssignScreenWidget();
				}
				return this._screenWidget;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0001168C File Offset: 0x0000F88C
		// (set) Token: 0x060005D0 RID: 1488 RVA: 0x00011694 File Offset: 0x0000F894
		[Editor(false)]
		public bool IsTupleLeftSide
		{
			get
			{
				return this._isTupleLeftSide;
			}
			set
			{
				if (this._isTupleLeftSide != value)
				{
					this._isTupleLeftSide = value;
					base.OnPropertyChanged(value, "IsTupleLeftSide");
				}
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x000116B2 File Offset: 0x0000F8B2
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x000116BC File Offset: 0x0000F8BC
		[Editor(false)]
		public InventoryTwoWaySliderWidget TransferSlider
		{
			get
			{
				return this._transferSlider;
			}
			set
			{
				if (this._transferSlider != value)
				{
					this._transferSlider = value;
					base.OnPropertyChanged<InventoryTwoWaySliderWidget>(value, "TransferSlider");
					value.intPropertyChanged += this.OnValueChanged;
					this._transferSlider.AddState("Selected");
					this._transferSlider.OverrideDefaultStateSwitchingEnabled = true;
				}
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x00011713 File Offset: 0x0000F913
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x0001171B File Offset: 0x0000F91B
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
				}
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00011739 File Offset: 0x0000F939
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x00011741 File Offset: 0x0000F941
		[Editor(false)]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (this._isMainHero != value)
				{
					base.AcceptDrag = !value;
					this._isMainHero = value;
					base.OnPropertyChanged(value, "IsMainHero");
				}
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x00011769 File Offset: 0x0000F969
		// (set) Token: 0x060005D8 RID: 1496 RVA: 0x00011771 File Offset: 0x0000F971
		[Editor(false)]
		public bool IsPrisoner
		{
			get
			{
				return this._isPrisoner;
			}
			set
			{
				if (this._isPrisoner != value)
				{
					this._isPrisoner = value;
					base.OnPropertyChanged(value, "IsPrisoner");
				}
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001178F File Offset: 0x0000F98F
		// (set) Token: 0x060005DA RID: 1498 RVA: 0x00011797 File Offset: 0x0000F997
		[Editor(false)]
		public int TransferAmount
		{
			get
			{
				return this._transferAmount;
			}
			set
			{
				if (this._transferAmount != value)
				{
					this._transferAmount = value;
					base.OnPropertyChanged(value, "TransferAmount");
				}
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x000117B5 File Offset: 0x0000F9B5
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x000117BD File Offset: 0x0000F9BD
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

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x000117DB File Offset: 0x0000F9DB
		// (set) Token: 0x060005DE RID: 1502 RVA: 0x000117E3 File Offset: 0x0000F9E3
		[Editor(false)]
		public Widget Main
		{
			get
			{
				return this._main;
			}
			set
			{
				if (this._main != value)
				{
					this._main = value;
					base.OnPropertyChanged<Widget>(value, "Main");
				}
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x00011801 File Offset: 0x0000FA01
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x00011809 File Offset: 0x0000FA09
		[Editor(false)]
		public Widget UpgradesPanel
		{
			get
			{
				return this._upgradesPanel;
			}
			set
			{
				if (this._upgradesPanel != value)
				{
					this._upgradesPanel = value;
					base.OnPropertyChanged<Widget>(value, "UpgradesPanel");
				}
			}
		}

		// Token: 0x0400027C RID: 636
		private PartyScreenWidget _screenWidget;

		// Token: 0x0400027D RID: 637
		public InventoryTwoWaySliderWidget _transferSlider;

		// Token: 0x0400027E RID: 638
		private bool _isTupleLeftSide;

		// Token: 0x0400027F RID: 639
		private bool _isTransferable;

		// Token: 0x04000280 RID: 640
		private bool _isMainHero;

		// Token: 0x04000281 RID: 641
		private bool _isPrisoner;

		// Token: 0x04000282 RID: 642
		private int _transferAmount;

		// Token: 0x04000283 RID: 643
		private InventoryTupleExtensionControlsWidget _extendedControlsContainer;

		// Token: 0x04000284 RID: 644
		private Widget _main;

		// Token: 0x04000285 RID: 645
		private Widget _upgradesPanel;
	}
}
