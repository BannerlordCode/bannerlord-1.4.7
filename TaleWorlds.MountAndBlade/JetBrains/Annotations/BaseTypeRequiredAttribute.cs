using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000E7 RID: 231
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	[BaseTypeRequired(typeof(Attribute))]
	public sealed class BaseTypeRequiredAttribute : Attribute
	{
		// Token: 0x0600090C RID: 2316 RVA: 0x0000F63F File Offset: 0x0000D83F
		public BaseTypeRequiredAttribute(Type baseType)
		{
			this.BaseTypes = new Type[] { baseType };
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x0000F657 File Offset: 0x0000D857
		// (set) Token: 0x0600090E RID: 2318 RVA: 0x0000F65F File Offset: 0x0000D85F
		public Type[] BaseTypes { get; private set; }
	}
}
