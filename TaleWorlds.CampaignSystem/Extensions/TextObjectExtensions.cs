using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000170 RID: 368
	public static class TextObjectExtensions
	{
		// Token: 0x06001B3C RID: 6972 RVA: 0x0008D4FA File Offset: 0x0008B6FA
		public static void SetCharacterProperties(this TextObject to, string tag, CharacterObject character, bool includeDetails = false)
		{
			StringHelpers.SetCharacterProperties(tag, character, to, includeDetails);
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x0008D508 File Offset: 0x0008B708
		public static void SetSettlementProperties(this TextObject to, Settlement settlement)
		{
			to.SetTextVariable("IS_SETTLEMENT", 1);
			to.SetTextVariable("IS_CASTLE", settlement.IsCastle ? 1 : 0);
			to.SetTextVariable("IS_TOWN", settlement.IsTown ? 1 : 0);
			to.SetTextVariable("IS_HIDEOUT", settlement.IsHideout ? 1 : 0);
		}
	}
}
