using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000093 RID: 147
	public interface ICampaignBehaviorManager
	{
		// Token: 0x06001272 RID: 4722
		void RegisterEvents();

		// Token: 0x06001273 RID: 4723
		T GetBehavior<T>();

		// Token: 0x06001274 RID: 4724
		IEnumerable<T> GetBehaviors<T>();

		// Token: 0x06001275 RID: 4725
		void AddBehavior(CampaignBehaviorBase campaignBehavior);

		// Token: 0x06001276 RID: 4726
		void RemoveBehavior<T>() where T : CampaignBehaviorBase;

		// Token: 0x06001277 RID: 4727
		void ClearBehaviors();

		// Token: 0x06001278 RID: 4728
		void LoadBehaviorData();

		// Token: 0x06001279 RID: 4729
		void InitializeCampaignBehaviors(IEnumerable<CampaignBehaviorBase> inputComponents);
	}
}
