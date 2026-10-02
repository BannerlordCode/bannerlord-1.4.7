using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013E RID: 318
	public class InventoryTupleExtensionControlsWidget : Widget
	{
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x0002D382 File Offset: 0x0002B582
		// (set) Token: 0x06001077 RID: 4215 RVA: 0x0002D38A File Offset: 0x0002B58A
		public Widget NavigationParent { get; set; }

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x0002D393 File Offset: 0x0002B593
		// (set) Token: 0x06001079 RID: 4217 RVA: 0x0002D39B File Offset: 0x0002B59B
		private GamepadNavigationScope _parentScope { get; set; }

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x0002D3A4 File Offset: 0x0002B5A4
		// (set) Token: 0x0600107B RID: 4219 RVA: 0x0002D3AC File Offset: 0x0002B5AC
		private GamepadNavigationScope _extensionSliderScope { get; set; }

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x0002D3B5 File Offset: 0x0002B5B5
		// (set) Token: 0x0600107D RID: 4221 RVA: 0x0002D3BD File Offset: 0x0002B5BD
		private GamepadNavigationScope _extensionIncreaseDecreaseScope { get; set; }

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x0002D3C6 File Offset: 0x0002B5C6
		// (set) Token: 0x0600107F RID: 4223 RVA: 0x0002D3CE File Offset: 0x0002B5CE
		private GamepadNavigationScope _extensionButtonsScope { get; set; }

		// Token: 0x06001080 RID: 4224 RVA: 0x0002D3D7 File Offset: 0x0002B5D7
		public InventoryTupleExtensionControlsWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0002D3E0 File Offset: 0x0002B5E0
		public void BuildNavigationData()
		{
			if (this._isNavigationActive)
			{
				return;
			}
			if (this.TransferSlider != null)
			{
				this._extensionSliderScope = new GamepadNavigationScope
				{
					ScopeID = "ExtensionSliderScope",
					ParentWidget = this.TransferSlider,
					IsEnabled = false,
					NavigateFromScopeEdges = true
				};
			}
			if (this.IncreaseDecreaseButtonsParent != null)
			{
				this._extensionIncreaseDecreaseScope = new GamepadNavigationScope
				{
					ScopeID = "ExtensionIncreaseDecreaseScope",
					ParentWidget = this.IncreaseDecreaseButtonsParent,
					IsEnabled = false,
					ScopeMovements = GamepadNavigationTypes.Horizontal,
					ExtendDiscoveryAreaTop = -40f,
					ExtendDiscoveryAreaBottom = -10f,
					ExtendDiscoveryAreaRight = -350f
				};
			}
			if (this.ButtonCarrier != null)
			{
				this._extensionButtonsScope = new GamepadNavigationScope
				{
					ScopeID = "ExtensionButtonsScope",
					ParentWidget = this.ButtonCarrier,
					IsEnabled = false,
					ScopeMovements = GamepadNavigationTypes.Horizontal
				};
			}
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0002D4C4 File Offset: 0x0002B6C4
		private void TransitionTick(float dt)
		{
			if (this._currentVisualStateAnimationState == VisualStateAnimationState.None)
			{
				if (!this._isNavigationActive)
				{
					this.AddGamepadNavigationControls();
					base.EventManager.AddLateUpdateAction(this, delegate(float _dt)
					{
						this.NavigateToBestChildScope();
					}, 1);
					return;
				}
			}
			else
			{
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.TransitionTick), 1);
			}
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0002D51C File Offset: 0x0002B71C
		private void AddGamepadNavigationControls()
		{
			if (this.ValidateParentScope() && !this._isNavigationActive)
			{
				if (this._extensionIncreaseDecreaseScope != null)
				{
					base.GamepadNavigationContext.AddNavigationScope(this._extensionIncreaseDecreaseScope, false);
				}
				if (this._extensionSliderScope != null)
				{
					base.GamepadNavigationContext.AddNavigationScope(this._extensionSliderScope, false);
				}
				if (this._extensionButtonsScope != null)
				{
					base.GamepadNavigationContext.AddNavigationScope(this._extensionButtonsScope, false);
				}
				this.SetEnabledAllScopes(true);
				if (this._extensionSliderScope != null)
				{
					this._extensionSliderScope.SetParentScope(this._parentScope);
				}
				if (this._extensionIncreaseDecreaseScope != null)
				{
					this._extensionIncreaseDecreaseScope.SetParentScope(this._parentScope);
				}
				if (this._extensionButtonsScope != null)
				{
					this._extensionButtonsScope.SetParentScope(this._parentScope);
				}
				base.DoNotAcceptNavigation = false;
				this._isNavigationActive = true;
			}
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0002D5F0 File Offset: 0x0002B7F0
		private void RemoveGamepadNavigationControls()
		{
			if (this.ValidateParentScope() && this._isNavigationActive)
			{
				this.SetEnabledAllScopes(false);
				if (this._extensionSliderScope != null)
				{
					this._extensionSliderScope.SetParentScope(null);
					base.GamepadNavigationContext.RemoveNavigationScope(this._extensionSliderScope);
					this._extensionSliderScope = null;
				}
				if (this._extensionIncreaseDecreaseScope != null)
				{
					this._extensionIncreaseDecreaseScope.SetParentScope(null);
					base.GamepadNavigationContext.RemoveNavigationScope(this._extensionIncreaseDecreaseScope);
					this._extensionIncreaseDecreaseScope = null;
				}
				if (this._extensionButtonsScope != null)
				{
					this._extensionButtonsScope.SetParentScope(null);
					base.GamepadNavigationContext.RemoveNavigationScope(this._extensionButtonsScope);
					this._extensionButtonsScope = null;
				}
				base.DoNotAcceptNavigation = true;
				this._isNavigationActive = false;
			}
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0002D6AC File Offset: 0x0002B8AC
		private void SetEnabledAllScopes(bool isEnabled)
		{
			if (this._extensionSliderScope != null)
			{
				this._extensionSliderScope.IsEnabled = isEnabled;
			}
			if (this._extensionIncreaseDecreaseScope != null)
			{
				this._extensionIncreaseDecreaseScope.IsEnabled = isEnabled;
			}
			if (this._extensionButtonsScope != null)
			{
				this._extensionButtonsScope.IsEnabled = isEnabled;
			}
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0002D6EC File Offset: 0x0002B8EC
		private void NavigateToBestChildScope()
		{
			if (this._parentScope.IsActiveScope)
			{
				GamepadNavigationScope[] array = new GamepadNavigationScope[] { this._extensionSliderScope, this._extensionButtonsScope, this._extensionIncreaseDecreaseScope };
				for (int i = 0; i < array.Length; i++)
				{
					if (GauntletGamepadNavigationManager.Instance.TryNavigateTo(array[i]))
					{
						return;
					}
				}
			}
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0002D746 File Offset: 0x0002B946
		private bool ValidateParentScope()
		{
			if (this._parentScope == null)
			{
				this._parentScope = this.GetParentScope();
			}
			return this._parentScope != null;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0002D768 File Offset: 0x0002B968
		private GamepadNavigationScope GetParentScope()
		{
			Widget navigationParent = this.NavigationParent;
			for (Widget widget = ((navigationParent != null) ? navigationParent.ParentWidget : null); widget != null; widget = widget.ParentWidget)
			{
				NavigationScopeTargeter navigationScopeTargeter;
				if ((navigationScopeTargeter = widget as NavigationScopeTargeter) != null)
				{
					return navigationScopeTargeter.NavigationScope;
				}
				NavigationScopeTargeter navigationScopeTargeter2 = widget.Children.FirstOrDefault<Widget>((Widget x) => x is NavigationScopeTargeter) as NavigationScopeTargeter;
				if (navigationScopeTargeter2 != null)
				{
					return navigationScopeTargeter2.NavigationScope;
				}
			}
			return null;
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x0002D7E0 File Offset: 0x0002B9E0
		// (set) Token: 0x0600108A RID: 4234 RVA: 0x0002D7E8 File Offset: 0x0002B9E8
		public bool IsExtended
		{
			get
			{
				return this._isExtended;
			}
			set
			{
				if (value != this._isExtended)
				{
					this._isExtended = value;
					base.IsEnabled = this._isExtended;
					this.SetEnabledAllScopes(false);
					if (this._isExtended)
					{
						this.BuildNavigationData();
						base.EventManager.AddLateUpdateAction(this, new Action<float>(this.TransitionTick), 1);
						return;
					}
					this.RemoveGamepadNavigationControls();
				}
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0002D846 File Offset: 0x0002BA46
		// (set) Token: 0x0600108C RID: 4236 RVA: 0x0002D84E File Offset: 0x0002BA4E
		[Editor(false)]
		public Widget TransferSlider
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
					base.OnPropertyChanged<Widget>(value, "TransferSlider");
				}
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x0002D86C File Offset: 0x0002BA6C
		// (set) Token: 0x0600108E RID: 4238 RVA: 0x0002D874 File Offset: 0x0002BA74
		[Editor(false)]
		public Widget IncreaseDecreaseButtonsParent
		{
			get
			{
				return this._increaseDecreaseButtonsParent;
			}
			set
			{
				if (this._increaseDecreaseButtonsParent != value)
				{
					this._increaseDecreaseButtonsParent = value;
					base.OnPropertyChanged<Widget>(value, "IncreaseDecreaseButtonsParent");
				}
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600108F RID: 4239 RVA: 0x0002D892 File Offset: 0x0002BA92
		// (set) Token: 0x06001090 RID: 4240 RVA: 0x0002D89A File Offset: 0x0002BA9A
		[Editor(false)]
		public Widget ButtonCarrier
		{
			get
			{
				return this._buttonCarrier;
			}
			set
			{
				if (this._buttonCarrier != value)
				{
					this._buttonCarrier = value;
					base.OnPropertyChanged<Widget>(value, "ButtonCarrier");
				}
			}
		}

		// Token: 0x04000774 RID: 1908
		private bool _isNavigationActive;

		// Token: 0x04000775 RID: 1909
		private bool _isExtended;

		// Token: 0x04000776 RID: 1910
		private Widget _transferSlider;

		// Token: 0x04000777 RID: 1911
		private Widget _increaseDecreaseButtonsParent;

		// Token: 0x04000778 RID: 1912
		private Widget _buttonCarrier;
	}
}
