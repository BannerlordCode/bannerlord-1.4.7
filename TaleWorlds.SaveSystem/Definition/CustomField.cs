using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006D RID: 109
	public class CustomField
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00010FF0 File Offset: 0x0000F1F0
		// (set) Token: 0x060003BA RID: 954 RVA: 0x00010FF8 File Offset: 0x0000F1F8
		public string Name { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00011001 File Offset: 0x0000F201
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00011009 File Offset: 0x0000F209
		public short SaveId { get; private set; }

		// Token: 0x060003BD RID: 957 RVA: 0x00011012 File Offset: 0x0000F212
		public CustomField(string name, short saveId)
		{
			this.Name = name;
			this.SaveId = saveId;
		}
	}
}
