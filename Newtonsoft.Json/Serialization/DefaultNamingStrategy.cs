using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000073 RID: 115
	public class DefaultNamingStrategy : NamingStrategy
	{
		// Token: 0x0600063B RID: 1595 RVA: 0x0001AB87 File Offset: 0x00018D87
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return name;
		}
	}
}
