using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003F4 RID: 1012
	public abstract class StrikeMagnitudeCalculationModel : MBGameModel<StrikeMagnitudeCalculationModel>
	{
		// Token: 0x06003748 RID: 14152
		public abstract float CalculateStrikeMagnitudeForMissile(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float missileSpeed);

		// Token: 0x06003749 RID: 14153
		public abstract float CalculateStrikeMagnitudeForSwing(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float swingSpeed, float impactPointAsPercent, float extraLinearSpeed);

		// Token: 0x0600374A RID: 14154
		public abstract float CalculateStrikeMagnitudeForThrust(in AttackInformation attackInformation, in AttackCollisionData collisionData, in MissionWeapon weapon, float thrustSpeed, float extraLinearSpeed, bool isThrown = false);

		// Token: 0x0600374B RID: 14155
		public abstract float CalculateBaseBlowMagnitudeForPassiveUsage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float extraLinearSpeed);

		// Token: 0x0600374C RID: 14156
		public abstract float ComputeRawDamage(DamageTypes damageType, float magnitude, float armorEffectiveness, float absorbedDamageRatio);

		// Token: 0x0600374D RID: 14157
		public abstract float CalculateStrikeMagnitudeForUnarmedAttack(in AttackInformation attackInformation, in AttackCollisionData collisionData, float progressEffect, float momentumRemaining);

		// Token: 0x0600374E RID: 14158
		public abstract float GetBluntDamageFactorByDamageType(DamageTypes damageType);

		// Token: 0x0600374F RID: 14159
		public abstract float CalculateHorseArcheryFactor(BasicCharacterObject characterObject);

		// Token: 0x06003750 RID: 14160 RVA: 0x000E4C92 File Offset: 0x000E2E92
		public virtual float CalculateAdjustedArmorForBlow(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseArmor, BasicCharacterObject attackerCharacter, BasicCharacterObject attackerCaptainCharacter, BasicCharacterObject victimCharacter, BasicCharacterObject victimCaptainCharacter, WeaponComponentData weaponComponent)
		{
			return baseArmor;
		}
	}
}
