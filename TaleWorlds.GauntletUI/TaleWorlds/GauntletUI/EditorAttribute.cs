using System;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200001F RID: 31
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class EditorAttribute : Attribute
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000C119 File Offset: 0x0000A319
		public EditorAttribute(bool includeInnerProperties = false)
		{
			this.IncludeInnerProperties = includeInnerProperties;
		}

		// Token: 0x04000134 RID: 308
		public readonly bool IncludeInnerProperties;
	}
}
