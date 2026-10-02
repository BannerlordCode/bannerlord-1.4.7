using System;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x0200020E RID: 526
	public interface ICharacterCreationContentHandler
	{
		// Token: 0x0600201A RID: 8218
		void InitializeContent(CharacterCreationManager characterCreationManager);

		// Token: 0x0600201B RID: 8219
		void AfterInitializeContent(CharacterCreationManager characterCreationManager);

		// Token: 0x0600201C RID: 8220
		void OnStageCompleted(CharacterCreationStageBase stage);

		// Token: 0x0600201D RID: 8221
		void OnCharacterCreationFinalize(CharacterCreationManager characterCreationManager);
	}
}
