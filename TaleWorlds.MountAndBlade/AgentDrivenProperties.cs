using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020000FF RID: 255
	public class AgentDrivenProperties
	{
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00016FD9 File Offset: 0x000151D9
		internal float[] Values
		{
			get
			{
				return this._statValues;
			}
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00016FE1 File Offset: 0x000151E1
		public AgentDrivenProperties()
		{
			this._statValues = new float[98];
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00016FF6 File Offset: 0x000151F6
		public float GetStat(DrivenProperty propertyEnum)
		{
			return this._statValues[(int)propertyEnum];
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00017000 File Offset: 0x00015200
		public void SetStat(DrivenProperty propertyEnum, float value)
		{
			this._statValues[(int)propertyEnum] = value;
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0001700B File Offset: 0x0001520B
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x00017015 File Offset: 0x00015215
		public float SwingSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.SwingSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.SwingSpeedMultiplier, value);
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00017020 File Offset: 0x00015220
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x0001702A File Offset: 0x0001522A
		public float ThrustOrRangedReadySpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.ThrustOrRangedReadySpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.ThrustOrRangedReadySpeedMultiplier, value);
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x00017035 File Offset: 0x00015235
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0001703F File Offset: 0x0001523F
		public float HandlingMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.HandlingMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.HandlingMultiplier, value);
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0001704A File Offset: 0x0001524A
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x00017054 File Offset: 0x00015254
		public float ReloadSpeed
		{
			get
			{
				return this.GetStat(DrivenProperty.ReloadSpeed);
			}
			set
			{
				this.SetStat(DrivenProperty.ReloadSpeed, value);
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0001705F File Offset: 0x0001525F
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x00017069 File Offset: 0x00015269
		public float MissileSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.MissileSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.MissileSpeedMultiplier, value);
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x00017074 File Offset: 0x00015274
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x0001707E File Offset: 0x0001527E
		public float WeaponInaccuracy
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponInaccuracy);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponInaccuracy, value);
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x00017089 File Offset: 0x00015289
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x00017093 File Offset: 0x00015293
		public float WeaponMaxMovementAccuracyPenalty
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponWorstMobileAccuracyPenalty);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponWorstMobileAccuracyPenalty, value);
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0001709E File Offset: 0x0001529E
		// (set) Token: 0x06000C49 RID: 3145 RVA: 0x000170A8 File Offset: 0x000152A8
		public float WeaponMaxUnsteadyAccuracyPenalty
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponWorstUnsteadyAccuracyPenalty);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponWorstUnsteadyAccuracyPenalty, value);
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x000170B3 File Offset: 0x000152B3
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x000170BD File Offset: 0x000152BD
		public float WeaponBestAccuracyWaitTime
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponBestAccuracyWaitTime);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponBestAccuracyWaitTime, value);
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x000170C8 File Offset: 0x000152C8
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x000170D2 File Offset: 0x000152D2
		public float WeaponUnsteadyBeginTime
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponUnsteadyBeginTime);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponUnsteadyBeginTime, value);
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x000170DD File Offset: 0x000152DD
		// (set) Token: 0x06000C4F RID: 3151 RVA: 0x000170E7 File Offset: 0x000152E7
		public float WeaponUnsteadyEndTime
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponUnsteadyEndTime);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponUnsteadyEndTime, value);
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x000170F2 File Offset: 0x000152F2
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x000170FC File Offset: 0x000152FC
		public float WeaponRotationalAccuracyPenaltyInRadians
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponRotationalAccuracyPenaltyInRadians);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponRotationalAccuracyPenaltyInRadians, value);
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x00017107 File Offset: 0x00015307
		// (set) Token: 0x06000C53 RID: 3155 RVA: 0x00017111 File Offset: 0x00015311
		public float WeaponExternalAccelerationAccuracyPenalty
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponExternalAccelerationAccuracyPenalty);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponExternalAccelerationAccuracyPenalty, value);
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x0001711C File Offset: 0x0001531C
		// (set) Token: 0x06000C55 RID: 3157 RVA: 0x00017126 File Offset: 0x00015326
		public float ArmorEncumbrance
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorEncumbrance);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorEncumbrance, value);
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x00017131 File Offset: 0x00015331
		// (set) Token: 0x06000C57 RID: 3159 RVA: 0x0001713B File Offset: 0x0001533B
		public float DamageMultiplierBonus
		{
			get
			{
				return this.GetStat(DrivenProperty.DamageMultiplierBonus);
			}
			set
			{
				this.SetStat(DrivenProperty.DamageMultiplierBonus, value);
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00017146 File Offset: 0x00015346
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x00017150 File Offset: 0x00015350
		public float ThrowingWeaponDamageMultiplierBonus
		{
			get
			{
				return this.GetStat(DrivenProperty.ThrowingWeaponDamageMultiplierBonus);
			}
			set
			{
				this.SetStat(DrivenProperty.ThrowingWeaponDamageMultiplierBonus, value);
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x0001715B File Offset: 0x0001535B
		// (set) Token: 0x06000C5B RID: 3163 RVA: 0x00017165 File Offset: 0x00015365
		public float MeleeWeaponDamageMultiplierBonus
		{
			get
			{
				return this.GetStat(DrivenProperty.MeleeWeaponDamageMultiplierBonus);
			}
			set
			{
				this.SetStat(DrivenProperty.MeleeWeaponDamageMultiplierBonus, value);
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x00017170 File Offset: 0x00015370
		// (set) Token: 0x06000C5D RID: 3165 RVA: 0x0001717A File Offset: 0x0001537A
		public float ArmorPenetrationMultiplierCrossbow
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorPenetrationMultiplierCrossbow);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorPenetrationMultiplierCrossbow, value);
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x00017185 File Offset: 0x00015385
		// (set) Token: 0x06000C5F RID: 3167 RVA: 0x0001718F File Offset: 0x0001538F
		public float ArmorPenetrationMultiplierBow
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorPenetrationMultiplierBow);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorPenetrationMultiplierBow, value);
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x0001719A File Offset: 0x0001539A
		// (set) Token: 0x06000C61 RID: 3169 RVA: 0x000171A4 File Offset: 0x000153A4
		public float WeaponsEncumbrance
		{
			get
			{
				return this.GetStat(DrivenProperty.WeaponsEncumbrance);
			}
			set
			{
				this.SetStat(DrivenProperty.WeaponsEncumbrance, value);
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x000171AF File Offset: 0x000153AF
		// (set) Token: 0x06000C63 RID: 3171 RVA: 0x000171B9 File Offset: 0x000153B9
		public float ArmorHead
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorHead);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorHead, value);
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x000171C4 File Offset: 0x000153C4
		// (set) Token: 0x06000C65 RID: 3173 RVA: 0x000171CE File Offset: 0x000153CE
		public float ArmorTorso
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorTorso);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorTorso, value);
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x000171D9 File Offset: 0x000153D9
		// (set) Token: 0x06000C67 RID: 3175 RVA: 0x000171E3 File Offset: 0x000153E3
		public float ArmorLegs
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorLegs);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorLegs, value);
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x000171EE File Offset: 0x000153EE
		// (set) Token: 0x06000C69 RID: 3177 RVA: 0x000171F8 File Offset: 0x000153F8
		public float ArmorArms
		{
			get
			{
				return this.GetStat(DrivenProperty.ArmorArms);
			}
			set
			{
				this.SetStat(DrivenProperty.ArmorArms, value);
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x00017203 File Offset: 0x00015403
		// (set) Token: 0x06000C6B RID: 3179 RVA: 0x0001720D File Offset: 0x0001540D
		public float AttributeRiding
		{
			get
			{
				return this.GetStat(DrivenProperty.AttributeRiding);
			}
			set
			{
				this.SetStat(DrivenProperty.AttributeRiding, value);
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00017218 File Offset: 0x00015418
		// (set) Token: 0x06000C6D RID: 3181 RVA: 0x00017222 File Offset: 0x00015422
		public float AttributeShield
		{
			get
			{
				return this.GetStat(DrivenProperty.AttributeShield);
			}
			set
			{
				this.SetStat(DrivenProperty.AttributeShield, value);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x0001722D File Offset: 0x0001542D
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x00017237 File Offset: 0x00015437
		public float AttributeShieldMissileCollisionBodySizeAdder
		{
			get
			{
				return this.GetStat(DrivenProperty.AttributeShieldMissileCollisionBodySizeAdder);
			}
			set
			{
				this.SetStat(DrivenProperty.AttributeShieldMissileCollisionBodySizeAdder, value);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00017242 File Offset: 0x00015442
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x0001724C File Offset: 0x0001544C
		public float ShieldBashStunDurationMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.ShieldBashStunDurationMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.ShieldBashStunDurationMultiplier, value);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00017257 File Offset: 0x00015457
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x00017261 File Offset: 0x00015461
		public float KickStunDurationMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.KickStunDurationMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.KickStunDurationMultiplier, value);
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x0001726C File Offset: 0x0001546C
		// (set) Token: 0x06000C75 RID: 3189 RVA: 0x00017276 File Offset: 0x00015476
		public float ReloadMovementPenaltyFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.ReloadMovementPenaltyFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.ReloadMovementPenaltyFactor, value);
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00017281 File Offset: 0x00015481
		// (set) Token: 0x06000C77 RID: 3191 RVA: 0x0001728B File Offset: 0x0001548B
		public float TopSpeedReachDuration
		{
			get
			{
				return this.GetStat(DrivenProperty.TopSpeedReachDuration);
			}
			set
			{
				this.SetStat(DrivenProperty.TopSpeedReachDuration, value);
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00017296 File Offset: 0x00015496
		// (set) Token: 0x06000C79 RID: 3193 RVA: 0x000172A0 File Offset: 0x000154A0
		public float MaxSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.MaxSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.MaxSpeedMultiplier, value);
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x000172AB File Offset: 0x000154AB
		// (set) Token: 0x06000C7B RID: 3195 RVA: 0x000172B5 File Offset: 0x000154B5
		public float CombatMaxSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.CombatMaxSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.CombatMaxSpeedMultiplier, value);
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x000172C0 File Offset: 0x000154C0
		// (set) Token: 0x06000C7D RID: 3197 RVA: 0x000172CA File Offset: 0x000154CA
		public float CrouchedSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.CrouchedSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.CrouchedSpeedMultiplier, value);
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x000172D5 File Offset: 0x000154D5
		// (set) Token: 0x06000C7F RID: 3199 RVA: 0x000172DF File Offset: 0x000154DF
		public float AttributeHorseArchery
		{
			get
			{
				return this.GetStat(DrivenProperty.AttributeHorseArchery);
			}
			set
			{
				this.SetStat(DrivenProperty.AttributeHorseArchery, value);
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x000172EA File Offset: 0x000154EA
		// (set) Token: 0x06000C81 RID: 3201 RVA: 0x000172F4 File Offset: 0x000154F4
		public float AttributeCourage
		{
			get
			{
				return this.GetStat(DrivenProperty.AttributeCourage);
			}
			set
			{
				this.SetStat(DrivenProperty.AttributeCourage, value);
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x000172FF File Offset: 0x000154FF
		// (set) Token: 0x06000C83 RID: 3203 RVA: 0x00017309 File Offset: 0x00015509
		public float MountManeuver
		{
			get
			{
				return this.GetStat(DrivenProperty.MountManeuver);
			}
			set
			{
				this.SetStat(DrivenProperty.MountManeuver, value);
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00017314 File Offset: 0x00015514
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x0001731E File Offset: 0x0001551E
		public float MountSpeed
		{
			get
			{
				return this.GetStat(DrivenProperty.MountSpeed);
			}
			set
			{
				this.SetStat(DrivenProperty.MountSpeed, value);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x00017329 File Offset: 0x00015529
		// (set) Token: 0x06000C87 RID: 3207 RVA: 0x00017333 File Offset: 0x00015533
		public float MountDashAccelerationMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.MountDashAccelerationMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.MountDashAccelerationMultiplier, value);
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x0001733E File Offset: 0x0001553E
		// (set) Token: 0x06000C89 RID: 3209 RVA: 0x00017348 File Offset: 0x00015548
		public float MountChargeDamage
		{
			get
			{
				return this.GetStat(DrivenProperty.MountChargeDamage);
			}
			set
			{
				this.SetStat(DrivenProperty.MountChargeDamage, value);
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x00017353 File Offset: 0x00015553
		// (set) Token: 0x06000C8B RID: 3211 RVA: 0x0001735D File Offset: 0x0001555D
		public float MountDifficulty
		{
			get
			{
				return this.GetStat(DrivenProperty.MountDifficulty);
			}
			set
			{
				this.SetStat(DrivenProperty.MountDifficulty, value);
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00017368 File Offset: 0x00015568
		// (set) Token: 0x06000C8D RID: 3213 RVA: 0x00017372 File Offset: 0x00015572
		public float BipedalRangedReadySpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.BipedalRangedReadySpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.BipedalRangedReadySpeedMultiplier, value);
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0001737D File Offset: 0x0001557D
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x00017387 File Offset: 0x00015587
		public float BipedalRangedReloadSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.BipedalRangedReloadSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.BipedalRangedReloadSpeedMultiplier, value);
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00017392 File Offset: 0x00015592
		// (set) Token: 0x06000C91 RID: 3217 RVA: 0x0001739C File Offset: 0x0001559C
		public float AiShooterErrorWoRangeUpdate
		{
			get
			{
				return this.GetStat(DrivenProperty.AiShooterErrorWoRangeUpdate);
			}
			set
			{
				this.SetStat(DrivenProperty.AiShooterErrorWoRangeUpdate, value);
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x000173A7 File Offset: 0x000155A7
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x000173B0 File Offset: 0x000155B0
		public float AiRangedHorsebackMissileRange
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRangedHorsebackMissileRange);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRangedHorsebackMissileRange, value);
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x000173BA File Offset: 0x000155BA
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x000173C3 File Offset: 0x000155C3
		public float AiFacingMissileWatch
		{
			get
			{
				return this.GetStat(DrivenProperty.AiFacingMissileWatch);
			}
			set
			{
				this.SetStat(DrivenProperty.AiFacingMissileWatch, value);
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x000173CD File Offset: 0x000155CD
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x000173D6 File Offset: 0x000155D6
		public float AiFlyingMissileCheckRadius
		{
			get
			{
				return this.GetStat(DrivenProperty.AiFlyingMissileCheckRadius);
			}
			set
			{
				this.SetStat(DrivenProperty.AiFlyingMissileCheckRadius, value);
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x000173E0 File Offset: 0x000155E0
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x000173E9 File Offset: 0x000155E9
		public float AiShootFreq
		{
			get
			{
				return this.GetStat(DrivenProperty.AiShootFreq);
			}
			set
			{
				this.SetStat(DrivenProperty.AiShootFreq, value);
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x000173F3 File Offset: 0x000155F3
		// (set) Token: 0x06000C9B RID: 3227 RVA: 0x000173FC File Offset: 0x000155FC
		public float AiWaitBeforeShootFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.AiWaitBeforeShootFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.AiWaitBeforeShootFactor, value);
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00017406 File Offset: 0x00015606
		// (set) Token: 0x06000C9D RID: 3229 RVA: 0x0001740F File Offset: 0x0001560F
		public float AIBlockOnDecideAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIBlockOnDecideAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIBlockOnDecideAbility, value);
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x00017419 File Offset: 0x00015619
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x00017422 File Offset: 0x00015622
		public float AIParryOnDecideAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIParryOnDecideAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIParryOnDecideAbility, value);
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0001742C File Offset: 0x0001562C
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x00017435 File Offset: 0x00015635
		public float AiTryChamberAttackOnDecide
		{
			get
			{
				return this.GetStat(DrivenProperty.AiTryChamberAttackOnDecide);
			}
			set
			{
				this.SetStat(DrivenProperty.AiTryChamberAttackOnDecide, value);
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0001743F File Offset: 0x0001563F
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x00017448 File Offset: 0x00015648
		public float AIAttackOnParryChance
		{
			get
			{
				return this.GetStat(DrivenProperty.AIAttackOnParryChance);
			}
			set
			{
				this.SetStat(DrivenProperty.AIAttackOnParryChance, value);
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x00017452 File Offset: 0x00015652
		// (set) Token: 0x06000CA5 RID: 3237 RVA: 0x0001745C File Offset: 0x0001565C
		public float AiAttackOnParryTiming
		{
			get
			{
				return this.GetStat(DrivenProperty.AiAttackOnParryTiming);
			}
			set
			{
				this.SetStat(DrivenProperty.AiAttackOnParryTiming, value);
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00017467 File Offset: 0x00015667
		// (set) Token: 0x06000CA7 RID: 3239 RVA: 0x00017471 File Offset: 0x00015671
		public float AIDecideOnAttackChance
		{
			get
			{
				return this.GetStat(DrivenProperty.AIDecideOnAttackChance);
			}
			set
			{
				this.SetStat(DrivenProperty.AIDecideOnAttackChance, value);
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0001747C File Offset: 0x0001567C
		// (set) Token: 0x06000CA9 RID: 3241 RVA: 0x00017486 File Offset: 0x00015686
		public float AIParryOnAttackAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIParryOnAttackAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIParryOnAttackAbility, value);
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00017491 File Offset: 0x00015691
		// (set) Token: 0x06000CAB RID: 3243 RVA: 0x0001749B File Offset: 0x0001569B
		public float AiKick
		{
			get
			{
				return this.GetStat(DrivenProperty.AiKick);
			}
			set
			{
				this.SetStat(DrivenProperty.AiKick, value);
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x000174A6 File Offset: 0x000156A6
		// (set) Token: 0x06000CAD RID: 3245 RVA: 0x000174B0 File Offset: 0x000156B0
		public float AiAttackCalculationMaxTimeFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.AiAttackCalculationMaxTimeFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.AiAttackCalculationMaxTimeFactor, value);
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x000174BB File Offset: 0x000156BB
		// (set) Token: 0x06000CAF RID: 3247 RVA: 0x000174C5 File Offset: 0x000156C5
		public float AiDecideOnAttackWhenReceiveHitTiming
		{
			get
			{
				return this.GetStat(DrivenProperty.AiDecideOnAttackWhenReceiveHitTiming);
			}
			set
			{
				this.SetStat(DrivenProperty.AiDecideOnAttackWhenReceiveHitTiming, value);
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x000174D0 File Offset: 0x000156D0
		// (set) Token: 0x06000CB1 RID: 3249 RVA: 0x000174DA File Offset: 0x000156DA
		public float AiDecideOnAttackContinueAction
		{
			get
			{
				return this.GetStat(DrivenProperty.AiDecideOnAttackContinueAction);
			}
			set
			{
				this.SetStat(DrivenProperty.AiDecideOnAttackContinueAction, value);
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x000174E5 File Offset: 0x000156E5
		// (set) Token: 0x06000CB3 RID: 3251 RVA: 0x000174EF File Offset: 0x000156EF
		public float AiDecideOnAttackingContinue
		{
			get
			{
				return this.GetStat(DrivenProperty.AiDecideOnAttackingContinue);
			}
			set
			{
				this.SetStat(DrivenProperty.AiDecideOnAttackingContinue, value);
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x000174FA File Offset: 0x000156FA
		// (set) Token: 0x06000CB5 RID: 3253 RVA: 0x00017504 File Offset: 0x00015704
		public float AIParryOnAttackingContinueAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIParryOnAttackingContinueAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIParryOnAttackingContinueAbility, value);
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0001750F File Offset: 0x0001570F
		// (set) Token: 0x06000CB7 RID: 3255 RVA: 0x00017519 File Offset: 0x00015719
		public float AIDecideOnRealizeEnemyBlockingAttackAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIDecideOnRealizeEnemyBlockingAttackAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIDecideOnRealizeEnemyBlockingAttackAbility, value);
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00017524 File Offset: 0x00015724
		// (set) Token: 0x06000CB9 RID: 3257 RVA: 0x0001752E File Offset: 0x0001572E
		public float AIRealizeBlockingFromIncorrectSideAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AIRealizeBlockingFromIncorrectSideAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AIRealizeBlockingFromIncorrectSideAbility, value);
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00017539 File Offset: 0x00015739
		// (set) Token: 0x06000CBB RID: 3259 RVA: 0x00017543 File Offset: 0x00015743
		public float AiAttackingShieldDefenseChance
		{
			get
			{
				return this.GetStat(DrivenProperty.AiAttackingShieldDefenseChance);
			}
			set
			{
				this.SetStat(DrivenProperty.AiAttackingShieldDefenseChance, value);
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x0001754E File Offset: 0x0001574E
		// (set) Token: 0x06000CBD RID: 3261 RVA: 0x00017558 File Offset: 0x00015758
		public float AiAttackingShieldDefenseTimer
		{
			get
			{
				return this.GetStat(DrivenProperty.AiAttackingShieldDefenseTimer);
			}
			set
			{
				this.SetStat(DrivenProperty.AiAttackingShieldDefenseTimer, value);
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00017563 File Offset: 0x00015763
		// (set) Token: 0x06000CBF RID: 3263 RVA: 0x0001756D File Offset: 0x0001576D
		public float AiCheckApplyMovementInterval
		{
			get
			{
				return this.GetStat(DrivenProperty.AiCheckApplyMovementInterval);
			}
			set
			{
				this.SetStat(DrivenProperty.AiCheckApplyMovementInterval, value);
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00017578 File Offset: 0x00015778
		// (set) Token: 0x06000CC1 RID: 3265 RVA: 0x00017582 File Offset: 0x00015782
		public float AiCheckCalculateMovementInterval
		{
			get
			{
				return this.GetStat(DrivenProperty.AiCheckCalculateMovementInterval);
			}
			set
			{
				this.SetStat(DrivenProperty.AiCheckCalculateMovementInterval, value);
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0001758D File Offset: 0x0001578D
		// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x00017597 File Offset: 0x00015797
		public float AiCheckDecideSimpleBehaviorInterval
		{
			get
			{
				return this.GetStat(DrivenProperty.AiCheckDecideSimpleBehaviorInterval);
			}
			set
			{
				this.SetStat(DrivenProperty.AiCheckDecideSimpleBehaviorInterval, value);
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x000175A2 File Offset: 0x000157A2
		// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x000175AC File Offset: 0x000157AC
		public float AiCheckDoSimpleBehaviorInterval
		{
			get
			{
				return this.GetStat(DrivenProperty.AiCheckDoSimpleBehaviorInterval);
			}
			set
			{
				this.SetStat(DrivenProperty.AiCheckDoSimpleBehaviorInterval, value);
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x000175B7 File Offset: 0x000157B7
		// (set) Token: 0x06000CC7 RID: 3271 RVA: 0x000175C1 File Offset: 0x000157C1
		public float AiMovementDelayFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.AiMovementDelayFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.AiMovementDelayFactor, value);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x000175CC File Offset: 0x000157CC
		// (set) Token: 0x06000CC9 RID: 3273 RVA: 0x000175D6 File Offset: 0x000157D6
		public float AiParryDecisionChangeValue
		{
			get
			{
				return this.GetStat(DrivenProperty.AiParryDecisionChangeValue);
			}
			set
			{
				this.SetStat(DrivenProperty.AiParryDecisionChangeValue, value);
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x000175E1 File Offset: 0x000157E1
		// (set) Token: 0x06000CCB RID: 3275 RVA: 0x000175EB File Offset: 0x000157EB
		public float AiDefendWithShieldDecisionChanceValue
		{
			get
			{
				return this.GetStat(DrivenProperty.AiDefendWithShieldDecisionChanceValue);
			}
			set
			{
				this.SetStat(DrivenProperty.AiDefendWithShieldDecisionChanceValue, value);
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x000175F6 File Offset: 0x000157F6
		// (set) Token: 0x06000CCD RID: 3277 RVA: 0x00017600 File Offset: 0x00015800
		public float AiMoveEnemySideTimeValue
		{
			get
			{
				return this.GetStat(DrivenProperty.AiMoveEnemySideTimeValue);
			}
			set
			{
				this.SetStat(DrivenProperty.AiMoveEnemySideTimeValue, value);
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0001760B File Offset: 0x0001580B
		// (set) Token: 0x06000CCF RID: 3279 RVA: 0x00017615 File Offset: 0x00015815
		public float AiMinimumDistanceToContinueFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.AiMinimumDistanceToContinueFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.AiMinimumDistanceToContinueFactor, value);
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00017620 File Offset: 0x00015820
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x0001762A File Offset: 0x0001582A
		public float AiChargeHorsebackTargetDistFactor
		{
			get
			{
				return this.GetStat(DrivenProperty.AiChargeHorsebackTargetDistFactor);
			}
			set
			{
				this.SetStat(DrivenProperty.AiChargeHorsebackTargetDistFactor, value);
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x00017635 File Offset: 0x00015835
		// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x0001763F File Offset: 0x0001583F
		public float AiRangerLeadErrorMin
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRangerLeadErrorMin);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRangerLeadErrorMin, value);
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x0001764A File Offset: 0x0001584A
		// (set) Token: 0x06000CD5 RID: 3285 RVA: 0x00017654 File Offset: 0x00015854
		public float AiRangerLeadErrorMax
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRangerLeadErrorMax);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRangerLeadErrorMax, value);
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0001765F File Offset: 0x0001585F
		// (set) Token: 0x06000CD7 RID: 3287 RVA: 0x00017669 File Offset: 0x00015869
		public float AiRangerVerticalErrorMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRangerVerticalErrorMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRangerVerticalErrorMultiplier, value);
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00017674 File Offset: 0x00015874
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x0001767E File Offset: 0x0001587E
		public float AiRangerHorizontalErrorMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRangerHorizontalErrorMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRangerHorizontalErrorMultiplier, value);
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00017689 File Offset: 0x00015889
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x00017693 File Offset: 0x00015893
		public float AIAttackOnDecideChance
		{
			get
			{
				return this.GetStat(DrivenProperty.AIAttackOnDecideChance);
			}
			set
			{
				this.SetStat(DrivenProperty.AIAttackOnDecideChance, value);
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0001769E File Offset: 0x0001589E
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x000176A8 File Offset: 0x000158A8
		public float AiRaiseShieldDelayTimeBase
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRaiseShieldDelayTimeBase);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRaiseShieldDelayTimeBase, value);
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x000176B3 File Offset: 0x000158B3
		// (set) Token: 0x06000CDF RID: 3295 RVA: 0x000176BD File Offset: 0x000158BD
		public float AiUseShieldAgainstEnemyMissileProbability
		{
			get
			{
				return this.GetStat(DrivenProperty.AiUseShieldAgainstEnemyMissileProbability);
			}
			set
			{
				this.SetStat(DrivenProperty.AiUseShieldAgainstEnemyMissileProbability, value);
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x000176C8 File Offset: 0x000158C8
		// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x000176D7 File Offset: 0x000158D7
		public int AiSpeciesIndex
		{
			get
			{
				return MathF.Round(this.GetStat(DrivenProperty.AiSpeciesIndex));
			}
			set
			{
				this.SetStat(DrivenProperty.AiSpeciesIndex, (float)value);
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x000176E3 File Offset: 0x000158E3
		// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x000176ED File Offset: 0x000158ED
		public float AiRandomizedDefendDirectionChance
		{
			get
			{
				return this.GetStat(DrivenProperty.AiRandomizedDefendDirectionChance);
			}
			set
			{
				this.SetStat(DrivenProperty.AiRandomizedDefendDirectionChance, value);
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x000176F8 File Offset: 0x000158F8
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x00017702 File Offset: 0x00015902
		public float AiShooterError
		{
			get
			{
				return this.GetStat(DrivenProperty.AiShooterError);
			}
			set
			{
				this.SetStat(DrivenProperty.AiShooterError, value);
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0001770D File Offset: 0x0001590D
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x00017717 File Offset: 0x00015917
		public float AiWeaponFavorMultiplierMelee
		{
			get
			{
				return this.GetStat(DrivenProperty.AiWeaponFavorMultiplierMelee);
			}
			set
			{
				this.SetStat(DrivenProperty.AiWeaponFavorMultiplierMelee, value);
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00017722 File Offset: 0x00015922
		// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x0001772C File Offset: 0x0001592C
		public float AiWeaponFavorMultiplierRanged
		{
			get
			{
				return this.GetStat(DrivenProperty.AiWeaponFavorMultiplierRanged);
			}
			set
			{
				this.SetStat(DrivenProperty.AiWeaponFavorMultiplierRanged, value);
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x00017737 File Offset: 0x00015937
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x00017741 File Offset: 0x00015941
		public float AiWeaponFavorMultiplierPolearm
		{
			get
			{
				return this.GetStat(DrivenProperty.AiWeaponFavorMultiplierPolearm);
			}
			set
			{
				this.SetStat(DrivenProperty.AiWeaponFavorMultiplierPolearm, value);
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0001774C File Offset: 0x0001594C
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x00017756 File Offset: 0x00015956
		public float AISetNoAttackTimerAfterBeingHitAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AISetNoAttackTimerAfterBeingHitAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AISetNoAttackTimerAfterBeingHitAbility, value);
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00017761 File Offset: 0x00015961
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x0001776B File Offset: 0x0001596B
		public float AISetNoAttackTimerAfterBeingParriedAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AISetNoAttackTimerAfterBeingParriedAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AISetNoAttackTimerAfterBeingParriedAbility, value);
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00017776 File Offset: 0x00015976
		// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x00017780 File Offset: 0x00015980
		public float AISetNoDefendTimerAfterHittingAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AISetNoDefendTimerAfterHittingAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AISetNoDefendTimerAfterHittingAbility, value);
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x0001778B File Offset: 0x0001598B
		// (set) Token: 0x06000CF3 RID: 3315 RVA: 0x00017795 File Offset: 0x00015995
		public float AISetNoDefendTimerAfterParryingAbility
		{
			get
			{
				return this.GetStat(DrivenProperty.AISetNoDefendTimerAfterParryingAbility);
			}
			set
			{
				this.SetStat(DrivenProperty.AISetNoDefendTimerAfterParryingAbility, value);
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x000177A0 File Offset: 0x000159A0
		// (set) Token: 0x06000CF5 RID: 3317 RVA: 0x000177AA File Offset: 0x000159AA
		public float AIEstimateStunDurationPrecision
		{
			get
			{
				return this.GetStat(DrivenProperty.AIEstimateStunDurationPrecision);
			}
			set
			{
				this.SetStat(DrivenProperty.AIEstimateStunDurationPrecision, value);
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x000177B5 File Offset: 0x000159B5
		// (set) Token: 0x06000CF7 RID: 3319 RVA: 0x000177BF File Offset: 0x000159BF
		public float AIHoldingReadyMaxDuration
		{
			get
			{
				return this.GetStat(DrivenProperty.AIHoldingReadyMaxDuration);
			}
			set
			{
				this.SetStat(DrivenProperty.AIHoldingReadyMaxDuration, value);
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x000177CA File Offset: 0x000159CA
		// (set) Token: 0x06000CF9 RID: 3321 RVA: 0x000177D4 File Offset: 0x000159D4
		public float AIHoldingReadyVariationPercentage
		{
			get
			{
				return this.GetStat(DrivenProperty.AIHoldingReadyVariationPercentage);
			}
			set
			{
				this.SetStat(DrivenProperty.AIHoldingReadyVariationPercentage, value);
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x000177DF File Offset: 0x000159DF
		// (set) Token: 0x06000CFB RID: 3323 RVA: 0x000177E9 File Offset: 0x000159E9
		public float OffhandWeaponDefendSpeedMultiplier
		{
			get
			{
				return this.GetStat(DrivenProperty.OffhandWeaponDefendSpeedMultiplier);
			}
			set
			{
				this.SetStat(DrivenProperty.OffhandWeaponDefendSpeedMultiplier, value);
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x000177F4 File Offset: 0x000159F4
		internal float[] InitializeDrivenProperties(Agent agent, Equipment spawnEquipment, AgentBuildData agentBuildData)
		{
			MissionGameModels.Current.AgentStatCalculateModel.InitializeAgentStats(agent, spawnEquipment, this, agentBuildData);
			MissionGameModels.Current.AgentStatCalculateModel.UpdateAgentStats(agent, this);
			return this._statValues;
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00017820 File Offset: 0x00015A20
		internal float[] UpdateDrivenProperties(Agent agent)
		{
			MissionGameModels.Current.AgentStatCalculateModel.UpdateAgentStats(agent, this);
			return this._statValues;
		}

		// Token: 0x040002C3 RID: 707
		private readonly float[] _statValues;
	}
}
