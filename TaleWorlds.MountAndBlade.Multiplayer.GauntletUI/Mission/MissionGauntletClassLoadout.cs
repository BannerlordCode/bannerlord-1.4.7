using System;
using System.Collections.Generic;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200000B RID: 11
	[OverrideView(typeof(MissionLobbyEquipmentUIHandler))]
	public class MissionGauntletClassLoadout : MissionView
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004984 File Offset: 0x00002B84
		// (set) Token: 0x06000096 RID: 150 RVA: 0x0000498C File Offset: 0x00002B8C
		public bool IsActive { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004995 File Offset: 0x00002B95
		// (set) Token: 0x06000098 RID: 152 RVA: 0x0000499D File Offset: 0x00002B9D
		public bool IsForceClosed { get; private set; }

		// Token: 0x06000099 RID: 153 RVA: 0x000049A8 File Offset: 0x00002BA8
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.ViewOrderPriority = 20;
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._missionLobbyEquipmentNetworkComponent = base.Mission.GetMissionBehavior<MissionLobbyEquipmentNetworkComponent>();
			this._gameModeClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._teamSelectComponent = base.Mission.GetMissionBehavior<MultiplayerTeamSelectComponent>();
			if (this._teamSelectComponent != null)
			{
				this._teamSelectComponent.OnSelectingTeam += this.OnSelectingTeam;
			}
			this._scoreboardGauntletComponent = base.Mission.GetMissionBehavior<MissionGauntletMultiplayerScoreboard>();
			if (this._scoreboardGauntletComponent != null)
			{
				MissionGauntletMultiplayerScoreboard scoreboardGauntletComponent = this._scoreboardGauntletComponent;
				scoreboardGauntletComponent.OnScoreboardToggled = (Action<bool>)Delegate.Combine(scoreboardGauntletComponent.OnScoreboardToggled, new Action<bool>(this.OnScoreboardToggled));
			}
			this._missionNetworkComponent = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
			if (this._missionNetworkComponent != null)
			{
				this._missionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
			}
			MissionPeer.OnTeamChanged += this.OnTeamChanged;
			this._missionLobbyEquipmentNetworkComponent.OnToggleLoadout += this.OnTryToggle;
			this._missionLobbyEquipmentNetworkComponent.OnEquipmentRefreshed += this.OnPeerEquipmentRefreshed;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004AD8 File Offset: 0x00002CD8
		private void OnMyClientSynchronized()
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			this._myRepresentative = ((myPeer != null) ? myPeer.VirtualPlayer.GetComponent<MissionRepresentativeBase>() : null);
			this._myRepresentative.OnGoldUpdated += this.OnGoldUpdated;
			this._missionLobbyComponent.OnClassRestrictionChanged += this.OnGoldUpdated;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004B2F File Offset: 0x00002D2F
		private void OnTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (peer.IsMine && newTeam != null && (newTeam.IsAttacker || newTeam.IsDefender))
			{
				if (this.IsActive)
				{
					this.OnTryToggle(false);
				}
				this.OnTryToggle(true);
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004B62 File Offset: 0x00002D62
		private void OnRefreshSelection(MultiplayerClassDivisions.MPHeroClass heroClass)
		{
			this._lastSelectedHeroClass = heroClass;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004B6C File Offset: 0x00002D6C
		public override void OnMissionScreenFinalize()
		{
			if (this._gauntletLayer != null)
			{
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
				this._gauntletLayer = null;
			}
			if (this._dataSource != null)
			{
				this._dataSource.OnFinalize();
				this._dataSource = null;
			}
			if (this._teamSelectComponent != null)
			{
				this._teamSelectComponent.OnSelectingTeam -= this.OnSelectingTeam;
			}
			if (this._scoreboardGauntletComponent != null)
			{
				MissionGauntletMultiplayerScoreboard scoreboardGauntletComponent = this._scoreboardGauntletComponent;
				scoreboardGauntletComponent.OnScoreboardToggled = (Action<bool>)Delegate.Remove(scoreboardGauntletComponent.OnScoreboardToggled, new Action<bool>(this.OnScoreboardToggled));
			}
			if (this._missionNetworkComponent != null)
			{
				this._missionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
				if (this._myRepresentative != null)
				{
					this._myRepresentative.OnGoldUpdated -= this.OnGoldUpdated;
					this._missionLobbyComponent.OnClassRestrictionChanged -= this.OnGoldUpdated;
				}
			}
			this._missionLobbyEquipmentNetworkComponent.OnToggleLoadout -= this.OnTryToggle;
			this._missionLobbyEquipmentNetworkComponent.OnEquipmentRefreshed -= this.OnPeerEquipmentRefreshed;
			MissionPeer.OnTeamChanged -= this.OnTeamChanged;
			base.OnMissionScreenFinalize();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004C9C File Offset: 0x00002E9C
		private void CreateView()
		{
			if (this._dataSource != null)
			{
				Debug.FailedAssert("MissionGauntletClassLoadout datasource is already created!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.GauntletUI\\Mission\\MissionGauntletClassLoadout.cs", "CreateView", 135);
			}
			MissionMultiplayerGameModeBaseClient missionBehavior = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._dataSource = new MultiplayerClassLoadoutVM(missionBehavior, new Action<MultiplayerClassDivisions.MPHeroClass>(this.OnRefreshSelection), this._lastSelectedHeroClass);
			this._gauntletLayer = new GauntletLayer("MultiplayerClassLoadout", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerClassLoadout", this._dataSource);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004D22 File Offset: 0x00002F22
		public void OnTryToggle(bool isActive)
		{
			if (isActive)
			{
				this._tryToInitialize = true;
				return;
			}
			this.IsForceClosed = false;
			this.OnToggled(false);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004D40 File Offset: 0x00002F40
		private bool OnToggled(bool isActive)
		{
			if (this.IsActive == isActive)
			{
				return true;
			}
			if (!base.MissionScreen.SetDisplayDialog(isActive))
			{
				return false;
			}
			if (isActive)
			{
				this.CreateView();
				this._dataSource.Tick(1f);
				this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				base.MissionScreen.AddLayer(this._gauntletLayer);
			}
			else
			{
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
				this._dataSource.OnFinalize();
				this._dataSource = null;
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				this._gauntletLayer = null;
			}
			this.IsActive = isActive;
			return true;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004DE8 File Offset: 0x00002FE8
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._tryToInitialize && GameNetwork.IsMyPeerReady && GameNetwork.MyPeer.GetComponent<MissionPeer>().HasSpawnedAgentVisuals && this.OnToggled(true))
			{
				this._tryToInitialize = false;
			}
			if (this.IsActive)
			{
				this._dataSource.Tick(dt);
				MissionMultiplayerGameModeFlagDominationClient missionMultiplayerGameModeFlagDominationClient;
				if (base.Input.IsHotKeyReleased("ForfeitSpawn") && (missionMultiplayerGameModeFlagDominationClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>() as MissionMultiplayerGameModeFlagDominationClient) != null)
				{
					missionMultiplayerGameModeFlagDominationClient.OnRequestForfeitSpawn();
				}
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004E6C File Offset: 0x0000306C
		private void OnSelectingTeam(List<Team> disableTeams)
		{
			this.IsForceClosed = true;
			this.OnToggled(false);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004E7D File Offset: 0x0000307D
		private void OnScoreboardToggled(bool isEnabled)
		{
			if (isEnabled)
			{
				GauntletLayer gauntletLayer = this._gauntletLayer;
				if (gauntletLayer == null)
				{
					return;
				}
				gauntletLayer.InputRestrictions.ResetInputRestrictions();
				return;
			}
			else
			{
				GauntletLayer gauntletLayer2 = this._gauntletLayer;
				if (gauntletLayer2 == null)
				{
					return;
				}
				gauntletLayer2.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				return;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004EAF File Offset: 0x000030AF
		private void OnPeerEquipmentRefreshed(MissionPeer peer)
		{
			if (this._gameModeClient.GameType == MultiplayerGameType.Skirmish || this._gameModeClient.GameType == MultiplayerGameType.Captain)
			{
				MultiplayerClassLoadoutVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.OnPeerEquipmentRefreshed(peer);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004EDE File Offset: 0x000030DE
		private void OnGoldUpdated()
		{
			MultiplayerClassLoadoutVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnGoldUpdated();
		}

		// Token: 0x04000030 RID: 48
		private MultiplayerClassLoadoutVM _dataSource;

		// Token: 0x04000031 RID: 49
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000032 RID: 50
		private MissionRepresentativeBase _myRepresentative;

		// Token: 0x04000033 RID: 51
		private MissionNetworkComponent _missionNetworkComponent;

		// Token: 0x04000034 RID: 52
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000035 RID: 53
		private MissionLobbyEquipmentNetworkComponent _missionLobbyEquipmentNetworkComponent;

		// Token: 0x04000036 RID: 54
		private MissionMultiplayerGameModeBaseClient _gameModeClient;

		// Token: 0x04000037 RID: 55
		private MultiplayerTeamSelectComponent _teamSelectComponent;

		// Token: 0x04000038 RID: 56
		private MissionGauntletMultiplayerScoreboard _scoreboardGauntletComponent;

		// Token: 0x04000039 RID: 57
		private MultiplayerClassDivisions.MPHeroClass _lastSelectedHeroClass;

		// Token: 0x0400003C RID: 60
		private bool _tryToInitialize;
	}
}
