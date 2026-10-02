using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000186 RID: 390
	public abstract class SettlementAccessModel : MBGameModel<SettlementAccessModel>
	{
		// Token: 0x06001BE0 RID: 7136
		public abstract void CanMainHeroEnterSettlement(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails);

		// Token: 0x06001BE1 RID: 7137
		public abstract void CanMainHeroEnterLordsHall(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails);

		// Token: 0x06001BE2 RID: 7138
		public abstract void CanMainHeroEnterDungeon(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails);

		// Token: 0x06001BE3 RID: 7139
		public abstract bool CanMainHeroAccessLocation(Settlement settlement, string locationId, out bool disableOption, out TextObject disabledText);

		// Token: 0x06001BE4 RID: 7140
		public abstract bool CanMainHeroDoSettlementAction(Settlement settlement, SettlementAccessModel.SettlementAction settlementAction, out bool disableOption, out TextObject disabledText);

		// Token: 0x06001BE5 RID: 7141
		public abstract bool IsRequestMeetingOptionAvailable(Settlement settlement, out bool disableOption, out TextObject disabledText);

		// Token: 0x020005F6 RID: 1526
		public enum AccessLevel
		{
			// Token: 0x040018D3 RID: 6355
			NoAccess,
			// Token: 0x040018D4 RID: 6356
			LimitedAccess,
			// Token: 0x040018D5 RID: 6357
			FullAccess
		}

		// Token: 0x020005F7 RID: 1527
		public enum AccessMethod
		{
			// Token: 0x040018D7 RID: 6359
			None,
			// Token: 0x040018D8 RID: 6360
			Direct,
			// Token: 0x040018D9 RID: 6361
			ByRequest
		}

		// Token: 0x020005F8 RID: 1528
		public enum AccessLimitationReason
		{
			// Token: 0x040018DB RID: 6363
			None,
			// Token: 0x040018DC RID: 6364
			HostileFaction,
			// Token: 0x040018DD RID: 6365
			RelationshipWithOwner,
			// Token: 0x040018DE RID: 6366
			CrimeRating,
			// Token: 0x040018DF RID: 6367
			VillageIsLooted,
			// Token: 0x040018E0 RID: 6368
			Disguised,
			// Token: 0x040018E1 RID: 6369
			ClanTier,
			// Token: 0x040018E2 RID: 6370
			LocationEmpty
		}

		// Token: 0x020005F9 RID: 1529
		public enum LimitedAccessSolution
		{
			// Token: 0x040018E4 RID: 6372
			None,
			// Token: 0x040018E5 RID: 6373
			Bribe,
			// Token: 0x040018E6 RID: 6374
			Disguise
		}

		// Token: 0x020005FA RID: 1530
		public enum PreliminaryActionObligation
		{
			// Token: 0x040018E8 RID: 6376
			None,
			// Token: 0x040018E9 RID: 6377
			Optional
		}

		// Token: 0x020005FB RID: 1531
		public enum PreliminaryActionType
		{
			// Token: 0x040018EB RID: 6379
			None,
			// Token: 0x040018EC RID: 6380
			FaceCharges
		}

		// Token: 0x020005FC RID: 1532
		public enum SettlementAction
		{
			// Token: 0x040018EE RID: 6382
			RecruitTroops,
			// Token: 0x040018EF RID: 6383
			Craft,
			// Token: 0x040018F0 RID: 6384
			WalkAroundTheArena,
			// Token: 0x040018F1 RID: 6385
			JoinTournament,
			// Token: 0x040018F2 RID: 6386
			WatchTournament,
			// Token: 0x040018F3 RID: 6387
			Trade,
			// Token: 0x040018F4 RID: 6388
			WaitInSettlement,
			// Token: 0x040018F5 RID: 6389
			ManageTown
		}

		// Token: 0x020005FD RID: 1533
		public struct AccessDetails
		{
			// Token: 0x040018F6 RID: 6390
			public SettlementAccessModel.AccessLevel AccessLevel;

			// Token: 0x040018F7 RID: 6391
			public SettlementAccessModel.AccessMethod AccessMethod;

			// Token: 0x040018F8 RID: 6392
			public SettlementAccessModel.AccessLimitationReason AccessLimitationReason;

			// Token: 0x040018F9 RID: 6393
			public SettlementAccessModel.LimitedAccessSolution LimitedAccessSolution;

			// Token: 0x040018FA RID: 6394
			public SettlementAccessModel.PreliminaryActionObligation PreliminaryActionObligation;

			// Token: 0x040018FB RID: 6395
			public SettlementAccessModel.PreliminaryActionType PreliminaryActionType;
		}
	}
}
