using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000027 RID: 39
	internal class ScriptingInterfaceOfISkeleton : ISkeleton
	{
		// Token: 0x06000576 RID: 1398 RVA: 0x00018224 File Offset: 0x00016424
		public void ActivateRagdoll(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_ActivateRagdollDelegate(skeletonPointer);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00018231 File Offset: 0x00016431
		public void AddComponent(UIntPtr skeletonPointer, UIntPtr componentPointer)
		{
			ScriptingInterfaceOfISkeleton.call_AddComponentDelegate(skeletonPointer, componentPointer);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00018240 File Offset: 0x00016440
		public void AddComponentToBone(UIntPtr skeletonPointer, sbyte boneIndex, GameEntityComponent component)
		{
			UIntPtr uintPtr = ((component != null) ? component.Pointer : UIntPtr.Zero);
			ScriptingInterfaceOfISkeleton.call_AddComponentToBoneDelegate(skeletonPointer, boneIndex, uintPtr);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00018271 File Offset: 0x00016471
		public void AddMesh(UIntPtr skeletonPointer, UIntPtr mesnPointer)
		{
			ScriptingInterfaceOfISkeleton.call_AddMeshDelegate(skeletonPointer, mesnPointer);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001827F File Offset: 0x0001647F
		public void AddMeshToBone(UIntPtr skeletonPointer, UIntPtr multiMeshPointer, sbyte bone_index)
		{
			ScriptingInterfaceOfISkeleton.call_AddMeshToBoneDelegate(skeletonPointer, multiMeshPointer, bone_index);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00018290 File Offset: 0x00016490
		public void AddPrefabEntityToBone(UIntPtr skeletonPointer, string prefab_name, sbyte boneIndex)
		{
			byte[] array = null;
			if (prefab_name != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(prefab_name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(prefab_name, 0, prefab_name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISkeleton.call_AddPrefabEntityToBoneDelegate(skeletonPointer, array, boneIndex);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x000182EC File Offset: 0x000164EC
		public void ClearComponents(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_ClearComponentsDelegate(skeletonPointer);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x000182F9 File Offset: 0x000164F9
		public void ClearMeshes(UIntPtr skeletonPointer, bool clearBoneComponents)
		{
			ScriptingInterfaceOfISkeleton.call_ClearMeshesDelegate(skeletonPointer, clearBoneComponents);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00018307 File Offset: 0x00016507
		public void ClearMeshesAtBone(UIntPtr skeletonPointer, sbyte boneIndex)
		{
			ScriptingInterfaceOfISkeleton.call_ClearMeshesAtBoneDelegate(skeletonPointer, boneIndex);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00018318 File Offset: 0x00016518
		public Skeleton CreateFromModel(string skeletonModelName)
		{
			byte[] array = null;
			if (skeletonModelName != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(skeletonModelName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(skeletonModelName, 0, skeletonModelName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfISkeleton.call_CreateFromModelDelegate(array);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000183A4 File Offset: 0x000165A4
		public Skeleton CreateFromModelWithNullAnimTree(UIntPtr entityPointer, string skeletonModelName, float scale)
		{
			byte[] array = null;
			if (skeletonModelName != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(skeletonModelName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(skeletonModelName, 0, skeletonModelName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfISkeleton.call_CreateFromModelWithNullAnimTreeDelegate(entityPointer, array, scale);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00018432 File Offset: 0x00016632
		public void EnableScriptDrivenPostIntegrateCallback(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_EnableScriptDrivenPostIntegrateCallbackDelegate(skeletonPointer);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001843F File Offset: 0x0001663F
		public void ForceUpdateBoneFrames(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_ForceUpdateBoneFramesDelegate(skeletonPointer);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001844C File Offset: 0x0001664C
		public void Freeze(UIntPtr skeletonPointer, bool isFrozen)
		{
			ScriptingInterfaceOfISkeleton.call_FreezeDelegate(skeletonPointer, isFrozen);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001845C File Offset: 0x0001665C
		public void GetAllMeshes(Skeleton skeleton, NativeObjectArray nativeObjectArray)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			UIntPtr uintPtr2 = ((nativeObjectArray != null) ? nativeObjectArray.Pointer : UIntPtr.Zero);
			ScriptingInterfaceOfISkeleton.call_GetAllMeshesDelegate(uintPtr, uintPtr2);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000184A3 File Offset: 0x000166A3
		public string GetAnimationAtChannel(UIntPtr skeletonPointer, int channelNo)
		{
			if (ScriptingInterfaceOfISkeleton.call_GetAnimationAtChannelDelegate(skeletonPointer, channelNo) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x000184BB File Offset: 0x000166BB
		public int GetAnimationIndexAtChannel(UIntPtr skeletonPointer, int channelNo)
		{
			return ScriptingInterfaceOfISkeleton.call_GetAnimationIndexAtChannelDelegate(skeletonPointer, channelNo);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000184C9 File Offset: 0x000166C9
		public void GetBoneBody(UIntPtr skeletonPointer, sbyte boneIndex, ref CapsuleData data)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneBodyDelegate(skeletonPointer, boneIndex, ref data);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000184D8 File Offset: 0x000166D8
		public sbyte GetBoneChildAtIndex(Skeleton skeleton, sbyte boneIndex, sbyte childIndex)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			return ScriptingInterfaceOfISkeleton.call_GetBoneChildAtIndexDelegate(uintPtr, boneIndex, childIndex);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001850C File Offset: 0x0001670C
		public sbyte GetBoneChildCount(Skeleton skeleton, sbyte boneIndex)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			return ScriptingInterfaceOfISkeleton.call_GetBoneChildCountDelegate(uintPtr, boneIndex);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001853C File Offset: 0x0001673C
		public GameEntityComponent GetBoneComponentAtIndex(UIntPtr skeletonPointer, sbyte boneIndex, int componentIndex)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfISkeleton.call_GetBoneComponentAtIndexDelegate(skeletonPointer, boneIndex, componentIndex);
			GameEntityComponent gameEntityComponent = NativeObject.CreateNativeObjectWrapper<GameEntityComponent>(nativeObjectPointer);
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntityComponent;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001857F File Offset: 0x0001677F
		public int GetBoneComponentCount(UIntPtr skeletonPointer, sbyte boneIndex)
		{
			return ScriptingInterfaceOfISkeleton.call_GetBoneComponentCountDelegate(skeletonPointer, boneIndex);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001858D File Offset: 0x0001678D
		public sbyte GetBoneCount(UIntPtr skeletonPointer)
		{
			return ScriptingInterfaceOfISkeleton.call_GetBoneCountDelegate(skeletonPointer);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001859A File Offset: 0x0001679A
		public void GetBoneEntitialFrame(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneEntitialFrameDelegate(skeletonPointer, boneIndex, ref outFrame);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x000185A9 File Offset: 0x000167A9
		public void GetBoneEntitialFrameAtChannel(UIntPtr skeletonPointer, int channelNo, sbyte boneIndex, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneEntitialFrameAtChannelDelegate(skeletonPointer, channelNo, boneIndex, ref outFrame);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000185BA File Offset: 0x000167BA
		public void GetBoneEntitialFrameWithIndex(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame outEntitialFrame)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneEntitialFrameWithIndexDelegate(skeletonPointer, boneIndex, ref outEntitialFrame);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x000185CC File Offset: 0x000167CC
		public void GetBoneEntitialFrameWithName(UIntPtr skeletonPointer, string boneName, ref MatrixFrame outEntitialFrame)
		{
			byte[] array = null;
			if (boneName != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(boneName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(boneName, 0, boneName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISkeleton.call_GetBoneEntitialFrameWithNameDelegate(skeletonPointer, array, ref outEntitialFrame);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00018628 File Offset: 0x00016828
		public void GetBoneEntitialRestFrame(UIntPtr skeletonPointer, sbyte boneIndex, bool useBoneMapping, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneEntitialRestFrameDelegate(skeletonPointer, boneIndex, useBoneMapping, ref outFrame);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001863C File Offset: 0x0001683C
		public sbyte GetBoneIndexFromName(string skeletonModelName, string boneName)
		{
			byte[] array = null;
			if (skeletonModelName != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(skeletonModelName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(skeletonModelName, 0, skeletonModelName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (boneName != null)
			{
				int byteCount2 = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(boneName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(boneName, 0, boneName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfISkeleton.call_GetBoneIndexFromNameDelegate(array, array2);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x000186D9 File Offset: 0x000168D9
		public void GetBoneLocalRestFrame(UIntPtr skeletonPointer, sbyte boneIndex, bool useBoneMapping, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneLocalRestFrameDelegate(skeletonPointer, boneIndex, useBoneMapping, ref outFrame);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x000186EC File Offset: 0x000168EC
		public string GetBoneName(Skeleton skeleton, sbyte boneIndex)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			if (ScriptingInterfaceOfISkeleton.call_GetBoneNameDelegate(uintPtr, boneIndex) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00018728 File Offset: 0x00016928
		public GameEntityComponent GetComponentAtIndex(UIntPtr skeletonPointer, GameEntity.ComponentType componentType, int index)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfISkeleton.call_GetComponentAtIndexDelegate(skeletonPointer, componentType, index);
			GameEntityComponent gameEntityComponent = NativeObject.CreateNativeObjectWrapper<GameEntityComponent>(nativeObjectPointer);
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntityComponent;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001876B File Offset: 0x0001696B
		public int GetComponentCount(UIntPtr skeletonPointer, GameEntity.ComponentType componentType)
		{
			return ScriptingInterfaceOfISkeleton.call_GetComponentCountDelegate(skeletonPointer, componentType);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00018779 File Offset: 0x00016979
		public RagdollState GetCurrentRagdollState(UIntPtr skeletonPointer)
		{
			return ScriptingInterfaceOfISkeleton.call_GetCurrentRagdollStateDelegate(skeletonPointer);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00018786 File Offset: 0x00016986
		public Transformation GetEntitialOutTransform(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex)
		{
			return ScriptingInterfaceOfISkeleton.call_GetEntitialOutTransformDelegate(skeletonPointer, animResultPointer, boneIndex);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00018798 File Offset: 0x00016998
		public string GetName(Skeleton skeleton)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			if (ScriptingInterfaceOfISkeleton.call_GetNameDelegate(uintPtr) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x000187D4 File Offset: 0x000169D4
		public sbyte GetParentBoneIndex(Skeleton skeleton, sbyte boneIndex)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			return ScriptingInterfaceOfISkeleton.call_GetParentBoneIndexDelegate(uintPtr, boneIndex);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00018804 File Offset: 0x00016A04
		public float GetSkeletonAnimationParameterAtChannel(UIntPtr skeletonPointer, int channelNo)
		{
			return ScriptingInterfaceOfISkeleton.call_GetSkeletonAnimationParameterAtChannelDelegate(skeletonPointer, channelNo);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00018812 File Offset: 0x00016A12
		public float GetSkeletonAnimationSpeedAtChannel(UIntPtr skeletonPointer, int channelNo)
		{
			return ScriptingInterfaceOfISkeleton.call_GetSkeletonAnimationSpeedAtChannelDelegate(skeletonPointer, channelNo);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00018820 File Offset: 0x00016A20
		public sbyte GetSkeletonBoneMapping(UIntPtr skeletonPointer, sbyte boneIndex)
		{
			return ScriptingInterfaceOfISkeleton.call_GetSkeletonBoneMappingDelegate(skeletonPointer, boneIndex);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00018830 File Offset: 0x00016A30
		public bool HasBoneComponent(UIntPtr skeletonPointer, sbyte boneIndex, GameEntityComponent component)
		{
			UIntPtr uintPtr = ((component != null) ? component.Pointer : UIntPtr.Zero);
			return ScriptingInterfaceOfISkeleton.call_HasBoneComponentDelegate(skeletonPointer, boneIndex, uintPtr);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00018861 File Offset: 0x00016A61
		public bool HasComponent(UIntPtr skeletonPointer, UIntPtr componentPointer)
		{
			return ScriptingInterfaceOfISkeleton.call_HasComponentDelegate(skeletonPointer, componentPointer);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001886F File Offset: 0x00016A6F
		public bool IsFrozen(UIntPtr skeletonPointer)
		{
			return ScriptingInterfaceOfISkeleton.call_IsFrozenDelegate(skeletonPointer);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001887C File Offset: 0x00016A7C
		public void RemoveBoneComponent(UIntPtr skeletonPointer, sbyte boneIndex, GameEntityComponent component)
		{
			UIntPtr uintPtr = ((component != null) ? component.Pointer : UIntPtr.Zero);
			ScriptingInterfaceOfISkeleton.call_RemoveBoneComponentDelegate(skeletonPointer, boneIndex, uintPtr);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x000188AD File Offset: 0x00016AAD
		public void RemoveComponent(UIntPtr SkeletonPointer, UIntPtr componentPointer)
		{
			ScriptingInterfaceOfISkeleton.call_RemoveComponentDelegate(SkeletonPointer, componentPointer);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x000188BB File Offset: 0x00016ABB
		public void ResetCloths(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_ResetClothsDelegate(skeletonPointer);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000188C8 File Offset: 0x00016AC8
		public void ResetFrames(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_ResetFramesDelegate(skeletonPointer);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x000188D5 File Offset: 0x00016AD5
		public void SetBoneLocalFrame(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame localFrame)
		{
			ScriptingInterfaceOfISkeleton.call_SetBoneLocalFrameDelegate(skeletonPointer, boneIndex, ref localFrame);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x000188E4 File Offset: 0x00016AE4
		public void SetOutBoneDisplacement(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex, Vec3 displacement)
		{
			ScriptingInterfaceOfISkeleton.call_SetOutBoneDisplacementDelegate(skeletonPointer, animResultPointer, boneIndex, displacement);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000188F5 File Offset: 0x00016AF5
		public void SetOutQuat(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex, Mat3 rotation)
		{
			ScriptingInterfaceOfISkeleton.call_SetOutQuatDelegate(skeletonPointer, animResultPointer, boneIndex, rotation);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00018906 File Offset: 0x00016B06
		public void SetSkeletonAnimationParameterAtChannel(UIntPtr skeletonPointer, int channelNo, float parameter)
		{
			ScriptingInterfaceOfISkeleton.call_SetSkeletonAnimationParameterAtChannelDelegate(skeletonPointer, channelNo, parameter);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00018915 File Offset: 0x00016B15
		public void SetSkeletonAnimationSpeedAtChannel(UIntPtr skeletonPointer, int channelNo, float speed)
		{
			ScriptingInterfaceOfISkeleton.call_SetSkeletonAnimationSpeedAtChannelDelegate(skeletonPointer, channelNo, speed);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00018924 File Offset: 0x00016B24
		public void SetSkeletonUptoDate(UIntPtr skeletonPointer, bool value)
		{
			ScriptingInterfaceOfISkeleton.call_SetSkeletonUptoDateDelegate(skeletonPointer, value);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00018932 File Offset: 0x00016B32
		public void SetUsePreciseBoundingVolume(UIntPtr skeletonPointer, bool value)
		{
			ScriptingInterfaceOfISkeleton.call_SetUsePreciseBoundingVolumeDelegate(skeletonPointer, value);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00018940 File Offset: 0x00016B40
		public bool SkeletonModelExist(string skeletonModelName)
		{
			byte[] array = null;
			if (skeletonModelName != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(skeletonModelName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(skeletonModelName, 0, skeletonModelName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfISkeleton.call_SkeletonModelExistDelegate(array);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001899A File Offset: 0x00016B9A
		public void TickAnimations(UIntPtr skeletonPointer, ref MatrixFrame globalFrame, float dt, bool tickAnimsForChildren)
		{
			ScriptingInterfaceOfISkeleton.call_TickAnimationsDelegate(skeletonPointer, ref globalFrame, dt, tickAnimsForChildren);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x000189AB File Offset: 0x00016BAB
		public void TickAnimationsAndForceUpdate(UIntPtr skeletonPointer, ref MatrixFrame globalFrame, float dt, bool tickAnimsForChildren)
		{
			ScriptingInterfaceOfISkeleton.call_TickAnimationsAndForceUpdateDelegate(skeletonPointer, ref globalFrame, dt, tickAnimsForChildren);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x000189BC File Offset: 0x00016BBC
		public void UpdateEntitialFramesFromLocalFrames(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_UpdateEntitialFramesFromLocalFramesDelegate(skeletonPointer);
		}

		// Token: 0x040004CC RID: 1228
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040004CD RID: 1229
		public static ScriptingInterfaceOfISkeleton.ActivateRagdollDelegate call_ActivateRagdollDelegate;

		// Token: 0x040004CE RID: 1230
		public static ScriptingInterfaceOfISkeleton.AddComponentDelegate call_AddComponentDelegate;

		// Token: 0x040004CF RID: 1231
		public static ScriptingInterfaceOfISkeleton.AddComponentToBoneDelegate call_AddComponentToBoneDelegate;

		// Token: 0x040004D0 RID: 1232
		public static ScriptingInterfaceOfISkeleton.AddMeshDelegate call_AddMeshDelegate;

		// Token: 0x040004D1 RID: 1233
		public static ScriptingInterfaceOfISkeleton.AddMeshToBoneDelegate call_AddMeshToBoneDelegate;

		// Token: 0x040004D2 RID: 1234
		public static ScriptingInterfaceOfISkeleton.AddPrefabEntityToBoneDelegate call_AddPrefabEntityToBoneDelegate;

		// Token: 0x040004D3 RID: 1235
		public static ScriptingInterfaceOfISkeleton.ClearComponentsDelegate call_ClearComponentsDelegate;

		// Token: 0x040004D4 RID: 1236
		public static ScriptingInterfaceOfISkeleton.ClearMeshesDelegate call_ClearMeshesDelegate;

		// Token: 0x040004D5 RID: 1237
		public static ScriptingInterfaceOfISkeleton.ClearMeshesAtBoneDelegate call_ClearMeshesAtBoneDelegate;

		// Token: 0x040004D6 RID: 1238
		public static ScriptingInterfaceOfISkeleton.CreateFromModelDelegate call_CreateFromModelDelegate;

		// Token: 0x040004D7 RID: 1239
		public static ScriptingInterfaceOfISkeleton.CreateFromModelWithNullAnimTreeDelegate call_CreateFromModelWithNullAnimTreeDelegate;

		// Token: 0x040004D8 RID: 1240
		public static ScriptingInterfaceOfISkeleton.EnableScriptDrivenPostIntegrateCallbackDelegate call_EnableScriptDrivenPostIntegrateCallbackDelegate;

		// Token: 0x040004D9 RID: 1241
		public static ScriptingInterfaceOfISkeleton.ForceUpdateBoneFramesDelegate call_ForceUpdateBoneFramesDelegate;

		// Token: 0x040004DA RID: 1242
		public static ScriptingInterfaceOfISkeleton.FreezeDelegate call_FreezeDelegate;

		// Token: 0x040004DB RID: 1243
		public static ScriptingInterfaceOfISkeleton.GetAllMeshesDelegate call_GetAllMeshesDelegate;

		// Token: 0x040004DC RID: 1244
		public static ScriptingInterfaceOfISkeleton.GetAnimationAtChannelDelegate call_GetAnimationAtChannelDelegate;

		// Token: 0x040004DD RID: 1245
		public static ScriptingInterfaceOfISkeleton.GetAnimationIndexAtChannelDelegate call_GetAnimationIndexAtChannelDelegate;

		// Token: 0x040004DE RID: 1246
		public static ScriptingInterfaceOfISkeleton.GetBoneBodyDelegate call_GetBoneBodyDelegate;

		// Token: 0x040004DF RID: 1247
		public static ScriptingInterfaceOfISkeleton.GetBoneChildAtIndexDelegate call_GetBoneChildAtIndexDelegate;

		// Token: 0x040004E0 RID: 1248
		public static ScriptingInterfaceOfISkeleton.GetBoneChildCountDelegate call_GetBoneChildCountDelegate;

		// Token: 0x040004E1 RID: 1249
		public static ScriptingInterfaceOfISkeleton.GetBoneComponentAtIndexDelegate call_GetBoneComponentAtIndexDelegate;

		// Token: 0x040004E2 RID: 1250
		public static ScriptingInterfaceOfISkeleton.GetBoneComponentCountDelegate call_GetBoneComponentCountDelegate;

		// Token: 0x040004E3 RID: 1251
		public static ScriptingInterfaceOfISkeleton.GetBoneCountDelegate call_GetBoneCountDelegate;

		// Token: 0x040004E4 RID: 1252
		public static ScriptingInterfaceOfISkeleton.GetBoneEntitialFrameDelegate call_GetBoneEntitialFrameDelegate;

		// Token: 0x040004E5 RID: 1253
		public static ScriptingInterfaceOfISkeleton.GetBoneEntitialFrameAtChannelDelegate call_GetBoneEntitialFrameAtChannelDelegate;

		// Token: 0x040004E6 RID: 1254
		public static ScriptingInterfaceOfISkeleton.GetBoneEntitialFrameWithIndexDelegate call_GetBoneEntitialFrameWithIndexDelegate;

		// Token: 0x040004E7 RID: 1255
		public static ScriptingInterfaceOfISkeleton.GetBoneEntitialFrameWithNameDelegate call_GetBoneEntitialFrameWithNameDelegate;

		// Token: 0x040004E8 RID: 1256
		public static ScriptingInterfaceOfISkeleton.GetBoneEntitialRestFrameDelegate call_GetBoneEntitialRestFrameDelegate;

		// Token: 0x040004E9 RID: 1257
		public static ScriptingInterfaceOfISkeleton.GetBoneIndexFromNameDelegate call_GetBoneIndexFromNameDelegate;

		// Token: 0x040004EA RID: 1258
		public static ScriptingInterfaceOfISkeleton.GetBoneLocalRestFrameDelegate call_GetBoneLocalRestFrameDelegate;

		// Token: 0x040004EB RID: 1259
		public static ScriptingInterfaceOfISkeleton.GetBoneNameDelegate call_GetBoneNameDelegate;

		// Token: 0x040004EC RID: 1260
		public static ScriptingInterfaceOfISkeleton.GetComponentAtIndexDelegate call_GetComponentAtIndexDelegate;

		// Token: 0x040004ED RID: 1261
		public static ScriptingInterfaceOfISkeleton.GetComponentCountDelegate call_GetComponentCountDelegate;

		// Token: 0x040004EE RID: 1262
		public static ScriptingInterfaceOfISkeleton.GetCurrentRagdollStateDelegate call_GetCurrentRagdollStateDelegate;

		// Token: 0x040004EF RID: 1263
		public static ScriptingInterfaceOfISkeleton.GetEntitialOutTransformDelegate call_GetEntitialOutTransformDelegate;

		// Token: 0x040004F0 RID: 1264
		public static ScriptingInterfaceOfISkeleton.GetNameDelegate call_GetNameDelegate;

		// Token: 0x040004F1 RID: 1265
		public static ScriptingInterfaceOfISkeleton.GetParentBoneIndexDelegate call_GetParentBoneIndexDelegate;

		// Token: 0x040004F2 RID: 1266
		public static ScriptingInterfaceOfISkeleton.GetSkeletonAnimationParameterAtChannelDelegate call_GetSkeletonAnimationParameterAtChannelDelegate;

		// Token: 0x040004F3 RID: 1267
		public static ScriptingInterfaceOfISkeleton.GetSkeletonAnimationSpeedAtChannelDelegate call_GetSkeletonAnimationSpeedAtChannelDelegate;

		// Token: 0x040004F4 RID: 1268
		public static ScriptingInterfaceOfISkeleton.GetSkeletonBoneMappingDelegate call_GetSkeletonBoneMappingDelegate;

		// Token: 0x040004F5 RID: 1269
		public static ScriptingInterfaceOfISkeleton.HasBoneComponentDelegate call_HasBoneComponentDelegate;

		// Token: 0x040004F6 RID: 1270
		public static ScriptingInterfaceOfISkeleton.HasComponentDelegate call_HasComponentDelegate;

		// Token: 0x040004F7 RID: 1271
		public static ScriptingInterfaceOfISkeleton.IsFrozenDelegate call_IsFrozenDelegate;

		// Token: 0x040004F8 RID: 1272
		public static ScriptingInterfaceOfISkeleton.RemoveBoneComponentDelegate call_RemoveBoneComponentDelegate;

		// Token: 0x040004F9 RID: 1273
		public static ScriptingInterfaceOfISkeleton.RemoveComponentDelegate call_RemoveComponentDelegate;

		// Token: 0x040004FA RID: 1274
		public static ScriptingInterfaceOfISkeleton.ResetClothsDelegate call_ResetClothsDelegate;

		// Token: 0x040004FB RID: 1275
		public static ScriptingInterfaceOfISkeleton.ResetFramesDelegate call_ResetFramesDelegate;

		// Token: 0x040004FC RID: 1276
		public static ScriptingInterfaceOfISkeleton.SetBoneLocalFrameDelegate call_SetBoneLocalFrameDelegate;

		// Token: 0x040004FD RID: 1277
		public static ScriptingInterfaceOfISkeleton.SetOutBoneDisplacementDelegate call_SetOutBoneDisplacementDelegate;

		// Token: 0x040004FE RID: 1278
		public static ScriptingInterfaceOfISkeleton.SetOutQuatDelegate call_SetOutQuatDelegate;

		// Token: 0x040004FF RID: 1279
		public static ScriptingInterfaceOfISkeleton.SetSkeletonAnimationParameterAtChannelDelegate call_SetSkeletonAnimationParameterAtChannelDelegate;

		// Token: 0x04000500 RID: 1280
		public static ScriptingInterfaceOfISkeleton.SetSkeletonAnimationSpeedAtChannelDelegate call_SetSkeletonAnimationSpeedAtChannelDelegate;

		// Token: 0x04000501 RID: 1281
		public static ScriptingInterfaceOfISkeleton.SetSkeletonUptoDateDelegate call_SetSkeletonUptoDateDelegate;

		// Token: 0x04000502 RID: 1282
		public static ScriptingInterfaceOfISkeleton.SetUsePreciseBoundingVolumeDelegate call_SetUsePreciseBoundingVolumeDelegate;

		// Token: 0x04000503 RID: 1283
		public static ScriptingInterfaceOfISkeleton.SkeletonModelExistDelegate call_SkeletonModelExistDelegate;

		// Token: 0x04000504 RID: 1284
		public static ScriptingInterfaceOfISkeleton.TickAnimationsDelegate call_TickAnimationsDelegate;

		// Token: 0x04000505 RID: 1285
		public static ScriptingInterfaceOfISkeleton.TickAnimationsAndForceUpdateDelegate call_TickAnimationsAndForceUpdateDelegate;

		// Token: 0x04000506 RID: 1286
		public static ScriptingInterfaceOfISkeleton.UpdateEntitialFramesFromLocalFramesDelegate call_UpdateEntitialFramesFromLocalFramesDelegate;

		// Token: 0x02000534 RID: 1332
		// (Invoke) Token: 0x06001AE9 RID: 6889
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ActivateRagdollDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000535 RID: 1333
		// (Invoke) Token: 0x06001AED RID: 6893
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddComponentDelegate(UIntPtr skeletonPointer, UIntPtr componentPointer);

		// Token: 0x02000536 RID: 1334
		// (Invoke) Token: 0x06001AF1 RID: 6897
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddComponentToBoneDelegate(UIntPtr skeletonPointer, sbyte boneIndex, UIntPtr component);

		// Token: 0x02000537 RID: 1335
		// (Invoke) Token: 0x06001AF5 RID: 6901
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshDelegate(UIntPtr skeletonPointer, UIntPtr mesnPointer);

		// Token: 0x02000538 RID: 1336
		// (Invoke) Token: 0x06001AF9 RID: 6905
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshToBoneDelegate(UIntPtr skeletonPointer, UIntPtr multiMeshPointer, sbyte bone_index);

		// Token: 0x02000539 RID: 1337
		// (Invoke) Token: 0x06001AFD RID: 6909
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddPrefabEntityToBoneDelegate(UIntPtr skeletonPointer, byte[] prefab_name, sbyte boneIndex);

		// Token: 0x0200053A RID: 1338
		// (Invoke) Token: 0x06001B01 RID: 6913
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearComponentsDelegate(UIntPtr skeletonPointer);

		// Token: 0x0200053B RID: 1339
		// (Invoke) Token: 0x06001B05 RID: 6917
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearMeshesDelegate(UIntPtr skeletonPointer, [MarshalAs(UnmanagedType.U1)] bool clearBoneComponents);

		// Token: 0x0200053C RID: 1340
		// (Invoke) Token: 0x06001B09 RID: 6921
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearMeshesAtBoneDelegate(UIntPtr skeletonPointer, sbyte boneIndex);

		// Token: 0x0200053D RID: 1341
		// (Invoke) Token: 0x06001B0D RID: 6925
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateFromModelDelegate(byte[] skeletonModelName);

		// Token: 0x0200053E RID: 1342
		// (Invoke) Token: 0x06001B11 RID: 6929
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateFromModelWithNullAnimTreeDelegate(UIntPtr entityPointer, byte[] skeletonModelName, float scale);

		// Token: 0x0200053F RID: 1343
		// (Invoke) Token: 0x06001B15 RID: 6933
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnableScriptDrivenPostIntegrateCallbackDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000540 RID: 1344
		// (Invoke) Token: 0x06001B19 RID: 6937
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ForceUpdateBoneFramesDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000541 RID: 1345
		// (Invoke) Token: 0x06001B1D RID: 6941
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FreezeDelegate(UIntPtr skeletonPointer, [MarshalAs(UnmanagedType.U1)] bool isFrozen);

		// Token: 0x02000542 RID: 1346
		// (Invoke) Token: 0x06001B21 RID: 6945
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetAllMeshesDelegate(UIntPtr skeleton, UIntPtr nativeObjectArray);

		// Token: 0x02000543 RID: 1347
		// (Invoke) Token: 0x06001B25 RID: 6949
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationAtChannelDelegate(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x02000544 RID: 1348
		// (Invoke) Token: 0x06001B29 RID: 6953
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationIndexAtChannelDelegate(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x02000545 RID: 1349
		// (Invoke) Token: 0x06001B2D RID: 6957
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneBodyDelegate(UIntPtr skeletonPointer, sbyte boneIndex, ref CapsuleData data);

		// Token: 0x02000546 RID: 1350
		// (Invoke) Token: 0x06001B31 RID: 6961
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetBoneChildAtIndexDelegate(UIntPtr skeleton, sbyte boneIndex, sbyte childIndex);

		// Token: 0x02000547 RID: 1351
		// (Invoke) Token: 0x06001B35 RID: 6965
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetBoneChildCountDelegate(UIntPtr skeleton, sbyte boneIndex);

		// Token: 0x02000548 RID: 1352
		// (Invoke) Token: 0x06001B39 RID: 6969
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetBoneComponentAtIndexDelegate(UIntPtr skeletonPointer, sbyte boneIndex, int componentIndex);

		// Token: 0x02000549 RID: 1353
		// (Invoke) Token: 0x06001B3D RID: 6973
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetBoneComponentCountDelegate(UIntPtr skeletonPointer, sbyte boneIndex);

		// Token: 0x0200054A RID: 1354
		// (Invoke) Token: 0x06001B41 RID: 6977
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetBoneCountDelegate(UIntPtr skeletonPointer);

		// Token: 0x0200054B RID: 1355
		// (Invoke) Token: 0x06001B45 RID: 6981
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameDelegate(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame outFrame);

		// Token: 0x0200054C RID: 1356
		// (Invoke) Token: 0x06001B49 RID: 6985
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameAtChannelDelegate(UIntPtr skeletonPointer, int channelNo, sbyte boneIndex, ref MatrixFrame outFrame);

		// Token: 0x0200054D RID: 1357
		// (Invoke) Token: 0x06001B4D RID: 6989
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameWithIndexDelegate(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame outEntitialFrame);

		// Token: 0x0200054E RID: 1358
		// (Invoke) Token: 0x06001B51 RID: 6993
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameWithNameDelegate(UIntPtr skeletonPointer, byte[] boneName, ref MatrixFrame outEntitialFrame);

		// Token: 0x0200054F RID: 1359
		// (Invoke) Token: 0x06001B55 RID: 6997
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialRestFrameDelegate(UIntPtr skeletonPointer, sbyte boneIndex, [MarshalAs(UnmanagedType.U1)] bool useBoneMapping, ref MatrixFrame outFrame);

		// Token: 0x02000550 RID: 1360
		// (Invoke) Token: 0x06001B59 RID: 7001
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetBoneIndexFromNameDelegate(byte[] skeletonModelName, byte[] boneName);

		// Token: 0x02000551 RID: 1361
		// (Invoke) Token: 0x06001B5D RID: 7005
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneLocalRestFrameDelegate(UIntPtr skeletonPointer, sbyte boneIndex, [MarshalAs(UnmanagedType.U1)] bool useBoneMapping, ref MatrixFrame outFrame);

		// Token: 0x02000552 RID: 1362
		// (Invoke) Token: 0x06001B61 RID: 7009
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetBoneNameDelegate(UIntPtr skeleton, sbyte boneIndex);

		// Token: 0x02000553 RID: 1363
		// (Invoke) Token: 0x06001B65 RID: 7013
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetComponentAtIndexDelegate(UIntPtr skeletonPointer, GameEntity.ComponentType componentType, int index);

		// Token: 0x02000554 RID: 1364
		// (Invoke) Token: 0x06001B69 RID: 7017
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetComponentCountDelegate(UIntPtr skeletonPointer, GameEntity.ComponentType componentType);

		// Token: 0x02000555 RID: 1365
		// (Invoke) Token: 0x06001B6D RID: 7021
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate RagdollState GetCurrentRagdollStateDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000556 RID: 1366
		// (Invoke) Token: 0x06001B71 RID: 7025
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Transformation GetEntitialOutTransformDelegate(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex);

		// Token: 0x02000557 RID: 1367
		// (Invoke) Token: 0x06001B75 RID: 7029
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr skeleton);

		// Token: 0x02000558 RID: 1368
		// (Invoke) Token: 0x06001B79 RID: 7033
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetParentBoneIndexDelegate(UIntPtr skeleton, sbyte boneIndex);

		// Token: 0x02000559 RID: 1369
		// (Invoke) Token: 0x06001B7D RID: 7037
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetSkeletonAnimationParameterAtChannelDelegate(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x0200055A RID: 1370
		// (Invoke) Token: 0x06001B81 RID: 7041
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetSkeletonAnimationSpeedAtChannelDelegate(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x0200055B RID: 1371
		// (Invoke) Token: 0x06001B85 RID: 7045
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetSkeletonBoneMappingDelegate(UIntPtr skeletonPointer, sbyte boneIndex);

		// Token: 0x0200055C RID: 1372
		// (Invoke) Token: 0x06001B89 RID: 7049
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasBoneComponentDelegate(UIntPtr skeletonPointer, sbyte boneIndex, UIntPtr component);

		// Token: 0x0200055D RID: 1373
		// (Invoke) Token: 0x06001B8D RID: 7053
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasComponentDelegate(UIntPtr skeletonPointer, UIntPtr componentPointer);

		// Token: 0x0200055E RID: 1374
		// (Invoke) Token: 0x06001B91 RID: 7057
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsFrozenDelegate(UIntPtr skeletonPointer);

		// Token: 0x0200055F RID: 1375
		// (Invoke) Token: 0x06001B95 RID: 7061
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveBoneComponentDelegate(UIntPtr skeletonPointer, sbyte boneIndex, UIntPtr component);

		// Token: 0x02000560 RID: 1376
		// (Invoke) Token: 0x06001B99 RID: 7065
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveComponentDelegate(UIntPtr SkeletonPointer, UIntPtr componentPointer);

		// Token: 0x02000561 RID: 1377
		// (Invoke) Token: 0x06001B9D RID: 7069
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResetClothsDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000562 RID: 1378
		// (Invoke) Token: 0x06001BA1 RID: 7073
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResetFramesDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000563 RID: 1379
		// (Invoke) Token: 0x06001BA5 RID: 7077
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBoneLocalFrameDelegate(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame localFrame);

		// Token: 0x02000564 RID: 1380
		// (Invoke) Token: 0x06001BA9 RID: 7081
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetOutBoneDisplacementDelegate(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex, Vec3 displacement);

		// Token: 0x02000565 RID: 1381
		// (Invoke) Token: 0x06001BAD RID: 7085
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetOutQuatDelegate(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex, Mat3 rotation);

		// Token: 0x02000566 RID: 1382
		// (Invoke) Token: 0x06001BB1 RID: 7089
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSkeletonAnimationParameterAtChannelDelegate(UIntPtr skeletonPointer, int channelNo, float parameter);

		// Token: 0x02000567 RID: 1383
		// (Invoke) Token: 0x06001BB5 RID: 7093
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSkeletonAnimationSpeedAtChannelDelegate(UIntPtr skeletonPointer, int channelNo, float speed);

		// Token: 0x02000568 RID: 1384
		// (Invoke) Token: 0x06001BB9 RID: 7097
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSkeletonUptoDateDelegate(UIntPtr skeletonPointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000569 RID: 1385
		// (Invoke) Token: 0x06001BBD RID: 7101
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetUsePreciseBoundingVolumeDelegate(UIntPtr skeletonPointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200056A RID: 1386
		// (Invoke) Token: 0x06001BC1 RID: 7105
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool SkeletonModelExistDelegate(byte[] skeletonModelName);

		// Token: 0x0200056B RID: 1387
		// (Invoke) Token: 0x06001BC5 RID: 7109
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickAnimationsDelegate(UIntPtr skeletonPointer, ref MatrixFrame globalFrame, float dt, [MarshalAs(UnmanagedType.U1)] bool tickAnimsForChildren);

		// Token: 0x0200056C RID: 1388
		// (Invoke) Token: 0x06001BC9 RID: 7113
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickAnimationsAndForceUpdateDelegate(UIntPtr skeletonPointer, ref MatrixFrame globalFrame, float dt, [MarshalAs(UnmanagedType.U1)] bool tickAnimsForChildren);

		// Token: 0x0200056D RID: 1389
		// (Invoke) Token: 0x06001BCD RID: 7117
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateEntitialFramesFromLocalFramesDelegate(UIntPtr skeletonPointer);
	}
}
