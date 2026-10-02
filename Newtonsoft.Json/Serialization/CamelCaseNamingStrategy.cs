using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000070 RID: 112
	public class CamelCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x060005F2 RID: 1522 RVA: 0x00018BF5 File Offset: 0x00016DF5
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00018C0B File Offset: 0x00016E0B
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00018C1C File Offset: 0x00016E1C
		public CamelCaseNamingStrategy()
		{
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00018C24 File Offset: 0x00016E24
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToCamelCase(name);
		}
	}
}
