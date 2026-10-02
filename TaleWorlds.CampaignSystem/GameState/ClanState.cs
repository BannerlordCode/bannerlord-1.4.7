using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200038C RID: 908
	public class ClanState : GameState
	{
		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x060034DB RID: 13531 RVA: 0x000D958D File Offset: 0x000D778D
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x060034DC RID: 13532 RVA: 0x000D9590 File Offset: 0x000D7790
		// (set) Token: 0x060034DD RID: 13533 RVA: 0x000D9598 File Offset: 0x000D7798
		public Hero InitialSelectedHero { get; private set; }

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x060034DE RID: 13534 RVA: 0x000D95A1 File Offset: 0x000D77A1
		// (set) Token: 0x060034DF RID: 13535 RVA: 0x000D95A9 File Offset: 0x000D77A9
		public PartyBase InitialSelectedParty { get; private set; }

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x060034E0 RID: 13536 RVA: 0x000D95B2 File Offset: 0x000D77B2
		// (set) Token: 0x060034E1 RID: 13537 RVA: 0x000D95BA File Offset: 0x000D77BA
		public Settlement InitialSelectedSettlement { get; private set; }

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x060034E2 RID: 13538 RVA: 0x000D95C3 File Offset: 0x000D77C3
		// (set) Token: 0x060034E3 RID: 13539 RVA: 0x000D95CB File Offset: 0x000D77CB
		public Workshop InitialSelectedWorkshop { get; private set; }

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x060034E4 RID: 13540 RVA: 0x000D95D4 File Offset: 0x000D77D4
		// (set) Token: 0x060034E5 RID: 13541 RVA: 0x000D95DC File Offset: 0x000D77DC
		public Alley InitialSelectedAlley { get; private set; }

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x060034E6 RID: 13542 RVA: 0x000D95E5 File Offset: 0x000D77E5
		// (set) Token: 0x060034E7 RID: 13543 RVA: 0x000D95ED File Offset: 0x000D77ED
		public IClanStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x000D95F6 File Offset: 0x000D77F6
		public ClanState()
		{
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x000D95FE File Offset: 0x000D77FE
		public ClanState(Hero initialSelectedHero)
		{
			this.InitialSelectedHero = initialSelectedHero;
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x000D960D File Offset: 0x000D780D
		public ClanState(PartyBase initialSelectedParty)
		{
			this.InitialSelectedParty = initialSelectedParty;
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x000D961C File Offset: 0x000D781C
		public ClanState(Settlement initialSelectedSettlement)
		{
			this.InitialSelectedSettlement = initialSelectedSettlement;
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x000D962B File Offset: 0x000D782B
		public ClanState(Workshop initialSelectedWorkshop)
		{
			this.InitialSelectedWorkshop = initialSelectedWorkshop;
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x000D963A File Offset: 0x000D783A
		public ClanState(Alley initialSelectedAlley)
		{
			this.InitialSelectedAlley = initialSelectedAlley;
		}

		// Token: 0x04000F22 RID: 3874
		private IClanStateHandler _handler;
	}
}
