using System;
using System.Reflection;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000065 RID: 101
	public abstract class MemberDefinition
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000E835 File Offset: 0x0000CA35
		// (set) Token: 0x06000359 RID: 857 RVA: 0x0000E83D File Offset: 0x0000CA3D
		public MemberTypeId Id { get; private set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000E846 File Offset: 0x0000CA46
		// (set) Token: 0x0600035B RID: 859 RVA: 0x0000E84E File Offset: 0x0000CA4E
		public MemberInfo MemberInfo { get; private set; }

		// Token: 0x0600035C RID: 860 RVA: 0x0000E857 File Offset: 0x0000CA57
		protected MemberDefinition(MemberInfo memberInfo, MemberTypeId id)
		{
			this.MemberInfo = memberInfo;
			this.Id = id;
		}

		// Token: 0x0600035D RID: 861
		public abstract Type GetMemberType();

		// Token: 0x0600035E RID: 862
		public abstract object GetValue(object target);
	}
}
