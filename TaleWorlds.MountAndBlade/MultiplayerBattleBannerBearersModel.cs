using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000202 RID: 514
	public class MultiplayerBattleBannerBearersModel : BattleBannerBearersModel
	{
		// Token: 0x06001DF6 RID: 7670 RVA: 0x000678D8 File Offset: 0x00065AD8
		public override int GetMinimumFormationTroopCountToBearBanners()
		{
			return int.MaxValue;
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000678DF File Offset: 0x00065ADF
		public override float GetBannerInteractionDistance(Agent interactingAgent)
		{
			return float.MaxValue;
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x000678E6 File Offset: 0x00065AE6
		public override bool CanAgentPickUpAnyBanner(Agent agent)
		{
			return false;
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x000678E9 File Offset: 0x00065AE9
		public override bool CanBannerBearerProvideEffectToFormation(Agent agent, Formation formation)
		{
			return false;
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x000678EC File Offset: 0x00065AEC
		public override bool CanAgentBecomeBannerBearer(Agent agent)
		{
			return false;
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x000678EF File Offset: 0x00065AEF
		public override int GetAgentBannerBearingPriority(Agent agent)
		{
			return 0;
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x000678F2 File Offset: 0x00065AF2
		public override bool CanFormationDeployBannerBearers(Formation formation)
		{
			return false;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x000678F5 File Offset: 0x00065AF5
		public override int GetDesiredNumberOfBannerBearersForFormation(Formation formation)
		{
			return 0;
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x000678F8 File Offset: 0x00065AF8
		public override ItemObject GetBannerBearerReplacementWeapon(BasicCharacterObject agentCharacter)
		{
			return null;
		}
	}
}
