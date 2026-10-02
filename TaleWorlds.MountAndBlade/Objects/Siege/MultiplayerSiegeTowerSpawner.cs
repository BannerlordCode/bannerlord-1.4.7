using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B7 RID: 951
	public class MultiplayerSiegeTowerSpawner : SiegeTowerSpawner
	{
		// Token: 0x06003550 RID: 13648 RVA: 0x000DB24B File Offset: 0x000D944B
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			base.AssignParameters(_spawnerMissionHelper);
			_spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<DestructableComponent>().MaxHitPoint = 15000f;
			SiegeTower firstScriptOfType = _spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<SiegeTower>();
			firstScriptOfType.MaxSpeed = 1f;
			firstScriptOfType.MinSpeed = 0.5f;
		}

		// Token: 0x040016B9 RID: 5817
		private const float MaxHitPoint = 15000f;

		// Token: 0x040016BA RID: 5818
		private const float MinimumSpeed = 0.5f;

		// Token: 0x040016BB RID: 5819
		private const float MaximumSpeed = 1f;
	}
}
