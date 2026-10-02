using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002CF RID: 719
	public class SiegeSpawnFrameBehavior : SpawnFrameBehaviorBase
	{
		// Token: 0x0600297E RID: 10622 RVA: 0x0009C1D0 File Offset: 0x0009A3D0
		public override void Initialize()
		{
			base.Initialize();
			this._spawnPointsByTeam = new List<GameEntity>[2];
			this._spawnZonesByTeam = new List<GameEntity>[2];
			this._spawnPointsByTeam[1] = this.SpawnPoints.Where<GameEntity>((GameEntity x) => x.HasTag("attacker")).ToList<GameEntity>();
			this._spawnPointsByTeam[0] = this.SpawnPoints.Where<GameEntity>((GameEntity x) => x.HasTag("defender")).ToList<GameEntity>();
			this._spawnZonesByTeam[1] = (from sz in this._spawnPointsByTeam[1].Select<GameEntity, GameEntity>((GameEntity sp) => sp.Parent).Distinct<GameEntity>()
				where sz != null
				select sz).ToList<GameEntity>();
			this._spawnZonesByTeam[0] = (from sz in this._spawnPointsByTeam[0].Select<GameEntity, GameEntity>((GameEntity sp) => sp.Parent).Distinct<GameEntity>()
				where sz != null
				select sz).ToList<GameEntity>();
			this._activeSpawnZoneIndex = 0;
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x0009C334 File Offset: 0x0009A534
		public override MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn)
		{
			List<GameEntity> list = new List<GameEntity>();
			GameEntity gameEntity = this._spawnZonesByTeam[(int)team.Side].First<GameEntity>((GameEntity sz) => sz.HasTag(string.Format("{0}{1}", "sp_zone_", this._activeSpawnZoneIndex)));
			list.AddRange(from sp in gameEntity.GetChildren()
				where sp.HasTag("spawnpoint")
				select sp);
			return base.GetSpawnFrameFromSpawnPoints(list, team, hasMount);
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x0009C39F File Offset: 0x0009A59F
		public void OnFlagDeactivated(FlagCapturePoint flag)
		{
			this._activeSpawnZoneIndex++;
		}

		// Token: 0x04000FE8 RID: 4072
		public const string SpawnZoneTagAffix = "sp_zone_";

		// Token: 0x04000FE9 RID: 4073
		public const string SpawnZoneEnableTagAffix = "enable_";

		// Token: 0x04000FEA RID: 4074
		public const string SpawnZoneDisableTagAffix = "disable_";

		// Token: 0x04000FEB RID: 4075
		public const int StartingActiveSpawnZoneIndex = 0;

		// Token: 0x04000FEC RID: 4076
		private List<GameEntity>[] _spawnPointsByTeam;

		// Token: 0x04000FED RID: 4077
		private List<GameEntity>[] _spawnZonesByTeam;

		// Token: 0x04000FEE RID: 4078
		private int _activeSpawnZoneIndex;
	}
}
