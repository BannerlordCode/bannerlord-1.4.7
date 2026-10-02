using System;
using System.Runtime.InteropServices;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E8 RID: 488
	[EngineStruct("Blow_weapon_record", false, null)]
	public struct BlowWeaponRecord
	{
		// Token: 0x06001C74 RID: 7284 RVA: 0x000614B4 File Offset: 0x0005F6B4
		public void FillAsMeleeBlow(ItemObject item, WeaponComponentData weaponComponentData, int affectorWeaponSlot, sbyte weaponAttachBoneIndex)
		{
			this._isMissile = false;
			if (weaponComponentData != null)
			{
				this.ItemFlags = item.ItemFlags;
				this.WeaponFlags = weaponComponentData.WeaponFlags;
				this.WeaponClass = weaponComponentData.WeaponClass;
				this.BoneNoToAttach = weaponAttachBoneIndex;
				this.AffectorWeaponSlotOrMissileIndex = affectorWeaponSlot;
				this.Weight = item.Weight;
				this._isMaterialMetal = weaponComponentData.PhysicsMaterial.Contains("metal");
				return;
			}
			this._isMaterialMetal = false;
			this.AffectorWeaponSlotOrMissileIndex = -1;
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x00061530 File Offset: 0x0005F730
		public void FillAsMissileBlow(ItemObject item, WeaponComponentData weaponComponentData, int missileIndex, sbyte weaponAttachBoneIndex, Vec3 startingPosition, Vec3 currentPosition, Vec3 velocity)
		{
			this._isMissile = true;
			this.StartingPosition = startingPosition;
			this.CurrentPosition = currentPosition;
			this.Velocity = velocity;
			this.ItemFlags = item.ItemFlags;
			this.WeaponFlags = weaponComponentData.WeaponFlags;
			this.WeaponClass = weaponComponentData.WeaponClass;
			this.BoneNoToAttach = weaponAttachBoneIndex;
			this.AffectorWeaponSlotOrMissileIndex = missileIndex;
			this.Weight = item.Weight;
			this._isMaterialMetal = weaponComponentData.PhysicsMaterial.Contains("metal");
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000615B1 File Offset: 0x0005F7B1
		public bool HasWeapon()
		{
			return this.AffectorWeaponSlotOrMissileIndex >= 0;
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001C77 RID: 7287 RVA: 0x000615BF File Offset: 0x0005F7BF
		public bool IsMissile
		{
			get
			{
				return this._isMissile;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001C78 RID: 7288 RVA: 0x000615C7 File Offset: 0x0005F7C7
		public bool IsShield
		{
			get
			{
				return !this.WeaponFlags.HasAnyFlag(WeaponFlags.WeaponMask) && this.WeaponFlags.HasAllFlags(WeaponFlags.HasHitPoints | WeaponFlags.CanBlockRanged);
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001C79 RID: 7289 RVA: 0x000615EB File Offset: 0x0005F7EB
		public bool IsRanged
		{
			get
			{
				return this.WeaponFlags.HasAnyFlag(WeaponFlags.RangedWeapon);
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001C7A RID: 7290 RVA: 0x000615FA File Offset: 0x0005F7FA
		public bool IsAmmo
		{
			get
			{
				return !this.WeaponFlags.HasAnyFlag(WeaponFlags.WeaponMask) && this.WeaponFlags.HasAnyFlag(WeaponFlags.Consumable);
			}
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x00061620 File Offset: 0x0005F820
		public int GetHitSound(bool isOwnerHumanoid, bool isCriticalBlow, bool isLowBlow, bool isNonTipThrust, AgentAttackType attackType, DamageTypes damageType)
		{
			int num;
			if (this.HasWeapon())
			{
				if (this.IsRanged || this.IsAmmo)
				{
					switch (this.WeaponClass)
					{
					case WeaponClass.Sling:
					case WeaponClass.Stone:
					case WeaponClass.BallistaStone:
						if (isCriticalBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingStoneHigh;
						}
						if (isLowBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingStoneLow;
						}
						return CombatSoundContainer.SoundCodeMissionCombatThrowingStoneMed;
					case WeaponClass.Boulder:
					case WeaponClass.BallistaBoulder:
						if (isCriticalBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatBoulderHigh;
						}
						if (isLowBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatBoulderLow;
						}
						return CombatSoundContainer.SoundCodeMissionCombatBoulderMed;
					case WeaponClass.ThrowingAxe:
						if (isCriticalBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingAxeHigh;
						}
						if (isLowBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingAxeLow;
						}
						return CombatSoundContainer.SoundCodeMissionCombatThrowingAxeMed;
					case WeaponClass.ThrowingKnife:
						if (isCriticalBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingDaggerHigh;
						}
						if (isLowBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatThrowingDaggerLow;
						}
						return CombatSoundContainer.SoundCodeMissionCombatThrowingDaggerMed;
					case WeaponClass.Javelin:
						if (isCriticalBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatMissileHigh;
						}
						if (isLowBlow)
						{
							return CombatSoundContainer.SoundCodeMissionCombatMissileLow;
						}
						return CombatSoundContainer.SoundCodeMissionCombatMissileMed;
					}
					if (isCriticalBlow)
					{
						num = CombatSoundContainer.SoundCodeMissionCombatMissileHigh;
					}
					else if (isLowBlow)
					{
						num = CombatSoundContainer.SoundCodeMissionCombatMissileLow;
					}
					else
					{
						num = CombatSoundContainer.SoundCodeMissionCombatMissileMed;
					}
				}
				else if (this.IsShield)
				{
					if (this._isMaterialMetal)
					{
						num = CombatSoundContainer.SoundCodeMissionCombatMetalShieldBash;
					}
					else
					{
						num = CombatSoundContainer.SoundCodeMissionCombatWoodShieldBash;
					}
				}
				else if (attackType == AgentAttackType.Bash)
				{
					num = CombatSoundContainer.SoundCodeMissionCombatBluntLow;
				}
				else
				{
					if (isNonTipThrust)
					{
						damageType = DamageTypes.Blunt;
					}
					switch (damageType)
					{
					case DamageTypes.Cut:
						if (isCriticalBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatCutHigh;
						}
						else if (isLowBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatCutLow;
						}
						else
						{
							num = CombatSoundContainer.SoundCodeMissionCombatCutMed;
						}
						break;
					case DamageTypes.Pierce:
						if (isCriticalBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatPierceHigh;
						}
						else if (isLowBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatPierceLow;
						}
						else
						{
							num = CombatSoundContainer.SoundCodeMissionCombatPierceMed;
						}
						break;
					case DamageTypes.Blunt:
						if (isCriticalBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatBluntHigh;
						}
						else if (isLowBlow)
						{
							num = CombatSoundContainer.SoundCodeMissionCombatBluntLow;
						}
						else
						{
							num = CombatSoundContainer.SoundCodeMissionCombatBluntMed;
						}
						break;
					default:
						num = CombatSoundContainer.SoundCodeMissionCombatBluntMed;
						Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\BlowWeaponRecord.cs", "GetHitSound", 250);
						break;
					}
				}
			}
			else if (!isOwnerHumanoid)
			{
				num = CombatSoundContainer.SoundCodeMissionCombatChargeDamage;
			}
			else if (attackType == AgentAttackType.Kick)
			{
				num = CombatSoundContainer.SoundCodeMissionCombatKick;
			}
			else if (isCriticalBlow)
			{
				num = CombatSoundContainer.SoundCodeMissionCombatPunchHigh;
			}
			else if (isLowBlow)
			{
				num = CombatSoundContainer.SoundCodeMissionCombatPunchLow;
			}
			else
			{
				num = CombatSoundContainer.SoundCodeMissionCombatPunchMed;
			}
			return num;
		}

		// Token: 0x040009AB RID: 2475
		public Vec3 StartingPosition;

		// Token: 0x040009AC RID: 2476
		public Vec3 CurrentPosition;

		// Token: 0x040009AD RID: 2477
		public Vec3 Velocity;

		// Token: 0x040009AE RID: 2478
		public ItemFlags ItemFlags;

		// Token: 0x040009AF RID: 2479
		public WeaponFlags WeaponFlags;

		// Token: 0x040009B0 RID: 2480
		public WeaponClass WeaponClass;

		// Token: 0x040009B1 RID: 2481
		public sbyte BoneNoToAttach;

		// Token: 0x040009B2 RID: 2482
		public int AffectorWeaponSlotOrMissileIndex;

		// Token: 0x040009B3 RID: 2483
		public float Weight;

		// Token: 0x040009B4 RID: 2484
		[CustomEngineStructMemberData(true)]
		[MarshalAs(UnmanagedType.U1)]
		private bool _isMissile;

		// Token: 0x040009B5 RID: 2485
		[MarshalAs(UnmanagedType.U1)]
		private bool _isMaterialMetal;
	}
}
