using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000036 RID: 54
	[ApplicationInterfaceBase]
	internal interface ITwoDimensionView
	{
		// Token: 0x0600056E RID: 1390
		[EngineMethod("create_twodimension_view", false, null, false)]
		TwoDimensionView CreateTwoDimensionView(string viewName);

		// Token: 0x0600056F RID: 1391
		[EngineMethod("begin_frame", false, null, false)]
		void BeginFrame(UIntPtr pointer);

		// Token: 0x06000570 RID: 1392
		[EngineMethod("end_frame", false, null, false)]
		void EndFrame(UIntPtr pointer);

		// Token: 0x06000571 RID: 1393
		[EngineMethod("clear", false, null, false)]
		void Clear(UIntPtr pointer);

		// Token: 0x06000572 RID: 1394
		[EngineMethod("add_new_mesh", false, null, false)]
		void AddNewMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData);

		// Token: 0x06000573 RID: 1395
		[EngineMethod("add_new_quad_mesh", false, null, false)]
		void AddNewQuadMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData);

		// Token: 0x06000574 RID: 1396
		[EngineMethod("add_cached_text_mesh", false, null, false)]
		bool AddCachedTextMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData);

		// Token: 0x06000575 RID: 1397
		[EngineMethod("add_new_text_mesh", false, null, false)]
		void AddNewTextMesh(UIntPtr pointer, float[] vertices, float[] uvs, uint[] indices, int vertexCount, int indexCount, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData);

		// Token: 0x06000576 RID: 1398
		[EngineMethod("get_or_create_material", false, null, false)]
		UIntPtr GetOrCreateMaterial(UIntPtr pointer, UIntPtr mainTexture, UIntPtr overlayTexture);
	}
}
