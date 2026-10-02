using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000034 RID: 52
	public interface IDataStore
	{
		// Token: 0x06000367 RID: 871
		bool SyncData<T>(string key, ref T data);

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000368 RID: 872
		bool IsSaving { get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000369 RID: 873
		bool IsLoading { get; }
	}
}
