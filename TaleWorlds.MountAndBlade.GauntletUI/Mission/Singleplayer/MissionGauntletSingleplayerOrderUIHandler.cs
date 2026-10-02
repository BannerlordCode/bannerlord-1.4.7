using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x0200003F RID: 63
	[OverrideView(typeof(MissionOrderUIHandler))]
	public class MissionGauntletSingleplayerOrderUIHandler : GauntletOrderUIHandler, ISiegeDeploymentView
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00010DF7 File Offset: 0x0000EFF7
		public override bool IsValidForTick
		{
			get
			{
				return !base.MissionScreen.IsPhotoModeEnabled && !GameStateManager.Current.ActiveStateDisabledByUser && (!base.MissionScreen.IsRadialMenuActive || this._dataSource.IsToggleOrderShown);
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00010E30 File Offset: 0x0000F030
		protected virtual MissionOrderVM CreateDataSource(OrderController orderController)
		{
			MissionOrderVM missionOrderVM = new MissionOrderVM(orderController, this.IsDeployment, false);
			missionOrderVM.SetDeploymentParemeters(base.MissionScreen.CombatCamera, this.IsSiegeDeployment ? this._siegeDeploymentHandler.PlayerDeploymentPoints.ToList<DeploymentPoint>() : new List<DeploymentPoint>());
			missionOrderVM.SetCallbacks(new MissionOrderCallbacks
			{
				ToggleMissionInputs = new Action<bool>(base.ToggleScreenRotation),
				RefreshVisuals = new MissionOrderCallbacks.OnRefreshVisualsDelegate(this.RefreshVisuals),
				GetVisualOrderExecutionParameters = new MissionOrderCallbacks.GetOrderExecutionParametersDelegate(base.GetVisualOrderExecutionParameters),
				SetSuspendTroopPlacer = new MissionOrderCallbacks.ToggleOrderPositionVisibilityDelegate(this.SetSuspendTroopPlacer),
				OnActivateToggleOrder = new MissionOrderCallbacks.OnToggleActivateOrderStateDelegate(base.OnActivateToggleOrder),
				OnDeactivateToggleOrder = new MissionOrderCallbacks.OnToggleActivateOrderStateDelegate(base.OnDeactivateToggleOrder),
				OnTransferTroopsFinished = new MissionOrderCallbacks.OnTransferTroopsFinishedDelegate(this.OnTransferFinished),
				OnBeforeOrder = new MissionOrderCallbacks.OnBeforeOrderDelegate(base.OnBeforeOrder)
			});
			return missionOrderVM;
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00010F23 File Offset: 0x0000F123
		public override bool IsDeployment
		{
			get
			{
				Mission mission = base.Mission;
				return mission != null && mission.Mode == MissionMode.Deployment;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsSiegeDeployment
		{
			get
			{
				return this.IsDeployment && this._siegeDeploymentHandler != null;
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00010F4E File Offset: 0x0000F14E
		public override void OnConversationBegin()
		{
			base.OnConversationBegin();
			MissionOrderVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.TryCloseToggleOrder(false);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00010F68 File Offset: 0x0000F168
		public MissionGauntletSingleplayerOrderUIHandler()
		{
			this.ViewOrderPriority = 14;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00010F78 File Offset: 0x0000F178
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			GameKeyContext category = HotKeyManager.GetCategory("MissionOrderHotkeyCategory");
			GameKeyContext category2 = HotKeyManager.GetCategory("GenericPanelGameKeyCategory");
			base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(category);
			this._orderTroopPlacer = base.Mission.GetMissionBehavior<OrderTroopPlacer>();
			OrderTroopPlacer orderTroopPlacer = this._orderTroopPlacer;
			if (((orderTroopPlacer != null) ? orderTroopPlacer.OrderFlag : null) == null)
			{
				Debug.FailedAssert("Order troop placer's order flag is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\Mission\\Singleplayer\\MissionGauntletSingleplayerOrderUIHandler.cs", "OnMissionScreenInitialize", 74);
			}
			base.MissionScreen.OrderFlag = this._orderTroopPlacer.OrderFlag;
			Debug.Print("MissionScreen.OrderFlag has been set (SP)", 0, Debug.DebugColor.White, 17592186044416UL);
			base.MissionScreen.SetOrderFlagVisibility(false);
			this._siegeDeploymentHandler = base.Mission.GetMissionBehavior<SiegeDeploymentHandler>();
			this._formationTargetHandler = base.Mission.GetMissionBehavior<MissionFormationTargetSelectionHandler>();
			if (this._formationTargetHandler != null)
			{
				this._formationTargetHandler.OnFormationFocused += this.OnFormationFocused;
			}
			this._deploymentPointDataSources = new List<DeploymentSiegeMachineVM>();
			this._dataSource = this.CreateDataSource(base.Mission.PlayerTeam.PlayerOrderController);
			this._dataSource.SetCancelInputKey(category2.GetHotKey("ToggleEscapeMenu"));
			this._dataSource.TroopController.SetDoneInputKey(category2.GetHotKey("Confirm"));
			this._dataSource.TroopController.SetCancelInputKey(category2.GetHotKey("Exit"));
			this._dataSource.TroopController.SetResetInputKey(category2.GetHotKey("Reset"));
			this._dataSource.SetOrderIndexKey(0, category.GetGameKey(69));
			this._dataSource.SetOrderIndexKey(1, category.GetGameKey(70));
			this._dataSource.SetOrderIndexKey(2, category.GetGameKey(71));
			this._dataSource.SetOrderIndexKey(3, category.GetGameKey(72));
			this._dataSource.SetOrderIndexKey(4, category.GetGameKey(73));
			this._dataSource.SetOrderIndexKey(5, category.GetGameKey(74));
			this._dataSource.SetOrderIndexKey(6, category.GetGameKey(75));
			this._dataSource.SetOrderIndexKey(7, category.GetGameKey(76));
			this._dataSource.SetOrderIndexKey(8, category.GetGameKey(77));
			this._dataSource.SetReturnKey(category.GetGameKey(77));
			if (this.IsSiegeDeployment)
			{
				foreach (DeploymentPoint deploymentPoint in this._siegeDeploymentHandler.PlayerDeploymentPoints)
				{
					DeploymentSiegeMachineVM deploymentSiegeMachineVM = new DeploymentSiegeMachineVM(deploymentPoint, null, base.MissionScreen.CombatCamera, new Action<DeploymentSiegeMachineVM>(this._dataSource.DeploymentController.OnRefreshSelectedDeploymentPoint), new Action<DeploymentPoint>(this._dataSource.DeploymentController.OnEntityHover), false);
					Vec3 vec = deploymentPoint.GameEntity.GetFrame().origin;
					for (int i = 0; i < deploymentPoint.GameEntity.ChildCount; i++)
					{
						if (deploymentPoint.GameEntity.GetChild(i).HasTag("deployment_point_icon_target"))
						{
							vec += deploymentPoint.GameEntity.GetChild(i).GetFrame().origin;
							break;
						}
					}
					this._deploymentPointDataSources.Add(deploymentSiegeMachineVM);
					deploymentSiegeMachineVM.RemainingCount = 0;
				}
			}
			this._gauntletLayer = new GauntletLayer("MissionOrder", this.ViewOrderPriority, false);
			this._gauntletLayer.Input.RegisterHotKeyCategory(category2);
			string text;
			if (this.IsDeployment)
			{
				text = this._radialOrderMovieName;
			}
			else
			{
				text = ((BannerlordConfig.OrderType == 0) ? this._barOrderMovieName : this._radialOrderMovieName);
			}
			this._spriteCategory = UIResourceManager.LoadSpriteCategory("ui_order");
			this._movie = this._gauntletLayer.LoadMovie(text, this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			if (!this.IsDeployment && BannerlordConfig.HideBattleUI)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
			this._dataSource.InputRestrictions = this._gauntletLayer.InputRestrictions;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000113CC File Offset: 0x0000F5CC
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.OrderType)
			{
				if (!this.IsDeployment)
				{
					this._gauntletLayer.ReleaseMovie(this._movie);
					string text = ((BannerlordConfig.OrderType == 0) ? this._barOrderMovieName : this._radialOrderMovieName);
					this._movie = this._gauntletLayer.LoadMovie(text, this._dataSource);
					return;
				}
			}
			else if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.OrderLayoutType)
			{
				MissionOrderVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.OnOrderLayoutTypeChanged();
				return;
			}
			else if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.HideBattleUI)
			{
				if (!this.IsDeployment)
				{
					this._gauntletLayer.UIContext.ContextAlpha = (BannerlordConfig.HideBattleUI ? 0f : 1f);
					return;
				}
			}
			else if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.SlowDownOnOrder && !BannerlordConfig.SlowDownOnOrder && this._slowedDownMission)
			{
				base.Mission.RemoveTimeSpeedRequest(864);
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00011494 File Offset: 0x0000F694
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			if (this._formationTargetHandler != null)
			{
				this._formationTargetHandler.OnFormationFocused -= this.OnFormationFocused;
			}
			this._deploymentPointDataSources = null;
			this._orderTroopPlacer = null;
			this._movie = null;
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._siegeDeploymentHandler = null;
			this._spriteCategory.Unload();
			this._formationTargetHandler = null;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0001152D File Offset: 0x0000F72D
		protected override void OnTransferFinished()
		{
			if (!this.IsDeployment)
			{
				this.SetLayerEnabled(false);
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0001153E File Offset: 0x0000F73E
		public void OnAutoDeploy()
		{
			this._dataSource.DeploymentController.ExecuteAutoDeploy();
			this.ClearFormationSelection();
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00011556 File Offset: 0x0000F756
		public void OnBeginMission()
		{
			this._dataSource.DeploymentController.ExecuteBeginMission();
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00011568 File Offset: 0x0000F768
		protected override void SetLayerEnabled(bool isEnabled)
		{
			if (isEnabled)
			{
				if (!base.MissionScreen.IsRadialMenuActive)
				{
					if (this._dataSource == null || this._dataSource.ActiveTargetState == 0)
					{
						this._orderTroopPlacer.SuspendTroopPlacer = false;
					}
					if (!this._slowedDownMission && BannerlordConfig.SlowDownOnOrder)
					{
						base.Mission.AddTimeSpeedRequest(new Mission.TimeSpeedRequest(0.25f, 864));
						this._slowedDownMission = true;
					}
					base.MissionScreen.SetOrderFlagVisibility(true);
					Game.Current.EventManager.TriggerEvent<MissionPlayerToggledOrderViewEvent>(new MissionPlayerToggledOrderViewEvent(true));
					return;
				}
			}
			else
			{
				this.SetSuspendTroopPlacer(true);
				if (this._slowedDownMission)
				{
					base.Mission.RemoveTimeSpeedRequest(864);
					this._slowedDownMission = false;
				}
				Game.Current.EventManager.TriggerEvent<MissionPlayerToggledOrderViewEvent>(new MissionPlayerToggledOrderViewEvent(false));
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0001163C File Offset: 0x0000F83C
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			this._dataSource.OnDeploymentFinished();
			this._dataSource.TryCloseToggleOrder(false);
			this._deploymentPointDataSources.Clear();
			this.SetSuspendTroopPlacer(true);
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
			this._gauntletLayer.UIContext.ContextAlpha = (BannerlordConfig.HideBattleUI ? 0f : 1f);
			string text = ((BannerlordConfig.OrderType == 0) ? this._barOrderMovieName : this._radialOrderMovieName);
			if (text != this._radialOrderMovieName)
			{
				this._gauntletLayer.ReleaseMovie(this._movie);
				this._movie = this._gauntletLayer.LoadMovie(text, this._dataSource);
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00011700 File Offset: 0x0000F900
		public override void OnAfterDeploymentFinished()
		{
			base.OnAfterDeploymentFinished();
			this._dataSource.OnAfterDeploymentFinished();
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00011714 File Offset: 0x0000F914
		protected void RefreshVisuals()
		{
			if (this.IsSiegeDeployment)
			{
				foreach (DeploymentSiegeMachineVM deploymentSiegeMachineVM in this._deploymentPointDataSources)
				{
					deploymentSiegeMachineVM.RefreshWithDeployedWeapon();
				}
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001176C File Offset: 0x0000F96C
		public void ClearFormationSelection()
		{
			MissionOrderVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.DeploymentController.ExecuteCancelSelectedDeploymentPoint();
			}
			MissionOrderVM dataSource2 = this._dataSource;
			if (dataSource2 != null)
			{
				dataSource2.OrderController.ClearSelectedFormations();
			}
			MissionOrderVM dataSource3 = this._dataSource;
			if (dataSource3 == null)
			{
				return;
			}
			dataSource3.TryCloseToggleOrder(false);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000117AC File Offset: 0x0000F9AC
		public void OnFiltersSet(List<MissionOrderVM.FormationConfiguration> filterData)
		{
			this._dataSource.OnFiltersSet(filterData);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000117BA File Offset: 0x0000F9BA
		private void OnFormationFocused(MBReadOnlyList<Formation> focusedFormations)
		{
			this._focusedFormationsCache = focusedFormations;
			this._dataSource.SetFocusedFormations(this._focusedFormationsCache);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000117D4 File Offset: 0x0000F9D4
		void ISiegeDeploymentView.OnEntityHover(WeakGameEntity hoveredEntity)
		{
			if (!this._gauntletLayer.IsHitThisFrame)
			{
				this._dataSource.DeploymentController.OnEntityHover(hoveredEntity);
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000117F4 File Offset: 0x0000F9F4
		void ISiegeDeploymentView.OnEntitySelection(WeakGameEntity selectedEntity)
		{
			this._dataSource.DeploymentController.OnEntitySelect(selectedEntity);
		}

		// Token: 0x0400017B RID: 379
		private const float _slowDownAmountWhileOrderIsOpen = 0.25f;

		// Token: 0x0400017C RID: 380
		private const int _missionTimeSpeedRequestID = 864;

		// Token: 0x0400017D RID: 381
		private List<DeploymentSiegeMachineVM> _deploymentPointDataSources;
	}
}
