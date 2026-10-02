using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000397 RID: 919
	public class KingdomState : GameState
	{
		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06003521 RID: 13601 RVA: 0x000D97F0 File Offset: 0x000D79F0
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06003522 RID: 13602 RVA: 0x000D97F3 File Offset: 0x000D79F3
		// (set) Token: 0x06003523 RID: 13603 RVA: 0x000D97FB File Offset: 0x000D79FB
		public Army InitialSelectedArmy { get; private set; }

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06003524 RID: 13604 RVA: 0x000D9804 File Offset: 0x000D7A04
		// (set) Token: 0x06003525 RID: 13605 RVA: 0x000D980C File Offset: 0x000D7A0C
		public Settlement InitialSelectedSettlement { get; private set; }

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06003526 RID: 13606 RVA: 0x000D9815 File Offset: 0x000D7A15
		// (set) Token: 0x06003527 RID: 13607 RVA: 0x000D981D File Offset: 0x000D7A1D
		public Clan InitialSelectedClan { get; private set; }

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06003528 RID: 13608 RVA: 0x000D9826 File Offset: 0x000D7A26
		// (set) Token: 0x06003529 RID: 13609 RVA: 0x000D982E File Offset: 0x000D7A2E
		public PolicyObject InitialSelectedPolicy { get; private set; }

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x0600352A RID: 13610 RVA: 0x000D9837 File Offset: 0x000D7A37
		// (set) Token: 0x0600352B RID: 13611 RVA: 0x000D983F File Offset: 0x000D7A3F
		public Kingdom InitialSelectedKingdom { get; private set; }

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x0600352C RID: 13612 RVA: 0x000D9848 File Offset: 0x000D7A48
		// (set) Token: 0x0600352D RID: 13613 RVA: 0x000D9850 File Offset: 0x000D7A50
		public KingdomDecision InitialSelectedDecision { get; private set; }

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x0600352E RID: 13614 RVA: 0x000D9859 File Offset: 0x000D7A59
		// (set) Token: 0x0600352F RID: 13615 RVA: 0x000D9861 File Offset: 0x000D7A61
		public IKingdomStateHandler Handler
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

		// Token: 0x06003530 RID: 13616 RVA: 0x000D986A File Offset: 0x000D7A6A
		public KingdomState()
		{
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000D9872 File Offset: 0x000D7A72
		public KingdomState(KingdomDecision initialSelectedDecision)
		{
			this.InitialSelectedDecision = initialSelectedDecision;
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x000D9881 File Offset: 0x000D7A81
		public KingdomState(Army initialSelectedArmy)
		{
			this.InitialSelectedArmy = initialSelectedArmy;
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x000D9890 File Offset: 0x000D7A90
		public KingdomState(Settlement initialSelectedSettlement)
		{
			this.InitialSelectedSettlement = initialSelectedSettlement;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x000D98A0 File Offset: 0x000D7AA0
		public KingdomState(IFaction initialSelectedFaction)
		{
			Clan clan;
			if ((clan = initialSelectedFaction as Clan) != null)
			{
				this.InitialSelectedClan = clan;
				return;
			}
			Kingdom kingdom;
			if ((kingdom = initialSelectedFaction as Kingdom) != null)
			{
				this.InitialSelectedKingdom = kingdom;
			}
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x000D98D6 File Offset: 0x000D7AD6
		public KingdomState(PolicyObject initialSelectedPolicy)
		{
			this.InitialSelectedPolicy = initialSelectedPolicy;
		}

		// Token: 0x04000F33 RID: 3891
		private IKingdomStateHandler _handler;
	}
}
