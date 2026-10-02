using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D5 RID: 725
	public class ViewMethod : Attribute
	{
		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060029C8 RID: 10696 RVA: 0x0009F107 File Offset: 0x0009D307
		// (set) Token: 0x060029C9 RID: 10697 RVA: 0x0009F10F File Offset: 0x0009D30F
		public string Name { get; private set; }

		// Token: 0x060029CA RID: 10698 RVA: 0x0009F118 File Offset: 0x0009D318
		public ViewMethod(string name)
		{
			this.Name = name;
		}
	}
}
