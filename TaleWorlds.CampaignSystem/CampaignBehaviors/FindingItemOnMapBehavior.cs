using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F0 RID: 1008
	public class FindingItemOnMapBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003F62 RID: 16226 RVA: 0x0011E39F File Offset: 0x0011C59F
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x0011E3B8 File Offset: 0x0011C5B8
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x0011E3BC File Offset: 0x0011C5BC
		public void DailyTickParty(MobileParty party)
		{
			if (MBRandom.RandomFloat < DefaultPerks.Scouting.BeastWhisperer.PrimaryBonus && party.HasPerk(DefaultPerks.Scouting.BeastWhisperer, false))
			{
				TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace);
				if (faceTerrainType == TerrainType.Steppe || faceTerrainType == TerrainType.Plain)
				{
					ItemObject randomElementWithPredicate = Items.All.GetRandomElementWithPredicate<ItemObject>((ItemObject x) => x.IsMountable && !x.NotMerchandise);
					if (randomElementWithPredicate != null)
					{
						party.ItemRoster.AddToCounts(randomElementWithPredicate, 1);
						if (party.IsMainParty)
						{
							TextObject textObject = new TextObject("{=vl9bawa7}{COUNT} {?(COUNT > 1)}{PLURAL(ANIMAL_NAME)} are{?}{ANIMAL_NAME} is{\\?} added to your party.", null);
							textObject.SetTextVariable("COUNT", 1);
							textObject.SetTextVariable("ANIMAL_NAME", randomElementWithPredicate.Name);
							InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
						}
					}
				}
			}
		}
	}
}
