using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000037 RID: 55
	public class OrderOfBattleHeroAssignedToFormationEvent : EventBase
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x000138C8 File Offset: 0x00011AC8
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x000138D0 File Offset: 0x00011AD0
		public Agent AssignedHero { get; private set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x000138D9 File Offset: 0x00011AD9
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x000138E1 File Offset: 0x00011AE1
		public Formation AssignedFormation { get; private set; }

		// Token: 0x060004DD RID: 1245 RVA: 0x000138EA File Offset: 0x00011AEA
		public OrderOfBattleHeroAssignedToFormationEvent(Agent assignedHero, Formation assignedFormation)
		{
			this.AssignedHero = assignedHero;
			this.AssignedFormation = assignedFormation;
		}
	}
}
