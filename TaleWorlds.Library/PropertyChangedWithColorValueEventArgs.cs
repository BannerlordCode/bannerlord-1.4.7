using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200004E RID: 78
	public struct PropertyChangedWithColorValueEventArgs
	{
		// Token: 0x06000271 RID: 625 RVA: 0x00007C43 File Offset: 0x00005E43
		public PropertyChangedWithColorValueEventArgs(string propertyName, Color value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000272 RID: 626 RVA: 0x00007C53 File Offset: 0x00005E53
		public string PropertyName { get; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00007C5B File Offset: 0x00005E5B
		public Color Value { get; }
	}
}
