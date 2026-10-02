using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000121 RID: 289
	public class DefaultInventoryCapacityModel : InventoryCapacityModel
	{
		// Token: 0x06001848 RID: 6216 RVA: 0x00075771 File Offset: 0x00073971
		public override int GetItemAverageWeight()
		{
			return 10;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x00075775 File Offset: 0x00073975
		public override float GetItemEffectiveWeight(EquipmentElement equipmentElement, MobileParty mobileParty, bool isCurrentlyAtSea, out TextObject description)
		{
			if (equipmentElement.Item.HasHorseComponent)
			{
				description = DefaultInventoryCapacityModel._textMountsAndPackAnimals;
				return 0f;
			}
			description = DefaultInventoryCapacityModel._textItems;
			return equipmentElement.GetEquipmentElementWeight();
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x000757A4 File Offset: 0x000739A4
		public override ExplainedNumber CalculateInventoryCapacity(MobileParty mobileParty, bool isCurrentlyAtSea, bool includeDescriptions = false, int additionalTroops = 0, int additionalSpareMounts = 0, int additionalPackAnimals = 0, bool includeFollowers = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			PartyBase party = mobileParty.Party;
			int num = party.NumberOfMounts;
			int num2 = party.NumberOfHealthyMembers;
			int num3 = party.NumberOfPackAnimals;
			if (includeFollowers)
			{
				foreach (MobileParty mobileParty2 in mobileParty.AttachedParties)
				{
					num += mobileParty2.Party.NumberOfMounts;
					num2 += mobileParty2.Party.NumberOfHealthyMembers;
					num3 += mobileParty2.Party.NumberOfPackAnimals;
				}
			}
			if (mobileParty.HasPerk(DefaultPerks.Steward.ArenicosHorses, false) && !isCurrentlyAtSea)
			{
				num2 += MathF.Round((float)num2 * DefaultPerks.Steward.ArenicosHorses.PrimaryBonus);
			}
			if (!mobileParty.IsCurrentlyAtSea && mobileParty.HasPerk(DefaultPerks.Steward.ForcedLabor, false))
			{
				num2 += party.PrisonRoster.TotalHealthyCount;
			}
			explainedNumber.Add(10f, DefaultInventoryCapacityModel._textBase, null);
			explainedNumber.Add((float)num2 * 2f * 10f, DefaultInventoryCapacityModel._textTroops, null);
			if (!isCurrentlyAtSea)
			{
				explainedNumber.Add((float)num * 2f * 10f, DefaultInventoryCapacityModel._textSpareMounts, null);
				ExplainedNumber explainedNumber2 = new ExplainedNumber((float)num3 * 10f * 10f, false, null);
				if (mobileParty.HasPerk(DefaultPerks.Scouting.BeastWhisperer, true))
				{
					explainedNumber2.AddFactor(DefaultPerks.Scouting.BeastWhisperer.SecondaryBonus, DefaultPerks.Scouting.BeastWhisperer.Name);
				}
				if (mobileParty.HasPerk(DefaultPerks.Riding.DeeperSacks, false))
				{
					explainedNumber2.AddFactor(DefaultPerks.Riding.DeeperSacks.PrimaryBonus, DefaultPerks.Riding.DeeperSacks.Name);
				}
				if (mobileParty.HasPerk(DefaultPerks.Steward.ArenicosMules, false))
				{
					explainedNumber2.AddFactor(DefaultPerks.Steward.ArenicosMules.PrimaryBonus, DefaultPerks.Steward.ArenicosMules.Name);
				}
				explainedNumber.Add(explainedNumber2.ResultNumber, DefaultInventoryCapacityModel._textPackAnimals, null);
				if (mobileParty.HasPerk(DefaultPerks.Trade.CaravanMaster, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Trade.CaravanMaster.PrimaryBonus, DefaultPerks.Trade.CaravanMaster.Name);
				}
			}
			explainedNumber.LimitMin(10f);
			return explainedNumber;
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x000759C8 File Offset: 0x00073BC8
		public override ExplainedNumber CalculateTotalWeightCarried(MobileParty mobileParty, bool isCurrentlyAtSea, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, DefaultInventoryCapacityModel._textItems);
			InventoryCapacityModel inventoryCapacityModel = Campaign.Current.Models.InventoryCapacityModel;
			foreach (ItemRosterElement itemRosterElement in mobileParty.ItemRoster)
			{
				TextObject textObject;
				float itemEffectiveWeight = inventoryCapacityModel.GetItemEffectiveWeight(itemRosterElement.EquipmentElement, mobileParty, isCurrentlyAtSea, out textObject);
				explainedNumber.Add(itemEffectiveWeight * (float)itemRosterElement.Amount, textObject, null);
			}
			return explainedNumber;
		}

		// Token: 0x040007F5 RID: 2037
		private const int _itemAverageWeight = 10;

		// Token: 0x040007F6 RID: 2038
		private const float TroopsFactor = 2f;

		// Token: 0x040007F7 RID: 2039
		private const float SpareMountsFactor = 2f;

		// Token: 0x040007F8 RID: 2040
		private const float PackAnimalsFactor = 10f;

		// Token: 0x040007F9 RID: 2041
		private static readonly TextObject _textTroops = new TextObject("{=5k4dxUEJ}Troops", null);

		// Token: 0x040007FA RID: 2042
		private static readonly TextObject _textBase = new TextObject("{=basevalue}Base", null);

		// Token: 0x040007FB RID: 2043
		private static readonly TextObject _textSpareMounts = new TextObject("{=rCiKbsyW}Spare Mounts", null);

		// Token: 0x040007FC RID: 2044
		private static readonly TextObject _textPackAnimals = new TextObject("{=dI1AOyqh}Pack Animals", null);

		// Token: 0x040007FD RID: 2045
		private static readonly TextObject _textMountsAndPackAnimals = new TextObject("{=Sb1MKbvP}Mounts and Pack Animals", null);

		// Token: 0x040007FE RID: 2046
		private static readonly TextObject _textLiveStocksAnimals = new TextObject("{=KxUgSAKi}Live Stock Animals", null);

		// Token: 0x040007FF RID: 2047
		private static readonly TextObject _textItems = new TextObject("{=U7er3V9s}Items", null);
	}
}
