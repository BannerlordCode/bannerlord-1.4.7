using System;
using System.Reflection;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000060 RID: 96
	internal class EnumDefinition : TypeDefinitionBase
	{
		// Token: 0x0600033F RID: 831 RVA: 0x0000E568 File Offset: 0x0000C768
		public EnumDefinition(Type type, SaveId saveId, IEnumResolver resolver)
			: base(type, saveId)
		{
			this.Resolver = resolver;
			this.HasFlags = type.GetCustomAttribute<FlagsAttribute>() != null;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000E588 File Offset: 0x0000C788
		public EnumDefinition(Type type, int saveId, IEnumResolver resolver)
			: this(type, new TypeSaveId(saveId), resolver)
		{
		}

		// Token: 0x040000F5 RID: 245
		public readonly IEnumResolver Resolver;

		// Token: 0x040000F6 RID: 246
		public readonly bool HasFlags;
	}
}
