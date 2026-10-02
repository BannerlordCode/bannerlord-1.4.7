using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace SandBox.GameComponents
{
	// Token: 0x020000C1 RID: 193
	public class SandboxAgentDecideKilledOrUnconsciousModel : AgentDecideKilledOrUnconsciousModel
	{
		// Token: 0x060007F8 RID: 2040 RVA: 0x00036460 File Offset: 0x00034660
		public override float GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out float useSurgeryProbability)
		{
			useSurgeryProbability = 1f;
			if (effectedAgent.IsHuman)
			{
				CharacterObject characterObject = (CharacterObject)effectedAgent.Character;
				if (Campaign.Current != null)
				{
					if (characterObject.IsHero && !characterObject.HeroObject.CanDie(KillCharacterAction.KillCharacterActionDetail.DiedInBattle))
					{
						return 0f;
					}
					CampaignAgentComponent component = effectedAgent.GetComponent<CampaignAgentComponent>();
					PartyBase partyBase = ((component != null) ? component.OwnerParty : null);
					if (affectorAgent != null && affectorAgent.IsHuman)
					{
						CampaignAgentComponent component2 = affectorAgent.GetComponent<CampaignAgentComponent>();
						PartyBase partyBase2 = ((component2 != null) ? component2.OwnerParty : null);
						return 1f - Campaign.Current.Models.PartyHealingModel.GetSurvivalChance(partyBase, characterObject, damageType, weaponFlags.HasAnyFlag(WeaponFlags.CanKillEvenIfBlunt), partyBase2);
					}
					return 1f - Campaign.Current.Models.PartyHealingModel.GetSurvivalChance(partyBase, characterObject, damageType, weaponFlags.HasAnyFlag(WeaponFlags.CanKillEvenIfBlunt), null);
				}
			}
			return 1f;
		}
	}
}
