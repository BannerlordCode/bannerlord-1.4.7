using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F9 RID: 505
	public abstract class VoiceOverModel : MBGameModel<VoiceOverModel>
	{
		// Token: 0x06001F7B RID: 8059
		public abstract string GetSoundPathForCharacter(CharacterObject character, VoiceObject voiceObject);

		// Token: 0x06001F7C RID: 8060
		public abstract string GetAccentClass(CultureObject culture, bool isHighClass);
	}
}
