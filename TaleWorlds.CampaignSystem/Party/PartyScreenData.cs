using System;
using System.Collections;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x02000300 RID: 768
	public class PartyScreenData : IEnumerable<ValueTuple<TroopRosterElement, bool>>, IEnumerable
	{
		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06002D10 RID: 11536 RVA: 0x000BD836 File Offset: 0x000BBA36
		// (set) Token: 0x06002D11 RID: 11537 RVA: 0x000BD83E File Offset: 0x000BBA3E
		public PartyBase RightParty { get; private set; }

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06002D12 RID: 11538 RVA: 0x000BD847 File Offset: 0x000BBA47
		// (set) Token: 0x06002D13 RID: 11539 RVA: 0x000BD84F File Offset: 0x000BBA4F
		public PartyBase LeftParty { get; private set; }

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06002D14 RID: 11540 RVA: 0x000BD858 File Offset: 0x000BBA58
		// (set) Token: 0x06002D15 RID: 11541 RVA: 0x000BD860 File Offset: 0x000BBA60
		public Hero RightPartyLeaderHero { get; private set; }

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x000BD869 File Offset: 0x000BBA69
		// (set) Token: 0x06002D17 RID: 11543 RVA: 0x000BD871 File Offset: 0x000BBA71
		public Hero LeftPartyLeaderHero { get; private set; }

		// Token: 0x06002D18 RID: 11544 RVA: 0x000BD87A File Offset: 0x000BBA7A
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000BD884 File Offset: 0x000BBA84
		public PartyScreenData()
		{
			this.PartyGoldChangeAmount = 0;
			this.PartyInfluenceChangeAmount = new ValueTuple<int, int, int>(0, 0, 0);
			this.PartyMoraleChangeAmount = 0;
			this.PartyHorseChangeAmount = 0;
			this.RightRecruitableData = new Dictionary<CharacterObject, int>();
			this.UpgradedTroopsHistory = new List<Tuple<CharacterObject, CharacterObject, int>>();
			this.TransferredPrisonersHistory = new List<Tuple<CharacterObject, int>>();
			this.RecruitedPrisonersHistory = new List<Tuple<CharacterObject, int>>();
			this.UsedUpgradeHorsesHistory = new List<Tuple<EquipmentElement, int>>();
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000BD8F4 File Offset: 0x000BBAF4
		public void InitializeCopyFrom(PartyBase rightParty, PartyBase leftParty)
		{
			if (rightParty != null)
			{
				this.RightParty = rightParty;
				this.RightPartyLeaderHero = rightParty.LeaderHero;
			}
			if (leftParty != null)
			{
				this.LeftParty = leftParty;
				this.LeftPartyLeaderHero = leftParty.LeaderHero;
			}
			this.RightMemberRoster = TroopRoster.CreateDummyTroopRoster();
			this.LeftMemberRoster = TroopRoster.CreateDummyTroopRoster();
			this.RightPrisonerRoster = TroopRoster.CreateDummyTroopRoster();
			this.LeftPrisonerRoster = TroopRoster.CreateDummyTroopRoster();
			this.RightItemRoster = new ItemRoster();
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x000BD964 File Offset: 0x000BBB64
		public void CopyFromPartyAndRoster(TroopRoster rightPartyMemberRoster, TroopRoster rightPartyPrisonerRoster, TroopRoster leftPartyMemberRoster, TroopRoster leftPartyPrisonerRoster, PartyBase rightParty)
		{
			PrisonerRecruitmentCalculationModel prisonerRecruitmentCalculationModel = Campaign.Current.Models.PrisonerRecruitmentCalculationModel;
			for (int i = 0; i < rightPartyMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = rightPartyMemberRoster.GetElementCopyAtIndex(i);
				this.RightMemberRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, elementCopyAtIndex.Xp, true, -1);
			}
			for (int j = 0; j < leftPartyMemberRoster.Count; j++)
			{
				TroopRosterElement elementCopyAtIndex2 = leftPartyMemberRoster.GetElementCopyAtIndex(j);
				this.LeftMemberRoster.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, elementCopyAtIndex2.Xp, true, -1);
			}
			this.RightRecruitableData.Clear();
			for (int k = 0; k < rightPartyPrisonerRoster.Count; k++)
			{
				TroopRosterElement elementCopyAtIndex3 = rightPartyPrisonerRoster.GetElementCopyAtIndex(k);
				this.RightPrisonerRoster.AddToCounts(elementCopyAtIndex3.Character, elementCopyAtIndex3.Number, false, elementCopyAtIndex3.WoundedNumber, elementCopyAtIndex3.Xp, true, -1);
				if (rightParty != null)
				{
					MobileParty mobileParty = rightParty.MobileParty;
					bool? flag = ((mobileParty != null) ? new bool?(mobileParty.IsMainParty) : null);
					bool flag2 = true;
					if ((flag.GetValueOrDefault() == flag2) & (flag != null))
					{
						int num = prisonerRecruitmentCalculationModel.CalculateRecruitableNumber(PartyBase.MainParty, elementCopyAtIndex3.Character);
						if (!this.RightRecruitableData.ContainsKey(elementCopyAtIndex3.Character))
						{
							this.RightRecruitableData.Add(elementCopyAtIndex3.Character, num);
						}
					}
				}
			}
			for (int l = 0; l < leftPartyPrisonerRoster.Count; l++)
			{
				TroopRosterElement elementCopyAtIndex4 = leftPartyPrisonerRoster.GetElementCopyAtIndex(l);
				this.LeftPrisonerRoster.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, elementCopyAtIndex4.Xp, true, -1);
			}
			if (rightParty != null)
			{
				for (int m = 0; m < rightParty.ItemRoster.Count; m++)
				{
					ItemRosterElement elementCopyAtIndex5 = rightParty.ItemRoster.GetElementCopyAtIndex(m);
					this.RightItemRoster.AddToCounts(elementCopyAtIndex5.EquipmentElement, elementCopyAtIndex5.Amount);
				}
			}
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000BDB74 File Offset: 0x000BBD74
		public void CopyFromScreenData(PartyScreenData data)
		{
			this.RightMemberRoster.Clear();
			for (int i = 0; i < data.RightMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = data.RightMemberRoster.GetElementCopyAtIndex(i);
				this.RightMemberRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, elementCopyAtIndex.Xp, true, -1);
			}
			this.RightPrisonerRoster.Clear();
			for (int j = 0; j < data.RightPrisonerRoster.Count; j++)
			{
				TroopRosterElement elementCopyAtIndex2 = data.RightPrisonerRoster.GetElementCopyAtIndex(j);
				this.RightPrisonerRoster.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, elementCopyAtIndex2.Xp, true, -1);
			}
			this.RightItemRoster.Clear();
			if (data.RightItemRoster != null)
			{
				for (int k = 0; k < data.RightItemRoster.Count; k++)
				{
					ItemRosterElement elementCopyAtIndex3 = data.RightItemRoster.GetElementCopyAtIndex(k);
					this.RightItemRoster.AddToCounts(elementCopyAtIndex3.EquipmentElement, elementCopyAtIndex3.Amount);
				}
			}
			this.LeftMemberRoster.Clear();
			for (int l = 0; l < data.LeftMemberRoster.Count; l++)
			{
				TroopRosterElement elementCopyAtIndex4 = data.LeftMemberRoster.GetElementCopyAtIndex(l);
				this.LeftMemberRoster.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, elementCopyAtIndex4.Xp, true, -1);
			}
			this.LeftPrisonerRoster.Clear();
			for (int m = 0; m < data.LeftPrisonerRoster.Count; m++)
			{
				TroopRosterElement elementCopyAtIndex5 = data.LeftPrisonerRoster.GetElementCopyAtIndex(m);
				this.LeftPrisonerRoster.AddToCounts(elementCopyAtIndex5.Character, elementCopyAtIndex5.Number, false, elementCopyAtIndex5.WoundedNumber, elementCopyAtIndex5.Xp, true, -1);
			}
			this.PartyGoldChangeAmount = data.PartyGoldChangeAmount;
			this.PartyInfluenceChangeAmount = data.PartyInfluenceChangeAmount;
			this.PartyMoraleChangeAmount = data.PartyMoraleChangeAmount;
			this.PartyHorseChangeAmount = data.PartyHorseChangeAmount;
			this.RightRecruitableData = new Dictionary<CharacterObject, int>(data.RightRecruitableData);
			this.UpgradedTroopsHistory = new List<Tuple<CharacterObject, CharacterObject, int>>(data.UpgradedTroopsHistory);
			this.TransferredPrisonersHistory = new List<Tuple<CharacterObject, int>>(data.TransferredPrisonersHistory);
			this.RecruitedPrisonersHistory = new List<Tuple<CharacterObject, int>>(data.RecruitedPrisonersHistory);
			this.UsedUpgradeHorsesHistory = new List<Tuple<EquipmentElement, int>>(data.UsedUpgradeHorsesHistory);
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000BDDCC File Offset: 0x000BBFCC
		public void BindRostersFrom(TroopRoster rightPartyMemberRoster, TroopRoster rightPartyPrisonerRoster, TroopRoster leftPartyMemberRoster, TroopRoster leftPartyPrisonerRoster, PartyBase rightParty, PartyBase leftParty)
		{
			this.RightParty = rightParty;
			this.LeftParty = leftParty;
			if (rightParty != null)
			{
				this.RightItemRoster = rightParty.ItemRoster;
				this.RightPartyLeaderHero = rightParty.LeaderHero;
			}
			if (leftParty != null)
			{
				this.LeftPartyLeaderHero = leftParty.LeaderHero;
			}
			this.RightMemberRoster = rightPartyMemberRoster;
			this.LeftMemberRoster = leftPartyMemberRoster;
			this.RightPrisonerRoster = rightPartyPrisonerRoster;
			this.LeftPrisonerRoster = leftPartyPrisonerRoster;
			if (rightParty != null)
			{
				MobileParty mobileParty = rightParty.MobileParty;
				bool? flag = ((mobileParty != null) ? new bool?(mobileParty.IsMainParty) : null);
				bool flag2 = true;
				if ((flag.GetValueOrDefault() == flag2) & (flag != null))
				{
					this.RightRecruitableData = new Dictionary<CharacterObject, int>();
					PrisonerRecruitmentCalculationModel prisonerRecruitmentCalculationModel = Campaign.Current.Models.PrisonerRecruitmentCalculationModel;
					foreach (TroopRosterElement troopRosterElement in rightParty.PrisonRoster.GetTroopRoster())
					{
						int num = prisonerRecruitmentCalculationModel.CalculateRecruitableNumber(PartyBase.MainParty, troopRosterElement.Character);
						if (!this.RightRecruitableData.ContainsKey(troopRosterElement.Character))
						{
							this.RightRecruitableData.Add(troopRosterElement.Character, num);
						}
					}
				}
			}
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000BDF14 File Offset: 0x000BC114
		private List<Tuple<Hero, List<PartyRole>>> GetPartyHeroesWithPerks(TroopRoster roster)
		{
			MobileParty mobileParty;
			if (roster == null)
			{
				mobileParty = null;
			}
			else
			{
				PartyBase ownerParty = roster.OwnerParty;
				mobileParty = ((ownerParty != null) ? ownerParty.MobileParty : null);
			}
			MobileParty mobileParty2 = mobileParty;
			if (mobileParty2 == null)
			{
				return null;
			}
			List<Tuple<Hero, List<PartyRole>>> list = new List<Tuple<Hero, List<PartyRole>>>();
			for (int i = 0; i < roster.Count; i++)
			{
				CharacterObject characterAtIndex = roster.GetCharacterAtIndex(i);
				Hero hero = ((characterAtIndex != null) ? characterAtIndex.HeroObject : null);
				if (hero != null)
				{
					List<PartyRole> heroPartyRoles = mobileParty2.GetHeroPartyRoles(hero);
					if (heroPartyRoles.Count > 0)
					{
						list.Add(new Tuple<Hero, List<PartyRole>>(hero, heroPartyRoles));
					}
				}
			}
			return list;
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000BDF90 File Offset: 0x000BC190
		public void ResetUsing(PartyScreenData partyScreenData)
		{
			List<Tuple<Hero, List<PartyRole>>> partyHeroesWithPerks = this.GetPartyHeroesWithPerks(this.LeftMemberRoster);
			List<Tuple<Hero, List<PartyRole>>> partyHeroesWithPerks2 = this.GetPartyHeroesWithPerks(this.RightMemberRoster);
			this.RightMemberRoster.Clear();
			this.RightMemberRoster.RemoveZeroCounts();
			for (int i = 0; i < partyScreenData.RightMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = partyScreenData.RightMemberRoster.GetElementCopyAtIndex(i);
				this.RightMemberRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, elementCopyAtIndex.Xp, true, -1);
			}
			PartyBase rightParty = this.RightParty;
			if (((rightParty != null) ? rightParty.MobileParty : null) != null && this.RightParty.MobileParty.LeaderHero != partyScreenData.RightPartyLeaderHero)
			{
				this.RightParty.MobileParty.ChangePartyLeader(partyScreenData.RightPartyLeaderHero);
			}
			this.LeftMemberRoster.Clear();
			this.LeftMemberRoster.RemoveZeroCounts();
			for (int j = 0; j < partyScreenData.LeftMemberRoster.Count; j++)
			{
				TroopRosterElement elementCopyAtIndex2 = partyScreenData.LeftMemberRoster.GetElementCopyAtIndex(j);
				this.LeftMemberRoster.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, elementCopyAtIndex2.Xp, true, -1);
			}
			PartyBase leftParty = this.LeftParty;
			if (((leftParty != null) ? leftParty.MobileParty : null) != null && this.LeftParty.MobileParty.LeaderHero != partyScreenData.LeftPartyLeaderHero)
			{
				this.LeftParty.MobileParty.ChangePartyLeader(partyScreenData.LeftPartyLeaderHero);
			}
			this.RightPrisonerRoster.Clear();
			this.LeftPrisonerRoster.Clear();
			this.RightPrisonerRoster.RemoveZeroCounts();
			for (int k = 0; k < partyScreenData.RightPrisonerRoster.Count; k++)
			{
				TroopRosterElement elementCopyAtIndex3 = partyScreenData.RightPrisonerRoster.GetElementCopyAtIndex(k);
				this.RightPrisonerRoster.AddToCounts(elementCopyAtIndex3.Character, elementCopyAtIndex3.Number, false, elementCopyAtIndex3.WoundedNumber, elementCopyAtIndex3.Xp, true, -1);
			}
			this.LeftPrisonerRoster.RemoveZeroCounts();
			for (int l = 0; l < partyScreenData.LeftPrisonerRoster.Count; l++)
			{
				TroopRosterElement elementCopyAtIndex4 = partyScreenData.LeftPrisonerRoster.GetElementCopyAtIndex(l);
				this.LeftPrisonerRoster.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, elementCopyAtIndex4.Xp, true, -1);
			}
			if (this.RightItemRoster != null)
			{
				this.RightItemRoster.Clear();
				for (int m = 0; m < partyScreenData.RightItemRoster.Count; m++)
				{
					ItemRosterElement elementCopyAtIndex5 = partyScreenData.RightItemRoster.GetElementCopyAtIndex(m);
					this.RightItemRoster.AddToCounts(elementCopyAtIndex5.EquipmentElement, elementCopyAtIndex5.Amount);
				}
			}
			this.PartyGoldChangeAmount = partyScreenData.PartyGoldChangeAmount;
			this.PartyInfluenceChangeAmount = partyScreenData.PartyInfluenceChangeAmount;
			this.PartyMoraleChangeAmount = partyScreenData.PartyMoraleChangeAmount;
			this.PartyHorseChangeAmount = partyScreenData.PartyHorseChangeAmount;
			this.RightRecruitableData = new Dictionary<CharacterObject, int>(partyScreenData.RightRecruitableData);
			this.UpgradedTroopsHistory = new List<Tuple<CharacterObject, CharacterObject, int>>(partyScreenData.UpgradedTroopsHistory);
			this.TransferredPrisonersHistory = new List<Tuple<CharacterObject, int>>(partyScreenData.TransferredPrisonersHistory);
			this.RecruitedPrisonersHistory = new List<Tuple<CharacterObject, int>>(partyScreenData.RecruitedPrisonersHistory);
			this.UsedUpgradeHorsesHistory = new List<Tuple<EquipmentElement, int>>(partyScreenData.UsedUpgradeHorsesHistory);
			if (partyHeroesWithPerks != null)
			{
				PartyBase leftParty2 = this.LeftParty;
				if (((leftParty2 != null) ? leftParty2.MobileParty : null) != null)
				{
					for (int n = 0; n < partyHeroesWithPerks.Count; n++)
					{
						foreach (PartyRole partyRole in partyHeroesWithPerks[n].Item2)
						{
							this.LeftParty.MobileParty.SetHeroPartyRole(partyHeroesWithPerks[n].Item1, partyRole);
						}
					}
				}
			}
			if (partyHeroesWithPerks2 != null)
			{
				PartyBase rightParty2 = this.RightParty;
				if (((rightParty2 != null) ? rightParty2.MobileParty : null) != null)
				{
					for (int num = 0; num < partyHeroesWithPerks2.Count; num++)
					{
						foreach (PartyRole partyRole2 in partyHeroesWithPerks2[num].Item2)
						{
							this.RightParty.MobileParty.SetHeroPartyRole(partyHeroesWithPerks2[num].Item1, partyRole2);
						}
					}
				}
			}
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000BE3E0 File Offset: 0x000BC5E0
		public bool IsThereAnyTroopTradeDifferenceBetween(PartyScreenData other)
		{
			MBList<TroopRosterElement> troopRoster = this.RightMemberRoster.GetTroopRoster();
			MBList<TroopRosterElement> troopRoster2 = other.RightMemberRoster.GetTroopRoster();
			if (troopRoster.Count != troopRoster2.Count)
			{
				return true;
			}
			using (List<TroopRosterElement>.Enumerator enumerator = troopRoster.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TroopRosterElement elem2 = enumerator.Current;
					if (troopRoster2.FindIndex((TroopRosterElement x) => x.Character == elem2.Character && x.Number == elem2.Number) == -1)
					{
						return true;
					}
				}
			}
			MBList<TroopRosterElement> troopRoster3 = this.RightPrisonerRoster.GetTroopRoster();
			MBList<TroopRosterElement> troopRoster4 = other.RightPrisonerRoster.GetTroopRoster();
			if (troopRoster3.Count != troopRoster4.Count)
			{
				return true;
			}
			using (List<TroopRosterElement>.Enumerator enumerator = troopRoster3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TroopRosterElement elem = enumerator.Current;
					if (troopRoster4.FindIndex((TroopRosterElement x) => x.Character == elem.Character && x.Number == elem.Number) == -1)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000BE508 File Offset: 0x000BC708
		public List<TroopTradeDifference> GetTroopTradeDifferencesFromTo(PartyScreenData toPartyScreenData, PartyScreenLogic.PartyRosterSide side = PartyScreenLogic.PartyRosterSide.None)
		{
			List<TroopTradeDifference> list = new List<TroopTradeDifference>();
			string text = "Current settlement: ";
			Settlement currentSettlement = Settlement.CurrentSettlement;
			Debug.Print(text + ((currentSettlement != null) ? currentSettlement.StringId : null), 0, Debug.DebugColor.White, 17592186044416UL);
			string text2 = "Left party id: ";
			PartyBase leftParty = toPartyScreenData.LeftParty;
			string text3;
			if (leftParty == null)
			{
				text3 = null;
			}
			else
			{
				MobileParty mobileParty = leftParty.MobileParty;
				text3 = ((mobileParty != null) ? mobileParty.StringId : null);
			}
			Debug.Print(text2 + text3, 0, Debug.DebugColor.White, 17592186044416UL);
			string text4 = "Right party id: ";
			PartyBase rightParty = toPartyScreenData.RightParty;
			string text5;
			if (rightParty == null)
			{
				text5 = null;
			}
			else
			{
				MobileParty mobileParty2 = rightParty.MobileParty;
				text5 = ((mobileParty2 != null) ? mobileParty2.StringId : null);
			}
			Debug.Print(text4 + text5, 0, Debug.DebugColor.White, 17592186044416UL);
			if (side == PartyScreenLogic.PartyRosterSide.None || side == PartyScreenLogic.PartyRosterSide.Right)
			{
				foreach (ValueTuple<TroopRosterElement, bool> valueTuple in this)
				{
					TroopRosterElement troopRosterElement = valueTuple.Item1;
					int number = troopRosterElement.Number;
					int num = 0;
					foreach (ValueTuple<TroopRosterElement, bool> valueTuple2 in toPartyScreenData)
					{
						if (valueTuple2.Item1.Character == valueTuple.Item1.Character && valueTuple2.Item2 == valueTuple.Item2)
						{
							int num2 = num;
							troopRosterElement = valueTuple2.Item1;
							num = num2 + troopRosterElement.Number;
						}
					}
					if (number != num)
					{
						TroopTradeDifference troopTradeDifference = new TroopTradeDifference
						{
							Troop = valueTuple.Item1.Character,
							ToCount = num,
							FromCount = number,
							IsPrisoner = valueTuple.Item2
						};
						list.Add(troopTradeDifference);
					}
					Debug.Print(string.Concat(new object[]
					{
						"currently owned: ",
						number,
						", previously owned: ",
						num,
						" name: ",
						valueTuple.Item1.Character.StringId
					}), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				using (IEnumerator<ValueTuple<TroopRosterElement, bool>> enumerator = toPartyScreenData.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ValueTuple<TroopRosterElement, bool> valueTuple3 = enumerator.Current;
						TroopRosterElement troopRosterElement = valueTuple3.Item1;
						int number2 = troopRosterElement.Number;
						int num3 = 0;
						foreach (ValueTuple<TroopRosterElement, bool> valueTuple4 in this)
						{
							if (valueTuple3.Item1.Character == valueTuple4.Item1.Character && valueTuple3.Item2 == valueTuple4.Item2)
							{
								int num4 = num3;
								troopRosterElement = valueTuple3.Item1;
								num3 = num4 + troopRosterElement.Number;
							}
						}
						if (num3 != number2)
						{
							TroopTradeDifference troopTradeDifference2 = new TroopTradeDifference
							{
								Troop = valueTuple3.Item1.Character,
								ToCount = number2,
								FromCount = num3,
								IsPrisoner = valueTuple3.Item2
							};
							if (!list.Contains(troopTradeDifference2))
							{
								list.Add(troopTradeDifference2);
								Debug.Print(string.Concat(new object[]
								{
									"currently owned: ",
									num3,
									", previously owned: ",
									number2,
									" name: ",
									valueTuple3.Item1.Character.StringId
								}), 0, Debug.DebugColor.White, 17592186044416UL);
							}
						}
					}
					return list;
				}
			}
			foreach (ValueTuple<TroopRosterElement, bool> valueTuple5 in this.GetLeftSideElements())
			{
				TroopRosterElement troopRosterElement = valueTuple5.Item1;
				int number3 = troopRosterElement.Number;
				int num5 = 0;
				foreach (ValueTuple<TroopRosterElement, bool> valueTuple6 in toPartyScreenData.GetLeftSideElements())
				{
					if (valueTuple6.Item1.Character == valueTuple5.Item1.Character && valueTuple6.Item2 == valueTuple5.Item2)
					{
						int num6 = num5;
						troopRosterElement = valueTuple6.Item1;
						num5 = num6 + troopRosterElement.Number;
					}
				}
				if (number3 != num5)
				{
					TroopTradeDifference troopTradeDifference3 = new TroopTradeDifference
					{
						Troop = valueTuple5.Item1.Character,
						ToCount = num5,
						FromCount = number3,
						IsPrisoner = valueTuple5.Item2
					};
					list.Add(troopTradeDifference3);
				}
				Debug.Print(string.Concat(new object[]
				{
					"currently owned: ",
					number3,
					", previously owned: ",
					num5,
					" name: ",
					valueTuple5.Item1.Character.StringId
				}), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			foreach (ValueTuple<TroopRosterElement, bool> valueTuple7 in toPartyScreenData.GetLeftSideElements())
			{
				TroopRosterElement troopRosterElement = valueTuple7.Item1;
				int number4 = troopRosterElement.Number;
				int num7 = 0;
				foreach (ValueTuple<TroopRosterElement, bool> valueTuple8 in this.GetLeftSideElements())
				{
					if (valueTuple7.Item1.Character == valueTuple8.Item1.Character && valueTuple7.Item2 == valueTuple8.Item2)
					{
						int num8 = num7;
						troopRosterElement = valueTuple7.Item1;
						num7 = num8 + troopRosterElement.Number;
					}
				}
				if (num7 != number4)
				{
					TroopTradeDifference troopTradeDifference4 = new TroopTradeDifference
					{
						Troop = valueTuple7.Item1.Character,
						ToCount = number4,
						FromCount = num7,
						IsPrisoner = valueTuple7.Item2
					};
					if (!list.Contains(troopTradeDifference4))
					{
						list.Add(troopTradeDifference4);
						Debug.Print(string.Concat(new object[]
						{
							"currently owned: ",
							num7,
							", previously owned: ",
							number4,
							" name: ",
							valueTuple7.Item1.Character.StringId
						}), 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
			}
			return list;
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000BEC0C File Offset: 0x000BCE0C
		private List<ValueTuple<TroopRosterElement, bool>> GetLeftSideElements()
		{
			List<ValueTuple<TroopRosterElement, bool>> list = new List<ValueTuple<TroopRosterElement, bool>>();
			for (int i = 0; i < this.LeftMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = this.LeftMemberRoster.GetElementCopyAtIndex(i);
				list.Add(new ValueTuple<TroopRosterElement, bool>(elementCopyAtIndex, false));
			}
			for (int j = 0; j < this.LeftPrisonerRoster.Count; j++)
			{
				TroopRosterElement elementCopyAtIndex2 = this.LeftPrisonerRoster.GetElementCopyAtIndex(j);
				list.Add(new ValueTuple<TroopRosterElement, bool>(elementCopyAtIndex2, true));
			}
			return list;
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x000BEC82 File Offset: 0x000BCE82
		private IEnumerator<ValueTuple<TroopRosterElement, bool>> EnumerateElements()
		{
			int num;
			for (int i = 0; i < this.RightMemberRoster.Count; i = num + 1)
			{
				TroopRosterElement elementCopyAtIndex = this.RightMemberRoster.GetElementCopyAtIndex(i);
				yield return new ValueTuple<TroopRosterElement, bool>(elementCopyAtIndex, false);
				num = i;
			}
			for (int i = 0; i < this.RightPrisonerRoster.Count; i = num + 1)
			{
				TroopRosterElement elementCopyAtIndex2 = this.RightPrisonerRoster.GetElementCopyAtIndex(i);
				yield return new ValueTuple<TroopRosterElement, bool>(elementCopyAtIndex2, true);
				num = i;
			}
			yield break;
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x000BEC91 File Offset: 0x000BCE91
		public IEnumerator<ValueTuple<TroopRosterElement, bool>> GetEnumerator()
		{
			return this.EnumerateElements();
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x000BEC99 File Offset: 0x000BCE99
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnumerateElements();
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000BECA1 File Offset: 0x000BCEA1
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x000BECA8 File Offset: 0x000BCEA8
		public static bool operator ==(PartyScreenData a, PartyScreenData b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.PartyGoldChangeAmount != b.PartyGoldChangeAmount || a.PartyInfluenceChangeAmount.Item1 != b.PartyInfluenceChangeAmount.Item1 || a.PartyInfluenceChangeAmount.Item2 != b.PartyInfluenceChangeAmount.Item2 || a.PartyInfluenceChangeAmount.Item3 != b.PartyInfluenceChangeAmount.Item3 || a.PartyMoraleChangeAmount != b.PartyMoraleChangeAmount || a.PartyHorseChangeAmount != b.PartyHorseChangeAmount)
			{
				return false;
			}
			if (a.RightMemberRoster.Count != b.RightMemberRoster.Count || a.RightPrisonerRoster.Count != b.RightPrisonerRoster.Count || a.RightRecruitableData.Count != b.RightRecruitableData.Count || a.UpgradedTroopsHistory.Count != b.UpgradedTroopsHistory.Count || a.TransferredPrisonersHistory.Count != b.TransferredPrisonersHistory.Count || a.RecruitedPrisonersHistory.Count != b.RecruitedPrisonersHistory.Count || a.UsedUpgradeHorsesHistory.Count != b.UsedUpgradeHorsesHistory.Count)
			{
				return false;
			}
			if (!TroopRoster.RostersAreIdentical(a.RightMemberRoster, b.RightMemberRoster))
			{
				return false;
			}
			if (!TroopRoster.RostersAreIdentical(a.RightPrisonerRoster, b.LeftPrisonerRoster))
			{
				return false;
			}
			foreach (CharacterObject characterObject in a.RightRecruitableData.Keys)
			{
				if (!b.RightRecruitableData.ContainsKey(characterObject) || a.RightRecruitableData[characterObject] != b.RightRecruitableData[characterObject])
				{
					return false;
				}
			}
			for (int i = 0; i < a.UpgradedTroopsHistory.Count; i++)
			{
				if (a.UpgradedTroopsHistory[i].Item1 != b.UpgradedTroopsHistory[i].Item1 || a.UpgradedTroopsHistory[i].Item2 != b.UpgradedTroopsHistory[i].Item2 || a.UpgradedTroopsHistory[i].Item3 != b.UpgradedTroopsHistory[i].Item3)
				{
					return false;
				}
			}
			for (int j = 0; j < a.TransferredPrisonersHistory.Count; j++)
			{
				if (a.TransferredPrisonersHistory[j].Item1 != b.TransferredPrisonersHistory[j].Item1 || a.TransferredPrisonersHistory[j].Item2 != b.TransferredPrisonersHistory[j].Item2)
				{
					return false;
				}
			}
			for (int k = 0; k < a.RecruitedPrisonersHistory.Count; k++)
			{
				if (a.RecruitedPrisonersHistory[k].Item1 != b.RecruitedPrisonersHistory[k].Item1 || a.RecruitedPrisonersHistory[k].Item2 != b.RecruitedPrisonersHistory[k].Item2)
				{
					return false;
				}
			}
			for (int l = 0; l < a.UsedUpgradeHorsesHistory.Count; l++)
			{
				if (a.UsedUpgradeHorsesHistory[l].Item1.Item != b.UsedUpgradeHorsesHistory[l].Item1.Item || a.UsedUpgradeHorsesHistory[l].Item2 != b.UsedUpgradeHorsesHistory[l].Item2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x000BF054 File Offset: 0x000BD254
		public static bool operator !=(PartyScreenData first, PartyScreenData second)
		{
			return !(first == second);
		}

		// Token: 0x04000D10 RID: 3344
		public TroopRoster RightMemberRoster;

		// Token: 0x04000D11 RID: 3345
		public TroopRoster LeftMemberRoster;

		// Token: 0x04000D12 RID: 3346
		public TroopRoster RightPrisonerRoster;

		// Token: 0x04000D13 RID: 3347
		public TroopRoster LeftPrisonerRoster;

		// Token: 0x04000D14 RID: 3348
		public ItemRoster RightItemRoster;

		// Token: 0x04000D15 RID: 3349
		public Dictionary<CharacterObject, int> RightRecruitableData;

		// Token: 0x04000D16 RID: 3350
		public int PartyGoldChangeAmount;

		// Token: 0x04000D17 RID: 3351
		public ValueTuple<int, int, int> PartyInfluenceChangeAmount;

		// Token: 0x04000D18 RID: 3352
		public int PartyMoraleChangeAmount;

		// Token: 0x04000D19 RID: 3353
		public int PartyHorseChangeAmount;

		// Token: 0x04000D1A RID: 3354
		public List<Tuple<CharacterObject, CharacterObject, int>> UpgradedTroopsHistory;

		// Token: 0x04000D1B RID: 3355
		public List<Tuple<CharacterObject, int>> TransferredPrisonersHistory;

		// Token: 0x04000D1C RID: 3356
		public List<Tuple<CharacterObject, int>> RecruitedPrisonersHistory;

		// Token: 0x04000D1D RID: 3357
		public List<Tuple<EquipmentElement, int>> UsedUpgradeHorsesHistory;
	}
}
