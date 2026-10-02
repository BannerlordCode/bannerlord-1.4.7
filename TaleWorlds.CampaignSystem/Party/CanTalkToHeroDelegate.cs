using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x020002FE RID: 766
	// (Invoke) Token: 0x06002C8D RID: 11405
	public delegate bool CanTalkToHeroDelegate(Hero hero, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty, out TextObject cantTalkReason);
}
