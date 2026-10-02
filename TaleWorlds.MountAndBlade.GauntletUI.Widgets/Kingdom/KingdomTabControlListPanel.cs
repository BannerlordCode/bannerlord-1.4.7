using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000136 RID: 310
	public class KingdomTabControlListPanel : ListPanel
	{
		// Token: 0x0600101F RID: 4127 RVA: 0x0002C3BC File Offset: 0x0002A5BC
		public KingdomTabControlListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0002C3C8 File Offset: 0x0002A5C8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.FiefsButton.IsSelected = this.FiefsPanel.IsVisible;
			this.PoliciesButton.IsSelected = this.PoliciesPanel.IsVisible;
			this.ClansButton.IsSelected = this.ClansPanel.IsVisible;
			this.ArmiesButton.IsSelected = this.ArmiesPanel.IsVisible;
			this.DiplomacyButton.IsSelected = this.DiplomacyPanel.IsVisible;
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001021 RID: 4129 RVA: 0x0002C44A File Offset: 0x0002A64A
		// (set) Token: 0x06001022 RID: 4130 RVA: 0x0002C452 File Offset: 0x0002A652
		[Editor(false)]
		public Widget DiplomacyPanel
		{
			get
			{
				return this._diplomacyPanel;
			}
			set
			{
				if (this._diplomacyPanel != value)
				{
					this._diplomacyPanel = value;
					base.OnPropertyChanged<Widget>(value, "DiplomacyPanel");
				}
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x0002C470 File Offset: 0x0002A670
		// (set) Token: 0x06001024 RID: 4132 RVA: 0x0002C478 File Offset: 0x0002A678
		[Editor(false)]
		public Widget ArmiesPanel
		{
			get
			{
				return this._armiesPanel;
			}
			set
			{
				if (this._armiesPanel != value)
				{
					this._armiesPanel = value;
					base.OnPropertyChanged<Widget>(value, "ArmiesPanel");
				}
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0002C496 File Offset: 0x0002A696
		// (set) Token: 0x06001026 RID: 4134 RVA: 0x0002C49E File Offset: 0x0002A69E
		[Editor(false)]
		public Widget ClansPanel
		{
			get
			{
				return this._clansPanel;
			}
			set
			{
				if (this._clansPanel != value)
				{
					this._clansPanel = value;
					base.OnPropertyChanged<Widget>(value, "ClansPanel");
				}
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x0002C4BC File Offset: 0x0002A6BC
		// (set) Token: 0x06001028 RID: 4136 RVA: 0x0002C4C4 File Offset: 0x0002A6C4
		[Editor(false)]
		public Widget PoliciesPanel
		{
			get
			{
				return this._policiesPanel;
			}
			set
			{
				if (this._policiesPanel != value)
				{
					this._policiesPanel = value;
					base.OnPropertyChanged<Widget>(value, "PoliciesPanel");
				}
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001029 RID: 4137 RVA: 0x0002C4E2 File Offset: 0x0002A6E2
		// (set) Token: 0x0600102A RID: 4138 RVA: 0x0002C4EA File Offset: 0x0002A6EA
		[Editor(false)]
		public Widget FiefsPanel
		{
			get
			{
				return this._fiefsPanel;
			}
			set
			{
				if (this._fiefsPanel != value)
				{
					this._fiefsPanel = value;
					base.OnPropertyChanged<Widget>(value, "FiefsPanel");
				}
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x0002C508 File Offset: 0x0002A708
		// (set) Token: 0x0600102C RID: 4140 RVA: 0x0002C510 File Offset: 0x0002A710
		[Editor(false)]
		public ButtonWidget FiefsButton
		{
			get
			{
				return this._fiefsButton;
			}
			set
			{
				if (this._fiefsButton != value)
				{
					this._fiefsButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FiefsButton");
				}
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x0002C52E File Offset: 0x0002A72E
		// (set) Token: 0x0600102E RID: 4142 RVA: 0x0002C536 File Offset: 0x0002A736
		[Editor(false)]
		public ButtonWidget PoliciesButton
		{
			get
			{
				return this._policiesButton;
			}
			set
			{
				if (this._policiesButton != value)
				{
					this._policiesButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "PoliciesButton");
				}
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x0002C554 File Offset: 0x0002A754
		// (set) Token: 0x06001030 RID: 4144 RVA: 0x0002C55C File Offset: 0x0002A75C
		[Editor(false)]
		public ButtonWidget ClansButton
		{
			get
			{
				return this._clansButton;
			}
			set
			{
				if (this._clansButton != value)
				{
					this._clansButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ClansButton");
				}
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x0002C57A File Offset: 0x0002A77A
		// (set) Token: 0x06001032 RID: 4146 RVA: 0x0002C582 File Offset: 0x0002A782
		[Editor(false)]
		public ButtonWidget ArmiesButton
		{
			get
			{
				return this._armiesButton;
			}
			set
			{
				if (this._armiesButton != value)
				{
					this._armiesButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ArmiesButton");
				}
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0002C5A0 File Offset: 0x0002A7A0
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x0002C5A8 File Offset: 0x0002A7A8
		[Editor(false)]
		public ButtonWidget DiplomacyButton
		{
			get
			{
				return this._diplomacyButton;
			}
			set
			{
				if (this._diplomacyButton != value)
				{
					this._diplomacyButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "DiplomacyButton");
				}
			}
		}

		// Token: 0x04000750 RID: 1872
		private Widget _armiesPanel;

		// Token: 0x04000751 RID: 1873
		private Widget _clansPanel;

		// Token: 0x04000752 RID: 1874
		private Widget _policiesPanel;

		// Token: 0x04000753 RID: 1875
		private Widget _fiefsPanel;

		// Token: 0x04000754 RID: 1876
		private Widget _diplomacyPanel;

		// Token: 0x04000755 RID: 1877
		private ButtonWidget _fiefsButton;

		// Token: 0x04000756 RID: 1878
		private ButtonWidget _clansButton;

		// Token: 0x04000757 RID: 1879
		private ButtonWidget _policiesButton;

		// Token: 0x04000758 RID: 1880
		private ButtonWidget _armiesButton;

		// Token: 0x04000759 RID: 1881
		private ButtonWidget _diplomacyButton;
	}
}
