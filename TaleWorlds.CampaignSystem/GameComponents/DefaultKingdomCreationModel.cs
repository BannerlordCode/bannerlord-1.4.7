using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000124 RID: 292
	public class DefaultKingdomCreationModel : KingdomCreationModel
	{
		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001860 RID: 6240 RVA: 0x00076008 File Offset: 0x00074208
		public override int MinimumClanTierToCreateKingdom
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x0007600B File Offset: 0x0007420B
		public override int MinimumNumberOfSettlementsOwnedToCreateKingdom
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001862 RID: 6242 RVA: 0x0007600E File Offset: 0x0007420E
		public override int MinimumTroopCountToCreateKingdom
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001863 RID: 6243 RVA: 0x00076012 File Offset: 0x00074212
		public override int MaximumNumberOfInitialPolicies
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x00076018 File Offset: 0x00074218
		public override bool IsPlayerKingdomCreationPossible(out List<TextObject> explanations)
		{
			bool flag = true;
			explanations = new List<TextObject>();
			if (Hero.MainHero.MapFaction.IsKingdomFaction)
			{
				flag = false;
				TextObject textObject = new TextObject("{=w5b79MmE}Player clan should be independent.", null);
				explanations.Add(textObject);
			}
			if (Clan.PlayerClan.Tier < this.MinimumClanTierToCreateKingdom)
			{
				flag = false;
				TextObject textObject2 = new TextObject("{=j0UDi2AN}Clan tier should be at least {TIER}.", null);
				textObject2.SetTextVariable("TIER", this.MinimumClanTierToCreateKingdom);
				explanations.Add(textObject2);
			}
			if (Clan.PlayerClan.Settlements.Count<Settlement>((Settlement t) => t.IsTown || t.IsCastle) < this.MinimumNumberOfSettlementsOwnedToCreateKingdom)
			{
				flag = false;
				TextObject textObject3 = new TextObject("{=YsGSgaba}Number of towns or castles you own should be at least {SETTLEMENT_COUNT}.", null);
				textObject3.SetTextVariable("SETTLEMENT_COUNT", this.MinimumNumberOfSettlementsOwnedToCreateKingdom);
				explanations.Add(textObject3);
			}
			if (Clan.PlayerClan.Fiefs.Sum<Town>(delegate(Town t)
			{
				MobileParty garrisonParty = t.GarrisonParty;
				int? num;
				if (garrisonParty == null)
				{
					num = null;
				}
				else
				{
					TroopRoster memberRoster = garrisonParty.MemberRoster;
					num = ((memberRoster != null) ? new int?(memberRoster.TotalHealthyCount) : null);
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 0;
				}
				return num2.GetValueOrDefault();
			}) + Clan.PlayerClan.WarPartyComponents.Sum<WarPartyComponent>((WarPartyComponent t) => t.MobileParty.MemberRoster.TotalHealthyCount) < this.MinimumTroopCountToCreateKingdom)
			{
				flag = false;
				TextObject textObject4 = new TextObject("{=K2txLdOS}You should have at least {TROOP_COUNT} men ready to fight.", null);
				textObject4.SetTextVariable("TROOP_COUNT", this.MinimumTroopCountToCreateKingdom);
				explanations.Add(textObject4);
			}
			return flag;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00076180 File Offset: 0x00074380
		public override bool IsPlayerKingdomAbdicationPossible(out List<TextObject> explanations)
		{
			explanations = new List<TextObject>();
			object obj = Clan.PlayerClan.Kingdom != null && Clan.PlayerClan.Kingdom.RulingClan == Clan.PlayerClan;
			bool flag = MobileParty.MainParty.MapEvent != null || MobileParty.MainParty.SiegeEvent != null;
			object obj2 = obj;
			bool flag2 = obj2 != null && !Clan.PlayerClan.Kingdom.UnresolvedDecisions.IsEmpty<KingdomDecision>();
			if (obj2 == null)
			{
				explanations.Add(new TextObject("{=s1ERZ4ZR}You must be the king", null));
			}
			if (flag)
			{
				explanations.Add(new TextObject("{=uaMmmhRV}You must conclude your current encounter", null));
			}
			if (flag2)
			{
				explanations.Add(new TextObject("{=etKrpcHe}You must resolve pending decisions", null));
			}
			return obj2 != null && !flag && !flag2;
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x0007623E File Offset: 0x0007443E
		public override IEnumerable<CultureObject> GetAvailablePlayerKingdomCultures()
		{
			List<CultureObject> list = new List<CultureObject>();
			list.Add(Clan.PlayerClan.Culture);
			foreach (Settlement settlement in Clan.PlayerClan.Settlements.Where<Settlement>((Settlement t) => t.IsTown || t.IsCastle))
			{
				if (!list.Contains(settlement.Culture))
				{
					list.Add(settlement.Culture);
				}
			}
			foreach (CultureObject cultureObject in list)
			{
				yield return cultureObject;
			}
			List<CultureObject>.Enumerator enumerator2 = default(List<CultureObject>.Enumerator);
			yield break;
			yield break;
		}
	}
}
