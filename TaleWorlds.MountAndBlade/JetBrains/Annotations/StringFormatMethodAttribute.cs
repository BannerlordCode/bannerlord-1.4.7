using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000DE RID: 222
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class StringFormatMethodAttribute : Attribute
	{
		// Token: 0x06000900 RID: 2304 RVA: 0x0000F5CF File Offset: 0x0000D7CF
		public StringFormatMethodAttribute(string formatParameterName)
		{
			this.FormatParameterName = formatParameterName;
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x0000F5DE File Offset: 0x0000D7DE
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x0000F5E6 File Offset: 0x0000D7E6
		[UsedImplicitly]
		public string FormatParameterName { get; private set; }
	}
}
