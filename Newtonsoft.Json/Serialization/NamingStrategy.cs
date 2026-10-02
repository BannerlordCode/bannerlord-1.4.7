using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000099 RID: 153
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class NamingStrategy
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00022F04 File Offset: 0x00021104
		// (set) Token: 0x06000806 RID: 2054 RVA: 0x00022F0C File Offset: 0x0002110C
		public bool ProcessDictionaryKeys { get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x00022F15 File Offset: 0x00021115
		// (set) Token: 0x06000808 RID: 2056 RVA: 0x00022F1D File Offset: 0x0002111D
		public bool ProcessExtensionDataNames { get; set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00022F26 File Offset: 0x00021126
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x00022F2E File Offset: 0x0002112E
		public bool OverrideSpecifiedNames { get; set; }

		// Token: 0x0600080B RID: 2059 RVA: 0x00022F37 File Offset: 0x00021137
		public virtual string GetPropertyName(string name, bool hasSpecifiedName)
		{
			if (hasSpecifiedName && !this.OverrideSpecifiedNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00022F4D File Offset: 0x0002114D
		public virtual string GetExtensionDataName(string name)
		{
			if (!this.ProcessExtensionDataNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00022F60 File Offset: 0x00021160
		public virtual string GetDictionaryKey(string key)
		{
			if (!this.ProcessDictionaryKeys)
			{
				return key;
			}
			return this.ResolvePropertyName(key);
		}

		// Token: 0x0600080E RID: 2062
		protected abstract string ResolvePropertyName(string name);

		// Token: 0x0600080F RID: 2063 RVA: 0x00022F74 File Offset: 0x00021174
		public override int GetHashCode()
		{
			return (((((base.GetType().GetHashCode() * 397) ^ this.ProcessDictionaryKeys.GetHashCode()) * 397) ^ this.ProcessExtensionDataNames.GetHashCode()) * 397) ^ this.OverrideSpecifiedNames.GetHashCode();
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00022FCB File Offset: 0x000211CB
		public override bool Equals(object obj)
		{
			return this.Equals(obj as NamingStrategy);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00022FDC File Offset: 0x000211DC
		[NullableContext(2)]
		protected bool Equals(NamingStrategy other)
		{
			return other != null && (base.GetType() == other.GetType() && this.ProcessDictionaryKeys == other.ProcessDictionaryKeys && this.ProcessExtensionDataNames == other.ProcessExtensionDataNames) && this.OverrideSpecifiedNames == other.OverrideSpecifiedNames;
		}
	}
}
