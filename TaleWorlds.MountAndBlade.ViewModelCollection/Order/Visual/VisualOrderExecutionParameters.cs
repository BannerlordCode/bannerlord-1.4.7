using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x02000029 RID: 41
	public readonly struct VisualOrderExecutionParameters
	{
		// Token: 0x0600036B RID: 875 RVA: 0x0000D08C File Offset: 0x0000B28C
		public VisualOrderExecutionParameters(Agent agent = null, Formation formation = null, WorldPosition? worldPosition = null)
		{
			this.HasWorldPosition = worldPosition != null;
			this.WorldPosition = ((worldPosition != null) ? worldPosition.Value : WorldPosition.Invalid);
			this.HasAgent = agent != null;
			this.Agent = agent;
			this.HasFormation = formation != null;
			this.Formation = formation;
		}

		// Token: 0x04000182 RID: 386
		public readonly bool HasWorldPosition;

		// Token: 0x04000183 RID: 387
		public readonly WorldPosition WorldPosition;

		// Token: 0x04000184 RID: 388
		public readonly bool HasAgent;

		// Token: 0x04000185 RID: 389
		public readonly Agent Agent;

		// Token: 0x04000186 RID: 390
		public readonly bool HasFormation;

		// Token: 0x04000187 RID: 391
		public readonly Formation Formation;
	}
}
