using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000097 RID: 151
	public class KebabCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x060007FB RID: 2043 RVA: 0x00022D29 File Offset: 0x00020F29
		public KebabCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00022D3F File Offset: 0x00020F3F
		public KebabCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
			: this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00022D50 File Offset: 0x00020F50
		public KebabCaseNamingStrategy()
		{
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00022D58 File Offset: 0x00020F58
		[NullableContext(1)]
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToKebabCase(name);
		}
	}
}
