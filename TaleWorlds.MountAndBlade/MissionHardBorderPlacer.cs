using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200028F RID: 655
	public class MissionHardBorderPlacer : MissionLogic
	{
		// Token: 0x06002472 RID: 9330 RVA: 0x000848BC File Offset: 0x00082ABC
		public override void EarlyStart()
		{
			base.EarlyStart();
			Scene scene = base.Mission.Scene;
			GameEntity gameEntity = GameEntity.CreateEmpty(scene, true, true, true);
			scene.FillEntityWithHardBorderPhysicsBarrier(gameEntity);
		}
	}
}
