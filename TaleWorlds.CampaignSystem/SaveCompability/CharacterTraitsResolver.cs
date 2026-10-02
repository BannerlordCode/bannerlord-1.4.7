using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D1 RID: 209
	public class CharacterTraitsResolver : IConflictResolver
	{
		// Token: 0x0600146B RID: 5227 RVA: 0x0005EEE1 File Offset: 0x0005D0E1
		public bool IsApplicable(ApplicationVersion version)
		{
			return version != ApplicationVersion.Empty && version.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0));
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0005EF04 File Offset: 0x0005D104
		public MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId == new MemberTypeId(3, 10))
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(this.GetNewType()), 10);
			}
			return MemberTypeId.Invalid;
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0005EF2E File Offset: 0x0005D12E
		public Type GetNewType()
		{
			return typeof(PropertyOwner<TraitObject>);
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0005EF3A File Offset: 0x0005D13A
		public MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId)
		{
			return MemberTypeId.Invalid;
		}
	}
}
