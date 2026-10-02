using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x0200021C RID: 540
	public interface IInteractablePoint
	{
		// Token: 0x060020A4 RID: 8356
		CampaignVec2 GetInteractionPosition(MobileParty interactingParty);

		// Token: 0x060020A5 RID: 8357
		bool CanPartyInteract(MobileParty mobileParty, float dt);

		// Token: 0x060020A6 RID: 8358
		void OnPartyInteraction(MobileParty mobileParty);
	}
}
