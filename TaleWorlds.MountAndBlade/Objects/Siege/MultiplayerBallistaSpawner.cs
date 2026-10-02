using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B1 RID: 945
	public class MultiplayerBallistaSpawner : BallistaSpawner
	{
		// Token: 0x06003544 RID: 13636 RVA: 0x000DB178 File Offset: 0x000D9378
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}
	}
}
