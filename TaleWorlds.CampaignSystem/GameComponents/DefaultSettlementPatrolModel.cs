using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200014F RID: 335
	public class DefaultSettlementPatrolModel : SettlementPatrolModel
	{
		// Token: 0x06001A27 RID: 6695 RVA: 0x00084480 File Offset: 0x00082680
		public override CampaignTime GetPatrolPartySpawnDuration(Settlement settlement, bool naval)
		{
			Building guardHouse = this.GetGuardHouse(settlement);
			return CampaignTime.Days(10f - ((float)guardHouse.CurrentLevel - 1f) * 2f);
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x000844B3 File Offset: 0x000826B3
		public override bool CanSettlementHavePatrolParties(Settlement settlement, bool naval)
		{
			return settlement.OwnerClan != null && !settlement.OwnerClan.IsRebelClan && settlement.IsTown && this.HasGuardHouse(settlement);
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x000844DB File Offset: 0x000826DB
		private bool HasGuardHouse(Settlement settlement)
		{
			return this.GetGuardHouse(settlement) != null;
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x000844E8 File Offset: 0x000826E8
		private Building GetGuardHouse(Settlement settlement)
		{
			if (settlement.Town != null)
			{
				foreach (Building building in settlement.Town.Buildings)
				{
					if (building.BuildingType == DefaultBuildingTypes.SettlementGuardHouse && building.CurrentLevel > 0)
					{
						return building;
					}
				}
			}
			return null;
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x00084560 File Offset: 0x00082760
		public override PartyTemplateObject GetPartyTemplateForPatrolParty(Settlement settlement, bool naval)
		{
			Building guardHouse = this.GetGuardHouse(settlement);
			if (guardHouse == null)
			{
				return null;
			}
			switch ((int)Campaign.Current.Models.BuildingEffectModel.GetBuildingEffect(guardHouse, BuildingEffectEnum.PatrolPartyStrength).ResultNumber)
			{
			case 1:
				return settlement.OwnerClan.Culture.SettlementPatrolPartyTemplateWeak;
			case 2:
				return settlement.OwnerClan.Culture.SettlementPatrolPartyTemplateModerate;
			case 3:
				return settlement.OwnerClan.Culture.SettlementPatrolPartyTemplateStrong;
			default:
				return settlement.OwnerClan.Culture.SettlementPatrolPartyTemplateWeak;
			}
		}
	}
}
