using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004B5 RID: 1205
	public static class GiveGoldAction
	{
		// Token: 0x06004AA6 RID: 19110 RVA: 0x00179760 File Offset: 0x00177960
		private static void ApplyInternal(Hero giverHero, PartyBase giverParty, Hero recipientHero, PartyBase recipientParty, int goldAmount, bool showQuickInformation, string transactionStringId = "")
		{
			if (giverHero != null)
			{
				goldAmount = MathF.Min(giverHero.Gold, goldAmount);
				giverHero.ChangeHeroGold(-goldAmount);
			}
			else if (giverParty != null && giverParty.IsMobile)
			{
				goldAmount = MathF.Min(giverParty.MobileParty.PartyTradeGold, goldAmount);
				giverParty.MobileParty.PartyTradeGold -= goldAmount;
			}
			else if (giverParty != null && giverParty.IsSettlement)
			{
				SettlementComponent settlementComponent = giverParty.Settlement.SettlementComponent;
				goldAmount = MathF.Min(settlementComponent.Gold, goldAmount);
				settlementComponent.ChangeGold(-goldAmount);
			}
			if (recipientHero != null)
			{
				recipientHero.ChangeHeroGold(goldAmount);
			}
			else if (recipientParty != null && recipientParty.IsMobile)
			{
				recipientParty.MobileParty.PartyTradeGold += goldAmount;
			}
			else if (recipientParty != null && recipientParty.IsSettlement)
			{
				recipientParty.Settlement.SettlementComponent.ChangeGold(goldAmount);
			}
			CampaignEventDispatcher.Instance.OnHeroOrPartyTradedGold(new ValueTuple<Hero, PartyBase>(giverHero, giverParty), new ValueTuple<Hero, PartyBase>(recipientHero, recipientParty), new ValueTuple<int, string>(goldAmount, transactionStringId), showQuickInformation);
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x0017985A File Offset: 0x00177A5A
		public static void ApplyBetweenCharacters(Hero giverHero, Hero recipientHero, int amount, bool disableNotification = false)
		{
			GiveGoldAction.ApplyInternal(giverHero, null, recipientHero, null, amount, !disableNotification && (giverHero == Hero.MainHero || recipientHero == Hero.MainHero), "");
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x00179884 File Offset: 0x00177A84
		public static void ApplyForCharacterToSettlement(Hero giverHero, Settlement settlement, int amount, bool disableNotification = false)
		{
			GiveGoldAction.ApplyInternal(giverHero, null, null, settlement.Party, amount, !disableNotification && giverHero == Hero.MainHero, "");
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x001798A8 File Offset: 0x00177AA8
		public static void ApplyForSettlementToCharacter(Settlement giverSettlement, Hero recipientHero, int amount, bool disableNotification = false)
		{
			GiveGoldAction.ApplyInternal(recipientHero, null, null, giverSettlement.Party, -amount, !disableNotification && recipientHero == Hero.MainHero, "");
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x001798CD File Offset: 0x00177ACD
		public static void ApplyForSettlementToParty(Settlement giverSettlement, PartyBase recipientParty, int amount, bool disableNotification = false)
		{
			GiveGoldAction.ApplyInternal(null, giverSettlement.Party, null, recipientParty, amount, !disableNotification && recipientParty.LeaderHero == Hero.MainHero, "");
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x001798F6 File Offset: 0x00177AF6
		public static void ApplyForPartyToSettlement(PartyBase giverParty, Settlement settlement, int amount, bool disableNotification = false)
		{
			GiveGoldAction.ApplyInternal(null, giverParty, null, settlement.Party, amount, !disableNotification && ((giverParty != null) ? giverParty.LeaderHero : null) == Hero.MainHero, "");
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x00179925 File Offset: 0x00177B25
		public static void ApplyForPartyToCharacter(PartyBase giverParty, Hero recipientHero, int amount, bool disableNotification = false)
		{
			GiveGoldAction.ApplyInternal(null, giverParty, recipientHero, null, amount, !disableNotification && giverParty != null && (giverParty.LeaderHero == Hero.MainHero || recipientHero == Hero.MainHero), "");
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x00179957 File Offset: 0x00177B57
		public static void ApplyForCharacterToParty(Hero giverHero, PartyBase receipentParty, int amount, bool disableNotification = false)
		{
			GiveGoldAction.ApplyInternal(giverHero, null, null, receipentParty, amount, !disableNotification && (giverHero == Hero.MainHero || receipentParty.LeaderHero == Hero.MainHero), "");
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x00179986 File Offset: 0x00177B86
		public static void ApplyForPartyToParty(PartyBase giverParty, PartyBase receipentParty, int amount, bool disableNotification = false)
		{
			GiveGoldAction.ApplyInternal(null, giverParty, null, receipentParty, amount, !disableNotification && (giverParty.LeaderHero == Hero.MainHero || receipentParty.LeaderHero == Hero.MainHero), "");
		}
	}
}
