using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003FB RID: 1019
	public abstract class BattleMoraleModel : MBGameModel<BattleMoraleModel>
	{
		// Token: 0x06003786 RID: 14214
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public abstract ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentIncapacitated(Agent affectedAgent, AgentState affectedAgentState, Agent affectorAgent, in KillingBlow killingBlow);

		// Token: 0x06003787 RID: 14215
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public abstract ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentPanicked(Agent agent);

		// Token: 0x06003788 RID: 14216
		public abstract float CalculateMoraleChangeToCharacter(Agent agent, float maxMoraleChange);

		// Token: 0x06003789 RID: 14217
		public abstract float GetEffectiveInitialMorale(Agent agent, float baseMorale);

		// Token: 0x0600378A RID: 14218
		public abstract bool CanPanicDueToMorale(Agent agent);

		// Token: 0x0600378B RID: 14219
		public abstract float CalculateCasualtiesFactor(BattleSideEnum battleSide);

		// Token: 0x0600378C RID: 14220
		public abstract float GetAverageMorale(Formation formation);

		// Token: 0x0600378D RID: 14221
		public abstract float CalculateMoraleChangeOnShipSunk(IShipOrigin shipOrigin);

		// Token: 0x0600378E RID: 14222
		public abstract float CalculateMoraleOnRamming(Agent agent, IShipOrigin rammingShip, IShipOrigin rammedShip);

		// Token: 0x0600378F RID: 14223
		public abstract float CalculateMoraleOnShipsConnected(Agent agent, IShipOrigin ownerShip, IShipOrigin targetShip);

		// Token: 0x040017CB RID: 6091
		public const float BaseMoraleGainOnKill = 3f;

		// Token: 0x040017CC RID: 6092
		public const float BaseMoraleLossOnKill = 4f;

		// Token: 0x040017CD RID: 6093
		public const float BaseMoraleGainOnPanic = 2f;

		// Token: 0x040017CE RID: 6094
		public const float BaseMoraleLossOnPanic = 1.1f;

		// Token: 0x040017CF RID: 6095
		public const float MeleeWeaponMoraleMultiplier = 0.75f;

		// Token: 0x040017D0 RID: 6096
		public const float RangedWeaponMoraleMultiplier = 0.5f;

		// Token: 0x040017D1 RID: 6097
		public const float SiegeWeaponMoraleMultiplier = 0.25f;

		// Token: 0x040017D2 RID: 6098
		public const float BurningSiegeWeaponMoraleBonus = 0.25f;

		// Token: 0x040017D3 RID: 6099
		public const float CasualtyFactorRate = 2f;
	}
}
