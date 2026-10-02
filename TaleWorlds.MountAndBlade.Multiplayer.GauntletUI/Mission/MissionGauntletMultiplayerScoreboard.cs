using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000018 RID: 24
	[OverrideView(typeof(MissionScoreboardUIHandler))]
	public class MissionGauntletMultiplayerScoreboard : MissionView
	{
		// Token: 0x0600010A RID: 266 RVA: 0x00006CCF File Offset: 0x00004ECF
		[UsedImplicitly]
		public MissionGauntletMultiplayerScoreboard(bool isSingleTeam)
		{
			this._isSingleTeam = isSingleTeam;
			this.ViewOrderPriority = 25;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006CE8 File Offset: 0x00004EE8
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.InitializeLayer();
			base.Mission.IsFriendlyMission = false;
			GameKeyContext category = HotKeyManager.GetCategory("ScoreboardHotKeyCategory");
			if (!base.MissionScreen.SceneLayer.Input.IsCategoryRegistered(category))
			{
				base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(category);
			}
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._scoreboardStayDuration = MissionLobbyComponent.PostMatchWaitDuration / 2f;
			this._teamSelectComponent = base.Mission.GetMissionBehavior<MultiplayerTeamSelectComponent>();
			this.RegisterEvents();
			if (this._dataSource != null)
			{
				this._dataSource.IsActive = false;
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006D93 File Offset: 0x00004F93
		public override void OnRemoveBehavior()
		{
			this.UnregisterEvents();
			this.FinalizeLayer();
			base.OnRemoveBehavior();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006DA7 File Offset: 0x00004FA7
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this.UnregisterEvents();
			this.FinalizeLayer();
			base.OnMissionScreenFinalize();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006DC4 File Offset: 0x00004FC4
		private void RegisterEvents()
		{
			if (base.MissionScreen != null)
			{
				base.MissionScreen.OnSpectateAgentFocusIn += this.HandleSpectateAgentFocusIn;
				base.MissionScreen.OnSpectateAgentFocusOut += this.HandleSpectateAgentFocusOut;
			}
			this._missionLobbyComponent.CurrentMultiplayerStateChanged += this.MissionLobbyComponentOnCurrentMultiplayerStateChanged;
			this._missionLobbyComponent.OnCultureSelectionRequested += this.OnCultureSelectionRequested;
			if (this._teamSelectComponent != null)
			{
				this._teamSelectComponent.OnSelectingTeam += this.OnSelectingTeam;
			}
			MissionPeer.OnTeamChanged += this.OnTeamChanged;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00006E68 File Offset: 0x00005068
		private void UnregisterEvents()
		{
			if (base.MissionScreen != null)
			{
				base.MissionScreen.OnSpectateAgentFocusIn -= this.HandleSpectateAgentFocusIn;
				base.MissionScreen.OnSpectateAgentFocusOut -= this.HandleSpectateAgentFocusOut;
			}
			this._missionLobbyComponent.CurrentMultiplayerStateChanged -= this.MissionLobbyComponentOnCurrentMultiplayerStateChanged;
			this._missionLobbyComponent.OnCultureSelectionRequested -= this.OnCultureSelectionRequested;
			if (this._teamSelectComponent != null)
			{
				this._teamSelectComponent.OnSelectingTeam -= this.OnSelectingTeam;
			}
			MissionPeer.OnTeamChanged -= this.OnTeamChanged;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006F0C File Offset: 0x0000510C
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._isMissionEnding)
			{
				if (this._scoreboardStayTimeElapsed >= this._scoreboardStayDuration)
				{
					this.ToggleScoreboard(false);
					return;
				}
				this._scoreboardStayTimeElapsed += dt;
			}
			this._dataSource.Tick(dt);
			if (TaleWorlds.InputSystem.Input.IsGamepadActive)
			{
				bool flag = base.MissionScreen.SceneLayer.Input.IsGameKeyPressed(4) || this._gauntletLayer.Input.IsGameKeyPressed(4);
				if (this._isMissionEnding)
				{
					this.ToggleScoreboard(true);
				}
				else if (flag && !base.MissionScreen.IsRadialMenuActive && !base.Mission.IsOrderMenuOpen)
				{
					this.ToggleScoreboard(!this._dataSource.IsActive);
				}
			}
			else
			{
				bool flag2 = base.MissionScreen.SceneLayer.Input.IsHotKeyDown("HoldShow") || this._gauntletLayer.Input.IsHotKeyDown("HoldShow");
				bool flag3 = this._isMissionEnding || (flag2 && !base.MissionScreen.IsRadialMenuActive && !base.Mission.IsOrderMenuOpen);
				this.ToggleScoreboard(flag3);
			}
			if (this._isActive && (base.MissionScreen.SceneLayer.Input.IsGameKeyPressed(35) || this._gauntletLayer.Input.IsGameKeyPressed(35)))
			{
				this._mouseRequstedWhileScoreboardActive = true;
			}
			bool flag4 = this._isMissionEnding || (this._isActive && this._mouseRequstedWhileScoreboardActive);
			this.SetMouseState(flag4);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000070A4 File Offset: 0x000052A4
		private void ToggleScoreboard(bool isActive)
		{
			if (this._isActive != isActive)
			{
				this._isActive = isActive;
				this._dataSource.IsActive = this._isActive;
				base.MissionScreen.SetCameraLockState(this._isActive);
				if (!this._isActive)
				{
					this._mouseRequstedWhileScoreboardActive = false;
				}
				Action<bool> onScoreboardToggled = this.OnScoreboardToggled;
				if (onScoreboardToggled == null)
				{
					return;
				}
				onScoreboardToggled(this._isActive);
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007108 File Offset: 0x00005308
		private void SetMouseState(bool isMouseVisible)
		{
			if (this._isMouseVisible != isMouseVisible)
			{
				this._isMouseVisible = isMouseVisible;
				if (!this._isMouseVisible)
				{
					this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				}
				else
				{
					this._gauntletLayer.InputRestrictions.SetInputRestrictions(this._isMouseVisible, InputUsageMask.Mouse);
				}
				MissionScoreboardVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.SetMouseState(isMouseVisible);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00007168 File Offset: 0x00005368
		private void HandleSpectateAgentFocusOut(Agent followedAgent)
		{
			if (followedAgent.MissionPeer != null)
			{
				MissionPeer component = followedAgent.MissionPeer.GetComponent<MissionPeer>();
				this._dataSource.DecreaseSpectatorCount(component);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00007198 File Offset: 0x00005398
		private void HandleSpectateAgentFocusIn(Agent followedAgent)
		{
			if (followedAgent.MissionPeer != null)
			{
				MissionPeer component = followedAgent.MissionPeer.GetComponent<MissionPeer>();
				this._dataSource.IncreaseSpectatorCount(component);
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000071C5 File Offset: 0x000053C5
		private void MissionLobbyComponentOnCurrentMultiplayerStateChanged(MissionLobbyComponent.MultiplayerGameState newState)
		{
			this._isMissionEnding = newState == MissionLobbyComponent.MultiplayerGameState.Ending;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000071D1 File Offset: 0x000053D1
		private void OnTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (peer.IsMine)
			{
				this.FinalizeLayer();
				this.InitializeLayer();
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000071E8 File Offset: 0x000053E8
		private void FinalizeLayer()
		{
			if (this._dataSource != null)
			{
				this._dataSource.OnFinalize();
			}
			if (this._gauntletLayer != null)
			{
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
			}
			this._gauntletLayer = null;
			this._dataSource = null;
			this._isActive = false;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007238 File Offset: 0x00005438
		private void InitializeLayer()
		{
			this._dataSource = new MissionScoreboardVM(this._isSingleTeam, base.Mission);
			this._gauntletLayer = new GauntletLayer("MultiplayerScoreboard", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerScoreboard", this._dataSource);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("Generic"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("ScoreboardHotKeyCategory"));
			base.MissionScreen.AddLayer(this._gauntletLayer);
			this._dataSource.IsActive = this._isActive;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000072E0 File Offset: 0x000054E0
		private void OnSelectingTeam(List<Team> disableTeams)
		{
			this.ToggleScoreboard(false);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000072E9 File Offset: 0x000054E9
		private void OnCultureSelectionRequested()
		{
			this.ToggleScoreboard(false);
		}

		// Token: 0x0400006E RID: 110
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400006F RID: 111
		private MissionScoreboardVM _dataSource;

		// Token: 0x04000070 RID: 112
		private bool _isSingleTeam;

		// Token: 0x04000071 RID: 113
		private bool _isActive;

		// Token: 0x04000072 RID: 114
		private bool _isMissionEnding;

		// Token: 0x04000073 RID: 115
		private bool _mouseRequstedWhileScoreboardActive;

		// Token: 0x04000074 RID: 116
		private bool _isMouseVisible;

		// Token: 0x04000075 RID: 117
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000076 RID: 118
		private MultiplayerTeamSelectComponent _teamSelectComponent;

		// Token: 0x04000077 RID: 119
		public Action<bool> OnScoreboardToggled;

		// Token: 0x04000078 RID: 120
		private float _scoreboardStayDuration;

		// Token: 0x04000079 RID: 121
		private float _scoreboardStayTimeElapsed;
	}
}
