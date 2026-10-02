using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace Helpers
{
	// Token: 0x0200000C RID: 12
	public static class CaravanHelper
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00005D04 File Offset: 0x00003F04
		public static PartyTemplateObject GetRandomCaravanTemplate(CultureObject culture, bool isElite, bool isLand)
		{
			PartyTemplateObject partyTemplateObject;
			if (isElite)
			{
				partyTemplateObject = culture.EliteCaravanPartyTemplates.GetRandomElementWithPredicate<PartyTemplateObject>((PartyTemplateObject x) => CaravanHelper.IsPartyTemplateSuitable(x, isLand));
			}
			else
			{
				partyTemplateObject = culture.CaravanPartyTemplates.GetRandomElementWithPredicate<PartyTemplateObject>((PartyTemplateObject x) => CaravanHelper.IsPartyTemplateSuitable(x, isLand));
			}
			return partyTemplateObject;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005D54 File Offset: 0x00003F54
		private static bool IsPartyTemplateSuitable(PartyTemplateObject template, bool isLand)
		{
			if (!isLand)
			{
				return template.ShipHulls.Count > 0;
			}
			return template.ShipHulls.Count == 0;
		}
	}
}
