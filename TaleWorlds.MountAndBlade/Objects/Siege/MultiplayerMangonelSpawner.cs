using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B6 RID: 950
	public class MultiplayerMangonelSpawner : MangonelSpawner
	{
		// Token: 0x0600354E RID: 13646 RVA: 0x000DB234 File Offset: 0x000D9434
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}
	}
}
