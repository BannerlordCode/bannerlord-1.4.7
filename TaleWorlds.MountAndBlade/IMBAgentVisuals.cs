using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A5 RID: 421
	[ScriptingInterfaceBase]
	internal interface IMBAgentVisuals
	{
		// Token: 0x06001671 RID: 5745
		[EngineMethod("validate_agent_visuals_reseted", false, null, false)]
		void ValidateAgentVisualsReseted(UIntPtr scenePointer, UIntPtr agentRendererSceneControllerPointer);

		// Token: 0x06001672 RID: 5746
		[EngineMethod("create_agent_renderer_scene_controller", false, null, false)]
		UIntPtr CreateAgentRendererSceneController(UIntPtr scenePointer);

		// Token: 0x06001673 RID: 5747
		[EngineMethod("destruct_agent_renderer_scene_controller", false, null, false)]
		void DestructAgentRendererSceneController(UIntPtr scenePointer, UIntPtr agentRendererSceneControllerPointer, bool deleteThisFrame);

		// Token: 0x06001674 RID: 5748
		[EngineMethod("set_do_timer_based_skeleton_forced_updates", false, null, false)]
		void SetDoTimerBasedForcedSkeletonUpdates(UIntPtr agentRendererSceneControllerPointer, bool value);

		// Token: 0x06001675 RID: 5749
		[EngineMethod("set_enforced_visibility_for_all_agents", false, null, false)]
		void SetEnforcedVisibilityForAllAgents(UIntPtr scenePointer, UIntPtr agentRendererSceneControllerPointer);

		// Token: 0x06001676 RID: 5750
		[EngineMethod("create_agent_visuals", false, null, false)]
		MBAgentVisuals CreateAgentVisuals(UIntPtr scenePtr, string ownerName, Vec3 eyeOffset);

		// Token: 0x06001677 RID: 5751
		[EngineMethod("tick", false, null, false)]
		void Tick(UIntPtr agentVisualsId, UIntPtr parentAgentVisualsId, float dt, bool entityMoving, float speed);

		// Token: 0x06001678 RID: 5752
		[EngineMethod("set_entity", false, null, false)]
		void SetEntity(UIntPtr agentVisualsId, UIntPtr entityPtr);

		// Token: 0x06001679 RID: 5753
		[EngineMethod("set_skeleton", false, null, false)]
		void SetSkeleton(UIntPtr agentVisualsId, UIntPtr skeletonPtr);

		// Token: 0x0600167A RID: 5754
		[EngineMethod("fill_entity_with_body_meshes_without_agent_visuals", false, null, false)]
		void FillEntityWithBodyMeshesWithoutAgentVisuals(UIntPtr entityPoinbter, ref SkinGenerationParams skinParams, ref BodyProperties bodyProperties, MetaMesh glovesMesh);

		// Token: 0x0600167B RID: 5755
		[EngineMethod("add_skin_meshes_to_agent_visuals", false, null, false)]
		void AddSkinMeshesToAgentEntity(UIntPtr agentVisualsId, ref SkinGenerationParams skinParams, ref BodyProperties bodyProperties, bool useGPUMorph, bool useFaceCache);

		// Token: 0x0600167C RID: 5756
		[EngineMethod("set_lod_atlas_shading_index", false, null, false)]
		void SetLodAtlasShadingIndex(UIntPtr agentVisualsId, int index, bool useTeamColor, uint teamColor1, uint teamColor2);

		// Token: 0x0600167D RID: 5757
		[EngineMethod("set_face_generation_params", false, null, false)]
		void SetFaceGenerationParams(UIntPtr agentVisualsId, FaceGenerationParams faceGenerationParams);

		// Token: 0x0600167E RID: 5758
		[EngineMethod("start_rhubarb_record", false, null, false)]
		void StartRhubarbRecord(UIntPtr agentVisualsId, string path, int soundId);

		// Token: 0x0600167F RID: 5759
		[EngineMethod("clear_visual_components", false, null, false)]
		void ClearVisualComponents(UIntPtr agentVisualsId, bool removeSkeleton, bool removeLabel);

		// Token: 0x06001680 RID: 5760
		[EngineMethod("lazy_update_agent_renderer_data", false, null, false)]
		void LazyUpdateAgentRendererData(UIntPtr agentVisualsId);

		// Token: 0x06001681 RID: 5761
		[EngineMethod("add_mesh", false, null, false)]
		void AddMesh(UIntPtr agentVisualsId, UIntPtr meshPointer);

		// Token: 0x06001682 RID: 5762
		[EngineMethod("remove_mesh", false, null, false)]
		void RemoveMesh(UIntPtr agentVisualsPtr, UIntPtr meshPointer);

		// Token: 0x06001683 RID: 5763
		[EngineMethod("add_multi_mesh", false, null, false)]
		void AddMultiMesh(UIntPtr agentVisualsPtr, UIntPtr multiMeshPointer, int bodyMeshIndex);

		// Token: 0x06001684 RID: 5764
		[EngineMethod("add_horse_reins_cloth_mesh", false, null, false)]
		void AddHorseReinsClothMesh(UIntPtr agentVisualsPtr, UIntPtr reinMeshPointer, UIntPtr ropeMeshPointer);

		// Token: 0x06001685 RID: 5765
		[EngineMethod("update_skeleton_scale", false, null, false)]
		void UpdateSkeletonScale(UIntPtr agentVisualsId, int bodyDeformType);

		// Token: 0x06001686 RID: 5766
		[EngineMethod("apply_skeleton_scale", false, null, false)]
		void ApplySkeletonScale(UIntPtr agentVisualsId, Vec3 mountSitBoneScale, float mountRadiusAdder, byte boneCount, sbyte[] boneIndices, Vec3[] boneScales);

		// Token: 0x06001687 RID: 5767
		[EngineMethod("batch_last_lod_meshes", false, null, false)]
		void BatchLastLodMeshes(UIntPtr agentVisualsPtr);

		// Token: 0x06001688 RID: 5768
		[EngineMethod("remove_multi_mesh", false, null, false)]
		void RemoveMultiMesh(UIntPtr agentVisualsPtr, UIntPtr multiMeshPointer, int bodyMeshIndex);

		// Token: 0x06001689 RID: 5769
		[EngineMethod("add_weapon_to_agent_entity", false, null, false)]
		void AddWeaponToAgentEntity(UIntPtr agentVisualsPtr, int slotIndex, in WeaponData agentEntityData, WeaponStatsData[] weaponStatsData, int weaponStatsDataLength, in WeaponData agentEntityAmmoData, WeaponStatsData[] ammoWeaponStatsData, int ammoWeaponStatsDataLength, GameEntity cachedEntity);

		// Token: 0x0600168A RID: 5770
		[EngineMethod("update_quiver_mesh_of_weapon_in_slot", false, null, false)]
		void UpdateQuiverMeshesWithoutAgent(UIntPtr agentVisualsId, int weaponIndex, int ammoCountToShow);

		// Token: 0x0600168B RID: 5771
		[EngineMethod("set_wielded_weapon_indices", false, null, false)]
		void SetWieldedWeaponIndices(UIntPtr agentVisualsId, int slotIndexRightHand, int slotIndexLeftHand);

		// Token: 0x0600168C RID: 5772
		[EngineMethod("clear_all_weapon_meshes", false, null, false)]
		void ClearAllWeaponMeshes(UIntPtr agentVisualsPtr);

		// Token: 0x0600168D RID: 5773
		[EngineMethod("clear_weapon_meshes", false, null, false)]
		void ClearWeaponMeshes(UIntPtr agentVisualsPtr, int weaponVisualIndex);

		// Token: 0x0600168E RID: 5774
		[EngineMethod("make_voice", false, null, false)]
		void MakeVoice(UIntPtr agentVisualsPtr, int voiceId, ref Vec3 position);

		// Token: 0x0600168F RID: 5775
		[EngineMethod("set_setup_morph_node", false, null, false)]
		void SetSetupMorphNode(UIntPtr agentVisualsPtr, bool value);

		// Token: 0x06001690 RID: 5776
		[EngineMethod("use_scaled_weapons", false, null, false)]
		void UseScaledWeapons(UIntPtr agentVisualsPtr, bool value);

		// Token: 0x06001691 RID: 5777
		[EngineMethod("set_cloth_component_keep_state_of_all_meshes", false, null, false)]
		void SetClothComponentKeepStateOfAllMeshes(UIntPtr agentVisualsPtr, bool keepState);

		// Token: 0x06001692 RID: 5778
		[EngineMethod("get_current_helmet_scaling_factor", false, null, false)]
		Vec3 GetCurrentHelmetScalingFactor(UIntPtr agentVisualsPtr);

		// Token: 0x06001693 RID: 5779
		[EngineMethod("set_voice_definition_index", false, null, false)]
		void SetVoiceDefinitionIndex(UIntPtr agentVisualsPtr, int voiceDefinitionIndex, float voicePitch);

		// Token: 0x06001694 RID: 5780
		[EngineMethod("set_agent_lod_make_zero_or_max", false, null, false)]
		void SetAgentLodMakeZeroOrMax(UIntPtr agentVisualsPtr, bool makeZero);

		// Token: 0x06001695 RID: 5781
		[EngineMethod("set_agent_local_speed", false, null, false)]
		void SetAgentLocalSpeed(UIntPtr agentVisualsPtr, Vec2 speed);

		// Token: 0x06001696 RID: 5782
		[EngineMethod("set_look_direction", false, null, false)]
		void SetLookDirection(UIntPtr agentVisualsPtr, Vec3 direction);

		// Token: 0x06001697 RID: 5783
		[EngineMethod("get_bone_entitial_frame_at_animation_progress", false, null, true)]
		MatrixFrame GetBoneEntitialFrameAtAnimationProgress(UIntPtr agentVisualsPtr, sbyte boneIndex, int animationIndex, float progress);

		// Token: 0x06001698 RID: 5784
		[EngineMethod("reset", false, null, false)]
		void Reset(UIntPtr agentVisualsPtr);

		// Token: 0x06001699 RID: 5785
		[EngineMethod("reset_next_frame", false, null, false)]
		void ResetNextFrame(UIntPtr agentVisualsPtr);

		// Token: 0x0600169A RID: 5786
		[EngineMethod("set_frame", false, null, false)]
		void SetFrame(UIntPtr agentVisualsPtr, ref MatrixFrame frame);

		// Token: 0x0600169B RID: 5787
		[EngineMethod("get_frame", false, null, true)]
		void GetFrame(UIntPtr agentVisualsPtr, ref MatrixFrame outFrame);

		// Token: 0x0600169C RID: 5788
		[EngineMethod("get_global_frame", false, null, true)]
		void GetGlobalFrame(UIntPtr agentVisualsPtr, ref MatrixFrame outFrame);

		// Token: 0x0600169D RID: 5789
		[EngineMethod("set_visible", false, null, false)]
		void SetVisible(UIntPtr agentVisualsPtr, bool value);

		// Token: 0x0600169E RID: 5790
		[EngineMethod("get_visible", false, null, false)]
		bool GetVisible(UIntPtr agentVisualsPtr);

		// Token: 0x0600169F RID: 5791
		[EngineMethod("get_skeleton", false, null, false)]
		Skeleton GetSkeleton(UIntPtr agentVisualsPtr);

		// Token: 0x060016A0 RID: 5792
		[EngineMethod("get_entity", false, null, false)]
		GameEntity GetEntity(UIntPtr agentVisualsPtr);

		// Token: 0x060016A1 RID: 5793
		[EngineMethod("get_entity_pointer", false, null, false)]
		UIntPtr GetEntityPointer(UIntPtr agentVisualsPtr);

		// Token: 0x060016A2 RID: 5794
		[EngineMethod("is_valid", false, null, false)]
		bool IsValid(UIntPtr agentVisualsPtr);

		// Token: 0x060016A3 RID: 5795
		[EngineMethod("get_global_stable_eye_point", false, null, false)]
		Vec3 GetGlobalStableEyePoint(UIntPtr agentVisualsPtr, bool isHumanoid);

		// Token: 0x060016A4 RID: 5796
		[EngineMethod("get_global_stable_neck_point", false, null, false)]
		Vec3 GetGlobalStableNeckPoint(UIntPtr agentVisualsPtr, bool isHumanoid);

		// Token: 0x060016A5 RID: 5797
		[EngineMethod("get_quick_bone_entitial_frame", false, null, false)]
		void GetBoneEntitialFrame(UIntPtr agentVisualsPtr, sbyte bone, bool useBoneMapping, ref MatrixFrame outFrame);

		// Token: 0x060016A6 RID: 5798
		[EngineMethod("set_attached_position_for_rope_entity_after_animation_post_integrate", false, null, false)]
		void SetAttachedPositionForRopeEntityAfterAnimationPostIntegrate(UIntPtr agentVisualsPtr, UIntPtr ropeEntity, sbyte bone);

		// Token: 0x060016A7 RID: 5799
		[EngineMethod("get_current_head_look_direction", false, null, false)]
		Vec3 GetCurrentHeadLookDirection(UIntPtr agentVisualsPtr);

		// Token: 0x060016A8 RID: 5800
		[EngineMethod("get_current_ragdoll_state", false, null, false)]
		RagdollState GetCurrentRagdollState(UIntPtr agentVisualsPtr);

		// Token: 0x060016A9 RID: 5801
		[EngineMethod("get_real_bone_index", false, null, false)]
		sbyte GetRealBoneIndex(UIntPtr agentVisualsPtr, HumanBone boneType);

		// Token: 0x060016AA RID: 5802
		[EngineMethod("add_prefab_to_agent_visual_bone_by_bone_type", false, null, false)]
		CompositeComponent AddPrefabToAgentVisualBoneByBoneType(UIntPtr agentVisualsPtr, string prefabName, HumanBone boneType);

		// Token: 0x060016AB RID: 5803
		[EngineMethod("add_prefab_to_agent_visual_bone_by_real_bone_index", false, null, false)]
		CompositeComponent AddPrefabToAgentVisualBoneByRealBoneIndex(UIntPtr agentVisualsPtr, string prefabName, sbyte realBoneIndex);

		// Token: 0x060016AC RID: 5804
		[EngineMethod("get_attached_weapon_entity", false, null, false)]
		GameEntity GetAttachedWeaponEntity(UIntPtr agentVisualsPtr, int attachedWeaponIndex);

		// Token: 0x060016AD RID: 5805
		[EngineMethod("create_particle_system_attached_to_bone", false, null, false)]
		void CreateParticleSystemAttachedToBone(UIntPtr agentVisualsPtr, int runtimeParticleindex, sbyte boneIndex, ref MatrixFrame boneLocalParticleFrame);

		// Token: 0x060016AE RID: 5806
		[EngineMethod("check_resources", false, null, false)]
		bool CheckResources(UIntPtr agentVisualsPtr, bool addToQueue);

		// Token: 0x060016AF RID: 5807
		[EngineMethod("add_child_entity", false, null, false)]
		bool AddChildEntity(UIntPtr agentVisualsPtr, UIntPtr EntityId);

		// Token: 0x060016B0 RID: 5808
		[EngineMethod("set_cloth_wind_to_weapon_at_index", false, null, false)]
		void SetClothWindToWeaponAtIndex(UIntPtr agentVisualsPtr, Vec3 windVector, bool isLocal, int index);

		// Token: 0x060016B1 RID: 5809
		[EngineMethod("remove_child_entity", false, null, false)]
		void RemoveChildEntity(UIntPtr agentVisualsPtr, UIntPtr EntityId, int removeReason);

		// Token: 0x060016B2 RID: 5810
		[EngineMethod("disable_contour", false, null, false)]
		void DisableContour(UIntPtr agentVisualsPtr);

		// Token: 0x060016B3 RID: 5811
		[EngineMethod("set_as_contour_entity", false, null, false)]
		void SetAsContourEntity(UIntPtr agentVisualsPtr, uint color);

		// Token: 0x060016B4 RID: 5812
		[EngineMethod("set_contour_state", false, null, false)]
		void SetContourState(UIntPtr agentVisualsPtr, bool alwaysVisible);

		// Token: 0x060016B5 RID: 5813
		[EngineMethod("set_enable_occlusion_culling", false, null, false)]
		void SetEnableOcclusionCulling(UIntPtr agentVisualsPtr, bool enable);

		// Token: 0x060016B6 RID: 5814
		[EngineMethod("get_bone_type_data", false, null, false)]
		void GetBoneTypeData(UIntPtr pointer, sbyte boneIndex, ref BoneBodyTypeData boneBodyTypeData);

		// Token: 0x060016B7 RID: 5815
		[EngineMethod("get_movement_mode", false, null, false)]
		int GetMovementMode(UIntPtr agentVisualsPtr);

		// Token: 0x060016B8 RID: 5816
		[EngineMethod("get_visual_strength_of_agent_visual", false, null, false)]
		float GetVisualStrengthOfAgentVisual(UIntPtr agentVisualsPtr, UIntPtr targetagentVisualsPtr, UIntPtr missionPointer, float ambientLightStrength, float sunMoonLightStrength, int agentIndexToIgnore);
	}
}
