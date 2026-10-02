using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000013 RID: 19
	public class ConsoleMatchStartEndHandler : MissionNetwork
	{
		// Token: 0x0600013E RID: 318 RVA: 0x0000510C File Offset: 0x0000330C
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._activeOtherPlayers = new List<VirtualPlayer>();
			this._matchState = ConsoleMatchStartEndHandler.MatchState.NotPlaying;
			this._gameModeClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._visualSpawnComponent = base.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			this._visualSpawnComponent.OnMyAgentSpawnedFromVisual += this.AgentVisualSpawnComponentOnOnMyAgentVisualSpawned;
			MissionPeer.OnTeamChanged += this.OnTeamChange;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000517B File Offset: 0x0000337B
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			MissionPeer.OnTeamChanged -= this.OnTeamChange;
			if (this._matchState == ConsoleMatchStartEndHandler.MatchState.Playing)
			{
				this._matchState = ConsoleMatchStartEndHandler.MatchState.NotPlaying;
				PlatformServices.MultiplayerGameStateChanged(false);
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000051AA File Offset: 0x000033AA
		private void AgentVisualSpawnComponentOnOnMyAgentVisualSpawned()
		{
			this._visualSpawnComponent.OnMyAgentSpawnedFromVisual -= this.AgentVisualSpawnComponentOnOnMyAgentVisualSpawned;
			this._inGameCheckActive = true;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000051CC File Offset: 0x000033CC
		private void OnTeamChange(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (newTeam.Side == BattleSideEnum.None)
			{
				if (peer.IsMine)
				{
					this._visualSpawnComponent.OnMyAgentVisualSpawned += this.AgentVisualSpawnComponentOnOnMyAgentVisualSpawned;
					this._inGameCheckActive = false;
					PlatformServices.MultiplayerGameStateChanged(false);
					return;
				}
				int num = this._activeOtherPlayers.IndexOf(peer.VirtualPlayer);
				if (num >= 0)
				{
					this._activeOtherPlayers.RemoveAt(num);
				}
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005234 File Offset: 0x00003434
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.MissionPeer != null && !agent.MissionPeer.IsMine && !this._activeOtherPlayers.Contains(agent.MissionPeer.Peer))
			{
				this._activeOtherPlayers.Add(agent.MissionPeer.Peer);
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005284 File Offset: 0x00003484
		public override void OnMissionTick(float dt)
		{
			this._playingCheckTimer -= dt;
			if (this._playingCheckTimer <= 0f)
			{
				this._playingCheckTimer += 1f;
				if (this._inGameCheckActive)
				{
					if (this._activeOtherPlayers.Count > 0)
					{
						if (this._matchState == ConsoleMatchStartEndHandler.MatchState.NotPlaying)
						{
							this._matchState = ConsoleMatchStartEndHandler.MatchState.Playing;
							PlatformServices.MultiplayerGameStateChanged(true);
							return;
						}
					}
					else if (this._matchState == ConsoleMatchStartEndHandler.MatchState.Playing)
					{
						this._matchState = ConsoleMatchStartEndHandler.MatchState.NotPlaying;
						PlatformServices.MultiplayerGameStateChanged(false);
					}
				}
			}
		}

		// Token: 0x04000032 RID: 50
		private MissionMultiplayerGameModeBaseClient _gameModeClient;

		// Token: 0x04000033 RID: 51
		private MultiplayerMissionAgentVisualSpawnComponent _visualSpawnComponent;

		// Token: 0x04000034 RID: 52
		private ConsoleMatchStartEndHandler.MatchState _matchState;

		// Token: 0x04000035 RID: 53
		private bool _inGameCheckActive;

		// Token: 0x04000036 RID: 54
		private float _playingCheckTimer;

		// Token: 0x04000037 RID: 55
		private List<VirtualPlayer> _activeOtherPlayers;

		// Token: 0x0200009A RID: 154
		private enum MatchState
		{
			// Token: 0x04000191 RID: 401
			NotPlaying,
			// Token: 0x04000192 RID: 402
			Playing
		}
	}
}
