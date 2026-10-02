using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Handlers;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000019 RID: 25
	public class MissionOrderDeploymentControllerVM : ViewModel
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00006FFD File Offset: 0x000051FD
		private Mission Mission
		{
			get
			{
				return Mission.Current;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00007004 File Offset: 0x00005204
		private Team Team
		{
			get
			{
				return Mission.Current.PlayerTeam;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00007010 File Offset: 0x00005210
		public OrderController OrderController
		{
			get
			{
				return this.Team.PlayerOrderController;
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007020 File Offset: 0x00005220
		public void SetMissionParameters(Camera deploymentCamera, List<DeploymentPoint> deploymentPoints)
		{
			this._deploymentPoints = deploymentPoints;
			this._deploymentCamera = deploymentCamera;
			this.SiegeDeploymentList.Clear();
			if (this._siegeDeploymentHandler != null)
			{
				int num = 1;
				foreach (DeploymentPoint deploymentPoint in this._deploymentPoints)
				{
					OrderSiegeMachineVM orderSiegeMachineVM = new OrderSiegeMachineVM(deploymentPoint, new Action<OrderSiegeMachineVM>(this.OnSelectOrderSiegeMachine), num++);
					this.SiegeMachineList.Add(orderSiegeMachineVM);
					if (deploymentPoint.DeployableWeapons.Any<SynchedMissionObject>((SynchedMissionObject x) => this._siegeDeploymentHandler.GetMaxDeployableWeaponCountOfPlayer(x.GetType()) > 0))
					{
						DeploymentSiegeMachineVM deploymentSiegeMachineVM = new DeploymentSiegeMachineVM(deploymentPoint, null, this._deploymentCamera, new Action<DeploymentSiegeMachineVM>(this.OnRefreshSelectedDeploymentPoint), new Action<DeploymentPoint>(this.OnEntityHover), deploymentPoint.IsDeployed);
						this.DeploymentTargets.Add(deploymentSiegeMachineVM);
					}
				}
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000710C File Offset: 0x0000530C
		public void SetCallbacks(MissionOrderCallbacks callbacks)
		{
			this._callbacks = callbacks;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007118 File Offset: 0x00005318
		public MissionOrderDeploymentControllerVM(MissionOrderVM missionOrder)
		{
			this._missionOrder = missionOrder;
			this.SiegeMachineList = new MBBindingList<OrderSiegeMachineVM>();
			this.SiegeDeploymentList = new MBBindingList<DeploymentSiegeMachineVM>();
			this.DeploymentTargets = new MBBindingList<DeploymentSiegeMachineVM>();
			MBTextManager.SetTextVariable("UNDEPLOYED_SIEGE_MACHINE_COUNT", this.SiegeMachineList.Count<OrderSiegeMachineVM>((OrderSiegeMachineVM s) => !s.SiegeWeapon.IsUsed).ToString(), false);
			this._siegeDeployQueryData = new InquiryData(new TextObject("{=TxphX8Uk}Deployment", null).ToString(), new TextObject("{=LlrlE199}You can still deploy siege engines.{newline}Begin anyway?", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), delegate
			{
				this._siegeDeploymentHandler.FinishDeployment();
				this._missionOrder.TryCloseToggleOrder(false);
			}, null, "", 0f, null, null, null);
			this.SiegeMachineList.Clear();
			this.OrderController.SiegeWeaponController.OnSelectedSiegeWeaponsChanged += this.OnSelectedSiegeWeaponsChanged;
			this._deploymentHandler = this.Mission.GetMissionBehavior<DeploymentHandler>();
			if (this._deploymentHandler != null)
			{
				this._deploymentHandler.OnPlayerSideDeploymentReady += this.ExecuteDeployPlayerSide;
				SiegeDeploymentHandler siegeDeploymentHandler;
				if ((siegeDeploymentHandler = this._deploymentHandler as SiegeDeploymentHandler) != null)
				{
					this._siegeDeploymentHandler = siegeDeploymentHandler;
					this._siegeDeploymentHandler.OnEnemySideDeploymentReady += this.ExecuteDeployEnemySide;
				}
			}
			this.RefreshValues();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007284 File Offset: 0x00005484
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._siegeMachineList.ApplyActionOnAllItems(delegate(OrderSiegeMachineVM x)
			{
				x.RefreshValues();
			});
			this._siegeDeploymentList.ApplyActionOnAllItems(delegate(DeploymentSiegeMachineVM x)
			{
				x.RefreshValues();
			});
			this._deploymentTargets.ApplyActionOnAllItems(delegate(DeploymentSiegeMachineVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00007318 File Offset: 0x00005518
		internal void Update()
		{
			for (int i = 0; i < this.DeploymentTargets.Count; i++)
			{
				this.DeploymentTargets[i].Update();
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000734C File Offset: 0x0000554C
		internal void DeployFormationsOfPlayer()
		{
			if (this._siegeDeploymentHandler != null)
			{
				this._siegeDeploymentHandler.AutoDeployTeamUsingTeamAI(this.Mission.PlayerTeam, false);
			}
			else if (!this.Mission.IsNavalBattle && !this.Mission.IsNavalRaidBattle && this._deploymentHandler != null)
			{
				this._deploymentHandler.AutoDeployTeamUsingDeploymentPlan(this.Mission.PlayerTeam);
			}
			AssignPlayerRoleInTeamMissionController missionBehavior = Mission.Current.GetMissionBehavior<AssignPlayerRoleInTeamMissionController>();
			if (missionBehavior != null)
			{
				missionBehavior.OnPlayerTeamDeployed();
			}
			if (this._siegeDeploymentHandler != null)
			{
				this._siegeDeploymentHandler.AutoAssignDetachmentsForDeployment(this.Mission.PlayerTeam);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000073E5 File Offset: 0x000055E5
		internal void SetSiegeMachineActiveOrders(OrderSiegeMachineVM siegeItemVM)
		{
			siegeItemVM.ActiveOrders.Clear();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000073F4 File Offset: 0x000055F4
		internal void ProcessSiegeMachines()
		{
			for (int i = 0; i < this.SiegeMachineList.Count; i++)
			{
				OrderSiegeMachineVM orderSiegeMachineVM = this.SiegeMachineList[i];
				orderSiegeMachineVM.RefreshSiegeWeapon();
				if (orderSiegeMachineVM.IsSelectable && this.OrderController.SiegeWeaponController.SelectedWeapons.Contains(orderSiegeMachineVM.SiegeWeapon))
				{
					orderSiegeMachineVM.IsSelected = true;
				}
				else if (!orderSiegeMachineVM.IsSelectable && orderSiegeMachineVM.IsSelected)
				{
					this.OnDeselectSiegeMachine(orderSiegeMachineVM);
				}
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00007470 File Offset: 0x00005670
		internal void SelectAllSiegeMachines()
		{
			if (this.SiegeMachineList.Any<OrderSiegeMachineVM>((OrderSiegeMachineVM t) => t.IsSelectable))
			{
				this.OrderController.SiegeWeaponController.SelectAll();
			}
			this._missionOrder.SetActiveOrders();
			this._callbacks.RefreshVisuals();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000074D4 File Offset: 0x000056D4
		internal void AddSelectedSiegeMachine(OrderSiegeMachineVM item)
		{
			if (!item.IsSelectable)
			{
				return;
			}
			this.OrderController.SiegeWeaponController.Select(item.SiegeWeapon);
			this._missionOrder.SetActiveOrders();
			this._callbacks.RefreshVisuals();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007510 File Offset: 0x00005710
		internal void SetSelectedSiegeMachine(OrderSiegeMachineVM item)
		{
			this.ProcessSiegeMachines();
			if (!item.IsSelectable)
			{
				return;
			}
			this.SetSiegeMachineActiveOrders(item);
			this.OrderController.SiegeWeaponController.ClearSelectedWeapons();
			this.AddSelectedSiegeMachine(item);
			this._callbacks.RefreshVisuals();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000754F File Offset: 0x0000574F
		internal void OnDeselectSiegeMachine(OrderSiegeMachineVM item)
		{
			if (item.IsSelected)
			{
				this.OrderController.SiegeWeaponController.Deselect(item.SiegeWeapon);
			}
			this._missionOrder.SetActiveOrders();
			this._callbacks.RefreshVisuals();
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000758C File Offset: 0x0000578C
		internal void OnSelectOrderSiegeMachine(OrderSiegeMachineVM item)
		{
			this.ProcessSiegeMachines();
			this._missionOrder.IsTroopPlacingActive = false;
			if (item.IsSelectable)
			{
				if (Input.DebugInput.IsControlDown())
				{
					if (item.IsSelected)
					{
						this.OnDeselectSiegeMachine(item);
					}
					else
					{
						this.AddSelectedSiegeMachine(item);
					}
				}
				else
				{
					this.SetSelectedSiegeMachine(item);
				}
				this._callbacks.RefreshVisuals();
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000075F0 File Offset: 0x000057F0
		internal void OnSelectDeploymentSiegeMachine(DeploymentSiegeMachineVM item)
		{
			this.IsSiegeDeploymentListActive = false;
			GameEntity currentSelectedEntity = this._currentSelectedEntity;
			if (currentSelectedEntity != null)
			{
				currentSelectedEntity.SetContourColor(null, true);
			}
			this._currentSelectedEntity = null;
			this._selectedDeploymentPointVM = null;
			this.SiegeDeploymentList.Clear();
			bool flag = false;
			if (item != null && (!(item.MachineType != null) || this._siegeDeploymentHandler.GetDeployableWeaponCountOfPlayer(item.MachineType) != 0) && (item.DeploymentPoint.DeployedWeapon == null || !(item.DeploymentPoint.DeployedWeapon.GetType() == item.MachineType)))
			{
				bool flag2 = !item.DeploymentPoint.IsDeployed || item.DeploymentPoint.DeployedWeapon != item.SiegeWeapon;
				if (item.DeploymentPoint.IsDeployed)
				{
					if (item.SiegeWeapon == null)
					{
						SoundEvent.PlaySound2D("event:/ui/dropdown");
					}
					item.DeploymentPoint.Disband();
				}
				flag = !this.SiegeMachineList.Any<OrderSiegeMachineVM>((OrderSiegeMachineVM s) => s.DeploymentPoint.IsDeployed);
				if (flag2 && item.SiegeWeapon != null)
				{
					SiegeEngineType machine = item.Machine;
					if (machine == DefaultSiegeEngineTypes.Catapult || machine == DefaultSiegeEngineTypes.FireCatapult || machine == DefaultSiegeEngineTypes.Onager || machine == DefaultSiegeEngineTypes.FireOnager)
					{
						SoundEvent.PlaySound2D("event:/ui/mission/catapult");
					}
					else if (machine == DefaultSiegeEngineTypes.Ram)
					{
						SoundEvent.PlaySound2D("event:/ui/mission/batteringram");
					}
					else if (machine == DefaultSiegeEngineTypes.SiegeTower)
					{
						SoundEvent.PlaySound2D("event:/ui/mission/siegetower");
					}
					else if (machine == DefaultSiegeEngineTypes.Trebuchet || machine == DefaultSiegeEngineTypes.Bricole)
					{
						SoundEvent.PlaySound2D("event:/ui/mission/catapult");
					}
					else if (machine == DefaultSiegeEngineTypes.Ballista || machine == DefaultSiegeEngineTypes.FireBallista)
					{
						SoundEvent.PlaySound2D("event:/ui/mission/ballista");
					}
					item.DeploymentPoint.Deploy(item.SiegeWeapon);
				}
			}
			this.ProcessSiegeMachines();
			if (flag && this._missionOrder.IsToggleOrderShown)
			{
				this._missionOrder.SetActiveOrders();
			}
			this._callbacks.RefreshVisuals();
			foreach (DeploymentSiegeMachineVM deploymentSiegeMachineVM in this.DeploymentTargets)
			{
				deploymentSiegeMachineVM.RefreshWithDeployedWeapon();
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00007838 File Offset: 0x00005A38
		internal void OnSelectedSiegeWeaponsChanged()
		{
			for (int i = 0; i < this.SiegeMachineList.Count; i++)
			{
				OrderSiegeMachineVM orderSiegeMachineVM = this.SiegeMachineList[i];
				orderSiegeMachineVM.IsSelected = this.OrderController.SiegeWeaponController.SelectedWeapons.Contains(orderSiegeMachineVM.SiegeWeapon);
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00007889 File Offset: 0x00005A89
		public void OnRefreshSelectedDeploymentPoint(DeploymentSiegeMachineVM item)
		{
			this.RefreshSelectedDeploymentPoint(item.DeploymentPoint);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00007898 File Offset: 0x00005A98
		public void OnEntityHover(WeakGameEntity hoveredEntity)
		{
			if (this._currentHoveredEntity == hoveredEntity)
			{
				return;
			}
			DeploymentPoint deploymentPoint = null;
			if (hoveredEntity.IsValid)
			{
				if (hoveredEntity.HasScriptOfType<DeploymentPoint>())
				{
					deploymentPoint = hoveredEntity.GetFirstScriptOfType<DeploymentPoint>();
				}
				else if (this._siegeDeploymentHandler != null)
				{
					deploymentPoint = this._siegeDeploymentHandler.PlayerDeploymentPoints.SingleOrDefault<DeploymentPoint>((DeploymentPoint dp) => dp.IsDeployed && hoveredEntity.GetScriptComponents().Any<ScriptComponentBehavior>((ScriptComponentBehavior sc) => dp.DeployedWeapon == sc));
				}
			}
			this.OnEntityHover(deploymentPoint);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000791C File Offset: 0x00005B1C
		public void OnEntityHover(DeploymentPoint deploymentPoint)
		{
			if (this._currentSelectedEntity != this._currentHoveredEntity)
			{
				GameEntity currentHoveredEntity = this._currentHoveredEntity;
				if (currentHoveredEntity != null)
				{
					currentHoveredEntity.SetContourColor(null, true);
				}
			}
			if (deploymentPoint != null)
			{
				this._currentHoveredEntity = GameEntity.CreateFromWeakEntity(deploymentPoint.IsDeployed ? deploymentPoint.DeployedWeapon.GameEntity : deploymentPoint.GameEntity);
			}
			else
			{
				this._currentHoveredEntity = null;
			}
			if (this._currentSelectedEntity != this._currentHoveredEntity)
			{
				GameEntity currentHoveredEntity2 = this._currentHoveredEntity;
				if (currentHoveredEntity2 == null)
				{
					return;
				}
				currentHoveredEntity2.SetContourColor(new uint?(4289622555U), true);
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000079B8 File Offset: 0x00005BB8
		public void OnEntitySelect(WeakGameEntity selectedEntity)
		{
			if (this._currentSelectedEntity == selectedEntity)
			{
				return;
			}
			DeploymentPoint deploymentPoint = null;
			if (selectedEntity.IsValid && this._siegeDeploymentHandler != null)
			{
				if (selectedEntity.HasScriptOfType<DeploymentPoint>())
				{
					deploymentPoint = selectedEntity.GetFirstScriptOfType<DeploymentPoint>();
				}
				else if (this._siegeDeploymentHandler != null)
				{
					deploymentPoint = this._siegeDeploymentHandler.PlayerDeploymentPoints.SingleOrDefault<DeploymentPoint>((DeploymentPoint dp) => dp.IsDeployed && selectedEntity.GetScriptComponents().Any<ScriptComponentBehavior>((ScriptComponentBehavior sc) => dp.DeployedWeapon == sc));
				}
			}
			if (deploymentPoint != null)
			{
				this._missionOrder.IsTroopPlacingActive = false;
				this.RefreshSelectedDeploymentPoint(deploymentPoint);
				return;
			}
			this.ExecuteCancelSelectedDeploymentPoint();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00007A5C File Offset: 0x00005C5C
		public void RefreshSelectedDeploymentPoint(DeploymentPoint selectedDeploymentPoint)
		{
			this.IsSiegeDeploymentListActive = false;
			foreach (DeploymentSiegeMachineVM deploymentSiegeMachineVM in this.DeploymentTargets)
			{
				if (deploymentSiegeMachineVM.DeploymentPoint == selectedDeploymentPoint)
				{
					this._selectedDeploymentPointVM = deploymentSiegeMachineVM;
				}
			}
			if (!this._selectedDeploymentPointVM.IsSelected)
			{
				this._selectedDeploymentPointVM.IsSelected = true;
			}
			this.SiegeDeploymentList.Clear();
			DeploymentSiegeMachineVM deploymentSiegeMachineVM2;
			foreach (SynchedMissionObject synchedMissionObject in selectedDeploymentPoint.DeployableWeapons)
			{
				Type type = synchedMissionObject.GetType();
				if (this._siegeDeploymentHandler.GetMaxDeployableWeaponCountOfPlayer(type) > 0)
				{
					deploymentSiegeMachineVM2 = new DeploymentSiegeMachineVM(selectedDeploymentPoint, synchedMissionObject as SiegeWeapon, this._deploymentCamera, new Action<DeploymentSiegeMachineVM>(this.OnSelectDeploymentSiegeMachine), null, selectedDeploymentPoint.IsDeployed && selectedDeploymentPoint.DeployedWeapon == synchedMissionObject);
					this.SiegeDeploymentList.Add(deploymentSiegeMachineVM2);
					deploymentSiegeMachineVM2.RemainingCount = this._siegeDeploymentHandler.GetDeployableWeaponCountOfPlayer(type);
				}
			}
			deploymentSiegeMachineVM2 = new DeploymentSiegeMachineVM(selectedDeploymentPoint, null, this._deploymentCamera, new Action<DeploymentSiegeMachineVM>(this.OnSelectDeploymentSiegeMachine), null, !selectedDeploymentPoint.IsDeployed);
			this.SiegeDeploymentList.Add(deploymentSiegeMachineVM2);
			selectedDeploymentPoint.GameEntity.SetContourColor(new uint?(4293481743U), true);
			this.IsSiegeDeploymentListActive = true;
			GameEntity currentSelectedEntity = this._currentSelectedEntity;
			if (currentSelectedEntity != null)
			{
				currentSelectedEntity.SetContourColor(null, true);
			}
			this._currentSelectedEntity = GameEntity.CreateFromWeakEntity(selectedDeploymentPoint.GameEntity);
			GameEntity currentSelectedEntity2 = this._currentSelectedEntity;
			if (currentSelectedEntity2 == null)
			{
				return;
			}
			currentSelectedEntity2.SetContourColor(new uint?(4293481743U), true);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00007C1C File Offset: 0x00005E1C
		public void ExecuteCancelSelectedDeploymentPoint()
		{
			this.OnSelectDeploymentSiegeMachine(null);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00007C28 File Offset: 0x00005E28
		public void ExecuteBeginMission()
		{
			this.IsSiegeDeploymentListActive = false;
			if (this._siegeDeploymentHandler != null && this._siegeDeploymentHandler.PlayerDeploymentPoints.Any<DeploymentPoint>((DeploymentPoint d) => !d.IsDeployed && d.DeployableWeaponTypes.Any<Type>((Type type) => this._siegeDeploymentHandler.GetDeployableWeaponCountOfPlayer(type) > 0)))
			{
				InformationManager.ShowInquiry(this._siegeDeployQueryData, false, false);
				return;
			}
			if (this._deploymentHandler != null)
			{
				this._missionOrder.TryCloseToggleOrder(false);
				this._deploymentHandler.FinishDeployment();
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00007C94 File Offset: 0x00005E94
		public void ExecuteAutoDeploy()
		{
			IMissionDeploymentPlan missionDeploymentPlan;
			this.Mission.GetDeploymentPlan<IMissionDeploymentPlan>(out missionDeploymentPlan);
			missionDeploymentPlan.RemakeDeploymentPlan(this.Mission.PlayerTeam);
			if (this._siegeDeploymentHandler != null)
			{
				this.AutoDeploySiegeMachines();
				this._siegeDeploymentHandler.AutoDeployTeamUsingTeamAI(this.Mission.PlayerTeam, true);
				return;
			}
			if (this._deploymentHandler != null)
			{
				this._deploymentHandler.AutoDeployTeamUsingDeploymentPlan(this.Mission.PlayerTeam);
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00007D08 File Offset: 0x00005F08
		private void AutoDeploySiegeMachines()
		{
			this.IsSiegeDeploymentListActive = false;
			foreach (DeploymentSiegeMachineVM deploymentSiegeMachineVM in this.DeploymentTargets)
			{
				if (!(deploymentSiegeMachineVM.MachineType != null))
				{
					deploymentSiegeMachineVM.ExecuteAction();
					DeploymentSiegeMachineVM deploymentSiegeMachineVM2 = this.SiegeDeploymentList.FirstOrDefault<DeploymentSiegeMachineVM>((DeploymentSiegeMachineVM d) => d.Machine != null && d.RemainingCount > 0);
					if (deploymentSiegeMachineVM2 != null)
					{
						deploymentSiegeMachineVM2.ExecuteAction();
					}
				}
			}
			this.IsSiegeDeploymentListActive = false;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00007DA8 File Offset: 0x00005FA8
		public void ExecuteDeployPlayerSide()
		{
			if (this._siegeDeploymentHandler != null)
			{
				this.Mission.ForceTickOccasionally = true;
				bool isTeleportingAgents = Mission.Current.IsTeleportingAgents;
				if (!this.Mission.IsNavalBattle)
				{
					this.Mission.IsTeleportingAgents = true;
				}
				if (!this.Mission.IsSallyOutBattle || this.Mission.PlayerTeam.Side == BattleSideEnum.Attacker)
				{
					this.DeployFormationsOfPlayer();
					this._siegeDeploymentHandler.ForceUpdateAllUnits();
				}
				this._missionOrder.OnDeployAll();
				foreach (OrderSiegeMachineVM orderSiegeMachineVM in this.SiegeMachineList)
				{
					orderSiegeMachineVM.RefreshSiegeWeapon();
				}
				foreach (DeploymentSiegeMachineVM deploymentSiegeMachineVM in this.DeploymentTargets)
				{
					deploymentSiegeMachineVM.RefreshWithDeployedWeapon();
				}
				if (!this.Mission.IsNavalBattle)
				{
					this.Mission.IsTeleportingAgents = isTeleportingAgents;
				}
				this.Mission.ForceTickOccasionally = false;
				this.SelectAllSiegeMachines();
				return;
			}
			if (this._deploymentHandler != null)
			{
				this.DeployFormationsOfPlayer();
				this._deploymentHandler.ForceUpdateAllUnits();
				this._missionOrder.OnDeployAll();
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00007EF4 File Offset: 0x000060F4
		private void ExecuteDeployEnemySide()
		{
			if (this._siegeDeploymentHandler != null)
			{
				this.Mission.ForceTickOccasionally = true;
				bool isTeleportingAgents = Mission.Current.IsTeleportingAgents;
				if (!this.Mission.IsNavalBattle)
				{
					this.Mission.IsTeleportingAgents = true;
				}
				if (!this.Mission.IsSallyOutBattle || this.Mission.PlayerTeam.Side == BattleSideEnum.Defender)
				{
					this._siegeDeploymentHandler.AutoDeployTeamUsingTeamAI(this.Mission.PlayerEnemyTeam, true);
					this._siegeDeploymentHandler.ForceUpdateAllUnits();
				}
				this._missionOrder.OnDeployAll();
				foreach (OrderSiegeMachineVM orderSiegeMachineVM in this.SiegeMachineList)
				{
					orderSiegeMachineVM.RefreshSiegeWeapon();
				}
				foreach (DeploymentSiegeMachineVM deploymentSiegeMachineVM in this.DeploymentTargets)
				{
					deploymentSiegeMachineVM.RefreshWithDeployedWeapon();
				}
				if (!this.Mission.IsNavalBattle)
				{
					this.Mission.IsTeleportingAgents = isTeleportingAgents;
				}
				this.Mission.ForceTickOccasionally = false;
				this.SelectAllSiegeMachines();
				return;
			}
			if (this._deploymentHandler != null)
			{
				this._deploymentHandler.ForceUpdateAllUnits();
				this._missionOrder.OnDeployAll();
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00008048 File Offset: 0x00006248
		public void FinalizeDeployment()
		{
			this._missionOrder.IsDeployment = false;
			foreach (OrderSiegeMachineVM orderSiegeMachineVM in this.SiegeMachineList.ToList<OrderSiegeMachineVM>())
			{
				if (orderSiegeMachineVM.DeploymentPoint.IsDeployed)
				{
					this.SetSiegeMachineActiveOrders(orderSiegeMachineVM);
				}
				else
				{
					this.SiegeMachineList.Remove(orderSiegeMachineVM);
				}
			}
			this.DeploymentTargets.Clear();
			this.SiegeDeploymentList.Clear();
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000080E0 File Offset: 0x000062E0
		internal void OnSelectFormationWithIndex(int formationTroopIndex)
		{
			OrderSiegeMachineVM orderSiegeMachineVM = this.SiegeMachineList.ElementAtOrDefault<OrderSiegeMachineVM>(formationTroopIndex);
			if (orderSiegeMachineVM != null)
			{
				this.OnSelectOrderSiegeMachine(orderSiegeMachineVM);
				return;
			}
			this.SelectAllSiegeMachines();
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000810C File Offset: 0x0000630C
		internal void SetCurrentActiveOrders()
		{
			if (this.SiegeMachineList.Any<OrderSiegeMachineVM>((OrderSiegeMachineVM x) => x.IsSelectable))
			{
				if (!this.SiegeMachineList.Any<OrderSiegeMachineVM>((OrderSiegeMachineVM x) => x.IsSelected))
				{
					this.SelectAllSiegeMachines();
				}
			}
			foreach (OrderSiegeMachineVM orderSiegeMachineVM in this.SiegeMachineList)
			{
				if (orderSiegeMachineVM.IsSelected)
				{
					this.SetSiegeMachineActiveOrders(orderSiegeMachineVM);
				}
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000081C0 File Offset: 0x000063C0
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.OrderController.SiegeWeaponController.OnSelectedSiegeWeaponsChanged -= this.OnSelectedSiegeWeaponsChanged;
			this.SiegeDeploymentList.Clear();
			foreach (OrderSiegeMachineVM orderSiegeMachineVM in this.SiegeMachineList.ToList<OrderSiegeMachineVM>())
			{
				if (!orderSiegeMachineVM.DeploymentPoint.IsDeployed)
				{
					this.SiegeMachineList.Remove(orderSiegeMachineVM);
				}
			}
			this._siegeDeploymentHandler = null;
			this._siegeDeployQueryData = null;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00008268 File Offset: 0x00006468
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00008270 File Offset: 0x00006470
		[DataSourceProperty]
		public MBBindingList<OrderSiegeMachineVM> SiegeMachineList
		{
			get
			{
				return this._siegeMachineList;
			}
			set
			{
				if (value != this._siegeMachineList)
				{
					this._siegeMachineList = value;
					base.OnPropertyChanged("SiegeMachineList");
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000828D File Offset: 0x0000648D
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00008295 File Offset: 0x00006495
		[DataSourceProperty]
		public MBBindingList<DeploymentSiegeMachineVM> DeploymentTargets
		{
			get
			{
				return this._deploymentTargets;
			}
			set
			{
				if (value != this._deploymentTargets)
				{
					this._deploymentTargets = value;
					base.OnPropertyChanged("DeploymentTargets");
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600020F RID: 527 RVA: 0x000082B2 File Offset: 0x000064B2
		// (set) Token: 0x06000210 RID: 528 RVA: 0x000082BC File Offset: 0x000064BC
		[DataSourceProperty]
		public bool IsSiegeDeploymentListActive
		{
			get
			{
				return this._isSiegeDeploymentListActive;
			}
			set
			{
				if (value != this._isSiegeDeploymentListActive)
				{
					this._isSiegeDeploymentListActive = value;
					base.OnPropertyChanged("IsSiegeDeploymentListActive");
					this._callbacks.ToggleMissionInputs(value);
					this._callbacks.RefreshVisuals();
					if (this._selectedDeploymentPointVM != null)
					{
						this._selectedDeploymentPointVM.IsSelected = value;
					}
				}
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00008319 File Offset: 0x00006519
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00008321 File Offset: 0x00006521
		[DataSourceProperty]
		public MBBindingList<DeploymentSiegeMachineVM> SiegeDeploymentList
		{
			get
			{
				return this._siegeDeploymentList;
			}
			set
			{
				if (value != this._siegeDeploymentList)
				{
					this._siegeDeploymentList = value;
					base.OnPropertyChanged("SiegeDeploymentList");
				}
			}
		}

		// Token: 0x040000EA RID: 234
		public const uint _entityHiglightColor = 4289622555U;

		// Token: 0x040000EB RID: 235
		public const uint _entitySelectedColor = 4293481743U;

		// Token: 0x040000EC RID: 236
		private GameEntity _currentSelectedEntity;

		// Token: 0x040000ED RID: 237
		private GameEntity _currentHoveredEntity;

		// Token: 0x040000EE RID: 238
		private InquiryData _siegeDeployQueryData;

		// Token: 0x040000EF RID: 239
		private DeploymentHandler _deploymentHandler;

		// Token: 0x040000F0 RID: 240
		private SiegeDeploymentHandler _siegeDeploymentHandler;

		// Token: 0x040000F1 RID: 241
		internal DeploymentSiegeMachineVM _selectedDeploymentPointVM;

		// Token: 0x040000F2 RID: 242
		private readonly MissionOrderVM _missionOrder;

		// Token: 0x040000F3 RID: 243
		private Camera _deploymentCamera;

		// Token: 0x040000F4 RID: 244
		private List<DeploymentPoint> _deploymentPoints;

		// Token: 0x040000F5 RID: 245
		private MissionOrderCallbacks _callbacks;

		// Token: 0x040000F6 RID: 246
		private MBBindingList<OrderSiegeMachineVM> _siegeMachineList;

		// Token: 0x040000F7 RID: 247
		private MBBindingList<DeploymentSiegeMachineVM> _siegeDeploymentList;

		// Token: 0x040000F8 RID: 248
		private MBBindingList<DeploymentSiegeMachineVM> _deploymentTargets;

		// Token: 0x040000F9 RID: 249
		private bool _isSiegeDeploymentListActive;
	}
}
