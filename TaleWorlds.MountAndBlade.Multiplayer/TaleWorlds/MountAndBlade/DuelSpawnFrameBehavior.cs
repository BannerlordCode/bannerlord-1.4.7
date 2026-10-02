using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200001A RID: 26
	public class DuelSpawnFrameBehavior : SpawnFrameBehaviorBase
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00006E30 File Offset: 0x00005030
		public override void Initialize()
		{
			base.Initialize();
			this._duelAreaSpawnPoints = new List<GameEntity>[16];
			this._spawnPointSelectors = new bool[16];
			foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTagExpression("spawnpoint_area(_\\d+)*"))
			{
				int num = int.Parse(gameEntity.Tags.Single<string>((string tag) => tag.StartsWith("spawnpoint_area_")).Replace("spawnpoint_area_", "")) - 1;
				if (this._duelAreaSpawnPoints[num] == null)
				{
					this._duelAreaSpawnPoints[num] = new List<GameEntity>();
				}
				this._duelAreaSpawnPoints[num].Add(gameEntity);
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006F0C File Offset: 0x0000510C
		public override MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn)
		{
			int duelAreaIndexIfDuelTeam = Mission.Current.GetMissionBehavior<MissionMultiplayerDuel>().GetDuelAreaIndexIfDuelTeam(team);
			List<GameEntity> list = ((duelAreaIndexIfDuelTeam >= 0) ? this._duelAreaSpawnPoints[duelAreaIndexIfDuelTeam].ToList<GameEntity>() : this.SpawnPoints.ToList<GameEntity>());
			if (duelAreaIndexIfDuelTeam >= 0)
			{
				list.RemoveAt(this._spawnPointSelectors[duelAreaIndexIfDuelTeam] ? 0 : 1);
				this._spawnPointSelectors[duelAreaIndexIfDuelTeam] = !this._spawnPointSelectors[duelAreaIndexIfDuelTeam];
			}
			return base.GetSpawnFrameFromSpawnPoints(list, team, hasMount);
		}

		// Token: 0x0400005F RID: 95
		private const string AreaSpawnPointTagExpression = "spawnpoint_area(_\\d+)*";

		// Token: 0x04000060 RID: 96
		private const string AreaSpawnPointTagPrefix = "spawnpoint_area_";

		// Token: 0x04000061 RID: 97
		private List<GameEntity>[] _duelAreaSpawnPoints;

		// Token: 0x04000062 RID: 98
		private bool[] _spawnPointSelectors;
	}
}
