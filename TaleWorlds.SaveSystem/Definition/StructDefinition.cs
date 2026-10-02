using System;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006B RID: 107
	internal class StructDefinition : TypeDefinition
	{
		// Token: 0x0600039E RID: 926 RVA: 0x00010A57 File Offset: 0x0000EC57
		public StructDefinition(Type type, int saveId)
			: this(type, saveId, null)
		{
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00010A62 File Offset: 0x0000EC62
		public StructDefinition(Type type, int saveId, IObjectResolver objectResolver)
			: base(type, saveId, objectResolver)
		{
		}
	}
}
