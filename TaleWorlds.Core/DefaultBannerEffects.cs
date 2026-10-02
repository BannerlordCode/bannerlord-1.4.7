using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200004E RID: 78
	public class DefaultBannerEffects
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00015C54 File Offset: 0x00013E54
		private static DefaultBannerEffects Instance
		{
			get
			{
				return Game.Current.DefaultBannerEffects;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00015C60 File Offset: 0x00013E60
		public static BannerEffect IncreasedMeleeDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMeleeDamage;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00015C6C File Offset: 0x00013E6C
		public static BannerEffect IncreasedMeleeDamageAgainstMountedTroops
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMeleeDamageAgainstMountedTroops;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00015C78 File Offset: 0x00013E78
		public static BannerEffect IncreasedRangedDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedRangedDamage;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00015C84 File Offset: 0x00013E84
		public static BannerEffect IncreasedChargeDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedChargeDamage;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00015C90 File Offset: 0x00013E90
		public static BannerEffect DecreasedChargeDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedChargeDamage;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00015C9C File Offset: 0x00013E9C
		public static BannerEffect DecreasedRangedAccuracyPenalty
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedRangedAccuracyPenalty;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00015CA8 File Offset: 0x00013EA8
		public static BannerEffect DecreasedMoraleShock
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedMoraleShock;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00015CB4 File Offset: 0x00013EB4
		public static BannerEffect DecreasedMeleeAttackDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedMeleeAttackDamage;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00015CC0 File Offset: 0x00013EC0
		public static BannerEffect DecreasedRangedAttackDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedRangedAttackDamage;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00015CCC File Offset: 0x00013ECC
		public static BannerEffect DecreasedShieldDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedShieldDamage;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00015CD8 File Offset: 0x00013ED8
		public static BannerEffect IncreasedTroopMovementSpeed
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedTroopMovementSpeed;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00015CE4 File Offset: 0x00013EE4
		public static BannerEffect IncreasedMountMovementSpeed
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMountMovementSpeed;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00015CF0 File Offset: 0x00013EF0
		public static BannerEffect IncreasedMoraleShockByMeleeTroops
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMoraleShockByMeleeTroops;
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00015CFC File Offset: 0x00013EFC
		public DefaultBannerEffects()
		{
			this.RegisterAll();
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00015D0C File Offset: 0x00013F0C
		private void RegisterAll()
		{
			this._increasedMeleeDamage = this.Create("IncreasedMeleeDamage");
			this._increasedMeleeDamageAgainstMountedTroops = this.Create("IncreasedMeleeDamageAgainstMountedTroops");
			this._increasedRangedDamage = this.Create("IncreasedRangedDamage");
			this._increasedChargeDamage = this.Create("IncreasedChargeDamage");
			this._decreasedChargeDamage = this.Create("DecreasedChargeDamage");
			this._decreasedRangedAccuracyPenalty = this.Create("DecreasedRangedAccuracyPenalty");
			this._decreasedMoraleShock = this.Create("DecreasedMoraleShock");
			this._decreasedMeleeAttackDamage = this.Create("DecreasedMeleeAttackDamage");
			this._decreasedRangedAttackDamage = this.Create("DecreasedRangedAttackDamage");
			this._decreasedShieldDamage = this.Create("DecreasedShieldDamage");
			this._increasedTroopMovementSpeed = this.Create("IncreasedTroopMovementSpeed");
			this._increasedMountMovementSpeed = this.Create("IncreasedMountMovementSpeed");
			this._increasedMoraleShockByMeleeTroops = this.Create("IncreasedMoraleShockByMeleeTroops");
			this.InitializeAll();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00015DFC File Offset: 0x00013FFC
		private BannerEffect Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<BannerEffect>(new BannerEffect(stringId));
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00015E14 File Offset: 0x00014014
		private void InitializeAll()
		{
			this._increasedMeleeDamage.Initialize("{=unaWKloT}Increased Melee Damage", "{=8ZNOgT8Z}{BONUS_AMOUNT}% melee damage to troops in your formation.", 0.05f, 0.1f, 0.15f, EffectIncrementType.AddFactor);
			this._increasedMeleeDamageAgainstMountedTroops.Initialize("{=t0Qzb7CY}Increased Melee Damage Against Mounted Troops", "{=sxGmF0tC}{BONUS_AMOUNT}% melee damage by troops in your formation against cavalry.", 0.1f, 0.2f, 0.3f, EffectIncrementType.AddFactor);
			this._increasedRangedDamage.Initialize("{=Ch5NpCd0}Increased Ranged Damage", "{=labbKop6}{BONUS_AMOUNT}% ranged damage to troops in your formation.", 0.04f, 0.06f, 0.08f, EffectIncrementType.AddFactor);
			this._increasedChargeDamage.Initialize("{=O2oBC9sH}Increased Charge Damage", "{=Z2xgnrDa}{BONUS_AMOUNT}% charge damage to mounted troops in your formation.", 0.1f, 0.2f, 0.3f, EffectIncrementType.AddFactor);
			this._decreasedChargeDamage.Initialize("{=PkFT0D9a}Decreased Charge Damage", "{=Z2xgnrDa}{BONUS_AMOUNT}% charge damage to mounted troops in your formation.", -0.1f, -0.2f, -0.3f, EffectIncrementType.AddFactor);
			this._decreasedRangedAccuracyPenalty.Initialize("{=MkBPRCuF}Decreased Ranged Accuracy Penalty", "{=Gu0Wxxul}{BONUS_AMOUNT}% accuracy penalty for ranged troops in your formation.", -0.04f, -0.06f, -0.08f, EffectIncrementType.AddFactor);
			this._decreasedMoraleShock.Initialize("{=nOMT0Cw6}Decreased Morale Shock", "{=W0agPHes}{BONUS_AMOUNT}% morale penalty from casualties to troops in your formation.", -0.1f, -0.2f, -0.3f, EffectIncrementType.AddFactor);
			this._decreasedMeleeAttackDamage.Initialize("{=a3Vc59WV}Decreased Taken Melee Attack Damage", "{=ORFrCYSn}{BONUS_AMOUNT}% damage by melee attacks to troops in your formation.", -0.05f, -0.1f, -0.15f, EffectIncrementType.AddFactor);
			this._decreasedRangedAttackDamage.Initialize("{=p0JFbL7G}Decreased Taken Ranged Attack Damage", "{=W0agPHes}{BONUS_AMOUNT}% morale penalty from casualties to troops in your formation.", -0.05f, -0.1f, -0.15f, EffectIncrementType.AddFactor);
			this._decreasedShieldDamage.Initialize("{=T79exjaP}Decreased Taken Shield Damage", "{=klGEDUmw}{BONUS_AMOUNT}% damage to shields of troops in your formation.", -0.15f, -0.25f, -0.3f, EffectIncrementType.AddFactor);
			this._increasedTroopMovementSpeed.Initialize("{=PbJAOKKZ}Increased Troop Movement Speed", "{=nqWulUTP}{BONUS_AMOUNT}% movement speed to infantry in your formation.", 0.15f, 0.25f, 0.3f, EffectIncrementType.AddFactor);
			this._increasedMountMovementSpeed.Initialize("{=nMfxbc0Y}Increased Mount Movement Speed", "{=g0l7W5xQ}{BONUS_AMOUNT}% movement speed to mounts in your formation.", 0.05f, 0.08f, 0.1f, EffectIncrementType.AddFactor);
			this._increasedMoraleShockByMeleeTroops.Initialize("{=nOMT0Cw6}Increased Morale Shock", "{=!}INCREASED MORALE SHOCK BY MELEE TROOPS DESCRIPTION", 0.1f, 0.2f, 0.3f, EffectIncrementType.AddFactor);
		}

		// Token: 0x040002FE RID: 766
		private BannerEffect _increasedMeleeDamage;

		// Token: 0x040002FF RID: 767
		private BannerEffect _increasedMeleeDamageAgainstMountedTroops;

		// Token: 0x04000300 RID: 768
		private BannerEffect _increasedRangedDamage;

		// Token: 0x04000301 RID: 769
		private BannerEffect _increasedChargeDamage;

		// Token: 0x04000302 RID: 770
		private BannerEffect _decreasedChargeDamage;

		// Token: 0x04000303 RID: 771
		private BannerEffect _decreasedRangedAccuracyPenalty;

		// Token: 0x04000304 RID: 772
		private BannerEffect _decreasedMoraleShock;

		// Token: 0x04000305 RID: 773
		private BannerEffect _decreasedMeleeAttackDamage;

		// Token: 0x04000306 RID: 774
		private BannerEffect _decreasedRangedAttackDamage;

		// Token: 0x04000307 RID: 775
		private BannerEffect _decreasedShieldDamage;

		// Token: 0x04000308 RID: 776
		private BannerEffect _increasedTroopMovementSpeed;

		// Token: 0x04000309 RID: 777
		private BannerEffect _increasedMountMovementSpeed;

		// Token: 0x0400030A RID: 778
		private BannerEffect _increasedMoraleShockByMeleeTroops;
	}
}
