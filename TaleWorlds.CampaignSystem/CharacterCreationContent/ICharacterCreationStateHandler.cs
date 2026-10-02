using System;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000210 RID: 528
	public interface ICharacterCreationStateHandler
	{
		// Token: 0x0600201F RID: 8223
		void OnCharacterCreationFinalized();

		// Token: 0x06002020 RID: 8224
		void OnRefresh();

		// Token: 0x06002021 RID: 8225
		void OnStageCreated(CharacterCreationStageBase stage);
	}
}
