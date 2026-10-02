using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x02000038 RID: 56
	[OverrideView(typeof(MissionFormationMarkerUIHandler))]
	public class MissionGauntletFormationMarker : MissionBattleUIBaseView
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000ECF8 File Offset: 0x0000CEF8
		protected override void OnCreateView()
		{
			this._dataSource = new MissionFormationMarkerVM(base.Mission);
			string text = "MissionFormationMarker";
			int viewOrderPriority = this.ViewOrderPriority;
			this.ViewOrderPriority = viewOrderPriority + 1;
			this._gauntletLayer = new GauntletLayer(text, viewOrderPriority, false);
			this._gauntletLayer.LoadMovie("FormationMarker", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			this._formationTargetHandler = base.Mission.GetMissionBehavior<MissionFormationTargetSelectionHandler>();
			if (this._formationTargetHandler != null)
			{
				this._formationTargetHandler.OnFormationFocused += this.OnFormationFocusedFromHandler;
			}
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			this.UpdateShowDistanceTexts();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		protected override void OnDestroyView()
		{
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			if (this._formationTargetHandler != null)
			{
				this._formationTargetHandler.OnFormationFocused -= this.OnFormationFocusedFromHandler;
			}
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000EE2E File Offset: 0x0000D02E
		protected override void OnSuspendView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000EE44 File Offset: 0x0000D044
		protected override void OnResumeView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000EE5A File Offset: 0x0000D05A
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType optionType)
		{
			if (optionType == ManagedOptions.ManagedOptionsType.ShowFormationDistances)
			{
				this.UpdateShowDistanceTexts();
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000EE67 File Offset: 0x0000D067
		private void UpdateShowDistanceTexts()
		{
			this._showDistanceTexts = ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.ShowFormationDistances) > 1E-05f;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000EE80 File Offset: 0x0000D080
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (base.IsViewCreated)
			{
				if (base.Mission.Mode != MissionMode.Deployment)
				{
					this._dataSource.IsEnabled = base.Input.IsGameKeyDown(5) || base.Mission.IsOrderMenuOpen;
				}
				this._dataSource.IsFormationTargetRelevant = this._formationTargetHandler != null && base.Mission.IsOrderMenuOpen;
				this._dataSource.ShowDistanceTexts = this._showDistanceTexts;
				if (this._dataSource.IsEnabled)
				{
					this._dataSource.RefreshFormationMarkers();
					this.RefreshTargetProperties();
					this.UpdateMarkerPositions();
					this._fadeOutTimer = 2f;
					return;
				}
				if (this._fadeOutTimer >= 0f)
				{
					this._fadeOutTimer -= dt;
					this.UpdateMarkerPositions();
				}
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000EF58 File Offset: 0x0000D158
		private void UpdateMarkerPositions()
		{
			for (int i = 0; i < this._dataSource.Targets.Count; i++)
			{
				MissionFormationMarkerTargetVM missionFormationMarkerTargetVM = this._dataSource.Targets[i];
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				WorldPosition cachedMedianPosition = missionFormationMarkerTargetVM.Formation.CachedMedianPosition;
				if (cachedMedianPosition.IsValid)
				{
					MBWindowManager.WorldToScreen(base.MissionScreen.CombatCamera, cachedMedianPosition.GetGroundVec3() + this._heightOffset, ref num, ref num2, ref num3);
					if (!MathF.IsValidValue(num3) || !MathF.IsValidValue(num) || !MathF.IsValidValue(num2))
					{
						num = -10000f;
						num2 = -10000f;
						num3 = -1f;
					}
					missionFormationMarkerTargetVM.WSign = ((num3 < 0f) ? (-1) : 1);
					missionFormationMarkerTargetVM.Distance = base.MissionScreen.CombatCamera.Position.Distance(cachedMedianPosition.GetGroundVec3());
					missionFormationMarkerTargetVM.ScreenPosition = new Vec2(num, num2);
					if (this._dataSource.ShowDistanceTexts)
					{
						MissionFormationMarkerTargetVM missionFormationMarkerTargetVM2 = missionFormationMarkerTargetVM;
						Agent main = Agent.Main;
						missionFormationMarkerTargetVM2.DistanceText = ((main != null && main.IsActive()) ? ((int)Agent.Main.Position.Distance(cachedMedianPosition.GetGroundVec3())).ToString() : ((int)missionFormationMarkerTargetVM.Distance).ToString());
					}
					else
					{
						missionFormationMarkerTargetVM.DistanceText = string.Empty;
					}
				}
				else
				{
					missionFormationMarkerTargetVM.WSign = -1;
					missionFormationMarkerTargetVM.Distance = 10000f;
					missionFormationMarkerTargetVM.DistanceText = string.Empty;
					missionFormationMarkerTargetVM.ScreenPosition = new Vec2(-10000f, -10000f);
				}
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000F0FC File Offset: 0x0000D2FC
		private unsafe void RefreshTargetProperties()
		{
			if (!this._dataSource.IsFormationTargetRelevant)
			{
				for (int i = 0; i < this._dataSource.Targets.Count; i++)
				{
					this._dataSource.Targets[i].SetTargetedState(false, false);
				}
				return;
			}
			List<Formation> list = new List<Formation>();
			Agent main = Agent.Main;
			MBReadOnlyList<Formation> mbreadOnlyList;
			if (main == null)
			{
				mbreadOnlyList = null;
			}
			else
			{
				OrderController playerOrderController = main.Team.PlayerOrderController;
				mbreadOnlyList = ((playerOrderController != null) ? playerOrderController.SelectedFormations : null);
			}
			MBReadOnlyList<Formation> mbreadOnlyList2 = mbreadOnlyList;
			if (mbreadOnlyList2 != null)
			{
				for (int j = 0; j < mbreadOnlyList2.Count; j++)
				{
					if (mbreadOnlyList2[j].TargetFormation != null)
					{
						MovementOrder movementOrder = *mbreadOnlyList2[j].GetReadonlyMovementOrderReference();
						if (movementOrder.OrderType == OrderType.Charge || movementOrder.OrderType == OrderType.Advance)
						{
							list.Add(mbreadOnlyList2[j].TargetFormation);
						}
					}
				}
			}
			for (int k = 0; k < this._dataSource.Targets.Count; k++)
			{
				MissionFormationMarkerTargetVM missionFormationMarkerTargetVM = this._dataSource.Targets[k];
				if (missionFormationMarkerTargetVM.TeamType == 2)
				{
					bool flag = list.Contains(missionFormationMarkerTargetVM.Formation);
					MissionFormationMarkerTargetVM missionFormationMarkerTargetVM2 = missionFormationMarkerTargetVM;
					MBReadOnlyList<Formation> focusedFormationsCache = this._focusedFormationsCache;
					missionFormationMarkerTargetVM2.SetTargetedState(focusedFormationsCache != null && focusedFormationsCache.Contains(missionFormationMarkerTargetVM.Formation), flag);
				}
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000F23D File Offset: 0x0000D43D
		private void OnFormationFocusedFromHandler(MBReadOnlyList<Formation> focusedFormations)
		{
			this._focusedFormationsCache = focusedFormations;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000F246 File Offset: 0x0000D446
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000F26B File Offset: 0x0000D46B
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000148 RID: 328
		private MissionFormationMarkerVM _dataSource;

		// Token: 0x04000149 RID: 329
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400014A RID: 330
		private MissionFormationTargetSelectionHandler _formationTargetHandler;

		// Token: 0x0400014B RID: 331
		private MBReadOnlyList<Formation> _focusedFormationsCache;

		// Token: 0x0400014C RID: 332
		private readonly Vec3 _heightOffset = new Vec3(0f, 0f, 3f, -1f);

		// Token: 0x0400014D RID: 333
		private float _fadeOutTimer;

		// Token: 0x0400014E RID: 334
		private bool _showDistanceTexts;
	}
}
