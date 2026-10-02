using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x0200002F RID: 47
	public class PlayerMoveTroopEvent : EventBase
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0001BAFA File Offset: 0x00019CFA
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x0001BB02 File Offset: 0x00019D02
		public CharacterObject Troop { get; private set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001BB0B File Offset: 0x00019D0B
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x0001BB13 File Offset: 0x00019D13
		public int Amount { get; private set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0001BB1C File Offset: 0x00019D1C
		// (set) Token: 0x060004B3 RID: 1203 RVA: 0x0001BB24 File Offset: 0x00019D24
		public bool IsPrisoner { get; private set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0001BB2D File Offset: 0x00019D2D
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x0001BB35 File Offset: 0x00019D35
		public PartyScreenLogic.PartyRosterSide FromSide { get; private set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x0001BB3E File Offset: 0x00019D3E
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x0001BB46 File Offset: 0x00019D46
		public PartyScreenLogic.PartyRosterSide ToSide { get; private set; }

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001BB4F File Offset: 0x00019D4F
		public PlayerMoveTroopEvent(CharacterObject troop, PartyScreenLogic.PartyRosterSide fromSide, PartyScreenLogic.PartyRosterSide toSide, int amount, bool isPrisoner)
		{
			this.Troop = troop;
			this.FromSide = fromSide;
			this.ToSide = toSide;
			this.IsPrisoner = isPrisoner;
			this.Amount = amount;
		}
	}
}
