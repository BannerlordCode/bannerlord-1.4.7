using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200004C RID: 76
	public struct PropertyChangedWithFloatValueEventArgs
	{
		// Token: 0x0600026B RID: 619 RVA: 0x00007C03 File Offset: 0x00005E03
		public PropertyChangedWithFloatValueEventArgs(string propertyName, float value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00007C13 File Offset: 0x00005E13
		public string PropertyName { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00007C1B File Offset: 0x00005E1B
		public float Value { get; }
	}
}
