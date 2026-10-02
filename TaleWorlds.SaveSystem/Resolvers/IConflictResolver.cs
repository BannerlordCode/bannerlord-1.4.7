using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Resolvers
{
	// Token: 0x02000032 RID: 50
	public interface IConflictResolver
	{
		// Token: 0x0600020F RID: 527
		bool IsApplicable(ApplicationVersion version);

		// Token: 0x06000210 RID: 528
		Type GetNewType();

		// Token: 0x06000211 RID: 529
		MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId);

		// Token: 0x06000212 RID: 530
		MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId);
	}
}
