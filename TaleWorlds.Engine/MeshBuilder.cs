using System;
using System.Collections.Generic;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000069 RID: 105
	public class MeshBuilder
	{
		// Token: 0x060009E1 RID: 2529 RVA: 0x00009BDD File Offset: 0x00007DDD
		public MeshBuilder()
		{
			this.vertices = new List<Vec3>();
			this.faceCorners = new List<MeshBuilder.FaceCorner>();
			this.faces = new List<MeshBuilder.Face>();
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00009C08 File Offset: 0x00007E08
		public int AddFaceCorner(Vec3 position, Vec3 normal, Vec2 uvCoord, uint color)
		{
			this.vertices.Add(new Vec3(position, -1f));
			MeshBuilder.FaceCorner faceCorner;
			faceCorner.vertexIndex = this.vertices.Count - 1;
			faceCorner.color = color;
			faceCorner.uvCoord = uvCoord;
			faceCorner.normal = normal;
			this.faceCorners.Add(faceCorner);
			return this.faceCorners.Count - 1;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00009C74 File Offset: 0x00007E74
		public int AddFace(int patchNode0, int patchNode1, int patchNode2)
		{
			MeshBuilder.Face face;
			face.fc0 = patchNode0;
			face.fc1 = patchNode1;
			face.fc2 = patchNode2;
			this.faces.Add(face);
			return this.faces.Count - 1;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00009CB2 File Offset: 0x00007EB2
		public void Clear()
		{
			this.vertices.Clear();
			this.faceCorners.Clear();
			this.faces.Clear();
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00009CD8 File Offset: 0x00007ED8
		public new Mesh Finalize()
		{
			Vec3[] array = this.vertices.ToArray();
			MeshBuilder.FaceCorner[] array2 = this.faceCorners.ToArray();
			MeshBuilder.Face[] array3 = this.faces.ToArray();
			Mesh mesh = EngineApplicationInterface.IMeshBuilder.FinalizeMeshBuilder(this.vertices.Count, array, this.faceCorners.Count, array2, this.faces.Count, array3);
			this.vertices.Clear();
			this.faceCorners.Clear();
			this.faces.Clear();
			return mesh;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00009D58 File Offset: 0x00007F58
		public static Mesh CreateUnitMesh()
		{
			Mesh mesh = Mesh.CreateMeshWithMaterial(Material.GetDefaultMaterial());
			Vec3 vec = new Vec3(0f, -1f, 0f, -1f);
			Vec3 vec2 = new Vec3(1f, -1f, 0f, -1f);
			Vec3 vec3 = new Vec3(1f, 0f, 0f, -1f);
			Vec3 vec4 = new Vec3(0f, 0f, 0f, -1f);
			Vec3 vec5 = new Vec3(0f, 0f, 1f, -1f);
			Vec2 vec6 = new Vec2(0f, 0f);
			Vec2 vec7 = new Vec2(1f, 0f);
			Vec2 vec8 = new Vec2(1f, 1f);
			Vec2 vec9 = new Vec2(0f, 1f);
			UIntPtr uintPtr = mesh.LockEditDataWrite();
			int num = mesh.AddFaceCorner(vec, vec5, vec6, uint.MaxValue, uintPtr);
			int num2 = mesh.AddFaceCorner(vec2, vec5, vec7, uint.MaxValue, uintPtr);
			int num3 = mesh.AddFaceCorner(vec3, vec5, vec8, uint.MaxValue, uintPtr);
			int num4 = mesh.AddFaceCorner(vec4, vec5, vec9, uint.MaxValue, uintPtr);
			mesh.AddFace(num, num2, num3, uintPtr);
			mesh.AddFace(num3, num4, num, uintPtr);
			mesh.UpdateBoundingBox();
			mesh.UnlockEditDataWrite(uintPtr);
			return mesh;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00009EAE File Offset: 0x000080AE
		public static Mesh CreateTilingWindowMesh(string baseMeshName, Vec2 meshSizeMin, Vec2 meshSizeMax, Vec2 borderThickness, Vec2 bgBorderThickness)
		{
			return EngineApplicationInterface.IMeshBuilder.CreateTilingWindowMesh(baseMeshName, ref meshSizeMin, ref meshSizeMax, ref borderThickness, ref bgBorderThickness);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00009EC3 File Offset: 0x000080C3
		public static Mesh CreateTilingButtonMesh(string baseMeshName, Vec2 meshSizeMin, Vec2 meshSizeMax, Vec2 borderThickness)
		{
			return EngineApplicationInterface.IMeshBuilder.CreateTilingButtonMesh(baseMeshName, ref meshSizeMin, ref meshSizeMax, ref borderThickness);
		}

		// Token: 0x04000146 RID: 326
		private List<Vec3> vertices;

		// Token: 0x04000147 RID: 327
		private List<MeshBuilder.FaceCorner> faceCorners;

		// Token: 0x04000148 RID: 328
		private List<MeshBuilder.Face> faces;

		// Token: 0x020000C9 RID: 201
		[EngineStruct("rglMeshBuilder_face_corner", false, null)]
		public struct FaceCorner
		{
			// Token: 0x04000429 RID: 1065
			public int vertexIndex;

			// Token: 0x0400042A RID: 1066
			public Vec2 uvCoord;

			// Token: 0x0400042B RID: 1067
			public Vec3 normal;

			// Token: 0x0400042C RID: 1068
			public uint color;
		}

		// Token: 0x020000CA RID: 202
		[EngineStruct("rglMeshBuilder_face", false, null)]
		public struct Face
		{
			// Token: 0x0400042D RID: 1069
			public int fc0;

			// Token: 0x0400042E RID: 1070
			public int fc1;

			// Token: 0x0400042F RID: 1071
			public int fc2;
		}
	}
}
