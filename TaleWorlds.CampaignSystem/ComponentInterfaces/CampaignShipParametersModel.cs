using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000201 RID: 513
	public abstract class CampaignShipParametersModel : MBGameModel<CampaignShipParametersModel>
	{
		// Token: 0x06001FAC RID: 8108
		public abstract float GetShipSizeWeatherFactor(ShipHull shipHull);

		// Token: 0x06001FAD RID: 8109
		public abstract float GetDefaultCombatFactor(ShipHull shipHull);

		// Token: 0x06001FAE RID: 8110
		public abstract float GetCampaignSpeedBonusFactor(Ship ship);

		// Token: 0x06001FAF RID: 8111
		public abstract float GetCrewCapacityBonusFactor(Ship ship);

		// Token: 0x06001FB0 RID: 8112
		public abstract float GetShipWeightFactor(Ship ship);

		// Token: 0x06001FB1 RID: 8113
		public abstract float GetForwardDragFactor(Ship ship);

		// Token: 0x06001FB2 RID: 8114
		public abstract float GetCrewShieldHitPointsFactor(Ship ship);

		// Token: 0x06001FB3 RID: 8115
		public abstract int GetAdditionalAmmoBonus(Ship ship);

		// Token: 0x06001FB4 RID: 8116
		public abstract float GetMaxOarPowerFactor(Ship ship);

		// Token: 0x06001FB5 RID: 8117
		public abstract float GetMaxOarForceFactor(Ship ship);

		// Token: 0x06001FB6 RID: 8118
		public abstract float GetSailForceFactor(Ship ship);

		// Token: 0x06001FB7 RID: 8119
		public abstract float GetCrewMeleeDamageFactor(Ship ship);

		// Token: 0x06001FB8 RID: 8120
		public abstract int GetAdditionalArcherQuivers(Ship ship);

		// Token: 0x06001FB9 RID: 8121
		public abstract int GetAdditionalThrowingWeaponStack(Ship ship);

		// Token: 0x06001FBA RID: 8122
		public abstract float GetSailRotationSpeedFactor(Ship ship);

		// Token: 0x06001FBB RID: 8123
		public abstract float GetFurlUnfurlSpeedFactor(Ship ship);
	}
}
