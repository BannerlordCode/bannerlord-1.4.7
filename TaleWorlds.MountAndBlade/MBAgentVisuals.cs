using System;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200019D RID: 413
	[EngineClass("Agent_visuals")]
	public sealed class MBAgentVisuals : NativeObject
	{
		// Token: 0x060015F4 RID: 5620 RVA: 0x00051626 File Offset: 0x0004F826
		internal MBAgentVisuals(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x00051635 File Offset: 0x0004F835
		private UIntPtr GetPtr()
		{
			return base.Pointer;
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0005163D File Offset: 0x0004F83D
		public static MBAgentVisuals CreateAgentVisuals(Scene scene, string ownerName, Vec3 eyeOffset)
		{
			return MBAPI.IMBAgentVisuals.CreateAgentVisuals(scene.Pointer, ownerName, eyeOffset);
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x00051651 File Offset: 0x0004F851
		public void Tick(MBAgentVisuals parentAgentVisuals, float dt, bool entityMoving, float speed)
		{
			MBAPI.IMBAgentVisuals.Tick(this.GetPtr(), (parentAgentVisuals != null) ? parentAgentVisuals.GetPtr() : UIntPtr.Zero, dt, entityMoving, speed);
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00051678 File Offset: 0x0004F878
		public MatrixFrame GetGlobalFrame()
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			MBAPI.IMBAgentVisuals.GetGlobalFrame(this.GetPtr(), ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x000516A0 File Offset: 0x0004F8A0
		public MatrixFrame GetFrame()
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			MBAPI.IMBAgentVisuals.GetFrame(this.GetPtr(), ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x000516C8 File Offset: 0x0004F8C8
		public GameEntity GetEntity()
		{
			return MBAPI.IMBAgentVisuals.GetEntity(this.GetPtr());
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x000516DA File Offset: 0x0004F8DA
		public WeakGameEntity GetWeakEntity()
		{
			return new WeakGameEntity(MBAPI.IMBAgentVisuals.GetEntityPointer(this.GetPtr()));
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x000516F1 File Offset: 0x0004F8F1
		public bool IsValid()
		{
			return MBAPI.IMBAgentVisuals.IsValid(this.GetPtr());
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00051703 File Offset: 0x0004F903
		public Vec3 GetGlobalStableEyePoint(bool isHumanoid)
		{
			return MBAPI.IMBAgentVisuals.GetGlobalStableEyePoint(this.GetPtr(), isHumanoid);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00051716 File Offset: 0x0004F916
		public Vec3 GetGlobalStableNeckPoint(bool isHumanoid)
		{
			return MBAPI.IMBAgentVisuals.GetGlobalStableNeckPoint(this.GetPtr(), isHumanoid);
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0005172C File Offset: 0x0004F92C
		public MatrixFrame GetBoneEntitialFrame(sbyte bone, bool useBoneMapping)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			MBAPI.IMBAgentVisuals.GetBoneEntitialFrame(base.Pointer, bone, useBoneMapping, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00051756 File Offset: 0x0004F956
		public void SetAttachedPositionForMeshAfterAnimationPostIntegrate(WeakGameEntity ropeEntity, sbyte bone)
		{
			MBAPI.IMBAgentVisuals.SetAttachedPositionForRopeEntityAfterAnimationPostIntegrate(base.Pointer, ropeEntity.Pointer, bone);
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x00051770 File Offset: 0x0004F970
		public Vec3 GetCurrentHeadLookDirection()
		{
			return MBAPI.IMBAgentVisuals.GetCurrentHeadLookDirection(base.Pointer);
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x00051782 File Offset: 0x0004F982
		public HumanWalkingMovementMode GetMovementMode()
		{
			return (HumanWalkingMovementMode)MBAPI.IMBAgentVisuals.GetMovementMode(base.Pointer);
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00051794 File Offset: 0x0004F994
		public float GetVisualStrengthOfAgentVisual(MBAgentVisuals targetAgentVisual, Mission mission, float ambientLightStrength, float sunMoonLightStrength, int agentIndexToIgnore)
		{
			return MBAPI.IMBAgentVisuals.GetVisualStrengthOfAgentVisual(base.Pointer, targetAgentVisual.Pointer, mission.Pointer, ambientLightStrength, sunMoonLightStrength, agentIndexToIgnore);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x000517B7 File Offset: 0x0004F9B7
		public RagdollState GetCurrentRagdollState()
		{
			return MBAPI.IMBAgentVisuals.GetCurrentRagdollState(base.Pointer);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x000517C9 File Offset: 0x0004F9C9
		public sbyte GetRealBoneIndex(HumanBone boneType)
		{
			return MBAPI.IMBAgentVisuals.GetRealBoneIndex(this.GetPtr(), boneType);
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x000517DC File Offset: 0x0004F9DC
		public CompositeComponent AddPrefabToAgentVisualBoneByBoneType(string prefabName, HumanBone boneType)
		{
			return MBAPI.IMBAgentVisuals.AddPrefabToAgentVisualBoneByBoneType(this.GetPtr(), prefabName, boneType);
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x000517F0 File Offset: 0x0004F9F0
		public CompositeComponent AddPrefabToAgentVisualBoneByRealBoneIndex(string prefabName, sbyte realBoneIndex)
		{
			return MBAPI.IMBAgentVisuals.AddPrefabToAgentVisualBoneByRealBoneIndex(this.GetPtr(), prefabName, realBoneIndex);
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x00051804 File Offset: 0x0004FA04
		public GameEntity GetAttachedWeaponEntity(int attachedWeaponIndex)
		{
			return MBAPI.IMBAgentVisuals.GetAttachedWeaponEntity(this.GetPtr(), attachedWeaponIndex);
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x00051817 File Offset: 0x0004FA17
		public void SetFrame(ref MatrixFrame frame)
		{
			MBAPI.IMBAgentVisuals.SetFrame(this.GetPtr(), ref frame);
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0005182A File Offset: 0x0004FA2A
		public void SetEntity(GameEntity value)
		{
			MBAPI.IMBAgentVisuals.SetEntity(this.GetPtr(), value.Pointer);
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00051842 File Offset: 0x0004FA42
		public static void FillEntityWithBodyMeshesWithoutAgentVisuals(GameEntity entity, SkinGenerationParams skinParams, BodyProperties bodyProperties, MetaMesh glovesMesh)
		{
			MBAPI.IMBAgentVisuals.FillEntityWithBodyMeshesWithoutAgentVisuals(entity.Pointer, ref skinParams, ref bodyProperties, glovesMesh);
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x0005185C File Offset: 0x0004FA5C
		public BoneBodyTypeData GetBoneTypeData(sbyte boneIndex)
		{
			BoneBodyTypeData boneBodyTypeData = default(BoneBodyTypeData);
			MBAPI.IMBAgentVisuals.GetBoneTypeData(base.Pointer, boneIndex, ref boneBodyTypeData);
			return boneBodyTypeData;
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00051885 File Offset: 0x0004FA85
		public Skeleton GetSkeleton()
		{
			return MBAPI.IMBAgentVisuals.GetSkeleton(this.GetPtr());
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00051897 File Offset: 0x0004FA97
		public void SetSkeleton(Skeleton newSkeleton)
		{
			MBAPI.IMBAgentVisuals.SetSkeleton(this.GetPtr(), newSkeleton.Pointer);
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x000518B0 File Offset: 0x0004FAB0
		public void CreateParticleSystemAttachedToBone(string particleName, sbyte boneIndex, ref MatrixFrame boneLocalParticleFrame)
		{
			int runtimeIdByName = ParticleSystemManager.GetRuntimeIdByName(particleName);
			this.CreateParticleSystemAttachedToBone(runtimeIdByName, boneIndex, ref boneLocalParticleFrame);
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x000518CD File Offset: 0x0004FACD
		public void CreateParticleSystemAttachedToBone(int runtimeParticleindex, sbyte boneIndex, ref MatrixFrame boneLocalParticleFrame)
		{
			MBAPI.IMBAgentVisuals.CreateParticleSystemAttachedToBone(this.GetPtr(), runtimeParticleindex, boneIndex, ref boneLocalParticleFrame);
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x000518E2 File Offset: 0x0004FAE2
		public void SetVisible(bool value)
		{
			MBAPI.IMBAgentVisuals.SetVisible(this.GetPtr(), value);
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x000518F5 File Offset: 0x0004FAF5
		public bool GetVisible()
		{
			return MBAPI.IMBAgentVisuals.GetVisible(this.GetPtr());
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00051907 File Offset: 0x0004FB07
		public void AddChildEntity(GameEntity entity)
		{
			MBAPI.IMBAgentVisuals.AddChildEntity(this.GetPtr(), entity.Pointer);
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00051920 File Offset: 0x0004FB20
		public void SetClothWindToWeaponAtIndex(Vec3 windVector, bool isLocal, EquipmentIndex weaponIndex)
		{
			MBAPI.IMBAgentVisuals.SetClothWindToWeaponAtIndex(this.GetPtr(), windVector, isLocal, (int)weaponIndex);
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00051935 File Offset: 0x0004FB35
		public void RemoveChildEntity(GameEntity entity, int removeReason)
		{
			MBAPI.IMBAgentVisuals.RemoveChildEntity(this.GetPtr(), entity.Pointer, removeReason);
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0005194E File Offset: 0x0004FB4E
		public bool CheckResources(bool addToQueue)
		{
			return MBAPI.IMBAgentVisuals.CheckResources(this.GetPtr(), addToQueue);
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00051961 File Offset: 0x0004FB61
		public void AddSkinMeshes(SkinGenerationParams skinParams, BodyProperties bodyProperties, bool useGPUMorph, bool useFaceCache)
		{
			MBAPI.IMBAgentVisuals.AddSkinMeshesToAgentEntity(this.GetPtr(), ref skinParams, ref bodyProperties, useGPUMorph, useFaceCache);
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x0005197A File Offset: 0x0004FB7A
		public void SetFaceGenerationParams(FaceGenerationParams faceGenerationParams)
		{
			MBAPI.IMBAgentVisuals.SetFaceGenerationParams(this.GetPtr(), faceGenerationParams);
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x0005198D File Offset: 0x0004FB8D
		public void SetLodAtlasShadingIndex(int index, bool useTeamColor, uint teamColor1, uint teamColor2)
		{
			MBAPI.IMBAgentVisuals.SetLodAtlasShadingIndex(this.GetPtr(), index, useTeamColor, teamColor1, teamColor2);
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x000519A4 File Offset: 0x0004FBA4
		public void ClearVisualComponents(bool removeSkeleton, bool removeLabel = true)
		{
			MBAPI.IMBAgentVisuals.ClearVisualComponents(this.GetPtr(), removeSkeleton, removeLabel);
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x000519B8 File Offset: 0x0004FBB8
		public void LazyUpdateAgentRendererData()
		{
			MBAPI.IMBAgentVisuals.LazyUpdateAgentRendererData(this.GetPtr());
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x000519CA File Offset: 0x0004FBCA
		public void AddMultiMesh(MetaMesh metaMesh, BodyMeshTypes bodyMeshIndex)
		{
			MBAPI.IMBAgentVisuals.AddMultiMesh(this.GetPtr(), metaMesh.Pointer, (int)bodyMeshIndex);
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x000519E3 File Offset: 0x0004FBE3
		public void ApplySkeletonScale(Vec3 mountSitBoneScale, float mountRadiusAdder, sbyte[] boneIndices, Vec3[] boneScales)
		{
			MBAPI.IMBAgentVisuals.ApplySkeletonScale(base.Pointer, mountSitBoneScale, mountRadiusAdder, (byte)boneIndices.Length, boneIndices, boneScales);
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x000519FE File Offset: 0x0004FBFE
		public void UpdateSkeletonScale(int bodyDeformType)
		{
			MBAPI.IMBAgentVisuals.UpdateSkeletonScale(base.Pointer, bodyDeformType);
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00051A11 File Offset: 0x0004FC11
		public void AddHorseReinsClothMesh(MetaMesh reinMesh, MetaMesh ropeMesh)
		{
			MBAPI.IMBAgentVisuals.AddHorseReinsClothMesh(base.Pointer, reinMesh.Pointer, ropeMesh.Pointer);
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00051A2F File Offset: 0x0004FC2F
		public void BatchLastLodMeshes()
		{
			MBAPI.IMBAgentVisuals.BatchLastLodMeshes(this.GetPtr());
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00051A44 File Offset: 0x0004FC44
		public void AddWeaponToAgentEntity(int slotIndex, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, in WeaponData ammoWeaponData, WeaponStatsData[] ammoWeaponStatsData, GameEntity cachedEntity)
		{
			MBAPI.IMBAgentVisuals.AddWeaponToAgentEntity(this.GetPtr(), slotIndex, in weaponData, weaponStatsData, weaponStatsData.Length, in ammoWeaponData, ammoWeaponStatsData, ammoWeaponStatsData.Length, cachedEntity);
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00051A71 File Offset: 0x0004FC71
		public void UpdateQuiverMeshesWithoutAgent(int weaponIndex, int ammoCount)
		{
			MBAPI.IMBAgentVisuals.UpdateQuiverMeshesWithoutAgent(this.GetPtr(), weaponIndex, ammoCount);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00051A85 File Offset: 0x0004FC85
		public void SetWieldedWeaponIndices(int slotIndexRightHand, int slotIndexLeftHand)
		{
			MBAPI.IMBAgentVisuals.SetWieldedWeaponIndices(this.GetPtr(), slotIndexRightHand, slotIndexLeftHand);
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00051A99 File Offset: 0x0004FC99
		public void ClearAllWeaponMeshes()
		{
			MBAPI.IMBAgentVisuals.ClearAllWeaponMeshes(this.GetPtr());
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00051AAB File Offset: 0x0004FCAB
		public void ClearWeaponMeshes(EquipmentIndex index)
		{
			MBAPI.IMBAgentVisuals.ClearWeaponMeshes(this.GetPtr(), (int)index);
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00051ABE File Offset: 0x0004FCBE
		public void MakeVoice(int voiceId, Vec3 position)
		{
			MBAPI.IMBAgentVisuals.MakeVoice(this.GetPtr(), voiceId, ref position);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00051AD3 File Offset: 0x0004FCD3
		public void SetSetupMorphNode(bool value)
		{
			MBAPI.IMBAgentVisuals.SetSetupMorphNode(this.GetPtr(), value);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00051AE6 File Offset: 0x0004FCE6
		public void UseScaledWeapons(bool value)
		{
			MBAPI.IMBAgentVisuals.UseScaledWeapons(this.GetPtr(), value);
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00051AF9 File Offset: 0x0004FCF9
		public void SetClothComponentKeepStateOfAllMeshes(bool keepState)
		{
			MBAPI.IMBAgentVisuals.SetClothComponentKeepStateOfAllMeshes(this.GetPtr(), keepState);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00051B0C File Offset: 0x0004FD0C
		public MatrixFrame GetFacegenScalingMatrix()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			Vec3 currentHelmetScalingFactor = MBAPI.IMBAgentVisuals.GetCurrentHelmetScalingFactor(this.GetPtr());
			identity.rotation.ApplyScaleLocal(in currentHelmetScalingFactor);
			return identity;
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00051B40 File Offset: 0x0004FD40
		public void ReplaceMeshWithMesh(MetaMesh oldMetaMesh, MetaMesh newMetaMesh, BodyMeshTypes bodyMeshIndex)
		{
			if (oldMetaMesh != null)
			{
				MBAPI.IMBAgentVisuals.RemoveMultiMesh(this.GetPtr(), oldMetaMesh.Pointer, (int)bodyMeshIndex);
			}
			if (newMetaMesh != null)
			{
				MBAPI.IMBAgentVisuals.AddMultiMesh(this.GetPtr(), newMetaMesh.Pointer, (int)bodyMeshIndex);
			}
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00051B8D File Offset: 0x0004FD8D
		public void SetAgentActionChannel(int actionChannelNo, int actionIndex, float channelParameter = 0f, float blendPeriodOverride = -0.2f, bool forceFaceMorphRestart = true, float blendWithNextActionFactor = 0f)
		{
			MBAPI.IMBSkeletonExtensions.SetAgentActionChannel(this.GetSkeleton().Pointer, actionChannelNo, actionIndex, channelParameter, blendPeriodOverride, forceFaceMorphRestart, blendWithNextActionFactor);
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00051BAD File Offset: 0x0004FDAD
		public void SetVoiceDefinitionIndex(int voiceDefinitionIndex, float voicePitch)
		{
			MBAPI.IMBAgentVisuals.SetVoiceDefinitionIndex(this.GetPtr(), voiceDefinitionIndex, voicePitch);
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x00051BC1 File Offset: 0x0004FDC1
		public void StartRhubarbRecord(string path, int soundId)
		{
			MBAPI.IMBAgentVisuals.StartRhubarbRecord(this.GetPtr(), path, soundId);
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x00051BD8 File Offset: 0x0004FDD8
		public void SetContourColor(uint? color, bool alwaysVisible = true)
		{
			if (color != null)
			{
				MBAPI.IMBAgentVisuals.SetAsContourEntity(this.GetPtr(), color.Value);
				MBAPI.IMBAgentVisuals.SetContourState(this.GetPtr(), alwaysVisible);
				return;
			}
			MBAPI.IMBAgentVisuals.DisableContour(this.GetPtr());
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x00051C27 File Offset: 0x0004FE27
		public void SetEnableOcclusionCulling(bool enable)
		{
			MBAPI.IMBAgentVisuals.SetEnableOcclusionCulling(this.GetPtr(), enable);
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x00051C3A File Offset: 0x0004FE3A
		public void SetAgentLodZeroOrMax(bool makeZero)
		{
			MBAPI.IMBAgentVisuals.SetAgentLodMakeZeroOrMax(this.GetPtr(), makeZero);
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00051C4D File Offset: 0x0004FE4D
		public void SetAgentLocalSpeed(Vec2 speed)
		{
			MBAPI.IMBAgentVisuals.SetAgentLocalSpeed(this.GetPtr(), speed);
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00051C60 File Offset: 0x0004FE60
		public void SetLookDirection(Vec3 direction)
		{
			MBAPI.IMBAgentVisuals.SetLookDirection(this.GetPtr(), direction);
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x00051C74 File Offset: 0x0004FE74
		public static BodyMeshTypes GetBodyMeshIndex(EquipmentIndex equipmentIndex)
		{
			switch (equipmentIndex)
			{
			case EquipmentIndex.NumAllWeaponSlots:
				return BodyMeshTypes.Cap;
			case EquipmentIndex.Body:
				return BodyMeshTypes.Chestpiece;
			case EquipmentIndex.Leg:
				return BodyMeshTypes.Footwear;
			case EquipmentIndex.Gloves:
				return BodyMeshTypes.Gloves;
			case EquipmentIndex.Cape:
				return BodyMeshTypes.Shoulderpiece;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Base\\MBAgentVisuals.cs", "GetBodyMeshIndex", 434);
				return BodyMeshTypes.Invalid;
			}
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00051CC6 File Offset: 0x0004FEC6
		public MatrixFrame GetBoneEntitialFrameAtAnimationProgress(sbyte boneIndex, int animationIndex, float progress)
		{
			return MBAPI.IMBAgentVisuals.GetBoneEntitialFrameAtAnimationProgress(this.GetPtr(), boneIndex, animationIndex, progress);
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00051CDB File Offset: 0x0004FEDB
		public void Reset()
		{
			MBAPI.IMBAgentVisuals.Reset(this.GetPtr());
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x00051CED File Offset: 0x0004FEED
		public void ResetNextFrame()
		{
			MBAPI.IMBAgentVisuals.ResetNextFrame(this.GetPtr());
		}
	}
}
