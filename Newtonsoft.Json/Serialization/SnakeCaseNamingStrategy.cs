using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200009F RID: 159
	public class SnakeCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x06000821 RID: 2081 RVA: 0x0002319A File Offset: 0x0002139A
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x000231B0 File Offset: 0x000213B0
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x000231C1 File Offset: 0x000213C1
		public SnakeCaseNamingStrategy()
		{
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000231C9 File Offset: 0x000213C9
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToSnakeCase(name);
		}
	}
}
