using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ViewModelCollection;

namespace TaleWorlds.CampaignSystem.FastMode
{
	// Token: 0x02000002 RID: 2
	public class FastModeOptionsProvider : ICampaignOptionProvider
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public IEnumerable<ICampaignOptionData> GetGameplayCampaignOptions()
		{
			yield return new BooleanCampaignOptionData("IsFastModeEnabled", 880, CampaignOptionEnableState.Disabled, () => 1f, delegate(float value)
			{
			}, null, false, null, null);
			yield break;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002051 File Offset: 0x00000251
		public IEnumerable<ICampaignOptionData> GetCharacterCreationCampaignOptions()
		{
			yield return new BooleanCampaignOptionData("IsFastModeEnabled", 880, CampaignOptionEnableState.Disabled, () => 1f, delegate(float value)
			{
			}, null, false, null, null);
			yield break;
		}
	}
}
