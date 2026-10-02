using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200009A RID: 154
	public interface ISaveManager
	{
		// Token: 0x060012CB RID: 4811
		int GetAutoSaveInterval();

		// Token: 0x060012CC RID: 4812
		bool IsAutoSaveDisabled();

		// Token: 0x060012CD RID: 4813
		void OnSaveOver(bool isSuccessful, string newSaveGameName);
	}
}
