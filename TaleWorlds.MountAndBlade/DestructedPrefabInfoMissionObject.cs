using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200032F RID: 815
	[Obsolete]
	public class DestructedPrefabInfoMissionObject : MissionObject
	{
		// Token: 0x0400122E RID: 4654
		public string DestructedPrefabName;

		// Token: 0x0400122F RID: 4655
		public Vec3 Translate = new Vec3(0f, 0f, 0f, -1f);

		// Token: 0x04001230 RID: 4656
		public Vec3 Rotation = new Vec3(0f, 0f, 0f, -1f);

		// Token: 0x04001231 RID: 4657
		public Vec3 Scale = new Vec3(1f, 1f, 1f, -1f);
	}
}
