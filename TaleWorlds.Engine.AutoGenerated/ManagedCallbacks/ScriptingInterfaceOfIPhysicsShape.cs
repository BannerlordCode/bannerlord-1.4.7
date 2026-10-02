using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000021 RID: 33
	internal class ScriptingInterfaceOfIPhysicsShape : IPhysicsShape
	{
		// Token: 0x060003E1 RID: 993 RVA: 0x000150BC File Offset: 0x000132BC
		public void AddCapsule(UIntPtr shapePointer, ref CapsuleData data)
		{
			ScriptingInterfaceOfIPhysicsShape.call_AddCapsuleDelegate(shapePointer, ref data);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000150CC File Offset: 0x000132CC
		public void AddPreloadQueueWithName(string bodyName, ref Vec3 scale)
		{
			byte[] array = null;
			if (bodyName != null)
			{
				int byteCount = ScriptingInterfaceOfIPhysicsShape._utf8.GetByteCount(bodyName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIPhysicsShape._utf8.GetBytes(bodyName, 0, bodyName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIPhysicsShape.call_AddPreloadQueueWithNameDelegate(array, ref scale);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00015127 File Offset: 0x00013327
		public void AddSphere(UIntPtr shapePointer, ref Vec3 origin, float radius)
		{
			ScriptingInterfaceOfIPhysicsShape.call_AddSphereDelegate(shapePointer, ref origin, radius);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00015136 File Offset: 0x00013336
		public int CapsuleCount(UIntPtr shapePointer)
		{
			return ScriptingInterfaceOfIPhysicsShape.call_CapsuleCountDelegate(shapePointer);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00015143 File Offset: 0x00013343
		public void clear(UIntPtr shapePointer)
		{
			ScriptingInterfaceOfIPhysicsShape.call_clearDelegate(shapePointer);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00015150 File Offset: 0x00013350
		public PhysicsShape CreateBodyCopy(UIntPtr bodyPointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIPhysicsShape.call_CreateBodyCopyDelegate(bodyPointer);
			PhysicsShape physicsShape = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				physicsShape = new PhysicsShape(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return physicsShape;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001519A File Offset: 0x0001339A
		public void GetBoundingBox(UIntPtr shapePointer, out BoundingBox boundingBox)
		{
			ScriptingInterfaceOfIPhysicsShape.call_GetBoundingBoxDelegate(shapePointer, out boundingBox);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000151A8 File Offset: 0x000133A8
		public Vec3 GetBoundingBoxCenter(UIntPtr shapePointer)
		{
			return ScriptingInterfaceOfIPhysicsShape.call_GetBoundingBoxCenterDelegate(shapePointer);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000151B5 File Offset: 0x000133B5
		public void GetCapsule(UIntPtr shapePointer, ref CapsuleData data, int index)
		{
			ScriptingInterfaceOfIPhysicsShape.call_GetCapsuleDelegate(shapePointer, ref data, index);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000151C4 File Offset: 0x000133C4
		public void GetCapsuleWithMaterial(UIntPtr shapePointer, ref CapsuleData data, ref int materialIndex, int index)
		{
			ScriptingInterfaceOfIPhysicsShape.call_GetCapsuleWithMaterialDelegate(shapePointer, ref data, ref materialIndex, index);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x000151D8 File Offset: 0x000133D8
		public int GetDominantMaterialForTriangleMesh(PhysicsShape shape, int meshIndex)
		{
			UIntPtr uintPtr = ((shape != null) ? shape.Pointer : UIntPtr.Zero);
			return ScriptingInterfaceOfIPhysicsShape.call_GetDominantMaterialForTriangleMeshDelegate(uintPtr, meshIndex);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00015208 File Offset: 0x00013408
		public PhysicsShape GetFromResource(string bodyName, bool mayReturnNull)
		{
			byte[] array = null;
			if (bodyName != null)
			{
				int byteCount = ScriptingInterfaceOfIPhysicsShape._utf8.GetByteCount(bodyName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIPhysicsShape._utf8.GetBytes(bodyName, 0, bodyName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIPhysicsShape.call_GetFromResourceDelegate(array, mayReturnNull);
			PhysicsShape physicsShape = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				physicsShape = new PhysicsShape(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return physicsShape;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00015298 File Offset: 0x00013498
		public string GetName(PhysicsShape shape)
		{
			UIntPtr uintPtr = ((shape != null) ? shape.Pointer : UIntPtr.Zero);
			if (ScriptingInterfaceOfIPhysicsShape.call_GetNameDelegate(uintPtr) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000152D1 File Offset: 0x000134D1
		public void GetSphere(UIntPtr shapePointer, ref SphereData data, int sphereIndex)
		{
			ScriptingInterfaceOfIPhysicsShape.call_GetSphereDelegate(shapePointer, ref data, sphereIndex);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x000152E0 File Offset: 0x000134E0
		public void GetSphereWithMaterial(UIntPtr shapePointer, ref SphereData data, ref int materialIndex, int sphereIndex)
		{
			ScriptingInterfaceOfIPhysicsShape.call_GetSphereWithMaterialDelegate(shapePointer, ref data, ref materialIndex, sphereIndex);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x000152F4 File Offset: 0x000134F4
		public void GetTriangle(UIntPtr pointer, Vec3[] data, int meshIndex, int triangleIndex)
		{
			PinnedArrayData<Vec3> pinnedArrayData = new PinnedArrayData<Vec3>(data, false);
			IntPtr pointer2 = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIPhysicsShape.call_GetTriangleDelegate(pointer, pointer2, meshIndex, triangleIndex);
			pinnedArrayData.Dispose();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00015328 File Offset: 0x00013528
		public void InitDescription(UIntPtr shapePointer)
		{
			ScriptingInterfaceOfIPhysicsShape.call_InitDescriptionDelegate(shapePointer);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00015335 File Offset: 0x00013535
		public void Prepare(UIntPtr shapePointer)
		{
			ScriptingInterfaceOfIPhysicsShape.call_PrepareDelegate(shapePointer);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00015342 File Offset: 0x00013542
		public void ProcessPreloadQueue()
		{
			ScriptingInterfaceOfIPhysicsShape.call_ProcessPreloadQueueDelegate();
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001534E File Offset: 0x0001354E
		public void SetCapsule(UIntPtr shapePointer, ref CapsuleData data, int index)
		{
			ScriptingInterfaceOfIPhysicsShape.call_SetCapsuleDelegate(shapePointer, ref data, index);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001535D File Offset: 0x0001355D
		public int SphereCount(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIPhysicsShape.call_SphereCountDelegate(pointer);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0001536A File Offset: 0x0001356A
		public void Transform(UIntPtr shapePointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIPhysicsShape.call_TransformDelegate(shapePointer, ref frame);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00015378 File Offset: 0x00013578
		public int TriangleCountInTriangleMesh(UIntPtr pointer, int meshIndex)
		{
			return ScriptingInterfaceOfIPhysicsShape.call_TriangleCountInTriangleMeshDelegate(pointer, meshIndex);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00015386 File Offset: 0x00013586
		public int TriangleMeshCount(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIPhysicsShape.call_TriangleMeshCountDelegate(pointer);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00015393 File Offset: 0x00013593
		public void UnloadDynamicBodies()
		{
			ScriptingInterfaceOfIPhysicsShape.call_UnloadDynamicBodiesDelegate();
		}

		// Token: 0x0400034C RID: 844
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400034D RID: 845
		public static ScriptingInterfaceOfIPhysicsShape.AddCapsuleDelegate call_AddCapsuleDelegate;

		// Token: 0x0400034E RID: 846
		public static ScriptingInterfaceOfIPhysicsShape.AddPreloadQueueWithNameDelegate call_AddPreloadQueueWithNameDelegate;

		// Token: 0x0400034F RID: 847
		public static ScriptingInterfaceOfIPhysicsShape.AddSphereDelegate call_AddSphereDelegate;

		// Token: 0x04000350 RID: 848
		public static ScriptingInterfaceOfIPhysicsShape.CapsuleCountDelegate call_CapsuleCountDelegate;

		// Token: 0x04000351 RID: 849
		public static ScriptingInterfaceOfIPhysicsShape.clearDelegate call_clearDelegate;

		// Token: 0x04000352 RID: 850
		public static ScriptingInterfaceOfIPhysicsShape.CreateBodyCopyDelegate call_CreateBodyCopyDelegate;

		// Token: 0x04000353 RID: 851
		public static ScriptingInterfaceOfIPhysicsShape.GetBoundingBoxDelegate call_GetBoundingBoxDelegate;

		// Token: 0x04000354 RID: 852
		public static ScriptingInterfaceOfIPhysicsShape.GetBoundingBoxCenterDelegate call_GetBoundingBoxCenterDelegate;

		// Token: 0x04000355 RID: 853
		public static ScriptingInterfaceOfIPhysicsShape.GetCapsuleDelegate call_GetCapsuleDelegate;

		// Token: 0x04000356 RID: 854
		public static ScriptingInterfaceOfIPhysicsShape.GetCapsuleWithMaterialDelegate call_GetCapsuleWithMaterialDelegate;

		// Token: 0x04000357 RID: 855
		public static ScriptingInterfaceOfIPhysicsShape.GetDominantMaterialForTriangleMeshDelegate call_GetDominantMaterialForTriangleMeshDelegate;

		// Token: 0x04000358 RID: 856
		public static ScriptingInterfaceOfIPhysicsShape.GetFromResourceDelegate call_GetFromResourceDelegate;

		// Token: 0x04000359 RID: 857
		public static ScriptingInterfaceOfIPhysicsShape.GetNameDelegate call_GetNameDelegate;

		// Token: 0x0400035A RID: 858
		public static ScriptingInterfaceOfIPhysicsShape.GetSphereDelegate call_GetSphereDelegate;

		// Token: 0x0400035B RID: 859
		public static ScriptingInterfaceOfIPhysicsShape.GetSphereWithMaterialDelegate call_GetSphereWithMaterialDelegate;

		// Token: 0x0400035C RID: 860
		public static ScriptingInterfaceOfIPhysicsShape.GetTriangleDelegate call_GetTriangleDelegate;

		// Token: 0x0400035D RID: 861
		public static ScriptingInterfaceOfIPhysicsShape.InitDescriptionDelegate call_InitDescriptionDelegate;

		// Token: 0x0400035E RID: 862
		public static ScriptingInterfaceOfIPhysicsShape.PrepareDelegate call_PrepareDelegate;

		// Token: 0x0400035F RID: 863
		public static ScriptingInterfaceOfIPhysicsShape.ProcessPreloadQueueDelegate call_ProcessPreloadQueueDelegate;

		// Token: 0x04000360 RID: 864
		public static ScriptingInterfaceOfIPhysicsShape.SetCapsuleDelegate call_SetCapsuleDelegate;

		// Token: 0x04000361 RID: 865
		public static ScriptingInterfaceOfIPhysicsShape.SphereCountDelegate call_SphereCountDelegate;

		// Token: 0x04000362 RID: 866
		public static ScriptingInterfaceOfIPhysicsShape.TransformDelegate call_TransformDelegate;

		// Token: 0x04000363 RID: 867
		public static ScriptingInterfaceOfIPhysicsShape.TriangleCountInTriangleMeshDelegate call_TriangleCountInTriangleMeshDelegate;

		// Token: 0x04000364 RID: 868
		public static ScriptingInterfaceOfIPhysicsShape.TriangleMeshCountDelegate call_TriangleMeshCountDelegate;

		// Token: 0x04000365 RID: 869
		public static ScriptingInterfaceOfIPhysicsShape.UnloadDynamicBodiesDelegate call_UnloadDynamicBodiesDelegate;

		// Token: 0x020003BA RID: 954
		// (Invoke) Token: 0x06001501 RID: 5377
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddCapsuleDelegate(UIntPtr shapePointer, ref CapsuleData data);

		// Token: 0x020003BB RID: 955
		// (Invoke) Token: 0x06001505 RID: 5381
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddPreloadQueueWithNameDelegate(byte[] bodyName, ref Vec3 scale);

		// Token: 0x020003BC RID: 956
		// (Invoke) Token: 0x06001509 RID: 5385
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddSphereDelegate(UIntPtr shapePointer, ref Vec3 origin, float radius);

		// Token: 0x020003BD RID: 957
		// (Invoke) Token: 0x0600150D RID: 5389
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int CapsuleCountDelegate(UIntPtr shapePointer);

		// Token: 0x020003BE RID: 958
		// (Invoke) Token: 0x06001511 RID: 5393
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void clearDelegate(UIntPtr shapePointer);

		// Token: 0x020003BF RID: 959
		// (Invoke) Token: 0x06001515 RID: 5397
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateBodyCopyDelegate(UIntPtr bodyPointer);

		// Token: 0x020003C0 RID: 960
		// (Invoke) Token: 0x06001519 RID: 5401
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoundingBoxDelegate(UIntPtr shapePointer, out BoundingBox boundingBox);

		// Token: 0x020003C1 RID: 961
		// (Invoke) Token: 0x0600151D RID: 5405
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetBoundingBoxCenterDelegate(UIntPtr shapePointer);

		// Token: 0x020003C2 RID: 962
		// (Invoke) Token: 0x06001521 RID: 5409
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetCapsuleDelegate(UIntPtr shapePointer, ref CapsuleData data, int index);

		// Token: 0x020003C3 RID: 963
		// (Invoke) Token: 0x06001525 RID: 5413
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetCapsuleWithMaterialDelegate(UIntPtr shapePointer, ref CapsuleData data, ref int materialIndex, int index);

		// Token: 0x020003C4 RID: 964
		// (Invoke) Token: 0x06001529 RID: 5417
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetDominantMaterialForTriangleMeshDelegate(UIntPtr shape, int meshIndex);

		// Token: 0x020003C5 RID: 965
		// (Invoke) Token: 0x0600152D RID: 5421
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFromResourceDelegate(byte[] bodyName, [MarshalAs(UnmanagedType.U1)] bool mayReturnNull);

		// Token: 0x020003C6 RID: 966
		// (Invoke) Token: 0x06001531 RID: 5425
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr shape);

		// Token: 0x020003C7 RID: 967
		// (Invoke) Token: 0x06001535 RID: 5429
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetSphereDelegate(UIntPtr shapePointer, ref SphereData data, int sphereIndex);

		// Token: 0x020003C8 RID: 968
		// (Invoke) Token: 0x06001539 RID: 5433
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetSphereWithMaterialDelegate(UIntPtr shapePointer, ref SphereData data, ref int materialIndex, int sphereIndex);

		// Token: 0x020003C9 RID: 969
		// (Invoke) Token: 0x0600153D RID: 5437
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetTriangleDelegate(UIntPtr pointer, IntPtr data, int meshIndex, int triangleIndex);

		// Token: 0x020003CA RID: 970
		// (Invoke) Token: 0x06001541 RID: 5441
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InitDescriptionDelegate(UIntPtr shapePointer);

		// Token: 0x020003CB RID: 971
		// (Invoke) Token: 0x06001545 RID: 5445
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PrepareDelegate(UIntPtr shapePointer);

		// Token: 0x020003CC RID: 972
		// (Invoke) Token: 0x06001549 RID: 5449
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ProcessPreloadQueueDelegate();

		// Token: 0x020003CD RID: 973
		// (Invoke) Token: 0x0600154D RID: 5453
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCapsuleDelegate(UIntPtr shapePointer, ref CapsuleData data, int index);

		// Token: 0x020003CE RID: 974
		// (Invoke) Token: 0x06001551 RID: 5457
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int SphereCountDelegate(UIntPtr pointer);

		// Token: 0x020003CF RID: 975
		// (Invoke) Token: 0x06001555 RID: 5461
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TransformDelegate(UIntPtr shapePointer, ref MatrixFrame frame);

		// Token: 0x020003D0 RID: 976
		// (Invoke) Token: 0x06001559 RID: 5465
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int TriangleCountInTriangleMeshDelegate(UIntPtr pointer, int meshIndex);

		// Token: 0x020003D1 RID: 977
		// (Invoke) Token: 0x0600155D RID: 5469
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int TriangleMeshCountDelegate(UIntPtr pointer);

		// Token: 0x020003D2 RID: 978
		// (Invoke) Token: 0x06001561 RID: 5473
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UnloadDynamicBodiesDelegate();
	}
}
