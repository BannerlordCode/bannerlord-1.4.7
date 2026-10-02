using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000ED RID: 237
	public class OrderOfBattleFormationItemListPanel : ListPanel
	{
		// Token: 0x06000C2C RID: 3116 RVA: 0x00021531 File Offset: 0x0001F731
		public OrderOfBattleFormationItemListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0002153A File Offset: 0x0001F73A
		private void OnStateChanged()
		{
			if (this.IsSelected)
			{
				Widget cardWidget = this.CardWidget;
				if (cardWidget == null)
				{
					return;
				}
				cardWidget.SetState("Selected");
				return;
			}
			else
			{
				Widget cardWidget2 = this.CardWidget;
				if (cardWidget2 == null)
				{
					return;
				}
				cardWidget2.SetState("Default");
				return;
			}
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0002156F File Offset: 0x0001F76F
		private void OnClassDropdownEnabledStateChanged(DropdownWidget widget)
		{
			this.IsClassDropdownEnabled = widget.IsOpen;
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x0002157D File Offset: 0x0001F77D
		// (set) Token: 0x06000C30 RID: 3120 RVA: 0x00021585 File Offset: 0x0001F785
		[Editor(false)]
		public Widget CardWidget
		{
			get
			{
				return this._cardWidget;
			}
			set
			{
				if (value != this._cardWidget)
				{
					this._cardWidget = value;
					base.OnPropertyChanged<Widget>(value, "CardWidget");
				}
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x000215A3 File Offset: 0x0001F7A3
		// (set) Token: 0x06000C32 RID: 3122 RVA: 0x000215AC File Offset: 0x0001F7AC
		[Editor(false)]
		public DropdownWidget FormationClassDropdown
		{
			get
			{
				return this._formationClassDropdown;
			}
			set
			{
				if (value != this._formationClassDropdown)
				{
					if (this._formationClassDropdown != null)
					{
						DropdownWidget formationClassDropdown = this._formationClassDropdown;
						formationClassDropdown.OnOpenStateChanged = (Action<DropdownWidget>)Delegate.Remove(formationClassDropdown.OnOpenStateChanged, new Action<DropdownWidget>(this.OnClassDropdownEnabledStateChanged));
					}
					this._formationClassDropdown = value;
					base.OnPropertyChanged<DropdownWidget>(value, "FormationClassDropdown");
					if (this._formationClassDropdown != null)
					{
						DropdownWidget formationClassDropdown2 = this._formationClassDropdown;
						formationClassDropdown2.OnOpenStateChanged = (Action<DropdownWidget>)Delegate.Combine(formationClassDropdown2.OnOpenStateChanged, new Action<DropdownWidget>(this.OnClassDropdownEnabledStateChanged));
						this.OnClassDropdownEnabledStateChanged(this._formationClassDropdown);
					}
				}
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0002163F File Offset: 0x0001F83F
		// (set) Token: 0x06000C34 RID: 3124 RVA: 0x00021648 File Offset: 0x0001F848
		[Editor(false)]
		public bool IsControlledByPlayer
		{
			get
			{
				return this._isControlledByPlayer;
			}
			set
			{
				if (value != this._isControlledByPlayer)
				{
					this._isControlledByPlayer = value;
					base.OnPropertyChanged(value, "IsControlledByPlayer");
					DropdownWidget formationClassDropdown = this.FormationClassDropdown;
					if (((formationClassDropdown != null) ? formationClassDropdown.Button : null) != null)
					{
						this.FormationClassDropdown.Button.IsEnabled = value;
					}
				}
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x00021696 File Offset: 0x0001F896
		// (set) Token: 0x06000C36 RID: 3126 RVA: 0x0002169E File Offset: 0x0001F89E
		[Editor(false)]
		public bool IsClassDropdownEnabled
		{
			get
			{
				return this._isClassDropdownEnabled;
			}
			set
			{
				if (value != this._isClassDropdownEnabled)
				{
					this._isClassDropdownEnabled = value;
					base.OnPropertyChanged(value, "IsClassDropdownEnabled");
					if (this.FormationClassDropdown != null)
					{
						this.FormationClassDropdown.IsOpen = value;
					}
				}
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x000216D0 File Offset: 0x0001F8D0
		// (set) Token: 0x06000C38 RID: 3128 RVA: 0x000216D8 File Offset: 0x0001F8D8
		[Editor(false)]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChanged(value, "IsSelected");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x000216FC File Offset: 0x0001F8FC
		// (set) Token: 0x06000C3A RID: 3130 RVA: 0x00021704 File Offset: 0x0001F904
		[Editor(false)]
		public bool HasFormation
		{
			get
			{
				return this._hasFormation;
			}
			set
			{
				if (value != this._hasFormation)
				{
					this._hasFormation = value;
					base.OnPropertyChanged(value, "HasFormation");
				}
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00021722 File Offset: 0x0001F922
		// (set) Token: 0x06000C3C RID: 3132 RVA: 0x0002172A File Offset: 0x0001F92A
		[Editor(false)]
		public float DefaultFocusYOffsetFromCenter
		{
			get
			{
				return this._defaultFocusYOffsetFromCenter;
			}
			set
			{
				if (value != this._defaultFocusYOffsetFromCenter)
				{
					this._defaultFocusYOffsetFromCenter = value;
					base.OnPropertyChanged(value, "DefaultFocusYOffsetFromCenter");
				}
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00021748 File Offset: 0x0001F948
		// (set) Token: 0x06000C3E RID: 3134 RVA: 0x00021750 File Offset: 0x0001F950
		[Editor(false)]
		public float NoFormationFocusYOffsetFromCenter
		{
			get
			{
				return this._noFormationFocusYOffsetFromCenter;
			}
			set
			{
				if (value != this._noFormationFocusYOffsetFromCenter)
				{
					this._noFormationFocusYOffsetFromCenter = value;
					base.OnPropertyChanged(value, "NoFormationFocusYOffsetFromCenter");
				}
			}
		}

		// Token: 0x0400057F RID: 1407
		private Widget _cardWidget;

		// Token: 0x04000580 RID: 1408
		private DropdownWidget _formationClassDropdown;

		// Token: 0x04000581 RID: 1409
		private bool _isControlledByPlayer;

		// Token: 0x04000582 RID: 1410
		private bool _isClassDropdownEnabled;

		// Token: 0x04000583 RID: 1411
		private bool _isSelected;

		// Token: 0x04000584 RID: 1412
		private bool _hasFormation;

		// Token: 0x04000585 RID: 1413
		private float _defaultFocusYOffsetFromCenter;

		// Token: 0x04000586 RID: 1414
		private float _noFormationFocusYOffsetFromCenter;
	}
}
