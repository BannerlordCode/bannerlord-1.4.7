using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E7 RID: 743
	public static class CompressionBasic
	{
		// Token: 0x0400104A RID: 4170
		public const float MaxPossibleAbsValueForSecondMaxQuaternionComponent = 0.7071068f;

		// Token: 0x0400104B RID: 4171
		public const float MaxPositionZForCompression = 2521f;

		// Token: 0x0400104C RID: 4172
		public const float MaxPositionForCompression = 10385f;

		// Token: 0x0400104D RID: 4173
		public const float MinPositionForCompression = -100f;

		// Token: 0x0400104E RID: 4174
		public static CompressionInfo.Integer PingValueCompressionInfo = new CompressionInfo.Integer(0, 1023, true);

		// Token: 0x0400104F RID: 4175
		public static CompressionInfo.Integer LossValueCompressionInfo = new CompressionInfo.Integer(0, 100, true);

		// Token: 0x04001050 RID: 4176
		public static CompressionInfo.Integer ServerPerformanceStateCompressionInfo = new CompressionInfo.Integer(0, 2, true);

		// Token: 0x04001051 RID: 4177
		public static CompressionInfo.UnsignedInteger ColorCompressionInfo = new CompressionInfo.UnsignedInteger(0U, 32);

		// Token: 0x04001052 RID: 4178
		public static CompressionInfo.Integer ItemDataValueCompressionInfo = new CompressionInfo.Integer(0, 16);

		// Token: 0x04001053 RID: 4179
		public static CompressionInfo.Integer RandomSeedCompressionInfo = new CompressionInfo.Integer(0, 2000, true);

		// Token: 0x04001054 RID: 4180
		public static CompressionInfo.Float PositionCompressionInfo = new CompressionInfo.Float(-100f, 10385f, 22);

		// Token: 0x04001055 RID: 4181
		public static CompressionInfo.Float LocalPositionCompressionInfo = new CompressionInfo.Float(-32f, 32f, 16);

		// Token: 0x04001056 RID: 4182
		public static CompressionInfo.Float LowResLocalPositionCompressionInfo = new CompressionInfo.Float(-32f, 32f, 12);

		// Token: 0x04001057 RID: 4183
		public static CompressionInfo.Float BigRangeLowResLocalPositionCompressionInfo = new CompressionInfo.Float(-1000f, 1000f, 16);

		// Token: 0x04001058 RID: 4184
		public static CompressionInfo.Integer PlayerCompressionInfo = new CompressionInfo.Integer(-1, 1022, true);

		// Token: 0x04001059 RID: 4185
		public static CompressionInfo.UnsignedInteger PeerComponentCompressionInfo = new CompressionInfo.UnsignedInteger(0U, 32);

		// Token: 0x0400105A RID: 4186
		public static CompressionInfo.UnsignedInteger GUIDCompressionInfo = new CompressionInfo.UnsignedInteger(0U, 32);

		// Token: 0x0400105B RID: 4187
		public static CompressionInfo.Integer FlagsCompressionInfo = new CompressionInfo.Integer(0, 30);

		// Token: 0x0400105C RID: 4188
		public static CompressionInfo.Integer GUIDIntCompressionInfo = new CompressionInfo.Integer(-1, 31);

		// Token: 0x0400105D RID: 4189
		public static CompressionInfo.Integer MissionObjectIDCompressionInfo = new CompressionInfo.Integer(-1, 8190, true);

		// Token: 0x0400105E RID: 4190
		public static CompressionInfo.Float UnitVectorCompressionInfo = new CompressionInfo.Float(-1.024f, 10, 0.002f);

		// Token: 0x0400105F RID: 4191
		public static CompressionInfo.Float LowResRadianCompressionInfo = new CompressionInfo.Float(-3.1515927f, 3.1515927f, 8);

		// Token: 0x04001060 RID: 4192
		public static CompressionInfo.Float RadianCompressionInfo = new CompressionInfo.Float(-3.1515927f, 3.1515927f, 10);

		// Token: 0x04001061 RID: 4193
		public static CompressionInfo.Float HighResRadianCompressionInfo = new CompressionInfo.Float(-3.1515927f, 3.1515927f, 13);

		// Token: 0x04001062 RID: 4194
		public static CompressionInfo.Float ScaleCompressionInfo = new CompressionInfo.Float(-0.001f, 10, 0.01f);

		// Token: 0x04001063 RID: 4195
		public static CompressionInfo.Float LowResQuaternionCompressionInfo = new CompressionInfo.Float(-0.7071068f, 0.7071068f, 6);

		// Token: 0x04001064 RID: 4196
		public static CompressionInfo.Integer OmittedQuaternionComponentIndexCompressionInfo = new CompressionInfo.Integer(0, 3, true);

		// Token: 0x04001065 RID: 4197
		public static CompressionInfo.Float ImpulseCompressionInfo = new CompressionInfo.Float(-500f, 16, 0.0153f);

		// Token: 0x04001066 RID: 4198
		public static CompressionInfo.Integer AnimationKeyCompressionInfo = new CompressionInfo.Integer(0, 8000, true);

		// Token: 0x04001067 RID: 4199
		public static CompressionInfo.Float AnimationSpeedCompressionInfo = new CompressionInfo.Float(0f, 9, 0.01f);

		// Token: 0x04001068 RID: 4200
		public static CompressionInfo.Float AnimationProgressCompressionInfo = new CompressionInfo.Float(0f, 1f, 9);

		// Token: 0x04001069 RID: 4201
		public static CompressionInfo.Float VertexAnimationSpeedCompressionInfo = new CompressionInfo.Float(0f, 9, 0.1f);

		// Token: 0x0400106A RID: 4202
		public static CompressionInfo.Integer PercentageCompressionInfo = new CompressionInfo.Integer(0, 100, true);

		// Token: 0x0400106B RID: 4203
		public static CompressionInfo.Integer EntityChildCountCompressionInfo = new CompressionInfo.Integer(0, 8);

		// Token: 0x0400106C RID: 4204
		public static CompressionInfo.Integer AgentHitDamageCompressionInfo = new CompressionInfo.Integer(0, 2000, true);

		// Token: 0x0400106D RID: 4205
		public static CompressionInfo.Integer AgentHitModifiedDamageCompressionInfo = new CompressionInfo.Integer(-2000, 2000, true);

		// Token: 0x0400106E RID: 4206
		public static CompressionInfo.Float AgentHitRelativeSpeedCompressionInfo = new CompressionInfo.Float(0f, 17, 0.01f);

		// Token: 0x0400106F RID: 4207
		public static CompressionInfo.Integer AgentHitArmorCompressionInfo = new CompressionInfo.Integer(0, 200, true);

		// Token: 0x04001070 RID: 4208
		public static CompressionInfo.Integer AgentHitBoneIndexCompressionInfo = new CompressionInfo.Integer(-1, 63, true);

		// Token: 0x04001071 RID: 4209
		public static CompressionInfo.Integer AgentHitBodyPartCompressionInfo = new CompressionInfo.Integer(-1, 8, true);

		// Token: 0x04001072 RID: 4210
		public static CompressionInfo.Integer AgentHitDamageTypeCompressionInfo = new CompressionInfo.Integer(-1, 2, true);

		// Token: 0x04001073 RID: 4211
		public static CompressionInfo.Integer RoundGoldAmountCompressionInfo = new CompressionInfo.Integer(-1, 2000, true);

		// Token: 0x04001074 RID: 4212
		public static CompressionInfo.Integer DebugIntNonCompressionInfo = new CompressionInfo.Integer(int.MinValue, 32);

		// Token: 0x04001075 RID: 4213
		public static CompressionInfo.UnsignedLongInteger DebugULongNonCompressionInfo = new CompressionInfo.UnsignedLongInteger(0UL, 64);

		// Token: 0x04001076 RID: 4214
		public static CompressionInfo.Float AgentAgeCompressionInfo = new CompressionInfo.Float(0f, 128f, 10);

		// Token: 0x04001077 RID: 4215
		public static CompressionInfo.Float FaceKeyDataCompressionInfo = new CompressionInfo.Float(0f, 1f, 10);

		// Token: 0x04001078 RID: 4216
		public static CompressionInfo.Integer PlayerChosenBadgeCompressionInfo = new CompressionInfo.Integer(-1, 8);

		// Token: 0x04001079 RID: 4217
		public static CompressionInfo.Integer MaxNumberOfPlayersCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.MaxNumberOfPlayers.GetMinimumValue(), MultiplayerOptions.OptionType.MaxNumberOfPlayers.GetMaximumValue(), true);

		// Token: 0x0400107A RID: 4218
		public static CompressionInfo.Integer MinNumberOfPlayersForMatchStartCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart.GetMinimumValue(), MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart.GetMaximumValue(), true);

		// Token: 0x0400107B RID: 4219
		public static CompressionInfo.Integer MapTimeLimitCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.MapTimeLimit.GetMinimumValue(), MultiplayerOptions.OptionType.MapTimeLimit.GetMaximumValue(), true);

		// Token: 0x0400107C RID: 4220
		public static CompressionInfo.Integer RoundTotalCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.RoundTotal.GetMinimumValue(), MultiplayerOptions.OptionType.RoundTotal.GetMaximumValue(), true);

		// Token: 0x0400107D RID: 4221
		public static CompressionInfo.Integer RoundTimeLimitCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.RoundTimeLimit.GetMinimumValue(), MultiplayerOptions.OptionType.RoundTimeLimit.GetMaximumValue(), true);

		// Token: 0x0400107E RID: 4222
		public static CompressionInfo.Integer WarmupTimeLimitCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.GetMinimumValue(), MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.GetMaximumValue(), true);

		// Token: 0x0400107F RID: 4223
		public static CompressionInfo.Integer RoundPreparationTimeLimitCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.RoundPreparationTimeLimit.GetMinimumValue(), MultiplayerOptions.OptionType.RoundPreparationTimeLimit.GetMaximumValue(), true);

		// Token: 0x04001080 RID: 4224
		public static CompressionInfo.Integer RespawnPeriodCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.RespawnPeriodTeam1.GetMinimumValue(), MultiplayerOptions.OptionType.RespawnPeriodTeam1.GetMaximumValue(), true);

		// Token: 0x04001081 RID: 4225
		public static CompressionInfo.Integer GoldGainChangePercentageCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.GetMinimumValue(), MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.GetMaximumValue(), true);

		// Token: 0x04001082 RID: 4226
		public static CompressionInfo.Integer SpectatorCameraTypeCompressionInfo = new CompressionInfo.Integer(-1, 7, true);

		// Token: 0x04001083 RID: 4227
		public static CompressionInfo.Integer PollAcceptThresholdCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.PollAcceptThreshold.GetMinimumValue(), MultiplayerOptions.OptionType.PollAcceptThreshold.GetMaximumValue(), true);

		// Token: 0x04001084 RID: 4228
		public static CompressionInfo.Integer NumberOfBotsTeamCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetMinimumValue(), MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetMaximumValue(), true);

		// Token: 0x04001085 RID: 4229
		public static CompressionInfo.Integer NumberOfBotsPerFormationCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetMinimumValue(), MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetMaximumValue(), true);

		// Token: 0x04001086 RID: 4230
		public static CompressionInfo.Integer AutoTeamBalanceLimitCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetMinimumValue(), MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetMaximumValue(), true);

		// Token: 0x04001087 RID: 4231
		public static CompressionInfo.Integer FriendlyFireDamageCompressionInfo = new CompressionInfo.Integer(MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.GetMinimumValue(), MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.GetMaximumValue(), true);

		// Token: 0x04001088 RID: 4232
		public static CompressionInfo.Integer ForcedAvatarIndexCompressionInfo = new CompressionInfo.Integer(-1, 99, true);

		// Token: 0x04001089 RID: 4233
		public static CompressionInfo.Integer IntermissionStateCompressionInfo = new CompressionInfo.Integer(0, Enum.GetNames(typeof(MultiplayerIntermissionState)).Length - 1, false);

		// Token: 0x0400108A RID: 4234
		public static CompressionInfo.Float IntermissionTimerCompressionInfo = new CompressionInfo.Float(0f, 240f, 14);

		// Token: 0x0400108B RID: 4235
		public static CompressionInfo.Integer IntermissionMapVoteItemCountCompressionInfo = new CompressionInfo.Integer(0, 99, true);

		// Token: 0x0400108C RID: 4236
		public static CompressionInfo.Integer IntermissionVoterCountCompressionInfo = new CompressionInfo.Integer(0, 1022, true);

		// Token: 0x0400108D RID: 4237
		public static CompressionInfo.Integer ActionCodeCompressionInfo;

		// Token: 0x0400108E RID: 4238
		public static CompressionInfo.Integer AnimationIndexCompressionInfo;

		// Token: 0x0400108F RID: 4239
		public static CompressionInfo.Integer CultureIndexCompressionInfo;

		// Token: 0x04001090 RID: 4240
		public static CompressionInfo.Integer SoundEventsCompressionInfo;

		// Token: 0x04001091 RID: 4241
		public static CompressionInfo.Integer NetworkComponentEventTypeFromServerCompressionInfo;

		// Token: 0x04001092 RID: 4242
		public static CompressionInfo.Integer NetworkComponentEventTypeFromClientCompressionInfo;

		// Token: 0x04001093 RID: 4243
		public static CompressionInfo.Integer TroopTypeCompressionInfo = new CompressionInfo.Integer(-1, 2, true);

		// Token: 0x04001094 RID: 4244
		public static CompressionInfo.Integer BannerDataCountCompressionInfo = new CompressionInfo.Integer(0, 31, true);

		// Token: 0x04001095 RID: 4245
		public static CompressionInfo.Integer BannerDataMeshIdCompressionInfo = new CompressionInfo.Integer(0, 13);

		// Token: 0x04001096 RID: 4246
		public static CompressionInfo.Integer BannerDataColorIndexCompressionInfo = new CompressionInfo.Integer(0, 10);

		// Token: 0x04001097 RID: 4247
		public static CompressionInfo.Integer BannerDataSizeCompressionInfo = new CompressionInfo.Integer(-8000, 8000, true);

		// Token: 0x04001098 RID: 4248
		public static CompressionInfo.Integer BannerDataRotationCompressionInfo = new CompressionInfo.Integer(0, 360, true);
	}
}
