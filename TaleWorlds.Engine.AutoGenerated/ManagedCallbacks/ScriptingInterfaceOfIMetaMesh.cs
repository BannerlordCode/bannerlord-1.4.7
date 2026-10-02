using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200001B RID: 27
	internal class ScriptingInterfaceOfIMetaMesh : IMetaMesh
	{
		// Token: 0x0600035A RID: 858 RVA: 0x0001420F File Offset: 0x0001240F
		public void AddEditDataUser(UIntPtr meshPointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_AddEditDataUserDelegate(meshPointer);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0001421C File Offset: 0x0001241C
		public void AddMesh(UIntPtr multiMeshPointer, UIntPtr meshPointer, uint lodLevel)
		{
			ScriptingInterfaceOfIMetaMesh.call_AddMeshDelegate(multiMeshPointer, meshPointer, lodLevel);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0001422B File Offset: 0x0001242B
		public void AddMetaMesh(UIntPtr metaMeshPtr, UIntPtr otherMetaMeshPointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_AddMetaMeshDelegate(metaMeshPtr, otherMetaMeshPointer);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00014239 File Offset: 0x00012439
		public void AssignClothBodyFrom(UIntPtr multiMeshPointer, UIntPtr multiMeshToMergePointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_AssignClothBodyFromDelegate(multiMeshPointer, multiMeshToMergePointer);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00014247 File Offset: 0x00012447
		public void BatchMultiMeshes(UIntPtr multiMeshPointer, UIntPtr multiMeshToMergePointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_BatchMultiMeshesDelegate(multiMeshPointer, multiMeshToMergePointer);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00014258 File Offset: 0x00012458
		public void BatchMultiMeshesMultiple(UIntPtr multiMeshPointer, UIntPtr[] multiMeshToMergePointers, int metaMeshCount)
		{
			PinnedArrayData<UIntPtr> pinnedArrayData = new PinnedArrayData<UIntPtr>(multiMeshToMergePointers, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIMetaMesh.call_BatchMultiMeshesMultipleDelegate(multiMeshPointer, pointer, metaMeshCount);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0001428C File Offset: 0x0001248C
		public void CheckMetaMeshExistence(string multiMeshPrefixName, int lod_count_check)
		{
			byte[] array = null;
			if (multiMeshPrefixName != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(multiMeshPrefixName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(multiMeshPrefixName, 0, multiMeshPrefixName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMetaMesh.call_CheckMetaMeshExistenceDelegate(array, lod_count_check);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000142E7 File Offset: 0x000124E7
		public int CheckResources(UIntPtr meshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_CheckResourcesDelegate(meshPointer);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x000142F4 File Offset: 0x000124F4
		public void ClearEditData(UIntPtr multiMeshPointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_ClearEditDataDelegate(multiMeshPointer);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00014301 File Offset: 0x00012501
		public void ClearMeshes(UIntPtr multiMeshPointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_ClearMeshesDelegate(multiMeshPointer);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0001430E File Offset: 0x0001250E
		public void ClearMeshesForLod(UIntPtr multiMeshPointer, int lodToClear)
		{
			ScriptingInterfaceOfIMetaMesh.call_ClearMeshesForLodDelegate(multiMeshPointer, lodToClear);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0001431C File Offset: 0x0001251C
		public void ClearMeshesForLowerLods(UIntPtr multiMeshPointer, int lod)
		{
			ScriptingInterfaceOfIMetaMesh.call_ClearMeshesForLowerLodsDelegate(multiMeshPointer, lod);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0001432A File Offset: 0x0001252A
		public void ClearMeshesForOtherLods(UIntPtr multiMeshPointer, int lodToKeep)
		{
			ScriptingInterfaceOfIMetaMesh.call_ClearMeshesForOtherLodsDelegate(multiMeshPointer, lodToKeep);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00014338 File Offset: 0x00012538
		public void CopyTo(UIntPtr metaMesh, UIntPtr targetMesh, bool copyMeshes)
		{
			ScriptingInterfaceOfIMetaMesh.call_CopyToDelegate(metaMesh, targetMesh, copyMeshes);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00014348 File Offset: 0x00012548
		public MetaMesh CreateCopy(UIntPtr ptr)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMetaMesh.call_CreateCopyDelegate(ptr);
			MetaMesh metaMesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				metaMesh = new MetaMesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return metaMesh;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00014394 File Offset: 0x00012594
		public MetaMesh CreateCopyFromName(string multiMeshPrefixName, bool showErrors, bool mayReturnNull)
		{
			byte[] array = null;
			if (multiMeshPrefixName != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(multiMeshPrefixName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(multiMeshPrefixName, 0, multiMeshPrefixName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMetaMesh.call_CreateCopyFromNameDelegate(array, showErrors, mayReturnNull);
			MetaMesh metaMesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				metaMesh = new MetaMesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return metaMesh;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00014424 File Offset: 0x00012624
		public MetaMesh CreateMetaMesh(string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMetaMesh.call_CreateMetaMeshDelegate(array);
			MetaMesh metaMesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				metaMesh = new MetaMesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return metaMesh;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x000144B0 File Offset: 0x000126B0
		public void DrawTextWithDefaultFont(UIntPtr multiMeshPointer, string text, Vec2 textPositionMin, Vec2 textPositionMax, Vec2 size, uint color, TextFlags flags)
		{
			byte[] array = null;
			if (text != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(text);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(text, 0, text.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMetaMesh.call_DrawTextWithDefaultFontDelegate(multiMeshPointer, array, textPositionMin, textPositionMax, size, color, flags);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00014514 File Offset: 0x00012714
		public int GetAllMultiMeshes(UIntPtr[] gameEntitiesTemp)
		{
			PinnedArrayData<UIntPtr> pinnedArrayData = new PinnedArrayData<UIntPtr>(gameEntitiesTemp, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			int num = ScriptingInterfaceOfIMetaMesh.call_GetAllMultiMeshesDelegate(pointer);
			pinnedArrayData.Dispose();
			return num;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00014544 File Offset: 0x00012744
		public void GetBoundingBox(UIntPtr multiMeshPointer, ref BoundingBox outBoundingBox)
		{
			ScriptingInterfaceOfIMetaMesh.call_GetBoundingBoxDelegate(multiMeshPointer, ref outBoundingBox);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00014552 File Offset: 0x00012752
		public uint GetFactor1(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_GetFactor1Delegate(multiMeshPointer);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0001455F File Offset: 0x0001275F
		public uint GetFactor2(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_GetFactor2Delegate(multiMeshPointer);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0001456C File Offset: 0x0001276C
		public void GetFrame(UIntPtr multiMeshPointer, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfIMetaMesh.call_GetFrameDelegate(multiMeshPointer, ref outFrame);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001457A File Offset: 0x0001277A
		public int GetLodMaskForMeshAtIndex(UIntPtr multiMeshPointer, int meshIndex)
		{
			return ScriptingInterfaceOfIMetaMesh.call_GetLodMaskForMeshAtIndexDelegate(multiMeshPointer, meshIndex);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00014588 File Offset: 0x00012788
		public Mesh GetMeshAtIndex(UIntPtr multiMeshPointer, int meshIndex)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMetaMesh.call_GetMeshAtIndexDelegate(multiMeshPointer, meshIndex);
			Mesh mesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				mesh = new Mesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return mesh;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000145D3 File Offset: 0x000127D3
		public int GetMeshCount(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_GetMeshCountDelegate(multiMeshPointer);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000145E0 File Offset: 0x000127E0
		public int GetMeshCountWithTag(UIntPtr multiMeshPointer, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMetaMesh.call_GetMeshCountWithTagDelegate(multiMeshPointer, array);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001463C File Offset: 0x0001283C
		public MetaMesh GetMorphedCopy(string multiMeshName, float morphTarget, bool showErrors)
		{
			byte[] array = null;
			if (multiMeshName != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(multiMeshName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(multiMeshName, 0, multiMeshName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMetaMesh.call_GetMorphedCopyDelegate(array, morphTarget, showErrors);
			MetaMesh metaMesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				metaMesh = new MetaMesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return metaMesh;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000146CC File Offset: 0x000128CC
		public MetaMesh GetMultiMesh(string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMetaMesh.call_GetMultiMeshDelegate(array);
			MetaMesh metaMesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				metaMesh = new MetaMesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return metaMesh;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00014758 File Offset: 0x00012958
		public int GetMultiMeshCount()
		{
			return ScriptingInterfaceOfIMetaMesh.call_GetMultiMeshCountDelegate();
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00014764 File Offset: 0x00012964
		public string GetName(UIntPtr multiMeshPointer)
		{
			if (ScriptingInterfaceOfIMetaMesh.call_GetNameDelegate(multiMeshPointer) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0001477B File Offset: 0x0001297B
		public int GetTotalGpuSize(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_GetTotalGpuSizeDelegate(multiMeshPointer);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00014788 File Offset: 0x00012988
		public Vec3 GetVectorArgument2(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_GetVectorArgument2Delegate(multiMeshPointer);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00014795 File Offset: 0x00012995
		public Vec3 GetVectorUserData(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_GetVectorUserDataDelegate(multiMeshPointer);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000147A2 File Offset: 0x000129A2
		public VisibilityMaskFlags GetVisibilityMask(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_GetVisibilityMaskDelegate(multiMeshPointer);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x000147AF File Offset: 0x000129AF
		public bool HasAnyGeneratedLods(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_HasAnyGeneratedLodsDelegate(multiMeshPointer);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000147BC File Offset: 0x000129BC
		public bool HasAnyLods(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_HasAnyLodsDelegate(multiMeshPointer);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000147C9 File Offset: 0x000129C9
		public bool HasClothData(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_HasClothDataDelegate(multiMeshPointer);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000147D6 File Offset: 0x000129D6
		public bool HasVertexBufferOrEditDataOrPackageItem(UIntPtr multiMeshPointer)
		{
			return ScriptingInterfaceOfIMetaMesh.call_HasVertexBufferOrEditDataOrPackageItemDelegate(multiMeshPointer);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000147E3 File Offset: 0x000129E3
		public void MergeMultiMeshes(UIntPtr multiMeshPointer, UIntPtr multiMeshToMergePointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_MergeMultiMeshesDelegate(multiMeshPointer, multiMeshToMergePointer);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000147F1 File Offset: 0x000129F1
		public void PreloadForRendering(UIntPtr multiMeshPointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_PreloadForRenderingDelegate(multiMeshPointer);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000147FE File Offset: 0x000129FE
		public void PreloadShaders(UIntPtr multiMeshPointer, bool useTableau, bool useTeamColor)
		{
			ScriptingInterfaceOfIMetaMesh.call_PreloadShadersDelegate(multiMeshPointer, useTableau, useTeamColor);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001480D File Offset: 0x00012A0D
		public void RecomputeBoundingBox(UIntPtr multiMeshPointer, bool recomputeMeshes)
		{
			ScriptingInterfaceOfIMetaMesh.call_RecomputeBoundingBoxDelegate(multiMeshPointer, recomputeMeshes);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001481B File Offset: 0x00012A1B
		public void Release(UIntPtr multiMeshPointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_ReleaseDelegate(multiMeshPointer);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00014828 File Offset: 0x00012A28
		public void ReleaseEditDataUser(UIntPtr meshPointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_ReleaseEditDataUserDelegate(meshPointer);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00014838 File Offset: 0x00012A38
		public int RemoveMeshesWithoutTag(UIntPtr multiMeshPointer, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMetaMesh.call_RemoveMeshesWithoutTagDelegate(multiMeshPointer, array);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00014894 File Offset: 0x00012A94
		public int RemoveMeshesWithTag(UIntPtr multiMeshPointer, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMetaMesh.call_RemoveMeshesWithTagDelegate(multiMeshPointer, array);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000148EF File Offset: 0x00012AEF
		public void SetBillboarding(UIntPtr multiMeshPointer, BillboardType billboard)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetBillboardingDelegate(multiMeshPointer, billboard);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000148FD File Offset: 0x00012AFD
		public void SetContourColor(UIntPtr meshPointer, uint color)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetContourColorDelegate(meshPointer, color);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0001490B File Offset: 0x00012B0B
		public void SetContourState(UIntPtr meshPointer, bool alwaysVisible)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetContourStateDelegate(meshPointer, alwaysVisible);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00014919 File Offset: 0x00012B19
		public void SetCullMode(UIntPtr metaMeshPtr, MBMeshCullingMode cullMode)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetCullModeDelegate(metaMeshPtr, cullMode);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00014927 File Offset: 0x00012B27
		public void SetEditDataPolicy(UIntPtr meshPointer, EditDataPolicy policy)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetEditDataPolicyDelegate(meshPointer, policy);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00014935 File Offset: 0x00012B35
		public void SetFactor1(UIntPtr multiMeshPointer, uint factorColor1)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetFactor1Delegate(multiMeshPointer, factorColor1);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00014943 File Offset: 0x00012B43
		public void SetFactor1Linear(UIntPtr multiMeshPointer, uint linearFactorColor1)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetFactor1LinearDelegate(multiMeshPointer, linearFactorColor1);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00014951 File Offset: 0x00012B51
		public void SetFactor2(UIntPtr multiMeshPointer, uint factorColor2)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetFactor2Delegate(multiMeshPointer, factorColor2);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001495F File Offset: 0x00012B5F
		public void SetFactor2Linear(UIntPtr multiMeshPointer, uint linearFactorColor2)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetFactor2LinearDelegate(multiMeshPointer, linearFactorColor2);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00014970 File Offset: 0x00012B70
		public void SetFactorColorToSubMeshesWithTag(UIntPtr meshPointer, uint color, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMetaMesh.call_SetFactorColorToSubMeshesWithTagDelegate(meshPointer, color, array);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000149CC File Offset: 0x00012BCC
		public void SetFrame(UIntPtr multiMeshPointer, ref MatrixFrame meshFrame)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetFrameDelegate(multiMeshPointer, ref meshFrame);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000149DA File Offset: 0x00012BDA
		public void SetGlossMultiplier(UIntPtr multiMeshPointer, float value)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetGlossMultiplierDelegate(multiMeshPointer, value);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000149E8 File Offset: 0x00012BE8
		public void SetLodBias(UIntPtr multiMeshPointer, int lod_bias)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetLodBiasDelegate(multiMeshPointer, lod_bias);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000149F6 File Offset: 0x00012BF6
		public void SetMaterial(UIntPtr multiMeshPointer, UIntPtr materialPointer)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetMaterialDelegate(multiMeshPointer, materialPointer);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00014A04 File Offset: 0x00012C04
		public void SetMaterialToSubMeshesWithTag(UIntPtr meshPointer, UIntPtr materialPointer, string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMetaMesh.call_SetMaterialToSubMeshesWithTagDelegate(meshPointer, materialPointer, array);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00014A60 File Offset: 0x00012C60
		public void SetNumLods(UIntPtr multiMeshPointer, int num_lod)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetNumLodsDelegate(multiMeshPointer, num_lod);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00014A70 File Offset: 0x00012C70
		public void SetShaderToMaterial(UIntPtr multiMeshPointer, string shaderName)
		{
			byte[] array = null;
			if (shaderName != null)
			{
				int byteCount = ScriptingInterfaceOfIMetaMesh._utf8.GetByteCount(shaderName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMetaMesh._utf8.GetBytes(shaderName, 0, shaderName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMetaMesh.call_SetShaderToMaterialDelegate(multiMeshPointer, array);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00014ACB File Offset: 0x00012CCB
		public void SetVectorArgument(UIntPtr multiMeshPointer, float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetVectorArgumentDelegate(multiMeshPointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00014ADE File Offset: 0x00012CDE
		public void SetVectorArgument2(UIntPtr multiMeshPointer, float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetVectorArgument2Delegate(multiMeshPointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00014AF1 File Offset: 0x00012CF1
		public void SetVectorUserData(UIntPtr multiMeshPointer, ref Vec3 vectorArg)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetVectorUserDataDelegate(multiMeshPointer, ref vectorArg);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00014AFF File Offset: 0x00012CFF
		public void SetVisibilityMask(UIntPtr multiMeshPointer, VisibilityMaskFlags visibilityMask)
		{
			ScriptingInterfaceOfIMetaMesh.call_SetVisibilityMaskDelegate(multiMeshPointer, visibilityMask);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00014B0D File Offset: 0x00012D0D
		public void UseHeadBoneFaceGenScaling(UIntPtr multiMeshPointer, UIntPtr skeleton, sbyte headLookDirectionBoneIndex, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIMetaMesh.call_UseHeadBoneFaceGenScalingDelegate(multiMeshPointer, skeleton, headLookDirectionBoneIndex, ref frame);
		}

		// Token: 0x040002CD RID: 717
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002CE RID: 718
		public static ScriptingInterfaceOfIMetaMesh.AddEditDataUserDelegate call_AddEditDataUserDelegate;

		// Token: 0x040002CF RID: 719
		public static ScriptingInterfaceOfIMetaMesh.AddMeshDelegate call_AddMeshDelegate;

		// Token: 0x040002D0 RID: 720
		public static ScriptingInterfaceOfIMetaMesh.AddMetaMeshDelegate call_AddMetaMeshDelegate;

		// Token: 0x040002D1 RID: 721
		public static ScriptingInterfaceOfIMetaMesh.AssignClothBodyFromDelegate call_AssignClothBodyFromDelegate;

		// Token: 0x040002D2 RID: 722
		public static ScriptingInterfaceOfIMetaMesh.BatchMultiMeshesDelegate call_BatchMultiMeshesDelegate;

		// Token: 0x040002D3 RID: 723
		public static ScriptingInterfaceOfIMetaMesh.BatchMultiMeshesMultipleDelegate call_BatchMultiMeshesMultipleDelegate;

		// Token: 0x040002D4 RID: 724
		public static ScriptingInterfaceOfIMetaMesh.CheckMetaMeshExistenceDelegate call_CheckMetaMeshExistenceDelegate;

		// Token: 0x040002D5 RID: 725
		public static ScriptingInterfaceOfIMetaMesh.CheckResourcesDelegate call_CheckResourcesDelegate;

		// Token: 0x040002D6 RID: 726
		public static ScriptingInterfaceOfIMetaMesh.ClearEditDataDelegate call_ClearEditDataDelegate;

		// Token: 0x040002D7 RID: 727
		public static ScriptingInterfaceOfIMetaMesh.ClearMeshesDelegate call_ClearMeshesDelegate;

		// Token: 0x040002D8 RID: 728
		public static ScriptingInterfaceOfIMetaMesh.ClearMeshesForLodDelegate call_ClearMeshesForLodDelegate;

		// Token: 0x040002D9 RID: 729
		public static ScriptingInterfaceOfIMetaMesh.ClearMeshesForLowerLodsDelegate call_ClearMeshesForLowerLodsDelegate;

		// Token: 0x040002DA RID: 730
		public static ScriptingInterfaceOfIMetaMesh.ClearMeshesForOtherLodsDelegate call_ClearMeshesForOtherLodsDelegate;

		// Token: 0x040002DB RID: 731
		public static ScriptingInterfaceOfIMetaMesh.CopyToDelegate call_CopyToDelegate;

		// Token: 0x040002DC RID: 732
		public static ScriptingInterfaceOfIMetaMesh.CreateCopyDelegate call_CreateCopyDelegate;

		// Token: 0x040002DD RID: 733
		public static ScriptingInterfaceOfIMetaMesh.CreateCopyFromNameDelegate call_CreateCopyFromNameDelegate;

		// Token: 0x040002DE RID: 734
		public static ScriptingInterfaceOfIMetaMesh.CreateMetaMeshDelegate call_CreateMetaMeshDelegate;

		// Token: 0x040002DF RID: 735
		public static ScriptingInterfaceOfIMetaMesh.DrawTextWithDefaultFontDelegate call_DrawTextWithDefaultFontDelegate;

		// Token: 0x040002E0 RID: 736
		public static ScriptingInterfaceOfIMetaMesh.GetAllMultiMeshesDelegate call_GetAllMultiMeshesDelegate;

		// Token: 0x040002E1 RID: 737
		public static ScriptingInterfaceOfIMetaMesh.GetBoundingBoxDelegate call_GetBoundingBoxDelegate;

		// Token: 0x040002E2 RID: 738
		public static ScriptingInterfaceOfIMetaMesh.GetFactor1Delegate call_GetFactor1Delegate;

		// Token: 0x040002E3 RID: 739
		public static ScriptingInterfaceOfIMetaMesh.GetFactor2Delegate call_GetFactor2Delegate;

		// Token: 0x040002E4 RID: 740
		public static ScriptingInterfaceOfIMetaMesh.GetFrameDelegate call_GetFrameDelegate;

		// Token: 0x040002E5 RID: 741
		public static ScriptingInterfaceOfIMetaMesh.GetLodMaskForMeshAtIndexDelegate call_GetLodMaskForMeshAtIndexDelegate;

		// Token: 0x040002E6 RID: 742
		public static ScriptingInterfaceOfIMetaMesh.GetMeshAtIndexDelegate call_GetMeshAtIndexDelegate;

		// Token: 0x040002E7 RID: 743
		public static ScriptingInterfaceOfIMetaMesh.GetMeshCountDelegate call_GetMeshCountDelegate;

		// Token: 0x040002E8 RID: 744
		public static ScriptingInterfaceOfIMetaMesh.GetMeshCountWithTagDelegate call_GetMeshCountWithTagDelegate;

		// Token: 0x040002E9 RID: 745
		public static ScriptingInterfaceOfIMetaMesh.GetMorphedCopyDelegate call_GetMorphedCopyDelegate;

		// Token: 0x040002EA RID: 746
		public static ScriptingInterfaceOfIMetaMesh.GetMultiMeshDelegate call_GetMultiMeshDelegate;

		// Token: 0x040002EB RID: 747
		public static ScriptingInterfaceOfIMetaMesh.GetMultiMeshCountDelegate call_GetMultiMeshCountDelegate;

		// Token: 0x040002EC RID: 748
		public static ScriptingInterfaceOfIMetaMesh.GetNameDelegate call_GetNameDelegate;

		// Token: 0x040002ED RID: 749
		public static ScriptingInterfaceOfIMetaMesh.GetTotalGpuSizeDelegate call_GetTotalGpuSizeDelegate;

		// Token: 0x040002EE RID: 750
		public static ScriptingInterfaceOfIMetaMesh.GetVectorArgument2Delegate call_GetVectorArgument2Delegate;

		// Token: 0x040002EF RID: 751
		public static ScriptingInterfaceOfIMetaMesh.GetVectorUserDataDelegate call_GetVectorUserDataDelegate;

		// Token: 0x040002F0 RID: 752
		public static ScriptingInterfaceOfIMetaMesh.GetVisibilityMaskDelegate call_GetVisibilityMaskDelegate;

		// Token: 0x040002F1 RID: 753
		public static ScriptingInterfaceOfIMetaMesh.HasAnyGeneratedLodsDelegate call_HasAnyGeneratedLodsDelegate;

		// Token: 0x040002F2 RID: 754
		public static ScriptingInterfaceOfIMetaMesh.HasAnyLodsDelegate call_HasAnyLodsDelegate;

		// Token: 0x040002F3 RID: 755
		public static ScriptingInterfaceOfIMetaMesh.HasClothDataDelegate call_HasClothDataDelegate;

		// Token: 0x040002F4 RID: 756
		public static ScriptingInterfaceOfIMetaMesh.HasVertexBufferOrEditDataOrPackageItemDelegate call_HasVertexBufferOrEditDataOrPackageItemDelegate;

		// Token: 0x040002F5 RID: 757
		public static ScriptingInterfaceOfIMetaMesh.MergeMultiMeshesDelegate call_MergeMultiMeshesDelegate;

		// Token: 0x040002F6 RID: 758
		public static ScriptingInterfaceOfIMetaMesh.PreloadForRenderingDelegate call_PreloadForRenderingDelegate;

		// Token: 0x040002F7 RID: 759
		public static ScriptingInterfaceOfIMetaMesh.PreloadShadersDelegate call_PreloadShadersDelegate;

		// Token: 0x040002F8 RID: 760
		public static ScriptingInterfaceOfIMetaMesh.RecomputeBoundingBoxDelegate call_RecomputeBoundingBoxDelegate;

		// Token: 0x040002F9 RID: 761
		public static ScriptingInterfaceOfIMetaMesh.ReleaseDelegate call_ReleaseDelegate;

		// Token: 0x040002FA RID: 762
		public static ScriptingInterfaceOfIMetaMesh.ReleaseEditDataUserDelegate call_ReleaseEditDataUserDelegate;

		// Token: 0x040002FB RID: 763
		public static ScriptingInterfaceOfIMetaMesh.RemoveMeshesWithoutTagDelegate call_RemoveMeshesWithoutTagDelegate;

		// Token: 0x040002FC RID: 764
		public static ScriptingInterfaceOfIMetaMesh.RemoveMeshesWithTagDelegate call_RemoveMeshesWithTagDelegate;

		// Token: 0x040002FD RID: 765
		public static ScriptingInterfaceOfIMetaMesh.SetBillboardingDelegate call_SetBillboardingDelegate;

		// Token: 0x040002FE RID: 766
		public static ScriptingInterfaceOfIMetaMesh.SetContourColorDelegate call_SetContourColorDelegate;

		// Token: 0x040002FF RID: 767
		public static ScriptingInterfaceOfIMetaMesh.SetContourStateDelegate call_SetContourStateDelegate;

		// Token: 0x04000300 RID: 768
		public static ScriptingInterfaceOfIMetaMesh.SetCullModeDelegate call_SetCullModeDelegate;

		// Token: 0x04000301 RID: 769
		public static ScriptingInterfaceOfIMetaMesh.SetEditDataPolicyDelegate call_SetEditDataPolicyDelegate;

		// Token: 0x04000302 RID: 770
		public static ScriptingInterfaceOfIMetaMesh.SetFactor1Delegate call_SetFactor1Delegate;

		// Token: 0x04000303 RID: 771
		public static ScriptingInterfaceOfIMetaMesh.SetFactor1LinearDelegate call_SetFactor1LinearDelegate;

		// Token: 0x04000304 RID: 772
		public static ScriptingInterfaceOfIMetaMesh.SetFactor2Delegate call_SetFactor2Delegate;

		// Token: 0x04000305 RID: 773
		public static ScriptingInterfaceOfIMetaMesh.SetFactor2LinearDelegate call_SetFactor2LinearDelegate;

		// Token: 0x04000306 RID: 774
		public static ScriptingInterfaceOfIMetaMesh.SetFactorColorToSubMeshesWithTagDelegate call_SetFactorColorToSubMeshesWithTagDelegate;

		// Token: 0x04000307 RID: 775
		public static ScriptingInterfaceOfIMetaMesh.SetFrameDelegate call_SetFrameDelegate;

		// Token: 0x04000308 RID: 776
		public static ScriptingInterfaceOfIMetaMesh.SetGlossMultiplierDelegate call_SetGlossMultiplierDelegate;

		// Token: 0x04000309 RID: 777
		public static ScriptingInterfaceOfIMetaMesh.SetLodBiasDelegate call_SetLodBiasDelegate;

		// Token: 0x0400030A RID: 778
		public static ScriptingInterfaceOfIMetaMesh.SetMaterialDelegate call_SetMaterialDelegate;

		// Token: 0x0400030B RID: 779
		public static ScriptingInterfaceOfIMetaMesh.SetMaterialToSubMeshesWithTagDelegate call_SetMaterialToSubMeshesWithTagDelegate;

		// Token: 0x0400030C RID: 780
		public static ScriptingInterfaceOfIMetaMesh.SetNumLodsDelegate call_SetNumLodsDelegate;

		// Token: 0x0400030D RID: 781
		public static ScriptingInterfaceOfIMetaMesh.SetShaderToMaterialDelegate call_SetShaderToMaterialDelegate;

		// Token: 0x0400030E RID: 782
		public static ScriptingInterfaceOfIMetaMesh.SetVectorArgumentDelegate call_SetVectorArgumentDelegate;

		// Token: 0x0400030F RID: 783
		public static ScriptingInterfaceOfIMetaMesh.SetVectorArgument2Delegate call_SetVectorArgument2Delegate;

		// Token: 0x04000310 RID: 784
		public static ScriptingInterfaceOfIMetaMesh.SetVectorUserDataDelegate call_SetVectorUserDataDelegate;

		// Token: 0x04000311 RID: 785
		public static ScriptingInterfaceOfIMetaMesh.SetVisibilityMaskDelegate call_SetVisibilityMaskDelegate;

		// Token: 0x04000312 RID: 786
		public static ScriptingInterfaceOfIMetaMesh.UseHeadBoneFaceGenScalingDelegate call_UseHeadBoneFaceGenScalingDelegate;

		// Token: 0x02000341 RID: 833
		// (Invoke) Token: 0x0600131D RID: 4893
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddEditDataUserDelegate(UIntPtr meshPointer);

		// Token: 0x02000342 RID: 834
		// (Invoke) Token: 0x06001321 RID: 4897
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshDelegate(UIntPtr multiMeshPointer, UIntPtr meshPointer, uint lodLevel);

		// Token: 0x02000343 RID: 835
		// (Invoke) Token: 0x06001325 RID: 4901
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMetaMeshDelegate(UIntPtr metaMeshPtr, UIntPtr otherMetaMeshPointer);

		// Token: 0x02000344 RID: 836
		// (Invoke) Token: 0x06001329 RID: 4905
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AssignClothBodyFromDelegate(UIntPtr multiMeshPointer, UIntPtr multiMeshToMergePointer);

		// Token: 0x02000345 RID: 837
		// (Invoke) Token: 0x0600132D RID: 4909
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void BatchMultiMeshesDelegate(UIntPtr multiMeshPointer, UIntPtr multiMeshToMergePointer);

		// Token: 0x02000346 RID: 838
		// (Invoke) Token: 0x06001331 RID: 4913
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void BatchMultiMeshesMultipleDelegate(UIntPtr multiMeshPointer, IntPtr multiMeshToMergePointers, int metaMeshCount);

		// Token: 0x02000347 RID: 839
		// (Invoke) Token: 0x06001335 RID: 4917
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CheckMetaMeshExistenceDelegate(byte[] multiMeshPrefixName, int lod_count_check);

		// Token: 0x02000348 RID: 840
		// (Invoke) Token: 0x06001339 RID: 4921
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int CheckResourcesDelegate(UIntPtr meshPointer);

		// Token: 0x02000349 RID: 841
		// (Invoke) Token: 0x0600133D RID: 4925
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearEditDataDelegate(UIntPtr multiMeshPointer);

		// Token: 0x0200034A RID: 842
		// (Invoke) Token: 0x06001341 RID: 4929
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearMeshesDelegate(UIntPtr multiMeshPointer);

		// Token: 0x0200034B RID: 843
		// (Invoke) Token: 0x06001345 RID: 4933
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearMeshesForLodDelegate(UIntPtr multiMeshPointer, int lodToClear);

		// Token: 0x0200034C RID: 844
		// (Invoke) Token: 0x06001349 RID: 4937
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearMeshesForLowerLodsDelegate(UIntPtr multiMeshPointer, int lod);

		// Token: 0x0200034D RID: 845
		// (Invoke) Token: 0x0600134D RID: 4941
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearMeshesForOtherLodsDelegate(UIntPtr multiMeshPointer, int lodToKeep);

		// Token: 0x0200034E RID: 846
		// (Invoke) Token: 0x06001351 RID: 4945
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CopyToDelegate(UIntPtr metaMesh, UIntPtr targetMesh, [MarshalAs(UnmanagedType.U1)] bool copyMeshes);

		// Token: 0x0200034F RID: 847
		// (Invoke) Token: 0x06001355 RID: 4949
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateCopyDelegate(UIntPtr ptr);

		// Token: 0x02000350 RID: 848
		// (Invoke) Token: 0x06001359 RID: 4953
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateCopyFromNameDelegate(byte[] multiMeshPrefixName, [MarshalAs(UnmanagedType.U1)] bool showErrors, [MarshalAs(UnmanagedType.U1)] bool mayReturnNull);

		// Token: 0x02000351 RID: 849
		// (Invoke) Token: 0x0600135D RID: 4957
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateMetaMeshDelegate(byte[] name);

		// Token: 0x02000352 RID: 850
		// (Invoke) Token: 0x06001361 RID: 4961
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DrawTextWithDefaultFontDelegate(UIntPtr multiMeshPointer, byte[] text, Vec2 textPositionMin, Vec2 textPositionMax, Vec2 size, uint color, TextFlags flags);

		// Token: 0x02000353 RID: 851
		// (Invoke) Token: 0x06001365 RID: 4965
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAllMultiMeshesDelegate(IntPtr gameEntitiesTemp);

		// Token: 0x02000354 RID: 852
		// (Invoke) Token: 0x06001369 RID: 4969
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoundingBoxDelegate(UIntPtr multiMeshPointer, ref BoundingBox outBoundingBox);

		// Token: 0x02000355 RID: 853
		// (Invoke) Token: 0x0600136D RID: 4973
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetFactor1Delegate(UIntPtr multiMeshPointer);

		// Token: 0x02000356 RID: 854
		// (Invoke) Token: 0x06001371 RID: 4977
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetFactor2Delegate(UIntPtr multiMeshPointer);

		// Token: 0x02000357 RID: 855
		// (Invoke) Token: 0x06001375 RID: 4981
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetFrameDelegate(UIntPtr multiMeshPointer, ref MatrixFrame outFrame);

		// Token: 0x02000358 RID: 856
		// (Invoke) Token: 0x06001379 RID: 4985
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetLodMaskForMeshAtIndexDelegate(UIntPtr multiMeshPointer, int meshIndex);

		// Token: 0x02000359 RID: 857
		// (Invoke) Token: 0x0600137D RID: 4989
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetMeshAtIndexDelegate(UIntPtr multiMeshPointer, int meshIndex);

		// Token: 0x0200035A RID: 858
		// (Invoke) Token: 0x06001381 RID: 4993
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMeshCountDelegate(UIntPtr multiMeshPointer);

		// Token: 0x0200035B RID: 859
		// (Invoke) Token: 0x06001385 RID: 4997
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMeshCountWithTagDelegate(UIntPtr multiMeshPointer, byte[] tag);

		// Token: 0x0200035C RID: 860
		// (Invoke) Token: 0x06001389 RID: 5001
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetMorphedCopyDelegate(byte[] multiMeshName, float morphTarget, [MarshalAs(UnmanagedType.U1)] bool showErrors);

		// Token: 0x0200035D RID: 861
		// (Invoke) Token: 0x0600138D RID: 5005
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetMultiMeshDelegate(byte[] name);

		// Token: 0x0200035E RID: 862
		// (Invoke) Token: 0x06001391 RID: 5009
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMultiMeshCountDelegate();

		// Token: 0x0200035F RID: 863
		// (Invoke) Token: 0x06001395 RID: 5013
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr multiMeshPointer);

		// Token: 0x02000360 RID: 864
		// (Invoke) Token: 0x06001399 RID: 5017
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetTotalGpuSizeDelegate(UIntPtr multiMeshPointer);

		// Token: 0x02000361 RID: 865
		// (Invoke) Token: 0x0600139D RID: 5021
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetVectorArgument2Delegate(UIntPtr multiMeshPointer);

		// Token: 0x02000362 RID: 866
		// (Invoke) Token: 0x060013A1 RID: 5025
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetVectorUserDataDelegate(UIntPtr multiMeshPointer);

		// Token: 0x02000363 RID: 867
		// (Invoke) Token: 0x060013A5 RID: 5029
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate VisibilityMaskFlags GetVisibilityMaskDelegate(UIntPtr multiMeshPointer);

		// Token: 0x02000364 RID: 868
		// (Invoke) Token: 0x060013A9 RID: 5033
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasAnyGeneratedLodsDelegate(UIntPtr multiMeshPointer);

		// Token: 0x02000365 RID: 869
		// (Invoke) Token: 0x060013AD RID: 5037
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasAnyLodsDelegate(UIntPtr multiMeshPointer);

		// Token: 0x02000366 RID: 870
		// (Invoke) Token: 0x060013B1 RID: 5041
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasClothDataDelegate(UIntPtr multiMeshPointer);

		// Token: 0x02000367 RID: 871
		// (Invoke) Token: 0x060013B5 RID: 5045
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasVertexBufferOrEditDataOrPackageItemDelegate(UIntPtr multiMeshPointer);

		// Token: 0x02000368 RID: 872
		// (Invoke) Token: 0x060013B9 RID: 5049
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void MergeMultiMeshesDelegate(UIntPtr multiMeshPointer, UIntPtr multiMeshToMergePointer);

		// Token: 0x02000369 RID: 873
		// (Invoke) Token: 0x060013BD RID: 5053
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PreloadForRenderingDelegate(UIntPtr multiMeshPointer);

		// Token: 0x0200036A RID: 874
		// (Invoke) Token: 0x060013C1 RID: 5057
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PreloadShadersDelegate(UIntPtr multiMeshPointer, [MarshalAs(UnmanagedType.U1)] bool useTableau, [MarshalAs(UnmanagedType.U1)] bool useTeamColor);

		// Token: 0x0200036B RID: 875
		// (Invoke) Token: 0x060013C5 RID: 5061
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RecomputeBoundingBoxDelegate(UIntPtr multiMeshPointer, [MarshalAs(UnmanagedType.U1)] bool recomputeMeshes);

		// Token: 0x0200036C RID: 876
		// (Invoke) Token: 0x060013C9 RID: 5065
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseDelegate(UIntPtr multiMeshPointer);

		// Token: 0x0200036D RID: 877
		// (Invoke) Token: 0x060013CD RID: 5069
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseEditDataUserDelegate(UIntPtr meshPointer);

		// Token: 0x0200036E RID: 878
		// (Invoke) Token: 0x060013D1 RID: 5073
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int RemoveMeshesWithoutTagDelegate(UIntPtr multiMeshPointer, byte[] tag);

		// Token: 0x0200036F RID: 879
		// (Invoke) Token: 0x060013D5 RID: 5077
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int RemoveMeshesWithTagDelegate(UIntPtr multiMeshPointer, byte[] tag);

		// Token: 0x02000370 RID: 880
		// (Invoke) Token: 0x060013D9 RID: 5081
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBillboardingDelegate(UIntPtr multiMeshPointer, BillboardType billboard);

		// Token: 0x02000371 RID: 881
		// (Invoke) Token: 0x060013DD RID: 5085
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetContourColorDelegate(UIntPtr meshPointer, uint color);

		// Token: 0x02000372 RID: 882
		// (Invoke) Token: 0x060013E1 RID: 5089
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetContourStateDelegate(UIntPtr meshPointer, [MarshalAs(UnmanagedType.U1)] bool alwaysVisible);

		// Token: 0x02000373 RID: 883
		// (Invoke) Token: 0x060013E5 RID: 5093
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCullModeDelegate(UIntPtr metaMeshPtr, MBMeshCullingMode cullMode);

		// Token: 0x02000374 RID: 884
		// (Invoke) Token: 0x060013E9 RID: 5097
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEditDataPolicyDelegate(UIntPtr meshPointer, EditDataPolicy policy);

		// Token: 0x02000375 RID: 885
		// (Invoke) Token: 0x060013ED RID: 5101
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFactor1Delegate(UIntPtr multiMeshPointer, uint factorColor1);

		// Token: 0x02000376 RID: 886
		// (Invoke) Token: 0x060013F1 RID: 5105
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFactor1LinearDelegate(UIntPtr multiMeshPointer, uint linearFactorColor1);

		// Token: 0x02000377 RID: 887
		// (Invoke) Token: 0x060013F5 RID: 5109
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFactor2Delegate(UIntPtr multiMeshPointer, uint factorColor2);

		// Token: 0x02000378 RID: 888
		// (Invoke) Token: 0x060013F9 RID: 5113
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFactor2LinearDelegate(UIntPtr multiMeshPointer, uint linearFactorColor2);

		// Token: 0x02000379 RID: 889
		// (Invoke) Token: 0x060013FD RID: 5117
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFactorColorToSubMeshesWithTagDelegate(UIntPtr meshPointer, uint color, byte[] tag);

		// Token: 0x0200037A RID: 890
		// (Invoke) Token: 0x06001401 RID: 5121
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFrameDelegate(UIntPtr multiMeshPointer, ref MatrixFrame meshFrame);

		// Token: 0x0200037B RID: 891
		// (Invoke) Token: 0x06001405 RID: 5125
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetGlossMultiplierDelegate(UIntPtr multiMeshPointer, float value);

		// Token: 0x0200037C RID: 892
		// (Invoke) Token: 0x06001409 RID: 5129
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLodBiasDelegate(UIntPtr multiMeshPointer, int lod_bias);

		// Token: 0x0200037D RID: 893
		// (Invoke) Token: 0x0600140D RID: 5133
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMaterialDelegate(UIntPtr multiMeshPointer, UIntPtr materialPointer);

		// Token: 0x0200037E RID: 894
		// (Invoke) Token: 0x06001411 RID: 5137
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMaterialToSubMeshesWithTagDelegate(UIntPtr meshPointer, UIntPtr materialPointer, byte[] tag);

		// Token: 0x0200037F RID: 895
		// (Invoke) Token: 0x06001415 RID: 5141
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetNumLodsDelegate(UIntPtr multiMeshPointer, int num_lod);

		// Token: 0x02000380 RID: 896
		// (Invoke) Token: 0x06001419 RID: 5145
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetShaderToMaterialDelegate(UIntPtr multiMeshPointer, byte[] shaderName);

		// Token: 0x02000381 RID: 897
		// (Invoke) Token: 0x0600141D RID: 5149
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVectorArgumentDelegate(UIntPtr multiMeshPointer, float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3);

		// Token: 0x02000382 RID: 898
		// (Invoke) Token: 0x06001421 RID: 5153
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVectorArgument2Delegate(UIntPtr multiMeshPointer, float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3);

		// Token: 0x02000383 RID: 899
		// (Invoke) Token: 0x06001425 RID: 5157
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVectorUserDataDelegate(UIntPtr multiMeshPointer, ref Vec3 vectorArg);

		// Token: 0x02000384 RID: 900
		// (Invoke) Token: 0x06001429 RID: 5161
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVisibilityMaskDelegate(UIntPtr multiMeshPointer, VisibilityMaskFlags visibilityMask);

		// Token: 0x02000385 RID: 901
		// (Invoke) Token: 0x0600142D RID: 5165
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UseHeadBoneFaceGenScalingDelegate(UIntPtr multiMeshPointer, UIntPtr skeleton, sbyte headLookDirectionBoneIndex, ref MatrixFrame frame);
	}
}
