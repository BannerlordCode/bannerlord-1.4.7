using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000087 RID: 135
	public class EditorVisibleScriptComponentVariable : Attribute
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x0000D5B7 File Offset: 0x0000B7B7
		// (set) Token: 0x06000C22 RID: 3106 RVA: 0x0000D5BF File Offset: 0x0000B7BF
		public bool Visible { get; set; }

		// Token: 0x06000C23 RID: 3107 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
		public EditorVisibleScriptComponentVariable(bool visible)
		{
			this.Visible = visible;
		}
	}
}
