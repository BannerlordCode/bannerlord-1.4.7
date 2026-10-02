using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000063 RID: 99
	internal class GenericTypeDefinition : TypeDefinition
	{
		// Token: 0x06000355 RID: 853 RVA: 0x0000E811 File Offset: 0x0000CA11
		public GenericTypeDefinition(Type type, GenericSaveId saveId)
			: base(type, saveId, null)
		{
		}
	}
}
