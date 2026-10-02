using System;

namespace SandBox.View.Map
{
	// Token: 0x0200004C RID: 76
	public class MapEncyclopediaView : MapView
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00017A7D File Offset: 0x00015C7D
		// (set) Token: 0x0600028B RID: 651 RVA: 0x00017A85 File Offset: 0x00015C85
		public bool IsEncyclopediaOpen { get; protected set; }

		// Token: 0x0600028C RID: 652 RVA: 0x00017A8E File Offset: 0x00015C8E
		public virtual void CloseEncyclopedia()
		{
		}
	}
}
