using System;
using SandBox.Tournaments.MissionLogics;
using SandBox.View.Missions.Tournaments;
using SandBox.ViewModelCollection.Tournament;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000023 RID: 35
	[OverrideView(typeof(MissionTournamentView))]
	public class MissionGauntletTournamentView : MissionView
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0000BFBC File Offset: 0x0000A1BC
		public MissionGauntletTournamentView()
		{
			this.ViewOrderPriority = 48;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource = new TournamentVM(new Action(this.DisableUi), this._behavior);
			this._gauntletLayer = new GauntletLayer("MissionTournament", this.ViewOrderPriority, false);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._gauntletMovie = this._gauntletLayer.LoadMovie("Tournament", this._dataSource);
			base.MissionScreen.CustomCamera = this._customCamera;
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000C0DC File Offset: 0x0000A2DC
		public override void OnMissionScreenFinalize()
		{
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
			this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
			this._gauntletMovie = null;
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
			base.OnMissionScreenFinalize();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000C138 File Offset: 0x0000A338
		public override void AfterStart()
		{
			this._behavior = base.Mission.GetMissionBehavior<TournamentBehavior>();
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("camera_instance");
			this._customCamera = Camera.CreateCamera();
			Vec3 vec = default(Vec3);
			gameEntity.GetCameraParamsFromCameraScript(this._customCamera, ref vec);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000C18C File Offset: 0x0000A38C
		public override void OnMissionTick(float dt)
		{
			if (this._behavior == null)
			{
				return;
			}
			if (this._gauntletLayer.IsFocusLayer && this._dataSource.IsCurrentMatchActive)
			{
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				this._gauntletLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this._gauntletLayer);
			}
			else if (!this._gauntletLayer.IsFocusLayer && !this._dataSource.IsCurrentMatchActive)
			{
				this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				this._gauntletLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this._gauntletLayer);
			}
			if (this._dataSource.IsBetWindowEnabled)
			{
				if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.ExecuteBet();
					this._dataSource.IsBetWindowEnabled = false;
				}
				else if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.IsBetWindowEnabled = false;
				}
			}
			if (!this._viewEnabled && ((this._behavior.LastMatch != null && this._behavior.CurrentMatch == null) || this._behavior.CurrentMatch.IsReady))
			{
				this._dataSource.Refresh();
				this.ShowUi();
			}
			if (!this._viewEnabled && this._dataSource.CurrentMatch.IsValid)
			{
				TournamentMatch currentMatch = this._behavior.CurrentMatch;
				if (currentMatch != null && currentMatch.State == TournamentMatch.MatchState.Started)
				{
					this._dataSource.CurrentMatch.RefreshActiveMatch();
				}
			}
			if (this._dataSource.IsOver && this._viewEnabled && !base.DebugInput.IsControlDown() && base.DebugInput.IsHotKeyPressed("ShowHighlightsSummary"))
			{
				HighlightsController missionBehavior = base.Mission.GetMissionBehavior<HighlightsController>();
				if (missionBehavior == null)
				{
					return;
				}
				missionBehavior.ShowSummary();
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000C370 File Offset: 0x0000A570
		private void DisableUi()
		{
			if (!this._viewEnabled)
			{
				return;
			}
			base.MissionScreen.UpdateFreeCamera(this._customCamera.Frame);
			base.MissionScreen.CustomCamera = null;
			this._viewEnabled = false;
			this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000C3BF File Offset: 0x0000A5BF
		private void ShowUi()
		{
			if (this._viewEnabled)
			{
				return;
			}
			base.MissionScreen.CustomCamera = this._customCamera;
			this._viewEnabled = true;
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		public override bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return !this._viewEnabled;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000C3FF File Offset: 0x0000A5FF
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			this._dataSource.OnAgentRemoved(affectedAgent);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000C418 File Offset: 0x0000A618
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000C43D File Offset: 0x0000A63D
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000098 RID: 152
		private TournamentBehavior _behavior;

		// Token: 0x04000099 RID: 153
		private Camera _customCamera;

		// Token: 0x0400009A RID: 154
		private bool _viewEnabled = true;

		// Token: 0x0400009B RID: 155
		private GauntletMovieIdentifier _gauntletMovie;

		// Token: 0x0400009C RID: 156
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400009D RID: 157
		private TournamentVM _dataSource;
	}
}
