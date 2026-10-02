using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F0 RID: 752
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	internal sealed class DefineSynchedMissionObjectType : Attribute
	{
		// Token: 0x06002AE7 RID: 10983 RVA: 0x000A50BE File Offset: 0x000A32BE
		public DefineSynchedMissionObjectType(Type type)
		{
			this.Type = type;
		}

		// Token: 0x040010C0 RID: 4288
		public readonly Type Type;
	}
}
