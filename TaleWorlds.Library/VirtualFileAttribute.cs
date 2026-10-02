using System;

namespace TaleWorlds.Library
{
	// Token: 0x020000A8 RID: 168
	public class VirtualFileAttribute : Attribute
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x0001655C File Offset: 0x0001475C
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x00016564 File Offset: 0x00014764
		public string Name { get; private set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x0001656D File Offset: 0x0001476D
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x00016575 File Offset: 0x00014775
		public string Content { get; private set; }

		// Token: 0x0600066C RID: 1644 RVA: 0x0001657E File Offset: 0x0001477E
		public VirtualFileAttribute(string name, string content)
		{
			this.Name = name;
			this.Content = content;
		}
	}
}
