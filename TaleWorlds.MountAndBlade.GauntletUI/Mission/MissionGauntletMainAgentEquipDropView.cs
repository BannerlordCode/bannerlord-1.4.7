using System;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x02000033 RID: 51
	[OverrideView(typeof(MissionMainAgentEquipDropView))]
	public class MissionGauntletMainAgentEquipDropView : MissionView
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000C96F File Offset: 0x0000AB6F
		private bool IsDisplayingADialog
		{
			get
			{
				IMissionScreen missionScreenAsInterface = this._missionScreenAsInterface;
				return (missionScreenAsInterface != null && missionScreenAsInterface.GetDisplayDialog()) || base.MissionScreen.IsRadialMenuActive || base.Mission.IsOrderMenuOpen;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000C99F File Offset: 0x0000AB9F
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0000C9A7 File Offset: 0x0000ABA7
		private bool HoldHandled
		{
			get
			{
				return this._holdHandled;
			}
			set
			{
				this._holdHandled = value;
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000C9B0 File Offset: 0x0000ABB0
		public MissionGauntletMainAgentEquipDropView()
		{
			this._missionScreenAsInterface = base.MissionScreen;
			this.HoldHandled = false;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000C9CC File Offset: 0x0000ABCC
		public override void EarlyStart()
		{
			base.EarlyStart();
			this._gauntletLayer = new GauntletLayer("MissionEquipDrop", this.ViewOrderPriority, false);
			this._dataSource = new MissionMainAgentControllerEquipDropVM(new Action<EquipmentIndex>(this.OnToggleItem));
			this._missionMainAgentController = base.Mission.GetMissionBehavior<MissionMainAgentController>();
			this._missionControllerLeaveLogic = base.Mission.GetMissionBehavior<EquipmentControllerLeaveLogic>();
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("CombatHotKeyCategory"));
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
			this._gauntletLayer.LoadMovie("MainAgentControllerEquipDrop", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.Mission.OnMainAgentChanged += this.OnMainAgentChanged;
			TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveChanged));
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000CABA File Offset: 0x0000ACBA
		public override void AfterStart()
		{
			base.AfterStart();
			this._dataSource.InitializeMainAgentPropterties();
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000CAD0 File Offset: 0x0000ACD0
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveChanged));
			base.Mission.OnMainAgentChanged -= this.OnMainAgentChanged;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._missionMainAgentController = null;
			this._missionControllerLeaveLogic = null;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000CB54 File Offset: 0x0000AD54
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._dataSource.IsActive && !this.IsMainAgentAvailable())
			{
				this.HandleClosingHold();
			}
			if (this.IsMainAgentAvailable() && (!base.MissionScreen.IsRadialMenuActive || this._dataSource.IsActive))
			{
				this.TickControls(dt);
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		private void OnMainAgentChanged(Agent oldAgent)
		{
			if (base.Mission.MainAgent == null)
			{
				if (this.HoldHandled)
				{
					this.HoldHandled = false;
				}
				this._toggleHoldTime = 0f;
				this._dataSource.OnCancelHoldController();
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent == Agent.Main)
			{
				this.HandleClosingHold();
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000CBF0 File Offset: 0x0000ADF0
		private void TickControls(float dt)
		{
			if ((base.MissionScreen.SceneLayer.Input.IsGameKeyDown(34) || this._gauntletLayer.Input.IsGameKeyDown(34)) && !this.IsDisplayingADialog && !base.MissionScreen.IsPhotoModeEnabled && base.Mission.Mode != MissionMode.Deployment && base.Mission.Mode != MissionMode.CutScene && !base.MissionScreen.IsRadialMenuActive)
			{
				if (this._toggleHoldTime > 0.3f && !this.HoldHandled)
				{
					this.HandleOpeningHold();
					this.HoldHandled = true;
				}
				this._toggleHoldTime += dt;
				this._prevKeyDown = true;
			}
			else if (this._prevKeyDown && !base.MissionScreen.SceneLayer.Input.IsGameKeyDown(34) && !this._gauntletLayer.Input.IsGameKeyDown(34))
			{
				if (this._toggleHoldTime < 0.3f)
				{
					this.HandleQuickRelease();
				}
				else
				{
					this.HandleClosingHold();
				}
				this.HoldHandled = false;
				this._toggleHoldTime = 0f;
				this._weaponDropHoldTime = 0f;
				this._prevKeyDown = false;
				this._weaponDropHandled = false;
			}
			if (!this.HoldHandled)
			{
				this._weaponDropHoldTime = 0f;
				this._weaponDropHandled = false;
				return;
			}
			int keyWeaponIndex = this.GetKeyWeaponIndex(false);
			int keyWeaponIndex2 = this.GetKeyWeaponIndex(true);
			this._dataSource.SetDropProgressForIndex(EquipmentIndex.None, this._weaponDropHoldTime / 0.5f);
			if (keyWeaponIndex != -1)
			{
				if (!this._weaponDropHandled)
				{
					int num = keyWeaponIndex;
					if (this._weaponDropHoldTime > 0.5f && !Agent.Main.Equipment[num].IsEmpty)
					{
						this.OnDropEquipment((EquipmentIndex)num);
						this._dataSource.OnWeaponDroppedAtIndex(keyWeaponIndex);
						this._weaponDropHandled = true;
					}
					this._dataSource.SetDropProgressForIndex((EquipmentIndex)num, this._weaponDropHoldTime / 0.5f);
				}
				this._weaponDropHoldTime += dt;
				return;
			}
			if (keyWeaponIndex2 != -1)
			{
				if (!this._weaponDropHandled)
				{
					int num2 = keyWeaponIndex2;
					if (!Agent.Main.Equipment[num2].IsEmpty && num2 != 4)
					{
						this.OnToggleItem((EquipmentIndex)num2);
						this._dataSource.OnWeaponEquippedAtIndex(keyWeaponIndex2);
						this._weaponDropHandled = true;
					}
				}
				this._weaponDropHoldTime = 0f;
				return;
			}
			this._weaponDropHoldTime = 0f;
			this._weaponDropHandled = false;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000CE44 File Offset: 0x0000B044
		private void HandleOpeningHold()
		{
			MissionMainAgentControllerEquipDropVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnToggle(true);
			}
			base.MissionScreen.RegisterRadialMenuObject<MissionGauntletMainAgentEquipDropView>(this);
			EquipmentControllerLeaveLogic missionControllerLeaveLogic = this._missionControllerLeaveLogic;
			if (missionControllerLeaveLogic != null)
			{
				missionControllerLeaveLogic.SetIsEquipmentSelectionActive(true);
			}
			if (!GameNetwork.IsMultiplayer && !this._isSlowDownApplied)
			{
				base.Mission.AddTimeSpeedRequest(new Mission.TimeSpeedRequest(0.25f, 624));
				this._isSlowDownApplied = true;
			}
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000CEC8 File Offset: 0x0000B0C8
		private void HandleClosingHold()
		{
			MissionMainAgentControllerEquipDropVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnToggle(false);
			}
			base.MissionScreen.UnregisterRadialMenuObject(this);
			EquipmentControllerLeaveLogic missionControllerLeaveLogic = this._missionControllerLeaveLogic;
			if (missionControllerLeaveLogic != null)
			{
				missionControllerLeaveLogic.SetIsEquipmentSelectionActive(false);
			}
			if (!GameNetwork.IsMultiplayer && this._isSlowDownApplied)
			{
				base.Mission.RemoveTimeSpeedRequest(624);
				this._isSlowDownApplied = false;
			}
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000CF42 File Offset: 0x0000B142
		private void HandleQuickRelease()
		{
			this._missionMainAgentController.OnWeaponUsageToggleRequested();
			MissionMainAgentControllerEquipDropVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnToggle(false);
			}
			base.MissionScreen.UnregisterRadialMenuObject(this);
			EquipmentControllerLeaveLogic missionControllerLeaveLogic = this._missionControllerLeaveLogic;
			if (missionControllerLeaveLogic == null)
			{
				return;
			}
			missionControllerLeaveLogic.SetIsEquipmentSelectionActive(false);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000CF80 File Offset: 0x0000B180
		private void OnToggleItem(EquipmentIndex indexToToggle)
		{
			bool flag = indexToToggle == Agent.Main.GetPrimaryWieldedItemIndex();
			bool flag2 = indexToToggle == Agent.Main.GetOffhandWieldedItemIndex();
			if (flag || flag2)
			{
				Agent.Main.TryToSheathWeaponInHand(flag ? Agent.HandIndex.MainHand : Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.WithAnimation);
				return;
			}
			Agent.Main.TryToWieldWeaponInSlot(indexToToggle, Agent.WeaponWieldActionType.WithAnimation, false);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000CFD0 File Offset: 0x0000B1D0
		private void OnDropEquipment(EquipmentIndex indexToDrop)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new DropWeapon(base.Input.IsGameKeyDown(10), indexToDrop));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			Agent.Main.HandleDropWeapon(base.Input.IsGameKeyDown(10), indexToDrop);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000D020 File Offset: 0x0000B220
		private bool IsMainAgentAvailable()
		{
			Agent main = Agent.Main;
			if (main != null && main.IsActive())
			{
				Agent main2 = Agent.Main;
				return (main2 != null && !main2.Mission.IsNavalBattle) || !Agent.Main.IsUsingGameObject;
			}
			return false;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000D06C File Offset: 0x0000B26C
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000D091 File Offset: 0x0000B291
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000D0B6 File Offset: 0x0000B2B6
		private void OnGamepadActiveChanged()
		{
			this._dataSource.OnGamepadActiveChanged(TaleWorlds.InputSystem.Input.IsGamepadActive);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000D0C8 File Offset: 0x0000B2C8
		private int GetKeyWeaponIndex(bool isReleased)
		{
			Func<string, bool> func;
			if (isReleased)
			{
				func = new Func<string, bool>(this._gauntletLayer.Input.IsHotKeyReleased);
			}
			else
			{
				func = new Func<string, bool>(this._gauntletLayer.Input.IsHotKeyDown);
			}
			string text = string.Empty;
			if (func("ControllerEquipDropWeapon1"))
			{
				text = "ControllerEquipDropWeapon1";
			}
			else if (func("ControllerEquipDropWeapon2"))
			{
				text = "ControllerEquipDropWeapon2";
			}
			else if (func("ControllerEquipDropWeapon3"))
			{
				text = "ControllerEquipDropWeapon3";
			}
			else if (func("ControllerEquipDropWeapon4"))
			{
				text = "ControllerEquipDropWeapon4";
			}
			else if (func("ControllerEquipDropExtraWeapon"))
			{
				text = "ControllerEquipDropExtraWeapon";
			}
			if (!string.IsNullOrEmpty(text))
			{
				for (int i = 0; i < this._dataSource.EquippedWeapons.Count; i++)
				{
					InputKeyItemVM shortcutKey = this._dataSource.EquippedWeapons[i].ShortcutKey;
					if (((shortcutKey != null) ? shortcutKey.HotKey.Id : null) == text)
					{
						return (int)this._dataSource.EquippedWeapons[i].Identifier;
					}
				}
				ControllerEquippedItemVM equippedExtraWeapon = this._dataSource.EquippedExtraWeapon;
				string text2;
				if (equippedExtraWeapon == null)
				{
					text2 = null;
				}
				else
				{
					InputKeyItemVM shortcutKey2 = equippedExtraWeapon.ShortcutKey;
					text2 = ((shortcutKey2 != null) ? shortcutKey2.HotKey.Id : null);
				}
				if (text2 == text)
				{
					return (int)this._dataSource.EquippedExtraWeapon.Identifier;
				}
			}
			return -1;
		}

		// Token: 0x04000116 RID: 278
		private const int _missionTimeSpeedRequestID = 624;

		// Token: 0x04000117 RID: 279
		private const float _slowDownAmountWhileRadialIsOpen = 0.25f;

		// Token: 0x04000118 RID: 280
		private bool _isSlowDownApplied;

		// Token: 0x04000119 RID: 281
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400011A RID: 282
		private MissionMainAgentControllerEquipDropVM _dataSource;

		// Token: 0x0400011B RID: 283
		private MissionMainAgentController _missionMainAgentController;

		// Token: 0x0400011C RID: 284
		private EquipmentControllerLeaveLogic _missionControllerLeaveLogic;

		// Token: 0x0400011D RID: 285
		private const float _minOpenHoldTime = 0.3f;

		// Token: 0x0400011E RID: 286
		private const float _minDropHoldTime = 0.5f;

		// Token: 0x0400011F RID: 287
		private readonly IMissionScreen _missionScreenAsInterface;

		// Token: 0x04000120 RID: 288
		private bool _holdHandled;

		// Token: 0x04000121 RID: 289
		private float _toggleHoldTime;

		// Token: 0x04000122 RID: 290
		private float _weaponDropHoldTime;

		// Token: 0x04000123 RID: 291
		private bool _prevKeyDown;

		// Token: 0x04000124 RID: 292
		private bool _weaponDropHandled;
	}
}
