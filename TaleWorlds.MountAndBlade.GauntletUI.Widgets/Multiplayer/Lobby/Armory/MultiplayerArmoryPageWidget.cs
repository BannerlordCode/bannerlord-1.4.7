using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B6 RID: 182
	public class MultiplayerArmoryPageWidget : Widget
	{
		// Token: 0x06000975 RID: 2421 RVA: 0x0001A90D File Offset: 0x00018B0D
		public MultiplayerArmoryPageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0001A918 File Offset: 0x00018B18
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.IsTauntAssignmentActive && !Input.IsGamepadActive)
			{
				Widget latestMouseUpWidget = base.EventManager.LatestMouseUpWidget;
				Widget latestMouseDownWidget = base.EventManager.LatestMouseDownWidget;
				if (latestMouseUpWidget != null && latestMouseUpWidget == latestMouseDownWidget && !this.IsWidgetUsedForTauntSelection(latestMouseUpWidget))
				{
					base.EventFired("ReleaseTauntSelections", Array.Empty<object>());
				}
			}
			if (this.TauntSlotsContainer != null && this.TauntCircleActionSelector != null)
			{
				this.TauntCircleActionSelector.IsCircularInputEnabled = this.IsTauntControlsOpen && this.TauntSlotsContainer.IsPointInsideMeasuredArea(base.EventManager.MousePosition);
			}
			if (this._cosmeticPanelScrollTarget != null && this._cosmeticsScrollablePanel != null)
			{
				ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters(0f, 0f, 0f, 0f, -1f, 0.5f, 0.3f);
				this._cosmeticsScrollablePanel.ScrollToChild(this._cosmeticPanelScrollTarget, autoScrollParameters);
				this._cosmeticPanelScrollTarget = null;
			}
			this.UpdateTauntControlStates(dt);
			this.AnimateTauntAssignmentStates(dt);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0001AA10 File Offset: 0x00018C10
		private bool IsWidgetUsedForTauntSelection(Widget widget)
		{
			CircleActionSelectorWidget tauntCircleActionSelector = this.TauntCircleActionSelector;
			MultiplayerLobbyArmoryCosmeticItemButtonWidget multiplayerLobbyArmoryCosmeticItemButtonWidget;
			return (tauntCircleActionSelector != null && tauntCircleActionSelector.CheckIsMyChildRecursive(widget)) || ((multiplayerLobbyArmoryCosmeticItemButtonWidget = widget as MultiplayerLobbyArmoryCosmeticItemButtonWidget) != null && multiplayerLobbyArmoryCosmeticItemButtonWidget.IsSelected);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0001AA49 File Offset: 0x00018C49
		private void RegisterForStateUpdate()
		{
			this._isTauntStateDirty = true;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0001AA54 File Offset: 0x00018C54
		private void UpdateTauntControlStates(float dt)
		{
			if (this._isTauntStateDirty)
			{
				string text = (this.IsTauntControlsOpen ? "TauntEnabled" : "Default");
				if (this.TauntCircleActionSelector != null)
				{
					this.TauntCircleActionSelector.AnimateDistanceFromCenterTo((float)(this.IsTauntControlsOpen ? this.TauntEnabledRadialDistance : this.TauntDisabledRadialDistance), this.TauntStateAnimationDuration);
					this.TauntCircleActionSelector.IsEnabled = this.IsTauntControlsOpen;
					this.TauntCircleActionSelector.SetGlobalAlphaRecursively(this.IsTauntControlsOpen ? 1f : 0.6f);
				}
				Widget tauntSlotsContainer = this.TauntSlotsContainer;
				if (tauntSlotsContainer != null)
				{
					tauntSlotsContainer.SetState(text);
				}
				Widget manageTauntsButton = this.ManageTauntsButton;
				if (manageTauntsButton != null)
				{
					manageTauntsButton.SetState(text);
				}
				Widget leftSideParent = this.LeftSideParent;
				if (leftSideParent != null)
				{
					leftSideParent.SetState(text);
				}
				Widget gameModesDropdownParent = this.GameModesDropdownParent;
				if (gameModesDropdownParent != null)
				{
					gameModesDropdownParent.SetState(text);
				}
				Widget heroPreviewParent = this.HeroPreviewParent;
				if (heroPreviewParent != null)
				{
					heroPreviewParent.SetState(text);
				}
				if (this.RightPanelTabControl != null && this.IsTauntControlsOpen)
				{
					this.RightPanelTabControl.SelectedIndex = 1;
				}
				this._isTauntStateDirty = false;
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0001AB60 File Offset: 0x00018D60
		private void OnTauntAssignmentStateChanged(bool isTauntAssignmentActive)
		{
			this._tauntAssignmentStateTimer = 0f;
			if (isTauntAssignmentActive && this.TauntCircleActionSelector != null)
			{
				if (this.TauntCircleActionSelector.GetFirstInChildrenRecursive(delegate(Widget c)
				{
					ButtonWidget buttonWidget = c as ButtonWidget;
					return buttonWidget != null && buttonWidget.IsSelected;
				}) != null)
				{
					if (this._cosmeticsScrollablePanel == null)
					{
						this._cosmeticsScrollablePanel = this.RightPanelTabControl.GetFirstInChildrenRecursive((Widget c) => c is ScrollablePanel) as ScrollablePanel;
					}
					if (this._cosmeticsScrollablePanel != null)
					{
						Widget firstInChildrenRecursive = this._cosmeticsScrollablePanel.GetFirstInChildrenRecursive(delegate(Widget c)
						{
							MultiplayerLobbyArmoryCosmeticItemButtonWidget multiplayerLobbyArmoryCosmeticItemButtonWidget;
							return (multiplayerLobbyArmoryCosmeticItemButtonWidget = c as MultiplayerLobbyArmoryCosmeticItemButtonWidget) != null && multiplayerLobbyArmoryCosmeticItemButtonWidget.IsSelectable;
						});
						if (firstInChildrenRecursive != null)
						{
							this._cosmeticPanelScrollTarget = firstInChildrenRecursive;
						}
					}
				}
			}
			if (Input.IsGamepadActive && isTauntAssignmentActive)
			{
				GauntletGamepadNavigationManager.Instance.TryNavigateTo(this.ManageTauntsButton);
			}
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0001AC4C File Offset: 0x00018E4C
		private void AnimateTauntAssignmentStates(float dt)
		{
			float num4;
			if (this._tauntAssignmentStateTimer < this.TauntStateAnimationDuration)
			{
				float num = this._tauntAssignmentStateTimer / this.TauntStateAnimationDuration;
				float num2 = (this.IsTauntAssignmentActive ? 0f : this.TauntAssignmentOverlayAlpha);
				float num3 = (this.IsTauntAssignmentActive ? this.TauntAssignmentOverlayAlpha : 0f);
				num4 = MathF.Lerp(num2, num3, num, 1E-05f);
				this._tauntAssignmentStateTimer += dt;
			}
			else
			{
				num4 = (this.IsTauntAssignmentActive ? this.TauntAssignmentOverlayAlpha : 0f);
			}
			if (this.TauntAssignmentOverlay != null)
			{
				this.TauntAssignmentOverlay.IsVisible = num4 != 0f;
				this.TauntAssignmentOverlay.SetGlobalAlphaRecursively(num4);
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x0001ACFD File Offset: 0x00018EFD
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x0001AD05 File Offset: 0x00018F05
		public bool IsTauntAssignmentActive
		{
			get
			{
				return this._isTauntAssignmentActive;
			}
			set
			{
				if (value != this._isTauntAssignmentActive)
				{
					this._isTauntAssignmentActive = value;
					base.OnPropertyChanged(value, "IsTauntAssignmentActive");
					this.OnTauntAssignmentStateChanged(value);
				}
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0001AD2A File Offset: 0x00018F2A
		// (set) Token: 0x0600097F RID: 2431 RVA: 0x0001AD32 File Offset: 0x00018F32
		public bool IsTauntControlsOpen
		{
			get
			{
				return this._isTauntControlsOpen;
			}
			set
			{
				if (value != this._isTauntControlsOpen)
				{
					this._isTauntControlsOpen = value;
					base.OnPropertyChanged(value, "IsTauntControlsOpen");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0001AD56 File Offset: 0x00018F56
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x0001AD5E File Offset: 0x00018F5E
		public int TauntEnabledRadialDistance
		{
			get
			{
				return this._tauntEnabledRadialDistance;
			}
			set
			{
				if (value != this._tauntEnabledRadialDistance)
				{
					this._tauntEnabledRadialDistance = value;
					base.OnPropertyChanged(value, "TauntEnabledRadialDistance");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0001AD82 File Offset: 0x00018F82
		// (set) Token: 0x06000983 RID: 2435 RVA: 0x0001AD8A File Offset: 0x00018F8A
		public int TauntDisabledRadialDistance
		{
			get
			{
				return this._tauntDisabledRadialDistance;
			}
			set
			{
				if (value != this._tauntDisabledRadialDistance)
				{
					this._tauntDisabledRadialDistance = value;
					base.OnPropertyChanged(value, "TauntDisabledRadialDistance");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0001ADAE File Offset: 0x00018FAE
		// (set) Token: 0x06000985 RID: 2437 RVA: 0x0001ADB6 File Offset: 0x00018FB6
		public float TauntStateAnimationDuration
		{
			get
			{
				return this._tauntStateAnimationDuration;
			}
			set
			{
				if (value != this._tauntStateAnimationDuration)
				{
					this._tauntStateAnimationDuration = value;
					base.OnPropertyChanged(value, "TauntStateAnimationDuration");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0001ADDA File Offset: 0x00018FDA
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x0001ADE2 File Offset: 0x00018FE2
		public float TauntAssignmentOverlayAlpha
		{
			get
			{
				return this._tauntAssignmentOverlayAlpha;
			}
			set
			{
				if (value != this._tauntAssignmentOverlayAlpha)
				{
					this._tauntAssignmentOverlayAlpha = value;
					base.OnPropertyChanged(value, "TauntAssignmentOverlayAlpha");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0001AE06 File Offset: 0x00019006
		// (set) Token: 0x06000989 RID: 2441 RVA: 0x0001AE0E File Offset: 0x0001900E
		public Widget LeftSideParent
		{
			get
			{
				return this._leftSideParent;
			}
			set
			{
				if (value != this._leftSideParent)
				{
					this._leftSideParent = value;
					base.OnPropertyChanged<Widget>(value, "LeftSideParent");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0001AE32 File Offset: 0x00019032
		// (set) Token: 0x0600098B RID: 2443 RVA: 0x0001AE3A File Offset: 0x0001903A
		public Widget GameModesDropdownParent
		{
			get
			{
				return this._gameModesDropdownParent;
			}
			set
			{
				if (value != this._gameModesDropdownParent)
				{
					this._gameModesDropdownParent = value;
					base.OnPropertyChanged<Widget>(value, "GameModesDropdownParent");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x0001AE5E File Offset: 0x0001905E
		// (set) Token: 0x0600098D RID: 2445 RVA: 0x0001AE66 File Offset: 0x00019066
		public Widget HeroPreviewParent
		{
			get
			{
				return this._heroPreviewParent;
			}
			set
			{
				if (value != this._heroPreviewParent)
				{
					this._heroPreviewParent = value;
					base.OnPropertyChanged<Widget>(value, "HeroPreviewParent");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0001AE8A File Offset: 0x0001908A
		// (set) Token: 0x0600098F RID: 2447 RVA: 0x0001AE92 File Offset: 0x00019092
		public Widget TauntAssignmentOverlay
		{
			get
			{
				return this._tauntAssignmentOverlay;
			}
			set
			{
				if (value != this._tauntAssignmentOverlay)
				{
					this._tauntAssignmentOverlay = value;
					base.OnPropertyChanged<Widget>(value, "TauntAssignmentOverlay");
				}
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x0001AEB0 File Offset: 0x000190B0
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x0001AEB8 File Offset: 0x000190B8
		public Widget ManageTauntsButton
		{
			get
			{
				return this._manageTauntsButton;
			}
			set
			{
				if (value != this._manageTauntsButton)
				{
					this._manageTauntsButton = value;
					base.OnPropertyChanged<Widget>(value, "ManageTauntsButton");
				}
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x0001AED6 File Offset: 0x000190D6
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x0001AEDE File Offset: 0x000190DE
		public Widget TauntSlotsContainer
		{
			get
			{
				return this._tauntSlotsContainer;
			}
			set
			{
				if (value != this._tauntSlotsContainer)
				{
					this._tauntSlotsContainer = value;
					base.OnPropertyChanged<Widget>(value, "TauntSlotsContainer");
				}
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0001AEFC File Offset: 0x000190FC
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x0001AF04 File Offset: 0x00019104
		public TabControl RightPanelTabControl
		{
			get
			{
				return this._rightPanelTabControl;
			}
			set
			{
				if (value != this._rightPanelTabControl)
				{
					this._rightPanelTabControl = value;
					base.OnPropertyChanged<TabControl>(value, "RightPanelTabControl");
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0001AF28 File Offset: 0x00019128
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x0001AF30 File Offset: 0x00019130
		public CircleActionSelectorWidget TauntCircleActionSelector
		{
			get
			{
				return this._tauntCircleActionSelector;
			}
			set
			{
				if (value != this._tauntCircleActionSelector)
				{
					this._tauntCircleActionSelector = value;
					base.OnPropertyChanged<CircleActionSelectorWidget>(value, "TauntCircleActionSelector");
					if (this._tauntCircleActionSelector != null)
					{
						this._tauntCircleActionSelector.DistanceFromCenterModifier = (float)(this.IsTauntControlsOpen ? this.TauntEnabledRadialDistance : this.TauntDisabledRadialDistance);
					}
					this.RegisterForStateUpdate();
				}
			}
		}

		// Token: 0x04000445 RID: 1093
		private bool _isTauntStateDirty;

		// Token: 0x04000446 RID: 1094
		private float _tauntAssignmentStateTimer;

		// Token: 0x04000447 RID: 1095
		private ScrollablePanel _cosmeticsScrollablePanel;

		// Token: 0x04000448 RID: 1096
		private Widget _cosmeticPanelScrollTarget;

		// Token: 0x04000449 RID: 1097
		private bool _isTauntAssignmentActive;

		// Token: 0x0400044A RID: 1098
		private bool _isTauntControlsOpen;

		// Token: 0x0400044B RID: 1099
		private int _tauntEnabledRadialDistance;

		// Token: 0x0400044C RID: 1100
		private int _tauntDisabledRadialDistance;

		// Token: 0x0400044D RID: 1101
		private float _tauntStateAnimationDuration;

		// Token: 0x0400044E RID: 1102
		private float _tauntAssignmentOverlayAlpha;

		// Token: 0x0400044F RID: 1103
		private Widget _leftSideParent;

		// Token: 0x04000450 RID: 1104
		private Widget _gameModesDropdownParent;

		// Token: 0x04000451 RID: 1105
		private Widget _heroPreviewParent;

		// Token: 0x04000452 RID: 1106
		private Widget _tauntAssignmentOverlay;

		// Token: 0x04000453 RID: 1107
		private Widget _manageTauntsButton;

		// Token: 0x04000454 RID: 1108
		private Widget _tauntSlotsContainer;

		// Token: 0x04000455 RID: 1109
		private TabControl _rightPanelTabControl;

		// Token: 0x04000456 RID: 1110
		private CircleActionSelectorWidget _tauntCircleActionSelector;
	}
}
