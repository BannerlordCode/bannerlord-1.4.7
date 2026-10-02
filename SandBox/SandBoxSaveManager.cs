using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace SandBox
{
	// Token: 0x02000027 RID: 39
	public class SandBoxSaveManager : ISaveManager
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00007E40 File Offset: 0x00006040
		public int GetAutoSaveInterval()
		{
			return BannerlordConfig.AutoSaveInterval;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007E47 File Offset: 0x00006047
		public bool IsAutoSaveDisabled()
		{
			return BannerlordConfig.AutoSaveInterval == -1;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007E51 File Offset: 0x00006051
		public void OnSaveOver(bool isSuccessful, string newSaveGameName)
		{
			if (isSuccessful)
			{
				BannerlordConfig.LatestSaveGameName = newSaveGameName;
				BannerlordConfig.Save();
			}
		}
	}
}
