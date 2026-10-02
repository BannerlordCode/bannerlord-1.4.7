using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000050 RID: 80
	public struct PropertyChangedWithVec2ValueEventArgs
	{
		// Token: 0x06000277 RID: 631 RVA: 0x00007C83 File Offset: 0x00005E83
		public PropertyChangedWithVec2ValueEventArgs(string propertyName, Vec2 value)
		{
			this.PropertyName = propertyName;
			this.Value = value;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00007C93 File Offset: 0x00005E93
		public string PropertyName { get; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00007C9B File Offset: 0x00005E9B
		public Vec2 Value { get; }
	}
}
