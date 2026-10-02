using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D3 RID: 211
	public class HeroDeveloperResolver : IConflictResolver
	{
		// Token: 0x06001472 RID: 5234 RVA: 0x0005EFE7 File Offset: 0x0005D1E7
		public bool IsApplicable(ApplicationVersion version)
		{
			return version != ApplicationVersion.Empty && version.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0));
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0005F00C File Offset: 0x0005D20C
		public MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId == new MemberTypeId(3, 10))
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(this.GetNewType()), 0);
			}
			if (memberTypeId.TypeLevel >= 4)
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(typeof(HeroDeveloper)), memberTypeId.LocalSaveId);
			}
			return MemberTypeId.Invalid;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0005F064 File Offset: 0x0005D264
		public Type GetNewType()
		{
			return typeof(HeroDeveloper);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0005F070 File Offset: 0x0005D270
		public MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId.TypeLevel >= 4)
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(typeof(HeroDeveloper)), memberTypeId.LocalSaveId);
			}
			return MemberTypeId.Invalid;
		}
	}
}
