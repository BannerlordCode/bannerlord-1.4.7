using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000073 RID: 115
	public sealed class ParticleSystemManager
	{
		// Token: 0x06000A7F RID: 2687 RVA: 0x0000ABB2 File Offset: 0x00008DB2
		public static int GetRuntimeIdByName(string particleSystemName)
		{
			return EngineApplicationInterface.IParticleSystem.GetRuntimeIdByName(particleSystemName);
		}
	}
}
