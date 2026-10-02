using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000274 RID: 628
	public class AnyNotableTypeTag : ConversationTag
	{
		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06002399 RID: 9113 RVA: 0x0009B77F File Offset: 0x0009997F
		public override string StringId
		{
			get
			{
				return "AnyNotableTypeTag";
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0009B786 File Offset: 0x00099986
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsNotable;
		}

		// Token: 0x04000AAB RID: 2731
		public const string Id = "AnyNotableTypeTag";
	}
}
