using System;
using System.Runtime.InteropServices;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E6 RID: 486
	[EngineStruct("Blow", false, null)]
	public struct Blow
	{
		// Token: 0x06001C6F RID: 7279 RVA: 0x00061460 File Offset: 0x0005F660
		public Blow(int ownerId)
		{
			this = default(Blow);
			this.OwnerId = ownerId;
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x00061470 File Offset: 0x0005F670
		public bool IsMissile
		{
			get
			{
				return this.WeaponRecord.IsMissile;
			}
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x0006147D File Offset: 0x0005F67D
		public bool IsBlowCrit(int maxHitPointsOfVictim)
		{
			return (float)this.InflictedDamage > (float)maxHitPointsOfVictim * 0.5f;
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x00061490 File Offset: 0x0005F690
		public bool IsBlowLow(int maxHitPointsOfVictim)
		{
			return (float)this.InflictedDamage <= (float)maxHitPointsOfVictim * 0.1f;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000614A6 File Offset: 0x0005F6A6
		public bool IsHeadShot()
		{
			return this.VictimBodyPart == BoneBodyPartType.Head;
		}

		// Token: 0x0400098B RID: 2443
		public BlowWeaponRecord WeaponRecord;

		// Token: 0x0400098C RID: 2444
		public Vec3 GlobalPosition;

		// Token: 0x0400098D RID: 2445
		public Vec3 Direction;

		// Token: 0x0400098E RID: 2446
		public Vec3 SwingDirection;

		// Token: 0x0400098F RID: 2447
		public int InflictedDamage;

		// Token: 0x04000990 RID: 2448
		public int SelfInflictedDamage;

		// Token: 0x04000991 RID: 2449
		public float BaseMagnitude;

		// Token: 0x04000992 RID: 2450
		public float DefenderStunPeriod;

		// Token: 0x04000993 RID: 2451
		public float AttackerStunPeriod;

		// Token: 0x04000994 RID: 2452
		public float AbsorbedByArmor;

		// Token: 0x04000995 RID: 2453
		public float MovementSpeedDamageModifier;

		// Token: 0x04000996 RID: 2454
		public StrikeType StrikeType;

		// Token: 0x04000997 RID: 2455
		public AgentAttackType AttackType;

		// Token: 0x04000998 RID: 2456
		[CustomEngineStructMemberData("blow_flags")]
		public BlowFlags BlowFlag;

		// Token: 0x04000999 RID: 2457
		public int OwnerId;

		// Token: 0x0400099A RID: 2458
		public sbyte BoneIndex;

		// Token: 0x0400099B RID: 2459
		public BoneBodyPartType VictimBodyPart;

		// Token: 0x0400099C RID: 2460
		public DamageTypes DamageType;

		// Token: 0x0400099D RID: 2461
		[MarshalAs(UnmanagedType.U1)]
		public bool NoIgnore;

		// Token: 0x0400099E RID: 2462
		[MarshalAs(UnmanagedType.U1)]
		public bool DamageCalculated;

		// Token: 0x0400099F RID: 2463
		[MarshalAs(UnmanagedType.U1)]
		public bool IsFallDamage;

		// Token: 0x040009A0 RID: 2464
		public float DamagedPercentage;
	}
}
