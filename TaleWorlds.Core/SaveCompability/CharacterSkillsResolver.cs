using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.Core.SaveCompability
{
	// Token: 0x020000E2 RID: 226
	public class CharacterSkillsResolver : IConflictResolver
	{
		// Token: 0x06000B85 RID: 2949 RVA: 0x000254F6 File Offset: 0x000236F6
		public bool IsApplicable(ApplicationVersion version)
		{
			return version != ApplicationVersion.Empty && version.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0));
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00025519 File Offset: 0x00023719
		public MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId == new MemberTypeId(3, 10))
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(this.GetNewType()), 10);
			}
			return MemberTypeId.Invalid;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00025543 File Offset: 0x00023743
		public Type GetNewType()
		{
			return typeof(PropertyOwner<SkillObject>);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002554F File Offset: 0x0002374F
		public MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId)
		{
			return MemberTypeId.Invalid;
		}
	}
}
