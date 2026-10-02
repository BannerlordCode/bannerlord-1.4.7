using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000049 RID: 73
	public struct PropertyChangedWithValueEventArgs
	{
		// Token: 0x06000262 RID: 610 RVA: 0x00007BA3 File Offset: 0x00005DA3
		public PropertyChangedWithValueEventArgs(string propertyName, object value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00007BB3 File Offset: 0x00005DB3
		public string PropertyName { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00007BBB File Offset: 0x00005DBB
		public object Value { get; }
	}
}
