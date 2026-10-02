using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000300 RID: 768
	public static class CompressionMission
	{
		// Token: 0x040010FB RID: 4347
		public static CompressionInfo.Float DebugScaleValueCompressionInfo = new CompressionInfo.Float(0.5f, 1.5f, 13);

		// Token: 0x040010FC RID: 4348
		public static CompressionInfo.Integer AgentCompressionInfo = new CompressionInfo.Integer(-1, 11);

		// Token: 0x040010FD RID: 4349
		public static CompressionInfo.Integer WeaponAttachmentIndexCompressionInfo = new CompressionInfo.Integer(0, 8);

		// Token: 0x040010FE RID: 4350
		public static CompressionInfo.Integer AgentOffsetCompressionInfo = new CompressionInfo.Integer(0, 8);

		// Token: 0x040010FF RID: 4351
		public static CompressionInfo.Integer AgentHealthCompressionInfo = new CompressionInfo.Integer(-1, 11);

		// Token: 0x04001100 RID: 4352
		public static CompressionInfo.Integer AgentControllerCompressionInfo = new CompressionInfo.Integer(0, 2, true);

		// Token: 0x04001101 RID: 4353
		public static CompressionInfo.Integer TeamCompressionInfo = new CompressionInfo.Integer(-1, 10);

		// Token: 0x04001102 RID: 4354
		public static CompressionInfo.Integer TeamSideCompressionInfo = new CompressionInfo.Integer(-1, 4);

		// Token: 0x04001103 RID: 4355
		public static CompressionInfo.Integer RoundEndReasonCompressionInfo = new CompressionInfo.Integer(-1, 2, true);

		// Token: 0x04001104 RID: 4356
		public static CompressionInfo.Integer TeamScoreCompressionInfo = new CompressionInfo.Integer(-1023000, 1023000, true);

		// Token: 0x04001105 RID: 4357
		public static CompressionInfo.Integer FactionCompressionInfo = new CompressionInfo.Integer(0, 4);

		// Token: 0x04001106 RID: 4358
		public static CompressionInfo.Integer MissionOrderTypeCompressionInfo = new CompressionInfo.Integer(-1, 5);

		// Token: 0x04001107 RID: 4359
		public static CompressionInfo.Integer MissionRoundCountCompressionInfo = new CompressionInfo.Integer(-1, 7);

		// Token: 0x04001108 RID: 4360
		public static CompressionInfo.Integer MissionRoundStateCompressionInfo = new CompressionInfo.Integer(-1, 5, true);

		// Token: 0x04001109 RID: 4361
		public static CompressionInfo.Integer RoundTimeCompressionInfo = new CompressionInfo.Integer(0, MultiplayerOptions.OptionType.RoundTimeLimit.GetMaximumValue(), true);

		// Token: 0x0400110A RID: 4362
		public static CompressionInfo.Integer SelectedTroopIndexCompressionInfo = new CompressionInfo.Integer(-1, 15, true);

		// Token: 0x0400110B RID: 4363
		public static CompressionInfo.Integer MissileCompressionInfo = new CompressionInfo.Integer(0, 10);

		// Token: 0x0400110C RID: 4364
		public static CompressionInfo.Float MissileSpeedCompressionInfo = new CompressionInfo.Float(0f, 12, 0.05f);

		// Token: 0x0400110D RID: 4365
		public static CompressionInfo.Integer MissileCollisionReactionCompressionInfo = new CompressionInfo.Integer(0, 3, true);

		// Token: 0x0400110E RID: 4366
		public static CompressionInfo.Integer FlagCapturePointIndexCompressionInfo = new CompressionInfo.Integer(0, 3);

		// Token: 0x0400110F RID: 4367
		public static CompressionInfo.Integer FlagpoleIndexCompressionInfo = new CompressionInfo.Integer(0, 5, true);

		// Token: 0x04001110 RID: 4368
		public static CompressionInfo.Float FlagCapturePointDurationCompressionInfo = new CompressionInfo.Float(-1f, 14, 0.01f);

		// Token: 0x04001111 RID: 4369
		public static CompressionInfo.Float FlagProgressCompressionInfo = new CompressionInfo.Float(-1f, 1f, 12);

		// Token: 0x04001112 RID: 4370
		public static CompressionInfo.Float FlagClassicProgressCompressionInfo = new CompressionInfo.Float(0f, 1f, 11);

		// Token: 0x04001113 RID: 4371
		public static CompressionInfo.Integer FlagDirectionEnumCompressionInfo = new CompressionInfo.Integer(-1, 2, true);

		// Token: 0x04001114 RID: 4372
		public static CompressionInfo.Float FlagSpeedCompressionInfo = new CompressionInfo.Float(-1f, 14, 0.01f);

		// Token: 0x04001115 RID: 4373
		public static CompressionInfo.Integer FlagCaptureResultCompressionInfo = new CompressionInfo.Integer(0, 3, true);

		// Token: 0x04001116 RID: 4374
		public static CompressionInfo.Integer UsableGameObjectDestructionStateCompressionInfo = new CompressionInfo.Integer(0, 3);

		// Token: 0x04001117 RID: 4375
		public static CompressionInfo.Float UsableGameObjectHealthCompressionInfo = new CompressionInfo.Float(-1f, 18, 0.1f);

		// Token: 0x04001118 RID: 4376
		public static CompressionInfo.Float UsableGameObjectBlowMagnitude = new CompressionInfo.Float(0f, DestructableComponent.MaxBlowMagnitude, 8);

		// Token: 0x04001119 RID: 4377
		public static CompressionInfo.Float UsableGameObjectBlowDirection = new CompressionInfo.Float(-1f, 1f, 7);

		// Token: 0x0400111A RID: 4378
		public static CompressionInfo.Float CapturePointProgressCompressionInfo = new CompressionInfo.Float(0f, 1f, 10);

		// Token: 0x0400111B RID: 4379
		public static CompressionInfo.Integer ItemSlotCompressionInfo = new CompressionInfo.Integer(0, 4, true);

		// Token: 0x0400111C RID: 4380
		public static CompressionInfo.Integer WieldSlotCompressionInfo = new CompressionInfo.Integer(-1, 4, true);

		// Token: 0x0400111D RID: 4381
		public static CompressionInfo.Integer ItemDataCompressionInfo = new CompressionInfo.Integer(0, 10);

		// Token: 0x0400111E RID: 4382
		public static CompressionInfo.Integer WeaponReloadPhaseCompressionInfo = new CompressionInfo.Integer(0, 9, true);

		// Token: 0x0400111F RID: 4383
		public static CompressionInfo.Integer WeaponUsageIndexCompressionInfo = new CompressionInfo.Integer(0, 2);

		// Token: 0x04001120 RID: 4384
		public static CompressionInfo.Integer TauntIndexCompressionInfo = new CompressionInfo.Integer(0, TauntUsageManager.Instance.GetTauntItemCount() - 1, true);

		// Token: 0x04001121 RID: 4385
		public static CompressionInfo.Integer BarkIndexCompressionInfo = new CompressionInfo.Integer(0, SkinVoiceManager.VoiceType.MpBarks.Length - 1, true);

		// Token: 0x04001122 RID: 4386
		public static CompressionInfo.Integer UsageDirectionCompressionInfo = new CompressionInfo.Integer(-1, 9, true);

		// Token: 0x04001123 RID: 4387
		public static CompressionInfo.Float SpawnedItemVelocityCompressionInfo = new CompressionInfo.Float(-50f, 50f, 12);

		// Token: 0x04001124 RID: 4388
		public static CompressionInfo.Float SpawnedItemAngularVelocityCompressionInfo = new CompressionInfo.Float(-10f, 10f, 12);

		// Token: 0x04001125 RID: 4389
		public static CompressionInfo.UnsignedInteger SpawnedItemWeaponSpawnFlagCompressionInfo = new CompressionInfo.UnsignedInteger(0U, EnumHelper.GetCombinedUIntEnumFlagsValue(typeof(Mission.WeaponSpawnFlags)), true);

		// Token: 0x04001126 RID: 4390
		public static CompressionInfo.Integer RangedSiegeWeaponAmmoCompressionInfo = new CompressionInfo.Integer(0, 7);

		// Token: 0x04001127 RID: 4391
		public static CompressionInfo.Integer RangedSiegeWeaponAmmoIndexCompressionInfo = new CompressionInfo.Integer(0, 3);

		// Token: 0x04001128 RID: 4392
		public static CompressionInfo.Integer RangedSiegeWeaponStateCompressionInfo = new CompressionInfo.Integer(0, 8, true);

		// Token: 0x04001129 RID: 4393
		public static CompressionInfo.Integer SiegeLadderStateCompressionInfo = new CompressionInfo.Integer(0, 9, true);

		// Token: 0x0400112A RID: 4394
		public static CompressionInfo.Integer BatteringRamStateCompressionInfo = new CompressionInfo.Integer(0, 2, true);

		// Token: 0x0400112B RID: 4395
		public static CompressionInfo.Integer SiegeLadderAnimationStateCompressionInfo = new CompressionInfo.Integer(0, 2, true);

		// Token: 0x0400112C RID: 4396
		public static CompressionInfo.Float SiegeMachineComponentAngularSpeedCompressionInfo = new CompressionInfo.Float(-20f, 20f, 12);

		// Token: 0x0400112D RID: 4397
		public static CompressionInfo.Integer SiegeTowerGateStateCompressionInfo = new CompressionInfo.Integer(0, 3, true);

		// Token: 0x0400112E RID: 4398
		public static CompressionInfo.Integer NumberOfPacesCompressionInfo = new CompressionInfo.Integer(0, 3);

		// Token: 0x0400112F RID: 4399
		public static CompressionInfo.Float WalkingSpeedLimitCompressionInfo = new CompressionInfo.Float(-0.01f, 9, 0.01f);

		// Token: 0x04001130 RID: 4400
		public static CompressionInfo.Float StepSizeCompressionInfo = new CompressionInfo.Float(-0.01f, 7, 0.01f);

		// Token: 0x04001131 RID: 4401
		public static CompressionInfo.Integer BoneIndexCompressionInfo = new CompressionInfo.Integer(0, 63, true);

		// Token: 0x04001132 RID: 4402
		public static CompressionInfo.Integer AgentPrefabComponentIndexCompressionInfo = new CompressionInfo.Integer(0, 4);

		// Token: 0x04001133 RID: 4403
		public static CompressionInfo.Integer AttachedWeaponsCompressionInfo = new CompressionInfo.Integer(-1, 11);

		// Token: 0x04001134 RID: 4404
		public static CompressionInfo.Integer MultiplayerPollRejectReasonCompressionInfo = new CompressionInfo.Integer(0, 3, true);

		// Token: 0x04001135 RID: 4405
		public static CompressionInfo.Integer MultiplayerNotificationCompressionInfo = new CompressionInfo.Integer(0, MultiplayerGameNotificationsComponent.NotificationCount, true);

		// Token: 0x04001136 RID: 4406
		public static CompressionInfo.Integer MultiplayerNotificationParameterCompressionInfo = new CompressionInfo.Integer(-1, 8);

		// Token: 0x04001137 RID: 4407
		public static CompressionInfo.Integer PerkListIndexCompressionInfo = new CompressionInfo.Integer(0, 2);

		// Token: 0x04001138 RID: 4408
		public static CompressionInfo.Integer PerkIndexCompressionInfo = new CompressionInfo.Integer(0, 4);

		// Token: 0x04001139 RID: 4409
		public static CompressionInfo.Float FlagDominationMoraleCompressionInfo = new CompressionInfo.Float(-1f, 8, 0.01f);

		// Token: 0x0400113A RID: 4410
		public static CompressionInfo.Integer TdmGoldChangeCompressionInfo = new CompressionInfo.Integer(0, 2000, true);

		// Token: 0x0400113B RID: 4411
		public static CompressionInfo.Integer TdmGoldGainTypeCompressionInfo = new CompressionInfo.Integer(0, 12);

		// Token: 0x0400113C RID: 4412
		public static CompressionInfo.Integer DuelAreaIndexCompressionInfo = new CompressionInfo.Integer(0, 4);

		// Token: 0x0400113D RID: 4413
		public static CompressionInfo.Integer AutomatedBattleIndexCompressionInfo = new CompressionInfo.Integer(0, 10, true);

		// Token: 0x0400113E RID: 4414
		public static CompressionInfo.Integer SiegeMoraleCompressionInfo = new CompressionInfo.Integer(0, 1440, true);

		// Token: 0x0400113F RID: 4415
		public static CompressionInfo.Integer SiegeMoralePerFlagCompressionInfo = new CompressionInfo.Integer(0, 90, true);

		// Token: 0x04001140 RID: 4416
		public static CompressionInfo.Integer ActionSetCompressionInfo;

		// Token: 0x04001141 RID: 4417
		public static CompressionInfo.Integer MonsterUsageSetCompressionInfo;

		// Token: 0x04001142 RID: 4418
		public static CompressionInfo.Integer OrderTypeCompressionInfo = new CompressionInfo.Integer(0, 41, true);

		// Token: 0x04001143 RID: 4419
		public static CompressionInfo.Integer FormationClassCompressionInfo = new CompressionInfo.Integer(-1, 10, true);

		// Token: 0x04001144 RID: 4420
		public static CompressionInfo.Float OrderPositionCompressionInfo = new CompressionInfo.Float(-100000f, 100000f, 24);

		// Token: 0x04001145 RID: 4421
		public static CompressionInfo.Integer SynchedMissionObjectReadableRecordTypeIndex = new CompressionInfo.Integer(-1, 8);
	}
}
