using System;
using System.Collections.Generic;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000038 RID: 56
	public struct RenderCallbackCollection
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000E4CE File Offset: 0x0000C6CE
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x0000E4D6 File Offset: 0x0000C6D6
		public List<Action<Texture>> SetActions { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000E4DF File Offset: 0x0000C6DF
		// (set) Token: 0x060001FA RID: 506 RVA: 0x0000E4E7 File Offset: 0x0000C6E7
		public List<Action> CancelActions { get; private set; }

		// Token: 0x060001FB RID: 507 RVA: 0x0000E4F0 File Offset: 0x0000C6F0
		public static RenderCallbackCollection CreateEmpty()
		{
			return new RenderCallbackCollection
			{
				SetActions = new List<Action<Texture>>(),
				CancelActions = new List<Action>()
			};
		}
	}
}
