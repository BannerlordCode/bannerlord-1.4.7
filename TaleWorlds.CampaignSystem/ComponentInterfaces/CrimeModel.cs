using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C7 RID: 455
	public abstract class CrimeModel : MBGameModel<CrimeModel>
	{
		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06001E06 RID: 7686
		public abstract float DeclareWarCrimeRatingThreshold { get; }

		// Token: 0x06001E07 RID: 7687
		public abstract float GetMaxCrimeRating();

		// Token: 0x06001E08 RID: 7688
		public abstract float GetMinAcceptableCrimeRating(IFaction faction);

		// Token: 0x06001E09 RID: 7689
		public abstract float GetCrimeRatingAfterPunishment();

		// Token: 0x06001E0A RID: 7690
		public abstract bool DoesPlayerHaveAnyCrimeRating(IFaction faction);

		// Token: 0x06001E0B RID: 7691
		public abstract bool IsPlayerCrimeRatingSevere(IFaction faction);

		// Token: 0x06001E0C RID: 7692
		public abstract bool IsPlayerCrimeRatingModerate(IFaction faction);

		// Token: 0x06001E0D RID: 7693
		public abstract bool IsPlayerCrimeRatingMild(IFaction faction);

		// Token: 0x06001E0E RID: 7694
		public abstract float GetCost(IFaction faction, CrimeModel.PaymentMethod paymentMethod, float minimumCrimeRating);

		// Token: 0x06001E0F RID: 7695
		public abstract ExplainedNumber GetDailyCrimeRatingChange(IFaction faction, bool includeDescriptions = false);

		// Token: 0x02000603 RID: 1539
		[Flags]
		public enum PaymentMethod : uint
		{
			// Token: 0x04001910 RID: 6416
			ExMachina = 4096U,
			// Token: 0x04001911 RID: 6417
			Gold = 1U,
			// Token: 0x04001912 RID: 6418
			Influence = 2U,
			// Token: 0x04001913 RID: 6419
			Punishment = 4U,
			// Token: 0x04001914 RID: 6420
			Execution = 8U
		}
	}
}
