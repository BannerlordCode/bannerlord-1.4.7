using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000DA RID: 218
	public class TutorialContextChangedEvent : EventBase
	{
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00024CDE File Offset: 0x00022EDE
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x00024CE6 File Offset: 0x00022EE6
		public TutorialContexts NewContext { get; private set; }

		// Token: 0x06000B4C RID: 2892 RVA: 0x00024CEF File Offset: 0x00022EEF
		public TutorialContextChangedEvent(TutorialContexts newContext)
		{
			this.NewContext = newContext;
		}
	}
}
