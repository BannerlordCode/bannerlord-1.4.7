using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000194 RID: 404
	[EngineStruct("Attack_collision_data", false, null)]
	public struct AttackCollisionData
	{
		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x0004F9BC File Offset: 0x0004DBBC
		public bool AttackBlockedWithShield
		{
			get
			{
				return this._attackBlockedWithShield;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001513 RID: 5395 RVA: 0x0004F9C4 File Offset: 0x0004DBC4
		public bool CorrectSideShieldBlock
		{
			get
			{
				return this._correctSideShieldBlock;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x0004F9CC File Offset: 0x0004DBCC
		public bool IsAlternativeAttack
		{
			get
			{
				return this._isAlternativeAttack;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001515 RID: 5397 RVA: 0x0004F9D4 File Offset: 0x0004DBD4
		public bool IsColliderAgent
		{
			get
			{
				return this._isColliderAgent;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x0004F9DC File Offset: 0x0004DBDC
		public bool CollidedWithShieldOnBack
		{
			get
			{
				return this._collidedWithShieldOnBack;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001517 RID: 5399 RVA: 0x0004F9E4 File Offset: 0x0004DBE4
		public bool IsMissile
		{
			get
			{
				return this._isMissile;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x0004F9EC File Offset: 0x0004DBEC
		public bool MissileBlockedWithWeapon
		{
			get
			{
				return this._missileBlockedWithWeapon;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001519 RID: 5401 RVA: 0x0004F9F4 File Offset: 0x0004DBF4
		public bool MissileHasPhysics
		{
			get
			{
				return this._missileHasPhysics;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600151A RID: 5402 RVA: 0x0004F9FC File Offset: 0x0004DBFC
		public bool EntityExists
		{
			get
			{
				return this._entityExists;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600151B RID: 5403 RVA: 0x0004FA04 File Offset: 0x0004DC04
		public bool ThrustTipHit
		{
			get
			{
				return this._thrustTipHit;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x0004FA0C File Offset: 0x0004DC0C
		public bool MissileGoneUnderWater
		{
			get
			{
				return this._missileGoneUnderWater;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600151D RID: 5405 RVA: 0x0004FA14 File Offset: 0x0004DC14
		public bool MissileGoneOutOfBorder
		{
			get
			{
				return this._missileGoneOutOfBorder;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x0600151E RID: 5406 RVA: 0x0004FA1C File Offset: 0x0004DC1C
		public bool CollidedWithLastBoneSegment
		{
			get
			{
				return this._collidedWithLastBoneSegment;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x0004FA24 File Offset: 0x0004DC24
		public bool IsHorseCharge
		{
			get
			{
				return this.ChargeVelocity > 0f;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x0004FA33 File Offset: 0x0004DC33
		public bool IsFallDamage
		{
			get
			{
				return this.FallSpeed > 0f;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x0004FA42 File Offset: 0x0004DC42
		public CombatCollisionResult CollisionResult
		{
			get
			{
				return (CombatCollisionResult)this._collisionResult;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001522 RID: 5410 RVA: 0x0004FA4A File Offset: 0x0004DC4A
		public int AffectorWeaponSlotOrMissileIndex { get; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001523 RID: 5411 RVA: 0x0004FA52 File Offset: 0x0004DC52
		public int StrikeType { get; }

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001524 RID: 5412 RVA: 0x0004FA5A File Offset: 0x0004DC5A
		public int DamageType { get; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x0004FA62 File Offset: 0x0004DC62
		// (set) Token: 0x06001526 RID: 5414 RVA: 0x0004FA6A File Offset: 0x0004DC6A
		public sbyte CollisionBoneIndex { get; private set; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0004FA73 File Offset: 0x0004DC73
		public BoneBodyPartType VictimHitBodyPart { get; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001528 RID: 5416 RVA: 0x0004FA7B File Offset: 0x0004DC7B
		// (set) Token: 0x06001529 RID: 5417 RVA: 0x0004FA83 File Offset: 0x0004DC83
		public sbyte AttackBoneIndex { get; private set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x0004FA8C File Offset: 0x0004DC8C
		public Agent.UsageDirection AttackDirection { get; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x0004FA94 File Offset: 0x0004DC94
		// (set) Token: 0x0600152C RID: 5420 RVA: 0x0004FA9C File Offset: 0x0004DC9C
		public int PhysicsMaterialIndex { get; private set; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x0004FAA5 File Offset: 0x0004DCA5
		// (set) Token: 0x0600152E RID: 5422 RVA: 0x0004FAAD File Offset: 0x0004DCAD
		public CombatHitResultFlags CollisionHitResultFlags { get; private set; }

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x0600152F RID: 5423 RVA: 0x0004FAB6 File Offset: 0x0004DCB6
		public float AttackProgress { get; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x0004FABE File Offset: 0x0004DCBE
		public float CollisionDistanceOnWeapon { get; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001531 RID: 5425 RVA: 0x0004FAC6 File Offset: 0x0004DCC6
		// (set) Token: 0x06001532 RID: 5426 RVA: 0x0004FACE File Offset: 0x0004DCCE
		public float AttackerStunPeriod { get; set; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x0004FAD7 File Offset: 0x0004DCD7
		// (set) Token: 0x06001534 RID: 5428 RVA: 0x0004FADF File Offset: 0x0004DCDF
		public float DefenderStunPeriod { get; set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x0004FAE8 File Offset: 0x0004DCE8
		public float MissileTotalDamage { get; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0004FAF0 File Offset: 0x0004DCF0
		public float MissileStartingBaseSpeed { get; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x0004FAF8 File Offset: 0x0004DCF8
		public float ChargeVelocity { get; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x0004FB00 File Offset: 0x0004DD00
		// (set) Token: 0x06001539 RID: 5433 RVA: 0x0004FB08 File Offset: 0x0004DD08
		public float FallSpeed { get; private set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x0004FB11 File Offset: 0x0004DD11
		public Vec3 WeaponRotUp { get; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600153B RID: 5435 RVA: 0x0004FB19 File Offset: 0x0004DD19
		public Vec3 WeaponBlowDir
		{
			get
			{
				return this._weaponBlowDir;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x0004FB21 File Offset: 0x0004DD21
		// (set) Token: 0x0600153D RID: 5437 RVA: 0x0004FB29 File Offset: 0x0004DD29
		public Vec3 CollisionGlobalPosition { get; private set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x0004FB32 File Offset: 0x0004DD32
		public Vec3 MissileVelocity { get; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x0600153F RID: 5439 RVA: 0x0004FB3A File Offset: 0x0004DD3A
		public Vec3 MissileStartingPosition { get; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x0004FB42 File Offset: 0x0004DD42
		public Vec3 VictimAgentCurVelocity { get; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x0004FB4A File Offset: 0x0004DD4A
		public Vec3 CollisionGlobalNormal { get; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x0004FB52 File Offset: 0x0004DD52
		public Vec3 LastBoneSegmentRotUp { get; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001543 RID: 5443 RVA: 0x0004FB5A File Offset: 0x0004DD5A
		public Vec3 LastBoneSegmentSwingDir { get; }

		// Token: 0x06001544 RID: 5444 RVA: 0x0004FB62 File Offset: 0x0004DD62
		public void SetCollisionBoneIndexForAreaDamage(sbyte boneIndex)
		{
			this.CollisionBoneIndex = boneIndex;
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0004FB6B File Offset: 0x0004DD6B
		public void UpdateCollisionPositionAndBoneForReflect(int inflictedDamage, Vec3 position, sbyte boneIndex)
		{
			this.InflictedDamage = inflictedDamage;
			this.CollisionGlobalPosition = position;
			this.AttackBoneIndex = boneIndex;
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0004FB84 File Offset: 0x0004DD84
		private AttackCollisionData(bool attackBlockedWithShield, bool correctSideShieldBlock, bool isAlternativeAttack, bool isColliderAgent, bool collidedWithShieldOnBack, bool isMissile, bool missileBlockedWithWeapon, bool missileHasPhysics, bool entityExists, bool thrustTipHit, bool missileGoneUnderWater, bool missileGoneOutOfBorder, bool collidedWithLastBoneSegment, CombatCollisionResult collisionResult, int affectorWeaponSlotOrMissileIndex, int StrikeType, int DamageType, sbyte CollisionBoneIndex, BoneBodyPartType VictimHitBodyPart, sbyte AttackBoneIndex, Agent.UsageDirection AttackDirection, int PhysicsMaterialIndex, CombatHitResultFlags CollisionHitResultFlags, float AttackProgress, float CollisionDistanceOnWeapon, float AttackerStunPeriod, float DefenderStunPeriod, float MissileTotalDamage, float MissileStartingBaseSpeed, float ChargeVelocity, float FallSpeed, Vec3 WeaponRotUp, Vec3 weaponBlowDir, Vec3 CollisionGlobalPosition, Vec3 MissileVelocity, Vec3 MissileStartingPosition, Vec3 VictimAgentCurVelocity, Vec3 GroundNormal, Vec3 LastBoneSegmentRotUp, Vec3 LastBoneSegmentSwingDir)
		{
			this._attackBlockedWithShield = attackBlockedWithShield;
			this._correctSideShieldBlock = correctSideShieldBlock;
			this._isAlternativeAttack = isAlternativeAttack;
			this._isColliderAgent = isColliderAgent;
			this._collidedWithShieldOnBack = collidedWithShieldOnBack;
			this._isMissile = isMissile;
			this._missileBlockedWithWeapon = missileBlockedWithWeapon;
			this._missileHasPhysics = missileHasPhysics;
			this._entityExists = entityExists;
			this._thrustTipHit = thrustTipHit;
			this._missileGoneUnderWater = missileGoneUnderWater;
			this._missileGoneOutOfBorder = missileGoneOutOfBorder;
			this._collidedWithLastBoneSegment = collidedWithLastBoneSegment;
			this._collisionResult = (int)collisionResult;
			this.AffectorWeaponSlotOrMissileIndex = affectorWeaponSlotOrMissileIndex;
			this.StrikeType = StrikeType;
			this.DamageType = DamageType;
			this.CollisionBoneIndex = CollisionBoneIndex;
			this.VictimHitBodyPart = VictimHitBodyPart;
			this.AttackBoneIndex = AttackBoneIndex;
			this.AttackDirection = AttackDirection;
			this.PhysicsMaterialIndex = PhysicsMaterialIndex;
			this.CollisionHitResultFlags = CollisionHitResultFlags;
			this.AttackProgress = AttackProgress;
			this.CollisionDistanceOnWeapon = CollisionDistanceOnWeapon;
			this.AttackerStunPeriod = AttackerStunPeriod;
			this.DefenderStunPeriod = DefenderStunPeriod;
			this.MissileTotalDamage = MissileTotalDamage;
			this.MissileStartingBaseSpeed = MissileStartingBaseSpeed;
			this.ChargeVelocity = ChargeVelocity;
			this.FallSpeed = FallSpeed;
			this.WeaponRotUp = WeaponRotUp;
			this._weaponBlowDir = weaponBlowDir;
			this.CollisionGlobalPosition = CollisionGlobalPosition;
			this.MissileVelocity = MissileVelocity;
			this.MissileStartingPosition = MissileStartingPosition;
			this.VictimAgentCurVelocity = VictimAgentCurVelocity;
			this.CollisionGlobalNormal = GroundNormal;
			this.LastBoneSegmentRotUp = LastBoneSegmentRotUp;
			this.LastBoneSegmentSwingDir = LastBoneSegmentSwingDir;
			this.BaseMagnitude = 0f;
			this.MovementSpeedDamageModifier = 0f;
			this.AbsorbedByArmor = 0;
			this.InflictedDamage = 0;
			this.SelfInflictedDamage = 0;
			this.IsShieldBroken = false;
			this.IsSneakAttack = false;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0004FD08 File Offset: 0x0004DF08
		public static AttackCollisionData GetAttackCollisionDataForDebugPurpose(bool _attackBlockedWithShield, bool _correctSideShieldBlock, bool _isAlternativeAttack, bool _isColliderAgent, bool _collidedWithShieldOnBack, bool _isMissile, bool _isMissileBlockedWithWeapon, bool _missileHasPhysics, bool _entityExists, bool _thrustTipHit, bool _missileGoneUnderWater, bool _missileGoneOutOfBorder, CombatCollisionResult collisionResult, int affectorWeaponSlotOrMissileIndex, int StrikeType, int DamageType, sbyte CollisionBoneIndex, BoneBodyPartType VictimHitBodyPart, sbyte AttackBoneIndex, Agent.UsageDirection AttackDirection, int PhysicsMaterialIndex, CombatHitResultFlags CollisionHitResultFlags, float AttackProgress, float CollisionDistanceOnWeapon, float AttackerStunPeriod, float DefenderStunPeriod, float MissileTotalDamage, float MissileInitialSpeed, float ChargeVelocity, float FallSpeed, Vec3 WeaponRotUp, Vec3 _weaponBlowDir, Vec3 CollisionGlobalPosition, Vec3 MissileVelocity, Vec3 MissileStartingPosition, Vec3 VictimAgentCurVelocity, Vec3 GroundNormal)
		{
			return new AttackCollisionData(_attackBlockedWithShield, _correctSideShieldBlock, _isAlternativeAttack, _isColliderAgent, _collidedWithShieldOnBack, _isMissile, _isMissileBlockedWithWeapon, _missileHasPhysics, _entityExists, _thrustTipHit, _missileGoneUnderWater, _missileGoneOutOfBorder, false, collisionResult, affectorWeaponSlotOrMissileIndex, StrikeType, DamageType, CollisionBoneIndex, VictimHitBodyPart, AttackBoneIndex, AttackDirection, PhysicsMaterialIndex, CollisionHitResultFlags, AttackProgress, CollisionDistanceOnWeapon, AttackerStunPeriod, DefenderStunPeriod, MissileTotalDamage, MissileInitialSpeed, ChargeVelocity, FallSpeed, WeaponRotUp, _weaponBlowDir, CollisionGlobalPosition, MissileVelocity, MissileStartingPosition, VictimAgentCurVelocity, GroundNormal, Vec3.Zero, Vec3.Zero);
		}

		// Token: 0x04000620 RID: 1568
		[MarshalAs(UnmanagedType.U1)]
		private bool _attackBlockedWithShield;

		// Token: 0x04000621 RID: 1569
		[MarshalAs(UnmanagedType.U1)]
		private bool _correctSideShieldBlock;

		// Token: 0x04000622 RID: 1570
		[MarshalAs(UnmanagedType.U1)]
		private bool _isAlternativeAttack;

		// Token: 0x04000623 RID: 1571
		[MarshalAs(UnmanagedType.U1)]
		private bool _isColliderAgent;

		// Token: 0x04000624 RID: 1572
		[MarshalAs(UnmanagedType.U1)]
		private bool _collidedWithShieldOnBack;

		// Token: 0x04000625 RID: 1573
		[MarshalAs(UnmanagedType.U1)]
		private bool _isMissile;

		// Token: 0x04000626 RID: 1574
		[MarshalAs(UnmanagedType.U1)]
		private bool _missileBlockedWithWeapon;

		// Token: 0x04000627 RID: 1575
		[MarshalAs(UnmanagedType.U1)]
		private bool _missileHasPhysics;

		// Token: 0x04000628 RID: 1576
		[MarshalAs(UnmanagedType.U1)]
		private bool _entityExists;

		// Token: 0x04000629 RID: 1577
		[MarshalAs(UnmanagedType.U1)]
		private bool _thrustTipHit;

		// Token: 0x0400062A RID: 1578
		[MarshalAs(UnmanagedType.U1)]
		private bool _missileGoneUnderWater;

		// Token: 0x0400062B RID: 1579
		[MarshalAs(UnmanagedType.U1)]
		private bool _missileGoneOutOfBorder;

		// Token: 0x0400062C RID: 1580
		[MarshalAs(UnmanagedType.U1)]
		private bool _collidedWithLastBoneSegment;

		// Token: 0x0400062D RID: 1581
		private int _collisionResult;

		// Token: 0x04000640 RID: 1600
		private Vec3 _weaponBlowDir;

		// Token: 0x04000648 RID: 1608
		[CustomEngineStructMemberData(true)]
		public float BaseMagnitude;

		// Token: 0x04000649 RID: 1609
		[CustomEngineStructMemberData(true)]
		public float MovementSpeedDamageModifier;

		// Token: 0x0400064A RID: 1610
		[CustomEngineStructMemberData(true)]
		public int AbsorbedByArmor;

		// Token: 0x0400064B RID: 1611
		[CustomEngineStructMemberData(true)]
		public int InflictedDamage;

		// Token: 0x0400064C RID: 1612
		[CustomEngineStructMemberData(true)]
		public int SelfInflictedDamage;

		// Token: 0x0400064D RID: 1613
		[CustomEngineStructMemberData(true)]
		[MarshalAs(UnmanagedType.U1)]
		public bool IsShieldBroken;

		// Token: 0x0400064E RID: 1614
		[CustomEngineStructMemberData(true)]
		[MarshalAs(UnmanagedType.U1)]
		public bool IsSneakAttack;
	}
}
