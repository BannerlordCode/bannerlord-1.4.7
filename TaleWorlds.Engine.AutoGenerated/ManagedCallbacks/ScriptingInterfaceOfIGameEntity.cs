using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000011 RID: 17
	internal class ScriptingInterfaceOfIGameEntity : IGameEntity
	{
		// Token: 0x06000127 RID: 295 RVA: 0x0001007E File Offset: 0x0000E27E
		public void ActivateRagdoll(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_ActivateRagdollDelegate(entityId);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0001008B File Offset: 0x0000E28B
		public void AddAllMeshesOfGameEntity(UIntPtr entityId, UIntPtr copiedEntityId)
		{
			ScriptingInterfaceOfIGameEntity.call_AddAllMeshesOfGameEntityDelegate(entityId, copiedEntityId);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0001009C File Offset: 0x0000E29C
		public void AddCapsuleAsBody(UIntPtr entityId, Vec3 p1, Vec3 p2, float radius, uint bodyFlags, string physicsMaterialName)
		{
			byte[] array = null;
			if (physicsMaterialName != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(physicsMaterialName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(physicsMaterialName, 0, physicsMaterialName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_AddCapsuleAsBodyDelegate(entityId, p1, p2, radius, bodyFlags, array);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00010101 File Offset: 0x0000E301
		public void AddChild(UIntPtr parententity, UIntPtr childentity, bool autoLocalizeFrame)
		{
			ScriptingInterfaceOfIGameEntity.call_AddChildDelegate(parententity, childentity, autoLocalizeFrame);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00010110 File Offset: 0x0000E310
		public void AddComponent(UIntPtr pointer, UIntPtr componentPointer)
		{
			ScriptingInterfaceOfIGameEntity.call_AddComponentDelegate(pointer, componentPointer);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0001011E File Offset: 0x0000E31E
		public UIntPtr AddDistanceJoint(UIntPtr entityId, UIntPtr otherEntityId, float minDistance, float maxDistance)
		{
			return ScriptingInterfaceOfIGameEntity.call_AddDistanceJointDelegate(entityId, otherEntityId, minDistance, maxDistance);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0001012F File Offset: 0x0000E32F
		public UIntPtr AddDistanceJointWithFrames(UIntPtr entityId, UIntPtr otherEntityId, MatrixFrame globalFrameOnA, MatrixFrame globalFrameOnB, float minDistance, float maxDistance)
		{
			return ScriptingInterfaceOfIGameEntity.call_AddDistanceJointWithFramesDelegate(entityId, otherEntityId, globalFrameOnA, globalFrameOnB, minDistance, maxDistance);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00010144 File Offset: 0x0000E344
		public void AddEditDataUserToAllMeshes(UIntPtr entityId, bool entity_components, bool skeleton_components)
		{
			ScriptingInterfaceOfIGameEntity.call_AddEditDataUserToAllMeshesDelegate(entityId, entity_components, skeleton_components);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00010153 File Offset: 0x0000E353
		public bool AddLight(UIntPtr entityId, UIntPtr lightPointer)
		{
			return ScriptingInterfaceOfIGameEntity.call_AddLightDelegate(entityId, lightPointer);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00010161 File Offset: 0x0000E361
		public void AddMesh(UIntPtr entityId, UIntPtr mesh, bool recomputeBoundingBox)
		{
			ScriptingInterfaceOfIGameEntity.call_AddMeshDelegate(entityId, mesh, recomputeBoundingBox);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00010170 File Offset: 0x0000E370
		public void AddMeshToBone(UIntPtr entityId, UIntPtr multiMeshPointer, sbyte boneIndex)
		{
			ScriptingInterfaceOfIGameEntity.call_AddMeshToBoneDelegate(entityId, multiMeshPointer, boneIndex);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0001017F File Offset: 0x0000E37F
		public void AddMultiMesh(UIntPtr entityId, UIntPtr multiMeshPtr, bool updateVisMask)
		{
			ScriptingInterfaceOfIGameEntity.call_AddMultiMeshDelegate(entityId, multiMeshPtr, updateVisMask);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0001018E File Offset: 0x0000E38E
		public void AddMultiMeshToSkeleton(UIntPtr gameEntity, UIntPtr multiMesh)
		{
			ScriptingInterfaceOfIGameEntity.call_AddMultiMeshToSkeletonDelegate(gameEntity, multiMesh);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0001019C File Offset: 0x0000E39C
		public void AddMultiMeshToSkeletonBone(UIntPtr gameEntity, UIntPtr multiMesh, sbyte boneIndex)
		{
			ScriptingInterfaceOfIGameEntity.call_AddMultiMeshToSkeletonBoneDelegate(gameEntity, multiMesh, boneIndex);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000101AC File Offset: 0x0000E3AC
		public void AddParticleSystemComponent(UIntPtr entityId, string particleid)
		{
			byte[] array = null;
			if (particleid != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(particleid);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(particleid, 0, particleid.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_AddParticleSystemComponentDelegate(entityId, array);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00010208 File Offset: 0x0000E408
		public void AddPhysics(UIntPtr entityId, UIntPtr body, float mass, ref Vec3 localCenterOfMass, ref Vec3 initialGlobalVelocity, ref Vec3 initialAngularGlobalVelocity, int physicsMaterial, bool isStatic, int collisionGroupID)
		{
			ScriptingInterfaceOfIGameEntity.call_AddPhysicsDelegate(entityId, body, mass, ref localCenterOfMass, ref initialGlobalVelocity, ref initialAngularGlobalVelocity, physicsMaterial, isStatic, collisionGroupID);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0001022E File Offset: 0x0000E42E
		public void AddSphereAsBody(UIntPtr entityId, Vec3 center, float radius, uint bodyFlags)
		{
			ScriptingInterfaceOfIGameEntity.call_AddSphereAsBodyDelegate(entityId, center, radius, bodyFlags);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0001023F File Offset: 0x0000E43F
		public void AddSplashPositionToWaterVisualRecord(UIntPtr entityPointer, UIntPtr visualPrefab, in Vec3 position)
		{
			ScriptingInterfaceOfIGameEntity.call_AddSplashPositionToWaterVisualRecordDelegate(entityPointer, visualPrefab, in position);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00010250 File Offset: 0x0000E450
		public void AddTag(UIntPtr entityId, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_AddTagDelegate(entityId, array);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000102AB File Offset: 0x0000E4AB
		public void ApplyAccelerationToDynamicBody(UIntPtr entityId, ref Vec3 acceleration)
		{
			ScriptingInterfaceOfIGameEntity.call_ApplyAccelerationToDynamicBodyDelegate(entityId, ref acceleration);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000102B9 File Offset: 0x0000E4B9
		public void ApplyForceToDynamicBody(UIntPtr entityId, ref Vec3 force, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			ScriptingInterfaceOfIGameEntity.call_ApplyForceToDynamicBodyDelegate(entityId, ref force, forceMode);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000102C8 File Offset: 0x0000E4C8
		public void ApplyGlobalForceAtLocalPosToDynamicBody(UIntPtr entityId, ref Vec3 localPosition, ref Vec3 globalForce, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			ScriptingInterfaceOfIGameEntity.call_ApplyGlobalForceAtLocalPosToDynamicBodyDelegate(entityId, ref localPosition, ref globalForce, forceMode);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000102D9 File Offset: 0x0000E4D9
		public void ApplyLocalForceAtLocalPosToDynamicBody(UIntPtr entityId, ref Vec3 localPosition, ref Vec3 localForce, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			ScriptingInterfaceOfIGameEntity.call_ApplyLocalForceAtLocalPosToDynamicBodyDelegate(entityId, ref localPosition, ref localForce, forceMode);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000102EA File Offset: 0x0000E4EA
		public void ApplyLocalImpulseToDynamicBody(UIntPtr entityId, ref Vec3 localPosition, ref Vec3 impulse)
		{
			ScriptingInterfaceOfIGameEntity.call_ApplyLocalImpulseToDynamicBodyDelegate(entityId, ref localPosition, ref impulse);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000102F9 File Offset: 0x0000E4F9
		public void ApplyTorqueToDynamicBody(UIntPtr entityId, ref Vec3 torque, GameEntityPhysicsExtensions.ForceMode forceMode)
		{
			ScriptingInterfaceOfIGameEntity.call_ApplyTorqueToDynamicBodyDelegate(entityId, ref torque, forceMode);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00010308 File Offset: 0x0000E508
		public void AttachNavigationMeshFaces(UIntPtr entityId, int faceGroupId, bool isConnected, bool isBlocker, bool autoLocalize, bool finalizeBlockerConvexHullComputation, bool updateEntityFrame)
		{
			ScriptingInterfaceOfIGameEntity.call_AttachNavigationMeshFacesDelegate(entityId, faceGroupId, isConnected, isBlocker, autoLocalize, finalizeBlockerConvexHullComputation, updateEntityFrame);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0001031F File Offset: 0x0000E51F
		public void BreakPrefab(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_BreakPrefabDelegate(entityId);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0001032C File Offset: 0x0000E52C
		public void BurstEntityParticle(UIntPtr entityId, bool doChildren)
		{
			ScriptingInterfaceOfIGameEntity.call_BurstEntityParticleDelegate(entityId, doChildren);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0001033A File Offset: 0x0000E53A
		public void CallScriptCallbacks(UIntPtr entityPointer, bool registerScriptComponents)
		{
			ScriptingInterfaceOfIGameEntity.call_CallScriptCallbacksDelegate(entityPointer, registerScriptComponents);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00010348 File Offset: 0x0000E548
		public void ChangeMetaMeshOrRemoveItIfNotExists(UIntPtr entityId, UIntPtr entityMetaMeshPointer, UIntPtr newMetaMeshPointer)
		{
			ScriptingInterfaceOfIGameEntity.call_ChangeMetaMeshOrRemoveItIfNotExistsDelegate(entityId, entityMetaMeshPointer, newMetaMeshPointer);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00010357 File Offset: 0x0000E557
		public void ChangeResolutionMultiplierOfWaterVisual(UIntPtr visualPrefab, float multiplier, in Vec3 waterEffectsBB)
		{
			ScriptingInterfaceOfIGameEntity.call_ChangeResolutionMultiplierOfWaterVisualDelegate(visualPrefab, multiplier, in waterEffectsBB);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00010366 File Offset: 0x0000E566
		public bool CheckIsPrefabLinkRootPrefab(UIntPtr entityPtr, int depth)
		{
			return ScriptingInterfaceOfIGameEntity.call_CheckIsPrefabLinkRootPrefabDelegate(entityPtr, depth);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00010374 File Offset: 0x0000E574
		public bool CheckPointWithOrientedBoundingBox(UIntPtr entityId, Vec3 point)
		{
			return ScriptingInterfaceOfIGameEntity.call_CheckPointWithOrientedBoundingBoxDelegate(entityId, point);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00010382 File Offset: 0x0000E582
		public bool CheckResources(UIntPtr entityId, bool addToQueue, bool checkFaceResources)
		{
			return ScriptingInterfaceOfIGameEntity.call_CheckResourcesDelegate(entityId, addToQueue, checkFaceResources);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00010391 File Offset: 0x0000E591
		public void ClearComponents(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_ClearComponentsDelegate(entityId);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0001039E File Offset: 0x0000E59E
		public void ClearEntityComponents(UIntPtr entityId, bool resetAll, bool removeScripts, bool deleteChildEntities)
		{
			ScriptingInterfaceOfIGameEntity.call_ClearEntityComponentsDelegate(entityId, resetAll, removeScripts, deleteChildEntities);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000103AF File Offset: 0x0000E5AF
		public void ClearOnlyOwnComponents(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_ClearOnlyOwnComponentsDelegate(entityId);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000103BC File Offset: 0x0000E5BC
		public void ComputeTrajectoryVolume(UIntPtr gameEntity, float missileSpeed, float verticalAngleMaxInDegrees, float verticalAngleMinInDegrees, float horizontalAngleRangeInDegrees, float airFrictionConstant)
		{
			ScriptingInterfaceOfIGameEntity.call_ComputeTrajectoryVolumeDelegate(gameEntity, missileSpeed, verticalAngleMaxInDegrees, verticalAngleMinInDegrees, horizontalAngleRangeInDegrees, airFrictionConstant);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000103D1 File Offset: 0x0000E5D1
		public void ComputeVelocityDeltaFromImpulse(UIntPtr entityPtr, in Vec3 impulsiveForce, in Vec3 impulsiveTorque, out Vec3 deltaLinearVelocity, out Vec3 deltaAngularVelocity)
		{
			ScriptingInterfaceOfIGameEntity.call_ComputeVelocityDeltaFromImpulseDelegate(entityPtr, in impulsiveForce, in impulsiveTorque, out deltaLinearVelocity, out deltaAngularVelocity);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000103E4 File Offset: 0x0000E5E4
		public void ConvertDynamicBodyToRayCast(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_ConvertDynamicBodyToRayCastDelegate(entityId);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000103F1 File Offset: 0x0000E5F1
		public void CookTrianglePhysxMesh(UIntPtr cookingInstancePointer, UIntPtr shapePointer, UIntPtr quadPinnedPointer, int physicsMaterial, int numberOfVertices, UIntPtr indicesPinnedPointer, int numberOfIndices)
		{
			ScriptingInterfaceOfIGameEntity.call_CookTrianglePhysxMeshDelegate(cookingInstancePointer, shapePointer, quadPinnedPointer, physicsMaterial, numberOfVertices, indicesPinnedPointer, numberOfIndices);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00010408 File Offset: 0x0000E608
		public void CopyComponentsToSkeleton(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_CopyComponentsToSkeletonDelegate(entityId);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00010418 File Offset: 0x0000E618
		public GameEntity CopyFromPrefab(UIntPtr prefab)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_CopyFromPrefabDelegate(prefab);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00010464 File Offset: 0x0000E664
		public void CopyScriptComponentFromAnotherEntity(UIntPtr prefab, UIntPtr other_prefab, string script_name)
		{
			byte[] array = null;
			if (script_name != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(script_name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(script_name, 0, script_name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_CopyScriptComponentFromAnotherEntityDelegate(prefab, other_prefab, array);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000104C0 File Offset: 0x0000E6C0
		public void CreateAndAddScriptComponent(UIntPtr entityId, string name, bool callScriptCallbacks)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_CreateAndAddScriptComponentDelegate(entityId, array, callScriptCallbacks);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0001051C File Offset: 0x0000E71C
		public GameEntity CreateEmpty(UIntPtr scenePointer, bool isModifiableFromEditor, UIntPtr entityId, bool createPhysics, bool callScriptCallbacks)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_CreateEmptyDelegate(scenePointer, isModifiableFromEditor, entityId, createPhysics, callScriptCallbacks);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0001056C File Offset: 0x0000E76C
		public UIntPtr CreateEmptyPhysxShape(UIntPtr entityPointer, bool isVariable, int physxMaterialIndex)
		{
			return ScriptingInterfaceOfIGameEntity.call_CreateEmptyPhysxShapeDelegate(entityPointer, isVariable, physxMaterialIndex);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0001057C File Offset: 0x0000E77C
		public GameEntity CreateEmptyWithoutScene()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_CreateEmptyWithoutSceneDelegate();
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000105C8 File Offset: 0x0000E7C8
		public GameEntity CreateFromPrefab(UIntPtr scenePointer, string prefabid, bool callScriptCallbacks, bool createPhysics, uint scriptInclusionHashTag)
		{
			byte[] array = null;
			if (prefabid != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(prefabid);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(prefabid, 0, prefabid.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_CreateFromPrefabDelegate(scenePointer, array, callScriptCallbacks, createPhysics, scriptInclusionHashTag);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0001065C File Offset: 0x0000E85C
		public GameEntity CreateFromPrefabWithInitialFrameAndRestOffset(UIntPtr scenePointer, string prefabId, bool createPhysics, ref MatrixFrame frame, bool hasCustomRestOffset, float restOffset, bool callScriptCallbacks, uint scriptInclusionHashTag)
		{
			byte[] array = null;
			if (prefabId != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(prefabId);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(prefabId, 0, prefabId.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_CreateFromPrefabWithInitialFrameAndRestOffsetDelegate(scenePointer, array, createPhysics, ref frame, hasCustomRestOffset, restOffset, callScriptCallbacks, scriptInclusionHashTag);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000106F4 File Offset: 0x0000E8F4
		public UIntPtr CreatePhysxCookingInstance()
		{
			return ScriptingInterfaceOfIGameEntity.call_CreatePhysxCookingInstanceDelegate();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00010700 File Offset: 0x0000E900
		public void CreateVariableRatePhysics(UIntPtr entityId, bool forChildren)
		{
			ScriptingInterfaceOfIGameEntity.call_CreateVariableRatePhysicsDelegate(entityId, forChildren);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0001070E File Offset: 0x0000E90E
		public void DeleteEmptyShape(UIntPtr entity, UIntPtr shape1, UIntPtr shape2)
		{
			ScriptingInterfaceOfIGameEntity.call_DeleteEmptyShapeDelegate(entity, shape1, shape2);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0001071D File Offset: 0x0000E91D
		public void DeletePhysxCookingInstance(UIntPtr pointer)
		{
			ScriptingInterfaceOfIGameEntity.call_DeletePhysxCookingInstanceDelegate(pointer);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0001072A File Offset: 0x0000E92A
		public void DeRegisterWaterMeshMaterials(UIntPtr entityPointer, UIntPtr visualPrefab)
		{
			ScriptingInterfaceOfIGameEntity.call_DeRegisterWaterMeshMaterialsDelegate(entityPointer, visualPrefab);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00010738 File Offset: 0x0000E938
		public void DeRegisterWaterSDFClip(UIntPtr entityId, int slot)
		{
			ScriptingInterfaceOfIGameEntity.call_DeRegisterWaterSDFClipDelegate(entityId, slot);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00010746 File Offset: 0x0000E946
		public void DeselectEntityOnEditor(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_DeselectEntityOnEditorDelegate(entityId);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00010753 File Offset: 0x0000E953
		public void DetachAllAttachedNavigationMeshFaces(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_DetachAllAttachedNavigationMeshFacesDelegate(entityId);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00010760 File Offset: 0x0000E960
		public void DisableContour(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_DisableContourDelegate(entityId);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0001076D File Offset: 0x0000E96D
		public void DisableDynamicBodySimulation(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_DisableDynamicBodySimulationDelegate(entityId);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0001077A File Offset: 0x0000E97A
		public void DisableGravity(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_DisableGravityDelegate(entityId);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00010787 File Offset: 0x0000E987
		public void EnableDynamicBody(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_EnableDynamicBodyDelegate(entityId);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00010794 File Offset: 0x0000E994
		public GameEntity FindWithName(UIntPtr scenePointer, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_FindWithNameDelegate(scenePointer, array);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00010821 File Offset: 0x0000EA21
		public void Freeze(UIntPtr entityId, bool isFrozen)
		{
			ScriptingInterfaceOfIGameEntity.call_FreezeDelegate(entityId, isFrozen);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0001082F File Offset: 0x0000EA2F
		public Vec3 GetAngularVelocity(UIntPtr entityPtr)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetAngularVelocityDelegate(entityPtr);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0001083C File Offset: 0x0000EA3C
		public int GetAttachedNavmeshFaceCount(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetAttachedNavmeshFaceCountDelegate(entityId);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0001084C File Offset: 0x0000EA4C
		public void GetAttachedNavmeshFaceRecords(UIntPtr entityId, PathFaceRecord[] faceRecords)
		{
			PinnedArrayData<PathFaceRecord> pinnedArrayData = new PinnedArrayData<PathFaceRecord>(faceRecords, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIGameEntity.call_GetAttachedNavmeshFaceRecordsDelegate(entityId, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00010880 File Offset: 0x0000EA80
		public void GetAttachedNavmeshFaceVertexIndices(UIntPtr entityId, in PathFaceRecord faceRecord, int[] indices)
		{
			PinnedArrayData<int> pinnedArrayData = new PinnedArrayData<int>(indices, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIGameEntity.call_GetAttachedNavmeshFaceVertexIndicesDelegate(entityId, in faceRecord, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000108B2 File Offset: 0x0000EAB2
		public uint GetBodyFlags(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetBodyFlagsDelegate(entityId);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000108C0 File Offset: 0x0000EAC0
		public PhysicsShape GetBodyShape(UIntPtr entityId)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_GetBodyShapeDelegate(entityId);
			PhysicsShape physicsShape = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				physicsShape = new PhysicsShape(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return physicsShape;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0001090A File Offset: 0x0000EB0A
		public void GetBodyVisualWorldTransform(UIntPtr entityPtr, out MatrixFrame frame)
		{
			ScriptingInterfaceOfIGameEntity.call_GetBodyVisualWorldTransformDelegate(entityPtr, out frame);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00010918 File Offset: 0x0000EB18
		public void GetBodyWorldTransform(UIntPtr entityPtr, out MatrixFrame frame)
		{
			ScriptingInterfaceOfIGameEntity.call_GetBodyWorldTransformDelegate(entityPtr, out frame);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00010926 File Offset: 0x0000EB26
		public sbyte GetBoneCount(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetBoneCountDelegate(entityId);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00010933 File Offset: 0x0000EB33
		public void GetBoneEntitialFrameWithIndex(UIntPtr entityId, sbyte boneIndex, ref MatrixFrame outEntitialFrame)
		{
			ScriptingInterfaceOfIGameEntity.call_GetBoneEntitialFrameWithIndexDelegate(entityId, boneIndex, ref outEntitialFrame);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00010944 File Offset: 0x0000EB44
		public void GetBoneEntitialFrameWithName(UIntPtr entityId, string boneName, ref MatrixFrame outEntitialFrame)
		{
			byte[] array = null;
			if (boneName != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(boneName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(boneName, 0, boneName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_GetBoneEntitialFrameWithNameDelegate(entityId, array, ref outEntitialFrame);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000109A0 File Offset: 0x0000EBA0
		public Vec3 GetBoundingBoxMax(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetBoundingBoxMaxDelegate(entityId);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000109AD File Offset: 0x0000EBAD
		public Vec3 GetBoundingBoxMin(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetBoundingBoxMinDelegate(entityId);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000109BA File Offset: 0x0000EBBA
		public void GetCameraParamsFromCameraScript(UIntPtr entityId, UIntPtr camPtr, ref Vec3 dof_params)
		{
			ScriptingInterfaceOfIGameEntity.call_GetCameraParamsFromCameraScriptDelegate(entityId, camPtr, ref dof_params);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000109C9 File Offset: 0x0000EBC9
		public Vec3 GetCenterOfMass(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetCenterOfMassDelegate(entityId);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000109D8 File Offset: 0x0000EBD8
		public GameEntity GetChild(UIntPtr entityId, int childIndex)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_GetChildDelegate(entityId, childIndex);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00010A23 File Offset: 0x0000EC23
		public int GetChildCount(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetChildCountDelegate(entityId);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00010A30 File Offset: 0x0000EC30
		public UIntPtr GetChildPointer(UIntPtr entityId, int childIndex)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetChildPointerDelegate(entityId, childIndex);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00010A40 File Offset: 0x0000EC40
		public GameEntityComponent GetComponentAtIndex(UIntPtr entityId, GameEntity.ComponentType componentType, int index)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_GetComponentAtIndexDelegate(entityId, componentType, index);
			GameEntityComponent gameEntityComponent = NativeObject.CreateNativeObjectWrapper<GameEntityComponent>(nativeObjectPointer);
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntityComponent;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00010A83 File Offset: 0x0000EC83
		public int GetComponentCount(UIntPtr entityId, GameEntity.ComponentType componentType)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetComponentCountDelegate(entityId, componentType);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00010A91 File Offset: 0x0000EC91
		public bool GetEditModeLevelVisibility(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetEditModeLevelVisibilityDelegate(entityId);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00010A9E File Offset: 0x0000EC9E
		public EntityFlags GetEntityFlags(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetEntityFlagsDelegate(entityId);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00010AAB File Offset: 0x0000ECAB
		public EntityVisibilityFlags GetEntityVisibilityFlags(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetEntityVisibilityFlagsDelegate(entityId);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00010AB8 File Offset: 0x0000ECB8
		public uint GetFactorColor(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetFactorColorDelegate(entityId);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00010AC8 File Offset: 0x0000ECC8
		public UIntPtr GetFirstChildWithTagRecursive(UIntPtr entityPtr, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIGameEntity.call_GetFirstChildWithTagRecursiveDelegate(entityPtr, array);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00010B24 File Offset: 0x0000ED24
		public UIntPtr GetFirstEntityWithTag(UIntPtr scenePointer, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIGameEntity.call_GetFirstEntityWithTagDelegate(scenePointer, array);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00010B80 File Offset: 0x0000ED80
		public UIntPtr GetFirstEntityWithTagExpression(UIntPtr scenePointer, string tagExpression)
		{
			byte[] array = null;
			if (tagExpression != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(tagExpression);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(tagExpression, 0, tagExpression.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIGameEntity.call_GetFirstEntityWithTagExpressionDelegate(scenePointer, array);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00010BDC File Offset: 0x0000EDDC
		public Mesh GetFirstMesh(UIntPtr entityId)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_GetFirstMeshDelegate(entityId);
			Mesh mesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				mesh = new Mesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return mesh;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00010C26 File Offset: 0x0000EE26
		public BoundingBox GetGlobalBoundingBox(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetGlobalBoundingBoxDelegate(entityId);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00010C33 File Offset: 0x0000EE33
		public Vec3 GetGlobalBoxMax(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetGlobalBoxMaxDelegate(entityId);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00010C40 File Offset: 0x0000EE40
		public Vec3 GetGlobalBoxMin(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetGlobalBoxMinDelegate(entityId);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00010C4D File Offset: 0x0000EE4D
		public void GetGlobalFrame(UIntPtr meshPointer, out MatrixFrame outFrame)
		{
			ScriptingInterfaceOfIGameEntity.call_GetGlobalFrameDelegate(meshPointer, out outFrame);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00010C5B File Offset: 0x0000EE5B
		public void GetGlobalFrameImpreciseForFixedTick(UIntPtr entityId, out MatrixFrame outFrame)
		{
			ScriptingInterfaceOfIGameEntity.call_GetGlobalFrameImpreciseForFixedTickDelegate(entityId, out outFrame);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00010C69 File Offset: 0x0000EE69
		public Vec3 GetGlobalScale(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetGlobalScaleDelegate(pointer);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00010C76 File Offset: 0x0000EE76
		public Vec2 GetGlobalWindStrengthVectorOfScene(UIntPtr entityPtr)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetGlobalWindStrengthVectorOfSceneDelegate(entityPtr);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00010C83 File Offset: 0x0000EE83
		public Vec2 GetGlobalWindVelocityOfScene(UIntPtr entityPtr)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetGlobalWindVelocityOfSceneDelegate(entityPtr);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00010C90 File Offset: 0x0000EE90
		public Vec2 GetGlobalWindVelocityWithGustNoiseOfScene(UIntPtr entityPtr, float globalTime)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetGlobalWindVelocityWithGustNoiseOfSceneDelegate(entityPtr, globalTime);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00010C9E File Offset: 0x0000EE9E
		public string GetGuid(UIntPtr entityId)
		{
			if (ScriptingInterfaceOfIGameEntity.call_GetGuidDelegate(entityId) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00010CB5 File Offset: 0x0000EEB5
		public Vec3 GetLastFinalRenderCameraPositionOfScene(UIntPtr entityPtr)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetLastFinalRenderCameraPositionOfSceneDelegate(entityPtr);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00010CC4 File Offset: 0x0000EEC4
		public Light GetLight(UIntPtr entityId)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_GetLightDelegate(entityId);
			Light light = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				light = new Light(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return light;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00010D0E File Offset: 0x0000EF0E
		public Vec3 GetLinearVelocity(UIntPtr entityPtr)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetLinearVelocityDelegate(entityPtr);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00010D1B File Offset: 0x0000EF1B
		public BoundingBox GetLocalBoundingBox(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetLocalBoundingBoxDelegate(entityId);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00010D28 File Offset: 0x0000EF28
		public void GetLocalFrame(UIntPtr entityId, out MatrixFrame outFrame)
		{
			ScriptingInterfaceOfIGameEntity.call_GetLocalFrameDelegate(entityId, out outFrame);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00010D36 File Offset: 0x0000EF36
		public void GetLocalPhysicsBoundingBox(UIntPtr entityId, bool includeChildren, out BoundingBox outBoundingBox)
		{
			ScriptingInterfaceOfIGameEntity.call_GetLocalPhysicsBoundingBoxDelegate(entityId, includeChildren, out outBoundingBox);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00010D45 File Offset: 0x0000EF45
		public float GetLodLevelForDistanceSq(UIntPtr entityId, float distanceSquared)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetLodLevelForDistanceSqDelegate(entityId, distanceSquared);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00010D53 File Offset: 0x0000EF53
		public float GetMass(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetMassDelegate(entityId);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00010D60 File Offset: 0x0000EF60
		public Vec3 GetMassSpaceInertia(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetMassSpaceInertiaDelegate(entityId);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00010D6D File Offset: 0x0000EF6D
		public Vec3 GetMassSpaceInverseInertia(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetMassSpaceInverseInertiaDelegate(entityId);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00010D7A File Offset: 0x0000EF7A
		public void GetMeshBendedPosition(UIntPtr entityId, ref MatrixFrame worldSpacePosition, ref MatrixFrame output)
		{
			ScriptingInterfaceOfIGameEntity.call_GetMeshBendedPositionDelegate(entityId, ref worldSpacePosition, ref output);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00010D89 File Offset: 0x0000EF89
		public GameEntity.Mobility GetMobility(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetMobilityDelegate(entityId);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00010D96 File Offset: 0x0000EF96
		public string GetName(UIntPtr entityId)
		{
			if (ScriptingInterfaceOfIGameEntity.call_GetNameDelegate(entityId) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00010DB0 File Offset: 0x0000EFB0
		public void GetNativeScriptComponentVariable(UIntPtr entityPtr, string className, string fieldName, ref ScriptComponentFieldHolder data, RglScriptFieldType variableType)
		{
			byte[] array = null;
			if (className != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(className);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(className, 0, className.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (fieldName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(fieldName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(fieldName, 0, fieldName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_GetNativeScriptComponentVariableDelegate(entityPtr, array, array2, ref data, variableType);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00010E54 File Offset: 0x0000F054
		public UIntPtr GetNextEntityWithTag(UIntPtr currententityId, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIGameEntity.call_GetNextEntityWithTagDelegate(currententityId, array);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00010EB0 File Offset: 0x0000F0B0
		public UIntPtr GetNextEntityWithTagExpression(UIntPtr currententityId, string tagExpression)
		{
			byte[] array = null;
			if (tagExpression != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(tagExpression);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(tagExpression, 0, tagExpression.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIGameEntity.call_GetNextEntityWithTagExpressionDelegate(currententityId, array);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00010F0C File Offset: 0x0000F10C
		public GameEntity GetNextPrefab(UIntPtr currentPrefab)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_GetNextPrefabDelegate(currentPrefab);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00010F56 File Offset: 0x0000F156
		public string GetOldPrefabName(UIntPtr prefab)
		{
			if (ScriptingInterfaceOfIGameEntity.call_GetOldPrefabNameDelegate(prefab) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00010F70 File Offset: 0x0000F170
		public GameEntity GetParent(UIntPtr entityId)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_GetParentDelegate(entityId);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00010FBA File Offset: 0x0000F1BA
		public UIntPtr GetParentPointer(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetParentPointerDelegate(entityId);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00010FC7 File Offset: 0x0000F1C7
		public uint GetPhysicsDescBodyFlags(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetPhysicsDescBodyFlagsDelegate(entityId);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00010FD4 File Offset: 0x0000F1D4
		public int GetPhysicsMaterialIndex(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetPhysicsMaterialIndexDelegate(entityId);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00010FE1 File Offset: 0x0000F1E1
		public void GetPhysicsMinMax(UIntPtr entityId, bool includeChildren, ref Vec3 bbmin, ref Vec3 bbmax, bool returnLocal)
		{
			ScriptingInterfaceOfIGameEntity.call_GetPhysicsMinMaxDelegate(entityId, includeChildren, ref bbmin, ref bbmax, returnLocal);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00010FF4 File Offset: 0x0000F1F4
		public bool GetPhysicsState(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetPhysicsStateDelegate(entityId);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00011001 File Offset: 0x0000F201
		public int GetPhysicsTriangleCount(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetPhysicsTriangleCountDelegate(entityId);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0001100E File Offset: 0x0000F20E
		public string GetPrefabName(UIntPtr prefab)
		{
			if (ScriptingInterfaceOfIGameEntity.call_GetPrefabNameDelegate(prefab) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00011025 File Offset: 0x0000F225
		public void GetPreviousGlobalFrame(UIntPtr entityPtr, out MatrixFrame frame)
		{
			ScriptingInterfaceOfIGameEntity.call_GetPreviousGlobalFrameDelegate(entityPtr, out frame);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00011033 File Offset: 0x0000F233
		public void GetQuickBoneEntitialFrame(UIntPtr entityId, sbyte index, out MatrixFrame frame)
		{
			ScriptingInterfaceOfIGameEntity.call_GetQuickBoneEntitialFrameDelegate(entityId, index, out frame);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00011042 File Offset: 0x0000F242
		public float GetRadius(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetRadiusDelegate(entityId);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0001104F File Offset: 0x0000F24F
		public UIntPtr GetRootParentPointer(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetRootParentPointerDelegate(entityId);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0001105C File Offset: 0x0000F25C
		public Scene GetScene(UIntPtr entityId)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_GetSceneDelegate(entityId);
			Scene scene = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				scene = new Scene(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return scene;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000110A6 File Offset: 0x0000F2A6
		public UIntPtr GetScenePointer(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetScenePointerDelegate(entityId);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000110B3 File Offset: 0x0000F2B3
		public ScriptComponentBehavior GetScriptComponent(UIntPtr entityId)
		{
			return DotNetObject.GetManagedObjectWithId(ScriptingInterfaceOfIGameEntity.call_GetScriptComponentDelegate(entityId)) as ScriptComponentBehavior;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000110CA File Offset: 0x0000F2CA
		public ScriptComponentBehavior GetScriptComponentAtIndex(UIntPtr entityId, int index)
		{
			return DotNetObject.GetManagedObjectWithId(ScriptingInterfaceOfIGameEntity.call_GetScriptComponentAtIndexDelegate(entityId, index)) as ScriptComponentBehavior;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000110E2 File Offset: 0x0000F2E2
		public int GetScriptComponentCount(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetScriptComponentCountDelegate(entityId);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000110EF File Offset: 0x0000F2EF
		public int GetScriptComponentIndex(UIntPtr entityId, uint nameHash)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetScriptComponentIndexDelegate(entityId, nameHash);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00011100 File Offset: 0x0000F300
		public Skeleton GetSkeleton(UIntPtr entityId)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntity.call_GetSkeletonDelegate(entityId);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0001114A File Offset: 0x0000F34A
		public string GetTags(UIntPtr entityId)
		{
			if (ScriptingInterfaceOfIGameEntity.call_GetTagsDelegate(entityId) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00011161 File Offset: 0x0000F361
		public uint GetUpgradeLevelMask(UIntPtr prefab)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetUpgradeLevelMaskDelegate(prefab);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0001116E File Offset: 0x0000F36E
		public uint GetUpgradeLevelMaskCumulative(UIntPtr prefab)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetUpgradeLevelMaskCumulativeDelegate(prefab);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0001117B File Offset: 0x0000F37B
		public bool GetVisibilityExcludeParents(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetVisibilityExcludeParentsDelegate(entityId);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00011188 File Offset: 0x0000F388
		public uint GetVisibilityLevelMaskIncludingParents(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetVisibilityLevelMaskIncludingParentsDelegate(entityId);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00011195 File Offset: 0x0000F395
		public float GetWaterLevelAtPosition(UIntPtr entityId, in Vec2 position, bool useWaterRenderer, bool checkWaterBodyEntities)
		{
			return ScriptingInterfaceOfIGameEntity.call_GetWaterLevelAtPositionDelegate(entityId, in position, useWaterRenderer, checkWaterBodyEntities);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000111A6 File Offset: 0x0000F3A6
		public bool HasBatchedKinematicPhysicsFlag(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasBatchedKinematicPhysicsFlagDelegate(entityId);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000111B3 File Offset: 0x0000F3B3
		public bool HasBatchedRayCastPhysicsFlag(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasBatchedRayCastPhysicsFlagDelegate(entityId);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000111C0 File Offset: 0x0000F3C0
		public bool HasBody(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasBodyDelegate(entityId);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000111CD File Offset: 0x0000F3CD
		public bool HasComplexAnimTree(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasComplexAnimTreeDelegate(entityId);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000111DA File Offset: 0x0000F3DA
		public bool HasComponent(UIntPtr pointer, UIntPtr componentPointer)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasComponentDelegate(pointer, componentPointer);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000111E8 File Offset: 0x0000F3E8
		public bool HasDynamicRigidBody(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasDynamicRigidBodyDelegate(entityId);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000111F5 File Offset: 0x0000F3F5
		public bool HasDynamicRigidBodyAndActiveSimulation(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasDynamicRigidBodyAndActiveSimulationDelegate(entityId);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00011202 File Offset: 0x0000F402
		public bool HasFrameChanged(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasFrameChangedDelegate(entityId);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0001120F File Offset: 0x0000F40F
		public bool HasKinematicRigidBody(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasKinematicRigidBodyDelegate(entityId);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0001121C File Offset: 0x0000F41C
		public bool HasPhysicsBody(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasPhysicsBodyDelegate(entityId);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00011229 File Offset: 0x0000F429
		public bool HasPhysicsDefinition(UIntPtr entityId, int excludeFlags)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasPhysicsDefinitionDelegate(entityId, excludeFlags);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00011237 File Offset: 0x0000F437
		public bool HasScene(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasSceneDelegate(entityId);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00011244 File Offset: 0x0000F444
		public bool HasScriptComponent(UIntPtr entityId, string scName)
		{
			byte[] array = null;
			if (scName != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(scName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(scName, 0, scName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIGameEntity.call_HasScriptComponentDelegate(entityId, array);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0001129F File Offset: 0x0000F49F
		public bool HasScriptComponentHash(UIntPtr entityId, uint scNameHash)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasScriptComponentHashDelegate(entityId, scNameHash);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000112AD File Offset: 0x0000F4AD
		public bool HasStaticPhysicsBody(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_HasStaticPhysicsBodyDelegate(entityId);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000112BC File Offset: 0x0000F4BC
		public bool HasTag(UIntPtr entityId, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIGameEntity.call_HasTagDelegate(entityId, array);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00011317 File Offset: 0x0000F517
		public bool IsDynamicBodyStationary(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_IsDynamicBodyStationaryDelegate(entityId);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00011324 File Offset: 0x0000F524
		public bool IsEngineBodySleeping(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_IsEngineBodySleepingDelegate(entityId);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00011331 File Offset: 0x0000F531
		public bool IsEntitySelectedOnEditor(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_IsEntitySelectedOnEditorDelegate(entityId);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0001133E File Offset: 0x0000F53E
		public bool IsFrozen(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_IsFrozenDelegate(entityId);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0001134B File Offset: 0x0000F54B
		public bool IsGhostObject(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_IsGhostObjectDelegate(entityId);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00011358 File Offset: 0x0000F558
		public bool IsGravityDisabled(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_IsGravityDisabledDelegate(entityId);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00011365 File Offset: 0x0000F565
		public bool IsGuidValid(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_IsGuidValidDelegate(entityId);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00011372 File Offset: 0x0000F572
		public bool IsInEditorScene(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIGameEntity.call_IsInEditorSceneDelegate(pointer);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0001137F File Offset: 0x0000F57F
		public bool IsVisibleIncludeParents(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIGameEntity.call_IsVisibleIncludeParentsDelegate(entityId);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0001138C File Offset: 0x0000F58C
		public void PauseParticleSystem(UIntPtr entityId, bool doChildren)
		{
			ScriptingInterfaceOfIGameEntity.call_PauseParticleSystemDelegate(entityId, doChildren);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0001139A File Offset: 0x0000F59A
		public void PopCapsuleShapeFromEntityBody(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_PopCapsuleShapeFromEntityBodyDelegate(entityId);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000113A8 File Offset: 0x0000F5A8
		public bool PrefabExists(string prefabName)
		{
			byte[] array = null;
			if (prefabName != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(prefabName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(prefabName, 0, prefabName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIGameEntity.call_PrefabExistsDelegate(array);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00011404 File Offset: 0x0000F604
		public void PushCapsuleShapeToEntityBody(UIntPtr entityId, Vec3 p1, Vec3 p2, float radius, string physicsMaterialName)
		{
			byte[] array = null;
			if (physicsMaterialName != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(physicsMaterialName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(physicsMaterialName, 0, physicsMaterialName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_PushCapsuleShapeToEntityBodyDelegate(entityId, p1, p2, radius, array);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00011467 File Offset: 0x0000F667
		public bool RayHitEntity(UIntPtr entityId, in Vec3 rayOrigin, in Vec3 rayDirection, float maxLength, ref float resultLength)
		{
			return ScriptingInterfaceOfIGameEntity.call_RayHitEntityDelegate(entityId, in rayOrigin, in rayDirection, maxLength, ref resultLength);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0001147A File Offset: 0x0000F67A
		public bool RayHitEntityWithNormal(UIntPtr entityId, in Vec3 rayOrigin, in Vec3 rayDirection, float maxLength, ref Vec3 resultNormal, ref float resultLength)
		{
			return ScriptingInterfaceOfIGameEntity.call_RayHitEntityWithNormalDelegate(entityId, in rayOrigin, in rayDirection, maxLength, ref resultNormal, ref resultLength);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0001148F File Offset: 0x0000F68F
		public void RecomputeBoundingBox(UIntPtr pointer)
		{
			ScriptingInterfaceOfIGameEntity.call_RecomputeBoundingBoxDelegate(pointer);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0001149C File Offset: 0x0000F69C
		public void RefreshMeshesToRenderToHullWater(UIntPtr entityPointer, UIntPtr visualPrefab, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_RefreshMeshesToRenderToHullWaterDelegate(entityPointer, visualPrefab, array);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000114F8 File Offset: 0x0000F6F8
		public int RegisterWaterSDFClip(UIntPtr entityId, UIntPtr textureID)
		{
			return ScriptingInterfaceOfIGameEntity.call_RegisterWaterSDFClipDelegate(entityId, textureID);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00011506 File Offset: 0x0000F706
		public void RelaxLocalBoundingBox(UIntPtr entityId, in BoundingBox boundingBox)
		{
			ScriptingInterfaceOfIGameEntity.call_RelaxLocalBoundingBoxDelegate(entityId, in boundingBox);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00011514 File Offset: 0x0000F714
		public void ReleaseEditDataUserToAllMeshes(UIntPtr entityId, bool entity_components, bool skeleton_components)
		{
			ScriptingInterfaceOfIGameEntity.call_ReleaseEditDataUserToAllMeshesDelegate(entityId, entity_components, skeleton_components);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00011523 File Offset: 0x0000F723
		public void Remove(UIntPtr entityId, int removeReason)
		{
			ScriptingInterfaceOfIGameEntity.call_RemoveDelegate(entityId, removeReason);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00011531 File Offset: 0x0000F731
		public void RemoveAllChildren(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_RemoveAllChildrenDelegate(entityId);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0001153E File Offset: 0x0000F73E
		public void RemoveAllParticleSystems(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_RemoveAllParticleSystemsDelegate(entityId);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0001154B File Offset: 0x0000F74B
		public void RemoveChild(UIntPtr parentEntity, UIntPtr childEntity, bool keepPhysics, bool keepScenePointer, bool callScriptCallbacks, int removeReason)
		{
			ScriptingInterfaceOfIGameEntity.call_RemoveChildDelegate(parentEntity, childEntity, keepPhysics, keepScenePointer, callScriptCallbacks, removeReason);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00011560 File Offset: 0x0000F760
		public bool RemoveComponent(UIntPtr pointer, UIntPtr componentPointer)
		{
			return ScriptingInterfaceOfIGameEntity.call_RemoveComponentDelegate(pointer, componentPointer);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0001156E File Offset: 0x0000F76E
		public bool RemoveComponentWithMesh(UIntPtr entityId, UIntPtr mesh)
		{
			return ScriptingInterfaceOfIGameEntity.call_RemoveComponentWithMeshDelegate(entityId, mesh);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0001157C File Offset: 0x0000F77C
		public void RemoveEnginePhysics(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_RemoveEnginePhysicsDelegate(entityId);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00011589 File Offset: 0x0000F789
		public void RemoveFromPredisplayEntity(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_RemoveFromPredisplayEntityDelegate(entityId);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00011596 File Offset: 0x0000F796
		public void RemoveJoint(UIntPtr jointId, UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_RemoveJointDelegate(jointId, entityId);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000115A4 File Offset: 0x0000F7A4
		public bool RemoveMultiMesh(UIntPtr entityId, UIntPtr multiMeshPtr)
		{
			return ScriptingInterfaceOfIGameEntity.call_RemoveMultiMeshDelegate(entityId, multiMeshPtr);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000115B2 File Offset: 0x0000F7B2
		public void RemoveMultiMeshFromSkeleton(UIntPtr gameEntity, UIntPtr multiMesh)
		{
			ScriptingInterfaceOfIGameEntity.call_RemoveMultiMeshFromSkeletonDelegate(gameEntity, multiMesh);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000115C0 File Offset: 0x0000F7C0
		public void RemoveMultiMeshFromSkeletonBone(UIntPtr gameEntity, UIntPtr multiMesh, sbyte boneIndex)
		{
			ScriptingInterfaceOfIGameEntity.call_RemoveMultiMeshFromSkeletonBoneDelegate(gameEntity, multiMesh, boneIndex);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000115CF File Offset: 0x0000F7CF
		public void RemovePhysics(UIntPtr entityId, bool clearingTheScene)
		{
			ScriptingInterfaceOfIGameEntity.call_RemovePhysicsDelegate(entityId, clearingTheScene);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000115DD File Offset: 0x0000F7DD
		public void RemoveScriptComponent(UIntPtr entityId, UIntPtr scriptComponentPtr, int removeReason)
		{
			ScriptingInterfaceOfIGameEntity.call_RemoveScriptComponentDelegate(entityId, scriptComponentPtr, removeReason);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000115EC File Offset: 0x0000F7EC
		public void RemoveTag(UIntPtr entityId, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_RemoveTagDelegate(entityId, array);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00011647 File Offset: 0x0000F847
		public void ReplacePhysicsBodyWithQuadPhysicsBody(UIntPtr pointer, UIntPtr quad, int physicsMaterial, BodyFlags bodyFlags, int numberOfVertices, UIntPtr indices, int numberOfIndices)
		{
			ScriptingInterfaceOfIGameEntity.call_ReplacePhysicsBodyWithQuadPhysicsBodyDelegate(pointer, quad, physicsMaterial, bodyFlags, numberOfVertices, indices, numberOfIndices);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0001165E File Offset: 0x0000F85E
		public void ResetHullWater(UIntPtr visualPrefab)
		{
			ScriptingInterfaceOfIGameEntity.call_ResetHullWaterDelegate(visualPrefab);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0001166B File Offset: 0x0000F86B
		public void ResumeParticleSystem(UIntPtr entityId, bool doChildren)
		{
			ScriptingInterfaceOfIGameEntity.call_ResumeParticleSystemDelegate(entityId, doChildren);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00011679 File Offset: 0x0000F879
		public void SelectEntityOnEditor(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_SelectEntityOnEditorDelegate(entityId);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00011686 File Offset: 0x0000F886
		public void SetAlpha(UIntPtr entityId, float alpha)
		{
			ScriptingInterfaceOfIGameEntity.call_SetAlphaDelegate(entityId, alpha);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00011694 File Offset: 0x0000F894
		public void SetAngularVelocity(UIntPtr entityPtr, in Vec3 newAngularVelocity)
		{
			ScriptingInterfaceOfIGameEntity.call_SetAngularVelocityDelegate(entityPtr, in newAngularVelocity);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000116A2 File Offset: 0x0000F8A2
		public void SetAnimationSoundActivation(UIntPtr entityId, bool activate)
		{
			ScriptingInterfaceOfIGameEntity.call_SetAnimationSoundActivationDelegate(entityId, activate);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000116B0 File Offset: 0x0000F8B0
		public void SetAnimTreeChannelParameter(UIntPtr entityId, float phase, int channel_no)
		{
			ScriptingInterfaceOfIGameEntity.call_SetAnimTreeChannelParameterDelegate(entityId, phase, channel_no);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000116BF File Offset: 0x0000F8BF
		public void SetAsContourEntity(UIntPtr entityId, uint color)
		{
			ScriptingInterfaceOfIGameEntity.call_SetAsContourEntityDelegate(entityId, color);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000116CD File Offset: 0x0000F8CD
		public void SetAsPredisplayEntity(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_SetAsPredisplayEntityDelegate(entityId);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000116DA File Offset: 0x0000F8DA
		public void SetAsReplayEntity(UIntPtr gameEntity)
		{
			ScriptingInterfaceOfIGameEntity.call_SetAsReplayEntityDelegate(gameEntity);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000116E7 File Offset: 0x0000F8E7
		public void SetBodyFlags(UIntPtr entityId, uint bodyFlags)
		{
			ScriptingInterfaceOfIGameEntity.call_SetBodyFlagsDelegate(entityId, bodyFlags);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000116F5 File Offset: 0x0000F8F5
		public void SetBodyFlagsRecursive(UIntPtr entityId, uint bodyFlags)
		{
			ScriptingInterfaceOfIGameEntity.call_SetBodyFlagsRecursiveDelegate(entityId, bodyFlags);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00011703 File Offset: 0x0000F903
		public void SetBodyShape(UIntPtr entityId, UIntPtr shape)
		{
			ScriptingInterfaceOfIGameEntity.call_SetBodyShapeDelegate(entityId, shape);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00011711 File Offset: 0x0000F911
		public void SetBoneFrameToAllMeshes(UIntPtr entityPtr, int boneIndex, in MatrixFrame frame)
		{
			ScriptingInterfaceOfIGameEntity.call_SetBoneFrameToAllMeshesDelegate(entityPtr, boneIndex, in frame);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00011720 File Offset: 0x0000F920
		public void SetBoundingboxDirty(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_SetBoundingboxDirtyDelegate(entityId);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0001172D File Offset: 0x0000F92D
		public void SetCenterOfMass(UIntPtr entityId, ref Vec3 localCenterOfMass)
		{
			ScriptingInterfaceOfIGameEntity.call_SetCenterOfMassDelegate(entityId, ref localCenterOfMass);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0001173B File Offset: 0x0000F93B
		public void SetClothComponentKeepState(UIntPtr entityId, UIntPtr metaMesh, bool keepState)
		{
			ScriptingInterfaceOfIGameEntity.call_SetClothComponentKeepStateDelegate(entityId, metaMesh, keepState);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0001174A File Offset: 0x0000F94A
		public void SetClothComponentKeepStateOfAllMeshes(UIntPtr entityId, bool keepState)
		{
			ScriptingInterfaceOfIGameEntity.call_SetClothComponentKeepStateOfAllMeshesDelegate(entityId, keepState);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00011758 File Offset: 0x0000F958
		public void SetClothMaxDistanceMultiplier(UIntPtr gameEntity, float multiplier)
		{
			ScriptingInterfaceOfIGameEntity.call_SetClothMaxDistanceMultiplierDelegate(gameEntity, multiplier);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00011768 File Offset: 0x0000F968
		public void SetColorToAllMeshesWithTagRecursive(UIntPtr gameEntity, uint color, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_SetColorToAllMeshesWithTagRecursiveDelegate(gameEntity, color, array);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000117C4 File Offset: 0x0000F9C4
		public void SetContourState(UIntPtr entityId, bool alwaysVisible)
		{
			ScriptingInterfaceOfIGameEntity.call_SetContourStateDelegate(entityId, alwaysVisible);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000117D2 File Offset: 0x0000F9D2
		public void SetCostAdderForAttachedFaces(UIntPtr entityId, float cost)
		{
			ScriptingInterfaceOfIGameEntity.call_SetCostAdderForAttachedFacesDelegate(entityId, cost);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000117E0 File Offset: 0x0000F9E0
		public void SetCullMode(UIntPtr entityPtr, MBMeshCullingMode cullMode)
		{
			ScriptingInterfaceOfIGameEntity.call_SetCullModeDelegate(entityPtr, cullMode);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000117EE File Offset: 0x0000F9EE
		public void SetCustomClipPlane(UIntPtr entityId, Vec3 position, Vec3 normal, bool setForChildren)
		{
			ScriptingInterfaceOfIGameEntity.call_SetCustomClipPlaneDelegate(entityId, position, normal, setForChildren);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000117FF File Offset: 0x0000F9FF
		public void SetCustomVertexPositionEnabled(UIntPtr entityId, bool customVertexPositionEnabled)
		{
			ScriptingInterfaceOfIGameEntity.call_SetCustomVertexPositionEnabledDelegate(entityId, customVertexPositionEnabled);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0001180D File Offset: 0x0000FA0D
		public void SetDamping(UIntPtr entityId, float linearDamping, float angularDamping)
		{
			ScriptingInterfaceOfIGameEntity.call_SetDampingDelegate(entityId, linearDamping, angularDamping);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0001181C File Offset: 0x0000FA1C
		public void SetDoNotCheckVisibility(UIntPtr entityPtr, bool value)
		{
			ScriptingInterfaceOfIGameEntity.call_SetDoNotCheckVisibilityDelegate(entityPtr, value);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0001182A File Offset: 0x0000FA2A
		public void SetEnforcedMaximumLodLevel(UIntPtr entityId, int lodLevel)
		{
			ScriptingInterfaceOfIGameEntity.call_SetEnforcedMaximumLodLevelDelegate(entityId, lodLevel);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00011838 File Offset: 0x0000FA38
		public void SetEntityEnvMapVisibility(UIntPtr entityId, bool value)
		{
			ScriptingInterfaceOfIGameEntity.call_SetEntityEnvMapVisibilityDelegate(entityId, value);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00011846 File Offset: 0x0000FA46
		public void SetEntityFlags(UIntPtr entityId, EntityFlags entityFlags)
		{
			ScriptingInterfaceOfIGameEntity.call_SetEntityFlagsDelegate(entityId, entityFlags);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00011854 File Offset: 0x0000FA54
		public void SetEntityVisibilityFlags(UIntPtr entityId, EntityVisibilityFlags entityVisibilityFlags)
		{
			ScriptingInterfaceOfIGameEntity.call_SetEntityVisibilityFlagsDelegate(entityId, entityVisibilityFlags);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00011862 File Offset: 0x0000FA62
		public void SetExternalReferencesUsage(UIntPtr entityId, bool value)
		{
			ScriptingInterfaceOfIGameEntity.call_SetExternalReferencesUsageDelegate(entityId, value);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00011870 File Offset: 0x0000FA70
		public void SetFactor2Color(UIntPtr entityId, uint factor2Color)
		{
			ScriptingInterfaceOfIGameEntity.call_SetFactor2ColorDelegate(entityId, factor2Color);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0001187E File Offset: 0x0000FA7E
		public void SetFactorColor(UIntPtr entityId, uint factorColor)
		{
			ScriptingInterfaceOfIGameEntity.call_SetFactorColorDelegate(entityId, factorColor);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0001188C File Offset: 0x0000FA8C
		public void SetForceDecalsToRender(UIntPtr entityPtr, bool value)
		{
			ScriptingInterfaceOfIGameEntity.call_SetForceDecalsToRenderDelegate(entityPtr, value);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0001189A File Offset: 0x0000FA9A
		public void SetForceNotAffectedBySeason(UIntPtr entityPtr, bool value)
		{
			ScriptingInterfaceOfIGameEntity.call_SetForceNotAffectedBySeasonDelegate(entityPtr, value);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000118A8 File Offset: 0x0000FAA8
		public void SetFrameChanged(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_SetFrameChangedDelegate(entityId);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000118B5 File Offset: 0x0000FAB5
		public void SetGlobalFrame(UIntPtr entityId, in MatrixFrame frame, bool isTeleportation)
		{
			ScriptingInterfaceOfIGameEntity.call_SetGlobalFrameDelegate(entityId, in frame, isTeleportation);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000118C4 File Offset: 0x0000FAC4
		public void SetGlobalPosition(UIntPtr entityId, in Vec3 position)
		{
			ScriptingInterfaceOfIGameEntity.call_SetGlobalPositionDelegate(entityId, in position);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000118D2 File Offset: 0x0000FAD2
		public void SetHasCustomBoundingBoxValidationSystem(UIntPtr entityId, bool hasCustomBoundingBox)
		{
			ScriptingInterfaceOfIGameEntity.call_SetHasCustomBoundingBoxValidationSystemDelegate(entityId, hasCustomBoundingBox);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000118E0 File Offset: 0x0000FAE0
		public void SetLinearVelocity(UIntPtr entityPtr, Vec3 newLinearVelocity)
		{
			ScriptingInterfaceOfIGameEntity.call_SetLinearVelocityDelegate(entityPtr, newLinearVelocity);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000118EE File Offset: 0x0000FAEE
		public void SetLocalFrame(UIntPtr entityId, ref MatrixFrame frame, bool isTeleportation)
		{
			ScriptingInterfaceOfIGameEntity.call_SetLocalFrameDelegate(entityId, ref frame, isTeleportation);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000118FD File Offset: 0x0000FAFD
		public void SetLocalPosition(UIntPtr entityId, Vec3 position)
		{
			ScriptingInterfaceOfIGameEntity.call_SetLocalPositionDelegate(entityId, position);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0001190B File Offset: 0x0000FB0B
		public void SetManualGlobalBoundingBox(UIntPtr entityId, Vec3 boundingBoxStartGlobal, Vec3 boundingBoxEndGlobal)
		{
			ScriptingInterfaceOfIGameEntity.call_SetManualGlobalBoundingBoxDelegate(entityId, boundingBoxStartGlobal, boundingBoxEndGlobal);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0001191A File Offset: 0x0000FB1A
		public void SetManualLocalBoundingBox(UIntPtr entityId, in BoundingBox boundingBox)
		{
			ScriptingInterfaceOfIGameEntity.call_SetManualLocalBoundingBoxDelegate(entityId, in boundingBox);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00011928 File Offset: 0x0000FB28
		public void SetMassAndUpdateInertiaAndCenterOfMass(UIntPtr entityId, float mass)
		{
			ScriptingInterfaceOfIGameEntity.call_SetMassAndUpdateInertiaAndCenterOfMassDelegate(entityId, mass);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00011936 File Offset: 0x0000FB36
		public void SetMassSpaceInertia(UIntPtr entityId, ref Vec3 inertia)
		{
			ScriptingInterfaceOfIGameEntity.call_SetMassSpaceInertiaDelegate(entityId, ref inertia);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00011944 File Offset: 0x0000FB44
		public void SetMaterialForAllMeshes(UIntPtr entityId, UIntPtr materialPointer)
		{
			ScriptingInterfaceOfIGameEntity.call_SetMaterialForAllMeshesDelegate(entityId, materialPointer);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00011952 File Offset: 0x0000FB52
		public void SetMaxDepenetrationVelocity(UIntPtr entityId, float maxDepenetrationVelocity)
		{
			ScriptingInterfaceOfIGameEntity.call_SetMaxDepenetrationVelocityDelegate(entityId, maxDepenetrationVelocity);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00011960 File Offset: 0x0000FB60
		public void SetMobility(UIntPtr entityId, GameEntity.Mobility mobility)
		{
			ScriptingInterfaceOfIGameEntity.call_SetMobilityDelegate(entityId, mobility);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0001196E File Offset: 0x0000FB6E
		public void SetMorphFrameOfComponents(UIntPtr entityId, float value)
		{
			ScriptingInterfaceOfIGameEntity.call_SetMorphFrameOfComponentsDelegate(entityId, value);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0001197C File Offset: 0x0000FB7C
		public void SetName(UIntPtr entityId, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_SetNameDelegate(entityId, array);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000119D8 File Offset: 0x0000FBD8
		public void SetNativeScriptComponentVariable(UIntPtr entityPtr, string className, string fieldName, ref ScriptComponentFieldHolder data, RglScriptFieldType variableType)
		{
			byte[] array = null;
			if (className != null)
			{
				int byteCount = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(className);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(className, 0, className.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (fieldName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIGameEntity._utf8.GetByteCount(fieldName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIGameEntity._utf8.GetBytes(fieldName, 0, fieldName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIGameEntity.call_SetNativeScriptComponentVariableDelegate(entityPtr, array, array2, ref data, variableType);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00011A7A File Offset: 0x0000FC7A
		public void SetPhysicsMoveToBatched(UIntPtr entityId, bool value)
		{
			ScriptingInterfaceOfIGameEntity.call_SetPhysicsMoveToBatchedDelegate(entityId, value);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00011A88 File Offset: 0x0000FC88
		public void SetPhysicsState(UIntPtr entityId, bool isEnabled, bool setChildren)
		{
			ScriptingInterfaceOfIGameEntity.call_SetPhysicsStateDelegate(entityId, isEnabled, setChildren);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00011A97 File Offset: 0x0000FC97
		public void SetPhysicsStateOnlyVariable(UIntPtr entityId, bool isEnabled, bool setChildren)
		{
			ScriptingInterfaceOfIGameEntity.call_SetPhysicsStateOnlyVariableDelegate(entityId, isEnabled, setChildren);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00011AA8 File Offset: 0x0000FCA8
		public void SetPositionsForAttachedNavmeshVertices(UIntPtr entityId, int[] indices, int indexCount, Vec3[] positions)
		{
			PinnedArrayData<int> pinnedArrayData = new PinnedArrayData<int>(indices, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			PinnedArrayData<Vec3> pinnedArrayData2 = new PinnedArrayData<Vec3>(positions, false);
			IntPtr pointer2 = pinnedArrayData2.Pointer;
			ScriptingInterfaceOfIGameEntity.call_SetPositionsForAttachedNavmeshVerticesDelegate(entityId, pointer, indexCount, pointer2);
			pinnedArrayData.Dispose();
			pinnedArrayData2.Dispose();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00011AF4 File Offset: 0x0000FCF4
		public void SetPreviousFrameInvalid(UIntPtr gameEntity)
		{
			ScriptingInterfaceOfIGameEntity.call_SetPreviousFrameInvalidDelegate(gameEntity);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00011B01 File Offset: 0x0000FD01
		public void SetReadyToRender(UIntPtr entityId, bool ready)
		{
			ScriptingInterfaceOfIGameEntity.call_SetReadyToRenderDelegate(entityId, ready);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00011B0F File Offset: 0x0000FD0F
		public void SetRuntimeEmissionRateMultiplier(UIntPtr entityId, float emission_rate_multiplier)
		{
			ScriptingInterfaceOfIGameEntity.call_SetRuntimeEmissionRateMultiplierDelegate(entityId, emission_rate_multiplier);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00011B1D File Offset: 0x0000FD1D
		public void SetSkeleton(UIntPtr entityId, UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfIGameEntity.call_SetSkeletonDelegate(entityId, skeletonPointer);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00011B2B File Offset: 0x0000FD2B
		public void SetSolverIterationCounts(UIntPtr entityId, int positionIterationCount, int velocityIterationCount)
		{
			ScriptingInterfaceOfIGameEntity.call_SetSolverIterationCountsDelegate(entityId, positionIterationCount, velocityIterationCount);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00011B3A File Offset: 0x0000FD3A
		public void SetupAdditionalBoneBufferForMeshes(UIntPtr entityPtr, int boneCount)
		{
			ScriptingInterfaceOfIGameEntity.call_SetupAdditionalBoneBufferForMeshesDelegate(entityPtr, boneCount);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00011B48 File Offset: 0x0000FD48
		public void SetUpdateValidityOnFrameChangedOfFacesWithId(UIntPtr entityId, int faceGroupId, bool updateValidity)
		{
			ScriptingInterfaceOfIGameEntity.call_SetUpdateValidityOnFrameChangedOfFacesWithIdDelegate(entityId, faceGroupId, updateValidity);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00011B57 File Offset: 0x0000FD57
		public void SetUpgradeLevelMask(UIntPtr prefab, uint mask)
		{
			ScriptingInterfaceOfIGameEntity.call_SetUpgradeLevelMaskDelegate(prefab, mask);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00011B65 File Offset: 0x0000FD65
		public void SetVectorArgument(UIntPtr entityId, float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			ScriptingInterfaceOfIGameEntity.call_SetVectorArgumentDelegate(entityId, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00011B78 File Offset: 0x0000FD78
		public void SetVelocityLimits(UIntPtr entityId, float maxLinearVelocity, float maxAngularVelocity)
		{
			ScriptingInterfaceOfIGameEntity.call_SetVelocityLimitsDelegate(entityId, maxLinearVelocity, maxAngularVelocity);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00011B87 File Offset: 0x0000FD87
		public void SetVisibilityExcludeParents(UIntPtr entityId, bool visibility)
		{
			ScriptingInterfaceOfIGameEntity.call_SetVisibilityExcludeParentsDelegate(entityId, visibility);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00011B95 File Offset: 0x0000FD95
		public void SetVisualRecordWakeParams(UIntPtr visualRecord, in Vec3 wakeParams)
		{
			ScriptingInterfaceOfIGameEntity.call_SetVisualRecordWakeParamsDelegate(visualRecord, in wakeParams);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00011BA3 File Offset: 0x0000FDA3
		public void SetWaterSDFClipData(UIntPtr entityId, int slotIndex, in MatrixFrame frame, bool visibility)
		{
			ScriptingInterfaceOfIGameEntity.call_SetWaterSDFClipDataDelegate(entityId, slotIndex, in frame, visibility);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00011BB4 File Offset: 0x0000FDB4
		public void SetWaterVisualRecordFrameAndDt(UIntPtr entityPointer, UIntPtr visualPrefab, in MatrixFrame frame, float dt)
		{
			ScriptingInterfaceOfIGameEntity.call_SetWaterVisualRecordFrameAndDtDelegate(entityPointer, visualPrefab, in frame, dt);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00011BC5 File Offset: 0x0000FDC5
		public void SwapPhysxShapeInEntity(UIntPtr entityPtr, UIntPtr oldShape, UIntPtr newShape, bool isVariable)
		{
			ScriptingInterfaceOfIGameEntity.call_SwapPhysxShapeInEntityDelegate(entityPtr, oldShape, newShape, isVariable);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00011BD6 File Offset: 0x0000FDD6
		public void UpdateAttachedNavigationMeshFaces(UIntPtr entityId)
		{
			ScriptingInterfaceOfIGameEntity.call_UpdateAttachedNavigationMeshFacesDelegate(entityId);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00011BE3 File Offset: 0x0000FDE3
		public void UpdateBodyRestOffset(UIntPtr entityId, float restOffset)
		{
			ScriptingInterfaceOfIGameEntity.call_UpdateBodyRestOffsetDelegate(entityId, restOffset);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00011BF1 File Offset: 0x0000FDF1
		public void UpdateGlobalBounds(UIntPtr entityPointer)
		{
			ScriptingInterfaceOfIGameEntity.call_UpdateGlobalBoundsDelegate(entityPointer);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00011BFE File Offset: 0x0000FDFE
		public void UpdateHullWaterEffectFrames(UIntPtr entityPointer, UIntPtr visualPrefab)
		{
			ScriptingInterfaceOfIGameEntity.call_UpdateHullWaterEffectFramesDelegate(entityPointer, visualPrefab);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00011C0C File Offset: 0x0000FE0C
		public void UpdateTriadFrameForEditor(UIntPtr meshPointer)
		{
			ScriptingInterfaceOfIGameEntity.call_UpdateTriadFrameForEditorDelegate(meshPointer);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00011C19 File Offset: 0x0000FE19
		public void UpdateVisibilityMask(UIntPtr entityPtr)
		{
			ScriptingInterfaceOfIGameEntity.call_UpdateVisibilityMaskDelegate(entityPtr);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00011C26 File Offset: 0x0000FE26
		public void ValidateBoundingBox(UIntPtr entityPointer)
		{
			ScriptingInterfaceOfIGameEntity.call_ValidateBoundingBoxDelegate(entityPointer);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00011C47 File Offset: 0x0000FE47
		void IGameEntity.SetWaterSDFClipData(UIntPtr entityId, int slotIndex, in MatrixFrame frame, bool visibility)
		{
			this.SetWaterSDFClipData(entityId, slotIndex, in frame, visibility);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00011C54 File Offset: 0x0000FE54
		void IGameEntity.SetGlobalFrame(UIntPtr entityId, in MatrixFrame frame, bool isTeleportation)
		{
			this.SetGlobalFrame(entityId, in frame, isTeleportation);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00011C5F File Offset: 0x0000FE5F
		void IGameEntity.ComputeVelocityDeltaFromImpulse(UIntPtr entityPtr, in Vec3 impulsiveForce, in Vec3 impulsiveTorque, out Vec3 deltaLinearVelocity, out Vec3 deltaAngularVelocity)
		{
			this.ComputeVelocityDeltaFromImpulse(entityPtr, in impulsiveForce, in impulsiveTorque, out deltaLinearVelocity, out deltaAngularVelocity);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00011C6E File Offset: 0x0000FE6E
		void IGameEntity.SetGlobalPosition(UIntPtr entityId, in Vec3 position)
		{
			this.SetGlobalPosition(entityId, in position);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00011C78 File Offset: 0x0000FE78
		void IGameEntity.SetVisualRecordWakeParams(UIntPtr visualRecord, in Vec3 wakeParams)
		{
			this.SetVisualRecordWakeParams(visualRecord, in wakeParams);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00011C82 File Offset: 0x0000FE82
		void IGameEntity.ChangeResolutionMultiplierOfWaterVisual(UIntPtr visualPrefab, float multiplier, in Vec3 waterEffectsBB)
		{
			this.ChangeResolutionMultiplierOfWaterVisual(visualPrefab, multiplier, in waterEffectsBB);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00011C8D File Offset: 0x0000FE8D
		void IGameEntity.SetWaterVisualRecordFrameAndDt(UIntPtr entityPointer, UIntPtr visualPrefab, in MatrixFrame frame, float dt)
		{
			this.SetWaterVisualRecordFrameAndDt(entityPointer, visualPrefab, in frame, dt);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00011C9A File Offset: 0x0000FE9A
		void IGameEntity.AddSplashPositionToWaterVisualRecord(UIntPtr entityPointer, UIntPtr visualPrefab, in Vec3 position)
		{
			this.AddSplashPositionToWaterVisualRecord(entityPointer, visualPrefab, in position);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00011CA5 File Offset: 0x0000FEA5
		void IGameEntity.GetAttachedNavmeshFaceVertexIndices(UIntPtr entityId, in PathFaceRecord faceRecord, int[] indices)
		{
			this.GetAttachedNavmeshFaceVertexIndices(entityId, in faceRecord, indices);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00011CB0 File Offset: 0x0000FEB0
		float IGameEntity.GetWaterLevelAtPosition(UIntPtr entityId, in Vec2 position, bool useWaterRenderer, bool checkWaterBodyEntities)
		{
			return this.GetWaterLevelAtPosition(entityId, in position, useWaterRenderer, checkWaterBodyEntities);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00011CBD File Offset: 0x0000FEBD
		bool IGameEntity.RayHitEntity(UIntPtr entityId, in Vec3 rayOrigin, in Vec3 rayDirection, float maxLength, ref float resultLength)
		{
			return this.RayHitEntity(entityId, in rayOrigin, in rayDirection, maxLength, ref resultLength);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00011CCC File Offset: 0x0000FECC
		bool IGameEntity.RayHitEntityWithNormal(UIntPtr entityId, in Vec3 rayOrigin, in Vec3 rayDirection, float maxLength, ref Vec3 resultNormal, ref float resultLength)
		{
			return this.RayHitEntityWithNormal(entityId, in rayOrigin, in rayDirection, maxLength, ref resultNormal, ref resultLength);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00011CDD File Offset: 0x0000FEDD
		void IGameEntity.SetManualLocalBoundingBox(UIntPtr entityId, in BoundingBox boundingBox)
		{
			this.SetManualLocalBoundingBox(entityId, in boundingBox);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00011CE7 File Offset: 0x0000FEE7
		void IGameEntity.RelaxLocalBoundingBox(UIntPtr entityId, in BoundingBox boundingBox)
		{
			this.RelaxLocalBoundingBox(entityId, in boundingBox);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00011CF1 File Offset: 0x0000FEF1
		void IGameEntity.SetAngularVelocity(UIntPtr entityPtr, in Vec3 newAngularVelocity)
		{
			this.SetAngularVelocity(entityPtr, in newAngularVelocity);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00011CFB File Offset: 0x0000FEFB
		void IGameEntity.SetBoneFrameToAllMeshes(UIntPtr entityPtr, int boneIndex, in MatrixFrame frame)
		{
			this.SetBoneFrameToAllMeshes(entityPtr, boneIndex, in frame);
		}

		// Token: 0x040000B5 RID: 181
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040000B6 RID: 182
		public static ScriptingInterfaceOfIGameEntity.ActivateRagdollDelegate call_ActivateRagdollDelegate;

		// Token: 0x040000B7 RID: 183
		public static ScriptingInterfaceOfIGameEntity.AddAllMeshesOfGameEntityDelegate call_AddAllMeshesOfGameEntityDelegate;

		// Token: 0x040000B8 RID: 184
		public static ScriptingInterfaceOfIGameEntity.AddCapsuleAsBodyDelegate call_AddCapsuleAsBodyDelegate;

		// Token: 0x040000B9 RID: 185
		public static ScriptingInterfaceOfIGameEntity.AddChildDelegate call_AddChildDelegate;

		// Token: 0x040000BA RID: 186
		public static ScriptingInterfaceOfIGameEntity.AddComponentDelegate call_AddComponentDelegate;

		// Token: 0x040000BB RID: 187
		public static ScriptingInterfaceOfIGameEntity.AddDistanceJointDelegate call_AddDistanceJointDelegate;

		// Token: 0x040000BC RID: 188
		public static ScriptingInterfaceOfIGameEntity.AddDistanceJointWithFramesDelegate call_AddDistanceJointWithFramesDelegate;

		// Token: 0x040000BD RID: 189
		public static ScriptingInterfaceOfIGameEntity.AddEditDataUserToAllMeshesDelegate call_AddEditDataUserToAllMeshesDelegate;

		// Token: 0x040000BE RID: 190
		public static ScriptingInterfaceOfIGameEntity.AddLightDelegate call_AddLightDelegate;

		// Token: 0x040000BF RID: 191
		public static ScriptingInterfaceOfIGameEntity.AddMeshDelegate call_AddMeshDelegate;

		// Token: 0x040000C0 RID: 192
		public static ScriptingInterfaceOfIGameEntity.AddMeshToBoneDelegate call_AddMeshToBoneDelegate;

		// Token: 0x040000C1 RID: 193
		public static ScriptingInterfaceOfIGameEntity.AddMultiMeshDelegate call_AddMultiMeshDelegate;

		// Token: 0x040000C2 RID: 194
		public static ScriptingInterfaceOfIGameEntity.AddMultiMeshToSkeletonDelegate call_AddMultiMeshToSkeletonDelegate;

		// Token: 0x040000C3 RID: 195
		public static ScriptingInterfaceOfIGameEntity.AddMultiMeshToSkeletonBoneDelegate call_AddMultiMeshToSkeletonBoneDelegate;

		// Token: 0x040000C4 RID: 196
		public static ScriptingInterfaceOfIGameEntity.AddParticleSystemComponentDelegate call_AddParticleSystemComponentDelegate;

		// Token: 0x040000C5 RID: 197
		public static ScriptingInterfaceOfIGameEntity.AddPhysicsDelegate call_AddPhysicsDelegate;

		// Token: 0x040000C6 RID: 198
		public static ScriptingInterfaceOfIGameEntity.AddSphereAsBodyDelegate call_AddSphereAsBodyDelegate;

		// Token: 0x040000C7 RID: 199
		public static ScriptingInterfaceOfIGameEntity.AddSplashPositionToWaterVisualRecordDelegate call_AddSplashPositionToWaterVisualRecordDelegate;

		// Token: 0x040000C8 RID: 200
		public static ScriptingInterfaceOfIGameEntity.AddTagDelegate call_AddTagDelegate;

		// Token: 0x040000C9 RID: 201
		public static ScriptingInterfaceOfIGameEntity.ApplyAccelerationToDynamicBodyDelegate call_ApplyAccelerationToDynamicBodyDelegate;

		// Token: 0x040000CA RID: 202
		public static ScriptingInterfaceOfIGameEntity.ApplyForceToDynamicBodyDelegate call_ApplyForceToDynamicBodyDelegate;

		// Token: 0x040000CB RID: 203
		public static ScriptingInterfaceOfIGameEntity.ApplyGlobalForceAtLocalPosToDynamicBodyDelegate call_ApplyGlobalForceAtLocalPosToDynamicBodyDelegate;

		// Token: 0x040000CC RID: 204
		public static ScriptingInterfaceOfIGameEntity.ApplyLocalForceAtLocalPosToDynamicBodyDelegate call_ApplyLocalForceAtLocalPosToDynamicBodyDelegate;

		// Token: 0x040000CD RID: 205
		public static ScriptingInterfaceOfIGameEntity.ApplyLocalImpulseToDynamicBodyDelegate call_ApplyLocalImpulseToDynamicBodyDelegate;

		// Token: 0x040000CE RID: 206
		public static ScriptingInterfaceOfIGameEntity.ApplyTorqueToDynamicBodyDelegate call_ApplyTorqueToDynamicBodyDelegate;

		// Token: 0x040000CF RID: 207
		public static ScriptingInterfaceOfIGameEntity.AttachNavigationMeshFacesDelegate call_AttachNavigationMeshFacesDelegate;

		// Token: 0x040000D0 RID: 208
		public static ScriptingInterfaceOfIGameEntity.BreakPrefabDelegate call_BreakPrefabDelegate;

		// Token: 0x040000D1 RID: 209
		public static ScriptingInterfaceOfIGameEntity.BurstEntityParticleDelegate call_BurstEntityParticleDelegate;

		// Token: 0x040000D2 RID: 210
		public static ScriptingInterfaceOfIGameEntity.CallScriptCallbacksDelegate call_CallScriptCallbacksDelegate;

		// Token: 0x040000D3 RID: 211
		public static ScriptingInterfaceOfIGameEntity.ChangeMetaMeshOrRemoveItIfNotExistsDelegate call_ChangeMetaMeshOrRemoveItIfNotExistsDelegate;

		// Token: 0x040000D4 RID: 212
		public static ScriptingInterfaceOfIGameEntity.ChangeResolutionMultiplierOfWaterVisualDelegate call_ChangeResolutionMultiplierOfWaterVisualDelegate;

		// Token: 0x040000D5 RID: 213
		public static ScriptingInterfaceOfIGameEntity.CheckIsPrefabLinkRootPrefabDelegate call_CheckIsPrefabLinkRootPrefabDelegate;

		// Token: 0x040000D6 RID: 214
		public static ScriptingInterfaceOfIGameEntity.CheckPointWithOrientedBoundingBoxDelegate call_CheckPointWithOrientedBoundingBoxDelegate;

		// Token: 0x040000D7 RID: 215
		public static ScriptingInterfaceOfIGameEntity.CheckResourcesDelegate call_CheckResourcesDelegate;

		// Token: 0x040000D8 RID: 216
		public static ScriptingInterfaceOfIGameEntity.ClearComponentsDelegate call_ClearComponentsDelegate;

		// Token: 0x040000D9 RID: 217
		public static ScriptingInterfaceOfIGameEntity.ClearEntityComponentsDelegate call_ClearEntityComponentsDelegate;

		// Token: 0x040000DA RID: 218
		public static ScriptingInterfaceOfIGameEntity.ClearOnlyOwnComponentsDelegate call_ClearOnlyOwnComponentsDelegate;

		// Token: 0x040000DB RID: 219
		public static ScriptingInterfaceOfIGameEntity.ComputeTrajectoryVolumeDelegate call_ComputeTrajectoryVolumeDelegate;

		// Token: 0x040000DC RID: 220
		public static ScriptingInterfaceOfIGameEntity.ComputeVelocityDeltaFromImpulseDelegate call_ComputeVelocityDeltaFromImpulseDelegate;

		// Token: 0x040000DD RID: 221
		public static ScriptingInterfaceOfIGameEntity.ConvertDynamicBodyToRayCastDelegate call_ConvertDynamicBodyToRayCastDelegate;

		// Token: 0x040000DE RID: 222
		public static ScriptingInterfaceOfIGameEntity.CookTrianglePhysxMeshDelegate call_CookTrianglePhysxMeshDelegate;

		// Token: 0x040000DF RID: 223
		public static ScriptingInterfaceOfIGameEntity.CopyComponentsToSkeletonDelegate call_CopyComponentsToSkeletonDelegate;

		// Token: 0x040000E0 RID: 224
		public static ScriptingInterfaceOfIGameEntity.CopyFromPrefabDelegate call_CopyFromPrefabDelegate;

		// Token: 0x040000E1 RID: 225
		public static ScriptingInterfaceOfIGameEntity.CopyScriptComponentFromAnotherEntityDelegate call_CopyScriptComponentFromAnotherEntityDelegate;

		// Token: 0x040000E2 RID: 226
		public static ScriptingInterfaceOfIGameEntity.CreateAndAddScriptComponentDelegate call_CreateAndAddScriptComponentDelegate;

		// Token: 0x040000E3 RID: 227
		public static ScriptingInterfaceOfIGameEntity.CreateEmptyDelegate call_CreateEmptyDelegate;

		// Token: 0x040000E4 RID: 228
		public static ScriptingInterfaceOfIGameEntity.CreateEmptyPhysxShapeDelegate call_CreateEmptyPhysxShapeDelegate;

		// Token: 0x040000E5 RID: 229
		public static ScriptingInterfaceOfIGameEntity.CreateEmptyWithoutSceneDelegate call_CreateEmptyWithoutSceneDelegate;

		// Token: 0x040000E6 RID: 230
		public static ScriptingInterfaceOfIGameEntity.CreateFromPrefabDelegate call_CreateFromPrefabDelegate;

		// Token: 0x040000E7 RID: 231
		public static ScriptingInterfaceOfIGameEntity.CreateFromPrefabWithInitialFrameAndRestOffsetDelegate call_CreateFromPrefabWithInitialFrameAndRestOffsetDelegate;

		// Token: 0x040000E8 RID: 232
		public static ScriptingInterfaceOfIGameEntity.CreatePhysxCookingInstanceDelegate call_CreatePhysxCookingInstanceDelegate;

		// Token: 0x040000E9 RID: 233
		public static ScriptingInterfaceOfIGameEntity.CreateVariableRatePhysicsDelegate call_CreateVariableRatePhysicsDelegate;

		// Token: 0x040000EA RID: 234
		public static ScriptingInterfaceOfIGameEntity.DeleteEmptyShapeDelegate call_DeleteEmptyShapeDelegate;

		// Token: 0x040000EB RID: 235
		public static ScriptingInterfaceOfIGameEntity.DeletePhysxCookingInstanceDelegate call_DeletePhysxCookingInstanceDelegate;

		// Token: 0x040000EC RID: 236
		public static ScriptingInterfaceOfIGameEntity.DeRegisterWaterMeshMaterialsDelegate call_DeRegisterWaterMeshMaterialsDelegate;

		// Token: 0x040000ED RID: 237
		public static ScriptingInterfaceOfIGameEntity.DeRegisterWaterSDFClipDelegate call_DeRegisterWaterSDFClipDelegate;

		// Token: 0x040000EE RID: 238
		public static ScriptingInterfaceOfIGameEntity.DeselectEntityOnEditorDelegate call_DeselectEntityOnEditorDelegate;

		// Token: 0x040000EF RID: 239
		public static ScriptingInterfaceOfIGameEntity.DetachAllAttachedNavigationMeshFacesDelegate call_DetachAllAttachedNavigationMeshFacesDelegate;

		// Token: 0x040000F0 RID: 240
		public static ScriptingInterfaceOfIGameEntity.DisableContourDelegate call_DisableContourDelegate;

		// Token: 0x040000F1 RID: 241
		public static ScriptingInterfaceOfIGameEntity.DisableDynamicBodySimulationDelegate call_DisableDynamicBodySimulationDelegate;

		// Token: 0x040000F2 RID: 242
		public static ScriptingInterfaceOfIGameEntity.DisableGravityDelegate call_DisableGravityDelegate;

		// Token: 0x040000F3 RID: 243
		public static ScriptingInterfaceOfIGameEntity.EnableDynamicBodyDelegate call_EnableDynamicBodyDelegate;

		// Token: 0x040000F4 RID: 244
		public static ScriptingInterfaceOfIGameEntity.FindWithNameDelegate call_FindWithNameDelegate;

		// Token: 0x040000F5 RID: 245
		public static ScriptingInterfaceOfIGameEntity.FreezeDelegate call_FreezeDelegate;

		// Token: 0x040000F6 RID: 246
		public static ScriptingInterfaceOfIGameEntity.GetAngularVelocityDelegate call_GetAngularVelocityDelegate;

		// Token: 0x040000F7 RID: 247
		public static ScriptingInterfaceOfIGameEntity.GetAttachedNavmeshFaceCountDelegate call_GetAttachedNavmeshFaceCountDelegate;

		// Token: 0x040000F8 RID: 248
		public static ScriptingInterfaceOfIGameEntity.GetAttachedNavmeshFaceRecordsDelegate call_GetAttachedNavmeshFaceRecordsDelegate;

		// Token: 0x040000F9 RID: 249
		public static ScriptingInterfaceOfIGameEntity.GetAttachedNavmeshFaceVertexIndicesDelegate call_GetAttachedNavmeshFaceVertexIndicesDelegate;

		// Token: 0x040000FA RID: 250
		public static ScriptingInterfaceOfIGameEntity.GetBodyFlagsDelegate call_GetBodyFlagsDelegate;

		// Token: 0x040000FB RID: 251
		public static ScriptingInterfaceOfIGameEntity.GetBodyShapeDelegate call_GetBodyShapeDelegate;

		// Token: 0x040000FC RID: 252
		public static ScriptingInterfaceOfIGameEntity.GetBodyVisualWorldTransformDelegate call_GetBodyVisualWorldTransformDelegate;

		// Token: 0x040000FD RID: 253
		public static ScriptingInterfaceOfIGameEntity.GetBodyWorldTransformDelegate call_GetBodyWorldTransformDelegate;

		// Token: 0x040000FE RID: 254
		public static ScriptingInterfaceOfIGameEntity.GetBoneCountDelegate call_GetBoneCountDelegate;

		// Token: 0x040000FF RID: 255
		public static ScriptingInterfaceOfIGameEntity.GetBoneEntitialFrameWithIndexDelegate call_GetBoneEntitialFrameWithIndexDelegate;

		// Token: 0x04000100 RID: 256
		public static ScriptingInterfaceOfIGameEntity.GetBoneEntitialFrameWithNameDelegate call_GetBoneEntitialFrameWithNameDelegate;

		// Token: 0x04000101 RID: 257
		public static ScriptingInterfaceOfIGameEntity.GetBoundingBoxMaxDelegate call_GetBoundingBoxMaxDelegate;

		// Token: 0x04000102 RID: 258
		public static ScriptingInterfaceOfIGameEntity.GetBoundingBoxMinDelegate call_GetBoundingBoxMinDelegate;

		// Token: 0x04000103 RID: 259
		public static ScriptingInterfaceOfIGameEntity.GetCameraParamsFromCameraScriptDelegate call_GetCameraParamsFromCameraScriptDelegate;

		// Token: 0x04000104 RID: 260
		public static ScriptingInterfaceOfIGameEntity.GetCenterOfMassDelegate call_GetCenterOfMassDelegate;

		// Token: 0x04000105 RID: 261
		public static ScriptingInterfaceOfIGameEntity.GetChildDelegate call_GetChildDelegate;

		// Token: 0x04000106 RID: 262
		public static ScriptingInterfaceOfIGameEntity.GetChildCountDelegate call_GetChildCountDelegate;

		// Token: 0x04000107 RID: 263
		public static ScriptingInterfaceOfIGameEntity.GetChildPointerDelegate call_GetChildPointerDelegate;

		// Token: 0x04000108 RID: 264
		public static ScriptingInterfaceOfIGameEntity.GetComponentAtIndexDelegate call_GetComponentAtIndexDelegate;

		// Token: 0x04000109 RID: 265
		public static ScriptingInterfaceOfIGameEntity.GetComponentCountDelegate call_GetComponentCountDelegate;

		// Token: 0x0400010A RID: 266
		public static ScriptingInterfaceOfIGameEntity.GetEditModeLevelVisibilityDelegate call_GetEditModeLevelVisibilityDelegate;

		// Token: 0x0400010B RID: 267
		public static ScriptingInterfaceOfIGameEntity.GetEntityFlagsDelegate call_GetEntityFlagsDelegate;

		// Token: 0x0400010C RID: 268
		public static ScriptingInterfaceOfIGameEntity.GetEntityVisibilityFlagsDelegate call_GetEntityVisibilityFlagsDelegate;

		// Token: 0x0400010D RID: 269
		public static ScriptingInterfaceOfIGameEntity.GetFactorColorDelegate call_GetFactorColorDelegate;

		// Token: 0x0400010E RID: 270
		public static ScriptingInterfaceOfIGameEntity.GetFirstChildWithTagRecursiveDelegate call_GetFirstChildWithTagRecursiveDelegate;

		// Token: 0x0400010F RID: 271
		public static ScriptingInterfaceOfIGameEntity.GetFirstEntityWithTagDelegate call_GetFirstEntityWithTagDelegate;

		// Token: 0x04000110 RID: 272
		public static ScriptingInterfaceOfIGameEntity.GetFirstEntityWithTagExpressionDelegate call_GetFirstEntityWithTagExpressionDelegate;

		// Token: 0x04000111 RID: 273
		public static ScriptingInterfaceOfIGameEntity.GetFirstMeshDelegate call_GetFirstMeshDelegate;

		// Token: 0x04000112 RID: 274
		public static ScriptingInterfaceOfIGameEntity.GetGlobalBoundingBoxDelegate call_GetGlobalBoundingBoxDelegate;

		// Token: 0x04000113 RID: 275
		public static ScriptingInterfaceOfIGameEntity.GetGlobalBoxMaxDelegate call_GetGlobalBoxMaxDelegate;

		// Token: 0x04000114 RID: 276
		public static ScriptingInterfaceOfIGameEntity.GetGlobalBoxMinDelegate call_GetGlobalBoxMinDelegate;

		// Token: 0x04000115 RID: 277
		public static ScriptingInterfaceOfIGameEntity.GetGlobalFrameDelegate call_GetGlobalFrameDelegate;

		// Token: 0x04000116 RID: 278
		public static ScriptingInterfaceOfIGameEntity.GetGlobalFrameImpreciseForFixedTickDelegate call_GetGlobalFrameImpreciseForFixedTickDelegate;

		// Token: 0x04000117 RID: 279
		public static ScriptingInterfaceOfIGameEntity.GetGlobalScaleDelegate call_GetGlobalScaleDelegate;

		// Token: 0x04000118 RID: 280
		public static ScriptingInterfaceOfIGameEntity.GetGlobalWindStrengthVectorOfSceneDelegate call_GetGlobalWindStrengthVectorOfSceneDelegate;

		// Token: 0x04000119 RID: 281
		public static ScriptingInterfaceOfIGameEntity.GetGlobalWindVelocityOfSceneDelegate call_GetGlobalWindVelocityOfSceneDelegate;

		// Token: 0x0400011A RID: 282
		public static ScriptingInterfaceOfIGameEntity.GetGlobalWindVelocityWithGustNoiseOfSceneDelegate call_GetGlobalWindVelocityWithGustNoiseOfSceneDelegate;

		// Token: 0x0400011B RID: 283
		public static ScriptingInterfaceOfIGameEntity.GetGuidDelegate call_GetGuidDelegate;

		// Token: 0x0400011C RID: 284
		public static ScriptingInterfaceOfIGameEntity.GetLastFinalRenderCameraPositionOfSceneDelegate call_GetLastFinalRenderCameraPositionOfSceneDelegate;

		// Token: 0x0400011D RID: 285
		public static ScriptingInterfaceOfIGameEntity.GetLightDelegate call_GetLightDelegate;

		// Token: 0x0400011E RID: 286
		public static ScriptingInterfaceOfIGameEntity.GetLinearVelocityDelegate call_GetLinearVelocityDelegate;

		// Token: 0x0400011F RID: 287
		public static ScriptingInterfaceOfIGameEntity.GetLocalBoundingBoxDelegate call_GetLocalBoundingBoxDelegate;

		// Token: 0x04000120 RID: 288
		public static ScriptingInterfaceOfIGameEntity.GetLocalFrameDelegate call_GetLocalFrameDelegate;

		// Token: 0x04000121 RID: 289
		public static ScriptingInterfaceOfIGameEntity.GetLocalPhysicsBoundingBoxDelegate call_GetLocalPhysicsBoundingBoxDelegate;

		// Token: 0x04000122 RID: 290
		public static ScriptingInterfaceOfIGameEntity.GetLodLevelForDistanceSqDelegate call_GetLodLevelForDistanceSqDelegate;

		// Token: 0x04000123 RID: 291
		public static ScriptingInterfaceOfIGameEntity.GetMassDelegate call_GetMassDelegate;

		// Token: 0x04000124 RID: 292
		public static ScriptingInterfaceOfIGameEntity.GetMassSpaceInertiaDelegate call_GetMassSpaceInertiaDelegate;

		// Token: 0x04000125 RID: 293
		public static ScriptingInterfaceOfIGameEntity.GetMassSpaceInverseInertiaDelegate call_GetMassSpaceInverseInertiaDelegate;

		// Token: 0x04000126 RID: 294
		public static ScriptingInterfaceOfIGameEntity.GetMeshBendedPositionDelegate call_GetMeshBendedPositionDelegate;

		// Token: 0x04000127 RID: 295
		public static ScriptingInterfaceOfIGameEntity.GetMobilityDelegate call_GetMobilityDelegate;

		// Token: 0x04000128 RID: 296
		public static ScriptingInterfaceOfIGameEntity.GetNameDelegate call_GetNameDelegate;

		// Token: 0x04000129 RID: 297
		public static ScriptingInterfaceOfIGameEntity.GetNativeScriptComponentVariableDelegate call_GetNativeScriptComponentVariableDelegate;

		// Token: 0x0400012A RID: 298
		public static ScriptingInterfaceOfIGameEntity.GetNextEntityWithTagDelegate call_GetNextEntityWithTagDelegate;

		// Token: 0x0400012B RID: 299
		public static ScriptingInterfaceOfIGameEntity.GetNextEntityWithTagExpressionDelegate call_GetNextEntityWithTagExpressionDelegate;

		// Token: 0x0400012C RID: 300
		public static ScriptingInterfaceOfIGameEntity.GetNextPrefabDelegate call_GetNextPrefabDelegate;

		// Token: 0x0400012D RID: 301
		public static ScriptingInterfaceOfIGameEntity.GetOldPrefabNameDelegate call_GetOldPrefabNameDelegate;

		// Token: 0x0400012E RID: 302
		public static ScriptingInterfaceOfIGameEntity.GetParentDelegate call_GetParentDelegate;

		// Token: 0x0400012F RID: 303
		public static ScriptingInterfaceOfIGameEntity.GetParentPointerDelegate call_GetParentPointerDelegate;

		// Token: 0x04000130 RID: 304
		public static ScriptingInterfaceOfIGameEntity.GetPhysicsDescBodyFlagsDelegate call_GetPhysicsDescBodyFlagsDelegate;

		// Token: 0x04000131 RID: 305
		public static ScriptingInterfaceOfIGameEntity.GetPhysicsMaterialIndexDelegate call_GetPhysicsMaterialIndexDelegate;

		// Token: 0x04000132 RID: 306
		public static ScriptingInterfaceOfIGameEntity.GetPhysicsMinMaxDelegate call_GetPhysicsMinMaxDelegate;

		// Token: 0x04000133 RID: 307
		public static ScriptingInterfaceOfIGameEntity.GetPhysicsStateDelegate call_GetPhysicsStateDelegate;

		// Token: 0x04000134 RID: 308
		public static ScriptingInterfaceOfIGameEntity.GetPhysicsTriangleCountDelegate call_GetPhysicsTriangleCountDelegate;

		// Token: 0x04000135 RID: 309
		public static ScriptingInterfaceOfIGameEntity.GetPrefabNameDelegate call_GetPrefabNameDelegate;

		// Token: 0x04000136 RID: 310
		public static ScriptingInterfaceOfIGameEntity.GetPreviousGlobalFrameDelegate call_GetPreviousGlobalFrameDelegate;

		// Token: 0x04000137 RID: 311
		public static ScriptingInterfaceOfIGameEntity.GetQuickBoneEntitialFrameDelegate call_GetQuickBoneEntitialFrameDelegate;

		// Token: 0x04000138 RID: 312
		public static ScriptingInterfaceOfIGameEntity.GetRadiusDelegate call_GetRadiusDelegate;

		// Token: 0x04000139 RID: 313
		public static ScriptingInterfaceOfIGameEntity.GetRootParentPointerDelegate call_GetRootParentPointerDelegate;

		// Token: 0x0400013A RID: 314
		public static ScriptingInterfaceOfIGameEntity.GetSceneDelegate call_GetSceneDelegate;

		// Token: 0x0400013B RID: 315
		public static ScriptingInterfaceOfIGameEntity.GetScenePointerDelegate call_GetScenePointerDelegate;

		// Token: 0x0400013C RID: 316
		public static ScriptingInterfaceOfIGameEntity.GetScriptComponentDelegate call_GetScriptComponentDelegate;

		// Token: 0x0400013D RID: 317
		public static ScriptingInterfaceOfIGameEntity.GetScriptComponentAtIndexDelegate call_GetScriptComponentAtIndexDelegate;

		// Token: 0x0400013E RID: 318
		public static ScriptingInterfaceOfIGameEntity.GetScriptComponentCountDelegate call_GetScriptComponentCountDelegate;

		// Token: 0x0400013F RID: 319
		public static ScriptingInterfaceOfIGameEntity.GetScriptComponentIndexDelegate call_GetScriptComponentIndexDelegate;

		// Token: 0x04000140 RID: 320
		public static ScriptingInterfaceOfIGameEntity.GetSkeletonDelegate call_GetSkeletonDelegate;

		// Token: 0x04000141 RID: 321
		public static ScriptingInterfaceOfIGameEntity.GetTagsDelegate call_GetTagsDelegate;

		// Token: 0x04000142 RID: 322
		public static ScriptingInterfaceOfIGameEntity.GetUpgradeLevelMaskDelegate call_GetUpgradeLevelMaskDelegate;

		// Token: 0x04000143 RID: 323
		public static ScriptingInterfaceOfIGameEntity.GetUpgradeLevelMaskCumulativeDelegate call_GetUpgradeLevelMaskCumulativeDelegate;

		// Token: 0x04000144 RID: 324
		public static ScriptingInterfaceOfIGameEntity.GetVisibilityExcludeParentsDelegate call_GetVisibilityExcludeParentsDelegate;

		// Token: 0x04000145 RID: 325
		public static ScriptingInterfaceOfIGameEntity.GetVisibilityLevelMaskIncludingParentsDelegate call_GetVisibilityLevelMaskIncludingParentsDelegate;

		// Token: 0x04000146 RID: 326
		public static ScriptingInterfaceOfIGameEntity.GetWaterLevelAtPositionDelegate call_GetWaterLevelAtPositionDelegate;

		// Token: 0x04000147 RID: 327
		public static ScriptingInterfaceOfIGameEntity.HasBatchedKinematicPhysicsFlagDelegate call_HasBatchedKinematicPhysicsFlagDelegate;

		// Token: 0x04000148 RID: 328
		public static ScriptingInterfaceOfIGameEntity.HasBatchedRayCastPhysicsFlagDelegate call_HasBatchedRayCastPhysicsFlagDelegate;

		// Token: 0x04000149 RID: 329
		public static ScriptingInterfaceOfIGameEntity.HasBodyDelegate call_HasBodyDelegate;

		// Token: 0x0400014A RID: 330
		public static ScriptingInterfaceOfIGameEntity.HasComplexAnimTreeDelegate call_HasComplexAnimTreeDelegate;

		// Token: 0x0400014B RID: 331
		public static ScriptingInterfaceOfIGameEntity.HasComponentDelegate call_HasComponentDelegate;

		// Token: 0x0400014C RID: 332
		public static ScriptingInterfaceOfIGameEntity.HasDynamicRigidBodyDelegate call_HasDynamicRigidBodyDelegate;

		// Token: 0x0400014D RID: 333
		public static ScriptingInterfaceOfIGameEntity.HasDynamicRigidBodyAndActiveSimulationDelegate call_HasDynamicRigidBodyAndActiveSimulationDelegate;

		// Token: 0x0400014E RID: 334
		public static ScriptingInterfaceOfIGameEntity.HasFrameChangedDelegate call_HasFrameChangedDelegate;

		// Token: 0x0400014F RID: 335
		public static ScriptingInterfaceOfIGameEntity.HasKinematicRigidBodyDelegate call_HasKinematicRigidBodyDelegate;

		// Token: 0x04000150 RID: 336
		public static ScriptingInterfaceOfIGameEntity.HasPhysicsBodyDelegate call_HasPhysicsBodyDelegate;

		// Token: 0x04000151 RID: 337
		public static ScriptingInterfaceOfIGameEntity.HasPhysicsDefinitionDelegate call_HasPhysicsDefinitionDelegate;

		// Token: 0x04000152 RID: 338
		public static ScriptingInterfaceOfIGameEntity.HasSceneDelegate call_HasSceneDelegate;

		// Token: 0x04000153 RID: 339
		public static ScriptingInterfaceOfIGameEntity.HasScriptComponentDelegate call_HasScriptComponentDelegate;

		// Token: 0x04000154 RID: 340
		public static ScriptingInterfaceOfIGameEntity.HasScriptComponentHashDelegate call_HasScriptComponentHashDelegate;

		// Token: 0x04000155 RID: 341
		public static ScriptingInterfaceOfIGameEntity.HasStaticPhysicsBodyDelegate call_HasStaticPhysicsBodyDelegate;

		// Token: 0x04000156 RID: 342
		public static ScriptingInterfaceOfIGameEntity.HasTagDelegate call_HasTagDelegate;

		// Token: 0x04000157 RID: 343
		public static ScriptingInterfaceOfIGameEntity.IsDynamicBodyStationaryDelegate call_IsDynamicBodyStationaryDelegate;

		// Token: 0x04000158 RID: 344
		public static ScriptingInterfaceOfIGameEntity.IsEngineBodySleepingDelegate call_IsEngineBodySleepingDelegate;

		// Token: 0x04000159 RID: 345
		public static ScriptingInterfaceOfIGameEntity.IsEntitySelectedOnEditorDelegate call_IsEntitySelectedOnEditorDelegate;

		// Token: 0x0400015A RID: 346
		public static ScriptingInterfaceOfIGameEntity.IsFrozenDelegate call_IsFrozenDelegate;

		// Token: 0x0400015B RID: 347
		public static ScriptingInterfaceOfIGameEntity.IsGhostObjectDelegate call_IsGhostObjectDelegate;

		// Token: 0x0400015C RID: 348
		public static ScriptingInterfaceOfIGameEntity.IsGravityDisabledDelegate call_IsGravityDisabledDelegate;

		// Token: 0x0400015D RID: 349
		public static ScriptingInterfaceOfIGameEntity.IsGuidValidDelegate call_IsGuidValidDelegate;

		// Token: 0x0400015E RID: 350
		public static ScriptingInterfaceOfIGameEntity.IsInEditorSceneDelegate call_IsInEditorSceneDelegate;

		// Token: 0x0400015F RID: 351
		public static ScriptingInterfaceOfIGameEntity.IsVisibleIncludeParentsDelegate call_IsVisibleIncludeParentsDelegate;

		// Token: 0x04000160 RID: 352
		public static ScriptingInterfaceOfIGameEntity.PauseParticleSystemDelegate call_PauseParticleSystemDelegate;

		// Token: 0x04000161 RID: 353
		public static ScriptingInterfaceOfIGameEntity.PopCapsuleShapeFromEntityBodyDelegate call_PopCapsuleShapeFromEntityBodyDelegate;

		// Token: 0x04000162 RID: 354
		public static ScriptingInterfaceOfIGameEntity.PrefabExistsDelegate call_PrefabExistsDelegate;

		// Token: 0x04000163 RID: 355
		public static ScriptingInterfaceOfIGameEntity.PushCapsuleShapeToEntityBodyDelegate call_PushCapsuleShapeToEntityBodyDelegate;

		// Token: 0x04000164 RID: 356
		public static ScriptingInterfaceOfIGameEntity.RayHitEntityDelegate call_RayHitEntityDelegate;

		// Token: 0x04000165 RID: 357
		public static ScriptingInterfaceOfIGameEntity.RayHitEntityWithNormalDelegate call_RayHitEntityWithNormalDelegate;

		// Token: 0x04000166 RID: 358
		public static ScriptingInterfaceOfIGameEntity.RecomputeBoundingBoxDelegate call_RecomputeBoundingBoxDelegate;

		// Token: 0x04000167 RID: 359
		public static ScriptingInterfaceOfIGameEntity.RefreshMeshesToRenderToHullWaterDelegate call_RefreshMeshesToRenderToHullWaterDelegate;

		// Token: 0x04000168 RID: 360
		public static ScriptingInterfaceOfIGameEntity.RegisterWaterSDFClipDelegate call_RegisterWaterSDFClipDelegate;

		// Token: 0x04000169 RID: 361
		public static ScriptingInterfaceOfIGameEntity.RelaxLocalBoundingBoxDelegate call_RelaxLocalBoundingBoxDelegate;

		// Token: 0x0400016A RID: 362
		public static ScriptingInterfaceOfIGameEntity.ReleaseEditDataUserToAllMeshesDelegate call_ReleaseEditDataUserToAllMeshesDelegate;

		// Token: 0x0400016B RID: 363
		public static ScriptingInterfaceOfIGameEntity.RemoveDelegate call_RemoveDelegate;

		// Token: 0x0400016C RID: 364
		public static ScriptingInterfaceOfIGameEntity.RemoveAllChildrenDelegate call_RemoveAllChildrenDelegate;

		// Token: 0x0400016D RID: 365
		public static ScriptingInterfaceOfIGameEntity.RemoveAllParticleSystemsDelegate call_RemoveAllParticleSystemsDelegate;

		// Token: 0x0400016E RID: 366
		public static ScriptingInterfaceOfIGameEntity.RemoveChildDelegate call_RemoveChildDelegate;

		// Token: 0x0400016F RID: 367
		public static ScriptingInterfaceOfIGameEntity.RemoveComponentDelegate call_RemoveComponentDelegate;

		// Token: 0x04000170 RID: 368
		public static ScriptingInterfaceOfIGameEntity.RemoveComponentWithMeshDelegate call_RemoveComponentWithMeshDelegate;

		// Token: 0x04000171 RID: 369
		public static ScriptingInterfaceOfIGameEntity.RemoveEnginePhysicsDelegate call_RemoveEnginePhysicsDelegate;

		// Token: 0x04000172 RID: 370
		public static ScriptingInterfaceOfIGameEntity.RemoveFromPredisplayEntityDelegate call_RemoveFromPredisplayEntityDelegate;

		// Token: 0x04000173 RID: 371
		public static ScriptingInterfaceOfIGameEntity.RemoveJointDelegate call_RemoveJointDelegate;

		// Token: 0x04000174 RID: 372
		public static ScriptingInterfaceOfIGameEntity.RemoveMultiMeshDelegate call_RemoveMultiMeshDelegate;

		// Token: 0x04000175 RID: 373
		public static ScriptingInterfaceOfIGameEntity.RemoveMultiMeshFromSkeletonDelegate call_RemoveMultiMeshFromSkeletonDelegate;

		// Token: 0x04000176 RID: 374
		public static ScriptingInterfaceOfIGameEntity.RemoveMultiMeshFromSkeletonBoneDelegate call_RemoveMultiMeshFromSkeletonBoneDelegate;

		// Token: 0x04000177 RID: 375
		public static ScriptingInterfaceOfIGameEntity.RemovePhysicsDelegate call_RemovePhysicsDelegate;

		// Token: 0x04000178 RID: 376
		public static ScriptingInterfaceOfIGameEntity.RemoveScriptComponentDelegate call_RemoveScriptComponentDelegate;

		// Token: 0x04000179 RID: 377
		public static ScriptingInterfaceOfIGameEntity.RemoveTagDelegate call_RemoveTagDelegate;

		// Token: 0x0400017A RID: 378
		public static ScriptingInterfaceOfIGameEntity.ReplacePhysicsBodyWithQuadPhysicsBodyDelegate call_ReplacePhysicsBodyWithQuadPhysicsBodyDelegate;

		// Token: 0x0400017B RID: 379
		public static ScriptingInterfaceOfIGameEntity.ResetHullWaterDelegate call_ResetHullWaterDelegate;

		// Token: 0x0400017C RID: 380
		public static ScriptingInterfaceOfIGameEntity.ResumeParticleSystemDelegate call_ResumeParticleSystemDelegate;

		// Token: 0x0400017D RID: 381
		public static ScriptingInterfaceOfIGameEntity.SelectEntityOnEditorDelegate call_SelectEntityOnEditorDelegate;

		// Token: 0x0400017E RID: 382
		public static ScriptingInterfaceOfIGameEntity.SetAlphaDelegate call_SetAlphaDelegate;

		// Token: 0x0400017F RID: 383
		public static ScriptingInterfaceOfIGameEntity.SetAngularVelocityDelegate call_SetAngularVelocityDelegate;

		// Token: 0x04000180 RID: 384
		public static ScriptingInterfaceOfIGameEntity.SetAnimationSoundActivationDelegate call_SetAnimationSoundActivationDelegate;

		// Token: 0x04000181 RID: 385
		public static ScriptingInterfaceOfIGameEntity.SetAnimTreeChannelParameterDelegate call_SetAnimTreeChannelParameterDelegate;

		// Token: 0x04000182 RID: 386
		public static ScriptingInterfaceOfIGameEntity.SetAsContourEntityDelegate call_SetAsContourEntityDelegate;

		// Token: 0x04000183 RID: 387
		public static ScriptingInterfaceOfIGameEntity.SetAsPredisplayEntityDelegate call_SetAsPredisplayEntityDelegate;

		// Token: 0x04000184 RID: 388
		public static ScriptingInterfaceOfIGameEntity.SetAsReplayEntityDelegate call_SetAsReplayEntityDelegate;

		// Token: 0x04000185 RID: 389
		public static ScriptingInterfaceOfIGameEntity.SetBodyFlagsDelegate call_SetBodyFlagsDelegate;

		// Token: 0x04000186 RID: 390
		public static ScriptingInterfaceOfIGameEntity.SetBodyFlagsRecursiveDelegate call_SetBodyFlagsRecursiveDelegate;

		// Token: 0x04000187 RID: 391
		public static ScriptingInterfaceOfIGameEntity.SetBodyShapeDelegate call_SetBodyShapeDelegate;

		// Token: 0x04000188 RID: 392
		public static ScriptingInterfaceOfIGameEntity.SetBoneFrameToAllMeshesDelegate call_SetBoneFrameToAllMeshesDelegate;

		// Token: 0x04000189 RID: 393
		public static ScriptingInterfaceOfIGameEntity.SetBoundingboxDirtyDelegate call_SetBoundingboxDirtyDelegate;

		// Token: 0x0400018A RID: 394
		public static ScriptingInterfaceOfIGameEntity.SetCenterOfMassDelegate call_SetCenterOfMassDelegate;

		// Token: 0x0400018B RID: 395
		public static ScriptingInterfaceOfIGameEntity.SetClothComponentKeepStateDelegate call_SetClothComponentKeepStateDelegate;

		// Token: 0x0400018C RID: 396
		public static ScriptingInterfaceOfIGameEntity.SetClothComponentKeepStateOfAllMeshesDelegate call_SetClothComponentKeepStateOfAllMeshesDelegate;

		// Token: 0x0400018D RID: 397
		public static ScriptingInterfaceOfIGameEntity.SetClothMaxDistanceMultiplierDelegate call_SetClothMaxDistanceMultiplierDelegate;

		// Token: 0x0400018E RID: 398
		public static ScriptingInterfaceOfIGameEntity.SetColorToAllMeshesWithTagRecursiveDelegate call_SetColorToAllMeshesWithTagRecursiveDelegate;

		// Token: 0x0400018F RID: 399
		public static ScriptingInterfaceOfIGameEntity.SetContourStateDelegate call_SetContourStateDelegate;

		// Token: 0x04000190 RID: 400
		public static ScriptingInterfaceOfIGameEntity.SetCostAdderForAttachedFacesDelegate call_SetCostAdderForAttachedFacesDelegate;

		// Token: 0x04000191 RID: 401
		public static ScriptingInterfaceOfIGameEntity.SetCullModeDelegate call_SetCullModeDelegate;

		// Token: 0x04000192 RID: 402
		public static ScriptingInterfaceOfIGameEntity.SetCustomClipPlaneDelegate call_SetCustomClipPlaneDelegate;

		// Token: 0x04000193 RID: 403
		public static ScriptingInterfaceOfIGameEntity.SetCustomVertexPositionEnabledDelegate call_SetCustomVertexPositionEnabledDelegate;

		// Token: 0x04000194 RID: 404
		public static ScriptingInterfaceOfIGameEntity.SetDampingDelegate call_SetDampingDelegate;

		// Token: 0x04000195 RID: 405
		public static ScriptingInterfaceOfIGameEntity.SetDoNotCheckVisibilityDelegate call_SetDoNotCheckVisibilityDelegate;

		// Token: 0x04000196 RID: 406
		public static ScriptingInterfaceOfIGameEntity.SetEnforcedMaximumLodLevelDelegate call_SetEnforcedMaximumLodLevelDelegate;

		// Token: 0x04000197 RID: 407
		public static ScriptingInterfaceOfIGameEntity.SetEntityEnvMapVisibilityDelegate call_SetEntityEnvMapVisibilityDelegate;

		// Token: 0x04000198 RID: 408
		public static ScriptingInterfaceOfIGameEntity.SetEntityFlagsDelegate call_SetEntityFlagsDelegate;

		// Token: 0x04000199 RID: 409
		public static ScriptingInterfaceOfIGameEntity.SetEntityVisibilityFlagsDelegate call_SetEntityVisibilityFlagsDelegate;

		// Token: 0x0400019A RID: 410
		public static ScriptingInterfaceOfIGameEntity.SetExternalReferencesUsageDelegate call_SetExternalReferencesUsageDelegate;

		// Token: 0x0400019B RID: 411
		public static ScriptingInterfaceOfIGameEntity.SetFactor2ColorDelegate call_SetFactor2ColorDelegate;

		// Token: 0x0400019C RID: 412
		public static ScriptingInterfaceOfIGameEntity.SetFactorColorDelegate call_SetFactorColorDelegate;

		// Token: 0x0400019D RID: 413
		public static ScriptingInterfaceOfIGameEntity.SetForceDecalsToRenderDelegate call_SetForceDecalsToRenderDelegate;

		// Token: 0x0400019E RID: 414
		public static ScriptingInterfaceOfIGameEntity.SetForceNotAffectedBySeasonDelegate call_SetForceNotAffectedBySeasonDelegate;

		// Token: 0x0400019F RID: 415
		public static ScriptingInterfaceOfIGameEntity.SetFrameChangedDelegate call_SetFrameChangedDelegate;

		// Token: 0x040001A0 RID: 416
		public static ScriptingInterfaceOfIGameEntity.SetGlobalFrameDelegate call_SetGlobalFrameDelegate;

		// Token: 0x040001A1 RID: 417
		public static ScriptingInterfaceOfIGameEntity.SetGlobalPositionDelegate call_SetGlobalPositionDelegate;

		// Token: 0x040001A2 RID: 418
		public static ScriptingInterfaceOfIGameEntity.SetHasCustomBoundingBoxValidationSystemDelegate call_SetHasCustomBoundingBoxValidationSystemDelegate;

		// Token: 0x040001A3 RID: 419
		public static ScriptingInterfaceOfIGameEntity.SetLinearVelocityDelegate call_SetLinearVelocityDelegate;

		// Token: 0x040001A4 RID: 420
		public static ScriptingInterfaceOfIGameEntity.SetLocalFrameDelegate call_SetLocalFrameDelegate;

		// Token: 0x040001A5 RID: 421
		public static ScriptingInterfaceOfIGameEntity.SetLocalPositionDelegate call_SetLocalPositionDelegate;

		// Token: 0x040001A6 RID: 422
		public static ScriptingInterfaceOfIGameEntity.SetManualGlobalBoundingBoxDelegate call_SetManualGlobalBoundingBoxDelegate;

		// Token: 0x040001A7 RID: 423
		public static ScriptingInterfaceOfIGameEntity.SetManualLocalBoundingBoxDelegate call_SetManualLocalBoundingBoxDelegate;

		// Token: 0x040001A8 RID: 424
		public static ScriptingInterfaceOfIGameEntity.SetMassAndUpdateInertiaAndCenterOfMassDelegate call_SetMassAndUpdateInertiaAndCenterOfMassDelegate;

		// Token: 0x040001A9 RID: 425
		public static ScriptingInterfaceOfIGameEntity.SetMassSpaceInertiaDelegate call_SetMassSpaceInertiaDelegate;

		// Token: 0x040001AA RID: 426
		public static ScriptingInterfaceOfIGameEntity.SetMaterialForAllMeshesDelegate call_SetMaterialForAllMeshesDelegate;

		// Token: 0x040001AB RID: 427
		public static ScriptingInterfaceOfIGameEntity.SetMaxDepenetrationVelocityDelegate call_SetMaxDepenetrationVelocityDelegate;

		// Token: 0x040001AC RID: 428
		public static ScriptingInterfaceOfIGameEntity.SetMobilityDelegate call_SetMobilityDelegate;

		// Token: 0x040001AD RID: 429
		public static ScriptingInterfaceOfIGameEntity.SetMorphFrameOfComponentsDelegate call_SetMorphFrameOfComponentsDelegate;

		// Token: 0x040001AE RID: 430
		public static ScriptingInterfaceOfIGameEntity.SetNameDelegate call_SetNameDelegate;

		// Token: 0x040001AF RID: 431
		public static ScriptingInterfaceOfIGameEntity.SetNativeScriptComponentVariableDelegate call_SetNativeScriptComponentVariableDelegate;

		// Token: 0x040001B0 RID: 432
		public static ScriptingInterfaceOfIGameEntity.SetPhysicsMoveToBatchedDelegate call_SetPhysicsMoveToBatchedDelegate;

		// Token: 0x040001B1 RID: 433
		public static ScriptingInterfaceOfIGameEntity.SetPhysicsStateDelegate call_SetPhysicsStateDelegate;

		// Token: 0x040001B2 RID: 434
		public static ScriptingInterfaceOfIGameEntity.SetPhysicsStateOnlyVariableDelegate call_SetPhysicsStateOnlyVariableDelegate;

		// Token: 0x040001B3 RID: 435
		public static ScriptingInterfaceOfIGameEntity.SetPositionsForAttachedNavmeshVerticesDelegate call_SetPositionsForAttachedNavmeshVerticesDelegate;

		// Token: 0x040001B4 RID: 436
		public static ScriptingInterfaceOfIGameEntity.SetPreviousFrameInvalidDelegate call_SetPreviousFrameInvalidDelegate;

		// Token: 0x040001B5 RID: 437
		public static ScriptingInterfaceOfIGameEntity.SetReadyToRenderDelegate call_SetReadyToRenderDelegate;

		// Token: 0x040001B6 RID: 438
		public static ScriptingInterfaceOfIGameEntity.SetRuntimeEmissionRateMultiplierDelegate call_SetRuntimeEmissionRateMultiplierDelegate;

		// Token: 0x040001B7 RID: 439
		public static ScriptingInterfaceOfIGameEntity.SetSkeletonDelegate call_SetSkeletonDelegate;

		// Token: 0x040001B8 RID: 440
		public static ScriptingInterfaceOfIGameEntity.SetSolverIterationCountsDelegate call_SetSolverIterationCountsDelegate;

		// Token: 0x040001B9 RID: 441
		public static ScriptingInterfaceOfIGameEntity.SetupAdditionalBoneBufferForMeshesDelegate call_SetupAdditionalBoneBufferForMeshesDelegate;

		// Token: 0x040001BA RID: 442
		public static ScriptingInterfaceOfIGameEntity.SetUpdateValidityOnFrameChangedOfFacesWithIdDelegate call_SetUpdateValidityOnFrameChangedOfFacesWithIdDelegate;

		// Token: 0x040001BB RID: 443
		public static ScriptingInterfaceOfIGameEntity.SetUpgradeLevelMaskDelegate call_SetUpgradeLevelMaskDelegate;

		// Token: 0x040001BC RID: 444
		public static ScriptingInterfaceOfIGameEntity.SetVectorArgumentDelegate call_SetVectorArgumentDelegate;

		// Token: 0x040001BD RID: 445
		public static ScriptingInterfaceOfIGameEntity.SetVelocityLimitsDelegate call_SetVelocityLimitsDelegate;

		// Token: 0x040001BE RID: 446
		public static ScriptingInterfaceOfIGameEntity.SetVisibilityExcludeParentsDelegate call_SetVisibilityExcludeParentsDelegate;

		// Token: 0x040001BF RID: 447
		public static ScriptingInterfaceOfIGameEntity.SetVisualRecordWakeParamsDelegate call_SetVisualRecordWakeParamsDelegate;

		// Token: 0x040001C0 RID: 448
		public static ScriptingInterfaceOfIGameEntity.SetWaterSDFClipDataDelegate call_SetWaterSDFClipDataDelegate;

		// Token: 0x040001C1 RID: 449
		public static ScriptingInterfaceOfIGameEntity.SetWaterVisualRecordFrameAndDtDelegate call_SetWaterVisualRecordFrameAndDtDelegate;

		// Token: 0x040001C2 RID: 450
		public static ScriptingInterfaceOfIGameEntity.SwapPhysxShapeInEntityDelegate call_SwapPhysxShapeInEntityDelegate;

		// Token: 0x040001C3 RID: 451
		public static ScriptingInterfaceOfIGameEntity.UpdateAttachedNavigationMeshFacesDelegate call_UpdateAttachedNavigationMeshFacesDelegate;

		// Token: 0x040001C4 RID: 452
		public static ScriptingInterfaceOfIGameEntity.UpdateBodyRestOffsetDelegate call_UpdateBodyRestOffsetDelegate;

		// Token: 0x040001C5 RID: 453
		public static ScriptingInterfaceOfIGameEntity.UpdateGlobalBoundsDelegate call_UpdateGlobalBoundsDelegate;

		// Token: 0x040001C6 RID: 454
		public static ScriptingInterfaceOfIGameEntity.UpdateHullWaterEffectFramesDelegate call_UpdateHullWaterEffectFramesDelegate;

		// Token: 0x040001C7 RID: 455
		public static ScriptingInterfaceOfIGameEntity.UpdateTriadFrameForEditorDelegate call_UpdateTriadFrameForEditorDelegate;

		// Token: 0x040001C8 RID: 456
		public static ScriptingInterfaceOfIGameEntity.UpdateVisibilityMaskDelegate call_UpdateVisibilityMaskDelegate;

		// Token: 0x040001C9 RID: 457
		public static ScriptingInterfaceOfIGameEntity.ValidateBoundingBoxDelegate call_ValidateBoundingBoxDelegate;

		// Token: 0x02000133 RID: 307
		// (Invoke) Token: 0x06000AE5 RID: 2789
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ActivateRagdollDelegate(UIntPtr entityId);

		// Token: 0x02000134 RID: 308
		// (Invoke) Token: 0x06000AE9 RID: 2793
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddAllMeshesOfGameEntityDelegate(UIntPtr entityId, UIntPtr copiedEntityId);

		// Token: 0x02000135 RID: 309
		// (Invoke) Token: 0x06000AED RID: 2797
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddCapsuleAsBodyDelegate(UIntPtr entityId, Vec3 p1, Vec3 p2, float radius, uint bodyFlags, byte[] physicsMaterialName);

		// Token: 0x02000136 RID: 310
		// (Invoke) Token: 0x06000AF1 RID: 2801
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddChildDelegate(UIntPtr parententity, UIntPtr childentity, [MarshalAs(UnmanagedType.U1)] bool autoLocalizeFrame);

		// Token: 0x02000137 RID: 311
		// (Invoke) Token: 0x06000AF5 RID: 2805
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddComponentDelegate(UIntPtr pointer, UIntPtr componentPointer);

		// Token: 0x02000138 RID: 312
		// (Invoke) Token: 0x06000AF9 RID: 2809
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr AddDistanceJointDelegate(UIntPtr entityId, UIntPtr otherEntityId, float minDistance, float maxDistance);

		// Token: 0x02000139 RID: 313
		// (Invoke) Token: 0x06000AFD RID: 2813
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr AddDistanceJointWithFramesDelegate(UIntPtr entityId, UIntPtr otherEntityId, MatrixFrame globalFrameOnA, MatrixFrame globalFrameOnB, float minDistance, float maxDistance);

		// Token: 0x0200013A RID: 314
		// (Invoke) Token: 0x06000B01 RID: 2817
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddEditDataUserToAllMeshesDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool entity_components, [MarshalAs(UnmanagedType.U1)] bool skeleton_components);

		// Token: 0x0200013B RID: 315
		// (Invoke) Token: 0x06000B05 RID: 2821
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool AddLightDelegate(UIntPtr entityId, UIntPtr lightPointer);

		// Token: 0x0200013C RID: 316
		// (Invoke) Token: 0x06000B09 RID: 2825
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshDelegate(UIntPtr entityId, UIntPtr mesh, [MarshalAs(UnmanagedType.U1)] bool recomputeBoundingBox);

		// Token: 0x0200013D RID: 317
		// (Invoke) Token: 0x06000B0D RID: 2829
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshToBoneDelegate(UIntPtr entityId, UIntPtr multiMeshPointer, sbyte boneIndex);

		// Token: 0x0200013E RID: 318
		// (Invoke) Token: 0x06000B11 RID: 2833
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMultiMeshDelegate(UIntPtr entityId, UIntPtr multiMeshPtr, [MarshalAs(UnmanagedType.U1)] bool updateVisMask);

		// Token: 0x0200013F RID: 319
		// (Invoke) Token: 0x06000B15 RID: 2837
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMultiMeshToSkeletonDelegate(UIntPtr gameEntity, UIntPtr multiMesh);

		// Token: 0x02000140 RID: 320
		// (Invoke) Token: 0x06000B19 RID: 2841
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMultiMeshToSkeletonBoneDelegate(UIntPtr gameEntity, UIntPtr multiMesh, sbyte boneIndex);

		// Token: 0x02000141 RID: 321
		// (Invoke) Token: 0x06000B1D RID: 2845
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddParticleSystemComponentDelegate(UIntPtr entityId, byte[] particleid);

		// Token: 0x02000142 RID: 322
		// (Invoke) Token: 0x06000B21 RID: 2849
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddPhysicsDelegate(UIntPtr entityId, UIntPtr body, float mass, ref Vec3 localCenterOfMass, ref Vec3 initialGlobalVelocity, ref Vec3 initialAngularGlobalVelocity, int physicsMaterial, [MarshalAs(UnmanagedType.U1)] bool isStatic, int collisionGroupID);

		// Token: 0x02000143 RID: 323
		// (Invoke) Token: 0x06000B25 RID: 2853
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddSphereAsBodyDelegate(UIntPtr entityId, Vec3 center, float radius, uint bodyFlags);

		// Token: 0x02000144 RID: 324
		// (Invoke) Token: 0x06000B29 RID: 2857
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddSplashPositionToWaterVisualRecordDelegate(UIntPtr entityPointer, UIntPtr visualPrefab, in Vec3 position);

		// Token: 0x02000145 RID: 325
		// (Invoke) Token: 0x06000B2D RID: 2861
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddTagDelegate(UIntPtr entityId, byte[] tag);

		// Token: 0x02000146 RID: 326
		// (Invoke) Token: 0x06000B31 RID: 2865
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyAccelerationToDynamicBodyDelegate(UIntPtr entityId, ref Vec3 acceleration);

		// Token: 0x02000147 RID: 327
		// (Invoke) Token: 0x06000B35 RID: 2869
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyForceToDynamicBodyDelegate(UIntPtr entityId, ref Vec3 force, GameEntityPhysicsExtensions.ForceMode forceMode);

		// Token: 0x02000148 RID: 328
		// (Invoke) Token: 0x06000B39 RID: 2873
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyGlobalForceAtLocalPosToDynamicBodyDelegate(UIntPtr entityId, ref Vec3 localPosition, ref Vec3 globalForce, GameEntityPhysicsExtensions.ForceMode forceMode);

		// Token: 0x02000149 RID: 329
		// (Invoke) Token: 0x06000B3D RID: 2877
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyLocalForceAtLocalPosToDynamicBodyDelegate(UIntPtr entityId, ref Vec3 localPosition, ref Vec3 localForce, GameEntityPhysicsExtensions.ForceMode forceMode);

		// Token: 0x0200014A RID: 330
		// (Invoke) Token: 0x06000B41 RID: 2881
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyLocalImpulseToDynamicBodyDelegate(UIntPtr entityId, ref Vec3 localPosition, ref Vec3 impulse);

		// Token: 0x0200014B RID: 331
		// (Invoke) Token: 0x06000B45 RID: 2885
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyTorqueToDynamicBodyDelegate(UIntPtr entityId, ref Vec3 torque, GameEntityPhysicsExtensions.ForceMode forceMode);

		// Token: 0x0200014C RID: 332
		// (Invoke) Token: 0x06000B49 RID: 2889
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AttachNavigationMeshFacesDelegate(UIntPtr entityId, int faceGroupId, [MarshalAs(UnmanagedType.U1)] bool isConnected, [MarshalAs(UnmanagedType.U1)] bool isBlocker, [MarshalAs(UnmanagedType.U1)] bool autoLocalize, [MarshalAs(UnmanagedType.U1)] bool finalizeBlockerConvexHullComputation, [MarshalAs(UnmanagedType.U1)] bool updateEntityFrame);

		// Token: 0x0200014D RID: 333
		// (Invoke) Token: 0x06000B4D RID: 2893
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void BreakPrefabDelegate(UIntPtr entityId);

		// Token: 0x0200014E RID: 334
		// (Invoke) Token: 0x06000B51 RID: 2897
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void BurstEntityParticleDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool doChildren);

		// Token: 0x0200014F RID: 335
		// (Invoke) Token: 0x06000B55 RID: 2901
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CallScriptCallbacksDelegate(UIntPtr entityPointer, [MarshalAs(UnmanagedType.U1)] bool registerScriptComponents);

		// Token: 0x02000150 RID: 336
		// (Invoke) Token: 0x06000B59 RID: 2905
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ChangeMetaMeshOrRemoveItIfNotExistsDelegate(UIntPtr entityId, UIntPtr entityMetaMeshPointer, UIntPtr newMetaMeshPointer);

		// Token: 0x02000151 RID: 337
		// (Invoke) Token: 0x06000B5D RID: 2909
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ChangeResolutionMultiplierOfWaterVisualDelegate(UIntPtr visualPrefab, float multiplier, in Vec3 waterEffectsBB);

		// Token: 0x02000152 RID: 338
		// (Invoke) Token: 0x06000B61 RID: 2913
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CheckIsPrefabLinkRootPrefabDelegate(UIntPtr entityPtr, int depth);

		// Token: 0x02000153 RID: 339
		// (Invoke) Token: 0x06000B65 RID: 2917
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CheckPointWithOrientedBoundingBoxDelegate(UIntPtr entityId, Vec3 point);

		// Token: 0x02000154 RID: 340
		// (Invoke) Token: 0x06000B69 RID: 2921
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CheckResourcesDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool addToQueue, [MarshalAs(UnmanagedType.U1)] bool checkFaceResources);

		// Token: 0x02000155 RID: 341
		// (Invoke) Token: 0x06000B6D RID: 2925
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearComponentsDelegate(UIntPtr entityId);

		// Token: 0x02000156 RID: 342
		// (Invoke) Token: 0x06000B71 RID: 2929
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearEntityComponentsDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool resetAll, [MarshalAs(UnmanagedType.U1)] bool removeScripts, [MarshalAs(UnmanagedType.U1)] bool deleteChildEntities);

		// Token: 0x02000157 RID: 343
		// (Invoke) Token: 0x06000B75 RID: 2933
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearOnlyOwnComponentsDelegate(UIntPtr entityId);

		// Token: 0x02000158 RID: 344
		// (Invoke) Token: 0x06000B79 RID: 2937
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ComputeTrajectoryVolumeDelegate(UIntPtr gameEntity, float missileSpeed, float verticalAngleMaxInDegrees, float verticalAngleMinInDegrees, float horizontalAngleRangeInDegrees, float airFrictionConstant);

		// Token: 0x02000159 RID: 345
		// (Invoke) Token: 0x06000B7D RID: 2941
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ComputeVelocityDeltaFromImpulseDelegate(UIntPtr entityPtr, in Vec3 impulsiveForce, in Vec3 impulsiveTorque, out Vec3 deltaLinearVelocity, out Vec3 deltaAngularVelocity);

		// Token: 0x0200015A RID: 346
		// (Invoke) Token: 0x06000B81 RID: 2945
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ConvertDynamicBodyToRayCastDelegate(UIntPtr entityId);

		// Token: 0x0200015B RID: 347
		// (Invoke) Token: 0x06000B85 RID: 2949
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CookTrianglePhysxMeshDelegate(UIntPtr cookingInstancePointer, UIntPtr shapePointer, UIntPtr quadPinnedPointer, int physicsMaterial, int numberOfVertices, UIntPtr indicesPinnedPointer, int numberOfIndices);

		// Token: 0x0200015C RID: 348
		// (Invoke) Token: 0x06000B89 RID: 2953
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CopyComponentsToSkeletonDelegate(UIntPtr entityId);

		// Token: 0x0200015D RID: 349
		// (Invoke) Token: 0x06000B8D RID: 2957
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CopyFromPrefabDelegate(UIntPtr prefab);

		// Token: 0x0200015E RID: 350
		// (Invoke) Token: 0x06000B91 RID: 2961
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CopyScriptComponentFromAnotherEntityDelegate(UIntPtr prefab, UIntPtr other_prefab, byte[] script_name);

		// Token: 0x0200015F RID: 351
		// (Invoke) Token: 0x06000B95 RID: 2965
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CreateAndAddScriptComponentDelegate(UIntPtr entityId, byte[] name, [MarshalAs(UnmanagedType.U1)] bool callScriptCallbacks);

		// Token: 0x02000160 RID: 352
		// (Invoke) Token: 0x06000B99 RID: 2969
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateEmptyDelegate(UIntPtr scenePointer, [MarshalAs(UnmanagedType.U1)] bool isModifiableFromEditor, UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool createPhysics, [MarshalAs(UnmanagedType.U1)] bool callScriptCallbacks);

		// Token: 0x02000161 RID: 353
		// (Invoke) Token: 0x06000B9D RID: 2973
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr CreateEmptyPhysxShapeDelegate(UIntPtr entityPointer, [MarshalAs(UnmanagedType.U1)] bool isVariable, int physxMaterialIndex);

		// Token: 0x02000162 RID: 354
		// (Invoke) Token: 0x06000BA1 RID: 2977
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateEmptyWithoutSceneDelegate();

		// Token: 0x02000163 RID: 355
		// (Invoke) Token: 0x06000BA5 RID: 2981
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateFromPrefabDelegate(UIntPtr scenePointer, byte[] prefabid, [MarshalAs(UnmanagedType.U1)] bool callScriptCallbacks, [MarshalAs(UnmanagedType.U1)] bool createPhysics, uint scriptInclusionHashTag);

		// Token: 0x02000164 RID: 356
		// (Invoke) Token: 0x06000BA9 RID: 2985
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateFromPrefabWithInitialFrameAndRestOffsetDelegate(UIntPtr scenePointer, byte[] prefabId, [MarshalAs(UnmanagedType.U1)] bool createPhysics, ref MatrixFrame frame, [MarshalAs(UnmanagedType.U1)] bool hasCustomRestOffset, float restOffset, [MarshalAs(UnmanagedType.U1)] bool callScriptCallbacks, uint scriptInclusionHashTag);

		// Token: 0x02000165 RID: 357
		// (Invoke) Token: 0x06000BAD RID: 2989
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr CreatePhysxCookingInstanceDelegate();

		// Token: 0x02000166 RID: 358
		// (Invoke) Token: 0x06000BB1 RID: 2993
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CreateVariableRatePhysicsDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool forChildren);

		// Token: 0x02000167 RID: 359
		// (Invoke) Token: 0x06000BB5 RID: 2997
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeleteEmptyShapeDelegate(UIntPtr entity, UIntPtr shape1, UIntPtr shape2);

		// Token: 0x02000168 RID: 360
		// (Invoke) Token: 0x06000BB9 RID: 3001
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeletePhysxCookingInstanceDelegate(UIntPtr pointer);

		// Token: 0x02000169 RID: 361
		// (Invoke) Token: 0x06000BBD RID: 3005
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeRegisterWaterMeshMaterialsDelegate(UIntPtr entityPointer, UIntPtr visualPrefab);

		// Token: 0x0200016A RID: 362
		// (Invoke) Token: 0x06000BC1 RID: 3009
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeRegisterWaterSDFClipDelegate(UIntPtr entityId, int slot);

		// Token: 0x0200016B RID: 363
		// (Invoke) Token: 0x06000BC5 RID: 3013
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeselectEntityOnEditorDelegate(UIntPtr entityId);

		// Token: 0x0200016C RID: 364
		// (Invoke) Token: 0x06000BC9 RID: 3017
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DetachAllAttachedNavigationMeshFacesDelegate(UIntPtr entityId);

		// Token: 0x0200016D RID: 365
		// (Invoke) Token: 0x06000BCD RID: 3021
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DisableContourDelegate(UIntPtr entityId);

		// Token: 0x0200016E RID: 366
		// (Invoke) Token: 0x06000BD1 RID: 3025
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DisableDynamicBodySimulationDelegate(UIntPtr entityId);

		// Token: 0x0200016F RID: 367
		// (Invoke) Token: 0x06000BD5 RID: 3029
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DisableGravityDelegate(UIntPtr entityId);

		// Token: 0x02000170 RID: 368
		// (Invoke) Token: 0x06000BD9 RID: 3033
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnableDynamicBodyDelegate(UIntPtr entityId);

		// Token: 0x02000171 RID: 369
		// (Invoke) Token: 0x06000BDD RID: 3037
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer FindWithNameDelegate(UIntPtr scenePointer, byte[] name);

		// Token: 0x02000172 RID: 370
		// (Invoke) Token: 0x06000BE1 RID: 3041
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FreezeDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool isFrozen);

		// Token: 0x02000173 RID: 371
		// (Invoke) Token: 0x06000BE5 RID: 3045
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetAngularVelocityDelegate(UIntPtr entityPtr);

		// Token: 0x02000174 RID: 372
		// (Invoke) Token: 0x06000BE9 RID: 3049
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAttachedNavmeshFaceCountDelegate(UIntPtr entityId);

		// Token: 0x02000175 RID: 373
		// (Invoke) Token: 0x06000BED RID: 3053
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetAttachedNavmeshFaceRecordsDelegate(UIntPtr entityId, IntPtr faceRecords);

		// Token: 0x02000176 RID: 374
		// (Invoke) Token: 0x06000BF1 RID: 3057
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetAttachedNavmeshFaceVertexIndicesDelegate(UIntPtr entityId, in PathFaceRecord faceRecord, IntPtr indices);

		// Token: 0x02000177 RID: 375
		// (Invoke) Token: 0x06000BF5 RID: 3061
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetBodyFlagsDelegate(UIntPtr entityId);

		// Token: 0x02000178 RID: 376
		// (Invoke) Token: 0x06000BF9 RID: 3065
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetBodyShapeDelegate(UIntPtr entityId);

		// Token: 0x02000179 RID: 377
		// (Invoke) Token: 0x06000BFD RID: 3069
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBodyVisualWorldTransformDelegate(UIntPtr entityPtr, out MatrixFrame frame);

		// Token: 0x0200017A RID: 378
		// (Invoke) Token: 0x06000C01 RID: 3073
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBodyWorldTransformDelegate(UIntPtr entityPtr, out MatrixFrame frame);

		// Token: 0x0200017B RID: 379
		// (Invoke) Token: 0x06000C05 RID: 3077
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetBoneCountDelegate(UIntPtr entityId);

		// Token: 0x0200017C RID: 380
		// (Invoke) Token: 0x06000C09 RID: 3081
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameWithIndexDelegate(UIntPtr entityId, sbyte boneIndex, ref MatrixFrame outEntitialFrame);

		// Token: 0x0200017D RID: 381
		// (Invoke) Token: 0x06000C0D RID: 3085
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameWithNameDelegate(UIntPtr entityId, byte[] boneName, ref MatrixFrame outEntitialFrame);

		// Token: 0x0200017E RID: 382
		// (Invoke) Token: 0x06000C11 RID: 3089
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetBoundingBoxMaxDelegate(UIntPtr entityId);

		// Token: 0x0200017F RID: 383
		// (Invoke) Token: 0x06000C15 RID: 3093
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetBoundingBoxMinDelegate(UIntPtr entityId);

		// Token: 0x02000180 RID: 384
		// (Invoke) Token: 0x06000C19 RID: 3097
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetCameraParamsFromCameraScriptDelegate(UIntPtr entityId, UIntPtr camPtr, ref Vec3 dof_params);

		// Token: 0x02000181 RID: 385
		// (Invoke) Token: 0x06000C1D RID: 3101
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetCenterOfMassDelegate(UIntPtr entityId);

		// Token: 0x02000182 RID: 386
		// (Invoke) Token: 0x06000C21 RID: 3105
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetChildDelegate(UIntPtr entityId, int childIndex);

		// Token: 0x02000183 RID: 387
		// (Invoke) Token: 0x06000C25 RID: 3109
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetChildCountDelegate(UIntPtr entityId);

		// Token: 0x02000184 RID: 388
		// (Invoke) Token: 0x06000C29 RID: 3113
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetChildPointerDelegate(UIntPtr entityId, int childIndex);

		// Token: 0x02000185 RID: 389
		// (Invoke) Token: 0x06000C2D RID: 3117
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetComponentAtIndexDelegate(UIntPtr entityId, GameEntity.ComponentType componentType, int index);

		// Token: 0x02000186 RID: 390
		// (Invoke) Token: 0x06000C31 RID: 3121
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetComponentCountDelegate(UIntPtr entityId, GameEntity.ComponentType componentType);

		// Token: 0x02000187 RID: 391
		// (Invoke) Token: 0x06000C35 RID: 3125
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetEditModeLevelVisibilityDelegate(UIntPtr entityId);

		// Token: 0x02000188 RID: 392
		// (Invoke) Token: 0x06000C39 RID: 3129
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate EntityFlags GetEntityFlagsDelegate(UIntPtr entityId);

		// Token: 0x02000189 RID: 393
		// (Invoke) Token: 0x06000C3D RID: 3133
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate EntityVisibilityFlags GetEntityVisibilityFlagsDelegate(UIntPtr entityId);

		// Token: 0x0200018A RID: 394
		// (Invoke) Token: 0x06000C41 RID: 3137
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetFactorColorDelegate(UIntPtr entityId);

		// Token: 0x0200018B RID: 395
		// (Invoke) Token: 0x06000C45 RID: 3141
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetFirstChildWithTagRecursiveDelegate(UIntPtr entityPtr, byte[] tag);

		// Token: 0x0200018C RID: 396
		// (Invoke) Token: 0x06000C49 RID: 3145
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetFirstEntityWithTagDelegate(UIntPtr scenePointer, byte[] tag);

		// Token: 0x0200018D RID: 397
		// (Invoke) Token: 0x06000C4D RID: 3149
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetFirstEntityWithTagExpressionDelegate(UIntPtr scenePointer, byte[] tagExpression);

		// Token: 0x0200018E RID: 398
		// (Invoke) Token: 0x06000C51 RID: 3153
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFirstMeshDelegate(UIntPtr entityId);

		// Token: 0x0200018F RID: 399
		// (Invoke) Token: 0x06000C55 RID: 3157
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate BoundingBox GetGlobalBoundingBoxDelegate(UIntPtr entityId);

		// Token: 0x02000190 RID: 400
		// (Invoke) Token: 0x06000C59 RID: 3161
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetGlobalBoxMaxDelegate(UIntPtr entityId);

		// Token: 0x02000191 RID: 401
		// (Invoke) Token: 0x06000C5D RID: 3165
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetGlobalBoxMinDelegate(UIntPtr entityId);

		// Token: 0x02000192 RID: 402
		// (Invoke) Token: 0x06000C61 RID: 3169
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetGlobalFrameDelegate(UIntPtr meshPointer, out MatrixFrame outFrame);

		// Token: 0x02000193 RID: 403
		// (Invoke) Token: 0x06000C65 RID: 3173
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetGlobalFrameImpreciseForFixedTickDelegate(UIntPtr entityId, out MatrixFrame outFrame);

		// Token: 0x02000194 RID: 404
		// (Invoke) Token: 0x06000C69 RID: 3177
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetGlobalScaleDelegate(UIntPtr pointer);

		// Token: 0x02000195 RID: 405
		// (Invoke) Token: 0x06000C6D RID: 3181
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec2 GetGlobalWindStrengthVectorOfSceneDelegate(UIntPtr entityPtr);

		// Token: 0x02000196 RID: 406
		// (Invoke) Token: 0x06000C71 RID: 3185
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec2 GetGlobalWindVelocityOfSceneDelegate(UIntPtr entityPtr);

		// Token: 0x02000197 RID: 407
		// (Invoke) Token: 0x06000C75 RID: 3189
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec2 GetGlobalWindVelocityWithGustNoiseOfSceneDelegate(UIntPtr entityPtr, float globalTime);

		// Token: 0x02000198 RID: 408
		// (Invoke) Token: 0x06000C79 RID: 3193
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetGuidDelegate(UIntPtr entityId);

		// Token: 0x02000199 RID: 409
		// (Invoke) Token: 0x06000C7D RID: 3197
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetLastFinalRenderCameraPositionOfSceneDelegate(UIntPtr entityPtr);

		// Token: 0x0200019A RID: 410
		// (Invoke) Token: 0x06000C81 RID: 3201
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetLightDelegate(UIntPtr entityId);

		// Token: 0x0200019B RID: 411
		// (Invoke) Token: 0x06000C85 RID: 3205
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetLinearVelocityDelegate(UIntPtr entityPtr);

		// Token: 0x0200019C RID: 412
		// (Invoke) Token: 0x06000C89 RID: 3209
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate BoundingBox GetLocalBoundingBoxDelegate(UIntPtr entityId);

		// Token: 0x0200019D RID: 413
		// (Invoke) Token: 0x06000C8D RID: 3213
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetLocalFrameDelegate(UIntPtr entityId, out MatrixFrame outFrame);

		// Token: 0x0200019E RID: 414
		// (Invoke) Token: 0x06000C91 RID: 3217
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetLocalPhysicsBoundingBoxDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool includeChildren, out BoundingBox outBoundingBox);

		// Token: 0x0200019F RID: 415
		// (Invoke) Token: 0x06000C95 RID: 3221
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetLodLevelForDistanceSqDelegate(UIntPtr entityId, float distanceSquared);

		// Token: 0x020001A0 RID: 416
		// (Invoke) Token: 0x06000C99 RID: 3225
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetMassDelegate(UIntPtr entityId);

		// Token: 0x020001A1 RID: 417
		// (Invoke) Token: 0x06000C9D RID: 3229
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetMassSpaceInertiaDelegate(UIntPtr entityId);

		// Token: 0x020001A2 RID: 418
		// (Invoke) Token: 0x06000CA1 RID: 3233
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetMassSpaceInverseInertiaDelegate(UIntPtr entityId);

		// Token: 0x020001A3 RID: 419
		// (Invoke) Token: 0x06000CA5 RID: 3237
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetMeshBendedPositionDelegate(UIntPtr entityId, ref MatrixFrame worldSpacePosition, ref MatrixFrame output);

		// Token: 0x020001A4 RID: 420
		// (Invoke) Token: 0x06000CA9 RID: 3241
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate GameEntity.Mobility GetMobilityDelegate(UIntPtr entityId);

		// Token: 0x020001A5 RID: 421
		// (Invoke) Token: 0x06000CAD RID: 3245
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr entityId);

		// Token: 0x020001A6 RID: 422
		// (Invoke) Token: 0x06000CB1 RID: 3249
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetNativeScriptComponentVariableDelegate(UIntPtr entityPtr, byte[] className, byte[] fieldName, ref ScriptComponentFieldHolder data, RglScriptFieldType variableType);

		// Token: 0x020001A7 RID: 423
		// (Invoke) Token: 0x06000CB5 RID: 3253
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetNextEntityWithTagDelegate(UIntPtr currententityId, byte[] tag);

		// Token: 0x020001A8 RID: 424
		// (Invoke) Token: 0x06000CB9 RID: 3257
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetNextEntityWithTagExpressionDelegate(UIntPtr currententityId, byte[] tagExpression);

		// Token: 0x020001A9 RID: 425
		// (Invoke) Token: 0x06000CBD RID: 3261
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetNextPrefabDelegate(UIntPtr currentPrefab);

		// Token: 0x020001AA RID: 426
		// (Invoke) Token: 0x06000CC1 RID: 3265
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetOldPrefabNameDelegate(UIntPtr prefab);

		// Token: 0x020001AB RID: 427
		// (Invoke) Token: 0x06000CC5 RID: 3269
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetParentDelegate(UIntPtr entityId);

		// Token: 0x020001AC RID: 428
		// (Invoke) Token: 0x06000CC9 RID: 3273
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetParentPointerDelegate(UIntPtr entityId);

		// Token: 0x020001AD RID: 429
		// (Invoke) Token: 0x06000CCD RID: 3277
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetPhysicsDescBodyFlagsDelegate(UIntPtr entityId);

		// Token: 0x020001AE RID: 430
		// (Invoke) Token: 0x06000CD1 RID: 3281
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetPhysicsMaterialIndexDelegate(UIntPtr entityId);

		// Token: 0x020001AF RID: 431
		// (Invoke) Token: 0x06000CD5 RID: 3285
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetPhysicsMinMaxDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool includeChildren, ref Vec3 bbmin, ref Vec3 bbmax, [MarshalAs(UnmanagedType.U1)] bool returnLocal);

		// Token: 0x020001B0 RID: 432
		// (Invoke) Token: 0x06000CD9 RID: 3289
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetPhysicsStateDelegate(UIntPtr entityId);

		// Token: 0x020001B1 RID: 433
		// (Invoke) Token: 0x06000CDD RID: 3293
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetPhysicsTriangleCountDelegate(UIntPtr entityId);

		// Token: 0x020001B2 RID: 434
		// (Invoke) Token: 0x06000CE1 RID: 3297
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetPrefabNameDelegate(UIntPtr prefab);

		// Token: 0x020001B3 RID: 435
		// (Invoke) Token: 0x06000CE5 RID: 3301
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetPreviousGlobalFrameDelegate(UIntPtr entityPtr, out MatrixFrame frame);

		// Token: 0x020001B4 RID: 436
		// (Invoke) Token: 0x06000CE9 RID: 3305
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetQuickBoneEntitialFrameDelegate(UIntPtr entityId, sbyte index, out MatrixFrame frame);

		// Token: 0x020001B5 RID: 437
		// (Invoke) Token: 0x06000CED RID: 3309
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetRadiusDelegate(UIntPtr entityId);

		// Token: 0x020001B6 RID: 438
		// (Invoke) Token: 0x06000CF1 RID: 3313
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetRootParentPointerDelegate(UIntPtr entityId);

		// Token: 0x020001B7 RID: 439
		// (Invoke) Token: 0x06000CF5 RID: 3317
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetSceneDelegate(UIntPtr entityId);

		// Token: 0x020001B8 RID: 440
		// (Invoke) Token: 0x06000CF9 RID: 3321
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetScenePointerDelegate(UIntPtr entityId);

		// Token: 0x020001B9 RID: 441
		// (Invoke) Token: 0x06000CFD RID: 3325
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetScriptComponentDelegate(UIntPtr entityId);

		// Token: 0x020001BA RID: 442
		// (Invoke) Token: 0x06000D01 RID: 3329
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetScriptComponentAtIndexDelegate(UIntPtr entityId, int index);

		// Token: 0x020001BB RID: 443
		// (Invoke) Token: 0x06000D05 RID: 3333
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetScriptComponentCountDelegate(UIntPtr entityId);

		// Token: 0x020001BC RID: 444
		// (Invoke) Token: 0x06000D09 RID: 3337
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetScriptComponentIndexDelegate(UIntPtr entityId, uint nameHash);

		// Token: 0x020001BD RID: 445
		// (Invoke) Token: 0x06000D0D RID: 3341
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetSkeletonDelegate(UIntPtr entityId);

		// Token: 0x020001BE RID: 446
		// (Invoke) Token: 0x06000D11 RID: 3345
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetTagsDelegate(UIntPtr entityId);

		// Token: 0x020001BF RID: 447
		// (Invoke) Token: 0x06000D15 RID: 3349
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetUpgradeLevelMaskDelegate(UIntPtr prefab);

		// Token: 0x020001C0 RID: 448
		// (Invoke) Token: 0x06000D19 RID: 3353
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetUpgradeLevelMaskCumulativeDelegate(UIntPtr prefab);

		// Token: 0x020001C1 RID: 449
		// (Invoke) Token: 0x06000D1D RID: 3357
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetVisibilityExcludeParentsDelegate(UIntPtr entityId);

		// Token: 0x020001C2 RID: 450
		// (Invoke) Token: 0x06000D21 RID: 3361
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetVisibilityLevelMaskIncludingParentsDelegate(UIntPtr entityId);

		// Token: 0x020001C3 RID: 451
		// (Invoke) Token: 0x06000D25 RID: 3365
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetWaterLevelAtPositionDelegate(UIntPtr entityId, in Vec2 position, [MarshalAs(UnmanagedType.U1)] bool useWaterRenderer, [MarshalAs(UnmanagedType.U1)] bool checkWaterBodyEntities);

		// Token: 0x020001C4 RID: 452
		// (Invoke) Token: 0x06000D29 RID: 3369
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasBatchedKinematicPhysicsFlagDelegate(UIntPtr entityId);

		// Token: 0x020001C5 RID: 453
		// (Invoke) Token: 0x06000D2D RID: 3373
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasBatchedRayCastPhysicsFlagDelegate(UIntPtr entityId);

		// Token: 0x020001C6 RID: 454
		// (Invoke) Token: 0x06000D31 RID: 3377
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasBodyDelegate(UIntPtr entityId);

		// Token: 0x020001C7 RID: 455
		// (Invoke) Token: 0x06000D35 RID: 3381
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasComplexAnimTreeDelegate(UIntPtr entityId);

		// Token: 0x020001C8 RID: 456
		// (Invoke) Token: 0x06000D39 RID: 3385
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasComponentDelegate(UIntPtr pointer, UIntPtr componentPointer);

		// Token: 0x020001C9 RID: 457
		// (Invoke) Token: 0x06000D3D RID: 3389
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasDynamicRigidBodyDelegate(UIntPtr entityId);

		// Token: 0x020001CA RID: 458
		// (Invoke) Token: 0x06000D41 RID: 3393
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasDynamicRigidBodyAndActiveSimulationDelegate(UIntPtr entityId);

		// Token: 0x020001CB RID: 459
		// (Invoke) Token: 0x06000D45 RID: 3397
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasFrameChangedDelegate(UIntPtr entityId);

		// Token: 0x020001CC RID: 460
		// (Invoke) Token: 0x06000D49 RID: 3401
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasKinematicRigidBodyDelegate(UIntPtr entityId);

		// Token: 0x020001CD RID: 461
		// (Invoke) Token: 0x06000D4D RID: 3405
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasPhysicsBodyDelegate(UIntPtr entityId);

		// Token: 0x020001CE RID: 462
		// (Invoke) Token: 0x06000D51 RID: 3409
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasPhysicsDefinitionDelegate(UIntPtr entityId, int excludeFlags);

		// Token: 0x020001CF RID: 463
		// (Invoke) Token: 0x06000D55 RID: 3413
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasSceneDelegate(UIntPtr entityId);

		// Token: 0x020001D0 RID: 464
		// (Invoke) Token: 0x06000D59 RID: 3417
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasScriptComponentDelegate(UIntPtr entityId, byte[] scName);

		// Token: 0x020001D1 RID: 465
		// (Invoke) Token: 0x06000D5D RID: 3421
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasScriptComponentHashDelegate(UIntPtr entityId, uint scNameHash);

		// Token: 0x020001D2 RID: 466
		// (Invoke) Token: 0x06000D61 RID: 3425
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasStaticPhysicsBodyDelegate(UIntPtr entityId);

		// Token: 0x020001D3 RID: 467
		// (Invoke) Token: 0x06000D65 RID: 3429
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasTagDelegate(UIntPtr entityId, byte[] tag);

		// Token: 0x020001D4 RID: 468
		// (Invoke) Token: 0x06000D69 RID: 3433
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsDynamicBodyStationaryDelegate(UIntPtr entityId);

		// Token: 0x020001D5 RID: 469
		// (Invoke) Token: 0x06000D6D RID: 3437
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEngineBodySleepingDelegate(UIntPtr entityId);

		// Token: 0x020001D6 RID: 470
		// (Invoke) Token: 0x06000D71 RID: 3441
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEntitySelectedOnEditorDelegate(UIntPtr entityId);

		// Token: 0x020001D7 RID: 471
		// (Invoke) Token: 0x06000D75 RID: 3445
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsFrozenDelegate(UIntPtr entityId);

		// Token: 0x020001D8 RID: 472
		// (Invoke) Token: 0x06000D79 RID: 3449
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsGhostObjectDelegate(UIntPtr entityId);

		// Token: 0x020001D9 RID: 473
		// (Invoke) Token: 0x06000D7D RID: 3453
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsGravityDisabledDelegate(UIntPtr entityId);

		// Token: 0x020001DA RID: 474
		// (Invoke) Token: 0x06000D81 RID: 3457
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsGuidValidDelegate(UIntPtr entityId);

		// Token: 0x020001DB RID: 475
		// (Invoke) Token: 0x06000D85 RID: 3461
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsInEditorSceneDelegate(UIntPtr pointer);

		// Token: 0x020001DC RID: 476
		// (Invoke) Token: 0x06000D89 RID: 3465
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsVisibleIncludeParentsDelegate(UIntPtr entityId);

		// Token: 0x020001DD RID: 477
		// (Invoke) Token: 0x06000D8D RID: 3469
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PauseParticleSystemDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool doChildren);

		// Token: 0x020001DE RID: 478
		// (Invoke) Token: 0x06000D91 RID: 3473
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PopCapsuleShapeFromEntityBodyDelegate(UIntPtr entityId);

		// Token: 0x020001DF RID: 479
		// (Invoke) Token: 0x06000D95 RID: 3477
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool PrefabExistsDelegate(byte[] prefabName);

		// Token: 0x020001E0 RID: 480
		// (Invoke) Token: 0x06000D99 RID: 3481
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PushCapsuleShapeToEntityBodyDelegate(UIntPtr entityId, Vec3 p1, Vec3 p2, float radius, byte[] physicsMaterialName);

		// Token: 0x020001E1 RID: 481
		// (Invoke) Token: 0x06000D9D RID: 3485
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool RayHitEntityDelegate(UIntPtr entityId, in Vec3 rayOrigin, in Vec3 rayDirection, float maxLength, ref float resultLength);

		// Token: 0x020001E2 RID: 482
		// (Invoke) Token: 0x06000DA1 RID: 3489
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool RayHitEntityWithNormalDelegate(UIntPtr entityId, in Vec3 rayOrigin, in Vec3 rayDirection, float maxLength, ref Vec3 resultNormal, ref float resultLength);

		// Token: 0x020001E3 RID: 483
		// (Invoke) Token: 0x06000DA5 RID: 3493
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RecomputeBoundingBoxDelegate(UIntPtr pointer);

		// Token: 0x020001E4 RID: 484
		// (Invoke) Token: 0x06000DA9 RID: 3497
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RefreshMeshesToRenderToHullWaterDelegate(UIntPtr entityPointer, UIntPtr visualPrefab, byte[] tag);

		// Token: 0x020001E5 RID: 485
		// (Invoke) Token: 0x06000DAD RID: 3501
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int RegisterWaterSDFClipDelegate(UIntPtr entityId, UIntPtr textureID);

		// Token: 0x020001E6 RID: 486
		// (Invoke) Token: 0x06000DB1 RID: 3505
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RelaxLocalBoundingBoxDelegate(UIntPtr entityId, in BoundingBox boundingBox);

		// Token: 0x020001E7 RID: 487
		// (Invoke) Token: 0x06000DB5 RID: 3509
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseEditDataUserToAllMeshesDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool entity_components, [MarshalAs(UnmanagedType.U1)] bool skeleton_components);

		// Token: 0x020001E8 RID: 488
		// (Invoke) Token: 0x06000DB9 RID: 3513
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveDelegate(UIntPtr entityId, int removeReason);

		// Token: 0x020001E9 RID: 489
		// (Invoke) Token: 0x06000DBD RID: 3517
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveAllChildrenDelegate(UIntPtr entityId);

		// Token: 0x020001EA RID: 490
		// (Invoke) Token: 0x06000DC1 RID: 3521
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveAllParticleSystemsDelegate(UIntPtr entityId);

		// Token: 0x020001EB RID: 491
		// (Invoke) Token: 0x06000DC5 RID: 3525
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveChildDelegate(UIntPtr parentEntity, UIntPtr childEntity, [MarshalAs(UnmanagedType.U1)] bool keepPhysics, [MarshalAs(UnmanagedType.U1)] bool keepScenePointer, [MarshalAs(UnmanagedType.U1)] bool callScriptCallbacks, int removeReason);

		// Token: 0x020001EC RID: 492
		// (Invoke) Token: 0x06000DC9 RID: 3529
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool RemoveComponentDelegate(UIntPtr pointer, UIntPtr componentPointer);

		// Token: 0x020001ED RID: 493
		// (Invoke) Token: 0x06000DCD RID: 3533
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool RemoveComponentWithMeshDelegate(UIntPtr entityId, UIntPtr mesh);

		// Token: 0x020001EE RID: 494
		// (Invoke) Token: 0x06000DD1 RID: 3537
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveEnginePhysicsDelegate(UIntPtr entityId);

		// Token: 0x020001EF RID: 495
		// (Invoke) Token: 0x06000DD5 RID: 3541
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveFromPredisplayEntityDelegate(UIntPtr entityId);

		// Token: 0x020001F0 RID: 496
		// (Invoke) Token: 0x06000DD9 RID: 3545
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveJointDelegate(UIntPtr jointId, UIntPtr entityId);

		// Token: 0x020001F1 RID: 497
		// (Invoke) Token: 0x06000DDD RID: 3549
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool RemoveMultiMeshDelegate(UIntPtr entityId, UIntPtr multiMeshPtr);

		// Token: 0x020001F2 RID: 498
		// (Invoke) Token: 0x06000DE1 RID: 3553
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveMultiMeshFromSkeletonDelegate(UIntPtr gameEntity, UIntPtr multiMesh);

		// Token: 0x020001F3 RID: 499
		// (Invoke) Token: 0x06000DE5 RID: 3557
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveMultiMeshFromSkeletonBoneDelegate(UIntPtr gameEntity, UIntPtr multiMesh, sbyte boneIndex);

		// Token: 0x020001F4 RID: 500
		// (Invoke) Token: 0x06000DE9 RID: 3561
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemovePhysicsDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool clearingTheScene);

		// Token: 0x020001F5 RID: 501
		// (Invoke) Token: 0x06000DED RID: 3565
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveScriptComponentDelegate(UIntPtr entityId, UIntPtr scriptComponentPtr, int removeReason);

		// Token: 0x020001F6 RID: 502
		// (Invoke) Token: 0x06000DF1 RID: 3569
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveTagDelegate(UIntPtr entityId, byte[] tag);

		// Token: 0x020001F7 RID: 503
		// (Invoke) Token: 0x06000DF5 RID: 3573
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReplacePhysicsBodyWithQuadPhysicsBodyDelegate(UIntPtr pointer, UIntPtr quad, int physicsMaterial, BodyFlags bodyFlags, int numberOfVertices, UIntPtr indices, int numberOfIndices);

		// Token: 0x020001F8 RID: 504
		// (Invoke) Token: 0x06000DF9 RID: 3577
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResetHullWaterDelegate(UIntPtr visualPrefab);

		// Token: 0x020001F9 RID: 505
		// (Invoke) Token: 0x06000DFD RID: 3581
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResumeParticleSystemDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool doChildren);

		// Token: 0x020001FA RID: 506
		// (Invoke) Token: 0x06000E01 RID: 3585
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SelectEntityOnEditorDelegate(UIntPtr entityId);

		// Token: 0x020001FB RID: 507
		// (Invoke) Token: 0x06000E05 RID: 3589
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAlphaDelegate(UIntPtr entityId, float alpha);

		// Token: 0x020001FC RID: 508
		// (Invoke) Token: 0x06000E09 RID: 3593
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAngularVelocityDelegate(UIntPtr entityPtr, in Vec3 newAngularVelocity);

		// Token: 0x020001FD RID: 509
		// (Invoke) Token: 0x06000E0D RID: 3597
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAnimationSoundActivationDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool activate);

		// Token: 0x020001FE RID: 510
		// (Invoke) Token: 0x06000E11 RID: 3601
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAnimTreeChannelParameterDelegate(UIntPtr entityId, float phase, int channel_no);

		// Token: 0x020001FF RID: 511
		// (Invoke) Token: 0x06000E15 RID: 3605
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAsContourEntityDelegate(UIntPtr entityId, uint color);

		// Token: 0x02000200 RID: 512
		// (Invoke) Token: 0x06000E19 RID: 3609
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAsPredisplayEntityDelegate(UIntPtr entityId);

		// Token: 0x02000201 RID: 513
		// (Invoke) Token: 0x06000E1D RID: 3613
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAsReplayEntityDelegate(UIntPtr gameEntity);

		// Token: 0x02000202 RID: 514
		// (Invoke) Token: 0x06000E21 RID: 3617
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBodyFlagsDelegate(UIntPtr entityId, uint bodyFlags);

		// Token: 0x02000203 RID: 515
		// (Invoke) Token: 0x06000E25 RID: 3621
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBodyFlagsRecursiveDelegate(UIntPtr entityId, uint bodyFlags);

		// Token: 0x02000204 RID: 516
		// (Invoke) Token: 0x06000E29 RID: 3625
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBodyShapeDelegate(UIntPtr entityId, UIntPtr shape);

		// Token: 0x02000205 RID: 517
		// (Invoke) Token: 0x06000E2D RID: 3629
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBoneFrameToAllMeshesDelegate(UIntPtr entityPtr, int boneIndex, in MatrixFrame frame);

		// Token: 0x02000206 RID: 518
		// (Invoke) Token: 0x06000E31 RID: 3633
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBoundingboxDirtyDelegate(UIntPtr entityId);

		// Token: 0x02000207 RID: 519
		// (Invoke) Token: 0x06000E35 RID: 3637
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCenterOfMassDelegate(UIntPtr entityId, ref Vec3 localCenterOfMass);

		// Token: 0x02000208 RID: 520
		// (Invoke) Token: 0x06000E39 RID: 3641
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetClothComponentKeepStateDelegate(UIntPtr entityId, UIntPtr metaMesh, [MarshalAs(UnmanagedType.U1)] bool keepState);

		// Token: 0x02000209 RID: 521
		// (Invoke) Token: 0x06000E3D RID: 3645
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetClothComponentKeepStateOfAllMeshesDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool keepState);

		// Token: 0x0200020A RID: 522
		// (Invoke) Token: 0x06000E41 RID: 3649
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetClothMaxDistanceMultiplierDelegate(UIntPtr gameEntity, float multiplier);

		// Token: 0x0200020B RID: 523
		// (Invoke) Token: 0x06000E45 RID: 3653
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetColorToAllMeshesWithTagRecursiveDelegate(UIntPtr gameEntity, uint color, byte[] tag);

		// Token: 0x0200020C RID: 524
		// (Invoke) Token: 0x06000E49 RID: 3657
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetContourStateDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool alwaysVisible);

		// Token: 0x0200020D RID: 525
		// (Invoke) Token: 0x06000E4D RID: 3661
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCostAdderForAttachedFacesDelegate(UIntPtr entityId, float cost);

		// Token: 0x0200020E RID: 526
		// (Invoke) Token: 0x06000E51 RID: 3665
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCullModeDelegate(UIntPtr entityPtr, MBMeshCullingMode cullMode);

		// Token: 0x0200020F RID: 527
		// (Invoke) Token: 0x06000E55 RID: 3669
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCustomClipPlaneDelegate(UIntPtr entityId, Vec3 position, Vec3 normal, [MarshalAs(UnmanagedType.U1)] bool setForChildren);

		// Token: 0x02000210 RID: 528
		// (Invoke) Token: 0x06000E59 RID: 3673
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCustomVertexPositionEnabledDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool customVertexPositionEnabled);

		// Token: 0x02000211 RID: 529
		// (Invoke) Token: 0x06000E5D RID: 3677
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDampingDelegate(UIntPtr entityId, float linearDamping, float angularDamping);

		// Token: 0x02000212 RID: 530
		// (Invoke) Token: 0x06000E61 RID: 3681
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDoNotCheckVisibilityDelegate(UIntPtr entityPtr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000213 RID: 531
		// (Invoke) Token: 0x06000E65 RID: 3685
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEnforcedMaximumLodLevelDelegate(UIntPtr entityId, int lodLevel);

		// Token: 0x02000214 RID: 532
		// (Invoke) Token: 0x06000E69 RID: 3689
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEntityEnvMapVisibilityDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000215 RID: 533
		// (Invoke) Token: 0x06000E6D RID: 3693
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEntityFlagsDelegate(UIntPtr entityId, EntityFlags entityFlags);

		// Token: 0x02000216 RID: 534
		// (Invoke) Token: 0x06000E71 RID: 3697
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEntityVisibilityFlagsDelegate(UIntPtr entityId, EntityVisibilityFlags entityVisibilityFlags);

		// Token: 0x02000217 RID: 535
		// (Invoke) Token: 0x06000E75 RID: 3701
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetExternalReferencesUsageDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000218 RID: 536
		// (Invoke) Token: 0x06000E79 RID: 3705
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFactor2ColorDelegate(UIntPtr entityId, uint factor2Color);

		// Token: 0x02000219 RID: 537
		// (Invoke) Token: 0x06000E7D RID: 3709
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFactorColorDelegate(UIntPtr entityId, uint factorColor);

		// Token: 0x0200021A RID: 538
		// (Invoke) Token: 0x06000E81 RID: 3713
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetForceDecalsToRenderDelegate(UIntPtr entityPtr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200021B RID: 539
		// (Invoke) Token: 0x06000E85 RID: 3717
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetForceNotAffectedBySeasonDelegate(UIntPtr entityPtr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200021C RID: 540
		// (Invoke) Token: 0x06000E89 RID: 3721
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFrameChangedDelegate(UIntPtr entityId);

		// Token: 0x0200021D RID: 541
		// (Invoke) Token: 0x06000E8D RID: 3725
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetGlobalFrameDelegate(UIntPtr entityId, in MatrixFrame frame, [MarshalAs(UnmanagedType.U1)] bool isTeleportation);

		// Token: 0x0200021E RID: 542
		// (Invoke) Token: 0x06000E91 RID: 3729
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetGlobalPositionDelegate(UIntPtr entityId, in Vec3 position);

		// Token: 0x0200021F RID: 543
		// (Invoke) Token: 0x06000E95 RID: 3733
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetHasCustomBoundingBoxValidationSystemDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool hasCustomBoundingBox);

		// Token: 0x02000220 RID: 544
		// (Invoke) Token: 0x06000E99 RID: 3737
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLinearVelocityDelegate(UIntPtr entityPtr, Vec3 newLinearVelocity);

		// Token: 0x02000221 RID: 545
		// (Invoke) Token: 0x06000E9D RID: 3741
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLocalFrameDelegate(UIntPtr entityId, ref MatrixFrame frame, [MarshalAs(UnmanagedType.U1)] bool isTeleportation);

		// Token: 0x02000222 RID: 546
		// (Invoke) Token: 0x06000EA1 RID: 3745
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLocalPositionDelegate(UIntPtr entityId, Vec3 position);

		// Token: 0x02000223 RID: 547
		// (Invoke) Token: 0x06000EA5 RID: 3749
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetManualGlobalBoundingBoxDelegate(UIntPtr entityId, Vec3 boundingBoxStartGlobal, Vec3 boundingBoxEndGlobal);

		// Token: 0x02000224 RID: 548
		// (Invoke) Token: 0x06000EA9 RID: 3753
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetManualLocalBoundingBoxDelegate(UIntPtr entityId, in BoundingBox boundingBox);

		// Token: 0x02000225 RID: 549
		// (Invoke) Token: 0x06000EAD RID: 3757
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMassAndUpdateInertiaAndCenterOfMassDelegate(UIntPtr entityId, float mass);

		// Token: 0x02000226 RID: 550
		// (Invoke) Token: 0x06000EB1 RID: 3761
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMassSpaceInertiaDelegate(UIntPtr entityId, ref Vec3 inertia);

		// Token: 0x02000227 RID: 551
		// (Invoke) Token: 0x06000EB5 RID: 3765
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMaterialForAllMeshesDelegate(UIntPtr entityId, UIntPtr materialPointer);

		// Token: 0x02000228 RID: 552
		// (Invoke) Token: 0x06000EB9 RID: 3769
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMaxDepenetrationVelocityDelegate(UIntPtr entityId, float maxDepenetrationVelocity);

		// Token: 0x02000229 RID: 553
		// (Invoke) Token: 0x06000EBD RID: 3773
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMobilityDelegate(UIntPtr entityId, GameEntity.Mobility mobility);

		// Token: 0x0200022A RID: 554
		// (Invoke) Token: 0x06000EC1 RID: 3777
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMorphFrameOfComponentsDelegate(UIntPtr entityId, float value);

		// Token: 0x0200022B RID: 555
		// (Invoke) Token: 0x06000EC5 RID: 3781
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetNameDelegate(UIntPtr entityId, byte[] name);

		// Token: 0x0200022C RID: 556
		// (Invoke) Token: 0x06000EC9 RID: 3785
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetNativeScriptComponentVariableDelegate(UIntPtr entityPtr, byte[] className, byte[] fieldName, ref ScriptComponentFieldHolder data, RglScriptFieldType variableType);

		// Token: 0x0200022D RID: 557
		// (Invoke) Token: 0x06000ECD RID: 3789
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPhysicsMoveToBatchedDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200022E RID: 558
		// (Invoke) Token: 0x06000ED1 RID: 3793
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPhysicsStateDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool isEnabled, [MarshalAs(UnmanagedType.U1)] bool setChildren);

		// Token: 0x0200022F RID: 559
		// (Invoke) Token: 0x06000ED5 RID: 3797
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPhysicsStateOnlyVariableDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool isEnabled, [MarshalAs(UnmanagedType.U1)] bool setChildren);

		// Token: 0x02000230 RID: 560
		// (Invoke) Token: 0x06000ED9 RID: 3801
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPositionsForAttachedNavmeshVerticesDelegate(UIntPtr entityId, IntPtr indices, int indexCount, IntPtr positions);

		// Token: 0x02000231 RID: 561
		// (Invoke) Token: 0x06000EDD RID: 3805
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPreviousFrameInvalidDelegate(UIntPtr gameEntity);

		// Token: 0x02000232 RID: 562
		// (Invoke) Token: 0x06000EE1 RID: 3809
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetReadyToRenderDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool ready);

		// Token: 0x02000233 RID: 563
		// (Invoke) Token: 0x06000EE5 RID: 3813
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRuntimeEmissionRateMultiplierDelegate(UIntPtr entityId, float emission_rate_multiplier);

		// Token: 0x02000234 RID: 564
		// (Invoke) Token: 0x06000EE9 RID: 3817
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSkeletonDelegate(UIntPtr entityId, UIntPtr skeletonPointer);

		// Token: 0x02000235 RID: 565
		// (Invoke) Token: 0x06000EED RID: 3821
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSolverIterationCountsDelegate(UIntPtr entityId, int positionIterationCount, int velocityIterationCount);

		// Token: 0x02000236 RID: 566
		// (Invoke) Token: 0x06000EF1 RID: 3825
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetupAdditionalBoneBufferForMeshesDelegate(UIntPtr entityPtr, int boneCount);

		// Token: 0x02000237 RID: 567
		// (Invoke) Token: 0x06000EF5 RID: 3829
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetUpdateValidityOnFrameChangedOfFacesWithIdDelegate(UIntPtr entityId, int faceGroupId, [MarshalAs(UnmanagedType.U1)] bool updateValidity);

		// Token: 0x02000238 RID: 568
		// (Invoke) Token: 0x06000EF9 RID: 3833
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetUpgradeLevelMaskDelegate(UIntPtr prefab, uint mask);

		// Token: 0x02000239 RID: 569
		// (Invoke) Token: 0x06000EFD RID: 3837
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVectorArgumentDelegate(UIntPtr entityId, float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3);

		// Token: 0x0200023A RID: 570
		// (Invoke) Token: 0x06000F01 RID: 3841
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVelocityLimitsDelegate(UIntPtr entityId, float maxLinearVelocity, float maxAngularVelocity);

		// Token: 0x0200023B RID: 571
		// (Invoke) Token: 0x06000F05 RID: 3845
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVisibilityExcludeParentsDelegate(UIntPtr entityId, [MarshalAs(UnmanagedType.U1)] bool visibility);

		// Token: 0x0200023C RID: 572
		// (Invoke) Token: 0x06000F09 RID: 3849
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVisualRecordWakeParamsDelegate(UIntPtr visualRecord, in Vec3 wakeParams);

		// Token: 0x0200023D RID: 573
		// (Invoke) Token: 0x06000F0D RID: 3853
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetWaterSDFClipDataDelegate(UIntPtr entityId, int slotIndex, in MatrixFrame frame, [MarshalAs(UnmanagedType.U1)] bool visibility);

		// Token: 0x0200023E RID: 574
		// (Invoke) Token: 0x06000F11 RID: 3857
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetWaterVisualRecordFrameAndDtDelegate(UIntPtr entityPointer, UIntPtr visualPrefab, in MatrixFrame frame, float dt);

		// Token: 0x0200023F RID: 575
		// (Invoke) Token: 0x06000F15 RID: 3861
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SwapPhysxShapeInEntityDelegate(UIntPtr entityPtr, UIntPtr oldShape, UIntPtr newShape, [MarshalAs(UnmanagedType.U1)] bool isVariable);

		// Token: 0x02000240 RID: 576
		// (Invoke) Token: 0x06000F19 RID: 3865
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateAttachedNavigationMeshFacesDelegate(UIntPtr entityId);

		// Token: 0x02000241 RID: 577
		// (Invoke) Token: 0x06000F1D RID: 3869
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateBodyRestOffsetDelegate(UIntPtr entityId, float restOffset);

		// Token: 0x02000242 RID: 578
		// (Invoke) Token: 0x06000F21 RID: 3873
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateGlobalBoundsDelegate(UIntPtr entityPointer);

		// Token: 0x02000243 RID: 579
		// (Invoke) Token: 0x06000F25 RID: 3877
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateHullWaterEffectFramesDelegate(UIntPtr entityPointer, UIntPtr visualPrefab);

		// Token: 0x02000244 RID: 580
		// (Invoke) Token: 0x06000F29 RID: 3881
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateTriadFrameForEditorDelegate(UIntPtr meshPointer);

		// Token: 0x02000245 RID: 581
		// (Invoke) Token: 0x06000F2D RID: 3885
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateVisibilityMaskDelegate(UIntPtr entityPtr);

		// Token: 0x02000246 RID: 582
		// (Invoke) Token: 0x06000F31 RID: 3889
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ValidateBoundingBoxDelegate(UIntPtr entityPointer);
	}
}
