using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B8 RID: 952
	public class MultiplayerTrebuchetSpawner : TrebuchetSpawner
	{
		// Token: 0x06003552 RID: 13650 RVA: 0x000DB291 File Offset: 0x000D9491
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}
	}
}
