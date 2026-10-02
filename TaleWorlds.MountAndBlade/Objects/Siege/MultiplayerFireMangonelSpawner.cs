using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B4 RID: 948
	public class MultiplayerFireMangonelSpawner : MangonelSpawner
	{
		// Token: 0x0600354A RID: 13642 RVA: 0x000DB206 File Offset: 0x000D9406
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelperFire = new SpawnerEntityMissionHelper(this, true);
		}
	}
}
