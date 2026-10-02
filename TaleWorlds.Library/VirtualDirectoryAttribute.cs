using System;

namespace TaleWorlds.Library
{
	// Token: 0x020000A7 RID: 167
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class VirtualDirectoryAttribute : Attribute
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0001653C File Offset: 0x0001473C
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x00016544 File Offset: 0x00014744
		public string Name { get; private set; }

		// Token: 0x06000667 RID: 1639 RVA: 0x0001654D File Offset: 0x0001474D
		public VirtualDirectoryAttribute(string name)
		{
			this.Name = name;
		}
	}
}
