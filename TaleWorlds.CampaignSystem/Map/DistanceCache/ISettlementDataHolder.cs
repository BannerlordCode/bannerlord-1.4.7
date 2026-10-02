using System;

namespace TaleWorlds.CampaignSystem.Map.DistanceCache
{
	// Token: 0x02000226 RID: 550
	public interface ISettlementDataHolder
	{
		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06002119 RID: 8473
		CampaignVec2 GatePosition { get; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x0600211A RID: 8474
		CampaignVec2 PortPosition { get; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x0600211B RID: 8475
		string StringId { get; }

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x0600211C RID: 8476
		bool IsFortification { get; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x0600211D RID: 8477
		bool HasPort { get; }
	}
}
