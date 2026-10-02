using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000E1 RID: 225
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public sealed class AssertionConditionAttribute : Attribute
	{
		// Token: 0x06000905 RID: 2309 RVA: 0x0000F5FF File Offset: 0x0000D7FF
		public AssertionConditionAttribute(AssertionConditionType conditionType)
		{
			this.ConditionType = conditionType;
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x0000F60E File Offset: 0x0000D80E
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x0000F616 File Offset: 0x0000D816
		public AssertionConditionType ConditionType { get; private set; }
	}
}
