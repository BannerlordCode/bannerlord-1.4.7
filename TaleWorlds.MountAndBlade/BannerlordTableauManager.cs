using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000198 RID: 408
	public static class BannerlordTableauManager
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x00051316 File Offset: 0x0004F516
		public static Scene[] TableauCharacterScenes
		{
			get
			{
				return BannerlordTableauManager._tableauCharacterScenes;
			}
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0005131D File Offset: 0x0004F51D
		public static void RequestCharacterTableauRender(int characterCodeId, string path, GameEntity poseEntity, Camera cameraObject, int tableauType)
		{
			MBAPI.IMBBannerlordTableauManager.RequestCharacterTableauRender(characterCodeId, path, poseEntity.Pointer, cameraObject.Pointer, tableauType);
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00051339 File Offset: 0x0004F539
		public static void ClearManager()
		{
			BannerlordTableauManager._tableauCharacterScenes = null;
			BannerlordTableauManager.RequestCallback = null;
			BannerlordTableauManager._isTableauRenderSystemInitialized = false;
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0005134D File Offset: 0x0004F54D
		public static void InitializeCharacterTableauRenderSystem()
		{
			if (!BannerlordTableauManager._isTableauRenderSystemInitialized)
			{
				MBAPI.IMBBannerlordTableauManager.InitializeCharacterTableauRenderSystem();
				BannerlordTableauManager._isTableauRenderSystemInitialized = true;
			}
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x00051366 File Offset: 0x0004F566
		public static int GetNumberOfPendingTableauRequests()
		{
			return MBAPI.IMBBannerlordTableauManager.GetNumberOfPendingTableauRequests();
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x00051372 File Offset: 0x0004F572
		[MBCallback(null, false)]
		internal static void RequestCharacterTableauSetup(int characterCodeId, Scene scene, GameEntity poseEntity)
		{
			BannerlordTableauManager.RequestCallback(characterCodeId, scene, poseEntity);
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x00051381 File Offset: 0x0004F581
		[MBCallback(null, false)]
		internal static void RegisterCharacterTableauScene(Scene scene, int type)
		{
			BannerlordTableauManager.TableauCharacterScenes[type] = scene;
		}

		// Token: 0x04000712 RID: 1810
		private static Scene[] _tableauCharacterScenes = new Scene[5];

		// Token: 0x04000713 RID: 1811
		private static bool _isTableauRenderSystemInitialized = false;

		// Token: 0x04000714 RID: 1812
		public static BannerlordTableauManager.RequestCharacterTableauSetupDelegate RequestCallback;

		// Token: 0x020004E8 RID: 1256
		// (Invoke) Token: 0x06003B2F RID: 15151
		public delegate void RequestCharacterTableauSetupDelegate(int characterCodeId, Scene scene, GameEntity poseEntity);
	}
}
