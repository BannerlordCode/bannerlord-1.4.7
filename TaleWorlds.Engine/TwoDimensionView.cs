using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000096 RID: 150
	[EngineClass("rglTwo_dimension_view")]
	public sealed class TwoDimensionView : View
	{
		// Token: 0x06000D23 RID: 3363 RVA: 0x0000EBDC File Offset: 0x0000CDDC
		internal TwoDimensionView(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0000EBE5 File Offset: 0x0000CDE5
		public static TwoDimensionView CreateTwoDimension(string viewName)
		{
			return EngineApplicationInterface.ITwoDimensionView.CreateTwoDimensionView(viewName);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0000EBF2 File Offset: 0x0000CDF2
		public void BeginFrame()
		{
			EngineApplicationInterface.ITwoDimensionView.BeginFrame(base.Pointer);
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0000EC04 File Offset: 0x0000CE04
		public void EndFrame()
		{
			EngineApplicationInterface.ITwoDimensionView.EndFrame(base.Pointer);
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0000EC16 File Offset: 0x0000CE16
		public void Clear()
		{
			EngineApplicationInterface.ITwoDimensionView.Clear(base.Pointer);
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0000EC28 File Offset: 0x0000CE28
		public void CreateMeshFromDescription(WeakMaterial material, TwoDimensionMeshDrawData meshDrawData)
		{
			EngineApplicationInterface.ITwoDimensionView.AddNewMesh(base.Pointer, material.Pointer, ref meshDrawData);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0000EC43 File Offset: 0x0000CE43
		public bool CreateTextMeshFromCache(Material material, TwoDimensionTextMeshDrawData meshDrawData)
		{
			return EngineApplicationInterface.ITwoDimensionView.AddCachedTextMesh(base.Pointer, material.Pointer, ref meshDrawData);
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0000EC60 File Offset: 0x0000CE60
		public void CreateTextMeshFromDescription(float[] vertices, float[] uvs, uint[] indices, int indexCount, Material material, TwoDimensionTextMeshDrawData meshDrawData)
		{
			EngineApplicationInterface.ITwoDimensionView.AddNewTextMesh(base.Pointer, vertices, uvs, indices, vertices.Length / 2, indexCount, material.Pointer, ref meshDrawData);
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0000EC90 File Offset: 0x0000CE90
		public WeakMaterial GetOrCreateMaterial(Texture mainTexture, Texture overlayTexture)
		{
			return new WeakMaterial(EngineApplicationInterface.ITwoDimensionView.GetOrCreateMaterial(base.Pointer, (mainTexture != null) ? mainTexture.Pointer : UIntPtr.Zero, (overlayTexture != null) ? overlayTexture.Pointer : UIntPtr.Zero));
		}
	}
}
