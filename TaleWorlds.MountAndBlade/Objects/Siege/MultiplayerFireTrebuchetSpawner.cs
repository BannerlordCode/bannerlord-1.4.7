using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B5 RID: 949
	public class MultiplayerFireTrebuchetSpawner : TrebuchetSpawner
	{
		// Token: 0x0600354C RID: 13644 RVA: 0x000DB21D File Offset: 0x000D941D
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelperFire = new SpawnerEntityMissionHelper(this, true);
		}
	}
}
