using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D0 RID: 208
	public class CharacterPerksResolver : IConflictResolver
	{
		// Token: 0x06001466 RID: 5222 RVA: 0x0005EE79 File Offset: 0x0005D079
		public bool IsApplicable(ApplicationVersion version)
		{
			return version != ApplicationVersion.Empty && version.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0));
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0005EE9C File Offset: 0x0005D09C
		public MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId == new MemberTypeId(3, 10))
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(this.GetNewType()), 10);
			}
			return MemberTypeId.Invalid;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0005EEC6 File Offset: 0x0005D0C6
		public Type GetNewType()
		{
			return typeof(PropertyOwner<PerkObject>);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0005EED2 File Offset: 0x0005D0D2
		public MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId)
		{
			return MemberTypeId.Invalid;
		}
	}
}
