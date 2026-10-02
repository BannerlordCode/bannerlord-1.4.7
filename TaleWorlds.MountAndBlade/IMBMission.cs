using System;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001AB RID: 427
	[ScriptingInterfaceBase]
	internal interface IMBMission
	{
		// Token: 0x060017EB RID: 6123
		[EngineMethod("clear_resources", false, null, false)]
		void ClearResources(UIntPtr missionPointer);

		// Token: 0x060017EC RID: 6124
		[EngineMethod("create_mission", false, null, false)]
		UIntPtr CreateMission(Mission mission);

		// Token: 0x060017ED RID: 6125
		[EngineMethod("set_close_proximity_wave_sounds_enabled", false, null, false)]
		void SetCloseProximityWaveSoundsEnabled(UIntPtr missionPointer, bool value);

		// Token: 0x060017EE RID: 6126
		[EngineMethod("force_disable_occlusion", false, null, false)]
		void ForceDisableOcclusion(UIntPtr missionPointer, bool value);

		// Token: 0x060017EF RID: 6127
		[EngineMethod("tick_agents_and_teams_async", false, null, false)]
		void TickAgentsAndTeamsAsync(UIntPtr missionPointer, float dt);

		// Token: 0x060017F0 RID: 6128
		[EngineMethod("get_tick_debug_paused", false, null, false)]
		bool GetTickDebugPaused(UIntPtr missionPointer);

		// Token: 0x060017F1 RID: 6129
		[EngineMethod("clear_agent_actions", false, null, false)]
		void ClearAgentActions(UIntPtr missionPointer);

		// Token: 0x060017F2 RID: 6130
		[EngineMethod("clear_missiles", false, null, false)]
		void ClearMissiles(UIntPtr missionPointer);

		// Token: 0x060017F3 RID: 6131
		[EngineMethod("clear_corpses", false, null, false)]
		void ClearCorpses(UIntPtr missionPointer, bool isMissionReset);

		// Token: 0x060017F4 RID: 6132
		[EngineMethod("get_pause_ai_tick", false, null, false)]
		bool GetPauseAITick(UIntPtr missionPointer);

		// Token: 0x060017F5 RID: 6133
		[EngineMethod("set_pause_ai_tick", false, null, false)]
		void SetPauseAITick(UIntPtr missionPointer, bool value);

		// Token: 0x060017F6 RID: 6134
		[EngineMethod("get_clear_scene_timer_elapsed_time", false, null, false)]
		float GetClearSceneTimerElapsedTime(UIntPtr missionPointer);

		// Token: 0x060017F7 RID: 6135
		[EngineMethod("reset_first_third_person_view", false, null, false)]
		void ResetFirstThirdPersonView(UIntPtr missionPointer);

		// Token: 0x060017F8 RID: 6136
		[EngineMethod("set_camera_is_first_person", false, null, false)]
		void SetCameraIsFirstPerson(bool value);

		// Token: 0x060017F9 RID: 6137
		[EngineMethod("set_camera_frame", false, null, false)]
		void SetCameraFrame(UIntPtr missionPointer, ref MatrixFrame cameraFrame, float zoomFactor, ref Vec3 attenuationPosition);

		// Token: 0x060017FA RID: 6138
		[EngineMethod("get_camera_frame", false, null, false)]
		MatrixFrame GetCameraFrame(UIntPtr missionPointer);

		// Token: 0x060017FB RID: 6139
		[EngineMethod("get_is_loading_finished", false, null, false)]
		bool GetIsLoadingFinished(UIntPtr missionPointer);

		// Token: 0x060017FC RID: 6140
		[EngineMethod("clear_scene", false, null, false)]
		void ClearScene(UIntPtr missionPointer);

		// Token: 0x060017FD RID: 6141
		[EngineMethod("initialize_mission", false, null, false)]
		void InitializeMission(UIntPtr missionPointer, ref MissionInitializerRecord rec);

		// Token: 0x060017FE RID: 6142
		[EngineMethod("finalize_mission", false, null, false)]
		void FinalizeMission(UIntPtr missionPointer);

		// Token: 0x060017FF RID: 6143
		[EngineMethod("get_time", false, null, false)]
		float GetTime(UIntPtr missionPointer);

		// Token: 0x06001800 RID: 6144
		[EngineMethod("get_average_fps", false, null, false)]
		float GetAverageFps(UIntPtr missionPointer);

		// Token: 0x06001801 RID: 6145
		[EngineMethod("get_combat_type", false, null, false)]
		int GetCombatType(UIntPtr missionPointer);

		// Token: 0x06001802 RID: 6146
		[EngineMethod("set_combat_type", false, null, false)]
		void SetCombatType(UIntPtr missionPointer, int combatType);

		// Token: 0x06001803 RID: 6147
		[EngineMethod("ray_cast_for_closest_agent", false, null, false)]
		Agent RayCastForClosestAgent(UIntPtr missionPointer, Vec3 sourcePoint, Vec3 rayFinishPoint, int excludeAgentIndex, float rayThickness, out float collisionDistance);

		// Token: 0x06001804 RID: 6148
		[EngineMethod("ray_cast_for_closest_agents_limbs", false, null, false)]
		Agent RayCastForClosestAgentsLimbs(UIntPtr missionPointer, Vec3 sourcePoint, Vec3 rayFinishPoint, int excludeAgentIndex, float rayThickness, out float collisionDistance, out sbyte boneIndex);

		// Token: 0x06001805 RID: 6149
		[EngineMethod("ray_cast_for_given_agents_limbs", false, null, false)]
		bool RayCastForGivenAgentsLimbs(UIntPtr missionPointer, Vec3 sourcePoint, Vec3 rayFinishPoint, int givenAgentIndex, float rayThickness, out float collisionDistance, out sbyte boneIndex);

		// Token: 0x06001806 RID: 6150
		[EngineMethod("get_number_of_teams", false, null, false)]
		int GetNumberOfTeams(UIntPtr missionPointer);

		// Token: 0x06001807 RID: 6151
		[EngineMethod("reset_teams", false, null, false)]
		void ResetTeams(UIntPtr missionPointer);

		// Token: 0x06001808 RID: 6152
		[EngineMethod("add_team", false, null, false)]
		int AddTeam(UIntPtr missionPointer);

		// Token: 0x06001809 RID: 6153
		[EngineMethod("restart_record", false, null, false)]
		void RestartRecord(UIntPtr missionPointer);

		// Token: 0x0600180A RID: 6154
		[EngineMethod("is_position_inside_boundaries", false, null, false)]
		bool IsPositionInsideBoundaries(UIntPtr missionPointer, Vec2 position);

		// Token: 0x0600180B RID: 6155
		[EngineMethod("is_position_inside_hard_boundaries", false, null, false)]
		bool IsPositionInsideHardBoundaries(UIntPtr missionPointer, Vec2 position);

		// Token: 0x0600180C RID: 6156
		[EngineMethod("is_position_inside_any_blocker_nav_mesh_face_2d", false, null, false)]
		bool IsPositionInsideAnyBlockerNavMeshFace2D(UIntPtr missionPointer, Vec2 position);

		// Token: 0x0600180D RID: 6157
		[EngineMethod("is_position_on_any_blocker_nav_mesh_face", false, null, false)]
		bool IsPositionOnAnyBlockerNavMeshFace(UIntPtr missionPointer, Vec3 position);

		// Token: 0x0600180E RID: 6158
		[EngineMethod("get_alternate_position_for_navmeshless_or_out_of_bounds_position", false, null, false)]
		WorldPosition GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(UIntPtr ptr, ref Vec2 directionTowards, ref WorldPosition originalPosition, ref float positionPenalty);

		// Token: 0x0600180F RID: 6159
		[EngineMethod("add_missile", false, null, false)]
		int AddMissile(UIntPtr missionPointer, bool isPrediction, int shooterAgentIndex, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, int weaponStatsDataLength, float damageBonus, ref Vec3 position, ref Vec3 direction, ref Mat3 orientation, float baseSpeed, float speed, bool addRigidBody, UIntPtr entityPointer, int forcedMissileIndex, bool isPrimaryWeaponShot, out UIntPtr missileEntity);

		// Token: 0x06001810 RID: 6160
		[EngineMethod("add_missile_single_usage", false, null, false)]
		int AddMissileSingleUsage(UIntPtr missionPointer, bool isPrediction, int shooterAgentIndex, in WeaponData weaponData, in WeaponStatsData weaponStatsData, float damageBonus, ref Vec3 position, ref Vec3 direction, ref Mat3 orientation, float baseSpeed, float speed, bool addRigidBody, UIntPtr entityPointer, int forcedMissileIndex, bool isPrimaryWeaponShot, out UIntPtr missileEntity);

		// Token: 0x06001811 RID: 6161
		[EngineMethod("get_missile_collision_point", false, null, false)]
		Vec3 GetMissileCollisionPoint(UIntPtr missionPointer, Vec3 missileStartingPosition, Vec3 missileDirection, float missileStartingSpeed, in WeaponData weaponData);

		// Token: 0x06001812 RID: 6162
		[EngineMethod("remove_missile", false, null, false)]
		void RemoveMissile(UIntPtr missionPointer, int missileIndex);

		// Token: 0x06001813 RID: 6163
		[EngineMethod("get_missile_vertical_aim_correction", false, null, false)]
		float GetMissileVerticalAimCorrection(Vec3 vecToTarget, float missileStartingSpeed, ref WeaponStatsData weaponStatsData, float airFrictionConstant);

		// Token: 0x06001814 RID: 6164
		[EngineMethod("get_missile_range", false, null, false)]
		float GetMissileRange(float missileStartingSpeed, float heightDifference);

		// Token: 0x06001815 RID: 6165
		[EngineMethod("compute_exact_missile_range_at_height_difference", false, null, false)]
		float ComputeExactMissileRangeAtHeightDifference(float targetHeightDifference, float initialSpeed, float airFrictionConstant, float maxDuration);

		// Token: 0x06001816 RID: 6166
		[EngineMethod("prepare_missile_weapon_for_drop", false, null, false)]
		void PrepareMissileWeaponForDrop(UIntPtr missionPointer, int missileIndex);

		// Token: 0x06001817 RID: 6167
		[EngineMethod("add_particle_system_burst_by_name", false, null, false)]
		void AddParticleSystemBurstByName(UIntPtr missionPointer, string particleSystem, ref MatrixFrame frame, bool synchThroughNetwork);

		// Token: 0x06001818 RID: 6168
		[EngineMethod("tick", false, null, false)]
		void Tick(UIntPtr missionPointer, float dt);

		// Token: 0x06001819 RID: 6169
		[EngineMethod("idle_tick", false, null, false)]
		void IdleTick(UIntPtr missionPointer, float dt);

		// Token: 0x0600181A RID: 6170
		[EngineMethod("make_sound", false, null, false)]
		void MakeSound(UIntPtr pointer, int nativeSoundCode, Vec3 position, bool soundCanBePredicted, bool isReliable, int relatedAgent1, int relatedAgent2);

		// Token: 0x0600181B RID: 6171
		[EngineMethod("make_sound_with_parameter", false, null, false)]
		void MakeSoundWithParameter(UIntPtr pointer, int nativeSoundCode, Vec3 position, bool soundCanBePredicted, bool isReliable, int relatedAgent1, int relatedAgent2, SoundEventParameter parameter);

		// Token: 0x0600181C RID: 6172
		[EngineMethod("make_sound_only_on_related_peer", false, null, false)]
		void MakeSoundOnlyOnRelatedPeer(UIntPtr pointer, int nativeSoundCode, Vec3 position, int relatedAgent);

		// Token: 0x0600181D RID: 6173
		[EngineMethod("create_agent", false, null, false)]
		Mission.AgentCreationResult CreateAgent(UIntPtr missionPointer, ulong monsterFlag, int forcedAgentIndex, bool isFemale, ref AgentSpawnData spawnData, ref CapsuleData bodyCapsule, ref CapsuleData crouchedBodyCapsule, ref AnimationSystemData animationSystemData, int instanceNo);

		// Token: 0x0600181E RID: 6174
		[EngineMethod("get_position_of_missile", false, null, false)]
		Vec3 GetPositionOfMissile(UIntPtr missionPointer, int index);

		// Token: 0x0600181F RID: 6175
		[EngineMethod("get_old_position_of_missile", false, null, false)]
		Vec3 GetOldPositionOfMissile(UIntPtr missionPointer, int index);

		// Token: 0x06001820 RID: 6176
		[EngineMethod("get_velocity_of_missile", false, null, false)]
		Vec3 GetVelocityOfMissile(UIntPtr missionPointer, int index);

		// Token: 0x06001821 RID: 6177
		[EngineMethod("set_velocity_of_missile", false, null, false)]
		void SetVelocityOfMissile(UIntPtr missionPointer, int index, in Vec3 velocity);

		// Token: 0x06001822 RID: 6178
		[EngineMethod("get_missile_has_rigid_body", false, null, false)]
		bool GetMissileHasRigidBody(UIntPtr missionPointer, int index);

		// Token: 0x06001823 RID: 6179
		[EngineMethod("add_boundary", false, null, false)]
		bool AddBoundary(UIntPtr missionPointer, string name, Vec2[] boundaryPoints, int boundaryPointCount, bool isAllowanceInside);

		// Token: 0x06001824 RID: 6180
		[EngineMethod("remove_boundary", false, null, false)]
		bool RemoveBoundary(UIntPtr missionPointer, string name);

		// Token: 0x06001825 RID: 6181
		[EngineMethod("get_boundary_points", false, null, false)]
		void GetBoundaryPoints(UIntPtr missionPointer, string name, int boundaryPointOffset, Vec2[] boundaryPoints, int boundaryPointsSize, ref int retrievedPointCount);

		// Token: 0x06001826 RID: 6182
		[EngineMethod("get_boundary_count", false, null, false)]
		int GetBoundaryCount(UIntPtr missionPointer);

		// Token: 0x06001827 RID: 6183
		[EngineMethod("get_boundary_radius", false, null, false)]
		float GetBoundaryRadius(UIntPtr missionPointer, string name);

		// Token: 0x06001828 RID: 6184
		[EngineMethod("get_boundary_name", false, null, false)]
		string GetBoundaryName(UIntPtr missionPointer, int boundaryIndex);

		// Token: 0x06001829 RID: 6185
		[EngineMethod("get_closest_boundary_position", false, null, false)]
		Vec2 GetClosestBoundaryPosition(UIntPtr missionPointer, Vec2 position);

		// Token: 0x0600182A RID: 6186
		[EngineMethod("get_navigation_points", false, null, false)]
		bool GetNavigationPoints(UIntPtr missionPointer, ref NavigationData navigationData);

		// Token: 0x0600182B RID: 6187
		[EngineMethod("set_navigation_face_cost_with_id_around_position", false, null, false)]
		void SetNavigationFaceCostWithIdAroundPosition(UIntPtr missionPointer, int navigationFaceId, Vec3 position, float cost);

		// Token: 0x0600182C RID: 6188
		[EngineMethod("pause_mission_scene_sounds", false, null, false)]
		void PauseMissionSceneSounds(UIntPtr missionPointer);

		// Token: 0x0600182D RID: 6189
		[EngineMethod("resume_mission_scene_sounds", false, null, false)]
		void ResumeMissionSceneSounds(UIntPtr missionPointer);

		// Token: 0x0600182E RID: 6190
		[EngineMethod("process_record_until_time", false, null, false)]
		void ProcessRecordUntilTime(UIntPtr missionPointer, float time);

		// Token: 0x0600182F RID: 6191
		[EngineMethod("end_of_record", false, null, false)]
		bool EndOfRecord(UIntPtr missionPointer);

		// Token: 0x06001830 RID: 6192
		[EngineMethod("record_current_state", false, null, false)]
		void RecordCurrentState(UIntPtr missionPointer);

		// Token: 0x06001831 RID: 6193
		[EngineMethod("start_recording", false, null, false)]
		void StartRecording();

		// Token: 0x06001832 RID: 6194
		[EngineMethod("backup_record_to_file", false, null, false)]
		void BackupRecordToFile(UIntPtr missionPointer, string fileName, string gameType, string sceneLevels);

		// Token: 0x06001833 RID: 6195
		[EngineMethod("restore_record_from_file", false, null, false)]
		void RestoreRecordFromFile(UIntPtr missionPointer, string fileName);

		// Token: 0x06001834 RID: 6196
		[EngineMethod("clear_record_buffers", false, null, false)]
		void ClearRecordBuffers(UIntPtr missionPointer);

		// Token: 0x06001835 RID: 6197
		[EngineMethod("get_scene_name_for_replay", false, null, false)]
		string GetSceneNameForReplay(PlatformFilePath replayName);

		// Token: 0x06001836 RID: 6198
		[EngineMethod("get_game_type_for_replay", false, null, false)]
		string GetGameTypeForReplay(PlatformFilePath replayName);

		// Token: 0x06001837 RID: 6199
		[EngineMethod("get_scene_levels_for_replay", false, null, false)]
		string GetSceneLevelsForReplay(PlatformFilePath replayName);

		// Token: 0x06001838 RID: 6200
		[EngineMethod("get_atmosphere_name_for_replay", false, null, false)]
		string GetAtmosphereNameForReplay(PlatformFilePath replayName);

		// Token: 0x06001839 RID: 6201
		[EngineMethod("get_atmosphere_season_for_replay", false, null, false)]
		int GetAtmosphereSeasonForReplay(PlatformFilePath replayName);

		// Token: 0x0600183A RID: 6202
		[EngineMethod("get_closest_enemy", false, null, false)]
		Agent GetClosestEnemy(UIntPtr missionPointer, int teamIndex, Vec3 position, float radius);

		// Token: 0x0600183B RID: 6203
		[EngineMethod("get_closest_ally", false, null, false)]
		Agent GetClosestAlly(UIntPtr missionPointer, int teamIndex, Vec3 position, float radius);

		// Token: 0x0600183C RID: 6204
		[EngineMethod("is_agent_in_proximity_map", false, null, false)]
		bool IsAgentInProximityMap(UIntPtr missionPointer, int agentIndex);

		// Token: 0x0600183D RID: 6205
		[EngineMethod("has_any_agents_of_team_around", false, null, false)]
		bool HasAnyAgentsOfTeamAround(UIntPtr missionPointer, Vec3 origin, float radius, int teamNo);

		// Token: 0x0600183E RID: 6206
		[EngineMethod("get_agent_count_around_position", false, null, false)]
		void GetAgentCountAroundPosition(UIntPtr missionPointer, int teamIndex, Vec2 position, float radius, ref int allyCount, ref int enemyCount);

		// Token: 0x0600183F RID: 6207
		[EngineMethod("find_agent_with_index", false, null, false)]
		Agent FindAgentWithIndex(UIntPtr missionPointer, int index);

		// Token: 0x06001840 RID: 6208
		[EngineMethod("set_random_decide_time_of_agents", false, null, false)]
		void SetRandomDecideTimeOfAgents(UIntPtr missionPointer, int agentCount, int[] agentIndices, float minAIReactionTime, float maxAIReactionTime);

		// Token: 0x06001841 RID: 6209
		[EngineMethod("get_average_morale_of_agents", false, null, false)]
		float GetAverageMoraleOfAgents(UIntPtr missionPointer, int agentCount, int[] agentIndices);

		// Token: 0x06001842 RID: 6210
		[EngineMethod("get_best_slope_towards_direction", false, null, false)]
		WorldPosition GetBestSlopeTowardsDirection(UIntPtr missionPointer, ref WorldPosition centerPosition, float halfsize, ref WorldPosition referencePosition);

		// Token: 0x06001843 RID: 6211
		[EngineMethod("get_best_slope_angle_height_pos_for_defending", false, null, false)]
		WorldPosition GetBestSlopeAngleHeightPosForDefending(UIntPtr missionPointer, WorldPosition enemyPosition, WorldPosition defendingPosition, int sampleSize, float distanceRatioAllowedFromDefendedPos, float distanceSqrdAllowedFromBoundary, float cosinusOfBestSlope, float cosinusOfMaxAcceptedSlope, float minSlopeScore, float maxSlopeScore, float excessiveSlopePenalty, float nearConeCenterRatio, float nearConeCenterBonus, float heightDifferenceCeiling, float maxDisplacementPenalty);

		// Token: 0x06001844 RID: 6212
		[EngineMethod("get_nearby_agents_aux", false, null, false)]
		void GetNearbyAgentsAux(UIntPtr missionPointer, Vec2 center, float radius, int teamIndex, int friendOrEnemyOrAll, int agentsArrayOffset, ref EngineStackArray.StackArray40Int agentIds, ref int retrievedAgentCount);

		// Token: 0x06001845 RID: 6213
		[EngineMethod("get_weighted_point_of_enemies", false, null, false)]
		Vec2 GetWeightedPointOfEnemies(UIntPtr missionPointer, int agentIndex, Vec2 basePoint);

		// Token: 0x06001846 RID: 6214
		[EngineMethod("is_formation_unit_position_available", false, null, false)]
		bool IsFormationUnitPositionAvailable(UIntPtr missionPointer, ref WorldPosition orderPosition, ref WorldPosition unitPosition, ref WorldPosition nearestAvailableUnitPosition, float manhattanDistance);

		// Token: 0x06001847 RID: 6215
		[EngineMethod("get_straight_path_to_target", false, null, false)]
		WorldPosition GetStraightPathToTarget(UIntPtr scenePointer, Vec2 targetPosition, WorldPosition startingPosition, float samplingDistance, bool stopAtObstacle);

		// Token: 0x06001848 RID: 6216
		[EngineMethod("set_bow_missile_speed_modifier", false, null, false)]
		void SetBowMissileSpeedModifier(UIntPtr missionPointer, float modifier);

		// Token: 0x06001849 RID: 6217
		[EngineMethod("set_crossbow_missile_speed_modifier", false, null, false)]
		void SetCrossbowMissileSpeedModifier(UIntPtr missionPointer, float modifier);

		// Token: 0x0600184A RID: 6218
		[EngineMethod("set_throwing_missile_speed_modifier", false, null, false)]
		void SetThrowingMissileSpeedModifier(UIntPtr missionPointer, float modifier);

		// Token: 0x0600184B RID: 6219
		[EngineMethod("set_missile_range_modifier", false, null, false)]
		void SetMissileRangeModifier(UIntPtr missionPointer, float modifier);

		// Token: 0x0600184C RID: 6220
		[EngineMethod("set_last_movement_key_pressed", false, null, false)]
		void SetLastMovementKeyPressed(UIntPtr missionPointer, Agent.MovementControlFlag lastMovementKeyPressed);

		// Token: 0x0600184D RID: 6221
		[EngineMethod("skip_forward_mission_replay", false, null, false)]
		void SkipForwardMissionReplay(UIntPtr missionPointer, float startTime, float endTime);

		// Token: 0x0600184E RID: 6222
		[EngineMethod("get_debug_agent", false, null, false)]
		int GetDebugAgent(UIntPtr missionPointer);

		// Token: 0x0600184F RID: 6223
		[EngineMethod("set_debug_agent", false, null, false)]
		void SetDebugAgent(UIntPtr missionPointer, int index);

		// Token: 0x06001850 RID: 6224
		[EngineMethod("add_ai_debug_text", false, null, false)]
		void AddAiDebugText(UIntPtr missionPointer, string text);

		// Token: 0x06001851 RID: 6225
		[EngineMethod("agent_proximity_map_begin_search", false, null, false)]
		AgentProximityMap.ProximityMapSearchStructInternal ProximityMapBeginSearch(UIntPtr missionPointer, Vec2 searchPos, float searchRadius);

		// Token: 0x06001852 RID: 6226
		[EngineMethod("agent_proximity_map_find_next", false, null, false)]
		void ProximityMapFindNext(UIntPtr missionPointer, ref AgentProximityMap.ProximityMapSearchStructInternal searchStruct);

		// Token: 0x06001853 RID: 6227
		[EngineMethod("agent_proximity_map_get_max_search_radius", false, null, false)]
		float ProximityMapMaxSearchRadius(UIntPtr missionPointer);

		// Token: 0x06001854 RID: 6228
		[EngineMethod("set_override_corpse_count", false, null, false)]
		void SetOverrideCorpseCount(UIntPtr missionPointer, int overrideCorpseCount);

		// Token: 0x06001855 RID: 6229
		[EngineMethod("get_biggest_agent_collision_padding", false, null, false)]
		float GetBiggestAgentCollisionPadding(UIntPtr missionPointer);

		// Token: 0x06001856 RID: 6230
		[EngineMethod("set_mission_corpse_fade_out_time_in_seconds", false, null, false)]
		void SetMissionCorpseFadeOutTimeInSeconds(UIntPtr missionPointer, float corpseFadeOutTimeInSeconds);

		// Token: 0x06001857 RID: 6231
		[EngineMethod("set_report_stuck_agents_mode", false, null, false)]
		void SetReportStuckAgentsMode(UIntPtr missionPointer, bool value);

		// Token: 0x06001858 RID: 6232
		[EngineMethod("batch_formation_unit_positions", false, null, false)]
		void BatchFormationUnitPositions(UIntPtr missionPointer, Vec2i[] orderedPositionIndices, Vec2[] orderedLocalPositions, int[] availabilityTable, WorldPosition[] globalPositionTable, WorldPosition orderPosition, Vec2 direction, int fileCount, int rankCount, bool fastCheckWithSameFaceGroupIdDigit);

		// Token: 0x06001859 RID: 6233
		[EngineMethod("get_fall_avoid_system_active", false, null, false)]
		bool GetFallAvoidSystemActive(UIntPtr missionPointer);

		// Token: 0x0600185A RID: 6234
		[EngineMethod("set_fall_avoid_system_active", false, null, false)]
		void SetFallAvoidSystemActive(UIntPtr missionPointer, bool fallAvoidActive);

		// Token: 0x0600185B RID: 6235
		[EngineMethod("get_water_level_at_position", false, null, false)]
		float GetWaterLevelAtPosition(UIntPtr missionPointer, Vec2 position, bool useWaterRenderer);

		// Token: 0x0600185C RID: 6236
		[EngineMethod("find_convex_hull", false, null, false)]
		void FindConvexHull(Vec2[] boundaryPoints, int boundaryPointCount, ref int convexPointCount);

		// Token: 0x0600185D RID: 6237
		[EngineMethod("on_fast_forward_state_changed", false, null, false)]
		void OnFastForwardStateChanged(UIntPtr missionPointer, bool state);

		// Token: 0x0600185E RID: 6238
		[EngineMethod("get_current_volume_generator_version", false, null, false)]
		int GetCurrentVolumeGeneratorVersion();
	}
}
