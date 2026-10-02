using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F2 RID: 498
	public abstract class ExecutionRelationModel : MBGameModel<ExecutionRelationModel>
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06001F4B RID: 8011
		public abstract int HeroKillingHeroClanRelationPenalty { get; }

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06001F4C RID: 8012
		public abstract int HeroKillingHeroFriendRelationPenalty { get; }

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06001F4D RID: 8013
		public abstract int PlayerExecutingHeroFactionRelationPenaltyDishonorable { get; }

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06001F4E RID: 8014
		public abstract int PlayerExecutingHeroClanRelationPenaltyDishonorable { get; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06001F4F RID: 8015
		public abstract int PlayerExecutingHeroFriendRelationPenaltyDishonorable { get; }

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06001F50 RID: 8016
		public abstract int PlayerExecutingHeroHonorPenalty { get; }

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06001F51 RID: 8017
		public abstract int PlayerExecutingHeroFactionRelationPenalty { get; }

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06001F52 RID: 8018
		public abstract int PlayerExecutingHeroHonorableNobleRelationPenalty { get; }

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06001F53 RID: 8019
		public abstract int PlayerExecutingHeroClanRelationPenalty { get; }

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06001F54 RID: 8020
		public abstract int PlayerExecutingHeroFriendRelationPenalty { get; }

		// Token: 0x06001F55 RID: 8021
		public abstract int GetRelationChangeForExecutingHero(Hero victim, Hero hero, out bool showQuickNotification);
	}
}
