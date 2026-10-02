using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000093 RID: 147
	[EngineClass("rglThumbnail_creator_view")]
	public sealed class ThumbnailCreatorView : View
	{
		// Token: 0x06000D13 RID: 3347 RVA: 0x0000E96C File Offset: 0x0000CB6C
		internal ThumbnailCreatorView(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0000E975 File Offset: 0x0000CB75
		[EngineCallback(null, false)]
		internal static void OnThumbnailRenderComplete(string renderId, Texture renderTarget)
		{
			ThumbnailCreatorView.renderCallback(renderId, renderTarget);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0000E983 File Offset: 0x0000CB83
		public static ThumbnailCreatorView CreateThumbnailCreatorView()
		{
			return EngineApplicationInterface.IThumbnailCreatorView.CreateThumbnailCreatorView();
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0000E98F File Offset: 0x0000CB8F
		public void RegisterScene(Scene scene, bool usePostFx = true)
		{
			EngineApplicationInterface.IThumbnailCreatorView.RegisterScene(base.Pointer, scene.Pointer, usePostFx);
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0000E9A8 File Offset: 0x0000CBA8
		public void RegisterCachedEntity(Scene scene, GameEntity entity, string cacheId)
		{
			EngineApplicationInterface.IThumbnailCreatorView.RegisterCachedEntity(base.Pointer, scene.Pointer, entity.Pointer, cacheId);
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0000E9C7 File Offset: 0x0000CBC7
		public void UnregisterCachedEntity(string cacheId)
		{
			EngineApplicationInterface.IThumbnailCreatorView.UnregisterCachedEntity(base.Pointer, cacheId);
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0000E9DA File Offset: 0x0000CBDA
		public void RegisterRenderRequest(ref ThumbnailRenderRequest request)
		{
			EngineApplicationInterface.IThumbnailCreatorView.RegisterRenderRequest(base.Pointer, ref request);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0000E9ED File Offset: 0x0000CBED
		public void ClearRequests()
		{
			EngineApplicationInterface.IThumbnailCreatorView.ClearRequests(base.Pointer);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0000E9FF File Offset: 0x0000CBFF
		public void CancelRequest(string renderID)
		{
			EngineApplicationInterface.IThumbnailCreatorView.CancelRequest(base.Pointer, renderID);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0000EA12 File Offset: 0x0000CC12
		public int GetNumberOfPendingRequests()
		{
			return EngineApplicationInterface.IThumbnailCreatorView.GetNumberOfPendingRequests(base.Pointer);
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0000EA24 File Offset: 0x0000CC24
		public bool IsMemoryCleared()
		{
			return EngineApplicationInterface.IThumbnailCreatorView.IsMemoryCleared(base.Pointer);
		}

		// Token: 0x040001CC RID: 460
		public static ThumbnailCreatorView.OnThumbnailRenderCompleteDelegate renderCallback;

		// Token: 0x020000D2 RID: 210
		// (Invoke) Token: 0x06001014 RID: 4116
		public delegate void OnThumbnailRenderCompleteDelegate(string renderId, Texture renderTarget);
	}
}
