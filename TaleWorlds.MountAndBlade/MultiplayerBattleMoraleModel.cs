using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000204 RID: 516
	public class MultiplayerBattleMoraleModel : BattleMoraleModel
	{
		// Token: 0x06001E03 RID: 7683 RVA: 0x00067915 File Offset: 0x00065B15
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public override ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentIncapacitated(Agent affectedAgent, AgentState affectedAgentState, Agent affectorAgent, in KillingBlow killingBlow)
		{
			return new ValueTuple<float, float>(0f, 0f);
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x00067926 File Offset: 0x00065B26
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public override ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentPanicked(Agent agent)
		{
			return new ValueTuple<float, float>(0f, 0f);
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x00067937 File Offset: 0x00065B37
		public override float CalculateMoraleChangeToCharacter(Agent agent, float maxMoraleChange)
		{
			return 0f;
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x0006793E File Offset: 0x00065B3E
		public override float GetEffectiveInitialMorale(Agent agent, float baseMorale)
		{
			return baseMorale;
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x00067941 File Offset: 0x00065B41
		public override bool CanPanicDueToMorale(Agent agent)
		{
			return true;
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x00067944 File Offset: 0x00065B44
		public override float CalculateCasualtiesFactor(BattleSideEnum battleSide)
		{
			return 1f;
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x0006794B File Offset: 0x00065B4B
		public override float GetAverageMorale(Formation formation)
		{
			return 0f;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x00067952 File Offset: 0x00065B52
		public override float CalculateMoraleChangeOnShipSunk(IShipOrigin shipOrigin)
		{
			return 0f;
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x00067959 File Offset: 0x00065B59
		public override float CalculateMoraleOnRamming(Agent agent, IShipOrigin rammingShip, IShipOrigin rammedShip)
		{
			return agent.GetMorale();
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x00067961 File Offset: 0x00065B61
		public override float CalculateMoraleOnShipsConnected(Agent agent, IShipOrigin ownerShip, IShipOrigin targetShip)
		{
			return agent.GetMorale();
		}
	}
}
