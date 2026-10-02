using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200029C RID: 668
	public static class ConversationTagHelper
	{
		// Token: 0x06002411 RID: 9233 RVA: 0x0009BEB6 File Offset: 0x0009A0B6
		public static bool UsesHighRegister(CharacterObject character)
		{
			return ConversationTagHelper.EducatedClass(character) && !ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x0009BECB File Offset: 0x0009A0CB
		public static bool UsesLowRegister(CharacterObject character)
		{
			return !ConversationTagHelper.EducatedClass(character) && !ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x0009BEE0 File Offset: 0x0009A0E0
		public static bool TribalVoiceGroup(CharacterObject character)
		{
			return character.Culture.StringId == "sturgia" || character.Culture.StringId == "aserai" || character.Culture.StringId == "khuzait" || character.Culture.StringId == "battania" || character.Culture.StringId == "vlandia" || character.Culture.StringId == "nord" || character.Culture.StringId == "vakken";
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x0009BF94 File Offset: 0x0009A194
		public static bool EducatedClass(CharacterObject character)
		{
			bool flag = false;
			if (character.HeroObject != null)
			{
				Clan clan = character.HeroObject.Clan;
				if (clan != null && clan.IsNoble)
				{
					flag = true;
				}
				if (character.HeroObject.IsMerchant)
				{
					flag = true;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Siegecraft) >= 5 || character.HeroObject.GetTraitLevel(DefaultTraits.Surgery) >= 5)
				{
					flag = true;
				}
				if (character.HeroObject.IsGangLeader)
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x0009C010 File Offset: 0x0009A210
		public static int TraitCompatibility(Hero hero1, Hero hero2, TraitObject trait)
		{
			int traitLevel = hero1.GetTraitLevel(trait);
			int traitLevel2 = hero2.GetTraitLevel(trait);
			if (traitLevel > 0 && traitLevel2 > 0)
			{
				return 1;
			}
			if (traitLevel < 0 || traitLevel2 < 0)
			{
				return MathF.Abs(traitLevel - traitLevel2) * -1;
			}
			return 0;
		}
	}
}
