using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010A RID: 266
	public class DefaultCrimeModel : CrimeModel
	{
		// Token: 0x06001757 RID: 5975 RVA: 0x0006DF0A File Offset: 0x0006C10A
		public override bool DoesPlayerHaveAnyCrimeRating(IFaction faction)
		{
			return faction.MainHeroCrimeRating > 0f;
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x0006DF19 File Offset: 0x0006C119
		public override bool IsPlayerCrimeRatingSevere(IFaction faction)
		{
			return faction.MainHeroCrimeRating >= 65f;
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x0006DF2B File Offset: 0x0006C12B
		public override bool IsPlayerCrimeRatingModerate(IFaction faction)
		{
			return faction.MainHeroCrimeRating > 30f && faction.MainHeroCrimeRating <= 65f;
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x0006DF4C File Offset: 0x0006C14C
		public override bool IsPlayerCrimeRatingMild(IFaction faction)
		{
			return faction.MainHeroCrimeRating > 0f && faction.MainHeroCrimeRating <= 30f;
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x0006DF70 File Offset: 0x0006C170
		public override float GetCost(IFaction faction, CrimeModel.PaymentMethod paymentMethod, float minimumCrimeRating)
		{
			float num = MathF.Max(0f, faction.MainHeroCrimeRating - minimumCrimeRating);
			if (paymentMethod == CrimeModel.PaymentMethod.Gold)
			{
				return (float)((int)(MathF.Pow(num, 1.2f) * 100f));
			}
			if (paymentMethod != CrimeModel.PaymentMethod.Influence)
			{
				return 0f;
			}
			return MathF.Pow(num, 1.2f);
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x0006DFC0 File Offset: 0x0006C1C0
		public override ExplainedNumber GetDailyCrimeRatingChange(IFaction faction, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			int num = faction.Settlements.Count<Settlement>(delegate(Settlement x)
			{
				if (x.IsTown)
				{
					return x.Alleys.Any<Alley>((Alley y) => y.Owner == Hero.MainHero);
				}
				return false;
			});
			explainedNumber.Add((float)num * Campaign.Current.Models.AlleyModel.GetDailyCrimeRatingOfAlley, includeDescriptions ? new TextObject("{=t87T82jq}Owned alleys", null) : null, null);
			if (faction.MainHeroCrimeRating.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return explainedNumber;
			}
			Clan clan = faction as Clan;
			if (Hero.MainHero.Clan == faction)
			{
				explainedNumber.Add(-5f, includeDescriptions ? new TextObject("{=eNtRt6F5}Your own Clan", null) : null, null);
			}
			else if (faction.IsKingdomFaction && faction.Leader == Hero.MainHero)
			{
				explainedNumber.Add(-5f, includeDescriptions ? new TextObject("{=xer2bta5}Your own Kingdom", null) : null, null);
			}
			else if (Hero.MainHero.MapFaction == faction)
			{
				explainedNumber.Add(-1.5f, includeDescriptions ? new TextObject("{=QRwaQIbm}Is in Kingdom", null) : null, null);
			}
			else if (clan != null && Hero.MainHero.MapFaction == clan.Kingdom)
			{
				explainedNumber.Add(-1.25f, includeDescriptions ? new TextObject("{=hXGByLG9}Sharing the same Kingdom", null) : null, null);
			}
			else if (Hero.MainHero.Clan.IsAtWarWith(faction))
			{
				explainedNumber.Add(-0.25f, includeDescriptions ? new TextObject("{=BYTrUJyj}In War", null) : null, null);
			}
			else
			{
				explainedNumber.Add(-1f, includeDescriptions ? new TextObject("{=basevalue}Base", null) : null, null);
			}
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.WhiteLies, Hero.MainHero.CharacterObject, true, ref explainedNumber, false);
			return explainedNumber;
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x0006E18C File Offset: 0x0006C38C
		public override float DeclareWarCrimeRatingThreshold
		{
			get
			{
				return 60f;
			}
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0006E193 File Offset: 0x0006C393
		public override float GetMaxCrimeRating()
		{
			return 100f;
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0006E19A File Offset: 0x0006C39A
		public override float GetMinAcceptableCrimeRating(IFaction faction)
		{
			if (faction != Hero.MainHero.MapFaction)
			{
				return 30f;
			}
			return 20f;
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0006E1B4 File Offset: 0x0006C3B4
		public override float GetCrimeRatingAfterPunishment()
		{
			return 25f;
		}

		// Token: 0x040007D9 RID: 2009
		private const float ModerateCrimeRatingThreshold = 30f;

		// Token: 0x040007DA RID: 2010
		private const float SevereCrimeRatingThreshold = 65f;
	}
}
