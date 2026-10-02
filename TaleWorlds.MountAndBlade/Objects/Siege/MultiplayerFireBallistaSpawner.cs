using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B3 RID: 947
	public class MultiplayerFireBallistaSpawner : BallistaSpawner
	{
		// Token: 0x06003548 RID: 13640 RVA: 0x000DB1EF File Offset: 0x000D93EF
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelperFire = new SpawnerEntityMissionHelper(this, true);
		}
	}
}
