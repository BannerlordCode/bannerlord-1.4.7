using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000064 RID: 100
	internal class InterfaceDefinition : TypeDefinitionBase
	{
		// Token: 0x06000356 RID: 854 RVA: 0x0000E81C File Offset: 0x0000CA1C
		public InterfaceDefinition(Type type, SaveId saveId)
			: base(type, saveId)
		{
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000E826 File Offset: 0x0000CA26
		public InterfaceDefinition(Type type, int saveId)
			: base(type, new TypeSaveId(saveId))
		{
		}
	}
}
