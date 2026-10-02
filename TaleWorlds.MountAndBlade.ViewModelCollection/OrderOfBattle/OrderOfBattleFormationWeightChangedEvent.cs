using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000039 RID: 57
	public class OrderOfBattleFormationWeightChangedEvent : EventBase
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00013920 File Offset: 0x00011B20
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00013928 File Offset: 0x00011B28
		public Formation Formation { get; private set; }

		// Token: 0x060004E3 RID: 1251 RVA: 0x00013931 File Offset: 0x00011B31
		public OrderOfBattleFormationWeightChangedEvent(Formation formation)
		{
			this.Formation = formation;
		}
	}
}
