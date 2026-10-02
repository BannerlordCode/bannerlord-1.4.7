using System;
using TaleWorlds.Core;
using TaleWorlds.Library.EventSystem;

namespace SandBox.ViewModelCollection.MapSiege
{
	// Token: 0x02000055 RID: 85
	public class PlayerStartEngineConstructionEvent : EventBase
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x000142A5 File Offset: 0x000124A5
		// (set) Token: 0x06000558 RID: 1368 RVA: 0x000142AD File Offset: 0x000124AD
		public SiegeEngineType Engine { get; private set; }

		// Token: 0x06000559 RID: 1369 RVA: 0x000142B6 File Offset: 0x000124B6
		public PlayerStartEngineConstructionEvent(SiegeEngineType engine)
		{
			this.Engine = engine;
		}
	}
}
