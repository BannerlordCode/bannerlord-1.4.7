using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000178 RID: 376
	public interface IQueryData
	{
		// Token: 0x060013E4 RID: 5092
		void Expire();

		// Token: 0x060013E5 RID: 5093
		void Evaluate(float currentTime);

		// Token: 0x060013E6 RID: 5094
		void SetSyncGroup(IQueryData[] syncGroup);
	}
}
