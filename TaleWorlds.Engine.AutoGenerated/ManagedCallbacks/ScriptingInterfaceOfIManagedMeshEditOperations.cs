using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000017 RID: 23
	internal class ScriptingInterfaceOfIManagedMeshEditOperations : IManagedMeshEditOperations
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x000130A5 File Offset: 0x000112A5
		public int AddFace(UIntPtr Pointer, int patchNode0, int patchNode1, int patchNode2)
		{
			return ScriptingInterfaceOfIManagedMeshEditOperations.call_AddFaceDelegate(Pointer, patchNode0, patchNode1, patchNode2);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000130B6 File Offset: 0x000112B6
		public int AddFaceCorner1(UIntPtr Pointer, int vertexIndex, ref Vec2 uv0, ref Vec3 color, ref Vec3 normal)
		{
			return ScriptingInterfaceOfIManagedMeshEditOperations.call_AddFaceCorner1Delegate(Pointer, vertexIndex, ref uv0, ref color, ref normal);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000130C9 File Offset: 0x000112C9
		public int AddFaceCorner2(UIntPtr Pointer, int vertexIndex, ref Vec2 uv0, ref Vec2 uv1, ref Vec3 color, ref Vec3 normal)
		{
			return ScriptingInterfaceOfIManagedMeshEditOperations.call_AddFaceCorner2Delegate(Pointer, vertexIndex, ref uv0, ref uv1, ref color, ref normal);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000130DE File Offset: 0x000112DE
		public void AddLine(UIntPtr Pointer, ref Vec3 start, ref Vec3 end, ref Vec3 color, float lineWidth)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddLineDelegate(Pointer, ref start, ref end, ref color, lineWidth);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000130F1 File Offset: 0x000112F1
		public void AddMesh(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddMeshDelegate(Pointer, meshPointer, ref frame);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00013100 File Offset: 0x00011300
		public void AddMeshAux(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame, sbyte boneIndex, ref Vec3 color, bool transformNormal, bool heightGradient, bool addSkinData, bool useDoublePrecision)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddMeshAuxDelegate(Pointer, meshPointer, ref frame, boneIndex, ref color, transformNormal, heightGradient, addSkinData, useDoublePrecision);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00013126 File Offset: 0x00011326
		public void AddMeshToBone(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame, sbyte boneIndex)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddMeshToBoneDelegate(Pointer, meshPointer, ref frame, boneIndex);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00013137 File Offset: 0x00011337
		public void AddMeshWithColor(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame, ref Vec3 vertexColor, bool useDoublePrecision)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddMeshWithColorDelegate(Pointer, meshPointer, ref frame, ref vertexColor, useDoublePrecision);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0001314A File Offset: 0x0001134A
		public void AddMeshWithFixedNormals(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddMeshWithFixedNormalsDelegate(Pointer, meshPointer, ref frame);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00013159 File Offset: 0x00011359
		public void AddMeshWithFixedNormalsWithHeightGradientColor(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddMeshWithFixedNormalsWithHeightGradientColorDelegate(Pointer, meshPointer, ref frame);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00013168 File Offset: 0x00011368
		public void AddMeshWithSkinData(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame, sbyte boneIndex)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddMeshWithSkinDataDelegate(Pointer, meshPointer, ref frame, boneIndex);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00013179 File Offset: 0x00011379
		public void AddRect(UIntPtr Pointer, ref Vec3 originBegin, ref Vec3 originEnd, ref Vec2 uvBegin, ref Vec2 uvEnd, ref Vec3 color)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddRectDelegate(Pointer, ref originBegin, ref originEnd, ref uvBegin, ref uvEnd, ref color);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0001318E File Offset: 0x0001138E
		public void AddRectangle3(UIntPtr Pointer, ref Vec3 o, ref Vec2 size, ref Vec2 uv_origin, ref Vec2 uvSize, ref Vec3 color)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddRectangle3Delegate(Pointer, ref o, ref size, ref uv_origin, ref uvSize, ref color);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000131A3 File Offset: 0x000113A3
		public void AddRectangleWithInverseUV(UIntPtr Pointer, ref Vec3 o, ref Vec2 size, ref Vec2 uv_origin, ref Vec2 uvSize, ref Vec3 color)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddRectangleWithInverseUVDelegate(Pointer, ref o, ref size, ref uv_origin, ref uvSize, ref color);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000131B8 File Offset: 0x000113B8
		public void AddRectWithZUp(UIntPtr Pointer, ref Vec3 originBegin, ref Vec3 originEnd, ref Vec2 uvBegin, ref Vec2 uvEnd, ref Vec3 color)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddRectWithZUpDelegate(Pointer, ref originBegin, ref originEnd, ref uvBegin, ref uvEnd, ref color);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000131CD File Offset: 0x000113CD
		public void AddSkinnedMeshWithColor(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame, ref Vec3 vertexColor, bool useDoublePrecision)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddSkinnedMeshWithColorDelegate(Pointer, meshPointer, ref frame, ref vertexColor, useDoublePrecision);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000131E0 File Offset: 0x000113E0
		public void AddTriangle1(UIntPtr Pointer, ref Vec3 p1, ref Vec3 p2, ref Vec3 p3, ref Vec2 uv1, ref Vec2 uv2, ref Vec2 uv3, ref Vec3 color)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddTriangle1Delegate(Pointer, ref p1, ref p2, ref p3, ref uv1, ref uv2, ref uv3, ref color);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00013204 File Offset: 0x00011404
		public void AddTriangle2(UIntPtr Pointer, ref Vec3 p1, ref Vec3 p2, ref Vec3 p3, ref Vec3 n1, ref Vec3 n2, ref Vec3 n3, ref Vec2 uv1, ref Vec2 uv2, ref Vec2 uv3, ref Vec3 c1, ref Vec3 c2, ref Vec3 c3)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_AddTriangle2Delegate(Pointer, ref p1, ref p2, ref p3, ref n1, ref n2, ref n3, ref uv1, ref uv2, ref uv3, ref c1, ref c2, ref c3);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00013232 File Offset: 0x00011432
		public int AddVertex(UIntPtr Pointer, ref Vec3 vertexPos)
		{
			return ScriptingInterfaceOfIManagedMeshEditOperations.call_AddVertexDelegate(Pointer, ref vertexPos);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00013240 File Offset: 0x00011440
		public void ApplyCPUSkinning(UIntPtr Pointer, UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_ApplyCPUSkinningDelegate(Pointer, skeletonPointer);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0001324E File Offset: 0x0001144E
		public void ClearAll(UIntPtr Pointer)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_ClearAllDelegate(Pointer);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0001325B File Offset: 0x0001145B
		public void ComputeCornerNormals(UIntPtr Pointer, bool checkFixedNormals, bool smoothCornerNormals)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_ComputeCornerNormalsDelegate(Pointer, checkFixedNormals, smoothCornerNormals);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0001326A File Offset: 0x0001146A
		public void ComputeCornerNormalsWithSmoothingData(UIntPtr Pointer)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_ComputeCornerNormalsWithSmoothingDataDelegate(Pointer);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00013277 File Offset: 0x00011477
		public int ComputeTangents(UIntPtr Pointer, bool checkFixedNormals)
		{
			return ScriptingInterfaceOfIManagedMeshEditOperations.call_ComputeTangentsDelegate(Pointer, checkFixedNormals);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00013288 File Offset: 0x00011488
		public ManagedMeshEditOperations Create(UIntPtr meshPointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIManagedMeshEditOperations.call_CreateDelegate(meshPointer);
			ManagedMeshEditOperations managedMeshEditOperations = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				managedMeshEditOperations = new ManagedMeshEditOperations(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return managedMeshEditOperations;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000132D2 File Offset: 0x000114D2
		public void EnsureTransformedVertices(UIntPtr Pointer)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_EnsureTransformedVerticesDelegate(Pointer);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000132DF File Offset: 0x000114DF
		public void FinalizeEditing(UIntPtr Pointer)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_FinalizeEditingDelegate(Pointer);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x000132EC File Offset: 0x000114EC
		public void GenerateGrid(UIntPtr Pointer, ref Vec2i numEdges, ref Vec2 edgeScale)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_GenerateGridDelegate(Pointer, ref numEdges, ref edgeScale);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x000132FB File Offset: 0x000114FB
		public Vec3 GetPositionOfVertex(UIntPtr Pointer, int vertexIndex)
		{
			return ScriptingInterfaceOfIManagedMeshEditOperations.call_GetPositionOfVertexDelegate(Pointer, vertexIndex);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00013309 File Offset: 0x00011509
		public Vec3 GetVertexColor(UIntPtr Pointer, int faceCornerIndex)
		{
			return ScriptingInterfaceOfIManagedMeshEditOperations.call_GetVertexColorDelegate(Pointer, faceCornerIndex);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00013317 File Offset: 0x00011517
		public float GetVertexColorAlpha(UIntPtr Pointer)
		{
			return ScriptingInterfaceOfIManagedMeshEditOperations.call_GetVertexColorAlphaDelegate(Pointer);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00013324 File Offset: 0x00011524
		public void InvertFacesWindingOrder(UIntPtr Pointer)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_InvertFacesWindingOrderDelegate(Pointer);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00013331 File Offset: 0x00011531
		public void MoveVerticesAlongNormal(UIntPtr Pointer, float moveAmount)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_MoveVerticesAlongNormalDelegate(Pointer, moveAmount);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0001333F File Offset: 0x0001153F
		public int RemoveDuplicatedCorners(UIntPtr Pointer)
		{
			return ScriptingInterfaceOfIManagedMeshEditOperations.call_RemoveDuplicatedCornersDelegate(Pointer);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0001334C File Offset: 0x0001154C
		public void RemoveFace(UIntPtr Pointer, int faceIndex)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_RemoveFaceDelegate(Pointer, faceIndex);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0001335A File Offset: 0x0001155A
		public void RescaleMesh2d(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_RescaleMesh2dDelegate(Pointer, ref scaleSizeMin, ref scaleSizeMax);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00013369 File Offset: 0x00011569
		public void RescaleMesh2dRepeatX(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax, float frameThickness, int frameSide)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_RescaleMesh2dRepeatXDelegate(Pointer, ref scaleSizeMin, ref scaleSizeMax, frameThickness, frameSide);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001337C File Offset: 0x0001157C
		public void RescaleMesh2dRepeatXWithTiling(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax, float frameThickness, int frameSide, float xyRatio)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_RescaleMesh2dRepeatXWithTilingDelegate(Pointer, ref scaleSizeMin, ref scaleSizeMax, frameThickness, frameSide, xyRatio);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00013391 File Offset: 0x00011591
		public void RescaleMesh2dRepeatY(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax, float frameThickness, int frameSide)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_RescaleMesh2dRepeatYDelegate(Pointer, ref scaleSizeMin, ref scaleSizeMax, frameThickness, frameSide);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x000133A4 File Offset: 0x000115A4
		public void RescaleMesh2dRepeatYWithTiling(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax, float frameThickness, int frameSide, float xyRatio)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_RescaleMesh2dRepeatYWithTilingDelegate(Pointer, ref scaleSizeMin, ref scaleSizeMax, frameThickness, frameSide, xyRatio);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000133B9 File Offset: 0x000115B9
		public void RescaleMesh2dWithoutChangingUV(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax, float remaining)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_RescaleMesh2dWithoutChangingUVDelegate(Pointer, ref scaleSizeMin, ref scaleSizeMax, remaining);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x000133CA File Offset: 0x000115CA
		public void ReserveFaceCorners(UIntPtr Pointer, int count)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_ReserveFaceCornersDelegate(Pointer, count);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000133D8 File Offset: 0x000115D8
		public void ReserveFaces(UIntPtr Pointer, int count)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_ReserveFacesDelegate(Pointer, count);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x000133E6 File Offset: 0x000115E6
		public void ReserveVertices(UIntPtr Pointer, int count)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_ReserveVerticesDelegate(Pointer, count);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x000133F4 File Offset: 0x000115F4
		public void ScaleVertices1(UIntPtr Pointer, float newScale)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_ScaleVertices1Delegate(Pointer, newScale);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00013402 File Offset: 0x00011602
		public void ScaleVertices2(UIntPtr Pointer, ref Vec3 newScale, bool keepUvX, float maxUvSize)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_ScaleVertices2Delegate(Pointer, ref newScale, keepUvX, maxUvSize);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00013413 File Offset: 0x00011613
		public void SetCornerUV(UIntPtr Pointer, int cornerNo, ref Vec2 newUV, int uvNumber)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_SetCornerUVDelegate(Pointer, cornerNo, ref newUV, uvNumber);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00013424 File Offset: 0x00011624
		public void SetCornerVertexColor(UIntPtr Pointer, int cornerNo, ref Vec3 vertexColor)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_SetCornerVertexColorDelegate(Pointer, cornerNo, ref vertexColor);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00013433 File Offset: 0x00011633
		public void SetPositionOfVertex(UIntPtr Pointer, int vertexIndex, ref Vec3 position)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_SetPositionOfVertexDelegate(Pointer, vertexIndex, ref position);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00013442 File Offset: 0x00011642
		public void SetTangentsOfFaceCorner(UIntPtr Pointer, int faceCornerIndex, ref Vec3 tangent, ref Vec3 binormal)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_SetTangentsOfFaceCornerDelegate(Pointer, faceCornerIndex, ref tangent, ref binormal);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00013453 File Offset: 0x00011653
		public void SetVertexColor(UIntPtr Pointer, ref Vec3 color)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_SetVertexColorDelegate(Pointer, ref color);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00013461 File Offset: 0x00011661
		public void SetVertexColorAlpha(UIntPtr Pointer, float newAlpha)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_SetVertexColorAlphaDelegate(Pointer, newAlpha);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0001346F File Offset: 0x0001166F
		public void TransformVerticesToLocal(UIntPtr Pointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_TransformVerticesToLocalDelegate(Pointer, ref frame);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0001347D File Offset: 0x0001167D
		public void TransformVerticesToParent(UIntPtr Pointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_TransformVerticesToParentDelegate(Pointer, ref frame);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001348B File Offset: 0x0001168B
		public void TranslateVertices(UIntPtr Pointer, ref Vec3 newOrigin)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_TranslateVerticesDelegate(Pointer, ref newOrigin);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00013499 File Offset: 0x00011699
		public void UpdateOverlappedVertexNormals(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame attachFrame, float mergeRadiusSQ)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_UpdateOverlappedVertexNormalsDelegate(Pointer, meshPointer, ref attachFrame, mergeRadiusSQ);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000134AA File Offset: 0x000116AA
		public void Weld(UIntPtr Pointer)
		{
			ScriptingInterfaceOfIManagedMeshEditOperations.call_WeldDelegate(Pointer);
		}

		// Token: 0x0400022E RID: 558
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400022F RID: 559
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddFaceDelegate call_AddFaceDelegate;

		// Token: 0x04000230 RID: 560
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddFaceCorner1Delegate call_AddFaceCorner1Delegate;

		// Token: 0x04000231 RID: 561
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddFaceCorner2Delegate call_AddFaceCorner2Delegate;

		// Token: 0x04000232 RID: 562
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddLineDelegate call_AddLineDelegate;

		// Token: 0x04000233 RID: 563
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddMeshDelegate call_AddMeshDelegate;

		// Token: 0x04000234 RID: 564
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddMeshAuxDelegate call_AddMeshAuxDelegate;

		// Token: 0x04000235 RID: 565
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddMeshToBoneDelegate call_AddMeshToBoneDelegate;

		// Token: 0x04000236 RID: 566
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddMeshWithColorDelegate call_AddMeshWithColorDelegate;

		// Token: 0x04000237 RID: 567
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddMeshWithFixedNormalsDelegate call_AddMeshWithFixedNormalsDelegate;

		// Token: 0x04000238 RID: 568
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddMeshWithFixedNormalsWithHeightGradientColorDelegate call_AddMeshWithFixedNormalsWithHeightGradientColorDelegate;

		// Token: 0x04000239 RID: 569
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddMeshWithSkinDataDelegate call_AddMeshWithSkinDataDelegate;

		// Token: 0x0400023A RID: 570
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddRectDelegate call_AddRectDelegate;

		// Token: 0x0400023B RID: 571
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddRectangle3Delegate call_AddRectangle3Delegate;

		// Token: 0x0400023C RID: 572
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddRectangleWithInverseUVDelegate call_AddRectangleWithInverseUVDelegate;

		// Token: 0x0400023D RID: 573
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddRectWithZUpDelegate call_AddRectWithZUpDelegate;

		// Token: 0x0400023E RID: 574
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddSkinnedMeshWithColorDelegate call_AddSkinnedMeshWithColorDelegate;

		// Token: 0x0400023F RID: 575
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddTriangle1Delegate call_AddTriangle1Delegate;

		// Token: 0x04000240 RID: 576
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddTriangle2Delegate call_AddTriangle2Delegate;

		// Token: 0x04000241 RID: 577
		public static ScriptingInterfaceOfIManagedMeshEditOperations.AddVertexDelegate call_AddVertexDelegate;

		// Token: 0x04000242 RID: 578
		public static ScriptingInterfaceOfIManagedMeshEditOperations.ApplyCPUSkinningDelegate call_ApplyCPUSkinningDelegate;

		// Token: 0x04000243 RID: 579
		public static ScriptingInterfaceOfIManagedMeshEditOperations.ClearAllDelegate call_ClearAllDelegate;

		// Token: 0x04000244 RID: 580
		public static ScriptingInterfaceOfIManagedMeshEditOperations.ComputeCornerNormalsDelegate call_ComputeCornerNormalsDelegate;

		// Token: 0x04000245 RID: 581
		public static ScriptingInterfaceOfIManagedMeshEditOperations.ComputeCornerNormalsWithSmoothingDataDelegate call_ComputeCornerNormalsWithSmoothingDataDelegate;

		// Token: 0x04000246 RID: 582
		public static ScriptingInterfaceOfIManagedMeshEditOperations.ComputeTangentsDelegate call_ComputeTangentsDelegate;

		// Token: 0x04000247 RID: 583
		public static ScriptingInterfaceOfIManagedMeshEditOperations.CreateDelegate call_CreateDelegate;

		// Token: 0x04000248 RID: 584
		public static ScriptingInterfaceOfIManagedMeshEditOperations.EnsureTransformedVerticesDelegate call_EnsureTransformedVerticesDelegate;

		// Token: 0x04000249 RID: 585
		public static ScriptingInterfaceOfIManagedMeshEditOperations.FinalizeEditingDelegate call_FinalizeEditingDelegate;

		// Token: 0x0400024A RID: 586
		public static ScriptingInterfaceOfIManagedMeshEditOperations.GenerateGridDelegate call_GenerateGridDelegate;

		// Token: 0x0400024B RID: 587
		public static ScriptingInterfaceOfIManagedMeshEditOperations.GetPositionOfVertexDelegate call_GetPositionOfVertexDelegate;

		// Token: 0x0400024C RID: 588
		public static ScriptingInterfaceOfIManagedMeshEditOperations.GetVertexColorDelegate call_GetVertexColorDelegate;

		// Token: 0x0400024D RID: 589
		public static ScriptingInterfaceOfIManagedMeshEditOperations.GetVertexColorAlphaDelegate call_GetVertexColorAlphaDelegate;

		// Token: 0x0400024E RID: 590
		public static ScriptingInterfaceOfIManagedMeshEditOperations.InvertFacesWindingOrderDelegate call_InvertFacesWindingOrderDelegate;

		// Token: 0x0400024F RID: 591
		public static ScriptingInterfaceOfIManagedMeshEditOperations.MoveVerticesAlongNormalDelegate call_MoveVerticesAlongNormalDelegate;

		// Token: 0x04000250 RID: 592
		public static ScriptingInterfaceOfIManagedMeshEditOperations.RemoveDuplicatedCornersDelegate call_RemoveDuplicatedCornersDelegate;

		// Token: 0x04000251 RID: 593
		public static ScriptingInterfaceOfIManagedMeshEditOperations.RemoveFaceDelegate call_RemoveFaceDelegate;

		// Token: 0x04000252 RID: 594
		public static ScriptingInterfaceOfIManagedMeshEditOperations.RescaleMesh2dDelegate call_RescaleMesh2dDelegate;

		// Token: 0x04000253 RID: 595
		public static ScriptingInterfaceOfIManagedMeshEditOperations.RescaleMesh2dRepeatXDelegate call_RescaleMesh2dRepeatXDelegate;

		// Token: 0x04000254 RID: 596
		public static ScriptingInterfaceOfIManagedMeshEditOperations.RescaleMesh2dRepeatXWithTilingDelegate call_RescaleMesh2dRepeatXWithTilingDelegate;

		// Token: 0x04000255 RID: 597
		public static ScriptingInterfaceOfIManagedMeshEditOperations.RescaleMesh2dRepeatYDelegate call_RescaleMesh2dRepeatYDelegate;

		// Token: 0x04000256 RID: 598
		public static ScriptingInterfaceOfIManagedMeshEditOperations.RescaleMesh2dRepeatYWithTilingDelegate call_RescaleMesh2dRepeatYWithTilingDelegate;

		// Token: 0x04000257 RID: 599
		public static ScriptingInterfaceOfIManagedMeshEditOperations.RescaleMesh2dWithoutChangingUVDelegate call_RescaleMesh2dWithoutChangingUVDelegate;

		// Token: 0x04000258 RID: 600
		public static ScriptingInterfaceOfIManagedMeshEditOperations.ReserveFaceCornersDelegate call_ReserveFaceCornersDelegate;

		// Token: 0x04000259 RID: 601
		public static ScriptingInterfaceOfIManagedMeshEditOperations.ReserveFacesDelegate call_ReserveFacesDelegate;

		// Token: 0x0400025A RID: 602
		public static ScriptingInterfaceOfIManagedMeshEditOperations.ReserveVerticesDelegate call_ReserveVerticesDelegate;

		// Token: 0x0400025B RID: 603
		public static ScriptingInterfaceOfIManagedMeshEditOperations.ScaleVertices1Delegate call_ScaleVertices1Delegate;

		// Token: 0x0400025C RID: 604
		public static ScriptingInterfaceOfIManagedMeshEditOperations.ScaleVertices2Delegate call_ScaleVertices2Delegate;

		// Token: 0x0400025D RID: 605
		public static ScriptingInterfaceOfIManagedMeshEditOperations.SetCornerUVDelegate call_SetCornerUVDelegate;

		// Token: 0x0400025E RID: 606
		public static ScriptingInterfaceOfIManagedMeshEditOperations.SetCornerVertexColorDelegate call_SetCornerVertexColorDelegate;

		// Token: 0x0400025F RID: 607
		public static ScriptingInterfaceOfIManagedMeshEditOperations.SetPositionOfVertexDelegate call_SetPositionOfVertexDelegate;

		// Token: 0x04000260 RID: 608
		public static ScriptingInterfaceOfIManagedMeshEditOperations.SetTangentsOfFaceCornerDelegate call_SetTangentsOfFaceCornerDelegate;

		// Token: 0x04000261 RID: 609
		public static ScriptingInterfaceOfIManagedMeshEditOperations.SetVertexColorDelegate call_SetVertexColorDelegate;

		// Token: 0x04000262 RID: 610
		public static ScriptingInterfaceOfIManagedMeshEditOperations.SetVertexColorAlphaDelegate call_SetVertexColorAlphaDelegate;

		// Token: 0x04000263 RID: 611
		public static ScriptingInterfaceOfIManagedMeshEditOperations.TransformVerticesToLocalDelegate call_TransformVerticesToLocalDelegate;

		// Token: 0x04000264 RID: 612
		public static ScriptingInterfaceOfIManagedMeshEditOperations.TransformVerticesToParentDelegate call_TransformVerticesToParentDelegate;

		// Token: 0x04000265 RID: 613
		public static ScriptingInterfaceOfIManagedMeshEditOperations.TranslateVerticesDelegate call_TranslateVerticesDelegate;

		// Token: 0x04000266 RID: 614
		public static ScriptingInterfaceOfIManagedMeshEditOperations.UpdateOverlappedVertexNormalsDelegate call_UpdateOverlappedVertexNormalsDelegate;

		// Token: 0x04000267 RID: 615
		public static ScriptingInterfaceOfIManagedMeshEditOperations.WeldDelegate call_WeldDelegate;

		// Token: 0x020002A6 RID: 678
		// (Invoke) Token: 0x060010B1 RID: 4273
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AddFaceDelegate(UIntPtr Pointer, int patchNode0, int patchNode1, int patchNode2);

		// Token: 0x020002A7 RID: 679
		// (Invoke) Token: 0x060010B5 RID: 4277
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AddFaceCorner1Delegate(UIntPtr Pointer, int vertexIndex, ref Vec2 uv0, ref Vec3 color, ref Vec3 normal);

		// Token: 0x020002A8 RID: 680
		// (Invoke) Token: 0x060010B9 RID: 4281
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AddFaceCorner2Delegate(UIntPtr Pointer, int vertexIndex, ref Vec2 uv0, ref Vec2 uv1, ref Vec3 color, ref Vec3 normal);

		// Token: 0x020002A9 RID: 681
		// (Invoke) Token: 0x060010BD RID: 4285
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddLineDelegate(UIntPtr Pointer, ref Vec3 start, ref Vec3 end, ref Vec3 color, float lineWidth);

		// Token: 0x020002AA RID: 682
		// (Invoke) Token: 0x060010C1 RID: 4289
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshDelegate(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame);

		// Token: 0x020002AB RID: 683
		// (Invoke) Token: 0x060010C5 RID: 4293
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshAuxDelegate(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame, sbyte boneIndex, ref Vec3 color, [MarshalAs(UnmanagedType.U1)] bool transformNormal, [MarshalAs(UnmanagedType.U1)] bool heightGradient, [MarshalAs(UnmanagedType.U1)] bool addSkinData, [MarshalAs(UnmanagedType.U1)] bool useDoublePrecision);

		// Token: 0x020002AC RID: 684
		// (Invoke) Token: 0x060010C9 RID: 4297
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshToBoneDelegate(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame, sbyte boneIndex);

		// Token: 0x020002AD RID: 685
		// (Invoke) Token: 0x060010CD RID: 4301
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshWithColorDelegate(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame, ref Vec3 vertexColor, [MarshalAs(UnmanagedType.U1)] bool useDoublePrecision);

		// Token: 0x020002AE RID: 686
		// (Invoke) Token: 0x060010D1 RID: 4305
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshWithFixedNormalsDelegate(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame);

		// Token: 0x020002AF RID: 687
		// (Invoke) Token: 0x060010D5 RID: 4309
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshWithFixedNormalsWithHeightGradientColorDelegate(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame);

		// Token: 0x020002B0 RID: 688
		// (Invoke) Token: 0x060010D9 RID: 4313
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshWithSkinDataDelegate(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame, sbyte boneIndex);

		// Token: 0x020002B1 RID: 689
		// (Invoke) Token: 0x060010DD RID: 4317
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddRectDelegate(UIntPtr Pointer, ref Vec3 originBegin, ref Vec3 originEnd, ref Vec2 uvBegin, ref Vec2 uvEnd, ref Vec3 color);

		// Token: 0x020002B2 RID: 690
		// (Invoke) Token: 0x060010E1 RID: 4321
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddRectangle3Delegate(UIntPtr Pointer, ref Vec3 o, ref Vec2 size, ref Vec2 uv_origin, ref Vec2 uvSize, ref Vec3 color);

		// Token: 0x020002B3 RID: 691
		// (Invoke) Token: 0x060010E5 RID: 4325
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddRectangleWithInverseUVDelegate(UIntPtr Pointer, ref Vec3 o, ref Vec2 size, ref Vec2 uv_origin, ref Vec2 uvSize, ref Vec3 color);

		// Token: 0x020002B4 RID: 692
		// (Invoke) Token: 0x060010E9 RID: 4329
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddRectWithZUpDelegate(UIntPtr Pointer, ref Vec3 originBegin, ref Vec3 originEnd, ref Vec2 uvBegin, ref Vec2 uvEnd, ref Vec3 color);

		// Token: 0x020002B5 RID: 693
		// (Invoke) Token: 0x060010ED RID: 4333
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddSkinnedMeshWithColorDelegate(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame frame, ref Vec3 vertexColor, [MarshalAs(UnmanagedType.U1)] bool useDoublePrecision);

		// Token: 0x020002B6 RID: 694
		// (Invoke) Token: 0x060010F1 RID: 4337
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddTriangle1Delegate(UIntPtr Pointer, ref Vec3 p1, ref Vec3 p2, ref Vec3 p3, ref Vec2 uv1, ref Vec2 uv2, ref Vec2 uv3, ref Vec3 color);

		// Token: 0x020002B7 RID: 695
		// (Invoke) Token: 0x060010F5 RID: 4341
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddTriangle2Delegate(UIntPtr Pointer, ref Vec3 p1, ref Vec3 p2, ref Vec3 p3, ref Vec3 n1, ref Vec3 n2, ref Vec3 n3, ref Vec2 uv1, ref Vec2 uv2, ref Vec2 uv3, ref Vec3 c1, ref Vec3 c2, ref Vec3 c3);

		// Token: 0x020002B8 RID: 696
		// (Invoke) Token: 0x060010F9 RID: 4345
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AddVertexDelegate(UIntPtr Pointer, ref Vec3 vertexPos);

		// Token: 0x020002B9 RID: 697
		// (Invoke) Token: 0x060010FD RID: 4349
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyCPUSkinningDelegate(UIntPtr Pointer, UIntPtr skeletonPointer);

		// Token: 0x020002BA RID: 698
		// (Invoke) Token: 0x06001101 RID: 4353
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearAllDelegate(UIntPtr Pointer);

		// Token: 0x020002BB RID: 699
		// (Invoke) Token: 0x06001105 RID: 4357
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ComputeCornerNormalsDelegate(UIntPtr Pointer, [MarshalAs(UnmanagedType.U1)] bool checkFixedNormals, [MarshalAs(UnmanagedType.U1)] bool smoothCornerNormals);

		// Token: 0x020002BC RID: 700
		// (Invoke) Token: 0x06001109 RID: 4361
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ComputeCornerNormalsWithSmoothingDataDelegate(UIntPtr Pointer);

		// Token: 0x020002BD RID: 701
		// (Invoke) Token: 0x0600110D RID: 4365
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int ComputeTangentsDelegate(UIntPtr Pointer, [MarshalAs(UnmanagedType.U1)] bool checkFixedNormals);

		// Token: 0x020002BE RID: 702
		// (Invoke) Token: 0x06001111 RID: 4369
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateDelegate(UIntPtr meshPointer);

		// Token: 0x020002BF RID: 703
		// (Invoke) Token: 0x06001115 RID: 4373
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnsureTransformedVerticesDelegate(UIntPtr Pointer);

		// Token: 0x020002C0 RID: 704
		// (Invoke) Token: 0x06001119 RID: 4377
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FinalizeEditingDelegate(UIntPtr Pointer);

		// Token: 0x020002C1 RID: 705
		// (Invoke) Token: 0x0600111D RID: 4381
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GenerateGridDelegate(UIntPtr Pointer, ref Vec2i numEdges, ref Vec2 edgeScale);

		// Token: 0x020002C2 RID: 706
		// (Invoke) Token: 0x06001121 RID: 4385
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetPositionOfVertexDelegate(UIntPtr Pointer, int vertexIndex);

		// Token: 0x020002C3 RID: 707
		// (Invoke) Token: 0x06001125 RID: 4389
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetVertexColorDelegate(UIntPtr Pointer, int faceCornerIndex);

		// Token: 0x020002C4 RID: 708
		// (Invoke) Token: 0x06001129 RID: 4393
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetVertexColorAlphaDelegate(UIntPtr Pointer);

		// Token: 0x020002C5 RID: 709
		// (Invoke) Token: 0x0600112D RID: 4397
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InvertFacesWindingOrderDelegate(UIntPtr Pointer);

		// Token: 0x020002C6 RID: 710
		// (Invoke) Token: 0x06001131 RID: 4401
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void MoveVerticesAlongNormalDelegate(UIntPtr Pointer, float moveAmount);

		// Token: 0x020002C7 RID: 711
		// (Invoke) Token: 0x06001135 RID: 4405
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int RemoveDuplicatedCornersDelegate(UIntPtr Pointer);

		// Token: 0x020002C8 RID: 712
		// (Invoke) Token: 0x06001139 RID: 4409
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveFaceDelegate(UIntPtr Pointer, int faceIndex);

		// Token: 0x020002C9 RID: 713
		// (Invoke) Token: 0x0600113D RID: 4413
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RescaleMesh2dDelegate(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax);

		// Token: 0x020002CA RID: 714
		// (Invoke) Token: 0x06001141 RID: 4417
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RescaleMesh2dRepeatXDelegate(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax, float frameThickness, int frameSide);

		// Token: 0x020002CB RID: 715
		// (Invoke) Token: 0x06001145 RID: 4421
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RescaleMesh2dRepeatXWithTilingDelegate(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax, float frameThickness, int frameSide, float xyRatio);

		// Token: 0x020002CC RID: 716
		// (Invoke) Token: 0x06001149 RID: 4425
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RescaleMesh2dRepeatYDelegate(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax, float frameThickness, int frameSide);

		// Token: 0x020002CD RID: 717
		// (Invoke) Token: 0x0600114D RID: 4429
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RescaleMesh2dRepeatYWithTilingDelegate(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax, float frameThickness, int frameSide, float xyRatio);

		// Token: 0x020002CE RID: 718
		// (Invoke) Token: 0x06001151 RID: 4433
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RescaleMesh2dWithoutChangingUVDelegate(UIntPtr Pointer, ref Vec2 scaleSizeMin, ref Vec2 scaleSizeMax, float remaining);

		// Token: 0x020002CF RID: 719
		// (Invoke) Token: 0x06001155 RID: 4437
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReserveFaceCornersDelegate(UIntPtr Pointer, int count);

		// Token: 0x020002D0 RID: 720
		// (Invoke) Token: 0x06001159 RID: 4441
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReserveFacesDelegate(UIntPtr Pointer, int count);

		// Token: 0x020002D1 RID: 721
		// (Invoke) Token: 0x0600115D RID: 4445
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReserveVerticesDelegate(UIntPtr Pointer, int count);

		// Token: 0x020002D2 RID: 722
		// (Invoke) Token: 0x06001161 RID: 4449
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ScaleVertices1Delegate(UIntPtr Pointer, float newScale);

		// Token: 0x020002D3 RID: 723
		// (Invoke) Token: 0x06001165 RID: 4453
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ScaleVertices2Delegate(UIntPtr Pointer, ref Vec3 newScale, [MarshalAs(UnmanagedType.U1)] bool keepUvX, float maxUvSize);

		// Token: 0x020002D4 RID: 724
		// (Invoke) Token: 0x06001169 RID: 4457
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCornerUVDelegate(UIntPtr Pointer, int cornerNo, ref Vec2 newUV, int uvNumber);

		// Token: 0x020002D5 RID: 725
		// (Invoke) Token: 0x0600116D RID: 4461
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCornerVertexColorDelegate(UIntPtr Pointer, int cornerNo, ref Vec3 vertexColor);

		// Token: 0x020002D6 RID: 726
		// (Invoke) Token: 0x06001171 RID: 4465
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPositionOfVertexDelegate(UIntPtr Pointer, int vertexIndex, ref Vec3 position);

		// Token: 0x020002D7 RID: 727
		// (Invoke) Token: 0x06001175 RID: 4469
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTangentsOfFaceCornerDelegate(UIntPtr Pointer, int faceCornerIndex, ref Vec3 tangent, ref Vec3 binormal);

		// Token: 0x020002D8 RID: 728
		// (Invoke) Token: 0x06001179 RID: 4473
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVertexColorDelegate(UIntPtr Pointer, ref Vec3 color);

		// Token: 0x020002D9 RID: 729
		// (Invoke) Token: 0x0600117D RID: 4477
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVertexColorAlphaDelegate(UIntPtr Pointer, float newAlpha);

		// Token: 0x020002DA RID: 730
		// (Invoke) Token: 0x06001181 RID: 4481
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TransformVerticesToLocalDelegate(UIntPtr Pointer, ref MatrixFrame frame);

		// Token: 0x020002DB RID: 731
		// (Invoke) Token: 0x06001185 RID: 4485
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TransformVerticesToParentDelegate(UIntPtr Pointer, ref MatrixFrame frame);

		// Token: 0x020002DC RID: 732
		// (Invoke) Token: 0x06001189 RID: 4489
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TranslateVerticesDelegate(UIntPtr Pointer, ref Vec3 newOrigin);

		// Token: 0x020002DD RID: 733
		// (Invoke) Token: 0x0600118D RID: 4493
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateOverlappedVertexNormalsDelegate(UIntPtr Pointer, UIntPtr meshPointer, ref MatrixFrame attachFrame, float mergeRadiusSQ);

		// Token: 0x020002DE RID: 734
		// (Invoke) Token: 0x06001191 RID: 4497
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WeldDelegate(UIntPtr Pointer);
	}
}
