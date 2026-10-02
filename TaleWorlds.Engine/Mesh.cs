using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000068 RID: 104
	[EngineClass("rglMesh")]
	public sealed class Mesh : Resource
	{
		// Token: 0x0600099A RID: 2458 RVA: 0x00009542 File Offset: 0x00007742
		internal Mesh(UIntPtr meshPointer)
			: base(meshPointer)
		{
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0000954B File Offset: 0x0000774B
		public static Mesh CreateMeshWithMaterial(Material material)
		{
			return EngineApplicationInterface.IMesh.CreateMeshWithMaterial(material.Pointer);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0000955D File Offset: 0x0000775D
		public static Mesh CreateMesh(bool editable = true)
		{
			return EngineApplicationInterface.IMesh.CreateMesh(editable);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0000956A File Offset: 0x0000776A
		public Mesh GetBaseMesh()
		{
			return EngineApplicationInterface.IMesh.GetBaseMesh(base.Pointer);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0000957C File Offset: 0x0000777C
		public static Mesh GetFromResource(string meshName)
		{
			return EngineApplicationInterface.IMesh.GetMeshFromResource(meshName);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00009589 File Offset: 0x00007789
		public static Mesh GetRandomMeshWithVdecl(int inputLayout)
		{
			return EngineApplicationInterface.IMesh.GetRandomMeshWithVdecl(inputLayout);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00009596 File Offset: 0x00007796
		public void SetColorAndStroke(uint color, uint strokeColor, bool drawStroke)
		{
			this.Color = color;
			this.Color2 = strokeColor;
			EngineApplicationInterface.IMesh.SetColorAndStroke(base.Pointer, drawStroke);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x000095B7 File Offset: 0x000077B7
		public void SetMeshRenderOrder(int renderOrder)
		{
			EngineApplicationInterface.IMesh.SetMeshRenderOrder(base.Pointer, renderOrder);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x000095CA File Offset: 0x000077CA
		public bool HasTag(string str)
		{
			return EngineApplicationInterface.IMesh.HasTag(base.Pointer, str);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x000095DD File Offset: 0x000077DD
		public Mesh CreateCopy()
		{
			return EngineApplicationInterface.IMesh.CreateMeshCopy(base.Pointer);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000095EF File Offset: 0x000077EF
		public void SetMaterial(string newMaterialName)
		{
			EngineApplicationInterface.IMesh.SetMaterialByName(base.Pointer, newMaterialName);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00009602 File Offset: 0x00007802
		public void SetVectorArgument(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IMesh.SetVectorArgument(base.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00009619 File Offset: 0x00007819
		public void SetVectorArgument2(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IMesh.SetVectorArgument2(base.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00009630 File Offset: 0x00007830
		public Vec3 GetVectorArgument()
		{
			return EngineApplicationInterface.IMesh.GetVectorArgument(base.Pointer);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00009642 File Offset: 0x00007842
		public Vec3 GetVectorArgument2()
		{
			return EngineApplicationInterface.IMesh.GetVectorArgument2(base.Pointer);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00009654 File Offset: 0x00007854
		public void SetupAdditionalBoneBuffer(int numBones)
		{
			EngineApplicationInterface.IMesh.SetupAdditionalBoneBuffer(base.Pointer, numBones);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00009667 File Offset: 0x00007867
		public void SetAdditionalBoneFrame(int boneIndex, in MatrixFrame frame)
		{
			EngineApplicationInterface.IMesh.SetAdditionalBoneFrame(base.Pointer, boneIndex, in frame);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0000967B File Offset: 0x0000787B
		public void SetMaterial(Material material)
		{
			EngineApplicationInterface.IMesh.SetMaterial(base.Pointer, material.Pointer);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00009693 File Offset: 0x00007893
		public Material GetMaterial()
		{
			return EngineApplicationInterface.IMesh.GetMaterial(base.Pointer);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x000096A5 File Offset: 0x000078A5
		public Material GetSecondMaterial()
		{
			return EngineApplicationInterface.IMesh.GetSecondMaterial(base.Pointer);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x000096B7 File Offset: 0x000078B7
		public int AddFaceCorner(Vec3 position, Vec3 normal, Vec2 uvCoord, uint color, UIntPtr lockHandle)
		{
			if (base.IsValid)
			{
				return EngineApplicationInterface.IMesh.AddFaceCorner(base.Pointer, position, normal, uvCoord, color, lockHandle);
			}
			return -1;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x000096DA File Offset: 0x000078DA
		public int AddFace(int patchNode0, int patchNode1, int patchNode2, UIntPtr lockHandle)
		{
			if (base.IsValid)
			{
				return EngineApplicationInterface.IMesh.AddFace(base.Pointer, patchNode0, patchNode1, patchNode2, lockHandle);
			}
			return -1;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x000096FB File Offset: 0x000078FB
		public void ClearMesh()
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.ClearMesh(base.Pointer);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x00009715 File Offset: 0x00007915
		// (set) Token: 0x060009B2 RID: 2482 RVA: 0x00009735 File Offset: 0x00007935
		public string Name
		{
			get
			{
				if (base.IsValid)
				{
					return EngineApplicationInterface.IMesh.GetName(base.Pointer);
				}
				return string.Empty;
			}
			set
			{
				EngineApplicationInterface.IMesh.SetName(base.Pointer, value);
			}
		}

		// Token: 0x17000056 RID: 86
		// (set) Token: 0x060009B3 RID: 2483 RVA: 0x00009748 File Offset: 0x00007948
		public MBMeshCullingMode CullingMode
		{
			set
			{
				if (base.IsValid)
				{
					EngineApplicationInterface.IMesh.SetCullingMode(base.Pointer, (uint)value);
				}
			}
		}

		// Token: 0x17000057 RID: 87
		// (set) Token: 0x060009B4 RID: 2484 RVA: 0x00009763 File Offset: 0x00007963
		public float MorphTime
		{
			set
			{
				if (base.IsValid)
				{
					EngineApplicationInterface.IMesh.SetMorphTime(base.Pointer, value);
				}
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x000097B3 File Offset: 0x000079B3
		// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0000977E File Offset: 0x0000797E
		public uint Color
		{
			get
			{
				return EngineApplicationInterface.IMesh.GetColor(base.Pointer);
			}
			set
			{
				if (base.IsValid)
				{
					EngineApplicationInterface.IMesh.SetColor(base.Pointer, value);
					return;
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\Mesh.cs", "Color", 331);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x000097FA File Offset: 0x000079FA
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x000097C5 File Offset: 0x000079C5
		public uint Color2
		{
			get
			{
				return EngineApplicationInterface.IMesh.GetColor2(base.Pointer);
			}
			set
			{
				if (base.IsValid)
				{
					EngineApplicationInterface.IMesh.SetColor2(base.Pointer, value);
					return;
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\Mesh.cs", "Color2", 354);
			}
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0000980C File Offset: 0x00007A0C
		public void SetColorAlpha(uint newAlpha)
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.SetColorAlpha(base.Pointer, newAlpha);
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00009827 File Offset: 0x00007A27
		public uint GetFaceCount()
		{
			if (!base.IsValid)
			{
				return 0U;
			}
			return EngineApplicationInterface.IMesh.GetFaceCount(base.Pointer);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00009843 File Offset: 0x00007A43
		public uint GetFaceCornerCount()
		{
			if (!base.IsValid)
			{
				return 0U;
			}
			return EngineApplicationInterface.IMesh.GetFaceCornerCount(base.Pointer);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0000985F File Offset: 0x00007A5F
		public void ComputeNormals()
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.ComputeNormals(base.Pointer);
			}
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x00009879 File Offset: 0x00007A79
		public void ComputeTangents()
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.ComputeTangents(base.Pointer);
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x00009894 File Offset: 0x00007A94
		public void AddMesh(string meshResourceName, MatrixFrame meshFrame)
		{
			if (base.IsValid)
			{
				Mesh fromResource = Mesh.GetFromResource(meshResourceName);
				EngineApplicationInterface.IMesh.AddMeshToMesh(base.Pointer, fromResource.Pointer, ref meshFrame);
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x000098C8 File Offset: 0x00007AC8
		public void AddMesh(Mesh mesh, MatrixFrame meshFrame)
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.AddMeshToMesh(base.Pointer, mesh.Pointer, ref meshFrame);
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000098EC File Offset: 0x00007AEC
		public MatrixFrame GetLocalFrame()
		{
			if (base.IsValid)
			{
				MatrixFrame matrixFrame = default(MatrixFrame);
				EngineApplicationInterface.IMesh.GetLocalFrame(base.Pointer, ref matrixFrame);
				return matrixFrame;
			}
			return default(MatrixFrame);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00009926 File Offset: 0x00007B26
		public void SetLocalFrame(MatrixFrame meshFrame)
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.SetLocalFrame(base.Pointer, ref meshFrame);
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00009942 File Offset: 0x00007B42
		public void SetVisibilityMask(VisibilityMaskFlags visibilityMask)
		{
			EngineApplicationInterface.IMesh.SetVisibilityMask(base.Pointer, visibilityMask);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00009955 File Offset: 0x00007B55
		public void UpdateBoundingBox()
		{
			if (base.IsValid)
			{
				EngineApplicationInterface.IMesh.UpdateBoundingBox(base.Pointer);
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0000996F File Offset: 0x00007B6F
		public void SetAsNotEffectedBySeason()
		{
			EngineApplicationInterface.IMesh.SetAsNotEffectedBySeason(base.Pointer);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00009981 File Offset: 0x00007B81
		public float GetBoundingBoxWidth()
		{
			if (!base.IsValid)
			{
				return 0f;
			}
			return EngineApplicationInterface.IMesh.GetBoundingBoxWidth(base.Pointer);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x000099A1 File Offset: 0x00007BA1
		public float GetBoundingBoxHeight()
		{
			if (!base.IsValid)
			{
				return 0f;
			}
			return EngineApplicationInterface.IMesh.GetBoundingBoxHeight(base.Pointer);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000099C1 File Offset: 0x00007BC1
		public Vec3 GetBoundingBoxMin()
		{
			return EngineApplicationInterface.IMesh.GetBoundingBoxMin(base.Pointer);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x000099D3 File Offset: 0x00007BD3
		public Vec3 GetBoundingBoxMax()
		{
			return EngineApplicationInterface.IMesh.GetBoundingBoxMax(base.Pointer);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x000099E8 File Offset: 0x00007BE8
		public void AddTriangle(Vec3 p1, Vec3 p2, Vec3 p3, Vec2 uv1, Vec2 uv2, Vec2 uv3, uint color, UIntPtr lockHandle)
		{
			EngineApplicationInterface.IMesh.AddTriangle(base.Pointer, p1, p2, p3, uv1, uv2, uv3, color, lockHandle);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00009A14 File Offset: 0x00007C14
		public void AddTriangleWithVertexColors(Vec3 p1, Vec3 p2, Vec3 p3, Vec2 uv1, Vec2 uv2, Vec2 uv3, uint c1, uint c2, uint c3, UIntPtr lockHandle)
		{
			EngineApplicationInterface.IMesh.AddTriangleWithVertexColors(base.Pointer, p1, p2, p3, uv1, uv2, uv3, c1, c2, c3, lockHandle);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00009A42 File Offset: 0x00007C42
		public void HintIndicesDynamic()
		{
			EngineApplicationInterface.IMesh.HintIndicesDynamic(base.Pointer);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00009A54 File Offset: 0x00007C54
		public void HintVerticesDynamic()
		{
			EngineApplicationInterface.IMesh.HintVerticesDynamic(base.Pointer);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00009A66 File Offset: 0x00007C66
		public void RecomputeBoundingBox()
		{
			EngineApplicationInterface.IMesh.RecomputeBoundingBox(base.Pointer);
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x00009A78 File Offset: 0x00007C78
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x00009A8A File Offset: 0x00007C8A
		public BillboardType Billboard
		{
			get
			{
				return EngineApplicationInterface.IMesh.GetBillboard(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IMesh.SetBillboard(base.Pointer, value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00009A9D File Offset: 0x00007C9D
		// (set) Token: 0x060009D1 RID: 2513 RVA: 0x00009AAF File Offset: 0x00007CAF
		public VisibilityMaskFlags VisibilityMask
		{
			get
			{
				return EngineApplicationInterface.IMesh.GetVisibilityMask(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IMesh.SetVisibilityMask(base.Pointer, value);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x00009AC2 File Offset: 0x00007CC2
		public int EditDataFaceCornerCount
		{
			get
			{
				return EngineApplicationInterface.IMesh.GetEditDataFaceCornerCount(base.Pointer);
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00009AD4 File Offset: 0x00007CD4
		public void SetEditDataFaceCornerVertexColor(int index, uint color)
		{
			EngineApplicationInterface.IMesh.SetEditDataFaceCornerVertexColor(base.Pointer, index, color);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00009AE8 File Offset: 0x00007CE8
		public uint GetEditDataFaceCornerVertexColor(int index)
		{
			return EngineApplicationInterface.IMesh.GetEditDataFaceCornerVertexColor(base.Pointer, index);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00009AFB File Offset: 0x00007CFB
		public void PreloadForRendering()
		{
			EngineApplicationInterface.IMesh.PreloadForRendering(base.Pointer);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00009B0D File Offset: 0x00007D0D
		public void SetContourColor(Vec3 color, bool alwaysVisible, bool maskMesh)
		{
			EngineApplicationInterface.IMesh.SetContourColor(base.Pointer, color, alwaysVisible, maskMesh);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00009B22 File Offset: 0x00007D22
		public void DisableContour()
		{
			EngineApplicationInterface.IMesh.DisableContour(base.Pointer);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00009B34 File Offset: 0x00007D34
		public void SetExternalBoundingBox(BoundingBox bbox)
		{
			EngineApplicationInterface.IMesh.SetExternalBoundingBox(base.Pointer, ref bbox);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00009B48 File Offset: 0x00007D48
		public void AddEditDataUser()
		{
			EngineApplicationInterface.IMesh.AddEditDataUser(base.Pointer);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00009B5A File Offset: 0x00007D5A
		public void ReleaseEditDataUser()
		{
			EngineApplicationInterface.IMesh.ReleaseEditDataUser(base.Pointer);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00009B6C File Offset: 0x00007D6C
		public void SetEditDataPolicy(EditDataPolicy policy)
		{
			EngineApplicationInterface.IMesh.SetEditDataPolicy(base.Pointer, policy);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00009B7F File Offset: 0x00007D7F
		public UIntPtr LockEditDataWrite()
		{
			return EngineApplicationInterface.IMesh.LockEditDataWrite(base.Pointer);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00009B91 File Offset: 0x00007D91
		public void UnlockEditDataWrite(UIntPtr handle)
		{
			EngineApplicationInterface.IMesh.UnlockEditDataWrite(base.Pointer, handle);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00009BA4 File Offset: 0x00007DA4
		public void SetCustomClipPlane(Vec3 clipPlanePosition, Vec3 clipPlaneNormal, int planeIndex)
		{
			EngineApplicationInterface.IMesh.SetCustomClipPlane(base.Pointer, clipPlanePosition, clipPlaneNormal, planeIndex);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00009BB9 File Offset: 0x00007DB9
		public float GetClothLinearVelocityMultiplier()
		{
			return EngineApplicationInterface.IMesh.GetClothLinearVelocityMultiplier(base.Pointer);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00009BCB File Offset: 0x00007DCB
		public bool HasCloth()
		{
			return EngineApplicationInterface.IMesh.HasCloth(base.Pointer);
		}
	}
}
