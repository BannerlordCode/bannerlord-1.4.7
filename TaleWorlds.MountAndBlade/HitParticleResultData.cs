using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000195 RID: 405
	[EngineStruct("Hit_particle_result_data", false, null)]
	public struct HitParticleResultData
	{
		// Token: 0x06001548 RID: 5448 RVA: 0x0004FD6B File Offset: 0x0004DF6B
		public void Reset()
		{
			this.StartHitParticleIndex = -1;
			this.ContinueHitParticleIndex = -1;
			this.EndHitParticleIndex = -1;
		}

		// Token: 0x0400064F RID: 1615
		public int StartHitParticleIndex;

		// Token: 0x04000650 RID: 1616
		public int ContinueHitParticleIndex;

		// Token: 0x04000651 RID: 1617
		public int EndHitParticleIndex;
	}
}
