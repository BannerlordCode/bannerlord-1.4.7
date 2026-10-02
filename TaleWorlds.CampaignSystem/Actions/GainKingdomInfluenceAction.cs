using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004B2 RID: 1202
	public static class GainKingdomInfluenceAction
	{
		// Token: 0x06004A96 RID: 19094 RVA: 0x001794B8 File Offset: 0x001776B8
		private static void ApplyInternal(Hero hero, MobileParty party, float gainedInfluence, GainKingdomInfluenceAction.InfluenceGainingReason detail)
		{
			Clan clan = null;
			if (hero != null)
			{
				if (hero.CompanionOf != null)
				{
					clan = hero.CompanionOf;
				}
				else if (hero.Clan != null)
				{
					clan = hero.Clan;
				}
			}
			else if (party.ActualClan != null)
			{
				clan = party.ActualClan;
			}
			else if (party.Owner != null)
			{
				clan = party.Owner.Clan;
			}
			if (clan == null || clan.Kingdom == null)
			{
				return;
			}
			MobileParty mobileParty = party ?? hero.PartyBelongedTo;
			if (detail != GainKingdomInfluenceAction.InfluenceGainingReason.BeingAtArmy && detail == GainKingdomInfluenceAction.InfluenceGainingReason.ClanSupport)
			{
				gainedInfluence = 0.5f;
			}
			if (detail != GainKingdomInfluenceAction.InfluenceGainingReason.Default && detail != GainKingdomInfluenceAction.InfluenceGainingReason.GivingFood && detail != GainKingdomInfluenceAction.InfluenceGainingReason.JoinFaction && detail != GainKingdomInfluenceAction.InfluenceGainingReason.ClanSupport && ((Kingdom)clan.MapFaction).ActivePolicies.Contains(DefaultPolicies.MilitaryCoronae))
			{
				gainedInfluence *= 1.2f;
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(gainedInfluence, false, null);
			if (detail == GainKingdomInfluenceAction.InfluenceGainingReason.Battle && gainedInfluence > 0f)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.PreBattleManeuvers, mobileParty, true, ref explainedNumber, false);
			}
			if (detail == GainKingdomInfluenceAction.InfluenceGainingReason.CaptureSettlement && (hero != null || mobileParty.LeaderHero != null))
			{
				Hero hero2 = hero ?? mobileParty.LeaderHero;
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Tactics.Besieged, hero2.CharacterObject, false, ref explainedNumber, false);
			}
			gainedInfluence = explainedNumber.ResultNumber;
			ChangeClanInfluenceAction.Apply(clan, gainedInfluence);
			int num = (int)gainedInfluence;
			if (MathF.Abs(num) > 0)
			{
				if ((detail == GainKingdomInfluenceAction.InfluenceGainingReason.DonatePrisoners && party == MobileParty.MainParty) || (detail == GainKingdomInfluenceAction.InfluenceGainingReason.Battle && hero == Hero.MainHero))
				{
					TextObject textObject = GameTexts.FindText("str_influence_gain_message", null);
					textObject.SetTextVariable("INFLUENCE", num);
					textObject.SetTextVariable("NEW_INFLUENCE", (int)clan.Influence);
					InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
				}
				if (detail == GainKingdomInfluenceAction.InfluenceGainingReason.SiegeSafePassage && hero == Hero.MainHero)
				{
					TextObject textObject2 = GameTexts.FindText("str_leave_siege_lose_influence_message", null);
					textObject2.SetTextVariable("INFLUENCE", -num);
					InformationManager.DisplayMessage(new InformationMessage(textObject2.ToString()));
				}
			}
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x0017966B File Offset: 0x0017786B
		public static void ApplyForBattle(Hero hero, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.Battle);
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x00179676 File Offset: 0x00177876
		public static void ApplyForGivingFood(Hero hero1, Hero hero2, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero1, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.GivingFood);
			GainKingdomInfluenceAction.ApplyInternal(hero2, null, -value, GainKingdomInfluenceAction.InfluenceGainingReason.GivingFood);
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x0017968B File Offset: 0x0017788B
		public static void ApplyForDefault(Hero hero, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.Default);
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x00179696 File Offset: 0x00177896
		public static void ApplyForJoiningFaction(Hero hero, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.JoinFaction);
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x001796A1 File Offset: 0x001778A1
		public static void ApplyForDonatePrisoners(MobileParty donatingParty, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(null, donatingParty, value, GainKingdomInfluenceAction.InfluenceGainingReason.DonatePrisoners);
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x001796AD File Offset: 0x001778AD
		public static void ApplyForRaidingEnemyVillage(MobileParty side1Party, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(null, side1Party, value, GainKingdomInfluenceAction.InfluenceGainingReason.Raiding);
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x001796B8 File Offset: 0x001778B8
		public static void ApplyForBesiegingEnemySettlement(MobileParty side1Party, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(null, side1Party, value, GainKingdomInfluenceAction.InfluenceGainingReason.Besieging);
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x001796C3 File Offset: 0x001778C3
		public static void ApplyForSiegeSafePassageBarter(MobileParty side1Party, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(null, side1Party, value, GainKingdomInfluenceAction.InfluenceGainingReason.SiegeSafePassage);
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x001796CF File Offset: 0x001778CF
		public static void ApplyForCapturingEnemySettlement(MobileParty side1Party, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(null, side1Party, value, GainKingdomInfluenceAction.InfluenceGainingReason.CaptureSettlement);
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x001796DA File Offset: 0x001778DA
		public static void ApplyForLeavingTroopToGarrison(Hero hero, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.LeaveGarrison);
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x001796E5 File Offset: 0x001778E5
		public static void ApplyForBoardGameWon(Hero hero, float value)
		{
			GainKingdomInfluenceAction.ApplyInternal(hero, null, value, GainKingdomInfluenceAction.InfluenceGainingReason.BoardGameWon);
		}

		// Token: 0x02000898 RID: 2200
		private enum InfluenceGainingReason
		{
			// Token: 0x040024BA RID: 9402
			Default,
			// Token: 0x040024BB RID: 9403
			BeingAtArmy,
			// Token: 0x040024BC RID: 9404
			Battle,
			// Token: 0x040024BD RID: 9405
			Raiding,
			// Token: 0x040024BE RID: 9406
			Besieging,
			// Token: 0x040024BF RID: 9407
			CaptureSettlement,
			// Token: 0x040024C0 RID: 9408
			JoinFaction,
			// Token: 0x040024C1 RID: 9409
			GivingFood,
			// Token: 0x040024C2 RID: 9410
			LeaveGarrison,
			// Token: 0x040024C3 RID: 9411
			BoardGameWon,
			// Token: 0x040024C4 RID: 9412
			ClanSupport,
			// Token: 0x040024C5 RID: 9413
			DonatePrisoners,
			// Token: 0x040024C6 RID: 9414
			SiegeSafePassage
		}
	}
}
