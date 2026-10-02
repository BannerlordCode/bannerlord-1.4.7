using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000038 RID: 56
	public class OrderOfBattleFormationClassChangedEvent : EventBase
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00013900 File Offset: 0x00011B00
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x00013908 File Offset: 0x00011B08
		public Formation Formation { get; private set; }

		// Token: 0x060004E0 RID: 1248 RVA: 0x00013911 File Offset: 0x00011B11
		public OrderOfBattleFormationClassChangedEvent(Formation formation)
		{
			this.Formation = formation;
		}
	}
}
